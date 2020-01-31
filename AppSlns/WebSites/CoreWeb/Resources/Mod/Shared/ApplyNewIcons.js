$page.add_pageLoad(function () {
    $jQuery(".rbPrimaryIcon.rbSave").removeClass().addClass("fa fa-floppy-o");
    $jQuery(".rbPrimaryIcon.rbCancel").removeClass().addClass("fa fa-ban");
    $jQuery(".rbPrimaryIcon.rbAdd").removeClass().addClass("fa fa-plus plus-color");
    $jQuery(".rbPrimaryIcon.rbRefresh").removeClass().addClass("fa fa-refresh refresh-color");
    $jQuery(".rbUndo").removeClass().addClass("fa fa-undo");
    $jQuery(".rbPrimaryIcon.rbSearch").removeClass().addClass("fa fa-search ");
    $jQuery(".rbArchive").removeClass().addClass("fa fa-archive");
    $jQuery(".rbUnArchive").removeClass().addClass("fa fa-archive");
    $jQuery(".rbPrimaryIcon.icnClearFilter").removeClass().addClass("fa fa-filter filter-color");
    $jQuery(".rbPrimaryIcon.rbOpen").removeClass().addClass("fa fa-download download-color");
    $jQuery(".rbPrimaryIcon.rbResett").removeClass().addClass("fa fa-undo");
    $jQuery(".rbPrimaryIcon.rbReset").removeClass().addClass("fa fa-undo");
    $jQuery(".rbReturn").removeClass().addClass("fa fa-mail-reply");
    $jQuery(".rbPrimaryIcon.rbUndo").removeClass().addClass("fa fa-undo");
    $jQuery(".rbPrimaryIcon.rbEdit").removeClass().addClass("fa fa-edit");
    $jQuery(".rbPrimaryIcon.rbClose").removeClass().addClass("fa fa-close");
    $jQuery(".rbPrimaryIcon.rbAddNew").removeClass().addClass("fa fa-plus plus-color smFornt");
    $jQuery(".rbPrimaryIcon.rbAdd").removeClass().addClass("fa fa-plus plus-color");
    $jQuery(".rbPrimaryIcon.rbCreateVersion").removeClass().addClass("fa fa-code-fork");
    $jQuery(".btn-primary.rbEditPKG").addClass("fa fa-pencil-square-o");
    $jQuery(".rbPrimaryIcon.rbClone").removeClass().addClass("fa fa-clone");
    $jQuery(".rbPrimaryIcon.icnreset").removeClass().addClass("fa fa-mail-reply");
    $jQuery(".rbEnvelope").removeClass().addClass("fa fa-envelope-o");
    $jQuery(".rbEnvelope2").removeClass().addClass("fa fa-envelope");
    $jQuery(".rbPrimaryIcon.rbArchive").removeClass().addClass("fa fa-archive");
    $jQuery(".rbPrimaryIcon.rbUsers").removeClass().addClass("fa fa-users");
    $jQuery(".rbPrimaryIcon.rbRefresh2").removeClass().addClass("fa fa-refresh");
    $jQuery(".rbPrimaryIcon.rbAssign").removeClass().addClass("fa fa-sign-in");
    $jQuery(".rbPrimaryIcon.rbUnassign").removeClass().addClass("fa fa-sign-out");
    $jQuery(".rbPrimaryIcon.btnPlane").removeClass().addClass("fa fa-paper-plane");
    $jQuery(".rbPrimaryIcon.rbSaveRotation").removeClass().addClass("fa fa-floppy-o");
    $jQuery(".rbPassport").removeClass().addClass("fa fa-line-chart");
    $jQuery(".rbPrimaryIcon.btnsave").removeClass().addClass("fa fa-floppy-o");

    $jQuery("caption").remove();
    $jQuery(".calanderFontSetting").parent().parent().css({ 'overflow-x': 'hidden', 'width': '212px' });
    $jQuery(".rcCalPopup").removeClass().empty().addClass("fa fa-calendar font22 calender-icon");
    $jQuery(".rcTimePopup").removeClass().empty().addClass("fa fa-clock-o font22 calender-icon");

    $jQuery(".rbExport").removeClass().addClass("fa fa-file-text");
    $jQuery(".rbSummary").removeClass().addClass("fa fa-bar-chart");
    $jQuery(".rbPrint").removeClass().addClass("fa fa-print");
    $jQuery(".rbUpload").removeClass().addClass("fa fa-upload");
    $jQuery(".rbSaveNotes").removeClass().addClass("fa fa-sticky-note-o");
    $jQuery(".rbAttest").removeClass().addClass("fa fa-eye");
    $jQuery(".resultReport").removeClass().addClass("fa fa-line-chart");

    $jQuery(".rbPrimaryIcon.rbNo").removeClass().addClass("fa fa-remove remove-color");
    $jQuery(".rbPrimaryIcon.rbNext").removeClass().addClass("fa fa-arrow-right right-arrow-color");
    $jQuery(".rbPrimaryIcon.rbOk").removeClass().addClass("fa fa-check green");

    $jQuery(".removeExtraSpace table thead tr.rgCommandRow:first").find('table').remove();
    $jQuery("table").find('tr.rgRow').find('.rgCollapse').parent().parent().addClass('rgRowGrey')
    $jQuery("table").find('tr.rgAltRow').find('.rgCollapse').parent().parent().addClass('rgRowGrey')
    $jQuery("table table").find('tr.rgRow').find('.rgCollapse').parent().parent().addClass('rgRowBlue')
    $jQuery("table table").find('tr.rgAltRow').find('.rgCollapse ').parent().parent().addClass('rgRowBlue')
    $jQuery("table").find('tr.rgRow').find('.rgExpand').parent().parent().addClass('rgRowGrey')
    $jQuery("table").find('tr.rgAltRow').find('.rgExpand').parent().parent().addClass('rgRowGrey')
    $jQuery("table table").find('tr.rgRow').find('.rgExpand').parent().parent().addClass('rgRowBlue')
    $jQuery("table table").find('tr.rgAltRow').find('.rgExpand').parent().parent().addClass('rgRowBlue')

    $jQuery(".rbPrimaryIcon.rbReset").removeClass().addClass("fa fa-undo");

    //ViewAgencyJobPost
    $jQuery(".rbPrimaryIcon.rbAssign").removeClass().addClass("fa fa-sign-in");
    $jQuery(".rbPrimaryIcon.rbUnassign").removeClass().addClass("fa fa-sign-out");
    $jQuery(".rbEnvelope2").removeClass().addClass("fa fa-envelope");

    //RotationPackageCategoryDetailPopUp
    $jQuery(".rbPrimaryIcon.btnPlane").removeClass().addClass("fa fa-paper-plane");

    //RotationDocuments
    $jQuery(".rbPrimaryIcon.rbSaveRotation").removeClass().addClass("fa fa-floppy-o");
    //StudentBucketAssignment
    $jQuery(".rbPrimaryIcon.rbUsers").removeClass().addClass("fa fa-users");
    $jQuery(".rbPrimaryIcon.rbRefresh2").removeClass().addClass("fa fa-refresh");
    //UpcomingExpirationsSearchControl
    $jQuery(".rbPassport").removeClass().addClass("fa fa-line-chart");
    //OrderDetailPage
    $jQuery(".rbPrimaryIcon.rbReview").removeClass().addClass("fa fa-check");
    $jQuery(".rbPrimaryIcon.rbRollback").removeClass().addClass("fa fa-undo");
    $jQuery(".rbPrimaryIcon.Retweet").removeClass().addClass("fa fa-retweet");

});