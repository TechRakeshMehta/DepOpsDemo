<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchyMultipleSelection.ascx.cs"
    Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchyMultipleSelection" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/AgencyHierarchy/AgencyHierarchyMultipleSelection.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div id='<%=this.ClientID %>'>
    <style type="text/css">
        .buttonHidden {
            display: none;
        }
        
    </style>
    
    <div class="col-md-12">
        <div class="row">
            <span class="cptn">Agency Hierarchy</span><span id="spnAgencyHierarchy" class="reqd buttonHidden">*</span>
            <a href="javascript:void(0)" style="color: blue;" id="AgencyHierarchy" onclick="openAgencyHierarchyPopUp('<%=this.ClientID %>');">Select Agency Hierarchy</a>&nbsp;&nbsp<br />
            <asp:Label ID="lblAgencyHierarchy" runat="server"></asp:Label>
        </div>
    </div>
    <asp:HiddenField ID="hdnAgencyHierarchyJsonObj" runat="server" Value="" />
    <asp:HiddenField ID="hdnTenantId" runat="server" Value="0" />
    <asp:HiddenField ID="AgencyHierarchyNodeIds" runat="server" Value="" />
    <asp:HiddenField ID="hdnAgencyHierarchyNodeSelection" runat="server" Value="" />
    <asp:HiddenField ID="hdnNodeHierarchySelection" runat="server" Value="" />
    <asp:HiddenField ID="hdnCurrentOrgUserID" runat="server" Value="" />
    <asp:HiddenField ID="hdnSelectedAgecnyIds" runat="server" />
    <asp:HiddenField ID="hdnSelectedNodeIds" runat="server" />
    <asp:HiddenField ID="hdnSelectedRootNodeId" runat="server" />
    <asp:HiddenField ID="hdnIsDisabledMode" Value="False" runat="server" />
    <asp:HiddenField ID="hdnIsParentDisable" Value="False" runat="server" />
    <asp:HiddenField ID="hdnIsInstitutionHierarchyRequired" runat="server" Value="false" />
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
    <asp:HiddenField ID="hdnIsAllNodeDisabledMode" Value="False" runat="server" />
    <asp:HiddenField ID="hdnInstitutionNodeIds" runat="server" Value="" />
    <asp:HiddenField ID="hdnIsAgencyNodeCheckable" runat="server" Value="" />
    <asp:HiddenField ID="hdnIsRotationPkgCopyFromAgencyHierarchy" runat="server" Value="" />
    <asp:HiddenField ID="hdnIsChildTreeNodeChecked" runat="server" />
    <asp:HiddenField ID="hdnIsChildBackButtonDisabled" runat="server" Value="" />
    <asp:HiddenField ID="hdnAddDisabledStyle" runat="server" Value=""/>
    <asp:HiddenField ID="hdnIsRequestFromAddRotationScrn" runat="server" Value=""/>
    <asp:HiddenField ID="hdnIsRequestFromManageRotationByAgencyScrn" runat="server" Value=""/>
</div>
