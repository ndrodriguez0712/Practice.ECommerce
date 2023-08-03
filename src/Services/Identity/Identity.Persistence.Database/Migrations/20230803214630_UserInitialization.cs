using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class UserInitialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStatus",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Question = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SignUpDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    IdStatus = table.Column<int>(type: "int", nullable: false),
                    EmailVerification = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_UserRole_IdRole",
                        column: x => x.IdRole,
                        principalSchema: "User",
                        principalTable: "UserRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_UserStatus_IdStatus",
                        column: x => x.IdStatus,
                        principalSchema: "User",
                        principalTable: "UserStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "UserRole",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Full access", "Administrator" },
                    { 2, "Basic operations access", "Basic" },
                    { 3, "Limited access", "Visit" }
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "UserStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Inactive" },
                    { 3, "Pending" },
                    { 4, "PendingSend" },
                    { 5, "PendingConfirmation" },
                    { 6, "Rejected" }
                });

            migrationBuilder.InsertData(
                schema: "User",
                table: "User",
                columns: new[] { "Id", "Answer", "Email", "EmailVerification", "FirstName", "IdRole", "IdStatus", "LastLoginDate", "LastName", "Password", "Phone", "Question", "SignUpDate" },
                values: new object[] { 1, "asd", "nico.d.rodriguez@hotmail.com", true, "Nicolás D.", 1, 1, null, "Rodríguez", "ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f", "1122223333", "asd", new DateTime(2023, 8, 3, 18, 46, 30, 683, DateTimeKind.Local).AddTicks(9999) });

            migrationBuilder.CreateIndex(
                name: "IX_User_IdRole",
                schema: "User",
                table: "User",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdStatus",
                schema: "User",
                table: "User",
                column: "IdStatus");

            migrationBuilder.CreateIndex(
                name: "UQ__Email__6B0F5AE070734E4D",
                schema: "User",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User",
                schema: "User");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "User");

            migrationBuilder.DropTable(
                name: "UserStatus",
                schema: "User");
        }
    }
}
