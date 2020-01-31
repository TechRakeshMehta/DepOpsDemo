<%@ Page Language="C#" Title="Item Payment" AutoEventWireup="true" MasterPageFile="~/Shared/PopupMaster.master"
    CodeBehind="ItemPaymentPopup.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Pages.ItemPaymentPopup" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceOperations/UserControl/ItemPayment.ascx" TagPrefix="infsu" TagName="ItemPayment" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" style="height: 900px; width: 900px" ContentPlaceHolderID="PoupContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxClinicalRotationMappingPopup">
        <%-- <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />--%>
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="../Resources/Generic/popup.min.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">
       
    </script>
    <div runat="server" id="dvItemPaymentStage">
        <infsu:ItemPayment runat="server" ID="ItemPayment" />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
</asp:Content>
