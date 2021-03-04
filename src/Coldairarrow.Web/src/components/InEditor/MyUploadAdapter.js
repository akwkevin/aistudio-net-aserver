import TokenCache from '@/utils/cache/TokenCache'
import { Axios } from '@/utils/plugin/axios-plugin'

/**
 * 自定义上传图片插件
 */
class MyUploadAdapter {
  constructor (loader) {
    this.loader = loader
  }

  async upload () {
    const param = new FormData()
    param.append('file', await this.loader.file)

    var config = {
      headers: { 'Content-Type': 'multipart/form-data' }
    }
    if (TokenCache.getToken()) {
      config.headers.Authorization = 'Bearer ' + TokenCache.getToken()
    }

    var url = `/Base_Manage/Upload/UploadFileByForm`
    const res = await Axios.post(url, param, config)

    return {
      default: res.url
    }
    // 方法返回数据格式： {default: "url"}
  }
}

export default MyUploadAdapter
