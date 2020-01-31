<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageAgencyHierarchyTenantAccess.ascx.cs"
    Inherits="CoreWeb.AgencyHierarchy.Views.ManageAgencyHierarchyTenantAccess" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxManageAgencyHierarchyPackages">
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script type="text/javascript">
    function RefreshHierarchyTree() {
        var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
        btn.click();
    }
</script>
<script src="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblRotation" Text="Agency Applicant Status" runat="server" />
            </h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="msgbox">
                <asp:Label ID="lblMsg" runat="server" CssClass="info"></asp:Label>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3">
            <span class='cptn'>Institution</span>
            <infs:WclComboBox ID="cmbTenant" runat="server" DataValueField="TenantID" DataTextField="TenantName" AutoPostBack="false" EmptyMessage="--SELECT--"
                EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
            </infs:WclComboBox>
            <%--<div class="vldx">
                <asp:RequiredFieldValidator runat="server" ID="tfvTenant" ControlToValidate="cmbTenant"
                    Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit1"
                    Text="Tenant is required." />
            </div>--%>
        </div>
    </div>
    <div class="col-md-12">
        <infsu:CommandBar ID="fsucCmdBar" runat="server" ButtonPosition="Right" DisplayButtons="Submit,Cancel" UseAutoSkinMode="false" ButtonSkin="Silk"
            AutoPostbackButtons="Submit,Cancel" CancelButtonText="Cancel" CancelButtonIconClass="rbCancel" OnCancelClick="fsucCmdBar_CancelClick"
            OnSubmitClick="fsucCmdBar_SubmitClick" SubmitButtonIconClass="rbSave" SubmitButtonText="Save" ValidationGroup="grpFormSubmit1">
        </infsu:CommandBar>
    </div>
</div>
