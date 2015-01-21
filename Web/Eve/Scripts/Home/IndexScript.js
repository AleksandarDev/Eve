$(document).ready(function () {
    $("#accountName").hover(function () {
        $("#accountMenu").stop(true, true).slideDown("slow");
    });
    $("#accountBar").hover(function () {
    }, function () {
        $("#accountMenu").stop(true, true).slideUp("slow");
    });
});
