import Util from '@antv/g6/lib/util'
import eventBus from '../utils/eventBus'
export default {
  getEvents: function () {
    return {
      'edge:mouseover': 'onMouseover',
      'edge:mouseleave': 'onMouseleave',
      'edge:click': 'onClick'
    }
  },
  onMouseover: function (e) {
    const self = this
    const item = e.item
    const graph = self.graph
    if (item.hasState('selected')) {
      return
    } else {
      if (self.shouldUpdate.call(self, e)) {
        graph.setItemState(item, 'hover', true)
      }
    }
    graph.paint()
  },
  onMouseleave: function (e) {
    const self = this
    const item = e.item
    const graph = self.graph
    const group = item.getContainer()
    group.find((g) => {
      if (
        (typeof g.attrs.isInPoint !== 'undefined' && g.attrs.isInPoint) ||
        (typeof g.attrs.isOutPoint !== 'undefined' && g.attrs.isOutPoint)
      ) {
        g.attr('fill', '#fff')
      }
    })
    if (self.shouldUpdate.call(self, e)) {
      if (!item.hasState('selected')) graph.setItemState(item, 'hover', false)
    }
    graph.paint()
  },
  onClick: function (e) {
    const self = this
    const item = e.item
    const graph = self.graph
    const autoPaint = graph.get('autoPaint')
    graph.setAutoPaint(false)
    const selectedNodes = graph.findAllByState('node', 'selected')
    // Util.each(selectedNodes, (node) => {
    //   graph.setItemState(node, "selected", false);
    // });
    selectedNodes.forEach((node) => {
      graph.setItemState(node, 'selected', false)
    })
    if (!self.keydown || !self.multiple) {
      const selected = graph.findAllByState('edge', 'selected')
      // Util.each(selected, (edge) => {
      //   if (edge !== item) {
      //     graph.setItemState(edge, "selected", false);
      //   }
      // });
      selected.forEach((edge) => {
        if (edge !== item) {
          graph.setItemState(edge, 'selected', false)
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
  }
}
