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
    if (response.status == "401") {
        window.location.href = "/Account/Login";
    }
}