<template>
  <a-modal
    width="70%"
    wrapClassName="web"
    :visible="visible"
    :confirmLoading="loading"
    @cancel="handleCancel"
  >
    <template slot="title">{{ title }}</template>
    <template slot="footer">
      <a-button
        type="primary"
        style="float:left;"
        key="g6editor"
        :loading="loading"
        @click="handleOpenG6Editor()"
      >查看流程图</a-button>
      <a-button
        type="danger"
        v-if="(status === 1 && entity.CreatorId === userInfo.Id)"
        key="del"
        :loading="loading"
        @click="handleDelete()"
      >废弃</a-button>
      <a-button
        v-if="(status === 1 && entity.UserIds.indexOf(userInfo.Id) !== -1)"
        key="event"
        type="primary"
        :loading="loading"
        @click="handleEvent()"
      >审批</a-button>
      <a-button
        v-if="status === 0"
        key="prestep"
        type="primary"
        :loading="loading"
        @click="handlePreStep()"
      >预览流程</a-button>
      <a-button
        v-if="status === 0"
        key="submit"
        type="primary"
        :loading="loading"
        @click="handleSubmit()"
      >提交</a-button>
      <a-button key="cancel" :loading="loading" @click="handleCancel">关闭</a-button>
    </template>
    <a-spin :spinning="loading">
      <template v-if="steps.length">
        <a-steps progress-dot style="width:100%">
          <a-step :status="item.Status | statusTypeFilterStep" v-for="item in steps" :key="item.Id">
            <a slot="title">{{ item.Label }}</a>
            <a slot="description">{{ item.ActRules.RoleNames }} {{ item.ActRules.UserNames }}</a>
          </a-step>
        </a-steps>
        <a-divider />
      </template>
      <a-form :form="form">
        <a-row :gutter="16">
          <a-col :xl="12" :lg="12" :md="12" :sm="24">
            <a-form-item label="摘要">
              <a-input
                :disabled="currentStepIndex !== 0"
                v-decorator="['Text', { rules: [{ required: true, message: '必填' }] }]"
              />
            </a-form-item>
          </a-col>
          <a-col :xl="6" :lg="6" :md="12" :sm="24">
            <a-form-item label="申请人">
              <a-select
                :disabled="currentStepIndex !== 0"
                show-search
                option-filter-prop="children"
                :filter-option="filterOption"
                style="width: 100%"
                v-decorator="['ApplicantUserId', { rules: [{ required: true, message: '必填' }] }]"
              >
                <a-select-option v-for="d in useroption" :key="d.value" >{{ d.text }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col :xl="6" :lg="6" :md="12" :sm="24">
            <a-form-item label="紧急程度">
              <a-select
                :disabled="currentStepIndex !== 0"
                v-decorator="['Grade',{ initialValue:'0' }]"
              >
                <a-select-option key="0" value="0">正常</a-select-option>
                <a-select-option key="1" value="1">紧急</a-select-option>
                <a-select-option key="2" value="2">特急</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col>
            <a-form-item label="备注">
              <a-input
                :disabled="currentStepIndex !== 0"
                v-decorator="['Remarks', { rules: [{ required: false, }] }]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col v-if="types.length" :xl="6" :lg="6" :md="12" :sm="24">
            <a-form-item label="类型">
              <a-select
                :disabled="currentStepIndex !== 0"
                v-decorator="['SubType', { rules: [{ required: true, message: '必填' }] }]"
                @change="handleChange"
              >
                <a-select-option v-for="d in types" :value="d.Name" :key="d.Id">{{ d.Name }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
          <a-col v-if="types.length" :xl="6" :lg="6" :md="12" :sm="24">
            <a-form-item>
              <span slot="label">{{ entity.Unit || "数值" }}</span>
              <a-input
                :disabled="currentStepIndex !== 0"
                v-decorator="['Flag', { rules: [{ required: false, message: '条件比较字段' }] }]"
              />
            </a-form-item>
          </a-col>
          <a-col :xl="6" :lg="6" :md="12" :sm="24">
            <a-form-item label="期望完成日期">
              <a-date-picker
                :disabled="currentStepIndex !== 0"
                style="width:100%"
                v-decorator="['ExpectedDate', { rules: [{ required: false, }] }]"
              />
            </a-form-item>
          </a-col>
          <a-col :xl="6" :lg="6" :md="12" :sm="24">
            <a-form-item label="附件">
              <c-upload-file :disabled="currentStepIndex !== 0" v-model="appendix" :maxCount="1"></c-upload-file>
            </a-form-item>
          </a-col>
        </a-row>
        <a-form-item>
          <a-list
            v-if="comments.length"
            style="margin-top:0"
            :data-source="comments"
            item-layout="horizontal"
          >
            <a-list-item slot="renderItem" slot-scope="item, index">
              <a-comment
                :author="item.CreatorName"
                :avatar="item.Avatar|AvatarFilter"
                :content="item.Remarks"
              >
                <a slot="datetime">
                  {{ item.RoleNames }} {{ item.CreateTime }} {{ item.Status | statusFilter }}
                  <a-badge :status="item.Status | statusTypeFilter" />
                </a>
              </a-comment>
            </a-list-item>
          </a-list>
        </a-form-item>
      </a-form>
      <a-form v-if="status=== 1">
        <a-form-item>
          <a-radio-group v-model="approve">
            <a-radio :value="100">通过</a-radio>
            <a-radio :value="2">驳回上一级</a-radio>
            <a-radio :value="3">驳回重提</a-radio>
            <a-radio :value="4">否决</a-radio>
          </a-radio-group>
          <a-input placeholder="意见" :rows="4" v-model="remark" />
        </a-form-item>
      </a-form>
    </a-spin>
    <g6-editor-form ref="g6editorForm" :parentObj="this"></g6-editor-form>
  </a-modal>
</template>

<script>
import moment from 'moment'
import CUploadFile from '@/components/CUploadFile/CUploadFile'
import G6EditorForm from './G6EditorForm'
import { mapGetters, mapActions } from 'vuex'

const statusMap = {
  0: {
    status: 'default',
    text: '未开始',
    stepstatus: 'wait'
  },
  1: {
    status: 'processing',
    text: '审批中',
    stepstatus: 'process'
  },
  2: {
    status: 'error',
    text: '驳回上一级',
    stepstatus: 'error'
  },
  3: {
    status: 'error',
    text: '驳回重提',
    stepstatus: 'error'
  },
  4: {
    status: 'error',
    text: '否决',
    stepstatus: 'error'
  },
  5: {
    status: 'warning',
    text: '废弃',
    stepstatus: 'error'
  },
  6: {
    status: 'warning',
    text: '挂起',
    stepstatus: 'wait'
  },
  7: {
    status: 'processing',
    text: '恢复',
    stepstatus: 'process'
  },
  8: {
    status: 'error',
    text: '操作失败',
    stepstatus: 'error'
  },
  99: {
    status: 'processing',
    text: '部分审批',
    stepstatus: 'process'
  },
  100: {
    status: 'success',
    text: '通过',
    stepstatus: 'finish'
  }
}

export default {
  props: {
    parentObj: Object
  },
  components: {
    CUploadFile,
    G6EditorForm
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
      appendix: '',
      comments: [],
      steps: [],
      currentStepIndex: 0,
      remark: '',
      approve: 100,
      status: 0,
      useroption: [],
      types: []
    }
  },
  mounted () {
    this.GetAllUser().then(res => {
      this.useroption = res
    })
  },
  filters: {
    statusFilter (type) {
      return statusMap[type].text
    },
    statusTypeFilter (type) {
      return statusMap[type].status
    },
    statusTypeFilterStep (type) {
      return statusMap[type].stepstatus
    }
  },
  computed: {
    ...mapGetters(['userInfo'])
  },
  methods: {
    ...mapActions(['GetAllUser']),
    openForm (id, title, type, key, jsonId, jsonVersion, json) {
      // 参数赋值
      this.title = title || '编辑表单'
      this.loading = true

      // 组件初始化
      this.init()

      this.$nextTick(() => {
        var queryParam = {
          condition: 'Type',
          keyword: type
        }
        this.$http
          .post('/OA_Manage/OA_DefType/GetDataList', {
            Search: queryParam
          })
          .then(resJson => {
            this.types = resJson.Data

            // 编辑赋值
            if (id) {
              this.$nextTick(() => {
                this.formFields = this.form.getFieldsValue()

                this.$http.post('/OA_Manage/OA_UserForm/GetTheData', { id: id }).then(resJson => {
                  this.entity = resJson.Data
                  this.status = this.entity.Status
                  this.comments = resJson.Data.Comments
                  this.steps = resJson.Data.Steps
                  this.currentStepIndex = resJson.Data.CurrentStepIndex

                  var setData = {}
                  Object.keys(this.formFields).forEach(item => {
                    if (item === 'ExpectedDate') {
                      if (this.entity[item] !== null) {
                        setData[item] = moment(this.entity[item])
                      }
                    } else if (item === 'Grade') {
                      setData[item] = this.entity[item] + '' // 转字符串，否则无法匹配value回显
                    } else {
                      setData[item] = this.entity[item]
                    }
                  })
                  this.form.setFieldsValue(setData)
                  if (this.entity.Appendix !== null) {
                    this.appendix = this.entity.Appendix
                  }
                  if (!this.entity.Unit && this.types.length > 0) {
                    this.entity.Unit = this.types[0].Unit
                  }

                  this.loading = false
                })
              })
            } else {
              this.entity.Type = type
              this.entity.DefFormId = key
              this.entity.DefFormName = title
              this.entity.DefFormJsonId = jsonId
              this.entity.DefFormJsonVersion = jsonVersion
              this.entity.WorkflowJSON = json
              this.$nextTick(() => {
                var setData = { ApplicantUserId: this.userInfo.Id }
                this.form.setFieldsValue(setData)
              })
              this.loading = false
            }
          })
      })
    },
    init () {
      this.entity = {}
      this.form.resetFields()
      this.appendix = ''
      this.comments = []
      this.steps = []
      this.currentStepIndex = 0
      this.remark = ''
      this.approve = 100
      this.status = 0
      this.visible = true
    },
    handleSubmit () {
      this.form.validateFields((errors, values) => {
        // 校验成功
        if (!errors) {
          this.entity = Object.assign(this.entity, this.form.getFieldsValue())
          this.entity.Appendix = this.appendix
          this.entity.ApplicantUser = this.useroption.find(d => d.value === this.entity.ApplicantUserId).text

          this.loading = true
          this.$http.post('/OA_Manage/OA_UserForm/SaveData', this.entity).then(resJson => {
            this.loading = false

            if (resJson.Success) {
              this.$message.success('操作成功!')
              this.visible = false
            } else {
              this.$message.error(resJson.Msg)
            }
          })
        }
      })
    },
    handlePreStep () {
      this.form.validateFields((errors, values) => {
        // 校验成功
        if (!errors) {
          this.entity = Object.assign(this.entity, this.form.getFieldsValue())
          this.entity.Appendix = this.appendix
          this.entity.ApplicantUser = this.useroption.find(d => d.value === this.entity.ApplicantUserId).text

          this.loading = true
          this.$http.post('/OA_Manage/OA_UserForm/PreStep', this.entity).then(resJson => {
            this.loading = false

            if (resJson.Success) {
              this.$message.success('操作成功!')
              this.steps = resJson.Data
            } else {
              this.$message.error(resJson.Msg)
            }
          })
        }
      })
    },
    handleEvent () {
      if (this.remark === null || this.remark === '') {
        this.$message.error('请填写意见')
        return
      }
      this.loading = true
      const myEvent = {
        EventName: 'MyEvent',
        EventKey: this.entity.Id + this.entity.CurrentStepId,
        UserId: this.userInfo.Id,
        UserName: this.userInfo.UserName,
        Status: this.approve,
        Remarks: this.remark
      }
      this.$http
        .post('/OA_Manage/OA_UserForm/EventData', {
          ...myEvent
        })
        .then(resJson => {
          this.loading = false

          if (resJson.Success) {
            this.$message.success(resJson.Msg)
            this.visible = false
            setTimeout(() => {
              this.parentObj.getDataList()
            }, 3000)
          } else {
            this.$message.error(resJson.Msg)
          }
        })
    },
    handleDelete () {
      if (this.remark === null || this.remark === '') {
        this.$message.error('请填写意见')
        return
      }
      this.loading = true
      this.$http
        .post('/OA_Manage/OA_UserForm/DisCardData', {
          id: this.entity.Id,
          remark: this.remark
        })
        .then(resJson => {
          this.loading = false

          if (resJson.Success) {
            this.$message.success('废弃成功!')
            this.visible = false
            setTimeout(() => {
              this.parentObj.getDataList()
            }, 100)
          } else {
            this.$message.error(resJson.Msg)
          }
        })
    },
    handleCancel () {
      this.visible = false
    },
    filterOption (input, option) {
      return option.componentOptions.children[0].text.toLowerCase().indexOf(input.toLowerCase()) >= 0
    },
    handleOpenG6Editor () {
      this.$refs.g6editorForm.openForm(JSON.parse(this.entity.WorkflowJSON), '查看流程图', 'read')
    },
    handleChange (value) {
      var type = this.types.find(p => p.Name === value)
      if (typeof type !== 'undefined') {
        this.entity.Unit = type.Unit
      } else {
        this.entity.Unit = ''
      }
    }
  }
}
</script>
<style lang="less" scoped>
.ant-form-item {
  margin-bottom: 0px;
}
.web {
  .ant-modal-body {
    padding-top: 0px;
    padding-bottom: 0px;
  }
}
</style>
