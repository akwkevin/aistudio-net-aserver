<template>
  <a-popover
    v-model="visible"
    trigger="click"
    placement="bottomRight"
    overlayClassName="header-notice-wrapper"
    :getPopupContainer="() => $refs.noticeRef.parentElement"
    :autoAdjustOverflow="true"
    :arrowPointAtCenter="true"
    :overlayStyle="{ width: '300px', top: '50px' }"
  >
    <template slot="content">
      <a-spin :spinning="loadding">
        <a-tabs v-model="activeKey" @tabClick="ontabClick" :tabBarGutter="0">
          <a-tab-pane key="1">
            <span slot="tab">
              <a-icon type="mail" />
              <a-badge :count="pagination1.total" :overflow-count="99" :offset="[6, -3]">通知</a-badge>
            </span>
            <a-list>
              <a-list-item
                :key="index"
                v-for="(item, index) in data1"
                @click="selectData1(item.Id)"
              >
                <a-list-item-meta :title="item.Title" :description="item.CreateTime">
                  <a-tooltip slot="avatar">
                    <template slot="title">
                      {{ item.CreatorName }}
                    </template>
                    <a-avatar
                      style="background-color: white;"
                      :src="item.Avatar | AvatarFilter"
                    />
                  </a-tooltip>
                </a-list-item-meta>
              </a-list-item>
              <div slot="footer">
                <router-link
                  style="float: right;"
                  @click.native="closeNotice"
                  :to="{ path: '/D_Manage/D_UserMail/Index' }"
                >查看更多</router-link>
              </div>
            </a-list>
          </a-tab-pane>
          <a-tab-pane key="2">
            <span slot="tab">
              <a-icon type="message" />
              <a-badge :count="pagination2.total" :overflow-count="99" :offset="[6, -3]">消息</a-badge>
            </span>
            <a-list>
              <a-list-item
                :key="index"
                v-for="(item, index) in data2"
                @click="selectData2(item.CreatorId, item.CreatorName, item.Avatar, item.GroupId,item.GroupName, item.UserIds, item.UserNames)"
              >
                <a-list-item-meta :title="item.Text" :description="item.CreateTime">
                  <a-tooltip slot="avatar">
                    <template slot="title">
                      {{ item.CreatorName }}
                    </template>
                    <a-avatar
                      style="background-color: white;"
                      :src="item.Avatar | AvatarFilter"
                    />
                  </a-tooltip>
                </a-list-item-meta>
              </a-list-item>
              <div slot="footer">
                <router-link
                  style="float: right;"
                  @click.native="closeNotice"
                  :to="{ path: '/D_Manage/D_UserMessage/List' }"
                >查看更多</router-link>
              </div>
            </a-list>
          </a-tab-pane>
          <a-tab-pane key="3">
            <span slot="tab">
              <a-icon type="clock-circle" />
              <a-badge :count="pagination3.total" :overflow-count="99" :offset="[6, -3]">待办</a-badge>
            </span>
            <a-list>
              <a-list-item
                :key="index"
                v-for="(item, index) in data3"
                @click="selectData3(item.Id, item.DefFormName)"
              >
                <a-list-item-meta :title="item.Text" :description="item.CreateTime">
                  <a-tooltip slot="avatar">
                    <template slot="title">
                      {{ item.CreatorName }}
                    </template>
                    <a-avatar
                      style="background-color: white;"
                      :src="item.Avatar | AvatarFilter"
                    />
                  </a-tooltip>
                </a-list-item-meta>
              </a-list-item>
              <div slot="footer">
                <router-link
                  style="float: right;"
                  @click.native="closeNotice"
                  :to="{ path: '/OA_Manage/OA_UserForm/List' }"
                >查看更多</router-link>
              </div>
            </a-list>
          </a-tab-pane>
        </a-tabs>
      </a-spin>
    </template>
    <span @click="fetchNotice" class="header-notice" ref="noticeRef">
      <a-badge :count="totalcount">
        <a-icon style="font-size: 16px; padding: 4px;" type="bell" />
      </a-badge>
      <edit-form1 ref="editForm1" :parentObj="this"></edit-form1>
      <edit-form2 ref="editForm2" :parentObj="this"></edit-form2>
      <edit-form3 ref="editForm3" :parentObj="this"></edit-form3>
    </span>
  </a-popover>
</template>

<script>
import { mapActions, mapGetters, mapMutations } from 'vuex'
import { formatDate } from '@/utils/util'
import EditForm1 from '@/views/D_Manage/D_UserMail/EditForm'
import EditForm2 from '@/views/D_Manage/D_UserMessage/ChatForm'
import EditForm3 from '@/views/OA_Manage/OA_UserForm/EditForm'

var messageType = 5
export default {
  name: 'HeaderNotice',
  components: {
    EditForm1,
    EditForm2,
    EditForm3
  },
  data () {
    return {
      loadding: false,
      visible: false,
      activeKey: '1',
      data1: [],
      data2: [],
      data3: [],
      pagination1: {
        current: 1,
        pageSize: 5,
        total: 0
      },
      pagination2: {
        current: 1,
        pageSize: 5,
        total: 0
      },
      pagination3: {
        current: 1,
        pageSize: 5,
        total: 0
      },
      totalcount: 0,
      sorter: { field: 'CreateTime', order: 'desc' }
    }
  },
  mounted () {
    this.socketApi.addcallback(this.getConfigResult, messageType)
  },

  computed: {
    ...mapGetters(['userInfo'])
  },
  methods: {
    ...mapActions(['ClearAllUser']),
    ...mapActions(['ClearAllRole']),
    getDataList () {
      this.loadding = true
      // setTimeout(() => {
      //   this.loadding = false
      // }, 1000)
      if (this.activeKey === '1') {
        var queryParam1 = {
          userId: this.userInfo.Id,
          markflag: true
        }

        this.$http
          .post('/D_Manage/D_UserMail/GetPageHistoryDataList', {
            PageIndex: this.pagination1.current,
            PageRows: this.pagination1.pageSize,
            SortField: this.sorter.field || 'Id',
            SortType: this.sorter.order,
            Search: queryParam1
          })
          .then(resJson => {
            this.loadding = false
            this.data1 = resJson.Data
            const pagination = { ...this.pagination1 }
            pagination.total = resJson.Total || 0
            this.pagination1 = pagination
            this.totalcount = this.pagination1.total + this.pagination2.total + this.pagination3.total
          })
      } else if (this.activeKey === '2') {
        var queryParam2 = {
          userId: this.userInfo.Id,
          markflag: true
        }
        this.$http
          .post('/D_Manage/D_UserMessage/GetPageHistoryGroupDataList', {
            Search: queryParam2
          })
          .then(resJson => {
            this.loadding = false
            this.data2 = resJson.Data
            const pagination = { ...this.pagination2 }
            pagination.total = resJson.Total || 0
            this.pagination2 = pagination
            this.totalcount = this.pagination1.total + this.pagination2.total + this.pagination3.total
          })
      } else if (this.activeKey === '3') {
        var queryParam3 = {
          userId: this.userInfo.Id
        }
        this.$http
          .post('/OA_Manage/OA_UserForm/GetPageHistoryDataList', {
            PageIndex: this.pagination3.current,
            PageRows: this.pagination3.pageSize,
            SortField: this.sorter.field || 'Id',
            SortType: this.sorter.order,
            Search: queryParam3
          })
          .then(resJson => {
            this.loadding = false
            this.data3 = resJson.Data
            const pagination = { ...this.pagination3 }
            pagination.total = resJson.Total || 0
            this.pagination3 = pagination
            this.totalcount = this.pagination1.total + this.pagination2.total + this.pagination3.total
          })
      }
    },
    fetchNotice () {
      if (!this.visible) {
        this.getDataList()
      } else {
        this.loadding = false
      }
      this.visible = !this.visible
    },
    ontabClick (key) {
      this.activeKey = key
      this.getDataList()
    },
    closeNotice () {
      this.visible = false
    },
    selectData1 (id) {
      this.$refs.editForm1.openForm(id)
    },
    selectData2 (id, name, avatar, groupid, groupname, userids, usernames) {
      this.$refs.editForm2.openForm(id, name, avatar, groupid, groupname, userids, usernames)
    },
    selectData3 (id, name) {
      this.$refs.editForm3.openForm(id, name)
    },
    setDataRead (id) {},
    getConfigResult (res) {
      // 接收回调函数返回数据的方法
      const resmsg = res
      this.pagination1.total = resmsg.UserMailCount
      this.pagination2.total = resmsg.UserMessageCount
      this.pagination3.total = resmsg.UserFormCount
      this.totalcount = this.pagination1.total + this.pagination2.total + this.pagination3.total
      const clearcache = resmsg.Clearcache

      if (clearcache.indexOf('Base_User') != -1) {
        this.ClearAllUser()
      }
      if (clearcache.indexOf('Base_Role') != -1) {
        this.ClearAllRole()
      }
    }
  }
}
</script>

<style lang="css">
.header-notice-wrapper {
  top: 50px !important;
}
</style>
<style lang="less" scoped>
.header-notice {
  display: inline-block;
  transition: all 0.3s;

  span {
    vertical-align: initial;
  }
}
</style>
