using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace News.Vampire.Service.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    isActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(maxLength: 16, nullable: true),
                    Password = table.Column<string>(maxLength: 64, nullable: true),
                    Role = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<long>(nullable: false),
                    Url = table.Column<string>(maxLength: 256, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    UpdateFrequencyMinutes = table.Column<int>(nullable: false),
                    NextLoadedTime = table.Column<long>(nullable: false),
                    GroupId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sources_Groups_GroupId1",
                        column: x => x.GroupId1,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewsItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 512, nullable: true),
                    Url = table.Column<List<string>>(type: "jsonb", maxLength: 2048, nullable: true),
                    Description = table.Column<string>(maxLength: 4096, nullable: true),
                    Category = table.Column<List<string>>(type: "jsonb", maxLength: 512, nullable: true),
                    PublicationDate = table.Column<long>(nullable: false),
                    TimeStamp = table.Column<long>(nullable: false),
                    ExternalId = table.Column<string>(nullable: true),
                    Author = table.Column<List<string>>(type: "jsonb", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsItem_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UserGroupId = table.Column<int>(nullable: true),
                    LastLoadedTime = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "Name", "isActive" },
                values: new object[,]
                {
                    { 1, "Корреспондент", true },
                    { 2, "Habr", true },
                    { 3, "News.RU", true }
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "Description", "GroupId", "GroupId1", "Name", "NextLoadedTime", "Sort", "UpdateFrequencyMinutes", "Url" },
                values: new object[,]
                {
                    { 1, null, 1L, null, "Последние новости по разделу События в Украине", 0L, 0, 0, "http://k.img.com.ua/rss/ru/events.xml" },
                    { 2, null, 1L, null, "Автомобили", 0L, 0, 0, "http://k.img.com.ua/rss/ru/motors.xml" },
                    { 3, null, 1L, null, "Новости кино", 0L, 0, 0, "http://k.img.com.ua/rss/ru/cinema.xml" },
                    { 4, null, 1L, null, "Технологии", 0L, 0, 0, "http://k.img.com.ua/rss/ru/technews.xml" },
                    { 5, null, 1L, null, "Космос", 0L, 0, 0, "http://k.img.com.ua/rss/ru/space.xml" },
                    { 6, null, 2L, null, "HABR. Все публикации в потоке Разработка", 0L, 0, 0, "https://habr.com/ru/rss/flows/develop/all/?fl=rul" },
                    { 7, null, 2L, null, "NEWSru.com :: Мнения", 0L, 0, 0, "https://rss.newsru.com/blog" }
                });

            migrationBuilder.InsertData(
                table: "UserGroups",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "My Group" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[] { 1, "Dmitry", null, null });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "LastLoadedTime", "SourceId", "UserGroupId", "UserId" },
                values: new object[,]
                {
                    { 3, null, 6, null, 1 },
                    { 4, null, 7, null, 1 },
                    { 1, null, 1, 1, 1 },
                    { 2, null, 2, 1, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsItem_SourceId",
                table: "NewsItem",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_GroupId1",
                table: "Sources",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SourceId",
                table: "Subscriptions",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserGroupId",
                table: "Subscriptions",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsItem");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
