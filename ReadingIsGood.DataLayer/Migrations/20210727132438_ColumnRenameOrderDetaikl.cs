using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadingIsGood.DataLayer.Migrations
{
    public partial class ColumnRenameOrderDetaikl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "Content",
                table: "OrderDetail",
                newName: "PriceSum");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceSum",
                schema: "Content",
                table: "OrderDetail",
                newName: "Price");
        }
    }
}
