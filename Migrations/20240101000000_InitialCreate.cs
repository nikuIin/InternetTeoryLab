using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814

namespace WineShop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    WineType = table.Column<string>(type: "text", nullable: false),
                    GrapeType = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Alcohol = table.Column<decimal>(type: "decimal(4,1)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ProducerEmail = table.Column<string>(type: "text", nullable: true),
                    ProducerPhone = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SearchVector = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WineId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    SessionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Wines_WineId",
                        column: x => x.WineId,
                        principalTable: "Wines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_WineId",
                table: "CartItems",
                column: "WineId");

            // Seed data
            migrationBuilder.InsertData("Wines",
                new[] { "Name","WineType","GrapeType","Year","Alcohol","Price","ImageUrl","Description" },
                new object[,]
                {
                    { "Наследие мастера",    "Красное",   "Каберне Совиньон", 2019, 13.5m, 3500m, "/img/wine-products/wine-product1.webp", "Элегантное красное вино с богатым букетом тёмных ягод и мягкими танинами." },
                    { "Коронационное",       "Красное",   "Пино Нуар",        2020, 12.5m, 4200m, "/img/wine-products/wine-product2.jpg",  "Изысканное вино с нотами вишни и земляники, лёгкой кислотностью." },
                    { "Брют Зеро",           "Игристое",  "Шардоне",          2021, 11.5m, 2800m, "/img/wine-products/wine-product3.jpg",  "Свежее игристое вино без добавления сахара, идеально к морепродуктам." },
                    { "Белый жемчуг",        "Белое",     "Шардоне",          2022, 12.0m, 2200m, "/img/wine-products/wine-product1.webp", "Лёгкое белое вино с цветочными нотами и освежающей кислотностью." },
                    { "Розовый рассвет",     "Розовое",   "Пино Нуар",        2021, 11.0m, 1900m, "/img/wine-products/wine-product2.jpg",  "Нежное розовое вино с ароматами клубники и лепестков роз." },
                    { "Дворянское собрание", "Красное",   "Каберне Совиньон", 2018, 14.0m, 5500m, "/img/wine-products/wine-product3.jpg",  "Выдержанное красное вино с оттенками ежевики, табака и ванили." },
                    { "Золотая осень",       "Десертное", "Мускат",           2020, 15.5m, 3100m, "/img/wine-products/wine-product1.webp", "Сладкое десертное вино с ароматом мёда, абрикоса и цитруса." },
                    { "Серебряный ключ",     "Белое",     "Рислинг",          2022, 10.5m, 1800m, "/img/wine-products/wine-product2.jpg",  "Полусухое белое с минеральным послевкусием и нотами зелёного яблока." },
                    { "Тёмная легенда",      "Красное",   "Мерло",            2017, 13.0m, 6200m, "/img/wine-products/wine-product3.jpg",  "Насыщенное выдержанное красное с нотками шоколада и сухофруктов." },
                    { "Горная свежесть",     "Белое",     "Совиньон Блан",    2023, 12.5m, 2100m, "/img/wine-products/wine-product1.webp", "Яркое белое вино с ароматами крыжовника, грейпфрута и свежей травы." },
                    { "Рубиновая звезда",    "Игристое",  "Каберне Совиньон", 2021, 12.0m, 3300m, "/img/wine-products/wine-product2.jpg",  "Красное игристое с выраженными танинами и игривыми пузырьками." },
                    { "Лунный виноград",     "Десертное", "Рислинг",          2019, 16.0m, 4800m, "/img/wine-products/wine-product3.jpg",  "Роскошное ледяное вино с концентрированными ароматами персика и мёда." },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "CartItems");
            migrationBuilder.DropTable(name: "Wines");
        }
    }
}
