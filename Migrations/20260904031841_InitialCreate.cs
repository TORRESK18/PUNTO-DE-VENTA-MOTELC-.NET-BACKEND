using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTLCRISTALVK18BACK.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Habitaciones",
                columns: table => new
                {
                    IdHbtn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numhab = table.Column<int>(type: "int", nullable: false),
                    Estadohab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipohab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tiemporenthab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diasemofinhab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preciohab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Statushab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Limpiezahab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Albercahab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Jacuzzihab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoCamahab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Folioordenhab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcargoUserMTL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TurnoUserMTL = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitaciones", x => x.IdHbtn);
                });

            migrationBuilder.CreateTable(
                name: "Usersadmin",
                columns: table => new
                {
                    IdUs = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NtrabajadorUs = table.Column<int>(type: "int", nullable: false),
                    NombreUs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PuestoUs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartamentoUs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsernameUs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailUs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordUs = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usersadmin", x => x.IdUs);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    IdResv = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numresv = table.Column<int>(type: "int", nullable: false),
                    Folioordenresv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdHbtn = table.Column<int>(type: "int", nullable: false),
                    Estadoresv = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Tiporesv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaHoraEntrada = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FechaHoraSalidaProgramada = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FechaHoraSalidaReal = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TiempoRentaMinutos = table.Column<int>(type: "int", nullable: false),
                    Precioresv = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalConsumos = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalReserva = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Statuspagohabresv = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IdUsRegistro = table.Column<int>(type: "int", nullable: false),
                    IdUsCierre = table.Column<int>(type: "int", nullable: true),
                    TurnoUserMTL = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.IdResv);
                    table.ForeignKey(
                        name: "FK_Reservas_Habitaciones_IdHbtn",
                        column: x => x.IdHbtn,
                        principalTable: "Habitaciones",
                        principalColumn: "IdHbtn",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Usersadmin_IdUsCierre",
                        column: x => x.IdUsCierre,
                        principalTable: "Usersadmin",
                        principalColumn: "IdUs",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservas_Usersadmin_IdUsRegistro",
                        column: x => x.IdUsRegistro,
                        principalTable: "Usersadmin",
                        principalColumn: "IdUs",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservaClientes",
                columns: table => new
                {
                    IdClte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdResv = table.Column<int>(type: "int", nullable: false),
                    StatusingresoCl = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AutPlacasCl = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AutMarcaCl = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    AutModeloCl = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    AutColorCl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FrecuenciaCl = table.Column<int>(type: "int", nullable: false),
                    AdvertCl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaClientes", x => x.IdClte);
                    table.ForeignKey(
                        name: "FK_ReservaClientes_Reservas_IdResv",
                        column: x => x.IdResv,
                        principalTable: "Reservas",
                        principalColumn: "IdResv",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservaConsumos",
                columns: table => new
                {
                    IdCsms = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdResv = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PrecioUnit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalLinea = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EstadoPedido = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    StatusPagado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaSolicitud = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FechaEntrega = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IdUsRegistro = table.Column<int>(type: "int", nullable: false),
                    IdUsEntrega = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaConsumos", x => x.IdCsms);
                    table.ForeignKey(
                        name: "FK_ReservaConsumos_Reservas_IdResv",
                        column: x => x.IdResv,
                        principalTable: "Reservas",
                        principalColumn: "IdResv",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaConsumos_Usersadmin_IdUsEntrega",
                        column: x => x.IdUsEntrega,
                        principalTable: "Usersadmin",
                        principalColumn: "IdUs",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservaConsumos_Usersadmin_IdUsRegistro",
                        column: x => x.IdUsRegistro,
                        principalTable: "Usersadmin",
                        principalColumn: "IdUs",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservaClientes_IdResv",
                table: "ReservaClientes",
                column: "IdResv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReservaConsumos_IdResv_EstadoPedido",
                table: "ReservaConsumos",
                columns: new[] { "IdResv", "EstadoPedido" });

            migrationBuilder.CreateIndex(
                name: "IX_ReservaConsumos_IdUsEntrega",
                table: "ReservaConsumos",
                column: "IdUsEntrega");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaConsumos_IdUsRegistro",
                table: "ReservaConsumos",
                column: "IdUsRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_Estadoresv",
                table: "Reservas",
                column: "Estadoresv");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdHbtn",
                table: "Reservas",
                column: "IdHbtn");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdUsCierre",
                table: "Reservas",
                column: "IdUsCierre");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdUsRegistro",
                table: "Reservas",
                column: "IdUsRegistro");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservaClientes");

            migrationBuilder.DropTable(
                name: "ReservaConsumos");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Habitaciones");

            migrationBuilder.DropTable(
                name: "Usersadmin");
        }
    }
}
