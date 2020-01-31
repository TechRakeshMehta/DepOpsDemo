function ConfirmCopy(sender, eventArgs) {
 
    var txtPackageName = $find($jQuery("[id$=txtPackageName]")[0].id);
    var cmbCompliancePackageToCopy = $find($jQuery("[id$=cmbCopyPackage]")[0].id);

    if (cmbCompliancePackageToCopy.get_value() == "0") {
        $alert("No package is selected to Copy.");
        sender.set_autoPostBack(false);
        return;
    }


    if (txtPackageName.get_textBoxValue().trim() != "") {
        sender.set_autoPostBack(window.confirm("Are you sure you want to copy this compliance package, its categories and items?"));
    }
    else {
        sender.set_autoPostBack(false);
        $alert("Package Name is required for Copy process.");
    }
}