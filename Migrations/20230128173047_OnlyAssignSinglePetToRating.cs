using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodRatingApi.Migrations
{
    /// <inheritdoc />
    public partial class OnlyAssignSinglePetToRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodRatingPet");

            migrationBuilder.AddColumn<Guid>(
                name: "PetId",
                table: "FoodRatings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodRatings_PetId",
                table: "FoodRatings",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodRatings_Pets_PetId",
                table: "FoodRatings",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodRatings_Pets_PetId",
                table: "FoodRatings");

            migrationBuilder.DropIndex(
                name: "IX_FoodRatings_PetId",
                table: "FoodRatings");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "FoodRatings");

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
                name: "IX_FoodRatingPet_PetsId",
                table: "FoodRatingPet",
                column: "PetsId");
        }
    }
}
