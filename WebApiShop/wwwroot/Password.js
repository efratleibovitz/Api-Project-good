const checkPasswordLevel = async () => {

    const pass = document.querySelector("#password").value;
    const prog = document.querySelector(".progressBar");

    const responsePost = await fetch("https://localhost:44320/api/PassWord", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(pass)
    });
    const dataPost = await responsePost.json();

    prog.value = dataPost * 25;
    if (responsePost.status == 200) {
        return dataPost / 4;
    }
    else {
        return 0;
    }
}