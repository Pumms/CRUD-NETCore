$(document).ready(function () {
    LoadDataEmp();
    LoadDataDepartment('#department');
    $('#Edit').hide();

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    });
});

document.getElementById("BtnAdd").addEventListener("click", function () {
    Clearall();
});

function ClearScreen() {
    $('#firstname').val('');
    $('#lastname').val('');
    $('#email').val('');
    $('#birthdate').val('');
    $('#address').val('');
    $('#phone').val('');
    $('#department').val(0);
    $('#Save').show();
    $('#edit').hide();
    $('#Delete').hide();
    $('#Modal').modal('hide');
    $('#text_email').show();
    $('#text_pass').show();
    $('#email').show();
    $('#password').show();
}

function Clearall() {
    $('#firstname').val('');
    $('#lastname').val('');
    $('#email').val('');
    $('#birthdate').val('');
    $('#address').val('');
    $('#phone').val('');
    $('#department').val(0);
    $('#Save').show();
    $('#Edit').hide();
}

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

function LoadDataEmp() {
    debugger;
    $.fn.dataTable.ext.errMode = 'none';
    $('#DataTable1').DataTable({
        "ajax": {
            url: "/Employee/LoadEmployee",
            type: "get",
            dataType: "json"
        },
        "columns": [
            { "data": "fullName" },
            { "data": "email" },
            {
                "data": "birthDate", "render": function (data) {
                    return moment(data).tz('Asia/Jakarta').format('DD/MM/YYYY');
                }
            },
            { "data": "address" },
            { "data": "phoneNumber" },
            { "data": "departmentName" },
            {
                "data": "createDate", "render": function (data) {
                    return moment(data).tz('Asia/Jakarta').format('DD/MM/YYYY');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var text = "Not Update Yet";
                    if (data == null) {
                        return text;
                    } else {
                        return moment(data).tz('Asia/Jakarta').format('DD/MM/YYYY');
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return '<button type="button" class="btn btn-sm btn-warning" id="BtnEdit" data-toggle="tooltip" data-placement="top" title="Edit" onclick="return GetByEmail(\'' + row.email + '\')"><i class="mdi mdi-pencil"></i></button> <button type="button" class="btn btn-sm btn-danger mt-2" id="BtnDelete" data-toggle="tooltip" data-placement="top" title="Hapus" onclick="return Delete(\'' + row.email + '\')"><i class="mdi mdi-delete"></i></button>';
                }, "orderable": false
            }
        ]
    });
}

function Save() {
    debugger;
    var table = $('#DataTable1').DataTable({
        "ajax": {
            url: "/Employee/LoadEmployee"
        }
    });
    var Employee = new Object();
    Employee.Email = $('#email').val();
    Employee.Password = $('#password').val();
    Employee.Firstname = $('#firstname').val();
    Employee.Lastname = $('#lastname').val();
    Employee.BirthDate = $('#birthdate').val();
    Employee.PhoneNumber = $('#phone').val();
    Employee.Address = $('#address').val();
    Employee.Department_Id = $('#department').val();
    $.ajax({
        type: 'POST',
        url: '/Auth/RegisterEmp/',
        data: Employee
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                icon: 'success',
                position: 'center',
                title: 'Employee Saved Successfully',
                timer: 5000
            }).then(function () {
                $('#Modal').modal('hide');
                table.ajax.reload();
                Clearscreen();
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to Insert!',
            })
            Clearscreen();
        }
    })
}

function GetByEmail(Email) {
    debugger;
    $('#text_email').hide();
    $('#text_pass').hide();
    $('#email').hide(); 
    $('#password').hide();
    $.ajax({
        url: "/Employee/GetByEmail/"+Email,
        type: "get",
        dataType: "json",
        data: { "Email": Email },
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
            $('#Edit').show();
            $('#Save').hide();
            $('#Modal').modal('show');
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
            var table = $('#DataTable1').DataTable({
                "ajax": {
                    url: "/Employee/LoadEmployee"
                }
            });
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
                            title: 'Employee Updated Successfully',
                            timer: 5000
                        }).then(function () {
                            $('#Modal').modal('hide');
                            table.ajax.reload();
                            ClearScreen();
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Failed to Update',
                        })
                        ClearScreen();
                    }
                })
            }
        }
    })
}

function Delete(Email) {
    debugger;
    var table = $('#DataTable1').DataTable({
        "ajax": {
            url: "/Employee/LoadEmployee"
        }
    });
    Swal.fire({
        title: "Are you sure ?",
        text: "You won't be able to Revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete it!",
        cancelButtonColor: "Red",
    }).then((result) => {
        if (result.value) {
            debugger;
            $.ajax({
                url: "Employee/Delete/",
                data: { Email: Email }
            }).then((result) => {
                debugger;
                if (result.statusCode == 200) {
                    Swal.fire({
                        icon: 'success',
                        position: 'center',
                        title: 'Employee Delete Successfully',
                        timer: 5000
                    }).then(function () {
                        table.ajax.reload();
                        ClearScreen();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Failed to Delete',
                    })
                    ClearScreen();
                }
            })
        }
    })
}