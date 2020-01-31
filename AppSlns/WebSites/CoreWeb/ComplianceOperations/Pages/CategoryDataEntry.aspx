<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.CategoryDataEntry" Title="Category Data Entry"
    MasterPageFile="~/Shared/PopupMaster.master" Codebehind="CategoryDataEntry.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register TagPrefix="infsu" TagName="ItemDetails" Src="~/ComplianceOperations/UserControl/ItemDetails.ascx" %>--%>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <div style="background-color: beige; border: 1px solid; padding: 10px">
        <h1>
            Help</h1>
        <p id="helpText" runat="server">
             
        </p>
    </div>
    <br />
    <div style="padding: 10px; color: Red; font-size: 120%;">
        <asp:Label Text="Requirement not compliant. Please go the previous screen and items sufficient to make the requirement compliant."
            runat="server" Visible="false" ID="lblmsg" />
    </div>
    <asp:Panel ID="pnl" runat="server">
    </asp:Panel>
    <asp:HiddenField ID="hdf" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
    <infsu:CommandBar ID="fsucCmdBar" runat="server" DefaultPanel="pnlName1" DisplayButtons="Save,Cancel"
        ButtonPosition="Center">
        <ExtraCommandButtons>
            <infs:WclButton runat="server" ID="btnPrevious" Text="Document Manager" OnClientClicked="evf_UploadNew"
                AutoPostBack="false">
                <Icon PrimaryIconCssClass="rbUpload" />
            </infs:WclButton>
        </ExtraCommandButtons>
    </infsu:CommandBar>
    <script type="text/javascript">
        function evf_UploadNew() {
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (80 / 100);

            var url = $page.url.create("Pages/Uploader.aspx");
            $window.createPopup(url, { size: "750,"+popupHeight });
        }
    </script>
</asp:Content>
