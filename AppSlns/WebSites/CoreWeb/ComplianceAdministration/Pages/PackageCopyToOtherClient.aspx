<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.PackageCopyToOtherClient"
    Title="PackageCopyToOtherClient" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="PackageCopyToOtherClient.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function ReturnToParent() {
            //get a reference to the current RadWindow
            var hdnPackageName = $jQuery("[id$=hdnCopiedPackageName]")[0].value;
            var hdnPackageId = $jQuery("[id$=hdnCopiedPackageId]")[0].value;
            var oArg = {};
            oArg.PackageName = hdnPackageName;
            oArg.PackageId = hdnPackageId;

            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }

        //Show Progress Bar
        function ShowProgressBar() {
            if ($jQuery('[id$=rfvPackageName]')[0].isvalid && $jQuery('[id$=rfvOrganization]')[0].isvalid)
                Page.showProgress('Processing...');
        }
    </script>

    <div class="section" id="divPackageName" runat="server">
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="pnlPackageName" CssClass="sxpnl" runat="server">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Package Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclTextBox ID="txtPackageName" runat="server" MaxLength="100">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPackageName" ControlToValidate="txtPackageName"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Package Name is required." />
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div id="divInstitute" runat="server">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Institution</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclComboBox ID="cmbOrganization" runat="server" DataTextField="TenantName"
                                    CausesValidation="true" DataValueField="TenantID" Skin="Windows7" AutoSkinMode="false"
                                    Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvOrganization" ControlToValidate="cmbOrganization"
                                        InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divButton" runat="server">
                <infsu:CommandBar ID="fsucCmdBarPrice" runat="server" DefaultPanel="pnlPackageName" OnSaveClientClick="ShowProgressBar"
                    DisplayButtons="Save,Cancel" AutoPostbackButtons="Save" OnSaveClick="fsucCmdBarPrice_SaveClick" CancelButtonText="Close" OnCancelClientClick="ReturnToParent"
                    ValidationGroup="grpFormSubmit">
                </infsu:CommandBar>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnClosePopUp" runat="server" />
    <asp:HiddenField ID="hdnCopiedPackageName" runat="server" Value="" />
    <asp:HiddenField ID="hdnCopiedPackageId" runat="server" Value="" />

</asp:Content>
