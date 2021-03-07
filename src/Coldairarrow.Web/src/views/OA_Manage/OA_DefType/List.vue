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
          <a-select style="width: 100%" allowClear v-model="queryParam.condition">
            <a-select-option key="Type">类型</a-select-option>
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
      :pagination="false"
      :loading="loading"
      @change="handleTableChange"
      :rowSelection="{ selectedRowKeys: selectedRowKeys, onChange: onSelectChange }"
      :bordered="true"
      size="small"
    >
      <span slot="action" slot-scope="text, record">
        <template>
          <a @click="handleEdit(record.RealId)">编辑</a>
          <a-divider type="vertical" />
          <a @click="handleDelete([record.RealId])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'

const columns = [
  { title: '类型', dataIndex: 'Type', width: '10%' },
  { title: '字段名称', dataIndex: 'Name', width: '10%' },
  { title: '单位', dataIndex: 'Unit', width: '10%' },
  { title: '操作', dataIndex: 'action', scopedSlots: { customRender: 'action' } }
]

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
      sorter: { field: 'Id', order: 'asc' },
      loading: false,
      columns,
      queryParam: {},
      selectedRowKeys: []
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
        .post('/OA_Manage/OA_DefType/GetTreeDataList', {
          Search: this.queryParam,
          ...this.filters
        })
        .then((resJson) => {
          this.loading = false
          this.data = resJson.Data
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
    handleDelete (ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/OA_Manage/OA_DefType/DeleteData', ids).then((resJson) => {
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
