var btnShowExportOptionsGridDataClicked = false;
var isPageNumberClicked = false;
var clickedPageNumber;
var isClickedFromHeader;
var isPrevNextNavClicked = false;
var isPageSizeChanged = false;

$page.add_pageLoad(function () {
    applyGridAccessibility();
});

function onGridCreated() {
    if (btnShowExportOptionsGridDataClicked) {
        $jQuery('[id$=btnShowExportOptionsGridData]').get(0).focus();
        btnShowExportOptionsGridDataClicked = false;
    }

    if (isPageNumberClicked) {
        isPageNumberClicked = false;

        if (isClickedFromHeader)
            $jQuery(".rgNumPart a span:contains('" + clickedPageNumber + "')").parent().get(0).focus();
        else
            $jQuery(".rgNumPart a span:contains('" + clickedPageNumber + "')").parent().get(1).focus();
    }

    if (isPrevNextNavClicked) {

        isPrevNextNavClicked = false;

        if (isClickedFromHeader)
            $jQuery("input[title*='" + clickedPageNumber + "']").get(0).focus();
        else
            $jQuery("input[title*='" + clickedPageNumber + "']").get(1).focus();
    }

    if (isPageSizeChanged) {
        isPageSizeChanged = false;
        if (isClickedFromHeader)
            $jQuery("div[id$='PageSizeComboBox']:visible .rcbReadOnly .radPreventDecorate").get(0).focus();
        else
            $jQuery("div[id$='PageSizeComboBox']:visible .rcbReadOnly .radPreventDecorate").get(1).focus();
    }
}

function applyGridAccessibility() {

    $jQuery('[id$=btnShowExportOptionsGridData]').unbind('click');
    $jQuery('[id$=btnCloseExport]').unbind('click');
    $jQuery('[id$=btnBeingExporting]').unbind('click');


    //Focus on download button, when pressed
    $jQuery('[id$=btnShowExportOptionsGridData]').on('click', function () {
        btnShowExportOptionsGridDataClicked = true;
    });

    $jQuery('[id$=btnShowExportOptionsGridData]').on('keydown', function (e) {
        if (e.keyCode == 13) {
            btnShowExportOptionsGridDataClicked = true;
        }
    });

    //Focus on download button, Close button pressed (from Export option rows)
    $jQuery('[id$=btnCloseExport]').on('click', function () {
        btnShowExportOptionsGridDataClicked = true;
    });

    $jQuery('[id$=btnCloseExport]').on('keydown', function (e) {
        if (e.keyCode == 13) {
            btnShowExportOptionsGridDataClicked = true;
        }
    });

    //Focus on download button, after download configured document
    $jQuery('[id$=btnBeingExporting]').on('click', function () {
        btnShowExportOptionsGridDataClicked = true;
    });

    $jQuery('[id$=btnBeingExporting]').on('keydown', function (e) {
        if (e.keyCode == 13) {
            btnShowExportOptionsGridDataClicked = true;
        }
    });

    //Altering page number links title
    $jQuery(".rgNumPart a").each(function (element) {
        var pageNumber = $jQuery(this).children().text();
        if (!isNaN(parseInt(pageNumber))) {
            $jQuery(this).attr('title', "Go to page " + pageNumber);
        }
    });

    //Adding title for page size combo box 
    $jQuery('[id$=PageSizeComboBox_Input]').attr('title', 'Change page size control');

    //Click Event of page number
    $jQuery(".rgNumPart a").on('click', function () {
        isPageNumberClicked = true;
        clickedPageNumber = $jQuery(this).children().text();
        if ($jQuery(this).parents('thead').length > 0) {
            isClickedFromHeader = true;
        }
    });

    //Prev-Next Nav Click
    $jQuery(".rgPageNext, .rgPageLast, .rgPagePrev, .rgPageFirst").on('click', function () {
        isPrevNextNavClicked = true;
        clickedPageNumber = $jQuery(this).attr('title');
        if ($jQuery(this).parents('thead').length > 0) {
            isClickedFromHeader = true;
        }
        else {
            isClickedFromHeader = false;
        }
    });

    //Click Event for Page Size - Change
    $jQuery("div[id$='PageSizeComboBox']:visible div.rcbScroll ul.rcbList li.rcbItem").on('click', function () {
        isPageSizeChanged = true;
        if ($jQuery("div[id$='PageSizeComboBox']:visible").parents('thead').length > 0) {
            isClickedFromHeader = true;
        }
        else {
            isClickedFromHeader = false;
        }
    });

    $jQuery(".rgFilterRow .rgFilterBox").each(function (i, e) {
        var lblID = "Filter_" + i;
        var lblText = $jQuery($jQuery(this)[0].id.split("_")).last()[0];
        $jQuery(this).parent().append("<label id='" + lblID + "' class='sr-only'>" + lblText + "</label>");
        $jQuery(this).attr('aria-labelledby', lblID);
    });

}