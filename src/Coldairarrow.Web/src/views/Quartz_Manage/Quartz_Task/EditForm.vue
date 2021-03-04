<template>
  <a-modal
    :title="title"
    width="40%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-spin :spinning="loading">
      <a-form :form="form">
        <a-form-item label="作业名称" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input
            v-decorator="['TaskName', { rules: [{ required: true, message: '必填' }] }]"
            :disabled="!this.canedit"
          />
        </a-form-item>
        <a-form-item label="分组" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input
            v-decorator="['GroupName', { rules: [{ required: true, message: '必填' }] }]"
            :disabled="!this.canedit"
          />
        </a-form-item>
        <a-form-item label="间隔(Cron)" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input v-decorator="['Interval', { rules: [{ required: true, message: '必填' }] }]" />
        </a-form-item>
        <a-form-item label="ApiUrl" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input v-decorator="['ApiUrl', { rules: [{ required: true, message: '必填' }] }]" />
        </a-form-item>
        <a-form-item label="Header(key)" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input v-decorator="['AuthKey', { rules: [{ required: false, message: '' }] }]" />
        </a-form-item>
        <a-form-item label="Header(value)" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input v-decorator="['AuthValue', { rules: [{ required: false, message: '' }] }]" />
        </a-form-item>
        <a-form-item label="请求方式" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input v-decorator="['RequestType', { rules: [{ required: true, message: '必填' }] }]" />
        </a-form-item>
        <a-form-item label="描述" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input v-decorator="['Describe', { rules: [{ required: false, message: '' }] }]" />
        </a-form-item>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
export default {
  props: {
    parentObj: Object
  },
  data () {
    return {
      form: this.$form.createForm(this),
      labelCol: { xs: { span: 24 }, sm: { span: 7 } },
      wrapperCol: { xs: { span: 24 }, sm: { span: 13 } },
      visible: false,
      loading: false,
      formFields: {},
      entity: {},
      title: '',
      canedit: true
    }
  },
  methods: {
    openForm (id, title) {
      // 参数赋值
      this.title = title || '编辑表单'
      this.loading = true

      // 组件初始化
      this.init()

      // 编辑赋值
      if (id) {
        this.canedit = false
        this.$nextTick(() => {
          this.formFields = this.form.getFieldsValue()

          this.$http.post('/Quartz_Manage/Quartz_Task/GetTheData', { id: id }).then(resJson => {
            this.entity = resJson.Data
            var setData = {}
            Object.keys(this.formFields).forEach(item => {
              setData[item] = this.entity[item]
            })
            this.form.setFieldsValue(setData)
            this.loading = false
          })
        })
      } else {
        this.canedit = true
        this.loading = false
      }
    },
    init () {
      this.entity = {}
      this.form.resetFields()
      this.visible = true
    },
    handleSubmit () {
      this.form.validateFields((errors, values) => {
        // 校验成功
        if (!errors) {
          this.entity = Object.assign(this.entity, this.form.getFieldsValue())

          this.loading = true
          this.$http.post('/Quartz_Manage/Quartz_Task/SaveData', this.entity).then(resJson => {
            this.loading = false

            if (resJson.Success) {
              this.$message.success('操作成功!')
              this.visible = false

              this.parentObj.getDataList()
            } else {
              this.$message.error(resJson.Msg)
            }
          })
        }
      })
    },
    handleCancel () {
      this.visible = false
    }
  }
}
</script>
