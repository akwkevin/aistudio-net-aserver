using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Business.OA_Manage;
using Coldairarrow.Business.Quartz_Manage;
using Coldairarrow.Entity;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Entity.OA_Manage;
using Coldairarrow.Entity.Quartz_Manage;
using Coldairarrow.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Coldairarrow.Api
{
    //astudio edit
    public class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            var logger = provider.GetRequiredService<ILogger<SeedData>>();

            var configuration = provider.GetRequiredService<IConfiguration>();
            var dbOptions = configuration.GetSection("Database:BaseDb").Get<DatabaseOptions>();

            var dbLinkBusiness = provider.GetRequiredService<IBase_DbLinkBusiness>();
            var baseDb = await dbLinkBusiness.FirstOrDefaultAsync(p => p.LinkName == "BaseDb");
            if (baseDb == null)
            {
                baseDb = new Base_DbLink()
                {
                    Id = IdHelper.GetId(),
                    LinkName = "BaseDb",
                    ConnectionStr = dbOptions.ConnectionString,
                    DbType = dbOptions.DatabaseType.ToString(),
                    CreateTime = DateTime.Now,
                };

                var result = await dbLinkBusiness.InsertAsync(baseDb);

                logger.LogTrace($"BaseDb {dbOptions.ConnectionString}-{dbOptions.DatabaseType.ToString()} created");
            }
            else
            {
                logger.LogTrace($"BaseDb {dbOptions.ConnectionString}-{dbOptions.DatabaseType.ToString()} already exists");
            }

            var roleBusiness = provider.GetRequiredService<IBase_RoleBusiness>();
            var admin = await roleBusiness.FirstOrDefaultAsync(p => p.RoleName == RoleTypes.部门管理员.ToString());
            if (admin == null)
            {
                admin = new Base_Role
                {
                    Id = IdHelper.GetId(),
                    RoleName = RoleTypes.部门管理员.ToString(),
                    CreateTime = DateTime.Now,
                };
                var result = roleBusiness.InsertAsync(admin).Result;
            }

            var superadmin = await roleBusiness.FirstOrDefaultAsync(p => p.RoleName == RoleTypes.超级管理员.ToString());
            if (superadmin == null)
            {
                superadmin = new Base_Role
                {
                    Id = IdHelper.GetId(),
                    RoleName = RoleTypes.超级管理员.ToString(),
                    CreateTime = DateTime.Now,
                };
                var result = roleBusiness.InsertAsync(superadmin).Result;
            }

            var departmentBussiness = provider.GetRequiredService<IBase_DepartmentBusiness>();
            var departmentCount = await departmentBussiness.GetIQueryable().CountAsync();
            if (departmentCount == 0)
            {
                List<Base_Department> departments = new List<Base_Department>()
                {
                    new Base_Department(){  Id="1", Name="Wpf控件公司", ParentId = null, CreateTime=DateTime.Now},
                    new Base_Department(){  Id="2", Name="UI部门", ParentId="1", CreateTime=DateTime.Now},
                    new Base_Department(){  Id="2_1", Name="UI子部门1", ParentId="2", CreateTime=DateTime.Now},
                    new Base_Department(){  Id="2_2", Name="UI子部门2", ParentId="2", CreateTime=DateTime.Now},
                    new Base_Department(){  Id="3", Name="C#部门", ParentId="1", CreateTime=DateTime.Now},
                    new Base_Department(){  Id="3_1", Name="C#子部门1", ParentId="3", CreateTime=DateTime.Now},
                    new Base_Department(){  Id="3_2", Name="C#子部门2", ParentId="3", CreateTime=DateTime.Now},
                };

                var result = await departmentBussiness.InsertAsync(departments);
                logger.LogTrace("departments created");
            }

            var userBusiness = provider.GetRequiredService<IBase_UserBusiness>();

            var adminUser = await userBusiness.FirstOrDefaultAsync(p => p.UserName == "Admin");
            if (adminUser == null)
            {
                adminUser = new Base_User
                {
                    Id = "Admin",
                    UserName = "Admin",
                    Password = "Admin".ToMD5String(),
                    DepartmentId = "1",
                    CreateTime = DateTime.Now,
                };
                var result = userBusiness.InsertAsync(adminUser).Result;
                await userBusiness.SetUserRoleAsync(adminUser.Id, new List<string> { superadmin.Id });

                logger.LogTrace("admin created");
            }

            //alice ,123456,
            var alice = await userBusiness.FirstOrDefaultAsync(p => p.UserName == "alice");
            if (alice == null)
            {
                alice = new Base_User
                {
                    Id = IdHelper.GetId(),
                    UserName = "alice",
                    Password = "123456".ToMD5String(),
                    DepartmentId = "2",
                    CreateTime = DateTime.Now,
                };
                var result = await userBusiness.InsertAsync(alice);

                await userBusiness.SetUserRoleAsync(alice.Id, new List<string> { admin.Id });

                logger.LogTrace("alice created");
            }

            //bob ,123456,
            var bob = await userBusiness.FirstOrDefaultAsync(p => p.UserName == "bob");
            if (bob == null)
            {
                bob = new Base_User
                {
                    Id = IdHelper.GetId(),
                    UserName = "bob",
                    Password = "123456".ToMD5String(),
                    DepartmentId = "3",
                    CreateTime = DateTime.Now,
                };
                var result = await userBusiness.InsertAsync(bob);

                logger.LogTrace("bob created");
            }            

            var actionBusiness = provider.GetRequiredService<IBase_ActionBusiness>();
            var actionBusinessCount = await actionBusiness.GetIQueryable().CountAsync();
            if (actionBusinessCount == 0)
            {
                List<Base_Action> actions = new List<Base_Action>()
                    {
                         new Base_Action(){ Id="1178957405992521728",Deleted = false, ParentId=null,                  Type = ActionType.菜单, Name="系统管理", Url=null,                               Value=null,                    NeedAction=true,    Icon="setting",        Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1178957553778823168",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="权限管理", Url="/Base_Manage/Base_Action/List",    Value=null,                    NeedAction=true,    Icon="lock",           Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1179018395304071168",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="密钥管理", Url="/Base_Manage/Base_AppSecret/List", Value=null,                    NeedAction=true,    Icon="key",            Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1182652266117599232",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="用户管理", Url="/Base_Manage/Base_User/List",      Value=null,                    NeedAction=true,    Icon="user-add",       Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1182652367447789568",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="角色管理", Url="/Base_Manage/Base_Role/List",      Value=null,                    NeedAction=true,    Icon="safety",         Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1182652433302556672",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="部门管理", Url="/Base_Manage/Base_Department/List",Value=null,                    NeedAction=true,    Icon="usergroup-add",  Sort=1, CreateTime=DateTime.Now},
                         new Base_Action(){ Id="1182652599069839362",Deleted = false, ParentId="1178957405992521728", Type = ActionType.页面, Name="字典管理", Url="/Base_Manage/Base_Dictionary/List",Value=null,                    NeedAction=true,    Icon="edit",           Sort=1, CreateTime=DateTime.Now},
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
                logger.LogTrace("action created");
            }

            var appSecretBussiness = provider.GetRequiredService<IBase_AppSecretBusiness>();
            var appSecretCount = await appSecretBussiness.GetIQueryable().CountAsync();
            if (appSecretCount == 0)
            {
                List<Base_AppSecret> actions = new List<Base_AppSecret>()
                {
                    new Base_AppSecret(){  Id="1172497995938271232", AppId="PcAdmin", AppSecret="wtMaiTRPTT3hrf5e", AppName="后台AppId", CreateTime=DateTime.Now},
                    new Base_AppSecret(){  Id="1173937877642383360", AppId="AppAdmin", AppSecret="IVh9LLSVFcoQPQ5K", AppName="APP密钥", CreateTime=DateTime.Now}
                };

                var result = await appSecretBussiness.InsertAsync(actions);
                logger.LogTrace("appSecret created");
            }

            var dictionaryBusiness = provider.GetRequiredService<IBase_DictionaryBusiness>();
            var dictionaryCount = await dictionaryBusiness.GetIQueryable().CountAsync();
            if (dictionaryCount == 0)
            {
                List<Base_Dictionary> dictionaries = new List<Base_Dictionary>()
                {
                    new Base_Dictionary(){ Id="1",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, ControlType = ControlType.ComboBox,  Text = "性别", Value="Sex", Code = "", Sort=1, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="1_1",Deleted = false, ParentId="1", Type = DictionaryType.数据集,  Text = "女", Value="0", Code ="",  Sort=1, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="1_2",Deleted = false, ParentId="1", Type = DictionaryType.数据集,  Text = "男", Value="1", Code ="",  Sort=2, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="2",Deleted = false, ParentId=null,  Type = DictionaryType.字典项,  Text = "姓名", Value="UserName", Code = "", Sort=2, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="3",Deleted = false, ParentId=null,  Type = DictionaryType.字典项,  Text = "真实姓名", Value="RealName", Code = "", Sort=3, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="4",Deleted = false, ParentId=null,  Type = DictionaryType.字典项,  Text = "密码", Value="Password", Code = "", Sort=4, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="5",Deleted = false, ParentId=null,  Type = DictionaryType.字典项,  Text = "生日", Value="Birthday", Code = "", Sort=5, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="6",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, ControlType = ControlType.MultiComboBox,  Text = "角色", Value="RoleIdList", Code = "Role", Sort=6, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="7",Deleted = false, ParentId=null,  Type = DictionaryType.字典项, ControlType = ControlType.TreeSelect,  Text = "部门", Value="DepartmentId", Code = "Department", Sort=7, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="8",Deleted = false, ParentId=null,  Type = DictionaryType.字典项,  Text = "手机号码", Value="PhoneNumber", Code = "", Sort=8, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="9",Deleted = false, ParentId=null,  Type = DictionaryType.字典项,  Text = "创建时间", Value="CreateTime", Code = "", Sort=9, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="10",Deleted = false, ParentId=null,  Type = DictionaryType.字典项,  Text = "修改时间", Value="ModifyTime", Code = "", Sort=10, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="11",Deleted = false, ParentId=null,  Type = DictionaryType.字典项,  Text = "创建者", Value="CreatorName", Code = "", Sort=11, CreateTime=DateTime.Now },
                    new Base_Dictionary(){ Id="12",Deleted = false, ParentId=null,  Type = DictionaryType.字典项,  Text = "修改者", Value="ModifyName", Code = "", Sort=12, CreateTime=DateTime.Now },     
                };

                var result = await dictionaryBusiness.InsertAsync(dictionaries);
                logger.LogTrace("dictionary created");
            }

            if (configuration.GetSection("UseQuartz").Get<bool>() == true)
            {
                var quartz_TaskBusiness = provider.GetRequiredService<IQuartz_TaskBusiness>();

                var resetDataJob = quartz_TaskBusiness.FirstOrDefaultAsync(p => p.TaskName == "ResetDataJob").Result;

                if (resetDataJob == null)
                {
                    resetDataJob = new Quartz_Task()
                    {
                        Id = IdHelper.GetId(),
                        TaskName = "ResetDataJob",
                        GroupName = "SystemJob",
                        Interval = "0 */5 * * * ?",
                        ApiUrl = "ResetDataJob",
                        RequestType = "System",
                        Status = (int)TriggerState.Normal,
                        ForbidEdit = true,
                        CreateTime = DateTime.Now,
                    };

                    var result = await quartz_TaskBusiness.InsertAsync(resetDataJob);

                    logger.LogDebug("resetDataJob created");
                }

                var saveMessageJob = quartz_TaskBusiness.FirstOrDefaultAsync(p => p.TaskName == "SaveMessageJob").Result;

                if (saveMessageJob == null)
                {
                    saveMessageJob = new Quartz_Task()
                    {
                        Id = IdHelper.GetId(),
                        TaskName = "SaveMessageJob",
                        GroupName = "SystemJob",
                        Interval = "0/10 * * * * ?",
                        ApiUrl = "SaveMessageJob",
                        RequestType = "System",
                        Status = (int)TriggerState.Normal,
                        ForbidEdit = true,
                        CreateTime = DateTime.Now,
                    };

                    var result = await quartz_TaskBusiness.InsertAsync(saveMessageJob);

                    logger.LogDebug("storeMessageJob created");
                }

                var pushMessageJob = await quartz_TaskBusiness.FirstOrDefaultAsync(p => p.TaskName == "PushMessageJob");

                if (pushMessageJob == null)
                {
                    pushMessageJob = new Quartz_Task()
                    {
                        Id = IdHelper.GetId(),
                        TaskName = "PushMessageJob",
                        GroupName = "SystemJob",
                        Interval = "0/10 * * * * ?",
                        ApiUrl = "PushMessageJob",
                        RequestType = "System",
                        Status = (int)TriggerState.Normal,
                        ForbidEdit = true,
                        CreateTime = DateTime.Now,
                    };

                    var result = quartz_TaskBusiness.InsertAsync(pushMessageJob).Result;

                    logger.LogDebug("pushMessageJob created");
                }

                var textJob = await quartz_TaskBusiness.FirstOrDefaultAsync(p => p.TaskName == "GetLog");

                if (textJob == null)
                {
                    textJob = new Quartz_Task()
                    {
                        Id = IdHelper.GetId(),
                        TaskName = "GetLog",
                        GroupName = "Test",
                        Interval = "0/10 * * * * ?",
                        ApiUrl = "http://localhost:5000/Test/GetLogList",
                        RequestType = "System",
                        Status = (int)TriggerState.Paused,
                        CreateTime = DateTime.Now,
                        AuthKey = "Authorization",
                        AuthValue = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJBZG1pbiIsImV4cCI6MTYxMzQ1NTI0MX0.XbnD6R0Ozgp1xoI6BUrRjYaHwRYYAJ7OgU6gRO1sdbA",
                    };

                    var result = await quartz_TaskBusiness.InsertAsync(textJob);

                    logger.LogDebug("textJob created");
                }

            }


            if (configuration.GetSection("UseWorkflow").Get<bool>() == true)
            {
                var oA_DefTypeBusiness = provider.GetRequiredService<IOA_DefTypeBusiness>();
                var defcount = await oA_DefTypeBusiness.GetIQueryable().CountAsync();
                if (defcount == 0)
                {
                    List<OA_DefType> defs = new List<OA_DefType>()
                        {
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "分类", Name="请假", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "分类", Name="报销", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "分类", Name="顺序", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "分类", Name="选择", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "分类", Name="或签", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "分类", Name="与签", CreateTime = DateTime.Now, },

                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "请假", Name="病假", Unit ="天数", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "请假", Name="事假", Unit ="天数", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "请假", Name="调休", Unit ="天数", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "请假", Name="年假", Unit ="天数", CreateTime = DateTime.Now, },

                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "报销", Name="差旅费用", Unit ="费用(元)", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "报销", Name="采购费用", Unit ="费用(元)", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "报销", Name="活动费用", Unit ="费用(元)", CreateTime = DateTime.Now, },
                            new OA_DefType(){ Id=IdHelper.GetId(),Type = "报销", Name="日常费用", Unit ="费用(元)", CreateTime = DateTime.Now, },

                        };

                    var result = await oA_DefTypeBusiness.InsertAsync(defs);
                    logger.LogDebug("oa_deftype created");
                }

                var oA_DefFormBusiness = provider.GetRequiredService<IOA_DefFormBusiness>();
                var defformcount = await oA_DefFormBusiness.GetIQueryable().CountAsync();
                if (defformcount == 0)
                {
                    var directory = AppContext.BaseDirectory;
                    directory = directory.Replace("\\", "/");

                    List<OA_DefForm> defs = new List<OA_DefForm>();
                    string id = IdHelper.GetId();
                    var def = new OA_DefForm()
                    {
                        Id = id,
                        WorkflowJSON = File.ReadAllText($"{directory}/WorkflowCore/OAStep/g6test1.json"),
                        JSONId = "1274618511506804736",
                        Type = "请假",
                        Name = "请假流程",
                        Text = "最简单的请假流程",
                        Status = 1,
                        CreateTime = DateTime.Now,
                    };
                    defs.Add(def);
                    id = IdHelper.GetId();
                    def = new OA_DefForm()
                    {
                        Id = id,
                        WorkflowJSON = File.ReadAllText($"{directory}/WorkflowCore/OAStep/g6test2.json"),
                        JSONId = "1274620801831669760",
                        Type = "报销",
                        Name = "报销审批-与签",
                        Text = "所有审批人都要同意",
                        Status = 1,
                        CreateTime = DateTime.Now,
                    };
                    defs.Add(def);
                    id = IdHelper.GetId();
                    def = new OA_DefForm()
                    {
                        Id = id,
                        WorkflowJSON = File.ReadAllText($"{directory}/WorkflowCore/OAStep/g6test3.json"),
                        JSONId = "1274621154383892480",
                        Type = "报销",
                        Name = "报销审批-或签",
                        Text = "只要有一个人审批就行",
                        Status = 1,
                        CreateTime = DateTime.Now,
                    };
                    defs.Add(def);
                    id = IdHelper.GetId();
                    def = new OA_DefForm()
                    {
                        Id = id,
                        WorkflowJSON = File.ReadAllText($"{directory}/WorkflowCore/OAStep/g6test4.json"),
                        JSONId = "1274621654579810304",
                        Type = "顺序",
                        Name = "部门领导审批",
                        Text = "根据申请人所在部门自动查找生成审批人",
                        Status = 1,
                        CreateTime = DateTime.Now,
                    };
                    defs.Add(def);
                    id = IdHelper.GetId();
                    def = new OA_DefForm()
                    {
                        Id = id,
                        WorkflowJSON = File.ReadAllText($"{directory}/WorkflowCore/OAStep/g6test5.json"),
                        JSONId = "1274622508779180032",
                        Type = "报销",
                        Name = "并行流程",
                        Text = "两个分管部门同时进行审批",
                        Status = 1,
                        CreateTime = DateTime.Now,
                    };
                    defs.Add(def);
                    id = IdHelper.GetId();
                    def = new OA_DefForm()
                    {
                        Id = id,
                        WorkflowJSON = File.ReadAllText($"{directory}/WorkflowCore/OAStep/g6test6.json"),
                        JSONId = "1274623039325081600",
                        Type = "顺序",
                        Name = "有创建权限的流程",
                        Text = "只有管理员能创建的流程",
                        Status = 1,
                        Value = $"^{superadmin.Id}^",
                        CreateTime = DateTime.Now,
                    };
                    defs.Add(def);
                    id = IdHelper.GetId();
                    def = new OA_DefForm()
                    {
                        Id = id,
                        WorkflowJSON = File.ReadAllText($"{directory}/WorkflowCore/OAStep/g6test7.json"),
                        JSONId = "1274623664695808000",
                        Type = "请假",
                        Name = "请假流程-条件",
                        Text = "根据请假天数是否需要分管领导审批",
                        Status = 1,
                        CreateTime = DateTime.Now,
                    };
                    defs.Add(def);
                    var result = await oA_DefFormBusiness.InsertAsync(defs);
                    logger.LogDebug("oa_defform created");
                }
            }
        }
    }
}