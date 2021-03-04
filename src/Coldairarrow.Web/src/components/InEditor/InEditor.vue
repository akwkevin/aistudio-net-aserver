<template>
  <ckeditor :editor="editor" @ready="onReady" :value="editorData" :config="editorConfig" @input="customHandler"></ckeditor>
</template>

<script>
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import CKEditor from '@ckeditor/ckeditor5-vue'
import MyUploadAdapter from './MyUploadAdapter'

export default {
  name: 'InEditor',
  props: {
    value: {
      type: String,
      default: ''
    },
    placeholder: {
      type: String,
      default: '请输入内容'
    }
  },
  data () {
    return {
      // 编辑器组件需要获取编辑器实例
      editor: ClassicEditor,
      editorData: '',
      editorConfig: {
        placeholder: this.placeholder
        // ckfinder: {
        //   uploadUrl: `${this.$rootUrl}/Base_Manage/Upload/UploadFileByForm` // 后端处理上传逻辑返回json数据,包括uploaded 上传的字节数 和url两个字段
        // }
      }
    }
  },
  watch: {
    value (val) {
      if (!this.editor) {
        return
      }

      // 外部内容发生变化时，将新值赋予编辑器
      if (val && val !== this.editorData) {
        this.editorData = this.value
      }
    }
  },
  methods: {
    onReady (editor) {
      // 自定义上传图片插件
      editor.plugins.get('FileRepository').createUploadAdapter = loader => {
        return new MyUploadAdapter(loader)
      }
    },
    customHandler: function (event) {
      if (event && event !== this.value) {
        // 编辑器内容发生变化时，告知外部，实现 v-model 双向监听效果
        this.$emit('input', event)
      }
    }
  },
  created () {
    // 编辑器组件创建时将外部传入的值直接赋予编辑器
    this.editorData = this.value
  },
  components: {
    // 编辑器组件的局部注册方式
    ckeditor: CKEditor.component
  }
}
</script>

<style scoped lang="stylus"></style>
