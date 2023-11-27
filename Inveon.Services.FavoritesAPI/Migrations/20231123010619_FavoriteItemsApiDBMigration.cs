using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inveon.Services.FavoritesAPI.Migrations
{
    /// <inheritdoc />
    public partial class FavoriteItemsApiDBMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteItems",
                columns: table => new
                {
                    FavoriteItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteItems", x => x.FavoriteItemID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteItems");
        }
    }
}
