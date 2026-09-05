using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTLCRISTALVK18BACK.Migrations
{
    public partial class Init : Migration
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
                    TurnoUserMTL = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitaciones", x => x.IdHbtn);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    IdResv = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numresv = table.Column<int>(type: "int", nullable: false),
                    Estadoresv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tiporesv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tiemporentresv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diasemofinresv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Precioresv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Statushabresv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Statuspagohabresv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Limpiezahabresv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Horarentaresv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Folioordenresv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcargoUserMTL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TurnoUserMTL = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.IdResv);
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
                name: "Tipo1Cliente",
                columns: table => new
                {
                    IdClte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusingresoCl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutPlacasCl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutMarcaCl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutColorCl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrecuenciaCl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdvertCl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumHabCl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEntradaCl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaSalidaCl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalConsumos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AcargoUserMTL1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservasIdResv = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo1Cliente", x => x.IdClte);
                    table.ForeignKey(
                        name: "FK_Tipo1Cliente_Reservas_ReservasIdResv",
                        column: x => x.ReservasIdResv,
                        principalTable: "Reservas",
                        principalColumn: "IdResv",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tipo2Consumos",
                columns: table => new
                {
                    IdCsms = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cantidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrecioUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusPagado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Totalpagado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Totalconsumos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcargoUserMTL2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservasIdResv = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo2Consumos", x => x.IdCsms);
                    table.ForeignKey(
                        name: "FK_Tipo2Consumos_Reservas_ReservasIdResv",
                        column: x => x.ReservasIdResv,
                        principalTable: "Reservas",
                        principalColumn: "IdResv",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tipo1Cliente_ReservasIdResv",
                table: "Tipo1Cliente",
                column: "ReservasIdResv");

            migrationBuilder.CreateIndex(
                name: "IX_Tipo2Consumos_ReservasIdResv",
                table: "Tipo2Consumos",
                column: "ReservasIdResv");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Habitaciones");

            migrationBuilder.DropTable(
                name: "Tipo1Cliente");

            migrationBuilder.DropTable(
                name: "Tipo2Consumos");

            migrationBuilder.DropTable(
                name: "Usersadmin");

            migrationBuilder.DropTable(
                name: "Reservas");
        }
    }
}
