using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadingIsGood.DataLayer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Content");

            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "Content",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountLeft = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Auth",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "Customer"),
                    Email = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    PasswordHashed = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: true),
                    FirstName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    LastName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "Content",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    OrderStatus = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, defaultValue: "Created"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "Auth",
                columns: table => new
                {
                    RefreshTokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Token = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    Rejected = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Auth",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                schema: "Content",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Content",
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Content",
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                schema: "Content",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Uuid",
                schema: "Content",
                table: "Order",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                schema: "Content",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductId",
                schema: "Content",
                table: "OrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_Uuid",
                schema: "Content",
                table: "OrderDetail",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Uuid",
                schema: "Content",
                table: "Product",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                schema: "Auth",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Uuid",
                schema: "Auth",
                table: "RefreshToken",
                column: "Uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Uuid",
                schema: "Auth",
                table: "User",
                column: "Uuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetail",
                schema: "Content");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "Content");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "Content");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Auth");
        }
    }
}
