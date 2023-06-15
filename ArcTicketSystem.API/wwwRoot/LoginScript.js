$(document).ready(function () {

    $("#login").click(function () {

        var options = {};
        options.url = "/api/User/"
        options.type = "POST";

        var obj = {};

        obj.Username = $('#username').val()
        obj.Password = $('#password').val();

        options.data = JSON.stringify(obj);
        options.contentType = "application/json";
        options.dataType = "json";

        options.success = function (obj) {
            sessionStorage.setItem("token", obj.token);
            window.location.href = "Home.html";
            $("#response").html("<h2>User successfully logged in.</h2 > ");
        };
        options.error = function () {
            $("#response").html("<h2>Error while calling the Web API!</h2 > ");
        };
        $.ajax(options);

    });

});



