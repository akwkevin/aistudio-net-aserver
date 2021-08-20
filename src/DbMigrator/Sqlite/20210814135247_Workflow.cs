using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.Sqlite
{
    public partial class Workflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Base_Action",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    ParentId = table.Column<string>(type: "TEXT", nullable: true, comment: "父级Id"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false, comment: "类型,菜单=0,页面=1,权限=2"),
                    Name = table.Column<string>(type: "TEXT", nullable: true, comment: "权限名/菜单名"),
                    Url = table.Column<string>(type: "TEXT", nullable: true, comment: "菜单地址"),
                    Value = table.Column<string>(type: "TEXT", nullable: true, comment: "权限值"),
                    NeedAction = table.Column<bool>(type: "INTEGER", nullable: false, comment: "是否需要权限(仅页面有效)"),
                    Icon = table.Column<string>(type: "TEXT", nullable: true, comment: "图标"),
                    Sort = table.Column<int>(type: "INTEGER", nullable: false, comment: "排序"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Action", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Base_AppSecret",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    AppId = table.Column<string>(type: "TEXT", nullable: true, comment: "应用Id"),
                    AppSecret = table.Column<string>(type: "TEXT", nullable: true, comment: "应用密钥"),
                    AppName = table.Column<string>(type: "TEXT", nullable: true, comment: "应用名"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_AppSecret", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Base_DbLink",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    LinkName = table.Column<string>(type: "TEXT", nullable: true, comment: "连接名"),
                    ConnectionStr = table.Column<string>(type: "TEXT", nullable: true, comment: "连接字符串"),
                    DbType = table.Column<string>(type: "TEXT", nullable: true, comment: "数据库类型"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_DbLink", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Base_Department",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    Name = table.Column<string>(type: "TEXT", nullable: true, comment: "部门名"),
                    ParentId = table.Column<string>(type: "TEXT", nullable: true, comment: "上级部门Id"),
                    ParentIds = table.Column<string>(type: "TEXT", nullable: true),
                    ParentNames = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Base_Role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    RoleName = table.Column<string>(type: "TEXT", nullable: true, comment: "角色名"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Base_RoleAction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    RoleId = table.Column<string>(type: "TEXT", nullable: true, comment: "用户Id"),
                    ActionId = table.Column<string>(type: "TEXT", nullable: true, comment: "权限Id"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_RoleAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Base_User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    UserName = table.Column<string>(type: "TEXT", nullable: true, comment: "用户名"),
                    Password = table.Column<string>(type: "TEXT", nullable: true, comment: "密码"),
                    RealName = table.Column<string>(type: "TEXT", nullable: true, comment: "姓名"),
                    Sex = table.Column<int>(type: "INTEGER", nullable: false, comment: "性别"),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "出生日期"),
                    DepartmentId = table.Column<string>(type: "TEXT", nullable: true, comment: "所属部门Id"),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Avatar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Base_UserLog",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false, comment: "自然主键"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    CreatorId = table.Column<string>(type: "TEXT", nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", nullable: true, comment: "创建人姓名"),
                    LogType = table.Column<string>(type: "TEXT", nullable: true, comment: "日志类型"),
                    LogContent = table.Column<string>(type: "TEXT", nullable: true, comment: "日志内容")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_UserLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Base_UserRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false, comment: "主键"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    CreatorId = table.Column<string>(type: "TEXT", nullable: true, comment: "创建人Id"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    UserId = table.Column<string>(type: "TEXT", nullable: true, comment: "用户Id"),
                    RoleId = table.Column<string>(type: "TEXT", nullable: true, comment: "角色Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "D_Notice",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    AnyId = table.Column<string>(type: "TEXT", nullable: true, comment: "Mode=0，对应ALL,Mode=1,对应用户Id,Mode=2,对应角色Id，Mode=3，对应部门Id"),
                    Text = table.Column<string>(type: "TEXT", nullable: true, comment: "内容"),
                    Title = table.Column<string>(type: "TEXT", nullable: true, comment: "标题"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false, comment: "类型=0，通告"),
                    Mode = table.Column<int>(type: "INTEGER", nullable: false, comment: "类型=0，全部，=1，发给指定用户，=2，发给指定角色，=3，发给指定部门"),
                    Appendix = table.Column<string>(type: "TEXT", nullable: true, comment: "附件"),
                    Status = table.Column<int>(type: "INTEGER", nullable: false, comment: "状态 =0草稿中，=1已发布，=2撤回"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_D_Notice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "D_NoticeReadingMarks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    NoticeId = table.Column<string>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_D_NoticeReadingMarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "D_UserGroup",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    UserIds = table.Column<string>(type: "TEXT", nullable: true),
                    UserNames = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    Appendix = table.Column<string>(type: "TEXT", nullable: true),
                    Avatar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ManagerIds = table.Column<string>(type: "TEXT", nullable: true),
                    ManagerNames = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_D_UserGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "D_UserMail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    CCIds = table.Column<string>(type: "TEXT", nullable: true),
                    CCNames = table.Column<string>(type: "TEXT", nullable: true),
                    ReadingMarks = table.Column<string>(type: "TEXT", nullable: true),
                    StarMark = table.Column<bool>(type: "INTEGER", nullable: false),
                    Appendix = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false, comment: "状态 =0草稿中，=1已发布，=2撤回"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id"),
                    UserIds = table.Column<string>(type: "TEXT", nullable: true),
                    UserNames = table.Column<string>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_D_UserMail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "D_UserMessage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    ReadingMarks = table.Column<string>(type: "TEXT", nullable: true),
                    GroupId = table.Column<string>(type: "TEXT", nullable: true),
                    GroupName = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false, comment: "状态 =0草稿中，=1已发送，=2废弃撤回，=3发送失败"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id"),
                    UserIds = table.Column<string>(type: "TEXT", nullable: true),
                    UserNames = table.Column<string>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_D_UserMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    EventKey = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    EventData = table.Column<string>(type: "TEXT", nullable: true),
                    EventTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsProcessed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionError",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkflowId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ExecutionPointerId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ErrorTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionError", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "OA_DefForm",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    WorkflowJSON = table.Column<string>(type: "TEXT", nullable: true),
                    JSONId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    JSONVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Sort = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true, comment: "权限值"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OA_DefForm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OA_DefType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Unit = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OA_DefType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OA_UserForm",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    DefFormId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DefFormName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DefFormJsonId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DefFormJsonVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    Grade = table.Column<int>(type: "INTEGER", nullable: false),
                    Flag = table.Column<double>(type: "REAL", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: true),
                    Appendix = table.Column<string>(type: "TEXT", nullable: true),
                    ExtendJSON = table.Column<string>(type: "TEXT", nullable: true),
                    ApplicantUser = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ApplicantUserId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ApplicantDepartment = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ApplicantDepartmentId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ApplicantRole = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ApplicantRoleId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserRoleNames = table.Column<string>(type: "TEXT", nullable: true),
                    UserRoleIds = table.Column<string>(type: "TEXT", nullable: true),
                    AlreadyUserNames = table.Column<string>(type: "TEXT", nullable: true),
                    AlreadyUserIds = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    SubType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Unit = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ExpectedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CurrentNode = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id"),
                    UserIds = table.Column<string>(type: "TEXT", nullable: true),
                    UserNames = table.Column<string>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OA_UserForm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OA_UserFormStep",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    UserFormId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    RoleIds = table.Column<string>(type: "TEXT", nullable: true),
                    RoleNames = table.Column<string>(type: "TEXT", nullable: true),
                    Remarks = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    StepName = table.Column<string>(type: "TEXT", nullable: true),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OA_UserFormStep", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quartz_Task",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    TaskName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    GroupName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Interval = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ApiUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    AuthKey = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    AuthValue = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Describe = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    RequestType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastRunTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ForbidOperate = table.Column<bool>(type: "INTEGER", nullable: false),
                    ForbidEdit = table.Column<bool>(type: "INTEGER", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quartz_Task", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubscriptionId = table.Column<Guid>(type: "TEXT", maxLength: 200, nullable: false),
                    WorkflowId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    StepId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExecutionPointerId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    EventName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    EventKey = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    SubscribeAsOf = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SubscriptionData = table.Column<string>(type: "TEXT", nullable: true),
                    ExternalToken = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ExternalWorkerId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ExternalTokenExpiry = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "Workflow",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InstanceId = table.Column<Guid>(type: "TEXT", maxLength: 200, nullable: false),
                    WorkflowDefinitionId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Reference = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    NextExecution = table.Column<long>(type: "INTEGER", nullable: true),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompleteTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflow", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionPointer",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkflowId = table.Column<long>(type: "INTEGER", nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    StepId = table.Column<int>(type: "INTEGER", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    SleepUntil = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PersistenceData = table.Column<string>(type: "TEXT", nullable: true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EventName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    EventKey = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    EventPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    EventData = table.Column<string>(type: "TEXT", nullable: true),
                    StepName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    RetryCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Children = table.Column<string>(type: "TEXT", nullable: true),
                    ContextItem = table.Column<string>(type: "TEXT", nullable: true),
                    PredecessorId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Outcome = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Scope = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionPointer", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionAttribute",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExecutionPointerId = table.Column<long>(type: "INTEGER", nullable: false),
                    AttributeKey = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    AttributeValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionAttribute", x => x.PersistenceId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionPointer_WorkflowId",
                table: "ExecutionPointer",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtensionAttribute_ExecutionPointerId",
                table: "ExtensionAttribute",
                column: "ExecutionPointerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Base_Action");

            migrationBuilder.DropTable(
                name: "Base_AppSecret");

            migrationBuilder.DropTable(
                name: "Base_DbLink");

            migrationBuilder.DropTable(
                name: "Base_Department");

            migrationBuilder.DropTable(
                name: "Base_Role");

            migrationBuilder.DropTable(
                name: "Base_RoleAction");

            migrationBuilder.DropTable(
                name: "Base_User");

            migrationBuilder.DropTable(
                name: "Base_UserLog");

            migrationBuilder.DropTable(
                name: "Base_UserRole");

            migrationBuilder.DropTable(
                name: "D_Notice");

            migrationBuilder.DropTable(
                name: "D_NoticeReadingMarks");

            migrationBuilder.DropTable(
                name: "D_UserGroup");

            migrationBuilder.DropTable(
                name: "D_UserMail");

            migrationBuilder.DropTable(
                name: "D_UserMessage");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "ExecutionError");

            migrationBuilder.DropTable(
                name: "ExtensionAttribute");

            migrationBuilder.DropTable(
                name: "OA_DefForm");

            migrationBuilder.DropTable(
                name: "OA_DefType");

            migrationBuilder.DropTable(
                name: "OA_UserForm");

            migrationBuilder.DropTable(
                name: "OA_UserFormStep");

            migrationBuilder.DropTable(
                name: "Quartz_Task");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "ExecutionPointer");

            migrationBuilder.DropTable(
                name: "Workflow");
        }
    }
}
