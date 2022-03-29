using Microsoft.EntityFrameworkCore.Migrations;

namespace MTRS.DAL.Migrations
{
    public partial class OpportunityId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OpportunityId",
                table: "TimeSheetDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpportunityId",
                table: "TimeSheetDetails");
        }
    }
}
