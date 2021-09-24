﻿loginComplete = function (xhr) {
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
            title: '<span style="color: #006400"><b>Success</b></span> Login Successfull!',
            background: '#CCFFCC',
            iconColor: '#006400',
        });
    } else if (xhr.status == "400") {
        Toast.fire({
            icon: 'error',
            title: '<span style="color: #8b0000"><b>Error!</b></span> User not found!',
            background: '#FF7F7F',
            iconColor: '#8b0000',
        });
    }
}