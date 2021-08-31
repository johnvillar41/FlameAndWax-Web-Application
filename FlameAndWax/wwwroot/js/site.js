function resetText() {
    document.getElementById("fullname").value = "";
    document.getElementById("contactNumber").value = "";
    document.getElementById("email").value = "";
    document.getElementById("username").value = "";
    document.getElementById("address").value = "";
    document.getElementById("password").value = "";
    document.getElementById("verifyPassword").value = "";
    return false;
}

completed = function () {
    var x = document.getElementById("completed");
    if (x.style.display === "none") {
        x.style.display = "block";
        document.getElementById("name").value = "";
        document.getElementById("email").value = "";
        document.getElementById("phone").value = "";
        document.getElementById("message").value = "";
    } else {
        x.style.display = "none";
    }
}
completedComment = function () {
    var x = document.getElementById("completedComment");
    if (x.style.display === "none") {
        x.style.display = "block";
        document.getElementById("name").value = "";
        document.getElementById("email").value = "";
        document.getElementById("phone").value = "";
        document.getElementById("message").value = "";
    } else {
        x.style.display = "none";
    }
}

checkIfPasswordIsEqual = function (password, validatePassword, e) {
    if (password === validatePassword) {
        return true;
    } else {
        e.preventDefault();
        return false;
    }
}