//for clearing Contact us
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

//For saving User profile
$('#submit').click(function () {
    completed = function (xhr) {
        var x = document.getElementById("completed");
        if (x.style.display === "none") {
            x.style.display = "block";
            if (xhr.status == "200") {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Your profile has been saved',
                    showConfirmButton: false,
                    timer: 1500
                })
            }
        } else {
            x.style.display = "none";
        }
    }
});

//For prompting successfull comment
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

checkIfPasswordIsEqual = function () {
    let password = document.querySelector('#password').value;
    let verifyPassword = document.querySelector('#verifyPassword').value;
    if (password === verifyPassword) {        
        return true;
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: '<h6>Passwords do not match!</h6>'
        })
        return false;
    }
}

contactUsPopup = function () {

}

//Updates the text for dropdown on Products
update = function (text) {
    document.getElementById("dropdownMenuButton").innerHTML = text + ' <i class="fas fa-caret-square-down"></i>';
}