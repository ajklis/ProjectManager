import { validateAndLogin } from "./log_validation.js";

document.addEventListener("DOMContentLoaded", () => {
    const button = document.getElementById("loginButton");
    button.addEventListener("click", (event) => {
        event.preventDefault();
        validateAndLogin();
    });
});