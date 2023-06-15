var TicketId = '';$(document).ready(function () {   
    var decodedToken = parseJwt(sessionStorage.getItem("token"));
    sessionStorage.setItem("Userid", decodedToken.userid);
    sessionStorage.setItem("Username", decodedToken.username);
    sessionStorage.setItem("Userrole", decodedToken.type);
    var UID = sessionStorage.getItem("Userid");
    if (decodedToken.type == "Admin") {
        $("#alltickets").css('display', 'block');
        showTickets(UID);
        showAllTickets();

    }
    else {
        $("#alltickets").css('display', 'none');
        showTickets(UID)

    }
    //update btnClick    $("#btnUpdateTicket").click(function () {        if (TicketId != '') {            UpdateTicket(TicketId)        }        else {            alert("No proper Ticket id found for update!")        }    });    $("#logout").click(function () {        window.location.href = "index.html";        sessionStorage.removeItem("token");        // $("#response").html("<h2>User successfully logged out.</h2 > ");    });    $('#tblTicketBody').on('click', '#delete', function () {        var currentrow = $(this).closest('tr');        var data = $("#tblTicketBody").DataTable().row(currentrow).data();        var id = data.ticketid;       /* var id = $(this).closest('tr').find('td:eq(0)').text();*/        var url = "/api/Ticket/" + id;        $.ajax({            url: url,            headers: { "Authorization": 'Bearer ' + sessionStorage.getItem("token") },            contentType: "application/json; charset=utf-8",            dataType: "json",            type: "Delete",            success: function (result) {                clearForm();                alert("Record has been deleted successfully!");                showTickets(UID);            },            error: function (msg) {                alert(msg);            }        });    });    $('#tblTicketBody').on('click', '#edit', function () {        var currentrow = $(this).closest('tr');        var data = $("#tblTicketBody").DataTable().row(currentrow).data();        var id = data.ticketid;        /*var id = $(this).closest('tr').find('td:eq(0)').text();*/        var ticketname = $(this).closest('tr').find('td:eq(0)').text();        var ticketcategory = $(this).closest('tr').find('td:eq(1)').text();        var ticketdescription = $(this).closest('tr').find('td:eq(2)').text();        var ticketstatus = $(this).closest('tr').find('td:eq(3)').text();        var createdby = sessionStorage.getItem("Userid");        // var createdon = $(this).closest('tr').find('td:eq(6)').text();
        var modifiedby = sessionStorage.getItem("Userid");
       // var modifiedon = $(this).closest('tr').find('td:eq(8)').text();
        TicketId = id;        $('#txtTicketname').val(ticketname);        $('#txtTicketCategory').val(ticketcategory);        $('#txtTicketDescription').val(ticketdescription);        $('#txtTicketStatus').val(ticketstatus);        /*$('#txtCreatedby').val(createdby);*/
        //$('#txtCreatedon').val(createdon);
        /*$('#txtModifiedby').val(modifiedby);*/
      //$('#txtModifiedon').val(modifiedon);        $("#btnCreateTicket").prop('disabled', true);        $("#btnUpdateTicket").prop('disabled', false);    });    $('#btnSubmit').click(function () {
        if ($('#uploadfile').val() == '') {
            alert('Please select file');
            return;
        }
        var formData = new FormData();
        var file = $('#uploadfile')[0];
        formData.append('file', file.files[0]);
        $.ajax({
            url: '/UploadFile',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (d) {
                $("#status").html("<span>File uploaded here: <a class='text-success' href=" + d + ">Open File</a></span>");
                $('#uploadfile').val(null);
            },
            error: function (d) {
                alert("Faild please try upload again");
            }
        });
    });    $("#btnExport").click(function () {        exportTicket();    });});function exportTicket() {    var url = "/api/Ticket/DownloadReport";    var id = sessionStorage.getItem("Userid");    if (id) {        $.ajax({            url: url,            headers: { "Authorization": 'Bearer ' + sessionStorage.getItem("token") },            contentType: "application/json; charset=utf-8",            dataType: "json",            data: JSON.stringify(id),            type: "Post",            success: function (response) {                var binaryString = window.atob(response.content);                var binaryLen = binaryString.length;                var bytes = new Uint8Array(binaryLen);                for (var i = 0; i < binaryLen; i++) {                    var ascii = binaryString.charCodeAt(i);                    bytes[i] = ascii;                }                var blob = new Blob([bytes], { type: response.type });                var link = document.createElement('a');                link.href = window.URL.createObjectURL(blob);                var fileName = response.filename;                link.download = fileName;                link.click();                alert("Ticket exported!");                location.reload();            },            error: function (msg) {                alert(msg);            }        });    }}//$(function () {//    var table = $('#tblTicketBody').dataTable();//    $("#btnExport").click(function (e) {//        e.preventDefault();//        window.open('data:application/vnd.ms-excel,' +//            encodeURIComponent(table[0].outerHTML));//    });//});//decodejwt
function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);

}//create functionfunction createTicket() {    var url = "/api/Ticket";    var Ticket = {};    if ($('#txtTicketname').val() === '' || $('#txtTicketCategory').val() === '' || $('#txtTicketDescription').val() === '' || $('#txtTicketStatus').val() === '')  {        alert("No feild can be left blank");    }    else {        Ticket.TicketName = $('#txtTicketname').val();        Ticket.TicketCategory = $('#txtTicketCategory').val();        Ticket.TicketDescription = $('#txtTicketDescription').val();        Ticket.TicketStatus = $('#txtTicketStatus').val();        Ticket.createdby = sessionStorage.getItem("Userid");
        Ticket.modifiedby = sessionStorage.getItem("Userid");
       

        if (Ticket) {            $.ajax({                url: url,                headers: { "Authorization": 'Bearer ' + sessionStorage.getItem("token") },                contentType: "application/json; charset=utf-8",                dataType: "json",                data: JSON.stringify(Ticket),                type: "Post",                success: function (result) {                    clearForm();                    location.reload();                    showTickets(UID);                },                error: function (msg) {                    alert(msg);                }            });        }    }}//show functionfunction showTickets(UID) {    var url = "api/Ticket/" + UID;    $.ajax({        url: url,        headers: { "Authorization": 'Bearer ' + sessionStorage.getItem("token") },        contentType: "application/json; charset=utf-8",        dataType: "json",        type: "Get",        success: function (data) {            $("#tblTicketBody").dataTable().fnDestroy();            if (data) {                $("#tblTicketBody").dataTable({                    data: data,                    columns: [                      /*  { data: 'ticketid' },*/                        { data: 'ticketname' },                        { data: 'ticketcategory' },                        { data: 'ticketdescription' },                        { data: 'ticketstatus' },                        /*{ data: 'createdby' },*/
                        { data: 'createdon' },
                        //{ data: 'modifiedby' },
                        //{ data: 'modifiedon' },
                        {                       
                                                        data: null,                            defaultContent: "<td><button id='edit' class='btn btn-primary'>Edit</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button id= 'delete' class='btn btn-danger'>Delete</button></td>",                            orderable: false                        }                    ]                });                           }        },        error: function (msg) {            alert(msg);        }    });}//show functionfunction showAllTickets() {    var url = "api/Ticket";    $.ajax({        url: url,        headers: { "Authorization": 'Bearer ' + sessionStorage.getItem("token") },        contentType: "application/json; charset=utf-8",        dataType: "json",        type: "Get",        success: function (data) {            $("#tblAllTicketBody").dataTable().fnDestroy();            if (data) {                $("#tblAllTicketBody").dataTable({                    data: data,                    columns: [                        { data: 'ticketid' },                        { data: 'ticketname' },                        { data: 'ticketcategory' },                        { data: 'ticketdescription' },                        { data: 'ticketstatus' },                        { data: 'createdby' },
                        { data: 'createdon' },
                        { data: 'modifiedby' },
                        { data: 'modifiedon' }
                        //{
                        //    data: null,                        //    defaultContent: "<td><button id='edit' class='btn btn-primary'>Edit</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button id= 'delete' class='btn btn-danger'>Delete</button></td>",                        //    orderable: false                        //}                    ]                });            }        },        error: function (msg) {            alert(msg);        }    });}//update Functionfunction UpdateTicket(TicketId) {    var url = "/api/Ticket/" + TicketId;    var Ticket = {};    Ticket.ticketid = TicketId;    Ticket.ticketname = $('#txtTicketname').val();    Ticket.ticketcategory = $('#txtTicketCategory').val();    Ticket.ticketdescription = $('#txtTicketDescription').val();    Ticket.ticketstatus = $('#txtTicketStatus').val();    Ticket.createdby = sessionStorage.getItem("Userid");     Ticket.modifiedby = sessionStorage.getItem("Userid");        if (Ticket) {        $.ajax({            url: url,            headers: { "Authorization": 'Bearer ' + sessionStorage.getItem("token") },            contentType: "application/json; charset=utf-8",            dataType: "json",            data: JSON.stringify(Ticket),            type: "Put",            success: function (result) {                clearForm();                location.reload();                showTickets(UID);                $("#btnCreateTicket").prop('disabled', false);                $("#btnUpdateTicket").prop('disabled', true);            },            error: function (msg) {                alert();            }        });    }}//Clear form functionfunction clearForm() {    $('#txtTicketname').val('');    $('#txtTicketCategory').val('');    $('#txtTicketDescription').val('');    $('#txtTicketStatus').val('');    $('#txtCreatedby').val('');
    //$('#txtCreatedon').val('');
    $('#txtModifiedby').val('');
    //$('#txtModifiedon').val('');
}//To Clear the session value