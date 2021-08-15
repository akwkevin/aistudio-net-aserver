using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.Migrations
{
    public partial class D_NoticeReadingMarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "D_NoticeReadingMarks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "自然主键"),
                    NoticeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_D_NoticeReadingMarks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "D_NoticeReadingMarks");
        }
    }
}
