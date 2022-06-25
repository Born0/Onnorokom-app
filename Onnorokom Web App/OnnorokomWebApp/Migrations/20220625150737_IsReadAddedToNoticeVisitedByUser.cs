using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnnorokomWebApp.Migrations
{
    public partial class IsReadAddedToNoticeVisitedByUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "NoticesVisitedByUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "NoticesVisitedByUser");
        }
    }
}
