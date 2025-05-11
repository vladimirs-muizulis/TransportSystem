using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddBusStopTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ArrivalTime",
                table: "BusStops",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DepartureTime",
                table: "BusStops",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "BusStops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                table: "BusStops");

            migrationBuilder.DropColumn(
                name: "DepartureTime",
                table: "BusStops");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "BusStops");
        }
    }
}
