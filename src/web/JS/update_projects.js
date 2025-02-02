const updateProjectUrl = "http://localhost:5000/api/projects/update"; // URL do edycji projektu
let currentProjectId = null; // Przechowuje ID edytowanego projektu

// Funkcja do pobrania projektu i wypełnienia formularza
function editProject(projectId) {
    fetch(`http://localhost:5000/api/projects/${projectId}`)
        .then(response => response.json())
        .then(project => {
            currentProjectId = projectId; // Przypisanie ID edytowanego projektu
            const form = document.getElementById("editProjectForm");
            form.elements["name"].value = project.name;
            form.elements["description"].value = project.description;
            form.elements["status"].value = project.status;
            form.elements["priority"].value = project.priority;
            
            // Ustawienie dat w formularzu w formacie YYYY-MM-DD
            form.elements["startDate"].value = formatDate(project.startDate);
            form.elements["endDate"].value = formatDate(project.endDate);
        })
        .catch(error => console.error("Error fetching project data:", error));
}

// Funkcja do formatowania daty do formatu YYYY-MM-DD
function formatDate(date) {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0'); // Miesiące są od 0, więc dodajemy 1
    const day = String(d.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

// Funkcja do aktualizacji projektu
function updateProject(event) {
    event.preventDefault(); // Zapobiega przeładowaniu strony

    if (!currentProjectId) {
        alert("No project selected for update.");
        return;
    }

    const form = document.getElementById("editProjectForm");
    const updatedProject = {
        id: currentProjectId,
        name: form.elements["name"].value,
        description: form.elements["description"].value,
        startDate: form.elements["startDate"].value,
        endDate: form.elements["endDate"].value,
        status: form.elements["status"].value,
        priority: form.elements["priority"].value,
    };

    fetch(updateProjectUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "*/*",
        },
        body: JSON.stringify(updatedProject),
    })
        .then(response => {
            if (response.ok) {
                alert("Project updated successfully!");
                window.location.href = "/projects.html"; // Przekierowanie na listę projektów
            } else {
                alert("Error updating project. Please try again.");
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("An error occurred. Please check the console for details.");
        });
}

// Nasłuchiwanie na formularz edycji projektu
document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("editProjectForm").addEventListener("submit", updateProject);
});

// Pobieranie ID projektu z URL i wczytanie danych do formularza
document.addEventListener("DOMContentLoaded", async () => {
    const urlParams = new URLSearchParams(window.location.search);
    const projectId = urlParams.get("id");

    if (projectId) {
        try {
            const response = await fetch(`http://localhost:5000/api/projects/${projectId}`);
            const project = await response.json();

            const form = document.getElementById("editProjectForm");
            form.elements["name"].value = project.name;
            form.elements["description"].value = project.description;
            form.elements["status"].value = project.status;
            form.elements["priority"].value = project.priority;

            // Ustawienie dat w formularzu w formacie YYYY-MM-DD
            form.elements["startDate"].value = formatDate(project.startDate);
            form.elements["endDate"].value = formatDate(project.endDate);
            currentProjectId = projectId;
        } catch (error) {
            console.error("Error loading project data:", error);
        }
    }
});
