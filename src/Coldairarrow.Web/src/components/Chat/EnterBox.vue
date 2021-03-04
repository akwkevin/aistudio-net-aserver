<template>
  <div class="web__msg" @keyup.enter="handleSend">
    <textarea ref="txtContent" v-model="currentMsg" rows="3" :placeholder="placeholder" class="web__msg-input"></textarea>
    <div class="web__msg-menu">
      <a-button class="web__msg-submit" type="primary" size="small" @click="handleSend">发送</a-button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Enterbox',
  props: {
    placeholder: {
      type: String,
      default: '请输入内容...'
    },
    value: {
      default: ''
    }
  },
  data () {
    return {
      currentMsg: this.msg
    }
  },
  watch: {
    value: {
      handler (newval) {
        if (newval && newval !== this.currentMsg) {
          this.currentMsg = newval
        }
      },
      immediate: true
    },
    currentMsg: {
      handler (newval) {
        if (newval && newval !== this.msg) {
          const msg = newval.trim()
          this.$emit('input', msg)
        }
      },
      immediate: true
    }
  },
  methods: {
    // 用户主动发送
    handleSend () {
      this.$emit('submit', this.currentMsg)
      this.$nextTick(() => {
        this.currentMsg = ''
      })
    }
  }
}
</script>
<style scoped>
.web__msg {
  padding: 0 10px;
  height: auto;
  overflow: hidden;
}
.web__msg--img,
.web__msg--video,
.web__msg--file {
  position: relative;
  max-width: 250px;
  min-width: 200px;
  width: 100%;
  margin: 10px 0;
  border: 1px solid #eee;
  overflow: hidden;
  border-radius: 5px;
  cursor: pointer;
  display: block;
}

.web__msg .web__msg {
  height: 150px;
  background-color: #fff;
}
.web__msg .web__msg span {
  box-sizing: border-box;
  padding: 3px 5px;
  color: #333;
  display: flex;
  align-items: center;
  width: 100%;
  justify-content: center;
  height: calc(100% - 80px);
  overflow: hidden;
  font-size: 13px;
  text-align: center;
}

.web__msg .web__msg h2 {
  margin: 0;
  width: 100%;
  text-align: center;
  line-height: 80px;
  background-color: #409eff;
  color: #fff;
}

.web__msg--map {
  height: 160px;
}

.web__msg-input {
  display: block;
  width: 100%;
  height: 60px;
  overflow-x: hidden;
  overflow-y: auto;
  box-sizing: border-box;
  resize: none;
  outline: 0;
  background-color: #fff;
  border: 0;
  word-break: break-all;
  font-size: 13px;
  line-height: 17px;
  -webkit-appearance: none;
}
.web__msg-menu {
  text-align: right;
}
.web__msg-submit {
  display: inline-block;
  outline: none;
  cursor: pointer;
  text-align: center;
}
</style>
