function ConfirmCopy(sender, eventArgs) {

    var txtItemName = $find($jQuery("[id$=txtItemName]")[0].id);
    var cmbComplianceItemToCopy = $find($jQuery("[id$=cmbCopyComplianceItem]")[0].id);

    if (cmbComplianceItemToCopy.get_value() == "0") {
        $alert("No compliance item is selected to Copy.");
        sender.set_autoPostBack(false);
        return;
    }
    if (txtItemName.get_textBoxValue().trim() != "") {
        sender.set_autoPostBack(window.confirm("Are you sure you want to copy this compliance item and its attributes?"));
    }
    else {
        sender.set_autoPostBack(false);
        $alert("Item Name is required for Copy process.");
    }
}

