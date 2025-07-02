using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class sedBebidas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen",
                table: "Hamburguesa",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagenMimeType",
                table: "Hamburguesa",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bebida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ImagenMimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bebida", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Bebida",
                columns: new[] { "Id", "Imagen", "ImagenMimeType", "Nombre", "Precio" },
                values: new object[,]
                {
                    { 1, null, null, "Coca Cola", 800m },
                    { 2, null, null, "Sprite", 800m },
                    { 3, null, null, "Agua Mineral", 600m }
                });

            migrationBuilder.UpdateData(
                table: "Hamburguesa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Imagen", "ImagenMimeType" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Hamburguesa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Imagen", "ImagenMimeType" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Hamburguesa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Imagen", "ImagenMimeType" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bebida");

            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Hamburguesa");

            migrationBuilder.DropColumn(
                name: "ImagenMimeType",
                table: "Hamburguesa");
        }
    }
}
