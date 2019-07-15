$(document).ready(function () {

    function bindDataTable() {
        $.ajax({
            url: '/Doctor/GetAllDoctors',
            method: 'post',
            dataType: 'json',
            success: function (data) {
                $("#datatable").dataTable({
                    data: data,
                    columns: [
                        { 'data': 'DoctorId' },
                        { 'data': 'FirstName' },
                        { 'data': 'HospitalName' },
                        { 'data': 'Address' },
                        { 'data': 'Phone1' }
                    ],
                    columnDefs: [
                        {
                            targets: 5,
                            render: function (data, type, row, meta) {
                                return '<a href="#" class="view-link" data-toggle="modal" data-target="#Modal">View</a>';
                            },
                        },
                        {
                            targets: 6,
                            render: function (data, type, row, meta) {
                                return '<a href="#" class="edit-link" data-toggle="modal" data-target="#Modal">Edit</a>';
                            },
                        },
                        {
                            targets: 7,
                            render: function (data, type, row, meta) {
                                return '<a href="#" class="delete-link">Delete</a>';
                            },
                        }
                    ]
                });
            },
            error: function (err) {
                alert(err);
            }
        });
    }

    function bindDropDowns() {
        $.ajax({
            url: '/Doctor/GetHospitals',
            type: 'post',
            dataType: "json",
            success: function (data) {
                $("#ddlHospitals").empty();
                $("#ddlHospitals").append("<option value='0'>Select Hospital</option>");
                $.each(data, function (index, value) {
                    $("#ddlHospitals").append("<option value='" + value.HospitalId + "'>" + value.HospitalName + "</option>");
                });
            },
            error: function (err) {
            }
        });

        $.ajax({
            url: '/Doctor/GetSpecialities',
            type: 'post',
            dataType: "json",
            success: function (data) {
                $("#ddlSpecialities").empty();
                $("#ddlSpecialities").append("<option value='0'>Select Speciality</option>");
                $.each(data, function (index, value) {
                    $("#ddlSpecialities").append("<option value='" + value.SpecialityId + "'>" + value.SpecialityName + "</option>");
                });
            },
            error: function (err) {
            }
        });
    }

    bindDataTable();

    $("body").on("click", ".view-link", function () {

        $("#txtFirstName").hide();
        $("#txtLastName").hide();
        $("#ddlHospitals").hide();
        $("#ddlSpecialities").hide();
        $("#txtAddress").hide();
        $("#txtEmail").hide();
        $("#txtContactNumber").hide();
        $("#txtAlternativeNumber").hide();
        $("#primaryRow").hide();
        $("#btnAddNewDoctor").hide();
        $("#btnEditDoctor").hide();

        $('#primaryDoctor').show();
        $("#spnFirstName").show();
        $("#spnLastName").show();
        $("#spnHospitalName").show();
        $("#spnSpeciality").show();
        $("#spnAddress").show();
        $("#spnEmail").show();
        $("#spnContactNumber").show();
        $("#spnAlternativeNumber").show();

        var id = $(this).closest('tr').find('td:eq(0)').text();
        var data = { 'doctorId': id };
        $.ajax({
            url: '/Doctor/GetDoctorDetails',
            type: 'post',
            data: data,
            dataType: "json",
            success: function (data) {
                $('#primaryDoctor').text("");
                $("#spnFirstName").text(data.FirstName);
                $("#spnLastName").text(data.LastName);
                $("#spnHospitalName").text(data.HospitalName);
                $("#spnSpeciality").text(data.SpecialistIn);
                $("#spnAddress").text(data.Address);
                $("#spnEmail").text(data.Email);
                $("#spnContactNumber").text(data.Phone1);
                $("#spnAlternativeNumber").text(data.Phone2);
                if (data.IsPrimary == 1) {
                    $('#primaryDoctor').text("(Primary Doctor)");
                }
            },
            error: function (err) {
                alert(err);
            }
        });
    });

    $("body").on("click", ".edit-link", function () {

        $('#primaryDoctor').hide();
        $("#spnFirstName").hide();
        $("#spnLastName").hide();
        $("#spnHospitalName").hide();
        $("#spnSpeciality").hide();
        $("#spnAddress").hide();
        $("#spnEmail").hide();
        $("#spnContactNumber").hide();
        $("#spnAlternativeNumber").hide();
        $("#btnAddNewDoctor").hide();

        $("#txtFirstName").show();
        $("#txtLastName").show();
        $("#ddlHospitals").show();
        $("#ddlSpecialities").show();
        $("#txtAddress").show();
        $("#txtEmail").show();
        $("#txtContactNumber").show();
        $("#txtAlternativeNumber").show();
        $("#primaryRow").show();
        $("#btnEditDoctor").show();

        var id = $(this).closest('tr').find('td:eq(0)').text();
        $("#txtDoctorId").val(id);
        var data = { 'doctorId': id };

        bindDropDowns();

        $.ajax({
            url: '/Doctor/GetDoctorDetails',
            type: 'post',
            data: data,
            dataType: "json",
            success: function (data) {
                $('#chkSetPrimary').prop('checked', false);
                $("#txtFirstName").val(data.FirstName);
                $("#txtLastName").val(data.LastName);
                $("#txtHospitalName").val(data.HospitalName);
                $("#txtSpeciality").val(data.SpecialistIn);
                $("#txtAddress").val(data.Address);
                $("#txtEmail").val(data.Email);
                $("#txtContactNumber").val(data.Phone1);
                $("#txtAlternativeNumber").val(data.Phone2);
                if (data.IsPrimary == 1) {
                    $('#chkSetPrimary').prop('checked', true);
                }
            },
            error: function (err) {
                alert(err);
            }
        });
    });

    $("body").on("click", ".delete-link", function () {

        if (!window.confirm("Are you sure?")) {
            return;
        }

        var id = $(this).closest('tr').find('td:eq(0)').text();
        var data = { 'doctorId': id };
        $.ajax({
            url: '/Doctor/DeleteDoctorV2',
            type: 'post',
            data: data,
            dataType: "json",
            success: function (data) {
                alert("Doctor recoed deleted.");
            },
            error: function (err) {
                alert("Some error occured.Please try again.");
            }
        });

        $("#datatable").dataTable().fnDestroy();
        bindDataTable();
    });

    $("body").on("click", ".add-new", function () {

        bindDropDowns();

        $('#primaryDoctor').hide();
        $("#spnFirstName").hide();
        $("#spnLastName").hide();
        $("#spnHospitalName").hide();
        $("#spnSpeciality").hide();
        $("#spnAddress").hide();
        $("#spnEmail").hide();
        $("#spnContactNumber").hide();
        $("#spnAlternativeNumber").hide();
        $("#btnEditDoctor").hide();

        $("#txtFirstName").show();
        $("#txtFirstName").val("");
        $("#txtLastName").show();
        $("#txtLastName").val("");
        $("#ddlHospitals").show();
        $("#ddlSpecialities").show();
        $("#txtAddress").show();
        $("#txtAddress").val("");
        $("#txtEmail").show();
        $("#txtEmail").val("");
        $("#txtContactNumber").show();
        $("#txtContactNumber").val("");
        $("#txtAlternativeNumber").show();
        $("#txtAlternativeNumber").val("");
        $('#chkSetPrimary').prop('checked', false);
        $("#primaryRow").show();
        $("#btnAddNewDoctor").show();

    });

    $("body").on("click", "#btnAddNewDoctor", function () {

        var isPrimary = 0;
        if ($("#chkSetPrimary").prop("checked") == true) {
            isPrimary = 1;
        }

        $.ajax({
            url: '/Doctor/AddNewDoctorV2',
            type: 'post',
            data: {
                firstName: $("#txtFirstName").val(),
                lastName: $("#txtFirstName").val(),
                hospitalId: $("#ddlHospitals").val(),
                specialityId: $("#ddlSpecialities").val(),
                address: $("#txtAddress").val(),
                email: $("#txtEmail").val(),
                contactNumber: $("#txtContactNumber").val(),
                alternativeNumber: $("#txtAlternativeNumber").val(),
                isPrimary: isPrimary
            },
            dataType: "json",
            success: function (data) {
                alert("Successfully added a new Doctor.");
            },
            error: function (err) {
                alert("Some error occured. Please try again.");
            }
        });

        $("#datatable").dataTable().fnDestroy();
        bindDataTable();

    });

    $("body").on("click", "#btnEditDoctor", function () {
        var isPrimary = 0;
        if ($("#chkSetPrimary").prop("checked") == true) {
            isPrimary = 1;
        }

        $.ajax({
            url: '/Doctor/EditDoctorV2',
            type: 'post',
            data: {
                doctorId: $("#txtDoctorId").val(),
                firstName: $("#txtFirstName").val(),
                lastName: $("#txtLastName").val(),
                hospitalId: $("#ddlHospitals").val(),
                specialityId: $("#ddlSpecialities").val(),
                address: $("#txtAddress").val(),
                email: $("#txtEmail").val(),
                contactNumber: $("#txtContactNumber").val(),
                alternativeNumber: $("#txtAlternativeNumber").val(),
                isPrimary: isPrimary
            },
            dataType: "json",
            success: function (data) {
                alert("Record updated.");
            },
            error: function (err) {
                alert("Some error occured. Please try again.");
            }
        });

        $("#datatable").dataTable().fnDestroy();
        bindDataTable();
    });
});