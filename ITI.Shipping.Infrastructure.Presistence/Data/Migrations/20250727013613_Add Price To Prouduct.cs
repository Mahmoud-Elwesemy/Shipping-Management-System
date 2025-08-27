using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.Shipping.Infrastructure.Presistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToProuduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01961d25-b4da-7184-a2a8-765486bd4857",
                column: "CreatedAt",
                value: new DateTime(2025, 7, 27, 4, 36, 12, 76, DateTimeKind.Local).AddTicks(3591));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01961d25-b4da-71e9-a488-1b8db232e984",
                column: "CreatedAt",
                value: new DateTime(2025, 7, 27, 4, 36, 12, 76, DateTimeKind.Local).AddTicks(5322));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01961d25-b4da-75a5-a1f4-a7aa10e421ed",
                column: "CreatedAt",
                value: new DateTime(2025, 7, 27, 4, 36, 12, 76, DateTimeKind.Local).AddTicks(5203));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0195d439-9ca1-7873-9c14-a4bc1c201593",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 7, 27, 4, 36, 12, 58, DateTimeKind.Local).AddTicks(5517), "AQAAAAIAAYagAAAAEEZH94+fgMG3Tm4i1Dp2EtwPQYdc2MFuzV4KgA7pYHY2Cexnr4U/pE5Ijq24qWypRg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

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
        }
    }
}
