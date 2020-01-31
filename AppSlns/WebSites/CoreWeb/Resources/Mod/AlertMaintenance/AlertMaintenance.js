Type.registerNamespace("Telerik.Web.UI");

var time;
function redirect() {
    //Redirect
    var url = $jQuery("[id$=hdnIsRedirect]").val();
    if (url.length > 0) {
        window.location = url;
        clearTimeout(time);
    }
}
function fnShowMessage() {
    //    alert('in Inspection Service----');
    time = window.setTimeout("redirect();", 3000);
    return true;
}