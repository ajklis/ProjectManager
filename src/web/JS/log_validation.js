const getTokenUrl = "http://localhost:5000/api/auth/user";

function validateAndLogin() {
    const login = document.getElementById("login").value;
    const password = document.getElementById("password").value;

    if (login.trim() === "" || password.trim() === "") {
        alert("Proszę wypełnić wszystkie pola!");
        return;
    }

    const bodyData = {
        Email: login,
        Password: password
    };

    console.log("Logowanie...");

    fetch(getTokenUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(bodyData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error("Błąd logowania");
        }
        return response.json();
    })
    .then(data => {
        console.log(data.token);
        sessionStorage.setItem("TokenId", (data.token || JSON.stringify(data)));
        window.location.href = 'index.html';
    })
    .catch(error => {
        console.error('Błąd:', error);
        alert("Nie udało się zalogować. Sprawdź dane.");
    });
}