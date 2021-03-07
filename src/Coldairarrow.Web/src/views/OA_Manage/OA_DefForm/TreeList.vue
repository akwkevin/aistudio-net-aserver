<template>
  <div class="page-header-index-wide page-header-wrapper-grid-content-main">
    <a-row :gutter="24">
      <a-col :md="24" :lg="12">
        <a-card :bordered="false">
          <div class="table-operator">
            <a-button type="primary" icon="redo" @click="getDataList()">刷新</a-button>
          </div>
          <a-tree default-expand-all :tree-data="data" :expanded-keys="expandedKeys">
            <template slot="title" slot-scope="{ title }">{{ title }}</template>
            <template slot="titleExtend" slot-scope="{key,title,type, jsonId,jsonVersion,json}">
              {{ title }}
              <a-divider type="vertical" />
              <a @click="handleEdit(key,title,type,jsonId,jsonVersion,json)">创建</a>
              <a-divider type="vertical" />
              <a @click="handleOpenG6Editor(json)">查看流程图</a>
            </template>
          </a-tree>
        </a-card>
      </a-col>
      <a-col :md="24" :lg="12">
        <a-card :bordered="false"></a-card>
      </a-col>
    </a-row>
    <edit-form ref="editForm" :parentObj="this"></edit-form>
    <g6-editor-form ref="g6editorForm" :parentObj="this"></g6-editor-form>
  </div>
</template>

<script>
import EditForm from '../OA_UserForm/EditForm'
import G6EditorForm from '../OA_UserForm/G6EditorForm'

export default {
  components: {
    EditForm,
    G6EditorForm
  },
  mounted () {
    this.getDataList()
  },

  data () {
    return {
      data: [],
      expandedKeys: [],
      loading: false
    }
  },
  methods: {
    getDataList () {
      this.loading = true
      this.$http.post('/OA_Manage/OA_DefForm/GetTreeDataList', {}).then(resJson => {
        this.loading = false
        this.data = resJson.Data
        this.expandedKeys = this.data.map(item => {
          return item.key
        })
      })
    },
    handleEdit (key, title, type, jsonId, jsonVersion, json) {
      this.$refs.editForm.openForm('', title, type, key, jsonId, jsonVersion, json)
    },
    handleOpenG6Editor (json) {
      this.$refs.g6editorForm.openForm(JSON.parse(json), '查看流程图', 'read')
    }
  }
}
</script>
