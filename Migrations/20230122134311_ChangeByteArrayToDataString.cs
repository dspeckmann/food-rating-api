using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodRatingApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeByteArrayToDataString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Picture");

            migrationBuilder.AddColumn<string>(
                name: "DataString",
                table: "Picture",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataString",
                table: "Picture");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "Picture",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
