using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheMatchaClub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOutOfStock",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockLevel",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOutOfStock",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StockLevel",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
