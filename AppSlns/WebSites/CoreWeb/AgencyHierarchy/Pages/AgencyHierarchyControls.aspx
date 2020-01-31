<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/NewChildPage.master" AutoEventWireup="true" CodeBehind="AgencyHierarchyControls.aspx.cs"
    Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchyControls" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="AgencyNodeMapping" Src="~/AgencyHierarchy/AgencyNodeMapping.ascx" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/ManageAgencyHierarchyPackage.ascx" TagPrefix="infsu" TagName="ManageAgencyHierarchyPackage" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMappedNodes.ascx" TagPrefix="infsu" TagName="MappedNodes" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/AgencyHierarchyUserPermission.ascx" TagPrefix="infsu" TagName="AgencyHierarchyUserPermission" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/SchoolNodeAssociation.ascx" TagPrefix="infsu" TagName="SchoolNodeAssociation" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/AgencyHierarchyRotationFieldOption.ascx" TagPrefix="infsu" TagName="AgencyHierarchyRotationFieldOption" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/ManageAgencyHierarchyTenantAccess.ascx" TagPrefix="infsu" TagName="ManageAgencyHierarchyTenantAccess" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/ManageRequirementApprovalNotificationDocument.ascx" TagPrefix="infsu" TagName="ManageRequirementApprovalNotificationDocument" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/AgencyHierarchySetting.ascx" TagPrefix="infsu" TagName="AgencyHierarchySetting" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/ManageAttestationFormDocument.ascx" TagPrefix="infsu" TagName="AgencyAttestationFormSetting" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/AgencyHierarchyRootNodeSetting.ascx" TagPrefix="infsu" TagName="AgencyHierarchyRootNodeSetting" %>
<%@ Register Src="~/AgencyHierarchy/UserControls/AgencyHierarchyNodeAvailabilitySetting.ascx" TagPrefix="infsu" TagName="AgencyHierarchyNodeAvailabilitySetting" %>
<%--<%@ Register Src="~/AgencyHierarchy/UserControls/AgencyHierarchyProfileSharePermission.ascx" TagPrefix="infsu" TagName="AgencyHierarchyProfileSharePermission" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rmpHierarchyControls">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Scripts/bootstrap.min.js" ResourceType="JavaScript" />
         <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">
      
        function RefreshHierarchyTreeRoot() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            var hdnAddedRootNodeId = $jQuery('[id$=hdnAddedRootNodeId]', $jQuery(parent.theForm));
            var hdnIsNewRootNodeAdded = $jQuery('[id$=hdnIsNewRootNodeAdded]', $jQuery(parent.theForm));
            var hdnAddedRootNodeIdTemp = $jQuery('[id$=hdnAddedRootNodeIdTemp]');
            if (hdnAddedRootNodeIdTemp.val() == "0") {
                hdnIsNewRootNodeAdded.val('false');
            }
            else {
                hdnAddedRootNodeId.val(hdnAddedRootNodeIdTemp.val());
                hdnIsNewRootNodeAdded.val('true');
            }
            btn.click();
        }

        $(document).ready(function () {
            parent.parent.StartCountDown('<%= TimeoutMinutes %>');
        });

    </script>
    <div class="container-fluid">
        <infs:WclButton ID="WclButton1" Width="0px" Height="0px" runat="server"></infs:WclButton>

        <div style="float: right; padding-top: 10px; padding-bottom: 20px;">
            <infs:WclButton runat="server" ID="btnAddNode" Text="Add Node" OnClick="btnAddNode_Click" Skin="Silk" AutoSkinMode="false"
                ButtonType="StandardButton" Icon-PrimaryIconCssClass="rbAddNew">
            </infs:WclButton>
        </div>
        <div id="dvAddNode" runat="server" visible="false">
            <asp:Panel ID="pnlAddNode" runat="server">
                <div class="row bgLightGreen">
                    <div class='col-md-12'>
                        <div class="row">
                            <div class='form-group col-md-3' title="Select a node">
                                <span class="cptn">Select Node</span><span class="reqd">*</span>

                                <infs:WclComboBox ID="cmbAgencyNode" runat="server" DataTextField="NodeName" Filter="Contains" AutoPostBack="false"
                                    DataValueField="NodeId" Width="100%" CssClass="form-control"
                                    Skin="Silk" AutoSkinMode="false">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAgencyNode" ControlToValidate="cmbAgencyNode"
                                        InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAddNode"
                                        Text="Select Node is required." />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" style="text-align: right;">
                        <infsu:CommandBar ID="cmdBarSaveNodeMapping" runat="server" ValidationGroup="grpAddNode" UseAutoSkinMode="false" ButtonSkin="Silk" Visible="true"
                            DefaultPanel="pnlAddNode" DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel" CauseValidationOnCancel="false"
                            OnSaveClick="cmdBarSaveNodeMapping_SaveClick" OnCancelClick="cmdBarSaveNodeMapping_CancelClick" />
                    </div>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="hdnAddedRootNodeIdTemp" runat="server" Value="0" />
        </div>
        <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:MappedNodes ID="ucMappedNodes" runat="server" Visible="true" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:AgencyHierarchyNodeAvailabilitySetting ID="ucAgencyHierarchyNodeAvailabilitySetting" runat="server" Visible="true" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:AgencyNodeMapping ID="ctrAgencyNodeMapping" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:AgencyHierarchyUserPermission runat="server" ID="AgencyHierarchyUserPermission" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:ManageAgencyHierarchyPackage runat="server" ID="ManageAgencyHierarchyPackage" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:SchoolNodeAssociation runat="server" ID="ucSchoolNodeAssociation" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:ManageAgencyHierarchyTenantAccess ID="ucManageAgencyHierarchyTenantAccess" runat="server" Visible="true" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:AgencyHierarchyRotationFieldOption runat="server" ID="ucAgencyHierarchyRotationFieldOption" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:ManageRequirementApprovalNotificationDocument runat="server" ID="ucManageRequirementApprovalNotificationDocument" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:AgencyHierarchySetting runat="server" id="ucAgencyHierarchySetting" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="row">
                    <infsu:AgencyAttestationFormSetting runat="server" id="ucAttestationFormSetting" ></infsu:AgencyAttestationFormSetting>
                </div>
            </div>
        </div>
        <%-- <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:AgencyHierarchyProfileSharePermission runat="server" ID="ucAgencyHierarchyProfileSharePermission" />
                </div>
            </div>
        </div>--%>
         <div class="row">
            <div class='col-md-12'>
                <div class="row">
                    <infsu:AgencyHierarchyRootNodeSetting runat="server" id="ucAgencyHierarchyRootNodeSetting" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
