debugger
$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "/api/Users",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $.each(data, function (index, user) {
                $("#userListBody").append("<tr><td>" + user.Name + "</td><td>" + user.AccessLevel + "</td></tr>");
            });
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
        }
    });
});