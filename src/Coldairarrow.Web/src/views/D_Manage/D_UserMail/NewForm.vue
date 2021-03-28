<template>
  <a-card :bordered="false">
    <a-form :form="form">
      <a-form-item label="收件人" :labelCol="labelCol" :wrapperCol="wrapperCol">
        <a-select mode="tags" style="width: 100%" v-model="tags">
          <a-select-option v-for="d in useroption" :key="d.value">{{ d.text }}</a-select-option>
        </a-select>
      </a-form-item>
      <a-form-item label="主题" :labelCol="labelCol" :wrapperCol="wrapperCol">
        <a-input v-model="etitle" />
      </a-form-item>
      <a-form-item label="正文" :labelCol="labelCol" :wrapperCol="wrapperCol">
        <in-editor v-model="editorData"></in-editor>
      </a-form-item>
      <a-form-item label="附件" :labelCol="labelCol" :wrapperCol="wrapperCol">
        <c-upload-file v-model="appendix" :maxCount="1"></c-upload-file>
      </a-form-item>
      <a-form-item :wrapperCol="{ span: 24 }" style="text-align: center">
        <a-button type="primary" :loading="loading" @click="handleSubmit(true)">存草稿</a-button>
        <a-button
          style="margin-left: 8px"
          type="primary"
          :loading="loading"
          @click="handleSubmit(false)"
        >发送</a-button>
        <a-button style="margin-left: 8px" :loading="loading" @click="handleCancel">重置</a-button>
      </a-form-item>
    </a-form>
  </a-card>
</template>

<script>
import InEditor from '@/components/InEditor/InEditor'
import { mapGetters, mapActions } from 'vuex'
import CUploadFile from '@/components/CUploadFile/CUploadFile'

export default {
  components: {
    InEditor,
    CUploadFile
  },
  props: {
    // eslint-disable-next-line vue/require-default-prop
    parentObj: Object
  },
  data () {
    return {
      form: this.$form.createForm(this),
      labelCol: { xs: { span: 24 }, sm: { span: 2 } },
      wrapperCol: { xs: { span: 24 }, sm: { span: 21 } },
      loading: false,
      entity: {},
      title: '',
      disabled: false,
      tags: [],
      tagInputVisible: false,
      tagInputValue: '',
      useroption: [],
      etitle: '',
      editorData: '',
      appendix: ''
    }
  },
  mounted () {
    this.GetAllUser().then((res) => {
      this.useroption = res
    })
  },
  computed: {
    ...mapGetters(['userInfo'])
  },
  methods: {
    ...mapActions(['GetAllUser']),
    openForm (id, title) {
      // 参数赋值
      this.title = title || '编辑表单'
      this.loading = true
      // 组件初始化
      this.init()

      // 编辑赋值
      if (id) {
        this.editorData = ''
        this.$nextTick(() => {
          this.$http.post('/D_Manage/D_UserMail/GetTheData', { id: id }).then((resJson) => {
            this.entity = resJson.Data
            this.tags = resJson.Data.UserIds.slice(1, -1).split('^')
            this.etitle = this.entity.Title
            this.editorData = this.entity.Text
            this.appendix = this.entity.Appendix
            this.disabled = true
            this.loading = false
          })
        })
      } else {
        this.loading = false
      }
    },
    init () {
      this.entity = {}
      this.editorData = ' '
      this.etitle = ''
      this.appendix = ''
      this.tags = []
      this.disabled = false
    },
    handleSubmit (isDraft) {
      if (this.tags.length === 0) {
        this.$message.error('收件人为空')
        return
      }
      if (this.etitle === '') {
        this.$message.error('主题为空')
        return
      }
      if (this.editorData === '') {
        this.$message.error('正文为空')
        return
      }
      const userNames = []
      let error = false
      this.tags.forEach((item) => {
        const tempuser = this.useroption.find((d) => d.value === item)
        if (tempuser !== null && typeof tempuser !== 'undefined') {
          userNames.push(tempuser.text)
        } else {
          this.$message.error('存在未知的收件人:' + item)
          error = true
          return true
        }
      })
      if (!error) {
        this.entity.Text = this.editorData
        this.entity.Title = this.etitle
        this.entity.UserNames = '^' + userNames.join('^') + '^'
        this.entity.UserIds = '^' + this.tags.join('^') + '^'
        this.entity.Avatar = this.userInfo.Avatar
        this.entity.Appendix = this.appendix
        this.entity.Status = isDraft ? 0 : 1

        this.loading = true
        this.$http.post('/D_Manage/D_UserMail/SaveData', this.entity).then((resJson) => {
          this.loading = false

          if (resJson.Success) {
            this.$message.success('操作成功!')
            this.init()
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      }
    },
    handleCancel () {
      this.init()
    }
  }
}
</script>

<style scoped>
/* /deep/.ck-editor__editable {
  min-height: 400px;
} */
/* /deep/.ck-editor {
  width: 100%;
} */
</style>
