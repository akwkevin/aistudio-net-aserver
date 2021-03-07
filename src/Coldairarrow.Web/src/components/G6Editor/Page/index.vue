<template>
  <div :style="{'margin-left': marginleft + 'px', 'margin-right': '200px' }">
    <div :id="pageId" class="graph-container" style="position: relative;"></div>
  </div>
</template>

<script>
import G6 from '@antv/g6'
import { initBehavors } from '../behavior'
import eventBus from '../utils/eventBus'
export default {
  data () {
    return {
      pageId: 'graph-container',
      graph: null,
      marginleft: 200
    }
  },
  props: {
    height: {
      type: Number,
      default: 0
    },
    width: {
      type: Number,
      default: 0
    },
    data: {
      type: Object,
      default: () => {}
    },
    mode: {
      type: String,
      default: 'edit'
    },
    parentObj: Object
  },
  watch: {
    mode (value) {
      if (this.mode === 'edit') {
        this.graph.addBehaviors(['keyboard', 'customer-events', 'add-menu'], 'default')
      } else {
        this.graph.removeBehaviors(['keyboard', 'customer-events', 'add-menu'], 'default')
      }
    }
  },
  created () {
    initBehavors()
    this.bindEvent()
  },
  mounted () {
    this.$nextTick(() => {
      this.init()
    })
    if (this.mode === 'edit') {
      this.marginleft = 200
    } else {
      this.marginleft = 0
    }
  },
  beforeDestroy () {
    eventBus.$off('reloadData')
  },
  methods: {
    init () {
      const height = this.height - 42
      const width = this.width - 400

      this.graph = new G6.Graph({
        container: 'graph-container',
        height: height,
        width: width,
        modes: {
          // 支持的 behavior
          default: [
            'drag-canvas',
            'zoom-canvas',
            'hover-node',
            'select-node',
            'hover-edge',
            'keyboard',
            'customer-events',
            'add-menu'
          ],
          mulitSelect: ['mulit-select'],
          addEdge: ['add-edge'],
          moveNode: ['drag-item']
        }
      })

      if (this.mode === 'edit') {
        this.graph.addBehaviors(['keyboard', 'customer-events', 'add-menu'], 'default')
      } else {
        this.graph.removeBehaviors(['keyboard', 'customer-events', 'add-menu'], 'default')
      }

      const { editor, command } = this.$parent
      editor.emit('afterAddPage', { graph: this.graph, command })

      this.readData()
    },
    bindEvent () {
      const self = this
      eventBus.$on('reloadData', data => {
        self.reloadData(data)
      })
    },
    readData () {
      const data = this.data
      if (data && JSON.stringify(data) !== '{}') {
        this.graph.read(data)
      }
    },
    reloadData (data) {
      if (data === null || JSON.stringify(data) === '{}') {
        data = { nodes: [], edges: [], groups: [] }
      }
      this.graph.changeData(data)
    }
  }
}
</script>

<style scoped>
.page-left {
  margin-left: 200px;
  margin-right: 200px;
}
.page {
  margin-left: 0px;
  margin-right: 200px;
}
</style>
