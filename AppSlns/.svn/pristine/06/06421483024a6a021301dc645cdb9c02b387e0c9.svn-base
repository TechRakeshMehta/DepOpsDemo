<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomFormLoadForServiceItem.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.CustomFormLoadForServiceItem" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="ApplicantDetails" Src="~/BkgOperations/UserControl/ApplicantDetails.ascx" %>
<%@ Register TagPrefix="uc" TagName="ApplicantData" Src="~/BkgOperations/UserControl/ApplicantData.ascx" %>

<uc:ApplicantDetails ID="ucApplicantDetails" runat="server" />

<asp:Panel ID="pnlExistingData" runat="server">
    <uc:ApplicantData ID="ucApplicantData" runat="server" />
</asp:Panel>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="Panel1" CssClass="sxpnl" runat="server">
                <div class='sxro sx2co'>
                    <div class="sxlb">
                        <span class='cptn'>Select Services</span><span class="reqd">*</span>
                    </div>
                    <div class="sxlm">
                        <infs:WclComboBox ID="cmbServices" EmptyMessage="--Select--" runat="server" CheckBoxes="true" AutoPostBack="false" CausesValidation="true" ValidationGroup="grpServc"
                            OnClientDropDownClosed="ReloadPageOnServiceChange">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvServiceName" ControlToValidate="cmbServices"
                                Display="Dynamic" ValidationGroup="grpServc" CssClass="errmsg" Text="Please select Services." />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>


<div class="section">
    <%--<h1 class="mhdr">--%>
    <asp:Label ID="lblServiceItemHeading" runat="server"></asp:Label>
    <%--</h1>--%>
    <div class="content">
        <asp:Panel ID="pnlLoader" runat="server"></asp:Panel>
    </div>
    <div style="display: none">
        <asp:Button ID="btnReloadPage" runat="server" OnClick="btnReload_Click" />
    </div>
</div>
<infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Save,Cancel"
    AutoPostbackButtons="Save,Cancel" CauseValidationOnCancel="true"
    SaveButtonText="Cancel Supplement" SaveButtonIconClass="rbPrevious" CancelButtonIconClass="rbNext" CancelButtonText="Continue" OnCancelClientClick="ValidatePage"
    OnSaveClick="CmdBarRestart_Click" OnCancelClick="CmdBarSave_Click">
</infsu:CommandBar>

<script type="text/javascript">
    function ValidatePage() {
        //UAT-2063:Combine the screens to add new Alias and add new locations
        var checkedItems = $jQuery("[id$=cmbServices]")[0].control.get_checkedItems();
        if (checkedItems.length > 0) {

            var validation = $jQuery(".errmsg");
            if (validation.length > 0) {
                var isValid = false;
                var groupIds = $jQuery("[id$=hdnGroupId]");
                if (groupIds.length > 0) {
                    for (i = 0; i < groupIds.length; i++) {
                        isValid = Page_ClientValidate('submitForm' + groupIds[i].value);
                        if (!isValid)
                        { return isValid; }
                    }
                }
                return isValid;
            }
            return true;
        }
        else {
            return false;
        }
    }
    //UAT-2063:Combine the screens to add new Alias and add new locations
    function ReloadPageOnServiceChange(sender, args) {
        var reloadrequired = false;
        var previousSelectedservices = $jQuery("[id$=hdnPreviousSelectedServices]").val();
        var previousServicesArray = previousSelectedservices.split(',');
        var checkedItems = sender.get_checkedItems();
        if (checkedItems.length > 0) {
            if (checkedItems.length != previousServicesArray.length) {
                reloadrequired = true;
            }
            else {
                var isMatched = false;
                for (var j = 0; j < checkedItems.length; j++) {
                    var indexOfCheckedItem = previousSelectedservices.indexOf(checkedItems[j].get_value());
                    if (indexOfCheckedItem > -1) {
                        isMatched = true;
                        break;
                    }
                }
                if (!reloadrequired) {
                    reloadrequired = !isMatched;
                }
            }
        }
        if (reloadrequired) {
            $jQuery("[id$=btnReloadPage]").click();
        }
    }
</script>

<asp:HiddenField runat="server" ID="hdnServiceItemId" />
<asp:HiddenField runat="server" ID="hdnFormId" />
<asp:HiddenField runat="server" ID="hdnInstanceCount" />
<asp:HiddenField ID="hdnHiddenPanels" runat="server" />
<asp:HiddenField runat="server" ID="hdnGroupidandIntanceNumber" />
<asp:HiddenField runat="server" ID="hdnPreviousSelectedServices" />
<asp:HiddenField ID="hdnAutofocusGroupID" runat="server" Value="" />
