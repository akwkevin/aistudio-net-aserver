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
            <a-select-option key="Table">表名</a-select-option>
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
  { title: '表名', dataIndex: 'Table', width: '10%' },
  { title: '列头', dataIndex: 'Header', width: '10%' },
  { title: '属性名', dataIndex: 'PropertyName', width: '10%' },
  { title: '显示索引', dataIndex: 'DisplayIndex', width: '10%' },
  { title: '配置类型', dataIndex: 'Type', width: '10%' },
  { title: '格式化', dataIndex: 'StringFormat', width: '10%' },
  { title: '是否显示', dataIndex: 'Visibility', width: '10%' },
  { title: '控件类型', dataIndex: 'ControlType', width: '10%' },
  { title: '只读', dataIndex: 'IsReadOnly', width: '10%' },
  { title: '必输项', dataIndex: 'IsRequired', width: '10%' },
  { title: '字典名', dataIndex: 'ItemSource', width: '10%' },
  { title: '默认值', dataIndex: 'Value', width: '10%' },
  { title: '排序名', dataIndex: 'SortMemberPath', width: '10%' },
  { title: '转换器', dataIndex: 'Converter', width: '10%' },
  { title: '转换参数', dataIndex: 'ConverterParameter', width: '10%' },
  { title: '对齐方式', dataIndex: 'HorizontalAlignment', width: '10%' },
  { title: '最大宽度', dataIndex: 'MaxWidth', width: '10%' },
  { title: '最小宽度', dataIndex: 'MinWidth', width: '10%' },
  { title: '列表宽度', dataIndex: 'Width', width: '10%' },
  { title: '是否可以重排', dataIndex: 'CanUserReorder', width: '10%' },
  { title: '是否可以调整大小', dataIndex: 'CanUserResize', width: '10%' },
  { title: '是否可以排序', dataIndex: 'CanUserSort', width: '10%' },
  { title: '是否冻结', dataIndex: 'IsFrozen', width: '10%' },
  { title: '背景颜色触发公式', dataIndex: 'BackgroundExpression', width: '10%' },
  { title: '前景颜色触发公式', dataIndex: 'ForegroundExpression', width: '10%' },
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
        .post('/Base_Manage/Base_CommonFormConfig/GetDataList', {
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
            thisObj.$http.post('/Base_Manage/Base_CommonFormConfig/DeleteData', ids).then(resJson => {
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
