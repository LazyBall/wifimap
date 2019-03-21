using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCFService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WiFiNetworks",
                columns: table => new
                {
                    BSSID = table.Column<string>(maxLength: 17, nullable: false),
                    SSID = table.Column<string>(maxLength: 32, nullable: false),
                    Encryption = table.Column<string>(maxLength: 16, nullable: false),
                    Frequency = table.Column<int>(nullable: false),
                    RSSI = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    LastDetection = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WiFiNetworks", x => x.BSSID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WiFiNetworks");
        }
    }
}
