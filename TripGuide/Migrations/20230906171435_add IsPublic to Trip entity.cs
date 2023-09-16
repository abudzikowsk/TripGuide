using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripGuide.Migrations
{
    /// <inheritdoc />
    public partial class addIsPublictoTripentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Trips",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Trips");
        }
    }
}
