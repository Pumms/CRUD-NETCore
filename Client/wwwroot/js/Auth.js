$(document).ready(function () {
    LoadDataDepartment('#department');
}); 

var Departments = []
function LoadDataDepartment(element) {
    if (Departments.length == 0) {
        $.ajax({
            type: "GET",
            url: "/Department/LoadDepartment",
            success: function (data) {
                Departments = data.data;
                renderDepartment(element);
            }
        })
    } else {
        renderDepartment(element);
    }
}

function renderDepartment(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select Department').hide());
    $.each(Departments, function (i, val) {
        $ele.append($('<option/>').val(val.id).text(val.name));
    })
}

function Register() {
    debugger;
    var Register = new Object();
    Register.Email = $('#email').val();
    Register.Password = $('#password').val();
    Register.Firstname = $('#firstname').val();
    Register.Lastname = $('#lastname').val();
    Register.BirthDate = $('#birthdate').val();
    Register.PhoneNumber = $('#phone').val();
    Register.Address = $('#address').val();
    Register.Department_Id = $('#department').val();
    $.ajax({
        type: 'POST',
        url: '/Auth/RegisterEmp/',
        data: Register
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                icon: 'success',
                position: 'center',
                title: 'Register Success!',
                timer: 5000
            }).then(function () {
                window.location.href = '/Auth';
                Clearform();
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to Register!',
            })
            Clearform();
        }
    })
}

function Login() {
    debugger;
    var Login = new Object();
    Login.Email = $('#email').val();
    Login.Password = $('#password').val();
    $.ajax({
        type: 'POST',
        url: '/Auth/Login',
        data: Login
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Incorrect Email or Password!',
            })
            $('#password').val('');
        }
    })
}

function Clearform() {
    $('#email').val('');
    $('#password').val('');
}