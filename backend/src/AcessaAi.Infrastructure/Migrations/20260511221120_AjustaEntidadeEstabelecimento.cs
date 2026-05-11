using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AcessaAi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustaEntidadeEstabelecimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Estrelas",
                table: "Avaliacoes",
                newName: "QuantidadeEstrelas");

            migrationBuilder.AddColumn<bool>(
                name: "CadastradoRecente",
                table: "Estabelecimentos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MediaEstrelas",
                table: "Estabelecimentos",
                type: "double precision",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "EstabelecimentoFotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    EstabelecimentoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstabelecimentoFotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstabelecimentoFotos_Estabelecimentos_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstabelecimentoRecursosAcessibilidade",
                columns: table => new
                {
                    EstabelecimentoId = table.Column<int>(type: "integer", nullable: false),
                    RecursosAcessibilidadeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstabelecimentoRecursosAcessibilidade", x => new { x.EstabelecimentoId, x.RecursosAcessibilidadeId });
                    table.ForeignKey(
                        name: "FK_EstabelecimentoRecursosAcessibilidade_Categorias_RecursosAc~",
                        column: x => x.RecursosAcessibilidadeId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstabelecimentoRecursosAcessibilidade_Estabelecimentos_Esta~",
                        column: x => x.EstabelecimentoId,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstabelecimentoFotos_EstabelecimentoId",
                table: "EstabelecimentoFotos",
                column: "EstabelecimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstabelecimentoRecursosAcessibilidade_RecursosAcessibilidad~",
                table: "EstabelecimentoRecursosAcessibilidade",
                column: "RecursosAcessibilidadeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstabelecimentoFotos");

            migrationBuilder.DropTable(
                name: "EstabelecimentoRecursosAcessibilidade");

            migrationBuilder.DropColumn(
                name: "CadastradoRecente",
                table: "Estabelecimentos");

            migrationBuilder.DropColumn(
                name: "MediaEstrelas",
                table: "Estabelecimentos");

            migrationBuilder.RenameColumn(
                name: "QuantidadeEstrelas",
                table: "Avaliacoes",
                newName: "Estrelas");
        }
    }
}
