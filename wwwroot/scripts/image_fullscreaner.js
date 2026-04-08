document.addEventListener("DOMContentLoaded", function () {
  const images = document.querySelectorAll("img");
  const overlay = document.getElementById("fullscreenOverlay");
  const fullscreenImage = document.getElementById("fullscreenImage");
  const closeBtn = document.getElementById("closeBtn");

  images.forEach((img) => {
    img.addEventListener("click", function () {
      fullscreenImage.src = this.src;
      overlay.classList.add("active");
    });
  });

  closeBtn.addEventListener("click", function () {
    overlay.classList.remove("active");
  });

  overlay.addEventListener("click", function (e) {
    if (e.target === overlay) {
      overlay.classList.remove("active");
    }
  });
});
