<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-row :gutter="5">
        <a-col :lg="11" :md="12" :sm="24">
          <a-button type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
          <a-button
            type="primary"
            icon="minus"
            @click="handleDelete(selectedRowKeys)"
            :disabled="!hasSelected()"
            :loading="loading"
          >删除</a-button
          >
          <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
        </a-col>
        <a-col :lg="5" :md="5" :sm="24">
          <a-select style="width:100%" allowClear v-model="queryParam.condition">
            <a-select-option key="Type">分类</a-select-option>
            <a-select-option key="Name">标题</a-select-option>
            <a-select-option key="Text">摘要</a-select-option>
            <a-select-option key="CreatorName">创建者</a-select-option>
            <a-select-option key="Status">状态</a-select-option>
          </a-select>
        </a-col>
        <a-col :lg="8" :md="7" :sm="24">
          <a-input-search
            allow-clear
            v-model="queryParam.keyword"
            placeholder="关键字"
            enter-button="Search"
            @search="getDataList"
          />

        </a-col>
      </a-row>
    </div>

    <a-table
      ref="table"
      :columns="columns"
      :rowKey="(row) => row.Id"
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
      <span slot="ValueRoles" slot-scope="text">
        <a-select
          v-if="text"
          disabled
          size="small"
          mode="tags"
          :value="text"
          style="width: 100%">
          <a-select-option v-for="d in roles" :key="d.value" >{{ d.text }}</a-select-option>
        </a-select>
      </span>
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleEdit(record.Id)">编辑</a>
          <a-divider type="vertical" />
          <a @click="handleCopy(record.Id)">复制</a>
          <a-divider type="vertical" />
          <a @click="handleStart(record.Id)">启用</a>
          <a-divider type="vertical" />
          <a @click="handleStop(record.Id)">停用</a>
          <a-divider type="vertical" />
          <a @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import { mapActions } from 'vuex'
const columns = [
  { title: '分类', dataIndex: 'Type', width: '10%' },
  { title: '标题', dataIndex: 'Name', width: '10%' },
  { title: '摘要', dataIndex: 'Text', width: '20%' },
  { title: '创建者', dataIndex: 'CreatorName', width: '10%' },
  { title: '状态', dataIndex: 'Status', width: '10%', scopedSlots: { customRender: 'Status' } },
  { title: '权限角色', dataIndex: 'ValueRoles', width: '15%', scopedSlots: { customRender: 'ValueRoles' } },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

const statusMap = {
  0: {
    status: 'default',
    text: '0-未启用'
  },
  1: {
    status: 'processing',
    text: '1-正常'
  }
}

export default {
  components: {
    EditForm
  },
  mounted () {
    this.getDataList()
    this.GetAllRole().then((res) => {
      this.roles = res
    })
  },
  filters: {
    statusFilter (type) {
      return statusMap[type].text
    },
    statusTypeFilter (type) {
      return statusMap[type].status
    }
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
      sorter: { field: 'Id', order: 'asc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: [],
      roles: []
    }
  },
  methods: {
    ...mapActions(['GetAllRole']),
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
        .post('/OA_Manage/OA_DefForm/GetDataList', {
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
      this.$refs.editForm.openForm(id, '编辑表单', 'read')
    },
    handleCopy (id) {
      this.$refs.editForm.openForm(id, '复制表单', 'edit')
    },
    handleStart (id) {
      var thisObj = this
      this.$confirm({
        title: '确认启用吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/OA_Manage/OA_DefForm/StartData', { id: id }).then((resJson) => {
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
    handleStop (id) {
      var thisObj = this
      this.$confirm({
        title: '确认停用吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/OA_Manage/OA_DefForm/StopData', { id: id }).then((resJson) => {
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
    handleDelete (ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            // let jsonids = thisObj.data.filter(item => ids.indexOf(item.Id) !== -1).map(p => p.JSONId)
            thisObj.$http.post('/OA_Manage/OA_DefForm/DeleteData', ids).then((resJson) => {
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
