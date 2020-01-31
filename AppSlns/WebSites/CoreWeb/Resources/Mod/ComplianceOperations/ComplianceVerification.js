var requiredCountItem;
var requiredCountException;
var requiredCountCatException;

function verifyRejection(sender, eventargs) {
    requiredCountCatException = 0;
    requiredCountItem = 0;
    requiredCountException = 0;

    validateCatExceptionRejectionReason();
    validateItemRejectionReason();
    validateExceptionRejectionReason();

    if (requiredCountCatException > 0) {
        sender.set_autoPostBack(false);
        $alert("Please enter the rejection reason for the category exception.");
    }

    if (requiredCountItem > 0) {
        sender.set_autoPostBack(false);
        $alert("Please enter the rejection reason for the item.");
    }

    if (requiredCountException > 0) {
        sender.set_autoPostBack(false);
        $alert("Please enter the rejection reason for the exception.");
    }

    if (requiredCountCatException == 0 && requiredCountItem == 0 && requiredCountException == 0) {
        //To check delete items
        //var IsChecked = false;
        var checkedItemsCount = 0;
        //UAT-850:WB: As an admin, I should be able to Delete a category exception/exception request
        var checkedCatException = 0;
        var confirmationMessage = "";
        $jQuery("[id$=chkDeleteCatException]").each(function () {
            if ($jQuery(this).is(':checked') == true) {
                checkedCatException += 1;
            }
        });

        $jQuery("[id$=chkDeleteItem]").each(function () {
            if ($jQuery(this).is(':checked') == true) {
                checkedItemsCount += 1;
                //IsChecked = true;
                //return false; //break;
            }
        });
        //UAT-850:WB: As an admin, I should be able to Delete a category exception/exception request
        confirmationMessage = 'Are you sure you want to delete ';
        if (checkedCatException > 0) {
            confirmationMessage = confirmationMessage + 'category exception';
        }
        if (checkedCatException > 0 && checkedItemsCount > 0) {
            confirmationMessage = confirmationMessage + ' and ';
        }
        else
        {
            if (checkedCatException > 0 && checkedItemsCount <= 0)
                confirmationMessage = confirmationMessage + '?';

        }
        if (checkedItemsCount > 0) {
            confirmationMessage = confirmationMessage + checkedItemsCount + ' item(s)?';
        }
        //if any item is going to be deleted then show alert message
        if (checkedItemsCount > 0 || checkedCatException > 0) {
            //$alert("Are you sure you want to delete item(s)?");
            //var isOKClicked = confirm('Are you sure you want to delete ' + checkedItemsCount + ' item(s)?');
            var isOKClicked = confirm(confirmationMessage);
            if (isOKClicked)
                sender.set_autoPostBack(true);
            else
                sender.set_autoPostBack(false);
        }
        else
            sender.set_autoPostBack(true);
    }
}

function validateCatExceptionRejectionReason() {
    var exceptionCode = $jQuery("[id$=hdfCatRejectionCodeException]").val();

    $jQuery("[id$=rbtnActions]").each(function () {
        var selectedValue = $jQuery(":checked", this).val();

        if (selectedValue != undefined && selectedValue == exceptionCode) {
            var currentActionButtonItemId = $jQuery(this).attr("exActionItemId");

            $jQuery("[id$=txtAdminNotes]").each(function () {
                var currentNotesItemId = $jQuery(this).attr("exNoteItemId");
                var _currentStatusCode = $jQuery(this).attr("exCurrentStatus");

                if (currentNotesItemId == currentActionButtonItemId && _currentStatusCode != exceptionCode) {
                    if ($jQuery(this).val().trim().length == 0) {
                        requiredCountCatException += 1;
                    }
                }
            })
        }
    });
}

function validateItemRejectionReason() {
    $jQuery("[id$=rbtnListAction]").each(function () {
        var selectedValue = $jQuery(":checked", this).val();

        var _itemRejectionCode = $jQuery("[id$=hdfRejectionCodeItem]").val();

        if (selectedValue != undefined && selectedValue == _itemRejectionCode) {
            var currentActionButtonItemId = $jQuery(this).attr("actionItemId");

            $jQuery("[id$=txtAdminNote]").each(function () {
                var currentNotesItemId = $jQuery(this).attr("noteItemId");

                if (currentNotesItemId == currentActionButtonItemId) {
                    var _currentStatusCode = $jQuery(this).attr("currentStatus");
                    if ($jQuery(this).val().trim().length == 0 && _currentStatusCode != _itemRejectionCode) {
                        requiredCountItem += 1;
                    }
                }
            })
        }
    });
}

function validateExceptionRejectionReason() {
    var exceptionCode = $jQuery("[id$=hdfRejectionCodeException]").val();

    $jQuery("[id$=rbtnActions]").each(function () {
        var selectedValue = $jQuery(":checked", this).val();

        if (selectedValue != undefined && selectedValue == exceptionCode) {
            var currentActionButtonItemId = $jQuery(this).attr("exActionItemId");

            $jQuery("[id$=txtAdminNotes]").each(function () {
                var currentNotesItemId = $jQuery(this).attr("exNoteItemId");
                var _currentStatusCode = $jQuery(this).attr("exCurrentStatus");

                if (currentNotesItemId == currentActionButtonItemId && _currentStatusCode != exceptionCode) {
                    if ($jQuery(this).val().trim().length == 0) {
                        requiredCountException += 1;
                    }
                }
            })
        }
    });
}

var ApplicantPackageComplianceStatus = {
    Compliant: 'COMP',
    Not_Compliant: 'NCMP'
}

function ChangeApplicantPanelData(newCatImgUrl, newCatImageTooltip, packageStatusImgUrl, ApplicantPackageComplianceStatus, IsSavedSuccess) {
    ChangeCategoryImageUrl(newCatImgUrl, newCatImageTooltip);
    ChangeComplianceImageUrl(packageStatusImgUrl, ApplicantPackageComplianceStatus);

    //    $jQuery("div.dvlbllastupdatedby #spnlbllastupdatedby").text(lbllastupdatedby);
    var lblMessageBox = $jQuery("div#dvMsgBox [id$=lblMessage]");
    if (IsSavedSuccess == 'True') {
        //.val(userMessage);
        lblMessageBox.show();
        lblMessageBox.text("Item(s) updated successfully.");
        lblMessageBox.addClass("sucs");
    }
    //else if (isDataNeedToSaveForIncompleteItm == 'True') {
    //    lblMessageBox.text("An item with data entered can not be incomplete.");
    //    lblMessageBox.addClass("error");
    //    lblMessageBox.show();
    //}
    else {
        lblMessageBox.text("Some of the items could not be saved.");
        lblMessageBox.addClass("error");
        lblMessageBox.show();
    }
    //userMessage
}

function ChangeComplianceImageUrl(packageStatusImgUrl, packageComplianceStatus) {
    var dvComplianceInfo = $jQuery("div.compliance_info");

    var originalImg = dvComplianceInfo.find("img.img_status");
    var originalImgURL = originalImg.attr('src');
    if (originalImg != packageStatusImgUrl) {
        originalImg.attr('src', packageStatusImgUrl);
        if (packageComplianceStatus == ApplicantPackageComplianceStatus.Compliant.toLowerCase()) {
            dvComplianceInfo.find("span.c_val").find('span[id$=lblOverComplianceStatus]').text('Compliant');
            dvComplianceInfo.find("span.c_val").find('span[id$=lblOverComplianceStatus]').css('color', 'green');
        }
        else {
            dvComplianceInfo.find("span.c_val").find('span[id$=lblOverComplianceStatus]').text('Not Compliant');
            dvComplianceInfo.find("span.c_val").find('span[id$=lblOverComplianceStatus]').css('color', 'red');
        }
    }
}

//function ChangeCategoryImageUrl(newImgUrl, newCatImageTooltip) {
//    var liCatItems = $jQuery("[id$=lstCategories]").find("li");

//    $jQuery(liCatItems).each(function () {
//        if ($jQuery(this).hasClass('rlbSelected')) {
//            var originalImgUrl = $jQuery(this).find('[id$=imgStatus]').attr('src');
//            if (originalImgUrl != newImgUrl) {
//                $jQuery(this).find('[id$=imgStatus]').attr('src', newImgUrl);
//                $jQuery(this).find('[id$=imgStatus]').attr('title', newCatImageTooltip);
//                return;
//            }
//        }
//    });
//}

function ChangeCategoryImageUrl(newImgUrl, newCatImageTooltip) {
    
    var PackageSubscriptionId = $jQuery("[id$=hdnPackageSubscriptionId]").val();
    var TenantId = $jQuery("[id$=hdnTenantId]").val();
    $jQuery.ajax({
        type: "POST",
        url: "Default.aspx/GetCategoryUpdatedUrl",
        data: "{'PackageSubscription':'" + PackageSubscriptionId + "','Tenant':'" + TenantId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //debugger;
            var liCatItems = $jQuery("[id$=lstCategories]").find("li");
            $jQuery(liCatItems).each(function () {
                //debugger;
                var CategoryId = $jQuery(this).find("a")[0].attributes["categoryid"].value;
                var originalImgUrl = $jQuery(this).find('[id$=imgStatus]').attr('src');
                var imgStatus = $jQuery(this).find('[id$=imgStatus]');

                response.d.forEach(function (i) {
                    //debugger;
                    var newImgUrl = i.Item2;
                    var newCatImageTooltip = i.Item3;
                    if (i.Item1 == CategoryId)
                    {
                        if (originalImgUrl != newImgUrl) {
                            imgStatus.attr('src', newImgUrl);
                            imgStatus.attr('title', newCatImageTooltip);
                        }
                    }
                });
            });
        },
        failure: function (response) {
            //debugger;
        }
    });


    //$jQuery(liCatItems).each(function () {
    //    var CategoryId = $jQuery(this).find("a")[0].attribute["categoryid"].value;

    //    var originalImgUrl = $jQuery(this).find('[id$=imgStatus]').attr('src');
    //    //if ($jQuery(this).find("a")) {
    //    //    var originalImgUrl = $jQuery(this).find('[id$=imgStatus]').attr('src');
    //    //    if (originalImgUrl != newImgUrl) {
    //    //        $jQuery(this).find('[id$=imgStatus]').attr('src', newImgUrl);
    //    //        $jQuery(this).find('[id$=imgStatus]').attr('title', newCatImageTooltip);
    //    //        return;
    //    //    }
    //    //}
    //});
}

function ChangePdfDocVwrScroll(pageID, uni_doc_val) {
    if (parent.pdfDocViewerChildWnd != undefined && parent.pdfDocViewerChildWnd != null) {
        parent.pdfDocViewerChildWnd.change_pdf_scrollpos(pageID, uni_doc_val);
    }
    else if (iframeInstance) {
        iframeInstance[0].contentWindow.change_pdf_scrollpos(pageID, uni_doc_val);
    }
}

function get_document_callback(result) {
    if (result == null) {
        var _mesage = "Document conversion and merging is in progress. Please retry after some time."
        if (parent.pdfDocViewerChildWnd != null && parent.pdfDocViewerChildWnd != undefined) {
            parent.pdfDocViewerChildWnd.ShowPdfMessage(_mesage);
        }
        else {
            var iFrame = $jQuery("[id$=iframePdfDocViewer]");
            if (iFrame.length != 0 && iFrame.contents().find("#lblPdfMessage").length != 0) {
                $jQuery("[id$=iframePdfDocViewer]").contents().find("#lblPdfMessage")[0].innerText = _mesage;
                $jQuery("[id$=iframePdfDocViewer]").contents().find("#lblPdfMessage").addClass("info");
                //$jQuery("#dvMsgBox").show();
                //$jQuery("#dvMsgBox").css("display", "block");
                $jQuery("[id$=iframePdfDocViewer]").contents().find("#dvMsgBox").show();
                $jQuery("[id$=iframePdfDocViewer]").contents().find("#dvMsgBox").css("display", "block");
            }
        }
    }
    else {
        var startPageNumber = result.split(';');
        ChangePdfDocVwrScroll(startPageNumber[0], startPageNumber[1]);
        return false;
    }
}

function GetFailedUnifiedDocument_CallBack(result) {
    if (result != null && result != undefined) {
        result = $jQuery.parseJSON(result)
        var iFrame = $jQuery("[id$=iframeDocViewer]");
        if (iFrame.length != 0) {
            iFrame.attr('src', result.redirectUrl);
        }
    }
}

//UAT-1538
function GetSingleDocument_CallBack(result) {
    if (result != null && result != undefined) {
        result = $jQuery.parseJSON(result)
        //var iFrame = $jQuery("[id$=iframeDocViewer]");
        var iFrame = $jQuery("[id$=iframePdfDocViewer]")
        if (iFrame.length != 0) {
            $jQuery("[id$=hdnDocVwr]").val(result.redirectUrl);
            $jQuery("[id$=hdnSingleDocVwr]").val(result.redirectUrl);
            if (parent.pdfDocViewerChildWnd != null) {
                parent.pdfDocViewerChildWnd.location = result.redirectUrl;
            }
            else { iFrame[0].src = result.redirectUrl; }
        }
    }
}

//UAT-1559
//function GetSingleDocument_ViewDoc(result) {

//    if (result != null && result != undefined) {
//        var iFrame = $jQuery("[id$=iframePdfDocViewer]")
//        if (iFrame.length != 0) {
//            $jQuery("[id$=hdnDocVwr]").val(result);
//            if (parent.pdfDocViewerChildWnd != null) {
//                parent.pdfDocViewerChildWnd.location = result;
//            }
//            else { iFrame[0].src = result; }
//        }
//    }
//}

//UAT-2525
function IncompleteItemHasDataInfoMessage()
{
    var lblMessageBox = $jQuery("div#dvMsgBox [id$=lblMessage]");
    lblMessageBox.text("An item with data entered can not be incomplete.");
    lblMessageBox.addClass("error");
    lblMessageBox.show();
}
