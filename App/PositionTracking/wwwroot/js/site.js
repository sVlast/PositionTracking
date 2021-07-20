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
    var btn = $(this);
    var id = btn.attr("data-id");
    var url = $("#getRankUrl").val() + id;

    var req = new XMLHttpRequest();


    req.open('GET', url, true);
    req.timeout = 60000;
    req.responseType = 'json';
    req.onreadystatechange = function () {

        try {

            if (req.readyState !== 4) {
                return;

            } else if (req.status === 0) {
                alert("status 0");

            } else if (req.status !== 200) {
                alert("status " + req.status + " (" + req.statusText + ")");

            } else {
                alert("success: " + req.responseType);
                 

            }
        }
        finally {
            btn.removeAttr('disabled');
            btn.find(".spinner-icon").hide();
            btn.find(".spinner-text").show();
        }
        //parent td tr i naci trecu kolonu i promjeniti trenutni broj sa # i brojem kojeg sandro salje


    };

    btn.attr('disabled', 'disabled');
    btn.find(".spinner-icon").show();
    btn.find(".spinner-text").hide();
    req.send();




});

