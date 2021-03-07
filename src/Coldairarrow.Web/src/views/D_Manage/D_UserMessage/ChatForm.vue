<template>
  <a-modal
    :title="title"
    :width="width"
    :visible="visible"
    :confirmLoading="loading"
    :footer="null"
    @cancel="handleCancel"
  >
    <chat-box
      :visible="visible"
      @submit="sendText"
      @history="getHisData"
      :messages="messages"
      :height="height"
      :loading="loading"
    ></chat-box>
  </a-modal>
</template>

<script>
import moment from 'moment'
import { mapGetters } from 'vuex'
import ChatBox from '@/components/Chat/ChatBox'
const uuid = require('uuid')

var messageType = 2
var readmessageType = 6
var uid = uuid.v4()

export default {
  components: {
    ChatBox
  },
  props: {
    height: {
      type: Number,
      default: document.documentElement.clientHeight * 1
    }
  },
  mounted () {
    this.socketApi.addcallback(this.getConfigResult, messageType)
  },
  data () {
    return {
      loading: false,
      messages: [],
      contentText: null,
      selectedUserId: null,
      selectedUserName: null,
      selectedUserAvatar: null,
      groupId: null,
      selectedUserIds: null,
      selectedUserNames: null,
      title: '',
      visible: false,
      width: '60%'
    }
  },
  methods: {
    openForm (id, name, avatar, groupid, groupname, userids, usernames) {
      // 参数赋值
      this.title = groupname || name
      this.loading = true
      // 组件初始化
      this.init()
      this.selectedUserId = id
      this.selectedUserName = name
      this.selectedUserAvatar = avatar
      this.groupId = groupid
      this.selectedUserIds = userids
      this.selectedUserNames = usernames
      // 编辑赋值
      this.getUnReadData()
    },
    init () {
      this.visible = true
      this.messages = []
      this.selectedUserId = null
      this.selectedUserName = null
      this.selectedUserAvatar = null
      this.groupId = null
      this.selectedUserIds = null
      this.selectedUserNames = null
    },
    sendText (text, type) {
      if (!text || !this.selectedUserId) {
        return
      }
      const sendMessage = {}
      sendMessage.Text = text.replace('\n', '')
      if (this.groupId) {
        sendMessage.GroupId = this.groupId
        sendMessage.GroupName = this.groupname
        sendMessage.UserNames = this.selectedUserNames
        sendMessage.UserIds = this.selectedUserIds
      } else {
        sendMessage.UserNames = '^' + this.selectedUserName + '^'
        sendMessage.UserIds = '^' + this.selectedUserId + '^'
      }
      sendMessage.Avatar = this.userInfo.Avatar
      if (type) {
        sendMessage.Type = type
      } else {
        sendMessage.Type = 1
      }
      const send = {
        Success: true,
        Data: sendMessage,
        MessageType: messageType
      }
      this.socketApi.sendSock(JSON.stringify(send))
    },
    getConfigResult (res) {
      // 接收回调函数返回数据的方法
      const resmsg = res
      if (!this.selectedUserId) {
        return
      }
      if (
        (!this.groupId &&
          !resmsg.GroupId &&
          (resmsg.CreatorId === this.userInfo.Id ||
            resmsg.CreatorId === this.selectedUserId)) ||
        (this.groupId && this.groupId === resmsg.GroupId)
      ) {
        let endmsg = null
        if (this.messages && this.messages.length > 0) {
          endmsg = this.messages[this.messages.length - 1]
        }
        if (endmsg !== null && typeof endmsg !== 'undefined') {
          const date1 = new Date(resmsg.CreateTime) // 开始时间
          const date2 = new Date(endmsg.CreateTime) // 结束时间
          const date3 = date1.getTime() - date2.getTime() // 时间差的毫秒数
          if (date3 > 60000) {
            resmsg.ShowTime = true
          } else {
            resmsg.ShowTime = false
          }
        } else {
          resmsg.ShowTime = true
        }
        this.messages = this.messages.concat(resmsg)

        if (resmsg.CreatorId != this.userInfo.Id) {
          if ((resmsg.ReadingMarks || '').indexOf('^' + this.userInfo.Id + '^') === -1) {
            resmsg.ReadingMarks = (resmsg.ReadingMarks || '^') + this.userInfo.Id + '^'
            // 设为已读
            const send = {
              Success: true,
              Data: resmsg,
              MessageType: readmessageType
            }
            this.socketApi.sendSock(JSON.stringify(send))
          }
        }
      }
    },
    getHisData (startValue, endValue) {
      if (!this.selectedUserId) {
        return
      }
      if (startValue && endValue) {
        this.loading = true
        var queryParam = {
          creatorId: this.groupId || this.selectedUserId,
          userId: this.selectedUserId,
          isGroup: !!this.groupId,
          start: moment(startValue).format('YYYY-MM-DD HH:mm:ss'),
          end: moment(endValue).format('YYYY-MM-DD HH:mm:ss')
        }

        this.$http
          .post('/D_Manage/D_UserMessage/GetHistoryDataList', {
            Search: queryParam
          })
          .then((resJson) => {
            this.messages = resJson.Data || []
            this.loading = false
          })
      }
    },
    getUnReadData () {
      if (this.selectedUserId) {
        this.loading = true
        var queryParam = {
          userId: this.userInfo.Id,
          creatorId: this.groupId || this.selectedUserId,
          markflag: true,
          isGroup: !!this.groupId
        }
        this.$http
          .post('/D_Manage/D_UserMessage/GetPageHistoryDataList', {
            Search: queryParam
          })
          .then((resJson) => {
            this.messages = resJson.Data || []
            this.loading = false
          })
      }
    },
    handleCancel () {
      this.selectedUserId = null
      this.visible = false
    }
  },
  computed: {
    ...mapGetters(['userInfo'])
  },
  watch: {
    userdatas: {
      handler: 'saveUserDatas',
      deep: true
    }
  }
}
</script>

<style lang="less" scoped>
.ant-avatar-lg {
  width: 48px;
  height: 48px;
  line-height: 48px;
}

.gray {
  -webkit-filter: grayscale(100%);
  -moz-filter: grayscale(100%);
  -ms-filter: grayscale(100%);
  -o-filter: grayscale(100%);
  filter: grayscale(100%);
  filter: gray;
  opacity: 0.7; //通过改变透明度来调节灰色的程度
}

.content {
  display: inline-block;
  padding: 1.5vh;
  background: #fff;
}

.selected {
  background: #9eea6a;
  // color: white;
}
</style>
