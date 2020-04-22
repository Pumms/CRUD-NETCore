$(document).ready(function () {
    LoadDataDept();
    $('#Edit').hide();

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    });
});

document.getElementById("BtnAdd").addEventListener("click", function () {
    Clearall();
});

function ClearScreen() {
    $('#Name').val('');
    $('#Save').show();
    $('#Update').hide();
    $('#Delete').hide();
    $('#Modal').modal('hide');
}

function Clearall() {
    $('#Id').val('');
    $('#Name').val('');
    $('#Save').show();
    $('#Edit').hide();
}

function LoadDataDept() {
    debugger;
    $.fn.dataTable.ext.errMode = 'none';
    $('#DataTable1').DataTable({
        "ajax": {
            url: "/Department/LoadDepartment",
            type: "get",
            dataType: "json"
        },
        "columns": [
            { "data": "name" },
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

function GetById(Id) {
    debugger;
    $.ajax({
        url: "/Department/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            debugger;
            var obj = JSON.parse(result);
            const obj2 = JSON.parse(obj);
            $('#Id').val(obj2.id);
            $('#Name').val(obj2.name);
            $('#Edit').show();
            $('#Save').hide();
            $('#Modal').modal('show');
        },
        error: function (errormsg) {
            alert(errormessage.responseText);
        }
    })
}

function Save() {
    debugger;
    var table = $('#DataTable1').DataTable({
        "ajax": {
            url: "/Department/LoadDepartment"
        }
    });
    var Department = new Object();
    Department.Name = $('#Name').val();
    if ($('#Name').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Name Cannot be Empty',
        })
        return false;
    } else {
        debugger;
        $.ajax({
            type: 'POST',
            url: '/Department/InsertorUpdate/',
            data: Department
        }).then((result) => {
            debugger;
            if (result.statusCode == 200) {
                Swal.fire({
                    icon: 'success',
                    position: 'center',
                    title: 'Department Saved Successfully',
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
                    url: "/Department/LoadDepartment"
                }
            });
            var Department = new Object();
            Department.Id = $('#Id').val();
            Department.Name = $('#Name').val();
            if ($('#Name').val() == "") {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Name Cannot be Empty',
                })
                return false;
            } else {
                $.ajax({
                    type: 'POST',
                    url: '/Department/InsertorUpdate/',
                    data: Department
                }).then((result) => {
                    debugger;
                    if (result.statusCode == 200) {
                        Swal.fire({
                            icon: 'success',
                            position: 'center',
                            title: 'Department Updated Successfully',
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
            url: "/Department/LoadDepartment"
        }
    });

    Swal.fire({
        title: "Are you sure ?",
        text: "You won't be able to Revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete it!",
        cancelButtonColor: "Red"
    }).then((result) => {
        if (result.value) {
            debugger;
            $.ajax({
                url: "Department/Delete/",
                data: { Id: Id }
            }).then((result) => {
                debugger;
                if (result.statusCode == 200) {
                    Swal.fire({
                        icon: 'success',
                        position: 'center',
                        title: 'Department Delete Successfully',
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