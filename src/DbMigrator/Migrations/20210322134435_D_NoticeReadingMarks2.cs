using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.Migrations
{
    public partial class D_NoticeReadingMarks2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "D_Notice");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "D_Notice",
                type: "int",
                nullable: false,
                comment: "类型=0，发给指定用户，=1，发给指定角色，=2，发给指定部门",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "类型");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                comment: "部门Id",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "角色Id");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                comment: "状态 =0草稿中，=1已发布，=2废弃");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "D_Notice");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "D_Notice",
                type: "int",
                nullable: false,
                comment: "类型",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "类型=0，发给指定用户，=1，发给指定角色，=2，发给指定部门");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                comment: "角色Id",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "部门Id");

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "D_Notice",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "草搞");
        }
    }
}
