<template>
  <div class="toolbar">
    <a-tooltip class="command" title="撤销">
      <a-button
        type="dashed"
        icon="undo"
        @click="handleUndo"
        :class="undoList.length > 0 ? '' : 'disable'"
      />
    </a-tooltip>
    <a-tooltip class="command" title="重做">
      <a-button
        type="dashed"
        icon="redo"
        @click="handleRedo"
        :class="redoList.length > 0 ? '' : 'disable'"
      />
    </a-tooltip>
    <span class="separator"></span>
    <!-- <i data-command="copy" class="command iconfont icon-copy-o disable" title="复制"></i>
    <i data-command="paste" class="command iconfont icon-paster-o disable" title="粘贴"></i>-->
    <a-tooltip class="command" title="删除">
      <a-button
        type="dashed"
        icon="delete"
        @click="handleDelete"
        :class="selectedItem ? '' : 'disable'"
      />
    </a-tooltip>
    <span class="separator"></span>
    <a-tooltip class="command" title="放大">
      <a-button type="dashed" icon="zoom-in" @click="handleZoomIn" />
    </a-tooltip>
    <a-tooltip class="command" title="缩小">
      <a-button type="dashed" icon="zoom-out" @click="handleZoomOut" />
    </a-tooltip>
    <a-tooltip class="command" title="适应画布">
      <a-button type="dashed" icon="drag" @click="handleAutoZoom" />
    </a-tooltip>
    <a-tooltip class="command" title="实际尺寸">
      <a-button type="dashed" icon="column-width" @click="handleResetZoom" />
    </a-tooltip>
    <span class="separator"></span>
    <a-tooltip class="command" title="层级后置">
      <a-button
        type="dashed"
        :class="selectedItem ? '' : 'disable'"
        icon="vertical-align-bottom"
        @click="handleToBack"
      />
    </a-tooltip>
    <a-tooltip class="command" title="层级前置">
      <a-button
        type="dashed"
        :class="selectedItem ? '' : 'disable'"
        icon="vertical-align-top"
        @click="handleToFront"
      />
    </a-tooltip>
    <span class="separator"></span>
    <a-tooltip class="command" title="多选">
      <a-button
        type="dashed"
        :class="multiSelect ? 'disable' : ''"
        icon="select"
        @click="handleMuiltSelect"
      />
    </a-tooltip>
    <span class="separator"></span>
    <a-tooltip class="command" title="成组">
      <a-button
        type="dashed"
        :class="addGroup ? '' : 'disable'"
        icon="block"
        @click="handleAddGroup"
      />
    </a-tooltip>
    <a-tooltip class="command" title="解组">
      <a-button
        type="dashed"
        :class="unGroup ? '' : 'disable'"
        icon="border"
        @click="handleUnGroup"
      />
    </a-tooltip>
    <span class="separator"></span>
    <a-tooltip class="command" title="清除数据">
      <a-button type="dashed" icon="close" @click="clearData" />
    </a-tooltip>
    <a-tooltip class="command" title="输出数据">
      <a-button type="dashed" icon="code" @click="consoleData" />
    </a-tooltip>
    <a-tooltip class="command" title="保存数据" v-if="false">
      <a-button type="dashed" icon="save" @click="saveData" />
    </a-tooltip>
  </div>
</template>

<script>
import eventBus from '../utils/eventBus'
import Util from '@antv/g6/lib/util'
import { uniqueId, getBox } from '../utils'
export default {
  props: {
    parentObj: Object
  },
  data () {
    return {
      page: {},
      graph: {},
      redoList: [],
      undoList: [],
      editor: null,
      command: null,
      selectedItem: null,
      multiSelect: false,
      addGroup: false,
      unGroup: false
    }
  },
  created () {
    this.init()
    this.bindEvent()
  },
  beforeDestroy () {
    eventBus.$off('afterAddPage')
    eventBus.$off('add')
    eventBus.$off('update')
    eventBus.$off('delete')
    eventBus.$off('updateItem')
    eventBus.$off('addItem')
    eventBus.$off('nodeselectchange')
    eventBus.$off('deleteItem')
    eventBus.$off('muliteSelectEnd')
  },
  watch: {
    selectedItem (val) {
      if (val && val.length > 1) {
        this.addGroup = true
      } else {
        this.addGroup = false
      }
    }
  },
  methods: {
    init () {
      const { editor, command } = this.$parent
      this.editor = editor
      this.command = command
    },
    bindEvent () {
      const self = this
      eventBus.$on('afterAddPage', page => {
        self.page = page
        self.graph = self.page.graph
      })
      eventBus.$on('add', data => {
        this.redoList = data.redoList
        this.undoList = data.undoList
      })
      eventBus.$on('update', data => {
        this.redoList = data.redoList
        this.undoList = data.undoList
      })
      eventBus.$on('delete', data => {
        this.redoList = data.redoList
        this.undoList = data.undoList
      })
      eventBus.$on('updateItem', item => {
        this.command.executeCommand('update', [item])
      })
      eventBus.$on('addItem', item => {
        this.command.executeCommand('add', [item])
      })
      eventBus.$on('nodeselectchange', () => {
        this.selectedItem = this.graph.findAllByState('node', 'selected')
        this.selectedItem = this.selectedItem.concat(...this.graph.findAllByState('edge', 'selected'))
      })
      eventBus.$on('deleteItem', () => {
        this.handleDelete()
      })
      eventBus.$on('muliteSelectEnd', () => {
        this.multiSelect = false
        this.selectedItem = this.graph.findAllByState('node', 'selected')
      })
    },
    handleUndo () {
      if (this.undoList.length > 0) this.command.undo()
    },
    handleRedo () {
      if (this.redoList.length > 0) this.command.redo()
    },
    handleDelete () {
      if (this.selectedItem.length > 0) {
        this.command.executeCommand('delete', this.selectedItem)
        this.selectedItem = null
      }
    },
    getFormatPadding () {
      return Util.formatPadding(this.graph.get('fitViewPadding'))
    },
    getViewCenter () {
      const padding = this.getFormatPadding()
      const graph = this.graph
      const width = this.graph.get('width')
      const height = graph.get('height')
      return {
        x: (width - padding[2] - padding[3]) / 2 + padding[3],
        y: (height - padding[0] - padding[2]) / 2 + padding[0]
      }
    },
    handleZoomIn () {
      const currentZoom = this.graph.getZoom()
      this.graph.zoomTo(currentZoom + 0.5, this.getViewCenter())
    },
    handleZoomOut () {
      const currentZoom = this.graph.getZoom()
      this.graph.zoomTo(currentZoom - 0.5, this.getViewCenter())
    },
    handleToBack () {
      if (this.selectedItem && this.selectedItem.length > 0) {
        this.selectedItem.forEach(item => {
          item.toBack()
          this.graph.paint()
        })
      }
    },
    handleToFront () {
      if (this.selectedItem && this.selectedItem.length > 0) {
        this.selectedItem.forEach(item => {
          if (item.getType() === 'edge') {
            // const nodeGroup = this.graph.get("nodeGroup");
            // const edgeGroup = item.get("group");
            // nodeGroup.toFront();
            // edgeGroup.toFront()
          } else {
            item.toFront()
          }

          this.graph.paint()
        })
      }
    },
    handleAutoZoom () {
      this.graph.fitView(20)
    },
    handleResetZoom () {
      this.graph.zoomTo(1, this.getViewCenter())
    },
    handleMuiltSelect () {
      this.multiSelect = true
      this.graph.setMode('mulitSelect')
    },
    handleAddGroup () {
      // TODO 这部分等阿里更新Group之后添加
      // const model = {
      //   id: "group" + uniqueId(),
      //   title: "新建分组"
      // };
      // // this.command.executeCommand("add", "group", model);
      // this.selectedItem.forEach(item => {
      //   console.log(item);
      // });
      // this.getPosition(this.selectedItem);
    },
    handleUnGroup () {},
    getPosition (items) {
      const boxList = []
      items.forEach(item => {
        const box = item.getBBox()
        boxList.push(getBox(box.x, box.y, box.width, box.height))
      })
      let minX1, minY1, MaxX2, MaxY2
      boxList.forEach(box => {
        if (typeof minX1 === 'undefined') {
          minX1 = box.x1
        }
        if (typeof minY1 === 'undefined') {
          minY1 = box.y1
        }
        if (typeof MaxX2 === 'undefined') {
          MaxX2 = box.x2
        }
        if (typeof MaxY2 === 'undefined') {
          MaxY2 = box.y2
        }
        if (minX1 > box.x1) {
          minX1 = box.x1
        }
        if (minY1 > box.y1) {
          minY1 = box.y1
        }
        if (MaxX2 < box.x2) {
          MaxX2 = box.x2
        }
        if (MaxY2 < box.y2) {
          MaxY2 = box.y2
        }
      })
      const width = MaxX2 - minX1
      const height = MaxY2 - minY1
      const x = minX1 + width / 2
      const y = minY1 + height / 2
      const id = 'team' + uniqueId()
      const model = {
        id: id,
        width,
        height,
        x,
        y,
        shape: 'teamNode'
      }
      this.command.executeCommand('add', model)
      // const item = this.graph.findById(id);
      // item.get("group").toBack();
      // const edgeGroup = this.graph.get("edgeGroup");
      // edgeGroup.toFront();
      // this.graph.paint();
    },
    clearData () {
      eventBus.$emit('reloadData', {})
    },
    consoleData () {
      console.log(this.graph.save())
    },
    saveData () {
      eventBus.$emit('saveData', this.graph.save())
    }
  }
}
</script>

<style scoped>
.toolbar {
  box-sizing: border-box;
  padding: 4px;
  width: 100%;
  border: 1px solid #e9e9e9;
  height: 42px;
  z-index: 3;
  box-shadow: 0px 8px 12px 0px rgba(0, 52, 107, 0.04);
  position: absolute;
}
.toolbar .command:nth-of-type(1) {
  margin-left: 4px;
}
.toolbar .command {
  box-sizing: border-box;
  margin: 0px 6px;
  border-radius: 2px;
  padding-left: 4px;
  display: inline-block;
  border: 1px solid rgba(2, 2, 2, 0);
}
.toolbar .command:hover {
  cursor: pointer;
  border: 1px solid #e9e9e9;
}
.toolbar .disable {
  color: rgba(0, 0, 0, 0.25);
}
.toolbar .separator {
  margin: 4px;
  border-left: 1px solid #e9e9e9;
}
</style>
