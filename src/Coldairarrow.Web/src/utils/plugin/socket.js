import defaultSettings from '@/config/defaultSettings'
import ProcessHelper from '@/utils/helper/ProcessHelper'
import moment from 'moment'

const rootUrl = () => {
  if (ProcessHelper.isProduction() || ProcessHelper.isPreview()) {
    return defaultSettings.publishRootUrl
  } else {
    return defaultSettings.localRootUrl
  }
}

var websock = null
var lockReconnect = false // 避免重复连接
var globalCallback = []
var userName = ''
var userId = ''

// 初始化weosocket
function initWebSocket (username, userid) {
  if (websock == null) {
    userName = username
    userId = userid
    // ws地址 -->这里是你的请求路径
    var ws =
      rootUrl().replace('http', 'ws') +
      '/ws?userName=' +
      userName +
      '&userId=' +
      userId // 'ws://localhost:9000/ws'  http://localhost:5000

    websock = new WebSocket(ws)
    websock.onmessage = function (e) {
      websocketonmessage(e)
      // 如果获取到消息，心跳检测重置
      // 拿到任何消息都说明当前连接是正常的
      heartCheck.reset().start()
    }
    websock.onclose = function (e) {
      websocketclose(e)
      reconnect()
    }
    websock.onopen = function () {
      websocketOpen()
      heartCheck.reset().start() // 传递信息
    }

    // 连接发生错误的回调方法
    websock.onerror = function () {
      console.log('WebSocket连接发生错误')
      reconnect()
    }
  }
}

function reconnect () {
  if (lockReconnect) return
  lockReconnect = true
  // 没连接上会一直重连，设置延迟避免请求过多
  setTimeout(function () {
    console.log('重新连接')
    websock = null
    initWebSocket(userName, userId)

    lockReconnect = false
  }, 2000)
}
// 心跳检测
var heartCheck = {
  timeout: 60000, // 60秒
  timeoutObj: null,
  serverTimeoutObj: null,
  reset: function () {
    clearTimeout(this.timeoutObj)
    clearTimeout(this.serverTimeoutObj)
    return this
  },
  start: function () {
    var self = this
    this.timeoutObj = setTimeout(function () {
      // 这里发送一个心跳，后端收到后，返回一个心跳消息，
      // onmessage拿到返回的心跳就说明连接正常

      const send = {
        Success: true,
        Data: moment(new Date()).format('YYYY-MM-DD HH:mm:ss'),
        MessageType: 7
      }
      websock.send(JSON.stringify(send))

      self.serverTimeoutObj = setTimeout(function () {
        // 如果超过一定时间还没重置，说明后端主动断开了
        websock.close() // 如果onclose会执行reconnect，我们执行ws.close()就行了.如果直接执行reconnect 会触发onclose导致重连两次
      }, self.timeout)
    }, this.timeout)
  }
}

function addcallback (callback, messageType) {
  const mycallback = { callback, messageType }
  globalCallback = globalCallback.concat(mycallback)
}

function removecallback (callback) {
  const index = globalCallback.findIndex(d => d.callback === callback)
  if (index >= 0) {
    globalCallback.splice(index, 1)
  }
}

function sendSock (agentData) {
  if (websock.readyState === websock.OPEN) {
    // 若是ws开启状态
    websocketsend(agentData)
  } else if (websock.readyState === websock.CONNECTING) {
    // 若是 正在开启状态，则等待1s后重新调用
    setTimeout(function () {
      sendSock(agentData)
    }, 1000)
  } else {
    // 若未开启 ，则等待1s后重新调用
    setTimeout(function () {
      sendSock(agentData)
    }, 1000)
  }
}

// 数据接收
function websocketonmessage (e) {
  const resmsg = JSON.parse(e.data)
  if (resmsg.Success) {
    globalCallback.forEach(item => {
      if (item.messageType === resmsg.MessageType) {
        item.callback(resmsg.Data)
      }
    })
  } else {
    console.log(resmsg.Msg)
  }
}

// 数据发送
function websocketsend (agentData) {
  websock.send(agentData)
}

// 关闭
function websocketclose (e) {
  console.log('连接关闭')
}

// 创建 websocket 连接
function websocketOpen (e) {
  console.log('连接成功')
}

function closeWebSocket () {
  if (websock) {
    websock.close(1000)
    websock = null
    globalCallback = []
  }
}

// initWebSocket();

// 将方法暴露出去
export { initWebSocket, closeWebSocket, sendSock, addcallback, removecallback }
