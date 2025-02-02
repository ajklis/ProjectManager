const getAllProjectsUrl = "http://localhost:5000/api/projects/all";
const deleteProjectUrl = "http://localhost:5000/api/projects/delete/#id";
const addProjectUrl = "http://localhost:5000/api/projects/add";
const editProjectUrl = "http://localhost:5000/api/projects/update";

// Pobieranie i wyświetlanie projektów
async function fetchProjects() {
    try {
        const response = await fetch(getAllProjectsUrl);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const projects = await response.json();
        displayProjects(projects);
    } catch (error) {
        console.error("Błąd podczas pobierania projektów:", error);
    }
}

// Wyświetlanie projektów w HTML
function displayProjects(projects) {
    const projectList = document.getElementById("project-list");

    if (!projectList) {
        console.error("Nie znaleziono elementu o ID 'project-list'");
        return;
    }

    projectList.innerHTML = "";

    projects.forEach(project => {
        const projectItem = document.createElement("div");
        projectItem.classList.add("feature-item");

        projectItem.innerHTML = `
        <h3>${project.name}</h3>
        <p><strong>Opis:</strong> ${project.description}</p>
        <p><strong>Status:</strong> ${project.status}</p>
        <p><strong>Data utworzenia:</strong> ${new Date(project.createdAt).toLocaleDateString()}</p>
        <p><strong>Start:</strong> ${new Date(project.startDate).toLocaleDateString()}</p>
        <p><strong>Koniec:</strong> ${new Date(project.endDate).toLocaleDateString()}</p>
        <p><strong>Priorytet:</strong> ${project.priority}</p>
        <div class="buttons">
            <button class="btn secondary edit-btn" data-id="${project.id}">Edytuj</button>
            <button class="btn secondary delete-btn" data-id="${project.id}">Usuń</button>
        </div>
        `;

        projectList.appendChild(projectItem);
    });

    // Obsługa usuwania projektu
    document.querySelectorAll(".delete-btn").forEach(button => {
        button.addEventListener("click", (event) => {
            const projectId = parseInt(event.target.getAttribute("data-id"), 10);
            deleteProject(projectId);
        });
    });

    // Obsługa edycji projektu
    document.querySelectorAll(".edit-btn").forEach(button => {
        button.addEventListener("click", (event) => {
            const projectId = parseInt(event.target.getAttribute("data-id"), 10);
            editProject(projectId);
        });
    });

    // Dodanie opcji "Dodaj nowy projekt"
    const addNewProject = document.createElement("div");
    addNewProject.classList.add("feature-item");

    addNewProject.innerHTML = `
        <h3>Dodaj nowy projekt</h3>
        <p>Wypełnij formularz, aby dodać projekt.</p>
        <a href="add_projects.html" class="a-button"><button class="btn secondary">Dodaj projekt</button></a>
    `;

    projectList.appendChild(addNewProject);
}

// Funkcja do usuwania projektu
async function deleteProject(id) {
    try {
        const response = await fetch(deleteProjectUrl.replace("#id", id), { method: "GET" });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        await fetchProjects(); // Odświeżenie listy projektów po usunięciu
    } catch (error) {
        console.error("Błąd podczas usuwania projektu:", error);    
    }
}

// Funkcja do edycji projektu
async function editProject(id) {
    window.location.href = `/update_projects.html?id=${id}`;
}

// Pobranie listy projektów po załadowaniu strony
document.addEventListener("DOMContentLoaded", fetchProjects);