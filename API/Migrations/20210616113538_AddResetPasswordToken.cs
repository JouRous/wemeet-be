using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddResetPasswordToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForgetToken",
                table: "AspNetUsers",
                newName: "ResetPasswordToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetPasswordToken",
                table: "AspNetUsers",
                newName: "ForgetToken");
        }
    }
}
