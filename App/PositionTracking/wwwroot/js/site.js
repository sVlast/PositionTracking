// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var modalResult;   //globalna varijabla za spremanje confirmationa

function showConfirmModal()
{

}

function confirmModalYesHandle()
{

}

$("form.form-confirm").on("submit", function (e) {
    return confirm("Are you sure?");
})


$('.btn-refresh').click(function (e) {
    var id = $(this).attr("data-id");

    $.ajax({
        type: "GET",
        dataType: "json",
        url: $("#getRankUrl") + id, //
        success: function(data) {
            alert(data);
        }
    });
});

