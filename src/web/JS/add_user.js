const addUserUrl = "http://localhost:5000/api/users/add";

// Funkcja do dodawania użytkownika
function addUser(event) {
    event.preventDefault(); // Zatrzymanie domyślnego zachowania formularza (przeładowanie strony)

    console.log("add user");
    const form = document.getElementById("addUserForm");
    const user = {
        name: form.elements["name"].value,
        email: form.elements["email"].value,
        password: form.elements["password"].value,
        role: form.elements["role"].value,
        createdAt: new Date(),
    };

    console.log("pre post");

    try {
        fetch(addUserUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "*/*",
                "Access-Control-Allow-Origin": "*",
            },
            body: JSON.stringify(user),
        }).then(response => {
            if (response.ok) {
                alert("User added successfully!");
                document.getElementById("addUserForm").reset(); // Resetowanie formularza po pomyślnym dodaniu
                window.location.href = "/users.html";
            } else {
                alert("Error adding user. Please try again.");
            }
        });
    } catch (error) {
        console.error("Error:", error);
        alert("An error occurred. Please check the console for details.");
    }
}

// Dodanie nasłuchiwacza do formularza po załadowaniu strony
document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("addUserForm").addEventListener("submit", addUser);
});
