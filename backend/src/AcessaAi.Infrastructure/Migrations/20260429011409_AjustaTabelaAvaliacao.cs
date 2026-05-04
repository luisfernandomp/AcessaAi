using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcessaAi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustaTabelaAvaliacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Estabelecimentos_EstabelecimentoId",
                table: "Avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Usuarios_UsuarioId",
                table: "Avaliacoes");

            migrationBuilder.AlterColumn<int>(
                name: "EstabelecimentoId",
                table: "Avaliacoes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Estabelecimentos_EstabelecimentoId",
                table: "Avaliacoes",
                column: "EstabelecimentoId",
                principalTable: "Estabelecimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Usuarios_UsuarioId",
                table: "Avaliacoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Estabelecimentos_EstabelecimentoId",
                table: "Avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Usuarios_UsuarioId",
                table: "Avaliacoes");

            migrationBuilder.AlterColumn<int>(
                name: "EstabelecimentoId",
                table: "Avaliacoes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Estabelecimentos_EstabelecimentoId",
                table: "Avaliacoes",
                column: "EstabelecimentoId",
                principalTable: "Estabelecimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Usuarios_UsuarioId",
                table: "Avaliacoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
