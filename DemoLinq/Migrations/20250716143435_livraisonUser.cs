using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoLinq.Migrations
{
    /// <inheritdoc />
    public partial class livraisonUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Livraisons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Livraisons_UserId",
                table: "Livraisons",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livraisons_AspNetUsers_UserId",
                table: "Livraisons",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livraisons_AspNetUsers_UserId",
                table: "Livraisons");

            migrationBuilder.DropIndex(
                name: "IX_Livraisons_UserId",
                table: "Livraisons");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Livraisons");
        }
    }
}
