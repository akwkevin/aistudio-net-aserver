<template>
  <ul>
    <li
      v-for="(item, index) in list"
      :key="index"
      class="getItem"
      :data-shape="item.shape"
      :data-type="item.type"
      :data-size="item.size"
      draggable
      @dragstart="handleDragstart"
      @dragend="handleDragEnd($event, item)"
    >
      <span class="pannel-type-icon" :style="{ background: 'url(' + item.image + ')' }"></span>
      {{ item.name }}
    </li>
  </ul>
</template>

<script>
import eventBus from '../utils/eventBus'
import okSvg from '../assets/icons/ok.svg'
import bgImg from '../assets/bg.jpg'
import leimu from '../assets/icons/leimu.svg'
import Shape from '../assets/icons/Shape.svg'
import startSvg from '../assets/icons/start.svg'
import endSvg from '../assets/icons/end.svg'
import wenhaoSvg from '../assets/icons/wenhao.svg'
import noneSvg from '../assets/icons/none.svg'
import noSvg from '../assets/icons/no.svg'
import jiahaoSvg from '../assets/icons/jiahao.svg'
import openSvg from '../assets/icons/open.svg'
import closeSvg from '../assets/icons/close.svg'

export default {
  data () {
    return {
      page: null,
      command: null,
      offsetX: 0,
      offsetY: 0,
      list: [
        // {
        //   name: '测试节点',
        //   label: '测试节点',
        //   size: '170*34',
        //   type: 'node',
        //   x: 0,
        //   y: 0,
        //   shape: 'customNode',
        //   color: '#1890ff',
        //   image: Shape,
        //   stateImage: okSvg,
        //   inPoints: [[0, 0.5]],
        //   outPoints: [[1, 0.5]]
        // },

        // {
        //   name: '背景图片节点',
        //   label: '背景图片节点',
        //   size: '170*34',
        //   type: 'node',
        //   x: 0,
        //   y: 0,
        //   shape: 'customNode',
        //   color: '#1890ff',
        //   image: Shape,
        //   stateImage: okSvg,
        //   backImage: bgImg,
        //   inPoints: [[0, 0.5]],
        //   outPoints: [[1, 0.5]]
        // },
        // {
        //   name: '双输出节点',
        //   label: '双输出节点',
        //   size: '170*34',
        //   type: 'node',
        //   x: 0,
        //   y: 0,
        //   shape: 'customNode',
        //   color: '#1890ff',
        //   image: Shape,
        //   stateImage: okSvg,
        //   inPoints: [[0, 0.5]],
        //   outPoints: [[1, 0.4], [1, 0.6]]
        // },
        // {
        //   name: '大型节点',
        //   label: '大型节点',
        //   size: '340*34',
        //   type: 'node',
        //   x: 0,
        //   y: 0,
        //   shape: 'customNode',
        //   color: '#1890ff',
        //   image: Shape,
        //   stateImage: okSvg,
        //   inPoints: [[0, 0.5]],
        //   outPoints: [[1, 0.5]]
        // },
        // {
        //   name: '动画开始节点',
        //   label: '动画开始',
        //   size: '170*34',
        //   type: 'node',
        //   x: 0,
        //   y: 0,
        //   shape: 'customNode',
        //   color: '#1890ff',
        //   image: Shape,
        //   stateImage: okSvg,
        //   inPoints: [[0, 0.5]],
        //   outPoints: [[1, 0.5]],
        //   isDoingStart: true
        // },
        // {
        //   name: '动画结束节点',
        //   label: '动画结束',
        //   size: '170*34',
        //   type: 'node',
        //   x: 0,
        //   y: 0,
        //   shape: 'customNode',
        //   color: '#1890ff',
        //   image: Shape,
        //   stateImage: okSvg,
        //   inPoints: [[0, 0.5]],
        //   outPoints: [[1, 0.5]],
        //   isDoingEnd: true
        // },
        {
          name: '开始节点',
          label: '开始',
          size: '170*34',
          type: 'node',
          x: 0,
          y: 0,
          shape: 'customNode',
          color: '#1890ff',
          image: Shape,
          stateImage: startSvg,
          inPoints: [[0, 0.5]],
          outPoints: [[1, 0.5]]
        },
        {
          name: '结束节点',
          label: '结束',
          size: '170*34',
          type: 'node',
          x: 0,
          y: 0,
          shape: 'customNode',
          color: '#1890ff',
          image: Shape,
          stateImage: endSvg,
          inPoints: [[0, 0.5]],
          outPoints: [[1, 0.5]]
        },
        {
          name: '中间节点',
          label: '节点',
          size: '170*34',
          type: 'node',
          x: 0,
          y: 0,
          shape: 'customNode',
          color: '#1890ff',
          image: Shape,
          stateImage: jiahaoSvg,
          inPoints: [[0, 0.5]],
          outPoints: [[1, 0.5]]
        },
        {
          name: '条件节点',
          label: '条件',
          size: '170*34',
          type: 'node',
          x: 0,
          y: 0,
          shape: 'customNode',
          color: '#1890ff',
          image: Shape,
          stateImage: wenhaoSvg,
          inPoints: [[0, 0.5]],
          outPoints: [
            [1, 0.4],
            [1, 0.6]
          ]
        },
        {
          name: '并行开始节点',
          label: '并行开始',
          size: '170*34',
          type: 'node',
          x: 0,
          y: 0,
          shape: 'customNode',
          color: '#1890ff',
          image: Shape,
          stateImage: openSvg,
          inPoints: [[0, 0.5]],
          outPoints: [
            [1, 0.4],
            [1, 0.6]
          ]
        },
        {
          name: '并行结束节点',
          label: '并行结束',
          size: '170*34',
          type: 'node',
          x: 0,
          y: 0,
          shape: 'customNode',
          color: '#1890ff',
          image: Shape,
          stateImage: closeSvg,
          inPoints: [
            [0, 0.4],
            [0, 0.6]
          ],
          outPoints: [[1, 0.5]]
        }
      ]
    }
  },
  created () {
    this.bindEvent()
  },
  beforeDestroy () {
    eventBus.$off('afterAddPage')
  },
  methods: {
    handleDragstart (e) {
      this.offsetX = e.offsetX
      this.offsetY = e.offsetY
    },
    handleDragEnd (e, item) {
      const data = {}
      Object.assign(data, item)
      data.offsetX = this.offsetX
      data.offsetY = this.offsetY
      if (this.page) {
        const graph = this.page.graph
        // const size = e.target.dataset.size.split("*");
        const xy = graph.getPointByClient(e.x, e.y)
        data.x = xy.x
        data.y = xy.y
        data.size = item.size.split('*')
        data.type = 'node'
        this.command.executeCommand('add', [data])
      }
    },
    bindEvent () {
      eventBus.$on('afterAddPage', page => {
        this.page = page
        this.command = page.command
      })
    }
  }
}
</script>

<style scoped>
.itempannel {
  height: 100%;
  position: absolute;
  left: 0px;
  z-index: 2;
  background: #f7f9fb;
  width: 200px;
  padding-top: 8px;
  border-right: 1px solid #e6e9ed;
}
.itempannel ul {
  padding: 0px;
  padding-left: 16px;
}
.itempannel li {
  color: rgba(0, 0, 0, 0.65);
  border-radius: 4px;
  width: 160px;
  height: 28px;
  line-height: 26px;
  padding-left: 8px;
  border: 1px solid rgba(0, 0, 0, 0);
  list-style-type: none;
}
.itempannel li:hover {
  background: white;
  border: 1px solid #ced4d9;
  cursor: move;
}

.itempannel .pannel-type-icon {
  width: 16px;
  height: 16px;
  display: inline-block;
  vertical-align: middle;
  margin-right: 8px;
}
</style>
