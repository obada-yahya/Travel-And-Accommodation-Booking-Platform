using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Guests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Guests",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Guests",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Guests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
            
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CountryCode", "CountryName", "Name", "PostOffice" },
                values: new object[,]
                {
                    { new Guid("3c7e66f5-46a9-4b8d-8e90-85b5a9e2c2fd"), "JP", "Japan", "Tokyo", "TKY" },
                    { new Guid("8d2aeb7a-7c67-4911-aa2c-d6a3b4dc7e9e"), "UK", "United Kingdom", "London", "LDN" },
                    { new Guid("f9e85d04-548c-4f98-afe9-2a8831c62a90"), "US", "United States", "New York", "NYC" }
                });

            migrationBuilder.InsertData(
                table: "Guests",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("aaf21a7d-8fc3-4c9f-8a8e-1eeec8dcd462"), "jane.smith@example.com", "Jane", "Smith", "2012345678" },
                    { new Guid("c6c45f7c-2dfe-4c1e-9a9b-8b173c71b32c"), "john.doe@example.com", "John", "Doe", "1234567890" },
                    { new Guid("f44c3eb4-2c8a-4a77-a31b-04c4619aa15a"), "hiroshi.tanaka@example.co.jp", "Hiroshi", "Tanaka", "312345678" }
                });

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("77b2c30b-65d0-4ea7-8a5e-71e7c294f117"), "muathejamil@gmail.com", "Muathe", "Jamil", "0598242354" },
                    { new Guid("a1d1aa11-12e7-4e0f-8425-67c1c1e62c2d"), "obadayahya.an@gmail.com", "Obada", "Yahya", "0598231234" }
                });
            
            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "CityId", "Description", "FloorsNumber", "Name", "OwnerId", "PhoneNumber", "Rating", "StreetAddress" },
                values: new object[,]
                {
                    { new Guid("9461e08b-92d3-45da-b6b3-efc0cfcc4a3a"), new Guid("3c7e66f5-46a9-4b8d-8e90-85b5a9e2c2fd"), "A resort with breathtaking sunset views over the ocean.", 5, "Sunset Resort", new Guid("77b2c30b-65d0-4ea7-8a5e-71e7c294f117"), "312345678", 4.2f, "789 Beachfront Road" },
                    { new Guid("98c2c9fe-1a1c-4eaa-a7f5-b9d19b246c27"), new Guid("f9e85d04-548c-4f98-afe9-2a8831c62a90"), "A luxurious hotel with top-notch amenities.", 10, "Luxury Inn", new Guid("a1d1aa11-12e7-4e0f-8425-67c1c1e62c2d"), "1234567890", 4.5f, "123 Main Street" },
                    { new Guid("bfa4173d-7893-48b9-a497-5f4c7fb2492b"), new Guid("8d2aeb7a-7c67-4911-aa2c-d6a3b4dc7e9e"), "A cozy lodge nestled in the heart of nature.", 3, "Cozy Lodge", new Guid("a1d1aa11-12e7-4e0f-8425-67c1c1e62c2d"), "2012345678", 3.8f, "456 Oak Avenue" }
                });

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "HotelId", "PricePerNight", "Category" },
                values: new object[,]
                {
                    { new Guid("4b4c0ea5-0b9a-4a20-8ad9-77441fb912d2"), new Guid("9461e08b-92d3-45da-b6b3-efc0cfcc4a3a"), 200f, "Suite" },
                    { new Guid("5a5de3b8-3ed8-4f0a-bda9-cf73225a64a1"), new Guid("98c2c9fe-1a1c-4eaa-a7f5-b9d19b246c27"), 100f, "Single" },
                    { new Guid("d67ddbe4-1f1a-4d85-bcc1-ec3a475ecb68"), new Guid("bfa4173d-7893-48b9-a497-5f4c7fb2492b"), 150f, "Double" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "AdultsCapacity", "ChildrenCapacity", "Rating", "RoomTypeId", "View" },
                values: new object[,]
                {
                    { new Guid("4e1cb3d9-bc3b-4997-a3d5-0c56cf17fe7a"), 3, 2, 4.2f, new Guid("d67ddbe4-1f1a-4d85-bcc1-ec3a475ecb68"), "Ocean View" },
                    { new Guid("a98b8a9d-4c5a-4a90-a2d2-5f1441b93db6"), 2, 1, 4.5f, new Guid("5a5de3b8-3ed8-4f0a-bda9-cf73225a64a1"), "City View" },
                    { new Guid("c6898b7e-ee09-4b36-8b20-22e8c2a63e29"), 4, 1, 4.8f, new Guid("4b4c0ea5-0b9a-4a20-8ad9-77441fb912d2"), "Mountain View" }
                });
            
            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "BookingDate", "CheckInDate", "CheckOutDate", "GuestId", "RoomId" },
                values: new object[,]
                {
                    { new Guid("0bf4a177-98b8-4f67-8a56-95669c320890"), new DateTime(2023, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f44c3eb4-2c8a-4a77-a31b-04c4619aa15a"), new Guid("c6898b7e-ee09-4b36-8b20-22e8c2a63e29") },
                    { new Guid("7d3155a2-95f8-4d9b-bc24-662ae053f1c9"), new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("c6c45f7c-2dfe-4c1e-9a9b-8b173c71b32c"), new Guid("a98b8a9d-4c5a-4a90-a2d2-5f1441b93db6") },
                    { new Guid("efeb3d13-3dab-46c9-aa9a-9f22dd58e06e"), new DateTime(2023, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("aaf21a7d-8fc3-4c9f-8a8e-1eeec8dcd462"), new Guid("4e1cb3d9-bc3b-4997-a3d5-0c56cf17fe7a") }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "BookingId", "Method", "Status" },
                values: new object[,]
                {
                    { new Guid("1c8d70bd-2534-4991-bddf-84c7edee1a79"), 1200.0, new Guid("efeb3d13-3dab-46c9-aa9a-9f22dd58e06e"), "Cash", "Pending" },
                    { new Guid("7f5cc9f0-796f-498d-9f3f-9e5249a4f6ae"), 1500.0, new Guid("0bf4a177-98b8-4f67-8a56-95669c320890"), "CreditCard", "Completed" },
                    { new Guid("8f974636-4f53-48d9-af99-2f7f1d3e0474"), 2000.0, new Guid("7d3155a2-95f8-4d9b-bc24-662ae053f1c9"), "MobileWallet", "Completed" }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "BookingId", "Comment", "Rating", "ReviewDate" },
                values: new object[,]
                {
                    { new Guid("192045db-c6db-49c9-aa6b-2e3d6c7f3b79"), new Guid("7d3155a2-95f8-4d9b-bc24-662ae053f1c9"), "Clean rooms and beautiful views.", 4.2f, new DateTime(2023, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("63e4bb25-28b1-4fc4-9b93-9254d94dab23"), new Guid("0bf4a177-98b8-4f67-8a56-95669c320890"), "Excellent service and comfortable stay!", 4.8f, new DateTime(2023, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("85a5a0b4-0e04-4c46-b7ac-6cf609e4f2aa"), new Guid("efeb3d13-3dab-46c9-aa9a-9f22dd58e06e"), "Friendly staff and great location.", 4.5f, new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("9461e08b-92d3-45da-b6b3-efc0cfcc4a3a"));

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("98c2c9fe-1a1c-4eaa-a7f5-b9d19b246c27"));

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("bfa4173d-7893-48b9-a497-5f4c7fb2492b"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: new Guid("1c8d70bd-2534-4991-bddf-84c7edee1a79"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: new Guid("7f5cc9f0-796f-498d-9f3f-9e5249a4f6ae"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: new Guid("8f974636-4f53-48d9-af99-2f7f1d3e0474"));

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: new Guid("192045db-c6db-49c9-aa6b-2e3d6c7f3b79"));

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: new Guid("63e4bb25-28b1-4fc4-9b93-9254d94dab23"));

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: new Guid("85a5a0b4-0e04-4c46-b7ac-6cf609e4f2aa"));

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("0bf4a177-98b8-4f67-8a56-95669c320890"));

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("7d3155a2-95f8-4d9b-bc24-662ae053f1c9"));

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("efeb3d13-3dab-46c9-aa9a-9f22dd58e06e"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("3c7e66f5-46a9-4b8d-8e90-85b5a9e2c2fd"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("8d2aeb7a-7c67-4911-aa2c-d6a3b4dc7e9e"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("f9e85d04-548c-4f98-afe9-2a8831c62a90"));

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: new Guid("77b2c30b-65d0-4ea7-8a5e-71e7c294f117"));

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: new Guid("a1d1aa11-12e7-4e0f-8425-67c1c1e62c2d"));

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: new Guid("aaf21a7d-8fc3-4c9f-8a8e-1eeec8dcd462"));

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: new Guid("c6c45f7c-2dfe-4c1e-9a9b-8b173c71b32c"));

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: new Guid("f44c3eb4-2c8a-4a77-a31b-04c4619aa15a"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("4e1cb3d9-bc3b-4997-a3d5-0c56cf17fe7a"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("a98b8a9d-4c5a-4a90-a2d2-5f1441b93db6"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("c6898b7e-ee09-4b36-8b20-22e8c2a63e29"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("4b4c0ea5-0b9a-4a20-8ad9-77441fb912d2"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("5a5de3b8-3ed8-4f0a-bda9-cf73225a64a1"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("d67ddbe4-1f1a-4d85-bcc1-ec3a475ecb68"));

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
