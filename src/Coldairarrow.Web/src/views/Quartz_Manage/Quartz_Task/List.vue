<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-row>
        <a-col :lg="15" :md="12" :sm="24">
          <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
          <a-button icon="pause" @click="handlePause(selectedRowKeys)" :disabled="!hasSelected()">暂停</a-button>
          <a-button
            icon="caret-right"
            @click="handleStart(selectedRowKeys)"
            :disabled="!hasSelected()"
          >开启</a-button>
          <a-button
            icon="step-forward"
            @click="handleTodo(selectedRowKeys)"
            :disabled="!hasSelected()"
          >立即执行</a-button>
          <a-button
            type="danger"
            icon="edit"
            @click="handleEdits(selectedRowKeys)"
            :disabled="!hasSelected()"
          >修改</a-button>
          <a-button
            type="danger"
            icon="minus"
            @click="handleDelete(selectedRowKeys)"
            :disabled="!hasSelected()"
            :loading="loading"
          >删除</a-button>
          <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
        </a-col>
        <a-col :lg="9" :md="12" :sm="24">
          <a-input-search
            allow-clear
            v-model="queryParam.keyword"
            placeholder="关键字"
            enter-button="Search"
            @search="
              () => {this.pagination.current = 1; this.getDataList()}"
          >
          </a-input-search></a-col>
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

      <span slot="Describe" slot-scope="text">
        <ellipsis :length="10" tooltip>{{ text }}</ellipsis>
      </span>

      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" />
          <a @click="handleDelete([record.Id])">删除</a>
          <a-divider type="vertical" />
          <a @click="handleLog(record.GroupName, record.TaskName)">记录</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <log-form ref="logForm" :parentObj="this"></log-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import LogForm from './LogForm'
import Ellipsis from '@/components/Ellipsis'
import { Alert } from 'ant-design-vue'

const columns = [
  { title: '作业名称', dataIndex: 'TaskName', width: '10%' },
  { title: '分组', dataIndex: 'GroupName', width: '10%' },
  { title: '最后执行时间', dataIndex: 'LastRunTime', width: '10%' },
  { title: '间隔(Cron)', dataIndex: 'Interval', width: '10%' },
  { title: '状态', dataIndex: 'Status', width: '10%', scopedSlots: { customRender: 'Status' } },
  { title: '描述', dataIndex: 'Describe', width: '10%', scopedSlots: { customRender: 'Describe' } },
  { title: 'ApiUrl', dataIndex: 'ApiUrl', width: '10%' },
  { title: '请求方式', dataIndex: 'RequestType', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

const statusMap = {
  0: {
    status: 'processing',
    text: '运行中'
  },
  1: {
    status: 'default',
    text: '关闭'
  },
  2: {
    status: 'success',
    text: '完成'
  },
  3: {
    status: 'error',
    text: '异常'
  },
  4: {
    status: 'error',
    text: '堵塞'
  },
  5: {
    status: 'default',
    text: '未知'
  }
}

export default {
  components: {
    EditForm,
    LogForm,
    Ellipsis
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
        showSizeChanger: true,
        pageSizeOptions: ['10', '20', '50', '100'],
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      filters: {},
      sorter: { field: 'Id', order: 'asc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: []
    }
  },
  filters: {
    statusFilter (type) {
      return statusMap[type].text
    },
    statusTypeFilter (type) {
      return statusMap[type].status
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

      this.loading = true
      this.$http
        .post('/Quartz_Manage/Quartz_Task/GetDataList', {
          PageIndex: this.pagination.current,
          PageRows: this.pagination.pageSize,
          SortField: this.sorter.field || 'Id',
          SortType: this.sorter.order,
          Search: this.queryParam,
          ...this.filters
        })
        .then((resJson) => {
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
    hanldleAdd () {
      this.$refs.editForm.openForm()
    },
    handleEdit (id) {
      this.$refs.editForm.openForm(id)
    },
    handleLog (groub, name) {
      this.$refs.logForm.openForm(groub + '.' + name)
    },
    handlePause (ids) {
      var thisObj = this
      this.$confirm({
        title: '确认暂停吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/Quartz_Manage/Quartz_Task/PauseData', ids).then((resJson) => {
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
    },
    handleStart (ids) {
      var thisObj = this
      this.$confirm({
        title: '确认开始吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/Quartz_Manage/Quartz_Task/StartData', ids).then((resJson) => {
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
    },
    handleTodo (ids) {
      var thisObj = this
      this.$confirm({
        title: '确认立即执行吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/Quartz_Manage/Quartz_Task/TodoData', ids).then((resJson) => {
              resolve()

              if (resJson.Success) {
                thisObj.$message.success('操作成功!')

                thisObj.getDataList()
              } else {
                thisObj.$message.error(resJson.Msg)
              }
            })
          })
        },
        onCancel () {
          Alert('取消')
        }
      })
    },
    handleEdits (ids) {
      if (ids.length > 0) this.$refs.editForm.openForm(ids[0])
    },
    handleDelete (ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            thisObj.$http
              .post('/Quartz_Manage/Quartz_Task/DeleteData', ids)
              .then((resJson) => {
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
