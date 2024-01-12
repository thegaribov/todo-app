using Microsoft.EntityFrameworkCore.Migrations;
using TodoListApp.Persistance.Entities.Enums;

#nullable disable

namespace TodoListApp.Migrations
{
    public partial class User_Roles_Default : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"UPDATE \"Users\"\r\nSET \"Role\" = @{(int)Role.User}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"UPDATE \"Users\"\r\nSET \"Role\" = 0");
        }
    }
}
