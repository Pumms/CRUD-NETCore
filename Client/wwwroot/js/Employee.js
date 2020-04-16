$(document).ready(function () {
    LoadDataEmp();
    LoadDataDepartment('#Department_Id');
    $('#Edit').hide();

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    });
});

document.getElementById("BtnAdd").addEventListener("click", function () {
    Clearall();
});

function ClearScreen() {
    $('#Id').val('');
    $('#FirstName').val('');
    $('#LastName').val('');
    $('#Email').val('');
    $('#BirthDate').val('');
    $('#Address').val('');
    $('#Phone').val('');
    $('#Department_Id').val(0);
    $('#Save').show();
    $('#Update').hide();
    $('#Delete').hide();
    $('#Modal').modal('hide');
}

function Clearall() {
    $('#Id').val('');
    $('#FirstName').val('');
    $('#LastName').val('');
    $('#Email').val('');
    $('#BirthDate').val('');
    $('#Address').val('');
    $('#Phone').val('');
    $('#Department_Id').val(0);
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
                    return '<button type="button" class="btn btn-warning" id="BtnEdit" data-toggle="tooltip" data-placement="top" title="Edit" onclick="return GetById(' + row.id + ')"><i class="mdi mdi-pencil"></i></button> &nbsp; <button type="button" class="btn btn-danger" id="BtnDelete" data-toggle="tooltip" data-placement="top" title="Hapus" onclick="return Delete(' + row.id + ')"><i class="mdi mdi-delete"></i></button>';
                }, "orderable": false
            }
        ]
    });
}

function Save() {
    debugger;
    var table = $('#DataTable1').DataTable({
        "ajax": {
            url: "/Department/LoadDepartment"
        }
    });
    var Employee = new Object();
    Employee.FirstName = $('#FirstName').val();
    Employee.LastName = $('#LastName').val();
    Employee.Email = $('#Email').val();
    Employee.BirthDate = $('#BirthDate').val();
    Employee.Address = $('#Address').val();
    Employee.PhoneNumber = $('#Phone').val();
    Employee.Department_Id = $('#Department_Id').val();
    if ($('#Fname').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'First Name Cannot be Empty',
        })
        return false;
    } else {
        debugger;
        $.ajax({
            type: 'POST',
            url: '/Employee/InsertorUpdate/',
            data: Employee
        }).then((result) => {
            debugger;
            if (result.statusCode == 201) {
                Swal.fire({
                    icon: 'success',
                    position: 'center',
                    title: 'Employee Saved Successfully',
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
                    text: 'Failed to Insert',
                })
                ClearScreen();
            }
        })
    }
}

function GetById(Id) {
    debugger;
    $.ajax({
        url: "/Employee/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            debugger;
            var obj = JSON.parse(result);
            const obj2 = JSON.parse(obj);
            $('#Id').val(obj2.id);
            $('#FirstName').val(obj2.firstName);
            $('#LastName').val(obj2.lastName);
            $('#Email').val(obj2.email);
            $('#BirthDate').val(moment(obj2.birthDate).format('YYYY-MM-DD'));
            $('#Address').val(obj2.address);
            $('#Phone').val(obj2.phoneNumber);
            $('#Department_Id').val(obj2.department_Id);
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
        if (result.value) {
            var table = $('#DataTable1').DataTable({
                "ajax": {
                    url: "/Employee/LoadEmployee"
                }
            });
            var Employee = new Object();
            Employee.Id = $('#Id').val();
            Employee.FirstName = $('#FirstName').val();
            Employee.LastName = $('#LastName').val();
            Employee.Email = $('#Email').val();
            Employee.BirthDate = $('#BirthDate').val();
            Employee.Address = $('#Address').val();
            Employee.PhoneNumber = $('#Phone').val();
            Employee.Department_Id = $('#Department_Id').val();
            if ($('#Fname').val() == "") {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'First Name Cannot be Empty',
                })
                return false;
            } else {
                $.ajax({
                    type: 'POST',
                    url: '/Employee/InsertorUpdate/',
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

function Delete(Id) {
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
                data: { Id: Id }
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