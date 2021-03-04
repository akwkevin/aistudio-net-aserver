<template>
  <div>
    <a-layout :style="{ height: height - 155 + 'px' }">
      <a-layout-sider width="250" :style="{ background: '#fff', margin: 0, padding: 0 }">
        <a-select
          allowClear
          show-search
          option-filter-prop="children"
          :filter-option="filterOption"
          style="width: 210px; margin: 2px"
          @change="search"
        >
          <a-select-option v-for="d in sortedUserDatas" :key="d.UserId">{{ d.UserName }}</a-select-option>
        </a-select>
        <a-button type="primary" icon="usergroup-add" style="margin: 2px" @click="hanldleAdd" />
        <a-list :dataSource="sortedUserDatas" :loading="loading">
          <a-list-item
            slot="renderItem"
            slot-scope="item, index"
            :class="{ selected: item === selectedData }"
            @click="selectData(item)"
            style="padding-left: 10px; padding-right: 10px"
          >
            <a-avatar
              :class="{ gray: item.Online === false }"
              size="large"
              shape="square"
              :src="item.Avatar | AvatarFilter"
              style="backgroundcolor: #87d068"
              @click="handleEdit(item)"
            />
            <a-row type="flex" style="padding-left: 5px; width: 80%">
              <a-col :span="24" style="color: blue">{{ item.UserName }}</a-col>
              <a-col :span="18">
                <ellipsis :length="16" tooltip>{{ item.LastMessage }}</ellipsis>
              </a-col>
              <a-col :span="6" style="text-align: right">{{ item.LastDateTime | filterDate }}</a-col>
            </a-row>
          </a-list-item>
        </a-list>
      </a-layout-sider>
      <a-layout style="padding: 0 12px 0">
        <a-layout-content :style="{ background: '#fff', padding: '0px', margin: 0 }">
          <chat-box
            @submit="sendText"
            @history="getHisData"
            :title="title"
            :messages="messages"
            :height="height"
            :loading="loading"
          ></chat-box>
        </a-layout-content>
      </a-layout>
    </a-layout>
    <new-group-form ref="editForm" :parentObj="this"></new-group-form>
  </div>
</template>

<script>
import moment from 'moment'
import Ellipsis from '@/components/Ellipsis'
import { mapGetters } from 'vuex'
import ChatBox from '@/components/Chat/ChatBox'
import NewGroupForm from './NewGroupForm'

var messageType = 2
var readmessageType = 6

export default {
  components: {
    Ellipsis,
    ChatBox,
    NewGroupForm
  },
  props: {
    height: {
      type: Number,
      default: document.documentElement.clientHeight * 1
    }
  },
  mounted () {
    this.getDataList()
    this.socketApi.addcallback(this.getConfigResult, messageType)
  },
  data () {
    return {
      userdatas: [],
      localdatas: JSON.parse(localStorage.getItem('userdatas')) || [],
      loading: false,
      messages: [],
      contentText: null,
      selectedUserId: null,
      title: ''
    }
  },
  methods: {
    getDataList () {
      this.loading = true
      this.$http.post('/D_Manage/D_UserMessage/GetUserList').then((resJson) => {
        this.loading = false

        if (resJson.Data) {
          this.userdatas = resJson.Data
          if (this.localdatas) {
            this.userdatas.forEach((item) => {
              const tempdata = this.localdatas.find((d) => d.UserId === item.UserId)
              if (tempdata) {
                item.LastDateTime = tempdata.LastDateTime
                item.LastMessage = tempdata.LastMessage
              }
            })
          }

          if (this.sortedUserDatas && this.selectedUserId === null) {
            this.selectedUserId = this.sortedUserDatas[0].UserId
            this.title = this.sortedUserDatas[0].UserName
          }
        }
      })
    },
    sendText (text, type) {
      if (!text || !this.selectedData) {
        return
      }
      const sendMessage = {}
      sendMessage.Text = text.replace('\n', '')
      if (this.selectedData.IsGroup) {
        sendMessage.GroupId = this.selectedData.UserId
        sendMessage.GroupName = this.selectedData.UserName
        sendMessage.UserNames = this.selectedData.UserNames
        sendMessage.UserIds = this.selectedData.UserIds
      } else {
        sendMessage.UserNames = '^' + this.selectedData.UserName + '^'
        sendMessage.UserIds = '^' + this.selectedData.UserId + '^'
      }
      sendMessage.Avatar = this.userInfo.Avatar
      if (type) {
        sendMessage.Type = type
      } else {
        sendMessage.Type = 1
      }
      const send = {
        Success: true,
        Data: sendMessage,
        MessageType: messageType
      }
      this.socketApi.sendSock(JSON.stringify(send))
    },
    getConfigResult (res) {
      // 接收回调函数返回数据的方法
      const resmsg = res
      if (!this.selectedUserId) {
        return
      }
      if (
        (this.selectedData.IsGroup === false &&
          !resmsg.GroupId &&
          (resmsg.CreatorId === this.userInfo.Id ||
            resmsg.CreatorId === this.selectedUserId)) ||
        (this.selectedData.IsGroup && this.selectedUserId === resmsg.GroupId)
      ) {
        let endmsg = null
        if (this.messages && this.messages.length > 0) {
          endmsg = this.messages[this.messages.length - 1]
        }
        if (endmsg !== null && typeof endmsg !== 'undefined') {
          const date1 = new Date(resmsg.CreateTime) // 开始时间
          const date2 = new Date(endmsg.CreateTime) // 结束时间
          const date3 = date1.getTime() - date2.getTime() // 时间差的毫秒数
          if (date3 > 60000) {
            resmsg.ShowTime = true
          } else {
            resmsg.ShowTime = false
          }
        } else {
          resmsg.ShowTime = true
        }
        this.messages = this.messages.concat(resmsg)
      }

      if (resmsg.CreatorId !== this.userInfo.Id) {
        if ((resmsg.ReadingMarks || '').indexOf('^' + this.userInfo.Id + '^') === -1) {
          resmsg.ReadingMarks = (resmsg.ReadingMarks || '^') + this.userInfo.Id + '^'
          // 设为已读
          const send = {
            Success: true,
            Data: resmsg,
            MessageType: readmessageType
          }
          this.socketApi.sendSock(JSON.stringify(send))
        }

        let tempdata = null
        if (!resmsg.GroupId) {
          tempdata = this.userdatas.find((d) => d.UserId === resmsg.CreatorId)
        } else {
          tempdata = this.userdatas.find((d) => d.UserId === resmsg.GroupId)
        }
        if (tempdata) {
          tempdata.LastDateTime = resmsg.CreateTime
          tempdata.LastMessage = resmsg.Text
        }
      }
    },
    selectData (item) {
      this.messages = []
      this.selectedUserId = item.UserId
      this.title = item.UserName
    },
    saveUserDatas () {
      localStorage.setItem('userdatas', JSON.stringify(this.userdatas))
    },
    mysort (a, b) {
      if (a.Online && b.Online) {
        if (a.LastDateTime && b.LastDateTime) {
          return a.LastDateTime > b.LastDateTime ? -1 : 1
        } else if (a.LastDateTime) {
          return -1
        } else {
          return 1
        }
      } else if (a.Online) {
        return -1
      } else {
        return 1
      }
    },
    getHisData (startValue, endValue) {
      if (!this.selectedUserId) {
        return
      }
      if (startValue && endValue) {
        this.loading = true
        var queryParam = {
          creatorId: this.userInfo.Id,
          userId: this.selectedData.UserId,
          isGroup: this.selectedData.IsGroup,
          start: moment(startValue).format('YYYY-MM-DD HH:mm:ss'),
          end: moment(endValue).format('YYYY-MM-DD HH:mm:ss')
        }

        this.$http
          .post('/D_Manage/D_UserMessage/GetHistoryDataList', {
            Search: queryParam
          })
          .then((resJson) => {
            this.messages = resJson.Data || []
            this.loading = false
          })
      }
    },
    filterOption (input, option) {
      return option.componentOptions.children[0].text.toLowerCase().indexOf(input.toLowerCase()) >= 0
    },
    search (value) {
      const tempdata = this.userdatas.find((d) => d.UserId === value)
      if (tempdata) {
        this.selectedUserId = tempdata.UserId
        this.title = tempdata.UserName
      }
    },
    hanldleAdd () {
      this.$refs.editForm.openForm()
    },
    handleEdit (item) {
      if (item.IsGroup) {
        this.$refs.editForm.openForm(item.UserId, item.UserName)
      }
    },
    getUnReadData () {
      if (this.selectedUserId) {
        this.loading = true
        var queryParam = {
          userId: this.userInfo.Id,
          creatorId: this.selectedUserId,
          markflag: true,
          isGroup: this.selectedData.IsGroup
        }

        this.$http
          .post('/D_Manage/D_UserMessage/GetHistoryDataPageList', {
            Search: queryParam
          })
          .then((resJson) => {
            this.messages = resJson.Data || []
            this.loading = false
          })
      }
    }
  },
  computed: {
    ...mapGetters(['userInfo']),
    selectedData () {
      // We return the matching note with selectedId
      return this.userdatas.find((d) => d.UserId === this.selectedUserId)
    },
    sortedUserDatas () {
      return this.userdatas.slice().sort((a, b) => this.mysort(a, b))
    }
  },
  watch: {
    userdatas: {
      handler: 'saveUserDatas',
      deep: true
    },
    selectedUserId: {
      handler: 'getUnReadData'
    }
  },
  filters: {
    filterDate: function (time) {
      if (time !== null && time !== '') {
        var date = new Date(time)
        var todaysDate = new Date()
        if (date.getDate() === todaysDate.getDate()) {
          return moment(date).format('HH:mm:ss')
        } else {
          return moment(date).format('YYYY-MM-DD')
        }
      } else {
        return ''
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

.gray {
  -webkit-filter: grayscale(100%);
  -moz-filter: grayscale(100%);
  -ms-filter: grayscale(100%);
  -o-filter: grayscale(100%);
  filter: grayscale(100%);
  filter: gray;
  opacity: 0.7; //通过改变透明度来调节灰色的程度
}

.content {
  display: inline-block;
  padding: 1.5vh;
  background: #fff;
}

.selected {
  background: #9eea6a;
  // color: white;
}
</style>
