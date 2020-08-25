using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPeeps.Services.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhoneBooks",
                columns: table => new
                {
                    PhoneBookId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneBooks", x => x.PhoneBookId);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    PhoneBookId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contacts_PhoneBooks_PhoneBookId",
                        column: x => x.PhoneBookId,
                        principalTable: "PhoneBooks",
                        principalColumn: "PhoneBookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PhoneBooks",
                columns: new[] { "PhoneBookId", "Name" },
                values: new object[,]
                {
                    { 1, "PhoneBook1" },
                    { 2, "PhoneBook2" },
                    { 3, "PhoneBook3" },
                    { 4, "PhoneBook4" },
                    { 5, "PhoneBook5" }
                });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "ContactId", "Name", "Number", "PhoneBookId" },
                values: new object[,]
                {
                    { 1, "Contact1", "1111111111", 1 },
                    { 2, "Contact2", "2222222222", 1 },
                    { 3, "Contact3", "3333333333", 1 },
                    { 4, "Contact4", "4444444444", 2 },
                    { 5, "Contact5", "5555555555", 2 },
                    { 6, "Contact6", "6666666666", 2 },
                    { 7, "Contact7", "7777777777", 3 },
                    { 8, "Contact8", "8888888888", 3 },
                    { 9, "Contact9", "9999999999", 3 },
                    { 10, "Contact10", "10101010", 4 },
                    { 11, "Contact11", "111111111", 4 },
                    { 12, "Contact12", "121212121", 4 },
                    { 13, "Contact13", "131313131", 5 },
                    { 14, "Contact14", "141414141", 5 },
                    { 15, "Contact15", "151515151", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PhoneBookId",
                table: "Contacts",
                column: "PhoneBookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "PhoneBooks");
        }
    }
}
