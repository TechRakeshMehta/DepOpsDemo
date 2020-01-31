<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ChildPage.master" AutoEventWireup="true" CodeBehind="PreviewAgencyJobPost.aspx.cs" Inherits="CoreWeb.AgencyJobBoard.Pages.PreviewAgencyJobPost" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxAdminView">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource ResourceType="StyleSheet" Path="~/Resources/Themes/Default/colors.css" />
        <infs:LinkedResource Path="~/Resources/Mod/Applicant/editprofile.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <asp:Panel ID="pnlPreviewJobPost" runat="server"></asp:Panel>
    <div class="container-fluid">
        <div class="row" style="padding-top: 40px;">
            <div class="col-md-12">
                <infsu:CommandBar ID="cmdSaveAssignments" runat="server" DisplayButtons="Cancel" CancelButtonText="Close" CancelButtonIconClass="rbCancel"
                    ButtonPosition="Center" CauseValidationOnCancel="false" OnCancelClientClick="ClosePopup" ButtonSkin="Silk" UseAutoSkinMode="false" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ClosePopup() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            oWindow.Close();
        }

    </script>
</asp:Content>
