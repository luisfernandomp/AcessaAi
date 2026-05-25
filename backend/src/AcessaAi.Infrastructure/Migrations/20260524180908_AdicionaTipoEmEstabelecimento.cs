using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcessaAi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaTipoEmEstabelecimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Estabelecimentos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Restaurante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Estabelecimentos");
        }
    }
}
