<template>
  <a-modal :title="title" :width="width" :visible="visible" :confirmLoading="loading" @cancel="handleCancel">
    <template slot="footer">
      <a-button
        v-if="!disabled && this.entity.Id"
        key="delete"
        type="danger"
        :loading="loading"
        @click="handleDelete"
      >解散</a-button
      >
      <a-button
        v-if="disabled && this.entity.Id"
        key="exit"
        type="danger"
        :loading="loading"
        @click="handleExit"
      >退出</a-button
      >
      <a-button
        key="submit"
        type="primary"
        :loading="loading"
        :disabled="disabled"
        @click="handleSubmit"
      >提交</a-button
      >
      <a-button key="cancel" :loading="loading" @click="handleCancel">关闭</a-button>
    </template>
    <a-spin :spinning="loading">
      <a-form :form="form">
        <a-form-item label="组员" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-select mode="tags" style="width: 100%" v-model="tags" :disabled="disabled">
            <a-select-option v-for="d in useroption" :key="d.value" >{{ d.text }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="组名" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input v-model="name" :disabled="disabled" />
        </a-form-item>
        <a-form-item label="头像" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <div class="ant-upload-preview" @click="$refs.modal.edit(1)">
            <!-- <a-icon type="cloud-upload-o" class="upload-icon" /> -->
            <div class="mask">
              <a-icon type="plus" />
            </div>
            <img :src="avatar | GroupAvatarFilter" />
          </div>
        </a-form-item>
        <a-form-item label="主题" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input v-model="gtitle" :disabled="disabled" />
        </a-form-item>
        <a-form-item label="备注" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <a-input v-model="remark" :disabled="disabled" />
        </a-form-item>
        <a-form-item label="附件" :labelCol="labelCol" :wrapperCol="wrapperCol">
          <c-upload-file v-model="appendix" :maxCount="1" :disabled="disabled"></c-upload-file>
        </a-form-item>
      </a-form>
    </a-spin>
    <avatar-modal ref="modal" @ok="setavatar" />
  </a-modal>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import CUploadFile from '@/components/CUploadFile/CUploadFile'
import AvatarModal from '../../account/settings/AvatarModal'

export default {
  components: {
    CUploadFile,
    AvatarModal
  },
  props: {
    // eslint-disable-next-line vue/require-default-prop
    parentObj: Object
  },
  data () {
    return {
      form: this.$form.createForm(this),
      labelCol: { xs: { span: 24 }, sm: { span: 2 } },
      wrapperCol: { xs: { span: 24 }, sm: { span: 21 } },
      visible: false,
      loading: false,
      entity: {},
      title: '',
      width: '60%',
      disabled: false,
      tags: [],
      tagInputVisible: false,
      tagInputValue: '',
      useroption: [],
      gtitle: '',
      remark: '',
      avatar: '',
      appendix: '',
      name: ''
    }
  },
  mounted () {
    this.GetAllUser().then((res) => {
      this.useroption = res
    })
  },
  computed: {
    ...mapGetters(['userInfo'])
  },
  methods: {
    ...mapActions(['GetAllUser']),
    openForm (id, title) {
      // 参数赋值
      this.title = title || '新增群组'
      this.loading = true
      // 组件初始化
      this.init()

      // 编辑赋值
      if (id) {
        this.$nextTick(() => {
          this.$http.post('/D_Manage/D_UserMessage/GetTheData', { id: id }).then((resJson) => {
            this.entity = resJson.Data
            this.tags = resJson.Data.UserIds.slice(1, -1).split('^')
            this.name = this.entity.Name
            this.gtitle = this.entity.Title
            this.remark = this.entity.Remark
            this.appendix = this.entity.Appendix
            this.avatar = this.entity.Avatar
            this.disabled = this.entity.CreatorId !== this.userInfo.Id
            this.loading = false
          })
        })
      } else {
        this.loading = false
      }
    },
    init () {
      this.entity = {}
      this.visible = true
      this.remark = ''
      this.gtitle = ''
      this.appendix = ''
      this.name = ''
      this.tags = []
      this.avatar = ''
      this.disabled = false
    },
    setavatar (url) {
      this.avatar = url
    },
    handleSubmit (isDraft) {
      if (this.tags.length === 0) {
        this.$message.error('群组成员为空')
        return
      }
      if (this.name === '') {
        this.$message.error('群组名为空')
        return
      }
      const userNames = []
      let error = false
      this.tags.forEach((item) => {
        const tempuser = this.useroption.find((d) => d.value === item)
        if (tempuser !== null && typeof tempuser !== 'undefined') {
          userNames.push(tempuser.text)
        } else {
          this.$message.error('存在未知的群组成员:' + item)
          error = true
          return true
        }
      })
      if (!error) {
        this.entity.Title = this.gtitle
        this.entity.UserNames = '^' + userNames.join('^') + '^'
        this.entity.UserIds = '^' + this.tags.join('^') + '^'
        this.entity.Appendix = this.appendix
        this.entity.Remark = this.remark
        this.entity.Name = this.name
        this.entity.Avatar = this.avatar || '/Images/group.jpg'

        this.loading = true
        this.$http.post('/D_Manage/D_UserMessage/SaveData', this.entity).then((resJson) => {
          this.loading = false

          if (resJson.Success) {
            this.$message.success('操作成功!')
            this.visible = false
            this.parentObj.getDataList()
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      }
    },
    handleDelete () {
      var thisObj = this
      this.$confirm({
        title: '确认解散吗?',
        onOk () {
          return new Promise((resolve, reject) => {
            thisObj.$http
              .post('/D_Manage/D_UserMessage/DeleteData', { ids: [thisObj.entity.Id] })
              .then((resJson) => {
                resolve()

                if (resJson.Success) {
                  thisObj.$message.success('操作成功!')
                  thisObj.visible = false
                  thisObj.parentObj.getDataList()
                } else {
                  thisObj.$message.error(resJson.Msg)
                }
              })
          })
        }
      })
    },
    handleExit () {
      var thisObj = this
      this.$confirm({
        title: '确认退出吗?',
        onOk () {
          const userNames = []
          const userIds = []
          thisObj.tags.forEach((item) => {
            const tempuser = thisObj.useroption.find((d) => d.value === item)
            if (tempuser !== null && typeof tempuser !== 'undefined' && tempuser.value !== thisObj.userInfo.Id) {
              userNames.push(tempuser.text)
              userIds.push(tempuser.value)
            }
          })
          thisObj.entity.UserNames = '^' + userNames.join('^') + '^'
          thisObj.entity.UserIds = '^' + userIds.join('^') + '^'
          return new Promise((resolve, reject) => {
            thisObj.$http.post('/D_Manage/D_UserMessage/SaveData', thisObj.entity).then((resJson) => {
              resolve()

              if (resJson.Success) {
                thisObj.$message.success('操作成功!')
                thisObj.visible = false
                thisObj.parentObj.getDataList()
              } else {
                thisObj.$message.error(resJson.Msg)
              }
            })
          })
        }
      })
    },
    handleCancel () {
      this.visible = false
    }
  },
  filters: {
    GroupAvatarFilter: function (data) {
      if (data === null || data === '' || typeof data === 'undefined') return require('@/images/Images/group.jpg')
      else if (data.indexOf('/Images') === 0) {
        return require('@/images' + data)
      } else {
        return data
      }
    }
  }
}
</script>

<style lang="less" scoped>
.avatar-upload-wrapper {
  height: 100px;
  width: 100%;
}

.ant-upload-preview {
  position: relative;
  margin: 0 auto;
  width: 100%;
  max-width: 90px;
  border-radius: 50%;
  box-shadow: 0 0 4px #ccc;

  .upload-icon {
    position: absolute;
    top: 0;
    right: 10px;
    font-size: 1.4rem;
    padding: 0.5rem;
    background: rgba(222, 221, 221, 0.7);
    border-radius: 50%;
    border: 1px solid rgba(0, 0, 0, 0.2);
  }
  .mask {
    opacity: 0;
    position: absolute;
    background: rgba(0, 0, 0, 0.4);
    cursor: pointer;
    transition: opacity 0.4s;

    &:hover {
      opacity: 1;
    }

    i {
      font-size: 2rem;
      position: absolute;
      top: 50%;
      left: 50%;
      margin-left: -1rem;
      margin-top: -1rem;
      color: #d6d6d6;
    }
  }

  img,
  .mask {
    width: 100%;
    max-width: 90px;
    height: 100%;
    border-radius: 50%;
    overflow: hidden;
  }
}
</style>
