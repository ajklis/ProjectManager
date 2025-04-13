const getTokenUrl = "http://localhost:5000/api/auth/user";
const validateUrl = "http://localhost:5000/api/auth/token"

export function validate(){
    const tokenBody = {
        TokenId: sessionStorage.getItem('TokenId')
    };
    
    fetch(validateUrl, {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json'
        },
        body: JSON.stringify(tokenBody) 
    })
    .then(response => {
        console.log(response);
        if (!response.ok)
            throw new Error("Unauthorized")
        return response.json()
    })
    .catch(error => {
        alert("User not logged in, please log in to continue");
        window.location.href = 'log.html'
    });
}

export function validateAndLogin() {
    const login = document.getElementById("login").value;
    const password = document.getElementById("password").value;

    if (login.trim() === "" || password.trim() === "") {
        alert("Incorrect email or password");
        return;
    }

    const bodyData = {
        Email: login,
        Password: password
    };

    console.log("login");

    fetch(getTokenUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(bodyData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error("Unauthorized");
        }
        return response.text();
    })
    .then(data => {
        const token = data.replace(/^"|"$/g, '');
        console.log('Token:', token);
        sessionStorage.setItem("TokenId", token);
        window.location.href = 'index.html';
    })
    .catch(error => {
        console.error('Erorr:', error);
        alert("Failed to log in");
    });
}

