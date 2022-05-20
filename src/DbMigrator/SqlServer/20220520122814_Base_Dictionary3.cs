using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.SqlServer
{
    public partial class Base_Dictionary3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Base_Dictionary");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Base_Dictionary",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Value相同，使用Code区分，暂时没启用",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "字典名相同，使用Code区分，暂时没启用");

            migrationBuilder.AddColumn<int>(
                name: "ControlType",
                table: "Base_Dictionary",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "数据类型");

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Base_Dictionary",
                type: "nvarchar(max)",
                nullable: true,
                comment: "描述");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControlType",
                table: "Base_Dictionary");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Base_Dictionary");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Base_Dictionary",
                type: "nvarchar(max)",
                nullable: true,
                comment: "字典名相同，使用Code区分，暂时没启用",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Value相同，使用Code区分，暂时没启用");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Base_Dictionary",
                type: "nvarchar(max)",
                nullable: true,
                comment: "字典名/数据名");
        }
    }
}
