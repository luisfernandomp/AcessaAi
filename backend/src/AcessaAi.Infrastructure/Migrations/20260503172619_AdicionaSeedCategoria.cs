using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AcessaAi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaSeedCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UrlIcone",
                table: "Categorias",
                newName: "Icone");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Categorias",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Ativo", "DataAtualizacao", "DataCadastro", "Descricao", "Icone", "Nome" },
                values: new object[,]
                {
                    { 1, true, null, new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(1768), new TimeSpan(0, 0, 0, 0, 0)), "Entrada principal nivelada ou com rampa.", "fa-wheelchair-move", "Rampa de Acesso" },
                    { 2, true, null, new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3335), new TimeSpan(0, 0, 0, 0, 0)), "Banheiro com barras de apoio e espaço para manobra.", "fa-restroom", "Banheiro Adaptado" },
                    { 3, true, null, new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3337), new TimeSpan(0, 0, 0, 0, 0)), "Elevador ou plataforma elevatória para acesso aos andares.", "fa-elevator", "Elevador" },
                    { 4, true, null, new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3338), new TimeSpan(0, 0, 0, 0, 0)), "Vagas demarcadas e próximas à entrada.", "fa-square-parking", "Estacionamento Reservado" },
                    { 5, true, null, new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3339), new TimeSpan(0, 0, 0, 0, 0)), "Piso de alerta e direcional para pessoas com deficiência visual.", "fa-person-walking-with-cane", "Piso Tátil" },
                    { 6, true, null, new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3340), new TimeSpan(0, 0, 0, 0, 0)), "Cardápios ou sinalizações em Braille.", "fa-braille", "Braille" },
                    { 7, true, null, new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3340), new TimeSpan(0, 0, 0, 0, 0)), "Equipe com conhecimento básico ou intérprete de Libras.", "fa-hands", "Libras" },
                    { 8, true, null, new DateTimeOffset(new DateTime(2026, 5, 3, 17, 26, 18, 197, DateTimeKind.Unspecified).AddTicks(3341), new TimeSpan(0, 0, 0, 0, 0)), "Local com baixo estímulo sonoro e visual (Amigável para TEA).", "fa-volume-xmark", "Ambiente Calmo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Categorias");

            migrationBuilder.RenameColumn(
                name: "Icone",
                table: "Categorias",
                newName: "UrlIcone");
        }
    }
}
