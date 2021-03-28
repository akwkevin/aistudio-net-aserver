using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.Migrations
{
    public partial class D_NoticeReadingMarks3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "D_UserMessage",
                type: "nvarchar(max)",
                nullable: true,
                comment: "状态 =0草稿中，=1已发送，=2废弃撤回，=3发送失败");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "D_UserMail",
                type: "nvarchar(max)",
                nullable: true,
                comment: "状态 =0草稿中，=1已发布，=2撤回");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                comment: "状态 =0草稿中，=1已发布，=2撤回",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "状态 =0草稿中，=1已发布，=2废弃");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "D_UserMessage");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "D_UserMail");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "D_Notice",
                type: "nvarchar(max)",
                nullable: true,
                comment: "状态 =0草稿中，=1已发布，=2废弃",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "状态 =0草稿中，=1已发布，=2撤回");
        }
    }
}
