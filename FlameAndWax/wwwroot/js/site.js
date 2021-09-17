﻿openFacebook = function () {
    window.open('https://www.facebook.com/FlameandWaxPH13');
}
openInstagram = function () {
    window.open('https://www.instagram.com/flameandwax13/');
}

//for saving ShippingAddress

saveAddress = function (xhr) {
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
            title: '<span style="color: #006400"><b>Success</b></span> Updated Shipping Address!',
            background: '#CCFFCC',
            iconColor: '#006400',
        });
        animateCustomerReview();
    }

}


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

// For updating picture
changePicture = function () {
    var selectedFile = document.getElementById('file-upload').files[0];
    var img = document.getElementById('profilePicture')

    var reader = new FileReader();
    reader.onload = function () {
        img.src = this.result
    }
    reader.readAsDataURL(selectedFile);

}

//For saving User profile

completedUserProfile = function (xhr) {
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
    var x = document.getElementById("completed");
    if (x.style.display === "none") {
        x.style.display = "block";
        if (xhr.status == "200") {
            Toast.fire({
                icon: 'success',
                title: '<span style="color: #006400"><b>Success</b></span> Updated User Profile!',
                background: '#CCFFCC',
                iconColor: '#006400',
            });
        }
    } else {
        x.style.display = "none";
    }
}

//For adding productReview
completedReviewComment = function (xhr) {
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
            title: '<span style="color: #006400"><b>Success</b></span> Comment Submitted!',
            background: '#CCFFCC',
            iconColor: '#006400',
        });
        animateCustomerReview();
    }
}


//For saving User profile
completedSendMessage = function (xhr) {
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
            title: '<span style="color: #006400"><b>Success</b></span> Message Submitted!',
            background: '#CCFFCC',
            iconColor: '#006400',
        });

        document.getElementById('name').value = "";
        document.getElementById('email').value = "";
        document.getElementById('phone').value = "";
        document.getElementById('message').value = "";

        var x = document.getElementById("completed");
        if (x.style.display === "none") {
            x.style.display = "block";
        } else {
            x.style.display = "none";
        }

    }
}


//For cart submission trigger

cartComplete = function (xhr) {
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
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    });
    if (xhr.status == "200") {
        Toast.fire({
            icon: 'success',
            title: '<span style="color: #006400"><b>Success</b></span> Cart Submitted!',
            background: '#CCFFCC',
            iconColor: '#006400',
        });
        document.getElementById('totalCartCount').innerHTML = "0";
        window.location.href = "/Cart";
    } else {
        swalWithBootstrapButtons.fire({
            title: 'Unable to checkout orders',
            text: "You need to setup your shipping address first!!",
            icon: 'error',
            showCancelButton: true,
            confirmButtonText: 'Yes, setup my shipping address!',
            cancelButtonText: 'No, cancel!',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = "/UserProfile/Index/";
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                Swal.fire({
                    icon: 'warning',
                    title: xhr.responseJSON.errorContent,
                    text: 'Please setup your shipping address first!',
                    footer: '<a href="/UserProfile/Index" class="btn btn-success">Setup Shipping Address</a>'
                })
            }
        })

    }
}

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

//Animation 
animateCustomerReview = function () {
    $('#customerReviewCards').fadeIn();
}

$(document).ready(function () {
    $('#productCards').fadeIn();
    $('#customerReviewCards').fadeIn();
    $('#orderCards').fadeIn();
});


//Updates the text for dropdown on Products
update = function (text) {
    document.getElementById("dropdownMenuButton").innerHTML = text + ' <i class="fas fa-caret-square-down"></i>';
}

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
    });
    if (xhr.status == "200") {
        Toast.fire({
            icon: 'success',
            title: '<span style="color: #006400"><b>Success</b></span> Added to cart!',
            background: '#CCFFCC',
            iconColor: '#006400',
        });
    } else if (xhr.status == "401") {
        Toast.fire({
            icon: 'error',
            title: '<span style="color: #8b0000"><b>Error!</b></span> Login First!',
            background: '#FF7F7F',
            iconColor: '#8b0000',
        });       
        window.location.href = xhr.responseText;
    } else if (xhr.status == "400") {
        Toast.fire({
            icon: 'error',
            title: '<span style="color: #8b0000"><b>Error!</b></span> ' + xhr.responseText,
            background: '#FF7F7F',
            iconColor: '#8b0000',
        }); 
    }
}
//Error increment cart mixin display
errorCartIncrement = function (response) {
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
        title: '<span style="color: #8b0000"><b>Error!</b></span> ' + response.responseText,
        background: '#FF7F7F',
        iconColor: '#8b0000',
    });  
}


//Deletion of cart Items
$('#deleteCartItem').click(function () {
    negateTotalNumberOfCart(xhr);
});

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
            title: '<span style="color: #006400"><b>Success</b></span> Removed item from cart!',
            background: '#CCFFCC',
            iconColor: '#006400',
        });

        let totalCartNumValue = Number(document.getElementById('totalCartCount').innerHTML);
        totalCartNumValue--;
        document.getElementById('totalCartCount').innerHTML = totalCartNumValue;
    }
}
updateAddToCartTotalCount = function (response) {
    var dataObject = JSON.parse(response);
    document.querySelector('#totalCartCount').innerHTML = dataObject;


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

//Scrolls up after pagination and animates
scrollUp = function () {
    $('body,html').animate({
        scrollTop: 0
    }, 600);
    $('#productCards').fadeIn();
    $('#orderCards').fadeIn();
}

//Registraing new User Functions-----------------------------------------------------

//For Registering new User

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
    var passwordField = document.getElementById("txtPassword").value;
    var verifyPasswordField = document.getElementById("txtVerifyPassword").value;
    var arePasswordFieldsEqual = false;

    if (passwordField === verifyPasswordField) {
        arePasswordFieldsEqual = true;
        document.getElementById("txtPassword").classList.remove('is-invalid');
        document.getElementById("txtVerifyPassword").classList.remove('is-invalid');
    }

    else {
        document.getElementById("txtPassword").classList.add('is-invalid');
        document.getElementById("txtVerifyPassword").classList.add('is-invalid');
        Toast.fire({
            icon: 'error',
            title: '<span style="color: #8b0000"><b>Error!</b></span> Password fields are not equal!',
            background: '#FF7F7F',
            iconColor: '#8b0000',
        });
    }

    if (xhr.status == "200" && arePasswordFieldsEqual) {
        Toast.fire({
            icon: 'success',
            title: '<span style="color: #006400"><b>Success</b></span> Registered new user!',
            background: '#CCFFCC',
            iconColor: '#006400',
        });
        window.location.href = "/Account/Login";
    }
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
                title: '<span style="color: #006400"><b>Success</b></span> Logging Successfull!',
                background: '#CCFFCC',
                iconColor: '#006400',
            });
        }
    }
});

//Login success routing
loginSuccess = function (response) {
    window.location.href = response;
}
//Login Functions----------------------------------------------------


//Error Messages
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
    var errorContent = response.responseJSON.errorContent;
    if (errorContent === "Duplicate Username! Please try a different username")
        document.getElementById("txtUsername").classList.add('is-invalid');
    else
        document.getElementById("txtUsername").classList.remove('is-invalid');

    Toast.fire({
        icon: 'error',
        title: '<span style="color: #8b0000"><b>Error!</b></span> ' + errorContent,
        background: '#FF7F7F',
        iconColor: '#8b0000',
    });
}