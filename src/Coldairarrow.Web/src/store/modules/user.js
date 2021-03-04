import { Axios } from '@/utils/plugin/axios-plugin'
import TokenCache from '@/utils/cache/TokenCache'
import Vue from 'vue'

const user = {
  state: {
    name: '',
    welcome: '',
    roles: [],
    info: {},
    inited: false,
    alluser: [],
    alluserinited: false,
    permissions: [],
    allrole: [],
    allroleinited: false
  },

  mutations: {
    SET_NAME: (state, { name, welcome }) => {
      state.name = name
      state.welcome = welcome
    },
    SET_AVATAR: (state, avatar) => {
      state.info.Avatar = avatar
    },
    SET_ROLES: (state, roles) => {
      state.roles = roles
    },
    SET_INFO: (state, info) => {
      state.info = info
    },
    SET_INITED: (state, inited) => {
      state.inited = inited
    },
    SET_ALLUser: (state, alluser) => {
      state.alluser = alluser
    },
    SET_AllUserINITED: (state, alluserinited) => {
      state.alluserinited = alluserinited
    },
    SET_PERMISSIONS: (state, permissions) => {
      state.permissions = permissions
    },
    SET_ALLRole: (state, allrole) => {
      state.allrole = allrole
    },
    SET_AllRoleINITED: (state, allroleinited) => {
      state.allroleinited = allroleinited
    }
  },

  actions: {
    // 登录
    Login ({ commit }, userInfo) {
      return new Promise((resolve, reject) => {
        Axios.post('/Base_Manage/Home/SubmitLogin', userInfo).then(resJson => {
          if (resJson.Success) {
            TokenCache.setToken(resJson.Data)

            resolve(resJson)
          } else {
            reject(resJson.Msg)
          }
        })
      })
    },

    // 获取用户信息
    GetInfo ({ state, commit }) {
      return new Promise((resolve, reject) => {
        if (state.inited === true) {
          resolve()
        }

        Axios.post('/Base_Manage/Home/GetOperatorInfo').then(resJson => {
          if (resJson.Success) {
            commit('SET_INFO', resJson.Data.UserInfo)
            commit('SET_PERMISSIONS', resJson.Data.Permissions)
            commit('SET_INITED', true)
            Vue.prototype.socketApi.initWebSocket(resJson.Data.UserInfo.UserName, resJson.Data.UserInfo.Id)
            resolve()
          } else {
            reject(resJson.Msg)
          }
        })
      })
    },

    GetAllUser ({ state, commit }) {
      return new Promise((resolve, reject) => {
        if (state.alluserinited.inited) resolve(state.alluser)
        Axios.post('/Base_Manage/Base_User/GetOptionList', { q: '' }).then(resJson => {
          if (resJson.Success) {
            commit('SET_ALLUser', resJson.Data)
            commit('SET_AllUserINITED', true)
            resolve(resJson.Data)
          } else {
            // eslint-disable-next-line prefer-promise-reject-errors
            reject([])
          }
        })
      })
    },

    ClearAllUser ({ state, commit }) {
      commit('SET_AllUserINITED', false)
    },

    GetAllRole ({ state, commit }) {
      return new Promise((resolve, reject) => {
        if (state.allroleinited.inited) resolve(state.allrole)
        Axios.post('/Base_Manage/Base_Role/GetOptionList', { q: '' }).then(resJson => {
          if (resJson.Success) {
            commit('SET_ALLRole', resJson.Data)
            commit('SET_AllRoleINITED', true)
            resolve(resJson.Data)
          } else {
            // eslint-disable-next-line prefer-promise-reject-errors
            reject([])
          }
        })
      })
    },

    ClearAllRole ({ state, commit }) {
      commit('SET_AllRoleINITED', false)
    },

    // 登出
    Logout ({ state, commit }) {
      return new Promise(resolve => {
        commit('SET_INITED', false)
        commit('SET_AllUserINITED', false)
        commit('SET_AllRoleINITED', false)
        commit('SET_TOKEN', '')
        commit('SET_PERMISSIONS', [])
        commit('SET_INFO', {})
        TokenCache.deleteToken()
        Vue.prototype.socketApi.closeWebSocket()
        resolve()
      })
    }
  }
}

export default user
