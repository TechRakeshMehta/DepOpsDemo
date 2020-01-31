

function ValidateMapping(sender, args) {
    if (Page_ClientValidate() == true) {
        sender.set_autoPostBack(false);
        PageMethods.ValidatePackageMapping(Validate);

        function Validate(d) {
            if (d.ValidationMessage.length > 0 && d.IfReviewMappingCanBeSkipped == false) {
                $alert(d.ValidationMessage);
                sender.set_autoPostBack(false);
            }
            else if (d.ValidationMessage.length == 0 && d.IsReviewRequiredEveryTransaction == true && d.IfReviewMappingCanBeSkipped == false) {
                if (window.confirm("Have you reviewed the mapping ?")) {
                    __doPostBack('ctl00$DefaultContent$ucDynamicControl$btnSave', '');
                }
            }
            else {
                __doPostBack('ctl00$DefaultContent$ucDynamicControl$btnSave', '');
            }
        }
    }
}
 