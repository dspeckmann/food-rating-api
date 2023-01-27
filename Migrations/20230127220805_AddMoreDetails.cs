using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodRatingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Pets_PetId",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_PetId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "FoodRatings");

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                table: "Pets",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Foods",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Foods",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Foods",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Foods",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "FoodRatings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                table: "FoodRatings",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Taste",
                table: "FoodRatings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Wellbeing",
                table: "FoodRatings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FoodRatingPet",
                columns: table => new
                {
                    FoodRatingsId = table.Column<Guid>(type: "uuid", nullable: false),
                    PetsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodRatingPet", x => new { x.FoodRatingsId, x.PetsId });
                    table.ForeignKey(
                        name: "FK_FoodRatingPet_FoodRatings_FoodRatingsId",
                        column: x => x.FoodRatingsId,
                        principalTable: "FoodRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodRatingPet_Pets_PetsId",
                        column: x => x.PetsId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_PictureId",
                table: "Pets",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodRatings_PictureId",
                table: "FoodRatings",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodRatingPet_PetsId",
                table: "FoodRatingPet",
                column: "PetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodRatings_Picture_PictureId",
                table: "FoodRatings",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodRatings_Picture_PictureId",
                table: "FoodRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Picture_PictureId",
                table: "Pets");

            migrationBuilder.DropTable(
                name: "FoodRatingPet");

            migrationBuilder.DropIndex(
                name: "IX_Pets_PictureId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_FoodRatings_PictureId",
                table: "FoodRatings");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "FoodRatings");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "FoodRatings");

            migrationBuilder.DropColumn(
                name: "Taste",
                table: "FoodRatings");

            migrationBuilder.DropColumn(
                name: "Wellbeing",
                table: "FoodRatings");

            migrationBuilder.AddColumn<Guid>(
                name: "PetId",
                table: "Foods",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "FoodRatings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Foods_PetId",
                table: "Foods",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Pets_PetId",
                table: "Foods",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");
        }
    }
}
