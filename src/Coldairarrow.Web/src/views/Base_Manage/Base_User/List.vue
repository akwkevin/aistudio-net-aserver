<template>
  <a-card :bordered="false">
    <div class="table-operator">
      <a-row :gutter="48">
        <a-col :lg="15" :md="12" :sm="24">
          <a-button v-if="hasPerm('Base_User.Add')" type="primary" icon="plus" @click="hanldleAdd()">新建</a-button>
          <a-button
            v-if="hasPerm('Base_User.Delete')"
            type="primary"
            icon="minus"
            @click="handleDelete(selectedRowKeys)"
            :disabled="!hasSelected()"
            :loading="loading"
          >删除</a-button>
        </a-col>
        <a-col :lg="9" :md="12" :sm="24">
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
          <template v-if="hasPerm('Base_User.Edit')">
            <a @click="handleEdit(record.Id)">编辑</a>
            <a-divider type="vertical" />
          </template>
          <a v-if="hasPerm('Base_User.Delete')" @click="handleDelete([record.Id])">删除</a>
        </template>
      </span>
    </a-table>

    <edit-form ref="editForm" :afterSubmit="getDataList"></edit-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import { mapGetters } from 'vuex'

const columns = [
  { title: '用户名', dataIndex: 'UserName', width: '10%' },
  { title: '姓名', dataIndex: 'RealName', width: '10%' },
  { title: '性别', dataIndex: 'SexText', width: '5%' },
  { title: '出生日期', dataIndex: 'BirthdayText', width: '10%' },
  { title: '所属部门', dataIndex: 'DepartmentName', width: '10%' },
  { title: '所属角色', dataIndex: 'RoleNames', width: '30%' },
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
      visible: false,
      selectedRowKeys: []
    }
  },
  computed: {
    ...mapGetters(['hasPerm'])
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
        .post('/Base_Manage/Base_User/GetDataList', {
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
            thisObj.submitDelete(ids, resolve, reject)
          }).catch(() => console.log('Oops errors!'))
        }
      })
    },
    submitDelete (ids, resolve, reject) {
      this.$http.post('/Base_Manage/Base_User/DeleteData', ids).then(resJson => {
        resolve()

        if (resJson.Success) {
          this.$message.success('操作成功!')

          this.getDataList()
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    }
  }
}
</script>
