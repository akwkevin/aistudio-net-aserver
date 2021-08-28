using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Entity;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Service.Quartz
{
    public class ResetDataJob : IJob
    {
        IBase_UserBusiness userBusiness { get { return ServiceLocator.Instance.GetRequiredService<IBase_UserBusiness>(); } }
        IBase_ActionBusiness actionBusiness { get { return ServiceLocator.Instance.GetRequiredService<IBase_ActionBusiness>(); } }

        public async Task Execute(IJobExecutionContext context)
        {
            var adminUser = await userBusiness.FirstOrDefaultAsync(p => p.UserName == "Admin");
            if (adminUser != null)
            {
                adminUser.Password = "Admin".ToMD5String();
                var result = await userBusiness.UpdateAsync(adminUser);
            }

            await actionBusiness.DeleteAllAsync();
            var actionBusinesscount = await actionBusiness.GetIQueryable().CountAsync();
            if (actionBusinesscount == 0)
            {
                List<Base_Action> actions = new List<Base_Action>()
                    {
                         new Base_Action(){ Id="1178957405992521728",Deleted = false, ParentId=null,                  Type = ActionType.菜单, Name="系统管理", Url=null,                               Value=null,                    NeedAction=true,    Icon="setting",        Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1178957553778823168",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="权限管理", Url="/Base_Manage/Base_Action/List",    Value=null,                    NeedAction=true,    Icon="lock",           Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1179018395304071168",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="密钥管理", Url="/Base_Manage/Base_AppSecret/List", Value=null,                    NeedAction=true,    Icon="key",            Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1182652266117599232",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="用户管理", Url="/Base_Manage/Base_User/List",      Value=null,                    NeedAction=true,    Icon="user-add",       Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1182652367447789568",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="角色管理", Url="/Base_Manage/Base_Role/List",      Value=null,                    NeedAction=true,    Icon="safety",         Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1182652433302556672",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="部门管理", Url="/Base_Manage/Base_Department/List",Value=null,                    NeedAction=true,    Icon="usergroup-add",  Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1182652599069839360",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="操作日志", Url="/Base_Manage/Base_UserLog/List",   Value=null,                    NeedAction=true,    Icon="file-search",    Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1182652599069839361",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="任务管理", Url="/Quartz_Manage/Quartz_Task/List",  Value=null,                    NeedAction=true,    Icon="calendar",       Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188800845714558976",Deleted = false, ParentId="1182652266117599232", Type = ActionType.权限, Name="增",       Url=null,                               Value="Base_User.Add",         NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188800845714558977",Deleted = false, ParentId="1182652266117599232", Type = ActionType.权限, Name="改",       Url=null,                               Value="Base_User.Edit",        NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188800845714558978",Deleted = false, ParentId="1182652266117599232", Type = ActionType.权限, Name="删",       Url=null,                               Value="Base_User.Delete",      NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801057778569216",Deleted = false, ParentId="1182652367447789568", Type = ActionType.权限, Name="增",       Url=null,                               Value="Base_Role.Add",         NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801057778569217",Deleted = false, ParentId="1182652367447789568", Type = ActionType.权限, Name="改",       Url=null,                               Value="Base_Role.Edit",        NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801057778569218",Deleted = false, ParentId="1182652367447789568", Type = ActionType.权限, Name="删",       Url=null,                               Value="Base_Role.Delete",      NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801109783744512",Deleted = false, ParentId="1182652433302556672", Type = ActionType.权限, Name="增",       Url=null,                               Value="Base_Department.Add",   NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801109783744513",Deleted = false, ParentId="1182652433302556672", Type = ActionType.权限, Name="改",       Url=null,                               Value="Base_Department.Edit",  NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801109783744514",Deleted = false, ParentId="1182652433302556672", Type = ActionType.权限, Name="删",       Url=null,                               Value="Base_Department.Delete",NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801273885888512",Deleted = false, ParentId="1179018395304071168", Type = ActionType.权限, Name="增",       Url=null,                               Value="Base_AppSecret.Add",    NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801273885888513",Deleted = false, ParentId="1179018395304071168", Type = ActionType.权限, Name="改",       Url=null,                               Value="Base_AppSecret.Edit",   NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801273885888514",Deleted = false, ParentId="1179018395304071168", Type = ActionType.权限, Name="删",       Url=null,                               Value="Base_AppSecret.Delete", NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801341661646848",Deleted = false, ParentId="1178957553778823168", Type = ActionType.权限, Name="增",       Url=null,                               Value="Base_Action.Add",       NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801341661646849",Deleted = false, ParentId="1178957553778823168", Type = ActionType.权限, Name="改",       Url=null,                               Value="Base_Action.Edit",      NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1188801341661646850",Deleted = false, ParentId="1178957553778823168", Type = ActionType.权限, Name="删",       Url=null,                               Value="Base_Action.Delete",    NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758848",Deleted = false, ParentId=null,                  Type = ActionType.菜单, Name="首页",     Url=null,                               Value=null,                    NeedAction=true,    Icon="home",           Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158630615027712",Deleted = false, ParentId="1193158266167758848", Type = ActionType.页面, Name="框架介绍", Url="/Home/Introduce",                  Value=null,                    NeedAction=false,   Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158780011941888",Deleted = false, ParentId="1193158266167758848", Type = ActionType.页面, Name="运营统计", Url="/Home/Statis",                     Value=null,                    NeedAction=false,   Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158780011941889",Deleted = false, ParentId="1193158266167758848", Type = ActionType.页面, Name="我的控制台", Url="/Home/UserConsole",              Value=null,                    NeedAction=false,   Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158780011941890",Deleted = false, ParentId="1193158266167758848", Type = ActionType.页面, Name="3D展台", Url="/Home/_3DShowcase",                     Value=null,                    NeedAction=false,   Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758849",Deleted = false, ParentId=null,                  Type = ActionType.菜单, Name="消息中心", Url=null,                               Value=null,                    NeedAction=true,    Icon="notification",   Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758867",Deleted = false, ParentId="1193158266167758849", Type = ActionType.页面, Name="站内消息", Url="/D_Manage/D_UserMessage/List",     Value=null,                    NeedAction=false,   Icon="message",        Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758868",Deleted = false, ParentId="1193158266167758849", Type = ActionType.页面, Name="站内信",   Url="/D_Manage/D_UserMail/Index",       Value=null,                    NeedAction=false,   Icon="mail",          Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758869",Deleted = false, ParentId="1193158266167758849", Type = ActionType.页面, Name="通告",     Url="/D_Manage/D_Notice/List",          Value=null,                    NeedAction=false,   Icon="sound",           Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758850",Deleted = false, ParentId=null,                  Type = ActionType.菜单, Name="流程中心", Url=null,                               Value=null,                    NeedAction=true,    Icon="clock-circle",   Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758851",Deleted = false, ParentId="1193158266167758850", Type = ActionType.页面, Name="流程管理", Url="/OA_Manage/OA_DefForm/List",       Value=null,                    NeedAction=true,    Icon="interaction",    Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758852",Deleted = false, ParentId="1193158266167758851", Type = ActionType.权限, Name="增",       Url=null,                               Value="OA_DefForm.Add",        NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758853",Deleted = false, ParentId="1193158266167758851", Type = ActionType.权限, Name="改",       Url=null,                               Value="OA_DefForm.Edit",       NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758854",Deleted = false, ParentId="1193158266167758851", Type = ActionType.权限, Name="删",       Url=null,                               Value="OA_DefForm.Delete",     NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758855",Deleted = false, ParentId="1193158266167758850", Type = ActionType.页面, Name="发起流程", Url="/OA_Manage/OA_DefForm/TreeList",   Value=null,                    NeedAction=true,    Icon="file-add",       Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758856",Deleted = false, ParentId="1193158266167758850", Type = ActionType.页面, Name="我的流程", Url="/OA_Manage/OA_UserForm/List",      Value=null,                    NeedAction=true,    Icon="file-done",      Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758860",Deleted = false, ParentId="1193158266167758856", Type = ActionType.权限, Name="增",       Url=null,                               Value="OA_UserForm.Add",       NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758861",Deleted = false, ParentId="1193158266167758856", Type = ActionType.权限, Name="改",       Url=null,                               Value="OA_UserForm.Edit",      NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758862",Deleted = false, ParentId="1193158266167758856", Type = ActionType.权限, Name="删",       Url=null,                               Value="OA_UserForm.Delete",    NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758863",Deleted = false, ParentId="1193158266167758850", Type = ActionType.页面, Name="表单管理", Url="/OA_Manage/OA_DefType/List",       Value=null,                    NeedAction=true,    Icon="form",           Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758864",Deleted = false, ParentId="1193158266167758863", Type = ActionType.权限, Name="增",       Url=null,                               Value="OA_DefType.Add",        NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758865",Deleted = false, ParentId="1193158266167758863", Type = ActionType.权限, Name="改",       Url=null,                               Value="OA_DefType.Edit",       NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758866",Deleted = false, ParentId="1193158266167758863", Type = ActionType.权限, Name="删",       Url=null,                               Value="OA_DefType.Delete",     NeedAction=true,    Icon=null,             Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758967",Deleted = false, ParentId=null,                  Type = ActionType.菜单, Name="个人页",   Url=null,                               Value=null,                    NeedAction=true,    Icon="user",           Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758968",Deleted = false, ParentId="1193158266167758967", Type = ActionType.页面, Name="个人中心", Url="/account/center/Index",            Value=null,                    NeedAction=false,   Icon="user",           Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1193158266167758969",Deleted = false, ParentId="1193158266167758967", Type = ActionType.页面, Name="个人设置", Url="/account/settings/Index",          Value=null,                    NeedAction=false,   Icon="user",           Sort=1, CreateTime=DateTime.Now}, };

                var result = await actionBusiness.InsertAsync(actions);
            }
        }
    }
}
