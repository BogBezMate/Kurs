using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kurs.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID_Customers = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Last_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    First_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Middle_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID_Customers);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID_Employees = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Last_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    First_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Middle_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID_Employees);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    ID_Suppliers = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Contact_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.ID_Suppliers);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    ID_Vehicles = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Customers = table.Column<int>(type: "int", nullable: false),
                    VIN = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    Plate_Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.ID_Vehicles);
                    table.ForeignKey(
                        name: "FK_Vehicles_Customers_ID_Customers",
                        column: x => x.ID_Customers,
                        principalTable: "Customers",
                        principalColumn: "ID_Customers",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID_Users = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Employees = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID_Users);
                    table.ForeignKey(
                        name: "FK_Users_Employees_ID_Employees",
                        column: x => x.ID_Employees,
                        principalTable: "Employees",
                        principalColumn: "ID_Employees",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    ID_Parts = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Suppliers = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.ID_Parts);
                    table.ForeignKey(
                        name: "FK_Parts_Suppliers_ID_Suppliers",
                        column: x => x.ID_Suppliers,
                        principalTable: "Suppliers",
                        principalColumn: "ID_Suppliers",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    ID_WorkOrders = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ID_Customers = table.Column<int>(type: "int", nullable: false),
                    ID_Vehicles = table.Column<int>(type: "int", nullable: false),
                    Date_Open = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Close = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Открыт"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.ID_WorkOrders);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Customers_ID_Customers",
                        column: x => x.ID_Customers,
                        principalTable: "Customers",
                        principalColumn: "ID_Customers",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Vehicles_ID_Vehicles",
                        column: x => x.ID_Vehicles,
                        principalTable: "Vehicles",
                        principalColumn: "ID_Vehicles",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderEmployees",
                columns: table => new
                {
                    ID_WorkOrderEmployees = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_WorkOrders = table.Column<int>(type: "int", nullable: false),
                    ID_Employees = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderEmployees", x => x.ID_WorkOrderEmployees);
                    table.ForeignKey(
                        name: "FK_WorkOrderEmployees_Employees_ID_Employees",
                        column: x => x.ID_Employees,
                        principalTable: "Employees",
                        principalColumn: "ID_Employees",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrderEmployees_WorkOrders_ID_WorkOrders",
                        column: x => x.ID_WorkOrders,
                        principalTable: "WorkOrders",
                        principalColumn: "ID_WorkOrders",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderParts",
                columns: table => new
                {
                    ID_WorkOrderParts = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_WorkOrders = table.Column<int>(type: "int", nullable: false),
                    ID_Parts = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Unit_Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderParts", x => x.ID_WorkOrderParts);
                    table.ForeignKey(
                        name: "FK_WorkOrderParts_Parts_ID_Parts",
                        column: x => x.ID_Parts,
                        principalTable: "Parts",
                        principalColumn: "ID_Parts",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrderParts_WorkOrders_ID_WorkOrders",
                        column: x => x.ID_WorkOrders,
                        principalTable: "WorkOrders",
                        principalColumn: "ID_WorkOrders",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderServices",
                columns: table => new
                {
                    ID_WorkOrderServices = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_WorkOrders = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderServices", x => x.ID_WorkOrderServices);
                    table.ForeignKey(
                        name: "FK_WorkOrderServices_WorkOrders_ID_WorkOrders",
                        column: x => x.ID_WorkOrders,
                        principalTable: "WorkOrders",
                        principalColumn: "ID_WorkOrders",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parts_ID_Suppliers",
                table: "Parts",
                column: "ID_Suppliers");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ID_Employees",
                table: "Users",
                column: "ID_Employees",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ID_Customers",
                table: "Vehicles",
                column: "ID_Customers");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VIN",
                table: "Vehicles",
                column: "VIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderEmployees_ID_Employees",
                table: "WorkOrderEmployees",
                column: "ID_Employees");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderEmployees_ID_WorkOrders_ID_Employees",
                table: "WorkOrderEmployees",
                columns: new[] { "ID_WorkOrders", "ID_Employees" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderParts_ID_Parts",
                table: "WorkOrderParts",
                column: "ID_Parts");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderParts_ID_WorkOrders",
                table: "WorkOrderParts",
                column: "ID_WorkOrders");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ID_Customers",
                table: "WorkOrders",
                column: "ID_Customers");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ID_Vehicles",
                table: "WorkOrders",
                column: "ID_Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_Number",
                table: "WorkOrders",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderServices_ID_WorkOrders",
                table: "WorkOrderServices",
                column: "ID_WorkOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorkOrderEmployees");

            migrationBuilder.DropTable(
                name: "WorkOrderParts");

            migrationBuilder.DropTable(
                name: "WorkOrderServices");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
