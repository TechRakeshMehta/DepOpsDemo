$page.add_pageLoad(function () {

    $jQuery(".rbPrimaryIcon.rbnew").removeClass().addClass("fa fa-floppy-o");
    //$jQuery(".rbPrimaryIcon.rbSave").removeClass().addClass("fa fa-floppy-o");
    //$jQuery(".rbPrimaryIcon.rbCancel").removeClass().addClass("fa fa-ban");
    //$jQuery(".rbPrimaryIcon.icnClearFilter").removeClass().addClass("fa fa-filter filter-color");
    //$jQuery("caption").remove();
    //$jQuery(".calanderFontSetting").parent().parent().css({ 'overflow-x': 'hidden', 'width': '212px' });

    //debugger;
    AddColumnConfiguration();

    //on clicking column configuration we are dispaying column configuration div.
    $jQuery('.containsColumnsConfiguration .ColConfigBtn').on('click', function () {
        var ExportDiv = $jQuery(".containsColumnsConfiguration .WclGrid-ExportOptions");
        if (ExportDiv.length == 0 || ExportDiv.length == undefined) {
            ExportDiv.empty();
            var ColumnConfig = $jQuery("[id$=dvColumnsConfiguration]").css("display", "block");

            $jQuery(".containsColumnsConfiguration .grdCmdBar").after(ColumnConfig);

        }
    });
});

//Add the column configuartion span / Btn inside the grid near download button.
function AddColumnConfiguration() {
    //debugger;
    //$jQuery(".containsColumnsConfiguration .grdCmdGrp").append("");
    $jQuery(".containsColumnsConfiguration .grdCmdGrp").append
        ("<span class='ColConfigBtn rbText rbPrimary' style='cursor:pointer; padding-left:6px; padding-bottom:2px;'><span class='fa fa-cog Configuration-Color'></span>Column(s) Configuration</span>");
}