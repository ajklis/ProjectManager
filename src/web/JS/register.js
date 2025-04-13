const registerUrl = "http://localhost:5000/api/register"

async function registerUser(){
    form = document.getElementById("registerForm")
    
    const registerForm = {
        name: form.elements["name"].value,
        email: form.elements["email"].value,
        password: form.elements["password"].value
    };

    await fetch(registerUrl, {
        method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "*/*",
                "Access-Control-Allow-Origin": "*",
            },
            body: JSON.stringify(registerForm),
    }).then(response => {
        if (response.ok) {
            alert("User added successfully!");
            window.location.href = "log.html";
        } else {
            alert(`Error while registering: ${response.text()} user. Please try again.`);
            form.reset();
        }
    })
}