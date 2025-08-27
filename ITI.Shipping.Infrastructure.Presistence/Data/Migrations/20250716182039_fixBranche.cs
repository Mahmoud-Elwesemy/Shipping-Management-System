using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.Shipping.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixBranche : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CitySettingId",
                table: "Branches",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01961d25-b4da-7184-a2a8-765486bd4857",
                column: "CreatedAt",
                value: new DateTime(2025, 7, 16, 21, 20, 37, 940, DateTimeKind.Local).AddTicks(9584));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01961d25-b4da-71e9-a488-1b8db232e984",
                column: "CreatedAt",
                value: new DateTime(2025, 7, 16, 21, 20, 37, 941, DateTimeKind.Local).AddTicks(1312));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01961d25-b4da-75a5-a1f4-a7aa10e421ed",
                column: "CreatedAt",
                value: new DateTime(2025, 7, 16, 21, 20, 37, 941, DateTimeKind.Local).AddTicks(1212));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195d439-9ca1-7873-9c14-a4bc1c201593",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 7, 16, 21, 20, 37, 923, DateTimeKind.Local).AddTicks(163), "AQAAAAIAAYagAAAAEOK/PiEECzWmVLm8GEQc2kJOccnfgdI2xJfYIq85yNX0FpamL8aPirTYL2UYeMlfGg==" });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CitySettingId",
                table: "Branches",
                column: "CitySettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_CitySettings_CitySettingId",
                table: "Branches",
                column: "CitySettingId",
                principalTable: "CitySettings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_CitySettings_CitySettingId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_CitySettingId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "CitySettingId",
                table: "Branches");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01961d25-b4da-7184-a2a8-765486bd4857",
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 3, 59, 22, 748, DateTimeKind.Local).AddTicks(2227));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01961d25-b4da-71e9-a488-1b8db232e984",
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 3, 59, 22, 748, DateTimeKind.Local).AddTicks(4162));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01961d25-b4da-75a5-a1f4-a7aa10e421ed",
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 3, 59, 22, 748, DateTimeKind.Local).AddTicks(4054));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195d439-9ca1-7873-9c14-a4bc1c201593",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 4, 12, 3, 59, 22, 724, DateTimeKind.Local).AddTicks(4662), "AQAAAAIAAYagAAAAEE5Z1EcBTN2upkNarqktQnuh9rt8tdGsiDXG/X2qDxJHc6dfNwjFzaaxzOxCWqAEQw==" });
        }
    }
}
