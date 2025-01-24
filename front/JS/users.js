const getAllUrl = 'http://localhost:5000/api/users/all'; 
const deleteUrl = 'http://localhost:5000/api/users/delete/#id'
const addUrl = "http://localhost:5000/api/users/add"
const editUrl = "http://localhost:5000/api/users/update"


async function fetchUsers() {
    try {
        const response = await fetch(getAllUrl); 
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const users = await response.json();
        displayUsers(users);
    } catch (error) {
        console.error('Błąd podczas pobierania danych:', error);
    }
}

function displayUsers(users) {
    const userList = document.getElementById("user-list");

    if (!userList) {
        console.error("Nie znaleziono elementu o ID 'user-list'");
        return;
    }

    userList.innerHTML = "";

    users.forEach(user => {
        const userItem = document.createElement("div");
        userItem.classList.add("feature-item");

        userItem.innerHTML = `
            <h3>${user.name}</h3>
            <p>Rola: ${user.role}</p>
            <p>Email: ${user.email}</p>
            <div class="buttons">
                <button class="btn secondary edit-btn data-id=${user.id}">Edytuj</button>
                <button class="btn secondary delete-btn" data-id="${user.id}">Usuń</button>
            </div>
        `;

        userList.appendChild(userItem);
    });

    const deleteButtons = document.querySelectorAll(".delete-btn");
    deleteButtons.forEach(button => {
        button.addEventListener("click", (event) => {
            const userId = parseInt(event.target.getAttribute("data-id"), 10);
            deleteUser(userId);
        });
    });

    const editButtons = document.querySelectorAll(".edit-btn");
    editButtons.forEach(button => {
        button.addEventListener("click", (event) => {
            const userId = parseInt(event.target.getAttribute("data-id"), 10);
            edit(userId);
        });
    });

    const addNewUser = document.createElement("div");
    addNewUser.classList.add("feature-item");

    addNewUser.innerHTML = `
        <h3>Dodaj nowego użytkownika</h3>
        <p>Wypełnij formularz, aby dodać użytkownika.</p>
        <a href="users_add.html" class="a-button"><button class="btn secondary">Dodaj użytkownika</button></a>
    `;

    userList.appendChild(addNewUser);
}

async function deleteUser(id){
    try {
        const response = await fetch(deleteUrl.replace("#id", id)); 
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        await fetchUsers();
    } catch (error) {
        console.error('Błąd podczas pobierania danych:', error);
    }
}

async function editUser(id){

}


document.addEventListener('DOMContentLoaded', fetchUsers);