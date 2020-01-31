<%@ Page Title="RuleTemplateCopy" Language="C#" MasterPageFile="~/Shared/ChildPage.master" AutoEventWireup="true"
    CodeBehind="RuleTemplateCopy.aspx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.RuleTemplateCopy" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function ShowProgressBar() { 
            if ($jQuery('[id$=rfvRuleTemplateName]')[0].isvalid && $jQuery('[id$=rfvOrganization]')[0].isvalid)
                Page.showProgress('Processing...');
        }
        function ReturnToParent() {
            var oWnd = GetRadWindow();
            oWnd.Close();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    </script>
    <div class="section" id="divPackageName" runat="server">
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="pnlRuleTemplateName" CssClass="sxpnl" runat="server">
                    <div class='sxro sx2co'>
                        <div class='sxlb'>
                            <span class="cptn">Rule Template Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm m3spn'>
                            <infs:WclTextBox ID="txtRuleTemplateName" runat="server" MaxLength="100">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvRuleTemplateName" ControlToValidate="txtRuleTemplateName"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Rule Template Name is required."/>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div id="divInstitute" runat="server">
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Institution</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm m3spn'>
                                <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" DataValueField="TenantID"
                                  CausesValidation="true"  Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnDataBound="cmbOrganization_DataBound">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvOrganization" ControlToValidate="ddlTenantName" ValidationGroup="grpFormSubmit"
                                        InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divButton" runat="server">
                <infsu:CommandBar ID="fsucCmdBarPrice" runat="server" DefaultPanel="pnlRuleTemplateName" OnSaveClientClick="ShowProgressBar"
                    DisplayButtons="Save,Cancel" AutoPostbackButtons="Save" OnSaveClick="fsucCmdBarPrice_SaveClick" CancelButtonText="Close" OnCancelClientClick="ReturnToParent"
                    ValidationGroup="grpFormSubmit">
                </infsu:CommandBar>
            </div>
        </div>
    </div>  
</asp:Content>
