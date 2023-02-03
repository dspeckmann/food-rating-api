using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodRatingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodRatings_Picture_PictureId",
                table: "FoodRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Picture_PictureId",
                table: "Foods");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Picture_PictureId",
                table: "Pets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Picture",
                table: "Picture");

            migrationBuilder.RenameTable(
                name: "Picture",
                newName: "Pictures");

            migrationBuilder.RenameColumn(
                name: "DataString",
                table: "Pictures",
                newName: "OriginalFileName");

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                table: "Pictures",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PetId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_PetId",
                table: "Invitations",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodRatings_Pictures_PictureId",
                table: "FoodRatings",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Pictures_PictureId",
                table: "Foods",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Pictures_PictureId",
                table: "Pets",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodRatings_Pictures_PictureId",
                table: "FoodRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Pictures_PictureId",
                table: "Foods");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Pictures_PictureId",
                table: "Pets");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                table: "Pictures");

            migrationBuilder.RenameTable(
                name: "Pictures",
                newName: "Picture");

            migrationBuilder.RenameColumn(
                name: "OriginalFileName",
                table: "Picture",
                newName: "DataString");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Picture",
                table: "Picture",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodRatings_Picture_PictureId",
                table: "FoodRatings",
                column: "PictureId",
                principalTable: "Picture",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Picture_PictureId",
                table: "Foods",
                column: "PictureId",
                principalTable: "Picture",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Picture_PictureId",
                table: "Pets",
                column: "PictureId",
                principalTable: "Picture",
                principalColumn: "Id");
        }
    }
}
