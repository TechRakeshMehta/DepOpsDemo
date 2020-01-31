<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgencyLocationDepartmentDetails.aspx.cs" MasterPageFile="~/Shared/NewChildPage.master" Inherits="CoreWeb.PlacementMatching.Views.AgencyLocationDepartmentDetails" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/PlacementMatching/UserControl/AgencyLocation.ascx" TagPrefix="uc" TagName="AgencyLocation" %>
<%@ Register Src="~/PlacementMatching/UserControl/LocationDepartment.ascx" TagPrefix="uc" TagName="LocationDepartment" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxAgencyLocationDepartmentDetail">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="../Resources/Generic/popup.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <div>
        <uc:AgencyLocation runat="server" id="ucAgencyLocation" />
        <uc:LocationDepartment runat="server" id="ucLocationDepartment" />
    </div>

</asp:Content>
