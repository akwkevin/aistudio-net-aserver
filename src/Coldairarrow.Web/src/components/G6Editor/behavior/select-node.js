import Util from '@antv/g6/lib/util'
import eventBus from '../utils/eventBus'
export default {
  getDefaultCfg: function () {
    return {
      multiple: true,
      keyCode: 16
    }
  },
  getEvents: function () {
    return {
      'node:click': 'onClick',
      'canvas:click': 'onCanvasClick',
      'canvas:mouseover': 'onCanvasMouseover',
      keyup: 'onKeyUp',
      keydown: 'onKeyDown'
    }
  },
  onClick: function (e) {
    const self = this
    const item = e.item
    const graph = self.graph
    const autoPaint = graph.get('autoPaint')
    graph.setAutoPaint(false)
    const selectedEdges = graph.findAllByState('edge', 'selected')

    // Util.each(selectedEdges, (edge) => {
    //   graph.setItemState(edge, "selected", false);
    // });
    selectedEdges.forEach((edge) => {
      graph.setItemState(edge, 'selected', false)
    })
    if (!self.keydown || !self.multiple) {
      const selected = graph.findAllByState('node', 'selected')
      // Util.each(selected, (node) => {
      //   if (node !== item) {
      //     graph.setItemState(node, "selected", false);
      //   }
      // });
      selected.forEach((node) => {
        if (node !== item) {
          graph.setItemState(node, 'selected', false)
        }
      })
    }
    if (item.hasState('selected')) {
      if (self.shouldUpdate.call(self, e)) {
        graph.setItemState(item, 'selected', false)
      }

      eventBus.$emit('nodeselectchange', { target: item, select: false })
    } else {
      if (self.shouldUpdate.call(self, e)) {
        graph.setItemState(item, 'selected', true)
      }
      eventBus.$emit('nodeselectchange', { target: item, select: true })
    }
    graph.setAutoPaint(autoPaint)
    graph.paint()
  },
  onCanvasClick: function () {
    const graph = this.graph
    const autoPaint = graph.get('autoPaint')
    graph.setAutoPaint(false)
    const selected = graph.findAllByState('node', 'selected')
    // Util.each(selected, (node) => {
    //   graph.setItemState(node, "selected", false);
    //   eventBus.$emit("nodeselectchange", { target: node, select: false });
    // });
    selected.forEach((node) => {
      graph.setItemState(node, 'selected', false)
      eventBus.$emit('nodeselectchange', { target: node, select: false })
    })

    const selectedEdges = graph.findAllByState('edge', 'selected')
    // Util.each(selectedEdges, (edge) => {
    //   graph.setItemState(edge, "selected", false);
    //   eventBus.$emit("nodeselectchange", { target: edge, select: false });
    // });
    selectedEdges.forEach((edge) => {
      graph.setItemState(edge, 'selected', false)
      eventBus.$emit('nodeselectchange', { target: edge, select: false })
    })

    graph.paint()
    graph.setAutoPaint(autoPaint)
  },
  onCanvasMouseover: function () {
    const graph = this.graph
    graph.paint()
  },
  onKeyDown: function (e) {
    const code = e.keyCode || e.which
    if (code === this.keyCode) {
      this.keydown = true
    } else {
      this.keydown = false
    }
  },
  onKeyUp: function () {
    this.keydown = false
  }
}
