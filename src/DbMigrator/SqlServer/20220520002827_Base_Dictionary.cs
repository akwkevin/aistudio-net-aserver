using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.SqlServer
{
    public partial class Base_Dictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Base_Dictionary",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "自然主键"),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "父级Id"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "类型,字典项=0,数据集=1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "字典名/数据名"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "字典名相同，使用Code区分，暂时没启用"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Dictionary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Base_Test",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "自然主键"),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "父级Id"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "类型,菜单=0,页面=1,权限=2"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "权限名/菜单名"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "菜单地址"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "权限值"),
                    NeedTest = table.Column<bool>(type: "bit", nullable: false, comment: "是否需要权限(仅页面有效)"),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "图标"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Test", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EventKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EventData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionError",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExecutionPointerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ErrorTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionError", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 200, nullable: false),
                    WorkflowId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    ExecutionPointerId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EventKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SubscribeAsOf = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriptionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalToken = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExternalWorkerId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExternalTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "Workflow",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstanceId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 200, nullable: false),
                    WorkflowDefinitionId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NextExecution = table.Column<long>(type: "bigint", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflow", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionPointer",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    SleepUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersistenceData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EventKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EventPublished = table.Column<bool>(type: "bit", nullable: false),
                    EventData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    Children = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContextItem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PredecessorId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionPointer", x => x.PersistenceId);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionAttribute",
                columns: table => new
                {
                    PersistenceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutionPointerId = table.Column<long>(type: "bigint", nullable: false),
                    AttributeKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Base_Dictionary");

            migrationBuilder.DropTable(
                name: "Base_Test");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "ExecutionError");

            migrationBuilder.DropTable(
                name: "ExtensionAttribute");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "ExecutionPointer");

            migrationBuilder.DropTable(
                name: "Workflow");
        }
    }
}
