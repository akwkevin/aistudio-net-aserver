using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.SqlServer
{
    public partial class Base_CommonFormConfig6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Error",
                table: "Base_CommonFormConfig",
                newName: "ErrorMessage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                table: "Base_CommonFormConfig",
                newName: "Error");
        }
    }
}
