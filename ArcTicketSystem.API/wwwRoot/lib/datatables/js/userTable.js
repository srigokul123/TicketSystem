﻿
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

        var modifiedby = sessionStorage.getItem("Userid");
       // var modifiedon = $(this).closest('tr').find('td:eq(8)').text();

        //$('#txtCreatedon').val(createdon);
        /*$('#txtModifiedby').val(modifiedby);*/
      //$('#txtModifiedon').val(modifiedon);
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
    });
function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);

}
        Ticket.modifiedby = sessionStorage.getItem("Userid");
       


                        { data: 'createdon' },
                        //{ data: 'modifiedby' },
                        //{ data: 'modifiedon' },

                            
                        { data: 'createdon' },
                        { data: 'modifiedby' },
                        { data: 'modifiedon' }


    //$('#txtCreatedon').val('');
    $('#txtModifiedby').val('');
    //$('#txtModifiedon').val('');
