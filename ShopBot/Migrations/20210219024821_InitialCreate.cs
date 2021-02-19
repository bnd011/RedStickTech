using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopBot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin_info",
                columns: table => new
                {
                    Admin_email = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Admin_position = table.Column<string>(type: "varchar(25)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Admin_email);
                });

            migrationBuilder.CreateTable(
                name: "failed_login",
                columns: table => new
                {
                    user_email = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    failed_num = table.Column<int>(type: "int", nullable: false, defaultValueSql: "'1'"),
                    Time_of_try = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.user_email);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    schedule_IDN = table.Column<int>(type: "integer", nullable: false),
                    user_email = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    archived = table.Column<ulong>(type: "bit(1)", nullable: true, defaultValueSql: "b'0'"),
                    url = table.Column<string>(type: "varchar(200)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_recurring = table.Column<ulong>(type: "bit(1)", nullable: true, defaultValueSql: "b'0'"),
                    frequency = table.Column<int>(type: "integer", nullable: true, defaultValueSql: "'0'"),
                    want_option = table.Column<int>(type: "integer", nullable: true, defaultValueSql: "'-1'"),
                    price_limit = table.Column<int>(type: "integer", nullable: true),
                    expire_date = table.Column<DateTime>(type: "date", nullable: true),
                    current_price = table.Column<float>(type: "float", nullable: false),
                    item = table.Column<string>(type: "varchar(200)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.schedule_IDN);
                });

            migrationBuilder.CreateTable(
                name: "user_login_info",
                columns: table => new
                {
                    user_email = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    verify = table.Column<string>(type: "varchar(12)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    salt = table.Column<string>(type: "varchar(65)", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    emailVerified = table.Column<ulong>(type: "bit(1)", nullable: true, defaultValueSql: "b'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.user_email);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_info");

            migrationBuilder.DropTable(
                name: "failed_login");

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.DropTable(
                name: "user_login_info");
        }
    }
}
