<template>
  <a-modal
    :title="title"
    :width="width"
    :visible="visible"
    :confirmLoading="loading"
    @cancel="handleCancel"
  >
    <template slot="footer">
      <a-button
        key="draft"
        type="primary"
        :loading="loading"
        :disabled="disabled"
        @click="handleSubmit(true)"
      >存草稿</a-button>
      <a-button
        key="submit"
        type="primary"
        :loading="loading"
        :disabled="disabled"
        @click="handleSubmit(false)"
      >发送</a-button>
      <a-button key="cancel" :loading="loading" @click="handleCancel">关闭</a-button>
    </template>
    <a-spin :spinning="loading">
      <a-form :form="form">
        <a-form-item label="类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-radio-group v-model="mode">
            <a-radio :value="0">
              全部
            </a-radio>
            <a-radio :value="1">
              按用户
            </a-radio>
            <a-radio :value="2">
              按角色
            </a-radio>
            <a-radio :value="3">
              按部门
            </a-radio>
          </a-radio-group>
        </a-form-item>
        <a-form-item label="收件人" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select mode="tags" style="width: 100%" v-model="tags">
            <a-select-option v-for="d in useroption" :key="d.value" >{{ d.text }}</a-select-option>
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
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import InEditor from '@/components/InEditor/InEditor'
import { mapGetters, mapActions } from 'vuex'
import CUploadFile from '@/components/CUploadFile/CUploadFile'

const OPTIONS = ['Apples', 'Nails', 'Bananas', 'Helicopters']

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
      visible: false,
      loading: false,
      entity: {},
      title: '',
      width: '60%',
      disabled: false,
      mode: 0,
      tags: [],
      tagInputVisible: false,
      tagInputValue: '',
      useroption: [],
      etitle: '',
      editorData: '',
      appendix: '',
      selectedItems: []
    }
  },
  mounted () {
    this.GetAllUser().then((res) => {
      this.useroption = res
    })
  },
  computed: {
    ...mapGetters(['userInfo']),
    filteredOptions () {
      return OPTIONS.filter(o => !this.selectedItems.includes(o))
    }
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
          this.$http.post('/D_Manage/D_Notice/GetTheData', { id: id }).then((resJson) => {
            this.entity = resJson.Data
            if (this.entity.AnyId && this.entity.mode === 1) {
              this.tags = this.entity.AnyId.slice(1, -1).split('^')
            }
            this.mode = this.entity.Mode
            this.etitle = this.entity.Title
            this.editorData = this.entity.Text
            this.appendix = this.entity.Appendix
            this.disabled = this.entity.Status !== 0
            this.loading = false
          })
        })
      } else {
        this.loading = false
      }
    },
    init () {
      this.entity = {}
      this.visible = true
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
        this.entity.Mode = this.mode
        this.entity.Text = this.editorData
        this.entity.Title = this.etitle
        this.entity.UserNames = '^' + userNames.join('^') + '^'
        this.entity.UserIds = '^' + this.tags.join('^') + '^'
        this.entity.Avatar = this.userInfo.Avatar
        this.entity.Appendix = this.appendix
        this.entity.Status = isDraft ? 0 : 1

        this.loading = true
        this.$http.post('/D_Manage/D_Notice/SaveData', this.entity).then((resJson) => {
          this.loading = false

          if (resJson.Success) {
            this.$message.success('操作成功!')
            this.visible = false
            this.editorData = ' '
            this.parentObj.getDataList()
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      }
    },
    handleCancel () {
      this.visible = false
      this.editorData = ' '
      this.appendix = ''
      if (this.entity.Id > 0 && (this.entity.ReadingMarks || '').indexOf(this.userInfo.Id) >= 0) {
        this.parentObj.setDataRead(this.entity.Id)
      }
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
