using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.SqlServer
{
    public partial class Base_CommonFormConfig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Base_CommonFormConfig",
                type: "int",
                nullable: false,
                comment: "配置类型 查询=0，列表=1",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "配置类型 编辑项=0，列表=1");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayIndex",
                table: "Base_CommonFormConfig",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "显示索引",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "显示索引");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Base_CommonFormConfig",
                type: "int",
                nullable: false,
                comment: "配置类型 编辑项=0，列表=1",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "配置类型 查询=0，列表=1");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayIndex",
                table: "Base_CommonFormConfig",
                type: "nvarchar(max)",
                nullable: true,
                comment: "显示索引",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "显示索引");
        }
    }
}
