function addItem() {
    var itemText = FSObject.$("[id$=txtEnterDate]")[0].value;
    var listBox = FSObject.$("[id*=lstCalendar]");
    var item = new Telerik.Web.UI.RadListBoxItem();

    var itemExists = false;

    item.set_text(itemText);
    item.set_selected(true);
    var duplicateCheck = false;
    for (var i = 0; i < listBox.length; i++) {

        if (listBox[0].control.get_items()._array == "") {
            duplicateCheck = false;
            break;
        }
        else {
            if (i < listBox[0].control.get_items()._array.length) {
                if (itemText == listBox[0].control.get_items()._array[i]._text) {
                    duplicateCheck = true;
                    break;
                }
            }
        }
    }
    if (duplicateCheck == true) {
        alert("Duplicate entry");
    }
    //    else {
    //        listBox[0].control.get_items().add(item);
    //        item.scrollIntoView();
    //        return false;
    //    }

    return false;
}

function chkbox() {
    var chk1 = FSObject.$("[id$=chkCopyCalendar]");
    var cmbClient = FSObject.$("[id$=cmbClient]").attr("id");
    var cmbFrom = FSObject.$("[id$=cmbFromClient]").attr("id");
    var hdnQueName = FSObject.$("[id$=hdnQueName]");
    if (chk1[0].checked) {
        $find(cmbFrom).enable();
        $find(cmbClient).disable();
    }
    else {
        if (hdnQueName.val() == "Self") {
            FSObject.$("[id*=cmbFromClient]").val("--SELECT--");
            FSObject.$("[id*=cmbFromClient]").attr('selectedIndex', 0);
            $find(cmbFrom).disable();
            $find(cmbClient).enable();
        }
        if ((hdnQueName.val() == "UserQueue") || (hdnQueName.val() == "ApprovalQueue")) {
            FSObject.$("[id*=cmbFromClient]").val("--SELECT--");
            FSObject.$("[id*=cmbFromClient]").attr('selectedIndex', 0);
            $find(cmbFrom).disable();
        }
    }
}


function deleteItem() {

    var listBox = FSObject.$("[id*=lstCalendar]");
    var selectedItem = listBox[0].control.get_selectedItem();
    if (listBox[0].control.get_items().get_count() < 1) {
        alert("The listBox is empty.");
        return false;
    }

    //    if (!selectedItem) {
    //        alert("You need to select a item first.");
    //        return false;
    //    }

    //    if (listBox[0].control.get_items().get_count()) {
    //        if (!confirm("This is the last item in the listBox. Are you sure you want to delete it?"))
    //            return false;
    //    }

    listBox[0].control.deleteItem(selectedItem);
    var txtbox = FSObject.$("[id$=txtEnterDate]");
    txtbox[0].value = " ";
    return false;
}

function ClearAll() {
    var calendar = FSObject.$("[id*=clientCalender]");
    calendar.unselectDates(calendar.get_selectedDates());
}

function ReBindClients(sender, eventArgs) {
    // $telerik.findControl(document, "cmbFromClient").trackChanges();

    var selectedText = $telerik.findControl(document, "cmbClient").get_selectedItem().get_text();
    if (($telerik.findControl(document, "cmbFromClient").findItemByText(selectedText)) != null) {
        $telerik.findControl(document, "cmbFromClient").get_items().remove($telerik.findControl(document, "cmbFromClient").findItemByText(selectedText));
        //$telerik.findControl(document, "cmbFromClient").commitChanges();
    }
}

var lstDeleting = function (sender, e) {
    if (!confirm("Are you sure you want to remove selected day?")) {
        e.set_cancel(true);
    }
}