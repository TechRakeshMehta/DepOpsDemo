///<reference path="/Resources/Generic/ref.js"/>

$page.add_pageReady(function () {
    $jQuery("span.reqd").html("").parent("label").addClass("req");
});