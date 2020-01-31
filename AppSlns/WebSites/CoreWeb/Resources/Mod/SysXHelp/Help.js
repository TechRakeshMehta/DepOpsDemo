
function selectDeselectALL(objCheck, chkSelect) {
    var chkHeader = document.getElementById(objCheck);

    var inputCtrls = document.getElementsByTagName("input");
    if (chkHeader.checked == true) {
        // Iterate through loop in order to search input controls
        for (var i = 0; i != inputCtrls.length; i++) {
            if (inputCtrls[i].id.indexOf(chkSelect) != -1) {
                inputCtrls[i].checked = true;
            }
        }
    }
    else if (chkHeader.checked == false) {
        // Iterate through loop in order to search input controls
        for (var i = 0; i != inputCtrls.length; i++) {
            if (inputCtrls[i].id.indexOf(chkSelect) != -1) {
                inputCtrls[i].checked = false;
            }
        }
    }
}