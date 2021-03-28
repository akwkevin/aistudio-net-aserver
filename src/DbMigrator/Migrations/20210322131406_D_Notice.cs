using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.Migrations
{
    public partial class D_Notice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "D_Notice",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "自然主键"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "角色Id"),
                    DepartmentId = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "角色Id"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "标题"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "类型"),
                    Appendix = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "附件"),
                    IsDraft = table.Column<bool>(type: "bit", nullable: false, comment: "草搞"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "租户Id"),
                    UserIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_D_Notice", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "D_Notice");
        }
    }
}
