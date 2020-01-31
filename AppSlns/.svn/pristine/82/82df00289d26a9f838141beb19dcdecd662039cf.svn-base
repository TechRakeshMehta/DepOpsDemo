function FilterMenuShowing(sender, eventArgs) {
    if (eventArgs.get_column()._data.DataTypeName == "System.String") {
        var items = eventArgs.get_menu()._itemData;
        //var StringItem = ["NoFilter", "Contains", "DoesNotContain", "StartsWith", "EndsWith", "Between", "NotBetween", "IsEmpty", "NotIsEmpty", "IsNull", "NotIsNull"];
        var StringItem = ["NoFilter", "Contains", "DoesNotContain", "StartsWith", "EndsWith"];
        var i = 0;
        while (i < items.length) {
            var item = eventArgs.get_menu()._findItemByValue(items[i].value);
            for (var j = 0; j < StringItem.length; j++) {
                if (StringItem[j].match(items[i].value)) {
                    if (item != null)
                        item._element.style.display = "block";
                    j = StringItem.length;
                }
                else {
                    item._element.style.display = "none";
                }
            }
            i++;
        }
    }
    else if (eventArgs.get_column()._data.DataTypeName == "System.Int32") {
        var items = eventArgs.get_menu()._itemData;
        var StringItem = ["NoFilter", "EqualTo", "NotEqualTo", "GreaterThan", "LessThan", "GreaterThanorEqualTo", "LessThanorEqualTo"];
        var i = 0;
        while (i < items.length) {
            var item = eventArgs.get_menu()._findItemByValue(items[i].value);
            for (var j = 0; j < StringItem.length; j++) {
                if (StringItem[j].match(items[i].value)) {
                    if (item != null)
                        item._element.style.display = "block";
                    j = StringItem.length;
                }
                else {
                    item._element.style.display = "none";
                }
            }
            i++;
        }
    } else if (eventArgs.get_column()._data.DataTypeName == "System.DateTime") {
        var items = eventArgs.get_menu()._itemData;
        var StringItem = ["NoFilter", "EqualTo", "NotEqualTo", "GreaterThan", "LessThan", "GreaterThanorEqualTo", "LessThanorEqualTo"];
        var i = 0;
        while (i < items.length) {
            var item = eventArgs.get_menu()._findItemByValue(items[i].value);
            for (var j = 0; j < StringItem.length; j++) {
                if (StringItem[j].match(items[i].value)) {
                    if (item != null)
                        item._element.style.display = "block";
                    j = StringItem.length;
                }
                else {
                    item._element.style.display = "none";
                }
            }
            i++;
        }
    }
}  