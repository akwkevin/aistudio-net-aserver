<template>
  <a-modal
    :title="title"
    width="80%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-spin :spinning="loading">
      <a-form :form="form" class="ant-advanced-search-form">
        <a-row :gutter="24">
          <a-col :span="8" :style="{ display: 1 < count ? 'block' : 'none' }">
            <a-form-item label="分类" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-select v-decorator="['Type', { rules: [{ required: true, message: '必填' }] }]">
                <a-select-option v-for="d in types" :value="d.Name" :key="d.Id">{{ d.Name }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :span="8" :style="{ display: 2 < count ? 'block' : 'none' }">
            <a-form-item label="标题" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input v-decorator="['Name', { rules: [{ required: true, message: '必填' }] }]" />
            </a-form-item>
          </a-col>
          <a-col :span="8" :style="{ display: 3 < count ? 'block' : 'none' }">
            <a-form-item label="摘要" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input v-decorator="['Text', { rules: [{ required: true, message: '必填' }] }]" />
            </a-form-item>
          </a-col>
          <a-col :span="8" :style="{ display: 4 < count ? 'block' : 'none' }">
            <a-form-item label="排序" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input v-decorator="['Sort', { rules: [{ required: false, message: '请输入排序' }] }]" />
            </a-form-item>
          </a-col>
          <a-col :span="8" :style="{ display: 5 < count ? 'block' : 'none' }">
            <a-form-item label="权限角色" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-select
                v-decorator="['ValueRoles', { rules: [{ required: false, message: '请选择有权限的角色' }] }]"
                show-search
                mode="multiple"
                option-filter-prop="children"
                :filter-option="filterOption"
                style="width: 100%"
              >
                <a-select-option v-for="d in roles" :key="d.value" >{{ d.text }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :span="8" :style="{ display: 6 < count ? 'block' : 'none' }">
            <a-form-item label="流程图" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-textarea
                :rows="1"
                v-decorator="['WorkflowJSON', { rules: [{ required: false }] }]"
                placeholder="json"
                allow-clear
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="24" :style="{ textAlign: 'right' }">
            <a :style="{ marginLeft: '8px', fontSize: '12px' }" @click="toggle">
              Collapse
              <a-icon :type="expand ? 'up' : 'down'" />
            </a>
          </a-col>
        </a-row>
        <a-row>
          <a-card
            :span="24"
            :bordered="false"
            :bodyStyle="{ padding: '0', height: '100%' }"
            :style="{ height: '100%' }"
          >
            <G6Editor
              v-if="visible"
              ref="g6editor"
              :mode="mode"
              :data="data"
              :users="users"
              :roles="roles"
              @ok="saveData"
            ></G6Editor>
          </a-card>
        </a-row>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import G6Editor from '@/components/G6Editor'
import { mapGetters, mapActions } from 'vuex'

export default {
  components: { G6Editor },
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
      expand: false,
      data: {},
      users: [],
      roles: [],
      mode: 'edit',
      types: []
    }
  },
  computed: {
    count () {
      return this.expand ? 11 : 4
    }
  },
  mounted () {
    this.GetAllUser().then(res => {
      this.users = res
    })
    this.GetAllRole().then(res => {
      this.roles = res
    })

    var queryParam = {
      condition: 'Type',
      keyword: '分类'
    }
    this.$http
      .post('/OA_Manage/OA_DefType/GetDataList', {
        Search: queryParam
      })
      .then(resJson => {
        this.types = resJson.Data
      })
  },
  methods: {
    ...mapActions(['GetAllUser']),
    ...mapActions(['GetAllRole']),
    openForm (id, title, mode) {
      // 参数赋值
      this.title = title || '编辑表单'
      this.loading = true
      this.mode = mode || 'edit'

      // 组件初始化
      this.init()

      // 编辑赋值
      if (id) {
        this.$nextTick(() => {
          this.formFields = this.form.getFieldsValue()

          this.$http.post('/OA_Manage/OA_DefForm/GetTheData', { id: id }).then(resJson => {
            this.entity = resJson.Data
            var setData = {}
            Object.keys(this.formFields).forEach(item => {
              setData[item] = this.entity[item]
            })
            this.form.setFieldsValue(setData)
            this.data = JSON.parse(this.entity.WorkflowJSON)
            this.loading = false
          })
        })
      } else {
        this.loading = false
      }
    },
    init () {
      this.entity = {}
      this.form.resetFields()
      this.visible = true
      this.data = {}
    },
    handleSubmit () {
      this.form.validateFields((errors, values) => {
        // 校验成功
        if (!errors) {
          this.entity = Object.assign(this.entity, this.form.getFieldsValue())

          if (this.mode === 'edit') {
            this.entity.id = ''
            var g6data = this.$refs.g6editor.getData()

            g6data.edges.forEach(p => {
              p.source = p.sourceId
              p.target = p.targetId
            })

            this.entity.WorkflowJSON = JSON.stringify(g6data)
          }

          this.loading = true
          this.$http.post('/OA_Manage/OA_DefForm/SaveData', this.entity).then(resJson => {
            this.loading = false

            if (resJson.Success) {
              this.$message.success('操作成功!')
              this.visible = false
              this.data = {}
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
      this.data = {}
      this.$refs.g6editor.reloadData()
    },
    toggle () {
      this.expand = !this.expand
    },
    saveData (data) {
      // this.$http.post('/OA_Manage/OA_UserForm/TestJsonData', { json: data }).then(resJson => {
      //   if (resJson.Success) {
      //     this.$message.success(resJson.Msg)
      //   } else {
      //     this.$message.error(resJson.Msg)
      //   }
      // })
      var setData = {}
      setData['WorkflowJSON'] = JSON.stringify(data)
      this.form.setFieldsValue(setData)
    },
    filterOption (input, option) {
      return option.componentOptions.children[0].text.toLowerCase().indexOf(input.toLowerCase()) >= 0
    }
  }
}
</script>

<style>
.ant-advanced-search-form .ant-form-item {
  display: flex;
}

.ant-advanced-search-form .ant-form-item-control-wrapper {
  flex: 1;
}
</style>
