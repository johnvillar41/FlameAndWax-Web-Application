displayToastMixin = function (message, isSuccessfull) {
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
    if (isSuccessfull) {
        Toast.fire({
            icon: 'success',
            title: '<span style="color: #006400">Success</span> ' + message,
            background: '#CCFFCC',
            iconColor: '#006400',
        });
    } else {
        Toast.fire({
            icon: 'error',
            title: '<span style="color: #8b0000">Error!</span> ' + message,
            background: '#FF7F7F',
            iconColor: '#8b0000',
        });
    }
}

loginComplete = function (xhr) {
    if (xhr.status == "200") {
        displayToastMixin('Login Successfull!', true);
        window.location.href = xhr.responseText;
    } else if (xhr.status == "400") {
        displayToastMixin('User not found!', false);
    }
}

$('#sidebarCollapse').click(function (e) {
    if ($('.sidebar').hasClass('active')) {
        $(".sidebar").toggleClass('inactive')
    } else {
        $(".sidebar").toggleClass('active')
    }
});