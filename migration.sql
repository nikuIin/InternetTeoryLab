-- =====================================================
-- WineShop — SQL-скрипт для PostgreSQL
-- Запустить ВМЕСТО dotnet ef database update,
-- если миграции EF Core недоступны
-- =====================================================

CREATE TABLE IF NOT EXISTS "Wines" (
    "Id"            SERIAL PRIMARY KEY,
    "Name"          VARCHAR(200) NOT NULL,
    "WineType"      VARCHAR(50)  NOT NULL,
    "GrapeType"     VARCHAR(100) NOT NULL,
    "Year"          INTEGER      NOT NULL,
    "Alcohol"       DECIMAL(4,1) NOT NULL,
    "Price"         DECIMAL(10,2) NOT NULL,
    "ProducerEmail" VARCHAR(200),
    "ProducerPhone" VARCHAR(30),
    "ImageUrl"      VARCHAR(500),
    "Description"   TEXT,
    "SearchVector"  TEXT
);

CREATE TABLE IF NOT EXISTS "CartItems" (
    "Id"        SERIAL PRIMARY KEY,
    "WineId"    INTEGER NOT NULL REFERENCES "Wines"("Id"),
    "Quantity"  INTEGER NOT NULL DEFAULT 1,
    "SessionId" VARCHAR(200) NOT NULL
);

-- История миграций EF Core (нужна для совместимости с dotnet ef)
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId"    VARCHAR(150) NOT NULL PRIMARY KEY,
    "ProductVersion" VARCHAR(32)  NOT NULL
);

-- =====================================================
-- Тестовые данные (12 вин — покрывают все фильтры)
-- Красное: 3 записи (Каберне Совиньон, Пино Нуар, Мерло)
-- Белое: 3 записи (Шардоне x2, Рислинг, Совиньон Блан)
-- Игристое: 2 (Шардоне, Каберне Совиньон)
-- Розовое: 1 (Пино Нуар)
-- Десертное: 2 (Мускат, Рислинг)
-- Цены: 1800–6200 ₽  |  Годы: 2017–2023
-- =====================================================

INSERT INTO "Wines" ("Name","WineType","GrapeType","Year","Alcohol","Price","ImageUrl","Description")
VALUES
('Наследие мастера',   'Красное',    'Каберне Совиньон', 2019, 13.5, 3500, '/img/wine-products/wine-product1.webp', 'Элегантное красное вино с богатым букетом тёмных ягод и мягкими танинами.'),
('Коронационное',      'Красное',    'Пино Нуар',        2020, 12.5, 4200, '/img/wine-products/wine-product2.jpg',  'Изысканное вино с нотами вишни и земляники, лёгкой кислотностью.'),
('Брют Зеро',          'Игристое',   'Шардоне',          2021, 11.5, 2800, '/img/wine-products/wine-product3.jpg',  'Свежее игристое вино без добавления сахара, идеально к морепродуктам.'),
('Белый жемчуг',       'Белое',      'Шардоне',          2022, 12.0, 2200, '/img/wine-products/wine-product1.webp', 'Лёгкое белое вино с цветочными нотами и освежающей кислотностью.'),
('Розовый рассвет',    'Розовое',    'Пино Нуар',        2021, 11.0, 1900, '/img/wine-products/wine-product2.jpg',  'Нежное розовое вино с ароматами клубники и лепестков роз.'),
('Дворянское собрание','Красное',    'Каберне Совиньон', 2018, 14.0, 5500, '/img/wine-products/wine-product3.jpg',  'Выдержанное красное вино с оттенками ежевики, табака и ванили.'),
('Золотая осень',      'Десертное',  'Мускат',           2020, 15.5, 3100, '/img/wine-products/wine-product1.webp', 'Сладкое десертное вино с ароматом мёда, абрикоса и цитруса.'),
('Серебряный ключ',    'Белое',      'Рислинг',          2022, 10.5, 1800, '/img/wine-products/wine-product2.jpg',  'Полусухое белое с минеральным послевкусием и нотами зелёного яблока.'),
('Тёмная легенда',     'Красное',    'Мерло',            2017, 13.0, 6200, '/img/wine-products/wine-product3.jpg',  'Насыщенное выдержанное красное с нотками шоколада и сухофруктов.'),
('Горная свежесть',    'Белое',      'Совиньон Блан',    2023, 12.5, 2100, '/img/wine-products/wine-product1.webp', 'Яркое белое вино с ароматами крыжовника, грейпфрута и свежей травы.'),
('Рубиновая звезда',   'Игристое',   'Каберне Совиньон', 2021, 12.0, 3300, '/img/wine-products/wine-product2.jpg',  'Красное игристое с выраженными танинами и игривыми пузырьками.'),
('Лунный виноград',    'Десертное',  'Рислинг',          2019, 16.0, 4800, '/img/wine-products/wine-product3.jpg',  'Роскошное ледяное вино с концентрированными ароматами персика и мёда.')
ON CONFLICT DO NOTHING;
