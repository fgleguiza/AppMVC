using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.Migrations
{
    /// <inheritdoc />
    public partial class seddUsuarioAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Contrasena", "Email", "FechaNacimiento", "NombreCompleto", "Telefono" },
                values: new object[] { 1, "123456", "fgleguiza2001@gmail.com", new DateTime(2001, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Facuno Gerardo Leguiza", "1138349103" }
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
