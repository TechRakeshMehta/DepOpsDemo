<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateRequestPopup.aspx.cs" Inherits="CoreWeb.PlacementMatching.Views.CreateRequestPopup" MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register Src="~/PlacementMatching/UserControl/CreateDraftControl.ascx" TagPrefix="uc" TagName="CreateDraftControl" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxCreateDraft">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <div class="container-fluid" style="padding-top: 20px;">
        <div>
            <uc:CreateDraftControl runat="server" ID="ucCreateDraftControl" />
        </div>
    </div>
</asp:Content>
