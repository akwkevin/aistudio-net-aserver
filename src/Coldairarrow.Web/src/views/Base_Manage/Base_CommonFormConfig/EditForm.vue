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
        <a-form-model-item label="表名" prop="Table">
          <a-input v-model="entity.Table" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="列头" prop="Header">
          <a-input v-model="entity.Header" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="属性名" prop="PropertyName">
          <a-input v-model="entity.PropertyName" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="显示索引" prop="DisplayIndex">
          <a-input v-model="entity.DisplayIndex" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="配置类型 编辑项=0，列表=1" prop="Type">
          <a-input v-model="entity.Type" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="格式化" prop="StringFormat">
          <a-input v-model="entity.StringFormat" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否显示 Visible = 0,Hidden = 1,Collapsed = 2" prop="Visibility">
          <a-input v-model="entity.Visibility" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="控件类型，仅控件框使用" prop="ControlType">
          <a-input v-model="entity.ControlType" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="只读" prop="IsReadOnly">
          <a-input v-model="entity.IsReadOnly" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="必输项" prop="IsRequired">
          <a-input v-model="entity.IsRequired" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="字典名" prop="ItemSource">
          <a-input v-model="entity.ItemSource" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="默认值" prop="Value">
          <a-input v-model="entity.Value" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="排序名" prop="SortMemberPath">
          <a-input v-model="entity.SortMemberPath" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="转换器" prop="Converter">
          <a-input v-model="entity.Converter" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="转换参数" prop="ConverterParameter">
          <a-input v-model="entity.ConverterParameter" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="对齐方式 Left = 0,Center = 1,Right = 2,Stretch = 3" prop="HorizontalAlignment">
          <a-input v-model="entity.HorizontalAlignment" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="最大宽度" prop="MaxWidth">
          <a-input v-model="entity.MaxWidth" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="最小宽度" prop="MinWidth">
          <a-input v-model="entity.MinWidth" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="列表宽度" prop="Width">
          <a-input v-model="entity.Width" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否可以重排" prop="CanUserReorder">
          <a-input v-model="entity.CanUserReorder" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否可以调整大小" prop="CanUserResize">
          <a-input v-model="entity.CanUserResize" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否可以排序" prop="CanUserSort">
          <a-input v-model="entity.CanUserSort" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="是否冻结" prop="IsFrozen">
          <a-input v-model="entity.IsFrozen" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="单元格样式，暂未实现" prop="CellStyle">
          <a-input v-model="entity.CellStyle" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="背景颜色触发公式" prop="BackgroundExpression">
          <a-input v-model="entity.BackgroundExpression" autocomplete="off" />
        </a-form-model-item>
        <a-form-model-item label="前景颜色触发公式" prop="ForegroundExpression">
          <a-input v-model="entity.ForegroundExpression" autocomplete="off" />
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
        this.$http.post('/Base_Manage/Base_CommonFormConfig/GetTheData', { id: id }).then(resJson => {
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
        this.$http.post('/Base_Manage/Base_CommonFormConfig/SaveData', this.entity).then(resJson => {
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
