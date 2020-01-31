var minDate = new Date("01/01/1980");
function SetMinDate(picker) {
    picker.set_minDate(minDate);
}

function SetMinEndDate(picker) {
    var date = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
    if (date != null) {
        picker.set_minDate(date);
    }
    else {
        picker.set_minDate(minDate);
    }
}

function CorrectStartToEndDate(picker) {
    var date1 = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
    var date2 = $jQuery("[id$=dpEndDate]")[0].control.get_selectedDate();
    if (date1 != null && date2 != null) {
        if (date1 > date2)
            $jQuery("[id$=dpEndDate]")[0].control.set_selectedDate(null);
    }
}