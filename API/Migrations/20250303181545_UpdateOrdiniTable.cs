using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrdiniTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotaleOrdine",
                table: "Ordini",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Ordini",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ordini_ClienteId",
                table: "Ordini",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ordini_Clienti_ClienteId",
                table: "Ordini",
                column: "ClienteId",
                principalTable: "Clienti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ordini_Clienti_ClienteId",
                table: "Ordini");

            migrationBuilder.DropIndex(
                name: "IX_Ordini_ClienteId",
                table: "Ordini");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Ordini");

            migrationBuilder.AlterColumn<string>(
                name: "TotaleOrdine",
                table: "Ordini",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
