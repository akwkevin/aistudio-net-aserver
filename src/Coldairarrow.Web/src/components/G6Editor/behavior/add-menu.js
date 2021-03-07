import eventBus from '../utils/eventBus'
export default {
  getEvents: function () {
    return {
      'node:contextmenu': 'onContextmenu',
      mousedown: 'onMousedown',
      'canvas:click': 'onCanvasClick'
    }
  },
  onContextmenu: function (e) {
    eventBus.$emit('contextmenuClick', e)
  },
  onMousedown: function (e) {
    eventBus.$emit('mousedown', e)
  },
  onCanvasClick: function (e) {
    eventBus.$emit('canvasClick', e)
  }
}
