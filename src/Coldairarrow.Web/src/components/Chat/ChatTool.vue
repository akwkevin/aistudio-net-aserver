<template>
  <div class="toolsBox">
    <div v-if="!history" class="web__tools">
      <dl v-if="showEmoji()">
        <a-popover placement="topLeft" v-model="visible" trigger="click" ref="popover">
          <template slot="content">
            <div class="emjioBox">
              <ul class="emjio">
                <li v-for="item in Object.keys(emoji)" :key="item" @click="selectEmit(item)">
                  <a v-if="emoji[item] && emoji[item].position" :style="emojiStyle(item)"></a>
                  <a v-else-if="emoji[item].length < 5">{{ emoji[item] }}</a>
                  <img v-else :src="require('@/images' + emoji[item])" />
                </li>
              </ul>
            </div>
          </template>
          <span>
            <a-icon style="{margin-left:28px;margin-top:13px;}" type="smile" title="表情" />
          </span>
        </a-popover>
      </dl>
      <template v-for="(item, k) in showkeys()">
        <span
          v-if="toolConfig[item]"
          :key="item"
          :style="item === 'history' && 'flex-grow: 100;text-align: right;'"
        >
          <a-icon
            :type="toolConfig[item].icon"
            :title="setTitle(item, k)"
            @click="bindButton(item)"
          />
        </span>
        <i :key="item" v-else :class="item" @click="bindButton(item)"></i>
      </template>
      <input
        type="file"
        ref="refImg"
        style="display:none"
        accept="image/jpg, image/png, image/jpeg, image/gif"
        @change="fileOnload"
      />
      <input
        type="file"
        ref="refVideo"
        style="display:none"
        accept="audio/mp4, video/mp4"
        @change="fileOnload"
      />
      <input
        type="file"
        ref="refAudio"
        style="display:none"
        accept="audio/mp4"
        @change="fileOnload"
      />
      <input type="file" ref="refFile" style="display:none" @change="fileOnload" />
    </div>
    <div v-else class="web__tools">
      <a-icon
        :type="toolConfig.history.icon"
        :title="toolConfig.history.title"
        @click="bindButton(toolConfig.history.icon)"
        :style="{ color: '#9eea6a' }"
        style="flex-grow: 100;text-align: right;margin-bottom:2px;"
      />
    </div>
  </div>
</template>

<script>
import emoji from './emoji'
import Vue from 'vue'
import { ACCESS_TOKEN } from '@/store/mutation-types'
const uuid = require('uuid')

export default {
  name: 'ChatTool',
  props: {
    tools: {
      type: Object,
      default: () => {
        return {
          show: ['img', 'video', 'file', 'history'],
          showEmoji: true,
          callback: () => {}
        }
      }
    },
    history: {
      type: Boolean,
      default: false
    }
  },
  data () {
    return {
      emoji,
      toolConfig: {
        img: { icon: 'picture', title: '图片' },
        video: { icon: 'video-camera', title: '视频' },
        audio: { icon: 'audio', title: '音频' },
        file: { icon: 'file', title: '文件' },
        hongbao: { icon: 'money-collect', title: '红包' },
        more: { icon: 'more', title: '更多' },
        history: { icon: 'history', title: '历史' }
      },
      newTitle: null,
      visible: false,
      type: 1
    }
  },
  methods: {
    showEmoji () {
      const { showEmoji = true } = this.tools || {}
      return showEmoji
    },
    showkeys () {
      let keys = Object.keys(this.toolConfig)
      const { show = [] } = this.tools || {}
      if (show.length > 0) {
        const _key = []
        let h = false
        show.forEach(i => {
          if (this.isArray(i)) {
            this.newTitle = i
            return
          }
          if (i === 'history') {
            h = true
          } else {
            _key.push(i)
          }
        })
        if (h) _key.push('history')
        keys = _key
      }
      return keys
    },
    setTitle (key, index) {
      let title = ''
      if (this.newTitle) {
        title = this.newTitle[index] || ''
      }
      if (!title) {
        title = this.toolConfig[key].title
      }
      return title
    },
    isArray (target) {
      return Object.prototype.toString.call(target) === '[object Array]'
    },
    selectEmit (item) {
      this.$emit('emoji', item)
      this.visible = false
    },
    bindButton (type) {
      // this.tools.callback && this.tools.callback(type)
      this.type = type
      if (this.type === 'img') {
        this.$refs.refImg.dispatchEvent(new MouseEvent('click'))
      } else if (this.type === 'video') {
        this.$refs.refVideo.dispatchEvent(new MouseEvent('click'))
      } else if (this.type === 'audio') {
        this.$refs.refAudio.dispatchEvent(new MouseEvent('click'))
      } else if (this.type === 'file') {
        this.$refs.refFile.dispatchEvent(new MouseEvent('click'))
      } else if (this.type === 'history') {
        this.$emit('tohistory', !this.history)
      }
    },
    emojiStyle (item) {
      const emojiitem = this.emoji[item]
      if (!emojiitem) return {}
      return {
        display: 'inline-block',
        background: `url('/emotion/6AfH8-r.png')  no-repeat`,
        width: `28px`,
        height: `28px`,
        'background-position': emojiitem.position
      }
    },
    getFileName (url) {
      const reg = /^.*\/(.*?)$/
      const match = reg.test(url)
      if (match) {
        return RegExp.$1
      } else {
        return ''
      }
    },
    fileOnload () {
      // const file = document.getElementById('files').files[0]
      var file = null
      if (this.type === 'img') {
        file = this.$refs.refImg.files[0]
      } else if (this.type === 'video') {
        file = this.$refs.refVideo.files[0]
      } else if (this.type === 'audio') {
        file = this.$refs.refAudio.files[0]
      } else if (this.type === 'file') {
        file = this.$refs.refFile.files[0]
      }
      var param = new FormData() // 创建form对象
      param.append('file', file) // 通过append向form对象添加数据
      var config = {
        headers: { 'Content-Type': 'multipart/form-data' }
      }
      if (Vue.ls.get(ACCESS_TOKEN)) {
        config.headers.Authorization = 'Bearer ' + Vue.ls.get(ACCESS_TOKEN)
      }
      var url = `${this.$rootUrl}/Base_Manage/Upload/UploadFileByForm`
      this.$http.post(url, param, config).then(res => {
        var file = { name: this.getFileName(res.url), uid: uuid.v4(), status: 'done', url: res.url }
        // 双向绑定
        // this.$emit('emoji', file.url)
        var type = 1
        if (this.type === 'img') {
          type = 2
        } else if (this.type === 'video') {
          type = 3
        } else if (this.type === 'audio') {
          type = 4
        } else if (this.type === 'file') {
          type = 5
        }
        this.$emit('file', file.url, type)
      })
    }
  }
}
</script>

<style scoped>
.toolsBox {
  position: relative;
}
.web__tools {
  text-align: left;
  padding-left: 8px;
  box-sizing: border-box;
  display: flex;
  align-items: center;
  height: 30px;
}
i {
  margin-right: 12px;
  font-size: 20px;
  color: #888a91;
}
i:hover {
  color: #76b1f9;
}
.emjioBox {
  background: #fff;
  height: 150px;
  width: 300px;
  overflow: auto;
  text-align: left;
}
.emjioBox .emjio {
  padding: 0;
}
.emjioBox li {
  display: inline-block;
  width: 28px;
  height: 28px;
  line-height: 28px;
  text-align: center;
  cursor: pointer;
}
</style>
