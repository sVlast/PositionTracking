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


var parentElem = $("b").parents()
    .map(function () {
        return this.tagNam;
    })


$('.btn-refresh').click(function (e) {
    var btn = $(this);
    var id = btn.attr("data-id");
    var rating = btn.attr("data-rating");
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
                var rating = req.response.rating;
                $("#" + btn.parent().parent().find("td")[2]).text(rating);


            }
        }
        finally {
            btn.removeAttr('disabled');
            btn.find(".spinner-icon").hide();
            btn.find(".spinner-text").show();
        }
    };

    btn.attr('disabled', 'disabled');
    btn.find(".spinner-icon").show();
    btn.find(".spinner-text").hide();
    req.send();

});



 //var id = btn.attr("data-id");

/*

$.getJSON() //Load a JSON with GET method.
load():     //Load a piece of html into a container DOM.

*/