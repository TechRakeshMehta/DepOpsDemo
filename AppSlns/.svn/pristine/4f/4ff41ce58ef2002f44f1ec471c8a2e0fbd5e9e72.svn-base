<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderProfileMappingPopup.aspx.cs" Inherits="CoreWeb.BkgOperations.Views.BkgOrderProfileMappingPopup"
    MasterPageFile="~/Shared/PopupMaster.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/BkgOperations/UserControl/BkgOrderProfileMapping.ascx" TagPrefix="uc" TagName="BkgOrderProfileMapping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxOrderProfileMapping">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <script type="text/javascript">

        function openCmbBoxOnTab(sender, e) {
            if (!sender.get_dropDownVisible()) sender.showDropDown();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function returnToParent() {
            //debugger;
            var hdnIsSavedSuccessfully = $jQuery("[id$=hdnIsSavedSuccessfully]")[0].value;
            var hdnIsUpdatesSuccessfully = $jQuery("[id$=hdnIsUpdatesSuccessfully]")[0].value;

            var oArg = {};

            oArg.IsSavedSuccessfully = hdnIsSavedSuccessfully;
            oArg.IsUpdatesSuccessfully = hdnIsUpdatesSuccessfully;
            //get a reference to the current RadWindow
            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }
    </script>
    <uc:BkgOrderProfileMapping runat="server" ID="ucBkgOrderProfileMapping" />

    
</asp:Content>

