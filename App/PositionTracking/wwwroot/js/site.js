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