<template>
  <div class="page-header-index-wide">
    <a-card
      :bordered="false"
      :bodyStyle="{ padding: '0', height: '100%' }"
      :style="{ height: '100%' }"
    >
      <G6Editor mode="edit" @ok="saveData"></G6Editor>
    </a-card>
  </div>
</template>

<script>
import G6Editor from '@/components/G6Editor'
export default {
  name: 'App',
  components: { G6Editor },

  methods: {
    saveData (data) {
      this.$http.post('/OA_Manage/OA_UserForm/TestJsonData', { json: data }).then(resJson => {
        if (resJson.Success) {
          this.$message.success(resJson.Msg)
        } else {
          this.$message.error(resJson.Msg)
        }
      })
    }
  }
}
</script>

<style>
/deep/ .ant-card-head-title {
  padding: 0 16px 0 16px;
}
</style>
