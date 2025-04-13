import { validate } from "./log_validation.js";

const updateUserUrl = "http://localhost:5000/api/users/update"; // URL do edycji użytkownika
let currentUserId = null; // Przechowuje ID edytowanego użytkownika

// Funkcja do aktualizacji użytkownika
function updateUser(event) {
    event.preventDefault(); // Zapobiega przeładowaniu strony

    if (!currentUserId) {
        alert("No user selected for update.");
        return;
    }

    const form = document.getElementById("editUserForm");
    const updatedUser = {
        id: currentUserId,
        name: form.elements["name"].value,
        email: form.elements["email"].value,
        role: form.elements["role"].value,
    };

    fetch(updateUserUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "*/*",
            "TokenId": sessionStorage.getItem('TokenId')
        },
        body: JSON.stringify(updatedUser),
    })
        .then(response => {
            if (response.ok) {
                alert("User updated successfully!");
                window.location.href = "/users.html"; // Przekierowanie na listę użytkowników
            } else {
                alert("Error updating user. Please try again.");
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("An error occurred. Please check the console for details.");
        });
}

// Nasłuchiwanie na formularz edycji użytkownika
document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("editUserForm").addEventListener("submit", updateUser);
});

document.addEventListener("DOMContentLoaded", async () => {
    validate();
    
    const urlParams = new URLSearchParams(window.location.search);
    const userId = urlParams.get("id");

    if (userId) {
        try {
            const response = await fetch(`http://localhost:5000/api/users/${userId}`, {
                method: 'GET',
                headers: {
                    'TokenId': sessionStorage.getItem('TokenId')
                }
            });
            const user = await response.json();

            const form = document.getElementById("editUserForm");
            form.elements["name"].value = user.name;
            form.elements["email"].value = user.email;
            form.elements["role"].value = user.role;
            currentUserId = userId;
        } catch (error) {
            console.error("Error loading user data:", error);
        }
    }
});
