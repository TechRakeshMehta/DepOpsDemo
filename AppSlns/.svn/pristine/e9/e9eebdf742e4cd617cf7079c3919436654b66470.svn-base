<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PackageCopyToLowerNode.aspx.cs"
    MasterPageFile="~/Shared/ChildPage.master"
    Inherits="CoreWeb.ComplianceAdministration.Views.PackageCopyToLowerNode" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
  
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
                                <span class="cptn">Nodes</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclComboBox ID="cmbNodes" runat="server" DataTextField="DPM_Label" EmptyMessage="-- Select --"
                                    CausesValidation="true" DataValueField="DPM_ID" Skin="Windows7" AutoSkinMode="false"
                                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvOrganization" ControlToValidate="cmbNodes"
                                        InitialValue="" Display="Dynamic" CssClass="errmsg" Text="Node is required." ValidationGroup="grpFormSubmit" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divButton" runat="server">
                <infsu:CommandBar ID="cmdBarCopy" runat="server" DefaultPanel="pnlPackageName"  
                    DisplayButtons="Save,Cancel" AutoPostbackButtons="Save" OnSaveClick="cmdBarCopy_SaveClick" CancelButtonText="Close" OnCancelClientClick="ReturnToParent"
                    ValidationGroup="grpFormSubmit">
                </infsu:CommandBar>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnClosePopUp" runat="server" />
    <asp:HiddenField ID="hdnCopiedPackageName" runat="server" Value="" />
    <asp:HiddenField ID="hdnCopiedPackageId" runat="server" Value="" />
</asp:Content>
