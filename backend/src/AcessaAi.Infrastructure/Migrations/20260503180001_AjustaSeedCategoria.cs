using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcessaAi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustaSeedCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(1768), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3335), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3337), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3338), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3339), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3340), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3340), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCadastro",
                value: new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3341), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
