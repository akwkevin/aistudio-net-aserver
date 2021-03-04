<template>
  <div class="page-header-index-wide">
    <a-card
      :bordered="false"
      :bodyStyle="{ padding: '16px 0', height: '100%' }"
      :style="{ height: '100%' }"
    >
      <a-tabs defaultActiveKey="1" tabPosition="left">
        <a-tab-pane key="0">
          <span slot="tab">写信</span>
          <new-form />
        </a-tab-pane>
        <a-tab-pane key="1">
          <span slot="tab">{{ revtitle }}</span>
          <reveive-list type="1" />
        </a-tab-pane>
        <a-tab-pane key="2">
          <span slot="tab">发件箱</span>
          <reveive-list type="2" />
        </a-tab-pane>
        <a-tab-pane key="3">
          <span slot="tab">草稿箱</span>
          <reveive-list type="3" />
        </a-tab-pane>
      </a-tabs>
    </a-card>
  </div>
</template>

<script>
import ReveiveList from './List'
import NewForm from './NewForm'
export default {
  components: {
    ReveiveList,
    NewForm
  },
  data () {
    return {
      revtitle: '收件箱'
    }
  },
  mounted () {},
  methods: {}
}
</script>

<style>
/* .card-container {
  background: #f5f5f5;
  overflow: hidden;
  padding: 0px;
} */
/* .ant-tabs-tab-active {
  border-color: #fff;
  background: #fff;
} */
.ant-tabs .ant-tabs-left-content {
  padding-left: 0px;
  border-left: 1px solid #e8e8e8;
}
</style>
