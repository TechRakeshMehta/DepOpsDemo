<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedUserCustomAttributeForm.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.SharedUserCustomAttributeForm" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeRowControl.ascx" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeControl.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxSharedUserCustomAttributeForm">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div runat="server" id="divSection">
    
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">
                    <asp:Literal ID="litTitle" runat="server"></asp:Literal>
                </h2>
            </div>
        </div>
        <div id="divForm" runat="server" class="row bgLightGreen">
            <asp:Panel ID="pnlRows" runat="server">
            </asp:Panel>
        </div>
    
</div>
