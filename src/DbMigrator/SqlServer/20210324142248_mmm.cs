using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.Migrations
{
    public partial class mmm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "D_Notice");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "D_Notice");

            migrationBuilder.DropColumn(
                name: "UserIds",
                table: "D_Notice");

            migrationBuilder.DropColumn(
                name: "UserNames",
                table: "D_Notice");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                comment: "内容",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnyId",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Mode=0，对应ALL,Mode=1,对应用户Id,Mode=2,对应角色Id，Mode=3，对应部门Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnyId",
                table: "D_Notice");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "内容");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentId",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                comment: "部门Id");

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                comment: "角色Id");

            migrationBuilder.AddColumn<string>(
                name: "UserIds",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNames",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
