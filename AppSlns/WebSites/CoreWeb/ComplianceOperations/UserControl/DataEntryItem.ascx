<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEntryItem.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.DataEntryItem" %>
<%@ Register TagPrefix="uc" TagName="DataEntryAttribute" Src="DataEntryAttribute.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Panel ID="pnlContainer" runat="server" ClientIDMode="Static">
</asp:Panel>
<asp:HiddenField ID="hdfCatId" runat="server" />
<asp:HiddenField ID="hdfItemId" runat="server" />
<asp:HiddenField ID="hdfSwappedItemId" runat="server" />

<asp:HiddenField ID="hdfCatDataId" runat="server" />
<asp:HiddenField ID="hdfItemDataId" runat="server" />
<asp:HiddenField ID="hdfCurrentSts" runat="server" />

<asp:HiddenField ID="hdfOldStatusCode" runat="server" />
<asp:HiddenField ID="hdfNewStatusCode" runat="server" />

<asp:HiddenField ID="hdfIsItemSeries" runat="server" />
<asp:HiddenField ID="hdfIsReadOnly" runat="server" />
<asp:HiddenField ID="hdfItemSeriesID" runat="server" />

<script type="text/javascript">

    function pageLoad() {
        $jQuery('input:checkbox').each(function () {
            if (this.checked) {
                this.click();
                this.click();
            }
        });
    }

    function ManageControlsEditablity(crntChkBox, id) {        
        //Production issue changes[26/12/2016]
        EnableDisableSaveButton();
        var checkBox = $jQuery('[id$=' + id + ']');
        $jQuery('tr[cattr=' + crntChkBox + ']').children().each(function (ctrl) {

            $jQuery(this).find('[id$=pnlAttrContainer]').find('[id$=pnlAttributes]').children().each(function (ctrl) {

                var control = $jQuery('[id$=' + $jQuery(this)[0].id + ']');

                var id = control[0].id.replace('_wrapper', '');

                var telrikControl = $find(id);
                var attr = $jQuery(this).attr('cattr');
                var controlType = $jQuery(this).attr('ctrlType');
                var ComplianceAttrType = $jQuery(this).attr('ComplianceAttrType');
                if (telrikControl != null && telrikControl != undefined && (attr == undefined || attr == null)) {
                    attr = telrikControl._element.getAttribute('cattr');
                }
                if (telrikControl != null && telrikControl != undefined && (controlType == undefined || controlType == null)) {
                    controlType = telrikControl._element.getAttribute('ctrlType');
                }
                if (telrikControl != null && telrikControl != undefined && (ComplianceAttrType == undefined || ComplianceAttrType == null || ComplianceAttrType == '')) {
                    ComplianceAttrType = telrikControl._element.getAttribute('ComplianceAttrType');
                }

                if (attr != null && attr != undefined && ComplianceAttrType != 'CATCALCU') {
                    if (checkBox[0].checked) {
                        telrikControl.set_enabled(true);
                        if (controlType.toUpperCase() == 'ADTTEX' || controlType.toUpperCase() == 'ADTNUM') {
                            telrikControl.enable();
                        }
                    }
                    else {
                        telrikControl.set_enabled(false);
                        if (controlType.toUpperCase() == 'ADTTEX' || controlType.toUpperCase() == 'ADTNUM') {
                            telrikControl.disable();
                        }
                    }
                }

            });
        });
    }

    function ManageControlOnSwap(cattrID, UpdateCheckBoxID, SwapCheckBoxID) {
        var checkBoxSwap = $jQuery('[id$=' + SwapCheckBoxID + ']');
        var checkBoxUpdate = $jQuery('[id$=' + UpdateCheckBoxID + ']');
        if (checkBoxSwap[0].checked) {
            checkBoxUpdate[0].checked = true;
            ManageControlsEditablity(cattrID, UpdateCheckBoxID);
        }
        else {
            checkBoxUpdate[0].checked = false;
            ManageControlsEditablity(cattrID, UpdateCheckBoxID);
        }
    }
    //Production issue changes[26/12/2016]
    function EnableDisableSaveButton() {
        var isUpdateCheckBoxChecked = false;

        $jQuery('input:checkbox').each(function () {

            if (this.checked && IsUpdateCheckBox($jQuery(this)[0].id)) {
                isUpdateCheckBoxChecked = true;
                return false;
            }
        });
        var saveAndReturnToQueue = $find($jQuery("[id$=btnSaveTemp]")[0].id);
        var saveAndDone = $find($jQuery("[id$=btnSaveDone]")[0].id);

        if (isUpdateCheckBoxChecked) {
            saveAndReturnToQueue.set_enabled(true);
            saveAndDone.set_enabled(true);
        }
        else {
            saveAndReturnToQueue.set_enabled(false);
            saveAndDone.set_enabled(false);
        }
    }

    function IsUpdateCheckBox(checkBoxId) {
        var splitedId = checkBoxId.split('_');
        if (splitedId != null && splitedId[0].toLowerCase() == "chkdocassociation") {
            return true;
        }
        return false;
    }
</script>
