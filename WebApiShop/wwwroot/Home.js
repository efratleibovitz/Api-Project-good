const postResponse = async () => { 

    const userN = document.querySelector("#userName");
    const fName = document.querySelector("#firstName");
    const lName = document.querySelector("#lastName");
    const pass = document.querySelector("#password");

    const postData = {
    UserEmail: userN.value,
    FirstName: fName.value,
    LastName: lName.value,
    Password: pass.value,
    Id:0
};

    const responsePost = await fetch("https://localhost:44320/api/Users", {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify(postData)
});

    const dataPost = await responsePost.json();
    sessionStorage.setItem("user", JSON.stringify(dataPost));
    if (responsePost.status==201)
        alert("משתמש נוסף בהצלחה");
    else if (responsePost.status == 400)
        alert("סיסמא חלשה, נסה שנית")
    else
        alert("הליך ההרשמה נכשל, אנא נסה שנית 🤗 . ")
}

const login = async () => {
    const userN = document.querySelector("#userN").value;
    const pass = document.querySelector("#pass").value;
    const postDataLogin = {

        UserEmail: userN,
        FirstName: "",
        LastName: "",
        Password: pass,
        Id: 0
    };

    const responsePostLogin = await fetch("https://localhost:44320/api/Users/Login", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(postDataLogin)

    });

    if (responsePostLogin.ok) {
        const dataPostLogin = await responsePostLogin.json();
        sessionStorage.setItem("user", JSON.stringify(dataPostLogin))
        alert("wellcome!!!!!!!")
        window.location.href = "../Update.html"
    }
    else {
        alert("אופסססססססססס שם או סיסמא לא קיימים במערכת")
    }
   



}