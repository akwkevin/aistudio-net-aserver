<template>
  <div id="mountNode" :style="{ width: width }">
    <div class="editor">
      <context-menu />
      <!--toolbar-->
      <toolbar v-if="mode === 'edit'" />
      <div v-if="mode === 'edit'" style="height: 42px;"></div>
      <div class="bottom-container">
        <!--itempannel-->
        <item-panel v-if="mode === 'edit'" />
        <!--detailpannel-->
        <detail-panel :mode="mode" :users="users" :roles="roles" />
        <!--miniMap-->
        <minimap />
        <!--page-->
        <page :height="height" :width="width" :mode="mode" :data="data" />
      </div>
    </div>
    <Flow />
  </div>
</template>

<script>
import Toolbar from './Toolbar'
import ItemPanel from './ItemPanel'
import DetailPanel from './DetailPanel'
import Minimap from './Minimap'
import Page from './Page'
import Flow from './Flow'
import ContextMenu from './ContextMenu'
import Editor from './Base/Editor'
import command from './command'
import eventBus from './utils/eventBus'

export default {
  name: 'G6Editor',
  components: {
    Toolbar,
    ItemPanel,
    DetailPanel,
    Minimap,
    Page,
    ContextMenu,
    Flow
  },
  props: {
    height: {
      type: Number,
      default: document.documentElement.clientHeight * 0.7
    },
    width: {
      type: Number,
      default: document.documentElement.clientWidth
    },
    data: {
      type: Object,
      default: () => {}
    },
    mode: {
      type: String,
      default: 'edit'
    },
    users: {
      type: Array,
      default: () => {}
    },
    roles: {
      type: Array,
      default: () => {}
    }
  },
  data () {
    return {
      page: {},
      graph: {}
    }
  },
  created () {
    this.init()
    this.bindEvent()
  },
  beforeDestroy () {
    eventBus.$off('saveData')
    eventBus.$off('afterAddPage')
  },
  data () {
    return {
      editor: {},
      command: null
    }
  },
  watch: {
    data (value) {
      this.reloadData(value)
      eventBus.$emit('nodeselectchange', { target: {}, select: false })
    }
  },
  methods: {
    init () {
      this.editor = new Editor()
      this.command = new command(this.editor)
    },
    bindEvent () {
      const self = this
      eventBus.$on('saveData', data => {
        self.saveData(data)
      })
      eventBus.$on('afterAddPage', page => {
        self.page = page
        self.graph = self.page.graph
      })
    },
    saveData (data) {
      this.$emit('ok', data)
    },
    reloadData (data) {
      eventBus.$emit('reloadData', data)
    },
    getData () {
      var data = this.graph.save()
      return data
    }
  }
}
</script>

<style scoped>
.editor {
  position: relative;
  width: 100%;
  user-select: none;
  -moz-user-select: none;
  -webkit-user-select: none;
  -ms-user-select: none;
}
.bottom-container {
  position: relative;
}
</style>
