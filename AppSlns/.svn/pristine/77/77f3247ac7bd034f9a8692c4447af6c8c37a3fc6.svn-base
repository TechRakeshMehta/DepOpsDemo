function SearchButtonClicked() {
    $jQuery("[id$=hdnIsSearchClicked]").val('true');
}

function OpenCustomControlPopUp() {    
    var tenantIDs = $jQuery("[id$=hdnSelectedTenantIds]").val();
    if (tenantIDs != undefined && tenantIDs != null && tenantIDs != '') {

        var url = $page.url.create("~/ClinicalRotation/Pages/RotationCustomAttributes.aspx?TenantIDs=" + tenantIDs);
        var title = "Requirement Custom Control";

        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url,
                                             {
                                                 size: "1000," + popupHeight,
                                                 behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnCustomControlPopUpClose
                                             },
                                             function () {
                                                 this.set_title("American Databank | " + title);
                                                 this.set_destroyOnClose(true);
                                                 this.set_status("");
                                             });

        
        parent.rotationCustomAttributeWindow = win;

        parent.$jQuery('.rwMaximizeButton').on('click', function () {
            setTimeout(function () {
                parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
            }, 50);
        });

        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 650);

        parent.$jQuery('.rwTitlebarControls').on('click', function () {
            setTimeout(function () {
                parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
            }, 40);
        });
    }
    else {
        $alert('Please select institution first.');
    }
}

function OnCustomControlPopUpClose(oWnd, args) {    
    var arg = args.get_argument();    
    oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
    oWnd.remove_close(OnCustomControlPopUpClose);    
    if ($jQuery("#lblCustomFields").length > 0) {
        if (parent.$jQuery("#hdnIsCustomFieldValue").val().trim().toLowerCase() == 'true') {
            $jQuery("#lblCustomFields").text('Filter applied');
            $jQuery("#txtCustomFieldsFilter").val('Filter applied');            
        }
        else {
            $jQuery("#lblCustomFields").text('No filter applied');
            $jQuery("#txtCustomFieldsFilter").val('No filter applied');
        }
    }
}

function OnCmbTenantSelectedIndexChanged(sender, args) {
    var selectedItems = radComboBoxSelectedIdList(sender);
    $jQuery("[id$=hdnSelectedTenantIds]").val(selectedItems.join(","));
}

function radComboBoxSelectedIdList(sender) {
    var selectedIdList = [];
    var combo = sender;
    var items = combo.get_items();
    var checkedIndices = items._parent._checkedIndices;
    var checkedIndicesCount = checkedIndices.length;
    for (var itemIndex = 0; itemIndex < checkedIndicesCount; itemIndex++) {
        var item = items.getItem(checkedIndices[itemIndex]);
        selectedIdList.push(item._properties._data.value);
    }
    return selectedIdList;
}


//UAT-3211
function AdvancedSearchPanelClick() {
    var classValues = $jQuery("[id$=mhdrPanel]").attr('class');
    var hdnAdvanceSearch = $jQuery("[id$=hdnAdvancesearch]");
    if (classValues == "mhdr colps") {
        hdnAdvanceSearch.val('true');
    }
    else {
        hdnAdvanceSearch.val('false');
    }
}