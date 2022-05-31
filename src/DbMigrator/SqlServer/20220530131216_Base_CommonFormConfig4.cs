using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.SqlServer
{
    public partial class Base_CommonFormConfig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyType",
                table: "Base_CommonFormConfig",
                type: "nvarchar(max)",
                nullable: true,
                comment: "属性类型");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "Base_CommonFormConfig");
        }
    }
}
