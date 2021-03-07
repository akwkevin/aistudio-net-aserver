<template>
  <a-modal width="70%" wrapClassName="web" :visible="visible" @cancel="handleCancel">
    <template slot="title">{{ title }}</template>
    <template slot="footer">
      <a-button key="ok" v-if="mode === 'edit'" @click="handleSubmit">关闭</a-button>
      <a-button key="cancel" @click="handleCancel">关闭</a-button>
    </template>
    <a-card
      :bordered="false"
      :bodyStyle="{ padding: '0', height: '100%' }"
      :style="{ height: '100%' }"
    >
      <G6Editor v-if="visible" :data="data" mode="read"></G6Editor>
    </a-card>
  </a-modal>
</template>

<script>
import G6Editor from '@/components/G6Editor'
export default {
  name: 'G6EditorForm',
  components: { G6Editor },
  props: {
    parentObj: Object
  },
  data () {
    return {
      data: {},
      visible: false,
      title: '',
      mode: 'edit'
    }
  },
  methods: {
    openForm (data, title, mode) {
      // 参数赋值
      this.title = title || '查看流程'
      this.data = data
      this.mode = mode || 'edit'
      this.visible = true
    },
    handleSubmit () {
      this.data = {}
      this.visible = false
    },
    handleCancel () {
      this.data = {}
      this.visible = false
    }
  }
}
</script>

<style>
/deep/ .ant-card-head-title {
  padding: 0 16px 0 16px;
}
</style>
