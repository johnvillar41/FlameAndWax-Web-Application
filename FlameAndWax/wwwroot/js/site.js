//for clearing User profile
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

//For cart submission trigger
$('#cartComplete').click(function () {
    cartComplete = function (xhr) {
        if (xhr.status == "200") {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'success',
                title: 'Cart Submitted!'
            })
            document.getElementById('totalCartCount').innerHTML = "0";
        } else {

        }
    }
});

//Cart validation
checkIfCartHasItems = function (cartItems) {
    if (cartItems == 0) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: '<h6>Cart has no items</h6>'
        })
        return false;
    }
    return true;
}

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

//User profile password validation
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

//Updates the text for dropdown on Products
update = function (text) {
    document.getElementById("dropdownMenuButton").innerHTML = text + ' <i class="fas fa-caret-square-down"></i>';
}

//Add to Cart trigger
$('#addtoCartBtn').click(function () {
    addToCart = function (xhr) {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        if (xhr.status == "200") {
            Toast.fire({
                icon: 'success',
                title: 'Added to cart!'
            })
        } else {
            Toast.fire({
                icon: 'error',
                title: 'Failed to add!'
            })
        }
    }
});

$('#deleteCartItem').click(function () {
    negateTotalNumberOfCart = function (xhr) {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        if (xhr.status == "200") {
            Toast.fire({
                icon: 'success',
                title: 'Removed item from cart!'
            })

            let totalCartNumValue = Number(document.getElementById('totalCartCount').innerHTML);
            totalCartNumValue--;
            document.getElementById('totalCartCount').innerHTML = totalCartNumValue;
        }
    }
});

updateAddToCartTotalCount = function (response) {
    var dataObject = JSON.parse(response);
    document.querySelector('#totalCartCount').innerHTML = dataObject;
}

failureAddToCart = function (response) {

    window.location.href = "/Account/Login/?returnUrl=" + response;

}

//Reloads page on back button trigger
window.addEventListener("pageshow", function (event) {
    var historyPage = event.persisted ||
        (typeof window.performance != "undefined" &&
            window.performance.navigation.type === 2);
    if (historyPage) {
        // Handle page restore.
        window.location.reload();
    }
});

//Scrolls up after pagination
scrollUp = function () {
    $('body,html').animate({
        scrollTop: 0
    }, 600);
}

//Registraing new User Functions-----------------------------------------------------

//For Registering new User
$('#registerUser').click(function () {
    registrationComplete = function (xhr) {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        });
        if (xhr.status == "200") {
            Toast.fire({
                icon: 'success',
                title: 'Registered new user!'
            });
        }
    }
});

//Error registration
errorContent = function (response) {    
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });
    
    Toast.fire({
        icon: 'error',
        title: response.responseJSON.errorContent    
    });
}

//Registraing new User Functions-----------------------------------------------------

//Login Functions----------------------------------------------------

//Login User
$('#loginBtn').click(function () {
    loginComplete = function (xhr) {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        });
        if (xhr.status == "200") {
            Toast.fire({
                icon: 'success',
                title: 'Login Successfull!'
            });
        }
    }
});

//Login success routing
loginSuccess = function (response) {
    window.location.href = response;
}

//Login Error
errorLogin = function (response) {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: 'error',
        title: response.responseJSON.errorContent
    });
}

//Login Functions----------------------------------------------------