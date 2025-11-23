let userData = JSON.parse(sessionStorage.getItem("user"));
console.log("userData:", userData);
console.log("userData:", userData.fName);

if (userData) {
    console.log("User:", userData);

    document.querySelector("h1").textContent = `Hellow ${userData.fName}!! wellcome back 🥰🤭🫣😊!!!!`;
}

const updateResponse = async () => {

    const userN = document.querySelector("#userName");
    const fName = document.querySelector("#firstName");
    const lName = document.querySelector("#lastName");
    const pass = document.querySelector("#password");


    const postData = {

        userName: userN.value,
        fName: fName.value,
        lName: lName.value,
        passWord: pass.value,
        id: userData.id
    };


    const responsePost = await fetch(`https://localhost:44320/api/Users/${userData.id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(postData)

    });

    const dataPost = await responsePost.json();
    if (responsePost.ok)
        alert("הנתונים עודכנו בהצלחה!!");


}


