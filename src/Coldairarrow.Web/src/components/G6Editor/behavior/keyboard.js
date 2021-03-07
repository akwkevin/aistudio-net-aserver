import eventBus from '../utils/eventBus'
export default {
  getDefaultCfg: function () {
    return {
      backKeyCode: 8,
      deleteKeyCode: 46
    }
  },
  getEvents: function () {
    return {
      keyup: 'onKeyUp',
      keydown: 'onKeyDown'
    }
  },

  onKeyDown: function (e) {
    const code = e.keyCode || e.which
    switch (code) {
      case this.deleteKeyCode:
      case this.backKeyCode:
        // eventBus.$emit('deleteItem')
        break
    }
  },
  onKeyUp: function () {
    this.keydown = false
  }
}
