<template>
  <a-card :bordered="false">
    <div slot="title">
      <a-button
        v-if="false"
        style="margin-left: 8px"
        type="primary"
        icon="plus"
        @click="hanldleAdd()"
      >新建</a-button>
      <a-button
        v-if="false"
        type="primary"
        icon="minus"
        @click="handleDelete(selectedRowKeys)"
        :disabled="!hasSelected()"
        :loading="loading"
      >删除</a-button>
      <a-button
        v-if="false"
        style="margin-left: 8px"
        type="primary"
        icon="redo"
        @click="getDataList()"
      >刷新</a-button>

      <div slot="extra" style="float:right;">
        <a-select style="margin-left: 8px;width: 100px;" allowClear v-model="queryParam.condition">
          <a-select-option :disabled="filterreceive" key="CreatorName">发件人</a-select-option>
          <a-select-option :disabled="filtersend" key="UserNames">收件人</a-select-option>
          <a-select-option key="Text">内容</a-select-option>
          <a-select-option key="Title">标题</a-select-option>
        </a-select>
        <a-input-search
          style="margin-left: 8px; width: 272px;"
          v-model="queryParam.keyword"
          placeholder="关键字"
          enter-button="Search"
          @search="getDataList"
        />
        <a-button style="margin-left: 8px" @click="() => (queryParam = {})">重置</a-button>
      </div>
    </div>

    <a-list size="small" :pagination="pagination">
      <a-list-item :key="index" v-for="(item, index) in data">
        <a-list-item-meta>
          <a-avatar
            slot="avatar"
            class="avatar"
            size="small"
            shape="square"
            :src="item.avatar | AvatarFilter"
          />
          <a slot="title">{{ item.Title }}</a>
          <!-- <a slot="description">
            <ellipsis :length="30" tooltip>{{ item.Text }}</ellipsis>
          </a>-->
        </a-list-item-meta>
        <div slot="actions">
          <a @click="handleEdit(item.Id)">编辑</a>
        </div>
        <div slot="actions">
          <a-dropdown>
            <a-menu slot="overlay">
              <a-menu-item>
                <a @click="handleDelete([item.Id])">删除</a>
              </a-menu-item>
            </a-menu>
            <a>
              更多
              <a-icon type="down" />
            </a>
          </a-dropdown>
        </div>
        <div class="list-content">
          <div v-if="type === '1'" class="list-content-item">
            <p
              :style="{
                color: item.IsReaded || (item.ReadingMarks || '').indexOf(userInfo.Id) >= 0 ? 'gray' : 'black',
                'font-weight': item.IsReaded || (item.ReadingMarks || '').indexOf(userInfo.Id) >= 0 ? 'normal' : 'bold'
              }"
            >{{ item.CreatorName }}</p>
          </div>
          <div v-if="type !== '1'" class="list-content-item">
            <p>{{ item.UserNames | filterReplaceName }}</p>
          </div>
          <div class="list-content-item">
            <p>{{ item.CreateTime | moment }}</p>
          </div>
        </div>
      </a-list-item>
    </a-list>

    <edit-form ref="editForm" :parentObj="this"></edit-form>
  </a-card>
</template>

<script>
import EditForm from './EditForm'
import Ellipsis from '@/components/Ellipsis'
import { mapGetters, mapActions } from 'vuex'

export default {
  components: {
    EditForm,
    Ellipsis
  },
  props: {
    type: {
      default: '1'
    }
  },
  mounted () {
    if (this.type === '1') {
      this.filtersend = true
    } else if (this.type === '2' || this.type === '3') {
      this.filterreceive = true
    }
    this.getDataList()
  },
  data () {
    return {
      data: [],
      pagination: {
        current: 1,
        pageSize: 10,
        showSizeChanger: true,
        showQuickJumper: true,
        showTotal: (total, range) => `总数:${total} 当前:${range[0]}-${range[1]}`
      },
      filters: {},
      sorter: { field: 'CreateTime', order: 'desc' },
      loading: false,
      queryParam: {},
      selectedRowKeys: [],
      filterreceive: false,
      filtersend: false
    }
  },
  filters: {
    filterReplaceName (item) {
      if (item === null) {
        return ''
      }
      return item.replace(/\^/g, '.').slice(1, -1)
    }
  },
  computed: {
    ...mapGetters(['userInfo'])
  },
  methods: {
    getDataList () {
      this.selectedRowKeys = []
      this.queryParam.userId = this.type === '1' ? this.userInfo.Id : ''
      this.queryParam.creatorId = this.type === '2' || this.type === '3' ? this.userInfo.Id : ''
      this.queryParam.status = this.type === '3' ? 0 : 1

      this.loading = true
      this.$http
        .post('/D_Manage/D_UserMail/GetDataList', {
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
    handleDelete (ids) {
      var thisObj = this
      this.$confirm({
        title: '确认删除吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/D_Manage/D_UserMail/DeleteData', ids).then((resJson) => {
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
    setDataRead (id) {
      var temp = this.data.find((p) => p.Id === id)
      if (temp !== null && typeof temp !== 'undefined') {
        temp.IsReaded = true
      }
    }
  }
}
</script>

<style lang="less" scoped>
.ant-avatar-lg {
  width: 48px;
  height: 48px;
  line-height: 48px;
}

.list-content-item {
  color: rgba(0, 0, 0, 0.45);
  display: inline-block;
  vertical-align: middle;
  font-size: 14px;
  margin-left: 40px;
  span {
    line-height: 20px;
  }
  p {
    margin-top: 4px;
    margin-bottom: 0;
    line-height: 22px;
  }
}

/deep/ .ant-card-head-title {
  padding: 0 16px 0 16px;
}
</style>
