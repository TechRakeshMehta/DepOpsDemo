
function ValidateOrg(sender, args) {
    if ($jQuery("#" + window.controlID).val().length <= 0) {
        args.IsValid = false;
    }
    args.IsValid = true;
}

function FilterMenuShowing(sender, eventArgs) {
    var counter;
    var item;
    var menu;
    var items;
    if (eventArgs.get_column().get_uniqueName() == "Active") {
        menu = eventArgs.get_menu();
        items = menu._itemData;
        counter = 0;
        while (counter < items.length) {
            if (items[counter].value != "NoFilter" && items[counter].value != "EqualTo") {
                item = menu.findItemByText(items[counter].value);
                item.get_element().style.cssText = "display: none;";
            }

            counter++;
        }
    }
    else {
        menu = eventArgs.get_menu();
        items = menu._itemData;
        counter = 0;
        while (counter < items.length) {
            if (items[counter].value != "NoFilter" && items[counter].value != "EqualTo") {
                item = menu.findItemByText(items[counter].value);
                item.get_element().style.cssText = "display: block;";
            }
            counter++;
        }
    }
}