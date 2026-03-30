using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFitness2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MembershipPlanId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MembershipPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipPlanType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    MonthlyClassLimit = table.Column<int>(type: "int", nullable: false),
                    FreeTrialWeeks = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembershipPlanFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    MembershipPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPlanFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipPlanFeatures_MembershipPlans_MembershipPlanId",
                        column: x => x.MembershipPlanId,
                        principalTable: "MembershipPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MembershipPlanId",
                table: "AspNetUsers",
                column: "MembershipPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPlanFeatures_MembershipPlanId",
                table: "MembershipPlanFeatures",
                column: "MembershipPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MembershipPlans_MembershipPlanId",
                table: "AspNetUsers",
                column: "MembershipPlanId",
                principalTable: "MembershipPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MembershipPlans_MembershipPlanId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MembershipPlanFeatures");

            migrationBuilder.DropTable(
                name: "MembershipPlans");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MembershipPlanId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MembershipPlanId",
                table: "AspNetUsers");
        }
    }
}
