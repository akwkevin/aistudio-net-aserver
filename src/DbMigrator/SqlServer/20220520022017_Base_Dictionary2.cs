using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.SqlServer
{
    public partial class Base_Dictionary2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Base_Dictionary",
                type: "nvarchar(max)",
                nullable: true,
                comment: "显示值");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Base_Dictionary",
                type: "nvarchar(max)",
                nullable: true,
                comment: "数据值");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Base_Dictionary");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Base_Dictionary");
        }
    }
}
