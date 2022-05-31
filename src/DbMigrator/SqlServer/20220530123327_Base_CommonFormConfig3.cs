using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.SqlServer
{
    public partial class Base_CommonFormConfig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFrozen",
                table: "Base_CommonFormConfig");

            migrationBuilder.AddColumn<string>(
                name: "HeaderStyle",
                table: "Base_CommonFormConfig",
                type: "nvarchar(max)",
                nullable: true,
                comment: "列头样式，赞未实现");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaderStyle",
                table: "Base_CommonFormConfig");

            migrationBuilder.AddColumn<bool>(
                name: "IsFrozen",
                table: "Base_CommonFormConfig",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "是否冻结");
        }
    }
}
