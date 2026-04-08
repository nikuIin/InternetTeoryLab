// Переключение вкладок
document
  .getElementById("wineTab")
  .addEventListener("click", () => switchTab("wine"));
document
  .getElementById("articleTab")
  .addEventListener("click", () => switchTab("article"));

function switchTab(tab) {
  // Скрыть все формы
  document.getElementById("wineForm").style.display = "none";
  document.getElementById("articleForm").style.display = "none";

  // Убрать активный класс у кнопок
  document
    .querySelectorAll(".tab-button")
    .forEach((btn) => btn.classList.remove("active"));

  // Показать нужную форму и активировать кнопку
  if (tab === "wine") {
    document.getElementById("wineForm").style.display = "block";
    document.getElementById("wineTab").classList.add("active");
  } else {
    document.getElementById("articleForm").style.display = "block";
    document.getElementById("articleTab").classList.add("active");
  }

  // Очистить ошибки
  clearAllErrors();
}

// Обработчики отправки форм
document.getElementById("wineForm").addEventListener("submit", (e) => {
  e.preventDefault();
  if (validateWineForm()) {
    showSuccess();
    clearForm("wineForm");
  }
});

document.getElementById("articleForm").addEventListener("submit", (e) => {
  e.preventDefault();
  if (validateArticleForm()) {
    showSuccess();
    clearForm("articleForm");
  }
});

// Маска для телефона
document.getElementById("winePhone").addEventListener("input", function (e) {
  let value = e.target.value.replace(/\D/g, "");
  let formattedValue = "+7 ";

  if (value.length > 1) {
    formattedValue += "(" + value.substring(1, 4);
  }
  if (value.length >= 5) {
    formattedValue += ") " + value.substring(4, 7);
  }
  if (value.length >= 8) {
    formattedValue += "-" + value.substring(7, 9);
  }
  if (value.length >= 10) {
    formattedValue += "-" + value.substring(9, 11);
  }

  e.target.value = formattedValue;
});

// Проверка формы вина (5 различных проверок)
function validateWineForm() {
  let isValid = true;

  // 1. Название (только буквы, пробелы, дефисы, апострофы, мин 3 символа)
  const name = document.getElementById("wineName").value.trim();
  const nameRegex = /^[a-zA-Zа-яА-ЯёЁ\s\-']{3,}$/;
  if (!nameRegex.test(name)) {
    showError(
      "wineNameError",
      "Название должно содержать только буквы, пробелы, дефисы, апострофы (мин. 3 символа)",
    );
    isValid = false;
  } else {
    clearError("wineNameError");
  }

  // 2. Год (целое число 1800-2025)
  const year = parseInt(document.getElementById("wineYear").value);
  if (isNaN(year) || year < 1800 || year > 2025) {
    showError("wineYearError", "Год должен быть целым числом от 1800 до 2025");
    isValid = false;
  } else {
    clearError("wineYearError");
  }

  // 3. Алкоголь (5.0-25.0, 1 десятичный знак)
  const alcohol = parseFloat(document.getElementById("wineAlcohol").value);
  if (
    isNaN(alcohol) ||
    alcohol < 5.0 ||
    alcohol > 25.0 ||
    !/^\d+\.\d$/.test(document.getElementById("wineAlcohol").value)
  ) {
    showError(
      "wineAlcoholError",
      "Алкоголь: 5.0-25.0 с одним десятичным знаком",
    );
    isValid = false;
  } else {
    clearError("wineAlcoholError");
  }

  // 4. Email (стандартная проверка email)
  const email = document.getElementById("wineEmail").value.trim();
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(email)) {
    showError("wineEmailError", "Введите корректный email адрес");
    isValid = false;
  } else {
    clearError("wineEmailError");
  }

  // 5. Телефон (+7 (XXX) XXX-XX-XX)
  const phone = document.getElementById("winePhone").value.trim();
  const phoneRegex = /^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$/;
  if (!phoneRegex.test(phone)) {
    showError("winePhoneError", "Формат: +7 (999) 123-45-67");
    isValid = false;
  } else {
    clearError("winePhoneError");
  }

  // 6. Проверка изображения вина
  const wineImage = document.getElementById("wineImage").files[0];
  if (!wineImage) {
    showError("wineImageError", "Пожалуйста, выберите изображение");
    isValid = false;
  } else if (wineImage.size > 2 * 1024 * 1024) {
    // 2MB
    showError("wineImageError", "Размер изображения не должен превышать 2MB");
    isValid = false;
  } else if (!wineImage.type.match(/image\/(jpeg|jpg|png|gif)/)) {
    showError(
      "wineImageError",
      "Пожалуйста, выберите изображение в формате JPG, PNG или GIF",
    );
    isValid = false;
  } else {
    clearError("wineImageError");
  }

  return isValid;
}

// Проверка формы статьи (5 различных проверок)
function validateArticleForm() {
  let isValid = true;

  // 1. Заголовок (5-100 символов, буквы+пробелы+дефисы)
  const title = document.getElementById("articleTitle").value.trim();
  const titleRegex = /^[a-zA-Zа-яА-ЯёЁ\s\-]{5,100}$/;
  if (!titleRegex.test(title)) {
    showError(
      "articleTitleError",
      "Заголовок: 5-100 символов, только буквы, пробелы, дефисы",
    );
    isValid = false;
  } else {
    clearError("articleTitleError");
  }

  // 2. Автор (ФИО: минимум 3 слова)
  const author = document.getElementById("articleAuthor").value.trim();
  const authorParts = author.split(/\s+/).filter((word) => word.length > 0);
  if (authorParts.length < 2 || authorParts.length > 3) {
    showError(
      "articleAuthorError",
      "ФИО: 2-3 слова (Фамилия И.О. или Имя Отчество)",
    );
    isValid = false;
  } else {
    clearError("articleAuthorError");
  }

  // 3. Страницы (1-500)
  const pages = parseInt(document.getElementById("articlePages").value);
  if (isNaN(pages) || pages < 1 || pages > 500) {
    showError("articlePagesError", "Количество страниц: 1-500");
    isValid = false;
  } else {
    clearError("articlePagesError");
  }

  // 4. ISBN (13 цифр через дефисы)
  const isbn = document
    .getElementById("articleISBN")
    .value.replace(/[-\s]/g, "");
  const isbnRegex = /^\d{13}$/;
  if (!isbnRegex.test(isbn)) {
    showError("articleISBNError", "ISBN: 13 цифр (978-0-306-40615-7)");
    isValid = false;
  } else {
    clearError("articleISBNError");
  }

  // 5. Дата (не позднее сегодня)
  const dateInput = document.getElementById("articleDate").value;
  const today = new Date("2025-12-11");
  const inputDate = new Date(dateInput);
  if (!dateInput || inputDate > today) {
    showError("articleDateError", "Дата не должна быть позже 2025-12-11");
    isValid = false;
  } else {
    clearError("articleDateError");
  }

  // 6. Проверка изображения статьи
  const articleImage = document.getElementById("articleImage").files[0];
  if (!articleImage) {
    showError("articleImageError", "Пожалуйста, выберите изображение");
    isValid = false;
  } else if (articleImage.size > 2 * 1024 * 1024) {
    // 2MB
    showError(
      "articleImageError",
      "Размер изображения не должен превышать 2MB",
    );
    isValid = false;
  } else if (!articleImage.type.match(/image\/(jpeg|jpg|png|gif)/)) {
    showError(
      "articleImageError",
      "Пожалуйста, выберите изображение в формате JPG, PNG или GIF",
    );
    isValid = false;
  } else {
    clearError("articleImageError");
  }

  return isValid;
}

// Вспомогательные функции

// Вспомогательные функции
function showError(inputId, errorId) {
  // Получаем поле ввода и подсказку
  const inputField = document.getElementById(inputId);
  const hintElement = inputField.parentElement.querySelector(".hint");
  const labelElement = inputField.parentElement.querySelector(".hint-error");

  // Добавляем красную рамку полю ввода
  inputField.style.backgroundColor = "#fff0f4";

  // Меняем цвет подсказки на красный
  if (hintElement) {
    hintElement.style.color = "red";
    labelElement.style.backgroundColor = "#bb6e6e";
  }
}

function clearError(inputId, errorId) {
  const inputField = document.getElementById(inputId);
  const hintElement = inputField.parentElement.querySelector(".hint");

  // Убираем красную рамку
  inputField.style.border = "";
  inputField.style.backgroundColor = "";

  // Возвращаем стандартный цвет подсказки
  if (hintElement) {
    hintElement.style.color = "";
  }
}

function clearAllErrors() {
  document.querySelectorAll(".error").forEach((el) => {
    el.textContent = "";
  });
}

function clearForm(formId) {
  document.getElementById(formId).reset();
  clearAllErrors();
}

function showSuccess() {
  const msg = document.getElementById("successMessage");
  msg.style.display = "block";
  setTimeout(() => {
    msg.style.display = "none";
  }, 3000);
}
