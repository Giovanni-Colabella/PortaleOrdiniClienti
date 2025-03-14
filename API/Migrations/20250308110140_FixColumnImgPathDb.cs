using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnImgPathDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Immagine",
                table: "Prodotti");

            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "Prodotti",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "Prodotti");

            migrationBuilder.AddColumn<byte[]>(
                name: "Immagine",
                table: "Prodotti",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
