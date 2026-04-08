using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFitness2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberEntityAndRefactorRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_AspNetUsers_UserId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_MembershipPlans_MembershipPlanId",
                table: "Memberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId_GymClassId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Memberships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemberEntityId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships",
                column: "Guid");

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MembershipGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Memberships_MembershipGuid",
                        column: x => x.MembershipGuid,
                        principalTable: "Memberships",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MemberId",
                table: "Memberships",
                column: "MemberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MemberEntityId",
                table: "Bookings",
                column: "MemberEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MemberId_GymClassId",
                table: "Bookings",
                columns: new[] { "MemberId", "GymClassId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_ApplicationUserId",
                table: "Members",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_MembershipGuid",
                table: "Members",
                column: "MembershipGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Members_MemberEntityId",
                table: "Bookings",
                column: "MemberEntityId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_MembershipPlans_MembershipPlanId",
                table: "Memberships",
                column: "MembershipPlanId",
                principalTable: "MembershipPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Members_MemberEntityId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_MembershipPlans_MembershipPlanId",
                table: "Memberships");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_MemberId",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_MemberEntityId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_MemberId_GymClassId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "MemberEntityId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Memberships",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Memberships",
                table: "Memberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId_GymClassId",
                table: "Bookings",
                columns: new[] { "UserId", "GymClassId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_AspNetUsers_UserId",
                table: "Memberships",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_MembershipPlans_MembershipPlanId",
                table: "Memberships",
                column: "MembershipPlanId",
                principalTable: "MembershipPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
