
function manageItems(type) {
    if (type == 1) {

        //ALL ITEMS FILTER
        $jQuery("[id$=divEditMode]").each(function () {
            $jQuery(this).show();
        });
        $jQuery("[id$=divExceptionMode]").each(function () {
            $jQuery(this).show();
        });
        $jQuery("[id$=pnlExceptionData]").each(function () {
            $jQuery(this).show();
        });
        $jQuery("[id$=pnlItemData]").each(function () {
            $jQuery(this).show();
        });

        // SHOW INCOMPLETE ITEMS IN THIRD PARTY MODE
        $jQuery("[id$=pnlNoItemData]").each(function () {
            $jQuery(this).show();
        });

        $jQuery(".filter-inuse").first().text("Showing all items.");

    }
    else if (type == 2) {

        //FILLED ITEMS FILTER
        $jQuery("[id$=divEditMode]").each(function () {

            var _divType = $jQuery(this).attr("divType");

            // SHOW PENDING FOR REVIEW ITEMS AND HIDE INCOMPLETE ITEMS FOR ADMIN AND CLIENT ADMIN
            if (_divType == 'PNGREV' || _divType == 'ITMAPPREJ') {
                $jQuery(this).show();
            }
            else {
                $jQuery(this).hide();
            }
        });

        // HIDE INCOMPLETE ITEMS FOR THIRD PARTY MODE
        $jQuery("[id$=pnlNoItemData]").each(function () {
            $jQuery(this).hide();
        });

        $jQuery("[id$=pnlExceptionData]").each(function () {
            $jQuery(this).hide();
        });

        // HIDE Exception Applied and Approved Cases, for Client Admin and Admin
        $jQuery("[id$=divExceptionMode]").each(function () {
            $jQuery(this).hide();
        });

        // SHOW APPROVED/REJECTED ITEM IN THE THIRD PARTY
        $jQuery("[id$=pnlItemData]").each(function () {

            var _divType = $jQuery(this).attr("divType");

            if (_divType == 'ITMAPPREJ') {
                $jQuery(this).show();
            }
        });

        $jQuery(".filter-inuse").first().text("Showing only filled items.");
    }

    else if (type == 3) {

        //PENDING FOR REVIEW ITEMS FILTER
        $jQuery("[id$=divEditMode]").each(function () {

            var _divType = $jQuery(this).attr("divType");

            // SHOW PENDING FOR REVIEW ITEMS AND HIDE OTHER ITEMS
            if (_divType == 'PNGREV') {
                $jQuery(this).show();
            }
            else {
                $jQuery(this).hide();
            }
        });

        // HIDE INCOMPLETE ITEMS FOR THIRD PARTY MODE
        $jQuery("[id$=pnlNoItemData]").each(function () {
            $jQuery(this).hide();
        });

        //HIDE THE PNLITEMDATA IN READ ONLY MODE FOR THE THIRD PARTY, WITH APPROVED & REJECTED
        $jQuery("[id$=pnlItemData]").each(function () {

            var _divType = $jQuery(this).attr("divType");

            if (_divType == 'ITMAPPREJ' || _divType == 'EXCAPPREJ' || _divType == 'OTHITMS') {
                $jQuery(this).hide();
            }

        });

        $jQuery("[id$=pnlExceptionData]").each(function () {
            $jQuery(this).hide();
        });

        // HIDE Exception Applied and Approved Cases, for Client Admin and Admin
        $jQuery("[id$=divExceptionMode]").each(function () {
            $jQuery(this).hide();
        });

        $jQuery(".filter-inuse").first().text("Showing pending for review items.");
    }
}

function HideDockUndock() {
    var divDockUndockId = $jQuery('[id$=divDockUnDock]');
    if (divDockUndockId.length > 0) {
        divDockUndockId[0].style.display = "none";
    }
}