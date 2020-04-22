$(document).ready(function () {
    LoadDataUser();
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

function LoadDataUser() {
    debugger;
    $('#email').hide();
    $.ajax({
        url: "/User/LoadDataUser/",
        type: "get",
        dataType: "json",
        success: function (result) {
            debugger;
            const obj = JSON.parse(result);
            const obj2 = JSON.parse(obj);
            $('#email').val(obj2.data.email);
            $('#firstname').val(obj2.data.firstName);
            $('#lastname').val(obj2.data.lastName);
            $('#birthdate').val(moment(obj2.data.birthDate).format('YYYY-MM-DD'));
            $('#address').val(obj2.data.address);
            $('#phone').val(obj2.data.phoneNumber);
            $('#department').val(obj2.data.department_Id);
        },
        error: function (errormsg) {
            alert(errormessage.responseText);
        }
    })
}

function Edit() {
    debugger;
    Swal.fire({
        title: "Are you sure ?",
        text: "You won't be able to Revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, Update this Data!",
        cancelButtonColor: "Red",
    }).then((result) => {
        debugger
        if (result.value) {
            var Employee = new Object();
            Employee.Email = $('#email').val();
            Employee.Firstname = $('#firstname').val();
            Employee.Lastname = $('#lastname').val();
            Employee.BirthDate = $('#birthdate').val();
            Employee.PhoneNumber = $('#phone').val();
            Employee.Address = $('#address').val();
            Employee.Department_Id = $('#department').val();
            if ($('#firstname').val() == "" || $('#lastname').val() == "" || $('#birthdate').val() == "" || $('#phone').val() == "" || $('#address').val() == "") {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Any Field Cannot be Empty',
                })
                return false;
            } else {
                debugger;
                $.ajax({
                    type: 'POST',
                    url: '/Employee/Update/',
                    data: Employee
                }).then((result) => {
                    debugger;
                    if (result.statusCode == 200) {
                        Swal.fire({
                            icon: 'success',
                            position: 'center',
                            title: 'Profile Updated!',
                            timer: 5000
                        }).then(function () {
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Failed to Update Profile',
                        })
                        ClearScreen();
                    }
                })
            }
        }
    })
}