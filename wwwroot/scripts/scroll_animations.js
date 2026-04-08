/**
 * Логика появления элементов при прокрутке страницы.
 */

document.addEventListener("DOMContentLoaded", () => {
  // Настройки для Intersection Observer
  const observerOptions = {
    threshold: 0.1, // Элемент считается видимым, если видно 10% его площади
    rootMargin: "0px 0px -50px 0px", // Срабатывает, когда элемент приближается к нижней границе экрана
  };

  // Создаем наблюдатель
  const observer = new IntersectionObserver((entries, observer) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        entry.target.classList.add("visible");
        observer.unobserve(entry.target); // Останавливаем наблюдение после появления
      }
    });
  }, observerOptions);

  // Находим все элементы с классом 'appear' и начинаем их наблюдать
  const appearElements = document.querySelectorAll(".appear");
  appearElements.forEach((element) => {
    observer.observe(element);
  });
});
