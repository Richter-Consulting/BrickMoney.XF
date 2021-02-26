using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrickMoney.Migrations
{
    public partial class Init_One : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicInfo",
                columns: table => new
                {
                    LegoSetId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameEN = table.Column<string>(nullable: true),
                    NameDE = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    RrPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicInfo", x => x.LegoSetId);
                });

            migrationBuilder.CreateTable(
                name: "LegoSetUserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    State = table.Column<int>(nullable: false),
                    IsSelected = table.Column<bool>(nullable: false),
                    LegoSetId = table.Column<int>(nullable: false),
                    PurchasingPrice = table.Column<double>(nullable: false),
                    Seller = table.Column<string>(nullable: true),
                    PurchaseDate = table.Column<DateTime>(nullable: false),
                    RetailPrice = table.Column<double>(nullable: false),
                    SaleDate = table.Column<DateTime>(nullable: false),
                    SoldOver = table.Column<string>(nullable: true),
                    Buyer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegoSetUserInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegoSetUserInfo_BasicInfo_LegoSetId",
                        column: x => x.LegoSetId,
                        principalTable: "BasicInfo",
                        principalColumn: "LegoSetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegoSetUserInfo_LegoSetId",
                table: "LegoSetUserInfo",
                column: "LegoSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegoSetUserInfo");

            migrationBuilder.DropTable(
                name: "BasicInfo");
        }
    }
}
