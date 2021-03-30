<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-row :gutter="5">
        <a-col :lg="14" :md="12" :sm="24">
          <a-radio-group button-style="solid" v-model="status" @change="getDataList">
            <a-radio-button value="processing">待审批</a-radio-button>
            <a-radio-button value="waiting">等待中</a-radio-button>
            <a-radio-button value="finish">审批过</a-radio-button>
            <a-radio-button value="created">创建的</a-radio-button>
            <a-radio-button value="all">全部</a-radio-button>
          </a-radio-group>
        </a-col>
        <a-col :lg="3" :md="5" :sm="24">
          <a-select
            style="width:100%"
            allowClear
            v-model="queryParam.condition"
          >
            <a-select-option key="Type">分类</a-select-option>
            <a-select-option key="DefFormName">标题</a-select-option>
            <a-select-option key="Text">摘要</a-select-option>
            <a-select-option key="ApplicantUser">申请人</a-select-option>
            <a-select-option key="UserNames">审批人</a-select-option>
          </a-select>
        </a-col>
        <a-col :lg="7" :md="7" :sm="24">
          <a-input-search
            allow-clear
            v-model="queryParam.keyword"
            placeholder="关键字"
            enter-button="Search"
            @search="
              () => {this.pagination.current = 1; this.getDataList()}"
          />
        </a-col>
      </a-row>
    </div>

    <a-table
      ref="table"
      :columns="columns"
      :rowKey="row => row.Id"
      :dataSource="data"
      :pagination="pagination"
      :loading="loading"
      @change="handleTableChange"
      :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
      :bordered="true"
      size="small"
    >
      <span slot="Status" slot-scope="text">
        <a-badge :status="text | statusTypeFilter" :text="text | statusFilter" />
      </span>
      <span slot="Grade" slot-scope="text">
        <a-badge :status="text | gradeTypeFilter" :text="text | gradeFilter" />
      </span>

      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleEdit(record.Id, record.DefFormName)">查看</a>
          <template v-if="hasPerm('OA_UserForm.Delete')">
            <a-divider type="vertical" />
            <a @click="handleDelete([record.Id])">删除</a>
          </template>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    </div></a-card>
</template>

<script>
import EditForm from './EditForm'
import { mapGetters } from 'vuex'

const columns = [
  { title: '分类', dataIndex: 'Type', width: '100', key: 'type' },
  { title: '标题', dataIndex: 'DefFormName', width: '100', key: 'defFormName' },
  { title: '摘要', dataIndex: 'Text', width: '100', key: 'title' },
  { title: '状态', dataIndex: 'Status', width: '100', scopedSlots: { customRender: 'Status' }, key: '1' },
  { title: '紧急程度', dataIndex: 'Grade', width: '100', scopedSlots: { customRender: 'Grade' }, key: '2' },
  { title: '申请人及部门', dataIndex: 'ApplicantUserAndDepartment', width: '100', key: '3' },
  { title: '当前节点', dataIndex: 'Current', width: '100', key: '4' },
  { title: '当前审批人', dataIndex: 'UserNamesAndRoles', width: '100', key: '5' },
  {
    title: '创建日期',
    dataIndex: 'CreateTime',
    defaultSortOrder: 'descend',
    sorter: (a, b) => a.CreateTime - b.CreateTime,
    width: '100',
    key: '6'
  },
  {
    title: '预计完成日期',
    dataIndex: 'ExpectedDateString',
    width: '100',
    key: '7'
  },
  {
    title: '操作',
    dataIndex: 'action',
    width: '100',
    scopedSlots: { customRender: 'action' },
    key: 'action'
  }
]

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

const gradeMap = {
  0: {
    status: 'default',
    text: '正常'
  },
  1: {
    status: 'processing',
    text: '紧急'
  },
  2: {
    status: 'warning',
    text: '特急'
  }
}

export default {
  components: {
    EditForm
  },
  mounted () {
    this.getDataList()
  },
  data () {
    return {
      data: [],
      pagination: {
        current: 1,
        pageSize: 10,
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      filters: {},
      sorter: { field: 'CreateTime', order: 'desc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: [],
      status: 'processing'
    }
  },
  computed: {
    ...mapGetters(['userInfo']),
    ...mapGetters(['hasPerm'])
  },
  filters: {
    statusFilter (type) {
      return statusMap[type].text
    },
    statusTypeFilter (type) {
      return statusMap[type].status
    },
    gradeFilter (type) {
      return gradeMap[type].text
    },
    gradeTypeFilter (type) {
      return gradeMap[type].status
    }
  },
  methods: {
    handleTableChange (pagination, filters, sorter) {
      this.pagination = { ...pagination }
      this.filters = { ...filters }
      this.sorter = { ...sorter }
      this.getDataList()
    },
    getDataList () {
      this.selectedRowKeys = []
      this.queryParam.userId = this.status === 'processing' ? this.userInfo.Id : ''
      this.queryParam.applicantUserId = this.status === 'waiting' ? this.userInfo.Id : ''
      this.queryParam.alreadyUserIds = this.status === 'finish' ? this.userInfo.Id : ''
      this.queryParam.creatorId = this.status === 'created' ? this.userInfo.Id : ''

      this.loading = true
      this.$http
        .post('/OA_Manage/OA_UserForm/GetDataList', {
          PageIndex: this.pagination.current,
          PageRows: this.pagination.pageSize,
          SortField: this.sorter.field || 'Id',
          SortType: this.sorter.order,
          Search: this.queryParam,
          ...this.filters
        })
        .then(resJson => {
          this.loading = false
          this.data = resJson.Data
          const pagination = { ...this.pagination }
          pagination.total = resJson.Total
          this.pagination = pagination
        })
    },
    onSelectChange (selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected () {
      return this.selectedRowKeys.length > 0
    },
    handleEdit (id, name) {
      this.$refs.editForm.openForm(id, name)
    },
    handleDelete (ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/OA_Manage/OA_UserForm/DeleteData', ids).then(resJson => {
              resolve()

              if (resJson.Success) {
                thisObj.$message.success('操作成功!')

                thisObj.getDataList()
              } else {
                thisObj.$message.error(resJson.Msg)
              }
            })
          })
        }
      })
    }
  }
}
</script>
