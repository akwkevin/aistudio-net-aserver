<template>
  <div class="user-wrapper">
    <div class="content-box">
      <!-- <a href="https://pro.loacg.com/docs/getting-started" target="_blank">
        <span class="action">
          <a-icon type="question-circle-o"></a-icon>
        </span>
      </a>-->
      <notice-icon class="action" />
      <a-dropdown>
        <span class="action ant-dropdown-link user-dropdown-menu">
          <a-avatar class="avatar" :src="avatar|AvatarFilter" />
          <span>{{ nickname }}</span>
        </span>
        <a-menu slot="overlay" class="user-dropdown-menu-wrapper">
          <a-menu-item key="0">
            <router-link :to="{ path: '/account/center/Index' }">
              <a-icon type="user" />
              <span>个人中心</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="1">
            <router-link :to="{ path: '/account/settings/Index' }">
              <a-icon type="setting" />
              <span>个人设置</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="2">
            <a href="javascript:;" @click="handleChangePwd()">
              <a-icon type="lock" />
              <span>修改密码</span>
            </a>
            <change-pwd-form ref="changePwd"></change-pwd-form>
          </a-menu-item>
          <a-menu-divider />
          <a-menu-item key="3">
            <a href="javascript:;" @click="handleLogout()">
              <a-icon type="logout" />
              <span>退出登录</span>
            </a>
          </a-menu-item>
        </a-menu>
      </a-dropdown>
    </div>
  </div>
</template>

<script>
import NoticeIcon from '@/components/NoticeIcon'
import { mapActions, mapGetters } from 'vuex'
import ChangePwdForm from './ChangePwdForm'

export default {
  name: 'UserMenu',
  components: {
    NoticeIcon,
    ChangePwdForm
  },
  computed: {
    ...mapGetters(['nickname', 'avatar'])
  },
  methods: {
    ...mapActions(['Logout']),
    handleLogout () {
      const that = this

      this.$confirm({
        title: '提示',
        content: '真的要注销登录吗 ?',
        onOk () {
          that.Logout().then(
            () => location.reload()
            // that.$router.push({ path: '/user/login' })
          )
        }
      })
    },
    handleChangePwd () {
      this.$refs.changePwd.open()
    }
  }
}
</script>
