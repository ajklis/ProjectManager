function validateAndLogin() {
    let login = document.getElementById("login").value;
    let haslo = document.getElementById("haslo").value;

    if (login.trim() === "" || haslo.trim() === "") {
        alert("Proszę wypełnić wszystkie pola!");
    } else {
        window.location.href = 'index.html';
    }
}