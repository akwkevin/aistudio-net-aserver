using Microsoft.Extensions.Logging;

namespace Coldairarrow.Util
{
    /// <summary>
    /// 系统日志类型
    /// </summary>
    public enum UserLogType
    {
        系统异常,
        系统用户管理,
        系统角色管理,
        接口密钥管理,
        部门管理,
        系统任务,  //aistudio
        系统任务执行,  //aistudio
        工作流程,  //aistudio
        WebSocket,  //aistudio
    }

    //aistudio
    public static class UserLogTypeHelper
    {
        public static EventId ToEventId(this UserLogType logType)
        {
            return new EventId((int)logType, logType.ToString());
        }
    }
}
