using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeGestionDeTicketsAereos.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatFlightStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SeatFlight",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Disponible")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql(
                "UPDATE SeatFlight SET Status = CASE WHEN Available = 1 THEN 'Disponible' ELSE 'Reservado' END;");

            migrationBuilder.DropColumn(
                name: "Available",
                table: "SeatFlight");

            migrationBuilder.UpdateData(
                table: "ClientFareBundleDisplay",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BasicBodyMarkup", "ClassicBodyMarkup", "FlexBodyMarkup" },
                values: new object[] { "[bold]Incluye[/]\n[#db2777]✓[/] 1 artículo personal (bolso)\n[#db2777]✓[/] Acumula 3 millas por USD\n[grey]$ Equipaje de mano (10 kg) - Desde {{CARRYON}}[/]\n[grey]$ Equipaje de bodega (23 kg) - Desde {{CHECKED}}[/]\n[grey]$ Check-in en aeropuerto[/]\n[grey]$ Selección de asientos - Desde {{SEAT}}[/]\n[grey]$ Menú a bordo[/]\n[grey]$ Cambios antes del vuelo[/]\n[grey]✗ Reembolsos antes del vuelo[/]", "[bold]Incluye[/]\n[#6d28d9]✓[/] 1 artículo personal (bolso)\n[#6d28d9]✓[/] 1 equipaje de mano (10 kg)\n[#6d28d9]✓[/] 1 equipaje de bodega (23 kg)\n[#6d28d9]✓[/] Check-in en aeropuerto\n[#6d28d9]✓[/] Asiento Economy incluido\n[#6d28d9]✓[/] Acumula 6 millas por USD\n[grey]$ Menú a bordo[/]\n[grey]$ Cambios antes del vuelo[/]\n[grey]✗ Reembolsos antes del vuelo[/]", "[bold]Incluye[/]\n[#ea580c]✓[/] 1 artículo personal (bolso)\n[#ea580c]✓[/] 1 equipaje de mano (10 kg)\n[#ea580c]✓[/] 1 equipaje de bodega (23 kg)\n[#ea580c]✓[/] Check-in en aeropuerto\n[#ea580c]✓[/] Asiento Plus\n[#ea580c]✓[/] Acumula 8 millas por USD\n[#ea580c]✓[/] Cambios antes del vuelo\n[#ea580c]✓[/] Reembolsos antes del vuelo\n[grey]$ Menú a bordo[/]" });

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 1,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 2,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 3,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 4,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 5,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 6,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 7,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 8,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 9,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 10,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 11,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 12,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 13,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 14,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 15,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 16,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 17,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 18,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 19,
                column: "Status",
                value: "Disponible");

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 20,
                column: "Status",
                value: "Disponible");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "SeatFlight",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.Sql(
                "UPDATE SeatFlight SET Available = CASE WHEN Status = 'Disponible' THEN 1 ELSE 0 END;");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SeatFlight");

            migrationBuilder.UpdateData(
                table: "ClientFareBundleDisplay",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BasicBodyMarkup", "ClassicBodyMarkup", "FlexBodyMarkup" },
                values: new object[] { "[bold]Incluye[/]\r\n[#db2777]✓[/] 1 artículo personal (bolso)\r\n[#db2777]✓[/] Acumula 3 millas por USD\r\n[grey]$ Equipaje de mano (10 kg) - Desde {{CARRYON}}[/]\r\n[grey]$ Equipaje de bodega (23 kg) - Desde {{CHECKED}}[/]\r\n[grey]$ Check-in en aeropuerto[/]\r\n[grey]$ Selección de asientos - Desde {{SEAT}}[/]\r\n[grey]$ Menú a bordo[/]\r\n[grey]$ Cambios antes del vuelo[/]\r\n[grey]✗ Reembolsos antes del vuelo[/]", "[bold]Incluye[/]\r\n[#6d28d9]✓[/] 1 artículo personal (bolso)\r\n[#6d28d9]✓[/] 1 equipaje de mano (10 kg)\r\n[#6d28d9]✓[/] 1 equipaje de bodega (23 kg)\r\n[#6d28d9]✓[/] Check-in en aeropuerto\r\n[#6d28d9]✓[/] Asiento Economy incluido\r\n[#6d28d9]✓[/] Acumula 6 millas por USD\r\n[grey]$ Menú a bordo[/]\r\n[grey]$ Cambios antes del vuelo[/]\r\n[grey]✗ Reembolsos antes del vuelo[/]", "[bold]Incluye[/]\r\n[#ea580c]✓[/] 1 artículo personal (bolso)\r\n[#ea580c]✓[/] 1 equipaje de mano (10 kg)\r\n[#ea580c]✓[/] 1 equipaje de bodega (23 kg)\r\n[#ea580c]✓[/] Check-in en aeropuerto\r\n[#ea580c]✓[/] Asiento Plus\r\n[#ea580c]✓[/] Acumula 8 millas por USD\r\n[#ea580c]✓[/] Cambios antes del vuelo\r\n[#ea580c]✓[/] Reembolsos antes del vuelo\r\n[grey]$ Menú a bordo[/]" });

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 1,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 2,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 3,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 4,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 5,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 6,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 7,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 8,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 9,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 10,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 11,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 12,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 13,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 14,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 15,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 16,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 17,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 18,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 19,
                column: "Available",
                value: true);

            migrationBuilder.UpdateData(
                table: "SeatFlight",
                keyColumn: "IdSeatFlight",
                keyValue: 20,
                column: "Available",
                value: true);
        }
    }
}
