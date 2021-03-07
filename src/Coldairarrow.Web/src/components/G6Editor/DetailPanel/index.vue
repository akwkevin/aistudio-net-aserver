<template>
  <div class="detailpannel">
    <div>
      <div v-if="status == 'node-selected'" class="pannel" id="node_detailpannel">
        <div class="pannel-title">模型详情</div>
        <div class="block-container">
          <a-row :gutter="2">
            <a-col :span="8">名称</a-col>
            <a-col :span="16">
              <a-input :disabled="mode !== 'edit'" v-model="node.label" @change="handleChangeName" />
            </a-col>
            <a-col v-if="node.name === '中间节点'" :span="8">用户名</a-col>
            <a-col v-if="node.name === '中间节点'" :span="16">
              <a-select
                :disabled="mode !== 'edit'"
                show-search
                mode="multiple"
                option-filter-prop="children"
                :filter-option="filterOption"
                style="width: 100%"
                v-model="node.UserIds"
              >
                <a-select-option v-for="d in users" :key="d.value" >{{ d.text }}</a-select-option>
              </a-select>
            </a-col>
            <a-col v-if="node.name === '中间节点'" :span="8">角色名</a-col>
            <a-col v-if="node.name === '中间节点'" :span="16">
              <a-select
                :disabled="mode !== 'edit'"
                show-search
                mode="multiple"
                option-filter-prop="children"
                :filter-option="filterOption"
                style="width: 100%"
                v-model="node.RoleIds"
              >
                <a-select-option v-for="d in roles" :key="d.value" >{{ d.text }}</a-select-option>
              </a-select>
            </a-col>
            <a-col v-if="node.name === '中间节点'" :span="8">or/and</a-col>
            <a-col v-if="node.name === '中间节点'" :span="16">
              <a-select :disabled="mode !== 'edit'" style="width: 100%" v-model="node.ActType" default-value="or">
                <a-select-option value="or">or</a-select-option>
                <a-select-option value="and">and</a-select-option>
              </a-select>
            </a-col>
          </a-row>
        </div>
      </div>
      <div v-if="status === 'canvas-selected'" class="pannel" id="canvas_detailpannel">
        <div class="pannel-title">画布</div>
        <div class="block-container">
          <a-checkbox v-model="showGrid" @change="changeGridState">网格对齐</a-checkbox>
        </div>
      </div>
      <!--我添加的-->
      <div v-if="status === 'edge-selected'" id="edge_detailpannel" class="pannel">
        <div class="pannel-title">连线</div>
        <div class="block-container">
          <a-col :span="8">内容</a-col>
          <a-col :span="16">
            <a-input v-model="edge.label" :disabled="mode !== 'edit'" @change="handleChange" />
          </a-col>
          <a-col :span="8">文字颜色</a-col>
          <a-col :span="16">
            <el-color-picker v-model="edge.textColor" :disabled="mode !== 'edit'" @change="handleChangeColor" />
          </a-col>
        </div>
      </div>
      <!-- <div v-if="status==='group-selected'" class="pannel" id="node_detailpannel">
        <div class="pannel-title">群组详情</div>
        <div class="block-container">
          <div class="p">
            名称：
            <a-input v-model="name" />
          </div>
          <div class="p">
            任意属性：
            <a-input v-model="color" />
          </div>
        </div>
      </div>
      -->
    </div>
  </div>
</template>

<script>
import eventBus from '../utils/eventBus'
import Grid from '@antv/g6/lib/plugins/grid'
export default {
  props: {
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
      status: 'canvas-selected',
      showGrid: false,
      page: {},
      graph: {},
      item: {},
      node: {},
      // 【我添加的】
      edge: {},
      grid: null,
      // 【我添加的】
      textColor: 'rgba(19, 206, 102, 0.8)'
    }
  },
  created () {
    this.init()
    this.bindEvent()
  },
  beforeDestroy () {
    eventBus.$off('afterAddPage')
    eventBus.$off('nodeselectchange')
  },
  methods: {
    init () {},
    bindEvent () {
      const self = this
      eventBus.$on('afterAddPage', page => {
        self.page = page
        self.graph = self.page.graph
        eventBus.$on('nodeselectchange', item => {
          if (item.select === true && item.target.getType() === 'node') {
            self.status = 'node-selected'
            self.item = item.target
            self.node = item.target.getModel()
          } // 【我添加的】
          else if (item.select === true && item.target.getType() === 'edge') {
            self.status = 'edge-selected'
            self.item = item.target
            self.edge = item.target.getModel()
          } else {
            self.status = 'canvas-selected'
            self.item = null
            self.node = null
          }
        })
      })
    },
    handleChangeName (e) {
      const model = {
        label: e.target.value
      }

      this.graph.update(this.item, model)
    },
    changeGridState (value) {
      if (value) {
        this.grid = new Grid()
        this.graph.addPlugin(this.grid)
      } else {
        this.graph.removePlugin(this.grid)
      }
    },
    // 【我添加的】
    handleChange (e) {
      const model = {
        label: e.target.value
      }
      this.graph.update(this.item, model)
    },
    handleChangeColor (e) {
      const model = {
        textColor: e
      }
      this.graph.update(this.item, model)
    },
    filterOption (input, option) {
      return option.componentOptions.children[0].text.toLowerCase().indexOf(input.toLowerCase()) >= 0
    }
  }
}
</script>

<style scoped>
.detailpannel {
  height: 100%;
  position: absolute;
  right: 0px;
  z-index: 2;
  background: #f7f9fb;
  width: 200px;
  border-left: 1px solid #e6e9ed;
}
.detailpannel .block-container {
  padding: 16px 8px;
}
.block-container .el-col {
  height: 28px;
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}
.pannel-title {
  height: 32px;
  border-top: 1px solid #dce3e8;
  border-bottom: 1px solid #dce3e8;
  background: #ebeef2;
  color: #000;
  line-height: 28px;
  padding-left: 12px;
}
</style>
