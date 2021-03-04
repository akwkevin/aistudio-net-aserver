<template>
  <a-modal
    :title="title"
    width="60%"
    :visible="visible"
    :confirmLoading="loading"
    @cancel="handleCancel"
  >
    <template slot="footer">
      <a-pagination
        style="float:left;"
        size="small"
        :pageSize="pagination.pageSize"
        :total="pagination.total"
        :showTotal="pagination.showTotal"
        :pageSizeOptions="pagination.pageSizeOptions"
        v-model="pagination.current"
        showSizeChanger
        showQuickJumper
        @change="onChange"
        @showSizeChange="onShowSizeChange"
      />
      <a-button key="submit" type="primary" :loading="loading" @click="handleCancel">关闭</a-button>
    </template>

    <div class="log-model-content">
      <a-timeline>
        <a-timeline-item v-for="(item,index) in this.data" :key="index">
          <span>{{ getNum(index) }}.{{ item.CreateTime }} {{ item.LogContent }}</span>
        </a-timeline-item>
      </a-timeline>
    </div>
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
      visible: false,
      loading: false,
      data: [],
      title: '',
      pagination: {
        current: 1,
        pageSize: 100,
        pageSizeOptions: ['10', '20', '50', '100', '200', '500', '1000'],
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      filters: {},
      sorter: { field: 'Id', order: 'asc' },
      queryParam: {}
    }
  },
  watch: {
    pageSize (val) {
      console.log('pageSize', val)
    },
    current (val) {
      console.log('current', val)
    }
  },
  methods: {
    openForm (fullname) {
      // 参数赋值
      this.title = '执行记录'
      this.loading = true

      // 组件初始化
      this.init(fullname)

      // 编辑赋值
      this.$nextTick(() => {
        this.getData()
      })
    },
    init (fullname) {
      this.data = []
      this.visible = true
      this.queryParam.data = fullname
      this.pagination.current = 1
      this.pagination.pageSize = 100
    },
    getData () {
      this.loading = true
      this.$http
        .post('/Base_Manage/Base_UserLog/GetLogList', {
          PageIndex: this.pagination.current,
          PageRows: this.pagination.pageSize,
          SortField: 'CreateTime',
          SortType: 'desc',
          ...this.queryParam,
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
    handleCancel () {
      this.visible = false
    },
    getNum (index) {
      return (this.pagination.current - 1) * this.pagination.pageSize + index + 1
    },
    onChange (pageNumber) {
      this.pagination.current = pageNumber
      this.getData()
    },
    onShowSizeChange (current, pageSize) {
      this.pagination.pageSize = pageSize
      this.getData()
    }
  }
}
</script>

<style scoped>
.log-model-content {
  height: 480px;
  padding: 10px 20px;
}
</style>
