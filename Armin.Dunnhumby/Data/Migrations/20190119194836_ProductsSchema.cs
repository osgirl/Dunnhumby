using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;
using System;

namespace Armin.Dunnhumby.Web.Data.Migrations
{
    // https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/
    public partial class ProductsSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256),
                    Price = table.Column<decimal>(),
                    Description = table.Column<string>(nullable: true),
                    LastUpdate = table.Column<DateTime?>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Products", x => x.Id); });


            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256),
                    Description = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(),
                    Start = table.Column<DateTime>(),
                    End = table.Column<DateTime?>(nullable: true),
                    LastUpdate = table.Column<DateTime?>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade, onUpdate: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}