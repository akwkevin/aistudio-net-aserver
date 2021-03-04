<template slot="footer">
  <div class="web__msg" @keyup.enter="handleOK">
    <div class="web__msg-input">
      <a-radio-group style="float: left" @change="onModeChange" v-model="selectMode">
        <a-radio-button :value="1">今天</a-radio-button>
        <a-radio-button :value="2">最近一周</a-radio-button>
        <a-radio-button :value="3">最近一个月</a-radio-button>
        <a-radio-button :value="4">最近三个月</a-radio-button>
      </a-radio-group>
      <div style="float: right">
        <a-date-picker
          :disabledDate="disabledStartDate"
          format="YYYY-MM-DD"
          mode="date"
          style="width:120px;"
          v-model="startValue"
          placeholder="Start"
          @openChange="handleStartOpenChange"
        />
        <a-date-picker
          :disabledDate="disabledEndDate"
          format="YYYY-MM-DD"
          mode="date"
          style="width:120px;margin-left:10px"
          placeholder="End"
          v-model="endValue"
          :open="endOpen"
          @openChange="handleEndOpenChange"
        />
      </div>
    </div>
    <div class="web__msg-menu">
      <a-button
        class="web__msg-submit"
        key="submit"
        type="primary"
        size="small"
        @click="handleOK"
      >确定</a-button>
    </div>
  </div>
</template>

<script>
import moment from 'moment'
import { mapGetters } from 'vuex'

export default {
  data () {
    return {
      startValue: null,
      endValue: null,
      endOpen: false,
      selectMode: 1
    }
  },
  mounted () {
    this.onModeChange()
    this.handleOK()
  },
  methods: {
    handleOK () {
      this.$emit('history', this.startValue, this.endValue)
    },
    disabledStartDate (startValue) {
      const endValue = this.endValue
      if (!startValue || !endValue) {
        return false
      }
      return startValue.valueOf() > endValue.valueOf()
    },
    disabledEndDate (endValue) {
      const startValue = this.startValue
      if (!endValue || !startValue) {
        return false
      }
      return startValue.valueOf() >= endValue.valueOf()
    },
    handleStartOpenChange (open) {
      if (!open) {
        this.endOpen = true
      }
    },
    handleEndOpenChange (open) {
      this.endOpen = open
    },
    onModeChange () {
      if (this.selectMode === 1) {
        this.startValue = moment(new Date(new Date(new Date().toLocaleDateString()).getTime()))
        this.endValue = moment(new Date())
      } else if (this.selectMode === 2) {
        this.startValue = moment(new Date(new Date().getTime() - 7 * 24 * 60 * 60 * 1000))
        this.endValue = moment(new Date())
      } else if (this.selectMode === 3) {
        this.startValue = moment(new Date(new Date().getTime() - 30 * 24 * 60 * 60 * 1000))
        this.endValue = moment(new Date())
      } else if (this.selectMode === 4) {
        this.startValue = moment(new Date(new Date().getTime() - 90 * 24 * 60 * 60 * 1000))
        this.endValue = moment(new Date())
      }
    }
  }
}
</script>

/script>
<style scoped>
.web__msg {
  padding: 0 10px;
  height: auto;
  overflow: hidden;
}

.web__msg-input {
  display: block;
  width: 100%;
  height: 60px;
  background-color: #fff;
  float: left;
}
.web__msg-menu {
  text-align: right;
}
.web__msg-submit {
  display: inline-block;
  outline: none;
  cursor: pointer;
  text-align: center;
}
</style>
