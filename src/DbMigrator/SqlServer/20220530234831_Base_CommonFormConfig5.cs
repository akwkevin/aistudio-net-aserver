using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.SqlServer
{
    public partial class Base_CommonFormConfig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "Base_CommonFormConfig",
                type: "nvarchar(max)",
                nullable: true,
                comment: "错误信息");

            migrationBuilder.AddColumn<string>(
                name: "Regex",
                table: "Base_CommonFormConfig",
                type: "nvarchar(max)",
                nullable: true,
                comment: "正则校验表达式");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "Base_CommonFormConfig");

            migrationBuilder.DropColumn(
                name: "Regex",
                table: "Base_CommonFormConfig");
        }
    }
}
