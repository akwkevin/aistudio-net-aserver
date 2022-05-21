using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.Sqlite
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Base_Dictionary",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, comment: "自然主键"),
                    ParentId = table.Column<string>(type: "TEXT", nullable: true, comment: "父级Id"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false, comment: "类型,字典项=0,数据集=1"),
                    ControlType = table.Column<int>(type: "INTEGER", nullable: false, comment: "数据类型"),
                    Value = table.Column<string>(type: "TEXT", nullable: true, comment: "数据值"),
                    Code = table.Column<string>(type: "TEXT", nullable: true, comment: "Value相同，使用Code区分，暂时没启用"),
                    Text = table.Column<string>(type: "TEXT", nullable: true, comment: "显示值"),
                    Remark = table.Column<string>(type: "TEXT", nullable: true, comment: "描述"),
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
                    table.PrimaryKey("PK_Base_Dictionary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Base_Test",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false, comment: "自然主键"),
                    ParentId = table.Column<string>(type: "TEXT", nullable: true, comment: "父级Id"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false, comment: "类型,菜单=0,页面=1,权限=2"),
                    Name = table.Column<string>(type: "TEXT", nullable: true, comment: "权限名/菜单名"),
                    Url = table.Column<string>(type: "TEXT", nullable: true, comment: "菜单地址"),
                    Value = table.Column<string>(type: "TEXT", nullable: true, comment: "权限值"),
                    NeedTest = table.Column<bool>(type: "INTEGER", nullable: false, comment: "是否需要权限(仅页面有效)"),
                    Icon = table.Column<string>(type: "TEXT", nullable: true, comment: "图标"),
                    Sort = table.Column<int>(type: "INTEGER", nullable: false, comment: "排序"),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "TEXT", nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "TEXT", nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "TEXT", nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "TEXT", nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "TEXT", nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_Test", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Base_Dictionary");

            migrationBuilder.DropTable(
                name: "Base_Test");
        }
    }
}
