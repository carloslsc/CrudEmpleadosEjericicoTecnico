using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrudEmpleadosEjercicioTecnico.Migrations
{
    public partial class MigrationInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    IdEmpleado = table.Column<Guid>(nullable: false),
                    NombreEmpleado = table.Column<string>(maxLength: 50, nullable: false),
                    ApellidosEmpleado = table.Column<string>(maxLength: 50, nullable: false),
                    EmailEmpleado = table.Column<string>(nullable: false),
                    TelefonoEmpleado = table.Column<string>(nullable: false),
                    FecNacEmpleado = table.Column<DateTime>(nullable: false),
                    SalarioEmpleado = table.Column<decimal>(type: "money", nullable: false),
                    EmpresaEmpleado = table.Column<string>(nullable: false),
                    FotoEmpleado = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado", x => x.IdEmpleado);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empleado");
        }
    }
}
