using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcessaAi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaUrlFotoPerfilEmUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlFotoPerfil",
                table: "Usuarios",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlFotoPerfil",
                table: "Usuarios");
        }
    }
}
