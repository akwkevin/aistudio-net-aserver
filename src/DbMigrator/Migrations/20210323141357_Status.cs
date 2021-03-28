using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.Migrations
{
    public partial class Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "D_UserMail");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "D_UserMessage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "状态 =0草稿中，=1已发送，=2废弃撤回，=3发送失败",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "状态 =0草稿中，=1已发送，=2废弃撤回，=3发送失败");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "D_UserMail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "状态 =0草稿中，=1已发布，=2撤回",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "状态 =0草稿中，=1已发布，=2撤回");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "D_Notice",
                type: "int",
                nullable: false,
                comment: "类型=0，通告",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "类型=0，发给指定用户，=1，发给指定角色，=2，发给指定部门");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "D_Notice",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "状态 =0草稿中，=1已发布，=2撤回",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "状态 =0草稿中，=1已发布，=2撤回");

            migrationBuilder.AddColumn<int>(
                name: "Mode",
                table: "D_Notice",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "类型=0，全部，=1，发给指定用户，=2，发给指定角色，=3，发给指定部门");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mode",
                table: "D_Notice");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "D_UserMessage",
                type: "nvarchar(max)",
                nullable: true,
                comment: "状态 =0草稿中，=1已发送，=2废弃撤回，=3发送失败",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "状态 =0草稿中，=1已发送，=2废弃撤回，=3发送失败");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "D_UserMail",
                type: "nvarchar(max)",
                nullable: true,
                comment: "状态 =0草稿中，=1已发布，=2撤回",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "状态 =0草稿中，=1已发布，=2撤回");

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "D_UserMail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "D_Notice",
                type: "int",
                nullable: false,
                comment: "类型=0，发给指定用户，=1，发给指定角色，=2，发给指定部门",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "类型=0，通告");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                comment: "状态 =0草稿中，=1已发布，=2撤回",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "状态 =0草稿中，=1已发布，=2撤回");
        }
    }
}
