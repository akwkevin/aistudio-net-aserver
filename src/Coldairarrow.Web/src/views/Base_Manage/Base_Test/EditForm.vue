<template>
  <a-modal
    :title="title"
    width="40%"
    :visible="visible"
    :confirmLoading="loading"
    @ok="handleSubmit"
    @cancel="()=>{this.visible=false}"
  >
    <a-spin :spinning="loading">
      <a-form-model ref="form" :model="entity" :rules="rules" v-bind="layout">
        <a-form-model-item label="父级Id" prop="ParentId">
          <a-input v-model="entity.ParentId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="类型,菜单=0,页面=1,权限=2" prop="Type">
          <a-input v-model="entity.Type" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="权限名/菜单名" prop="Name">
          <a-input v-model="entity.Name" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="菜单地址" prop="Url">
          <a-input v-model="entity.Url" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="权限值" prop="Value">
          <a-input v-model="entity.Value" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否需要权限(仅页面有效)" prop="NeedTest">
          <a-input v-model="entity.NeedTest" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="图标" prop="Icon">
          <a-input v-model="entity.Icon" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="排序" prop="Sort">
          <a-input v-model="entity.Sort" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="修改时间" prop="ModifyTime">
          <a-input v-model="entity.ModifyTime" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="修改人Id" prop="ModifyId">
          <a-input v-model="entity.ModifyId" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="修改人" prop="ModifyName">
          <a-input v-model="entity.ModifyName" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="租户Id" prop="TenantId">
          <a-input v-model="entity.TenantId" autocomplete="off" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
  </a-modal>
</template>

<script>
export default {
  props: {
    parentObj: Object
  },
  data() {
    return {
      layout: {
        labelCol: { span: 5 },
        wrapperCol: { span: 18 }
      },
      visible: false,
      loading: false,
      entity: {},
      rules: {},
      title: ''
    }
  },
  methods: {
    init() {
      this.visible = true
      this.entity = {}
      this.$nextTick(() => {
        this.$refs['form'].clearValidate()
      })
    },
    openForm(id, title) {
      this.init()

      if (id) {
        this.loading = true
        this.$http.post('/Base_Manage/Base_Test/GetTheData', { id: id }).then(resJson => {
          this.loading = false

          this.entity = resJson.Data
        })
      }
    },
    handleSubmit() {
      this.$refs['form'].validate(valid => {
        if (!valid) {
          return
        }
        this.loading = true
        this.$http.post('/Base_Manage/Base_Test/SaveData', this.entity).then(resJson => {
          this.loading = false

          if (resJson.Success) {
            this.$message.success('操作成功!')
            this.visible = false

            this.parentObj.getDataList()
          } else {
            this.$message.error(resJson.Msg)
          }
        })
      })
    }
  }
}
</script>
