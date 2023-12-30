using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RatingAndPriceConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_RoomType_PriceRange",
                table: "RoomTypes",
                sql: "[PricePerNight] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_RatingRange1",
                table: "Rooms",
                sql: "[Rating] >= 0 AND [Rating] <= 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_RatingRange",
                table: "Reviews",
                sql: "[Rating] >= 0 AND [Rating] <= 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Hotel_RatingRange",
                table: "Hotels",
                sql: "[Rating] >= 0 AND [Rating] <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_RoomType_PriceRange",
                table: "RoomTypes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_RatingRange1",
                table: "Rooms");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_RatingRange",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Hotel_RatingRange",
                table: "Hotels");
        }
    }
}
