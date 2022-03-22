<template>
  <a-card :bordered="false">
    <div class="table-operator">
     <a-row :gutter="5">
        <a-col :lg="14" :md="12" :sm="24">
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
        <a-col :lg="3" :md="5" :sm="24">
          <a-select style="width: 100%" allowClear v-model="queryParam.condition">
                <a-select-option key="ParentId">父级Id</a-select-option>
                <a-select-option key="Name">权限名/菜单名</a-select-option>
                <a-select-option key="Url">菜单地址</a-select-option>
                <a-select-option key="Value">权限值</a-select-option>
                <a-select-option key="Icon">图标</a-select-option>
                <a-select-option key="ModifyId">修改人Id</a-select-option>
                <a-select-option key="ModifyName">修改人</a-select-option>
                <a-select-option key="TenantId">租户Id</a-select-option>
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
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleEdit(record.Id)">编辑</a>
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

const columns = [
  { title: '父级Id', dataIndex: 'ParentId', width: '10%' },
  { title: '类型,菜单=0,页面=1,权限=2', dataIndex: 'Type', width: '10%' },
  { title: '权限名/菜单名', dataIndex: 'Name', width: '10%' },
  { title: '菜单地址', dataIndex: 'Url', width: '10%' },
  { title: '权限值', dataIndex: 'Value', width: '10%' },
  { title: '是否需要权限(仅页面有效)', dataIndex: 'NeedTest', width: '10%' },
  { title: '图标', dataIndex: 'Icon', width: '10%' },
  { title: '排序', dataIndex: 'Sort', width: '10%' },
  { title: '修改时间', dataIndex: 'ModifyTime', width: '10%' },
  { title: '修改人Id', dataIndex: 'ModifyId', width: '10%' },
  { title: '修改人', dataIndex: 'ModifyName', width: '10%' },
  { title: '租户Id', dataIndex: 'TenantId', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

export default {
  components: {
    EditForm
  },
  mounted() {
    this.getDataList()
  },
  data() {
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
      selectedRowKeys: []
    }
  },
  methods: {
    handleTableChange(pagination, filters, sorter) {
      this.pagination = { ...pagination }
      this.filters = { ...filters }
      this.sorter = { ...sorter }
      this.getDataList()
    },
    getDataList() {
      this.selectedRowKeys = []

      this.loading = true
      this.$http
        .post('/Base_Manage/Base_Test/GetDataList', {
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
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys
    },
    hasSelected() {
      return this.selectedRowKeys.length > 0
    },
    hanldleAdd() {
      this.$refs.editForm.openForm()
    },
    handleEdit(id) {
      this.$refs.editForm.openForm(id)
    },
    handleDelete(ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk() {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/Base_Manage/Base_Test/DeleteData', ids).then(resJson => {
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