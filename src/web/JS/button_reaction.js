document.addEventListener("DOMContentLoaded", function () {
    const moreBtn = document.getElementById("moreInfoBtn");
    const moreText = document.getElementById("moreText");
    const heroImage = document.getElementById("heroImage");

    moreBtn.addEventListener("click", function () {
        moreText.classList.toggle("active");
        heroImage.classList.toggle("expanded");

        moreBtn.textContent = moreText.classList.contains("active") 
            ? "Hide info" 
            : "More info";
    });
});
