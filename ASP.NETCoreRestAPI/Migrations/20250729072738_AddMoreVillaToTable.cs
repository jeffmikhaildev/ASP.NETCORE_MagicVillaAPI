using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP.NETCoreRestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreVillaToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "CreatedDate", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedDate" },
                values: new object[,]
                {
                    { 3, "", new DateOnly(2025, 7, 29), "A serene villa with stunning mountain views.", "https://example.com/images/mountain-view-villa.jpg", "Mountain View Villa", 5, 250.0, 1600, new DateOnly(1, 1, 1) },
                    { 4, "", new DateOnly(2025, 7, 29), "A charming villa surrounded by lush gardens.", "https://example.com/images/garden-villa.jpg", "Garden Villa", 3, 180.0, 1400, new DateOnly(1, 1, 1) },
                    { 5, "", new DateOnly(2025, 7, 29), "A modern villa located in the heart of the city.", "https://example.com/images/city-center-villa.jpg", "City Center Villa", 2, 220.0, 1300, new DateOnly(1, 1, 1) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
