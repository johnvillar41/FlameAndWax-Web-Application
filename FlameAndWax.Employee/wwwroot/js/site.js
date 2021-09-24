loginComplete = function (xhr) {
    if (xhr.status == "200") {
        console.log("Success! nigga");
    } else if (xhr.status == "400") {
        console.log("Error login!");
    }
}