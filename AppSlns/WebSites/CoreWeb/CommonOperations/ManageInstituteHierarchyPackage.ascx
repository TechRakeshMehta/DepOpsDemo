<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageInstituteHierarchyPackage.ascx.cs" Inherits="CoreWeb.CommonOperations.Views.ManageInstituteHierarchyPackage" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/CommonOperations/ManageInstituteHierarchyPackage.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div id='<%=this.ClientID %>'>
    <style type="text/css">/**/
        .buttonHidden {
            display: none;
        }
    </style>
    <div class="col-md-12">
        <div class="row">
            <a href="#" style="color: blue;" id="InstituteHierarchyPackage" onclick="openInstituteHierarchyPackagePopUp('<%=this.ClientID %>');">
                <asp:Label ID="lblDisplayText" runat="server"></asp:Label></a>&nbsp;&nbsp<br />
            <asp:Label ID="lblInstituteHierarchyPackage" runat="server"></asp:Label>
        </div>
    </div>
    <asp:HiddenField ID="hdnTenantId" runat="server" Value="0" />
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
    <asp:HiddenField ID="hdnCompliancePackageTypeCode" runat="server" Value="" />
    <asp:HiddenField ID="hdnIsCompliancePackage" runat="server" Value="" />
    <asp:HiddenField ID="hdnPackageNodeMappingID" runat="server" />
    <asp:HiddenField ID="hdnPackageName" runat="server" />
    <asp:HiddenField ID="hdnDispalyText" runat="server" />
    <asp:HiddenField ID="hdnPackageId" runat="server" />
    <asp:HiddenField ID="hdnInstitutionHierarchyNodeID" runat="server" />
<%--    <asp:HiddenField ID="hdnDeptProgramMappingID" runat="server" />--%>
</div>
