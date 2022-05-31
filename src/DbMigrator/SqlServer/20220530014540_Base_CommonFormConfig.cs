using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator.SqlServer
{
    public partial class Base_CommonFormConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Base_CommonFormConfig",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "自然主键"),
                    Table = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "表名"),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "列头"),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "属性名"),
                    DisplayIndex = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "显示索引"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "配置类型 编辑项=0，列表=1"),
                    StringFormat = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "格式化"),
                    Visibility = table.Column<int>(type: "int", nullable: false, comment: "是否显示 Visible = 0,Hidden = 1,Collapsed = 2"),
                    ControlType = table.Column<int>(type: "int", nullable: false, comment: "控件类型，仅控件框使用"),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false, comment: "只读"),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, comment: "必输项"),
                    ItemSource = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "字典名"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "默认值"),
                    SortMemberPath = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "排序名"),
                    Converter = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "转换器"),
                    ConverterParameter = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "转换参数"),
                    HorizontalAlignment = table.Column<int>(type: "int", nullable: false, comment: "对齐方式 Left = 0,Center = 1,Right = 2,Stretch = 3"),
                    MaxWidth = table.Column<double>(type: "float", nullable: false, comment: "最大宽度"),
                    MinWidth = table.Column<double>(type: "float", nullable: false, comment: "最小宽度"),
                    Width = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "列表宽度"),
                    CanUserReorder = table.Column<bool>(type: "bit", nullable: false, comment: "是否可以重排"),
                    CanUserResize = table.Column<bool>(type: "bit", nullable: false, comment: "是否可以调整大小"),
                    CanUserSort = table.Column<bool>(type: "bit", nullable: false, comment: "是否可以排序"),
                    IsFrozen = table.Column<bool>(type: "bit", nullable: false, comment: "是否冻结"),
                    CellStyle = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "单元格样式，暂未实现"),
                    BackgroundExpression = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "背景颜色触发公式"),
                    ForegroundExpression = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "前景颜色触发公式"),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, comment: "否已删除"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间"),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "修改时间"),
                    CreatorId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "创建人Id"),
                    CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "创建人"),
                    ModifyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "修改人Id"),
                    ModifyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "修改人"),
                    TenantId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "租户Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Base_CommonFormConfig", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Base_CommonFormConfig");
        }
    }
}
