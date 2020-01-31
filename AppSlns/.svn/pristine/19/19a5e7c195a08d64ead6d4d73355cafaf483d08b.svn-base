/*-------------Parameter Details---------
 1) IncludeDocumentBody can be true or false. Should be passed as true if Enter press on document body is also to be captured.
 2) SectionID is Id of the section whose Enter key is to be captured.
 3) Name of the button whose click should be called when Enter Key is pressed. 
    In case of commandBar, it should be in following format : commandBarNme_ButtonType. e.g. cmdName_btnSearch
*/
function SetDefaultButtonForSection(SectionID, ButtonName, IncludeDocumentBody) {
    $jQuery(document).unbind("keypress");
    $jQuery(document).keypress(function (e) {
        SetDefaultButton(e, SectionID, ButtonName, IncludeDocumentBody);
    });
}

function SetDefaultButton(e, SectionID, ButtonName, IncludeDocumentBody) {
    //check if enter was clicked
    if (e.which == 13) {

        //find currently focussed element
        var focusElementID = document.activeElement.id;

        // find passed SectionID element in parent of focussed Element
        var focusElement = $jQuery("#" + focusElementID).parents().closest($jQuery("[id$=" + SectionID + "]"));

        if (focusElement.length > 0 || (IncludeDocumentBody && focusElementID == "")) {

            //prevent default functionality of Enter
            e.preventDefault();
            e.stopPropagation();

            //Click the button
            $jQuery("[id$=" + ButtonName + "]").click();
        }
    }
}