using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelSplitAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateTrips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Destination = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "Destination", "EndDate", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, "金門、廈門", new DateTime(2026, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "金廈小三通", new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "台南", new DateTime(2026, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "台南美食之旅", new DateTime(2026, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trips");
        }
    }
}
