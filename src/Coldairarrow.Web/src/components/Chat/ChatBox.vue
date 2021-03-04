<template>
  <div>

    <div>
      <div class="message" :style="{ height: height - 270 + 'px' }">
        <a-list :dataSource="messages" :split="false">
          <a-list-item slot="renderItem" slot-scope="item">
            <p class="time" v-if="item.ShowTime">
              <span v-text="item.CreateTime"></span>
            </p>
            <p class="time system" v-if="item.Type == 10000">
              <span v-html="item.Text"></span>
            </p>
            <div :class="'main' + (item.CreatorId === userInfo.Id ? ' self' : '')" v-else>
              <a-avatar
                class="avatar"
                size="large"
                shape="square"
                :title="item.CreatorName"
                :src="item.Avatar | AvatarFilter"
              />
              <!-- 文本 -->
              <!-- <div class="text" v-emotion="item.Text" v-if="item.type == 1"></div> -->
              <!-- <div class="text" v-if="item.Type == 1">{{ item.Text }}</div> -->

              <div class="text" ref="content" v-html="toEmotion(item.Text)" v-if="item.Type == 1"></div>

              <!-- 图片 -->
              <div class="text" v-else-if="item.Type == 2">
                <img :src="item.Text" class="image" controls="controls" alt="图片" @click="handleEvent(item)" />
              </div>

              <!-- 视频 -->
              <div class="text" v-else-if="item.Type == 3">
                <video :src="item.Text" class="image" controls="controls" alt="视频" @click="handleEvent(item)" />
              </div>

              <!-- 录音 -->
              <div class="text" v-else-if="item.Type == 4">
                <audio :src="item.Text" class="image" controls="controls" alt="语音" @click="handleEvent(item)" />
              </div>

              <!-- 文件 -->
              <div class="text" v-else-if="item.Type == 5">
                <c-upload-file v-model="item.Text" :maxCount="1" class="image" disabled></c-upload-file>
              </div>

              <!-- 红包 -->
              <div class="text" v-else-if="item.Type == 6">
                <img :src="item.Text" class="image" alt="红包" />
              </div>

              <!-- 其他 -->
              <div
                class="text"
                v-else-if="item.Type != 10000"
                v-text="'[暂未支持的消息类型:' + item.Type + ']\n\r' + item.Text"
              ></div>
            </div>
          </a-list-item>
        </a-list>
      </div>
      <div class="bottom-container">
        <chat-tool @emoji="bindEmoji" @tohistory="ChangeHistory" @file="bindFile" :history="history" />
        <enter-box v-if="!history" ref="enterbox" @submit="sendText" v-model="contentText" />
        <history-tool v-else @history="BindHistory" />
      </div>
    </div>

    <a-modal :visible="show" width="40%" :footer="null" @cancel="handleCancel">
      <img :src="imgSrc" v-if="imgSrc" style="width: 100%; object-fit: cover" />
      <video :src="videoSrc" v-if="videoSrc" style="width: 100%; object-fit: cover" controls="controls"></video>
      <audio :src="audioSrc" v-if="audioSrc" style="width: 100%; object-fit: cover" controls="controls"></audio>
    </a-modal>

  </div>
</template>

<script>
import emojiParser from 'wechat-emoji-parser'
import Ellipsis from '@/components/Ellipsis'
import { mapGetters } from 'vuex'
import EnterBox from './EnterBox'
import ChatTool from './ChatTool'
import HistoryTool from './HistoryTool'
import CUploadFile from '@/components/CUploadFile/CUploadFile'

export default {
  components: {
    Ellipsis,
    EnterBox,
    ChatTool,
    HistoryTool,
    CUploadFile
  },
  props: {
    height: {
      type: Number,
      default: document.documentElement.clientHeight * 0.8
    },
    messages: {
      type: Array,
      default: () => []
    },
    title: {
      type: String,
      default: ''
    },
    loading: {
      type: Boolean,
      default: false
    }
  },
  watch: {
    messages: {
      handler: 'scrollToBottom'
    }
  },
  data () {
    return {
      contentText: '',
      selectedUserId: null,
      show: false,
      imgSrc: '',
      videoSrc: '',
      audioSrc: '',
      history: false
    }
  },
  methods: {
    sendText (text) {
      if (text) {
        this.$emit('submit', text)
        this.contentText = ''
      }
    },
    // 聚焦输入框
    focusTxtContent: function () {
      this.$refs.enterbox.$refs.txtContent.focus()
    },
    // 滚动条滚动到底部
    scrollToBottom: function () {
      setTimeout(function () {
        var chatlist = document.getElementsByClassName('message')[0]
        chatlist.scrollTop = chatlist.scrollHeight
      }, 100)
    },
    // 处理排版
    toEmotion (html) {
      return emojiParser(html).replace(/(<img src)/g, '<img data-class="iconBox" src')
    },
    bindEmoji (emoji) {
      this.contentText += emoji
      this.focusTxtContent()
    },
    bindFile (src, type) {
      this.$emit('submit', src, type)
    },
    BindHistory (startValue, endValue) {
      this.$emit('history', startValue, endValue)
    },
    ChangeHistory (value) {
      this.history = value
    },
    // 处理事件
    handleEvent (params) {
      if (params.Type === 2) {
        this.imgSrc = params.Text
        this.show = true
      } else if (params.Type === 3) {
        this.videoSrc = params.Text
        this.show = true
      } else if (params.Type === 4) {
        this.audioSrc = params.Text
        this.show = true
      } else if (params.Type === 5) {
        window.open(params.Text)
      }
    },
    handleCancel () {
      this.imgSrc = undefined
      this.videoSrc = undefined
      this.audioSrc = undefined
      this.show = false
    }
  },
  computed: {
    ...mapGetters(['userInfo'])
  }
}
</script>

<style lang="less" scoped>
.ant-avatar-lg {
  width: 48px;
  height: 48px;
  line-height: 48px;
}

.content {
  display: inline-block;
  padding: 1.5vh;
  background: #fff;
}

//对话框中的样式
.message {
  padding: 10px 15px;
  overflow-x: hidden;
  background-color: #fff;
}
.message li {
  margin-bottom: 15px;
  left: 0;
  position: relative;
  display: block;
}
.message .time {
  margin: 10px 0;
  text-align: center;
}
.message .text {
  display: inline-block;
  position: relative;
  padding: 0 10px;
  max-width: calc(100% - 50px);
  min-height: 35px;
  line-height: 2.1;
  font-size: 15px;
  padding: 6px 10px;
  text-align: left;
  word-break: break-all;
  background-color: #fff;
  color: #000;
  border-radius: 4px;
  box-shadow: 0px 1px 7px -5px #000;
}
.message .avatar {
  float: left;
  margin: 0 10px 0 0;
  border-radius: 3px;
  background: #fff;
}
.message .time > span {
  display: inline-block;
  padding: 0 5px;
  font-size: 12px;
  color: #fff;
  border-radius: 2px;
  background-color: #dadada;
}
.message .system > span {
  padding: 4px 9px;
  text-align: left;
}
.message .text:before {
  content: ' ';
  position: absolute;
  top: 9px;
  right: 100%;
  border: 6px solid transparent;
  border-right-color: #fff;
}
.message .self {
  text-align: right;
}
.message .self .avatar {
  float: right;
  margin: 0 0 0 10px;
}
.message .self .text {
  background-color: #9eea6a;
}

.message .self .text:before {
  right: inherit;
  left: 100%;
  border-right-color: transparent;
  border-left-color: #9eea6a;
}
.message .image {
  max-width: 200px;
}
img.static-emotion-gif,
img.static-emotion {
  vertical-align: middle !important;
}
.an-move-left {
  left: 0;
  //animation: moveLeft 0.7s ease;
  //-webkit-animation: moveLeft 0.7s ease;
}
.an-move-right {
  left: 0;
  //animation: moveRight 0.7s ease;
  //-webkit-animation: moveRight 0.7s ease;
}
.bgnone {
  background: none;
}
.bottom-container {
  background: #f1efef;
}
</style>
