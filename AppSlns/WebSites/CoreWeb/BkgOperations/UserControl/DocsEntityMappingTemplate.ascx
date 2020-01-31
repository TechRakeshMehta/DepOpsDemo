<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocsEntityMappingTemplate.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.DocsEntityMappingTemplate" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="InstituteHierarchy" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/ComplianceInstitutionHierarchy.ascx" %>



<div class="content">
    <div class="sxform auto">
        <div class="msgbox">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
        </div>
        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlDRDocsMapping">
            <div class='sxro sx3co'>
                <div class='sxlb'>
                    <span class='cptn'>Country</span><span class="reqd">*</span>
                </div>
                <div class='sxlm'>
                    <infs:WclComboBox runat="server" ID="cmbMapCountry" OnClientSelectedIndexChanged="PopulateBindMappingStateDropdown" EmptyMessage="--Select--"
                      Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"  DataTextField="Name" DataValueField="ID" ValidationGroup="grpFormEdit">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvCountryName" ControlToValidate="cmbMapCountry"
                            InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Country is required." ValidationGroup="grpFormEdit" />
                    </div>
                </div>
                <div class='sxlb'>
                    <span class='cptn'>State</span>
                </div>
                <div class='sxlm'>
                    <infs:WclComboBox runat="server" ID="cmbMapState" DataTextField="Name" DataValueField="ID"
                        Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnDataBound="cmbMapState_DataBound">
                    </infs:WclComboBox>
                </div>
                <div class='sxlb'>
                    <span class='cptn'>Service Name</span>
                </div>
                <div class='sxlm'>
                    <infs:WclComboBox runat="server" ID="cmbMapService" DataTextField="Name" DataValueField="ID" 
                       Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnDataBound="cmbMapService_DataBound">
                    </infs:WclComboBox>
                </div>
                <div class='sxroend'>
                </div>
            </div>
            <div class='sxro sx3co'>
                <div class='sxlb'>
                    <span class='cptn'>Institution</span>
                </div>
                <div class='sxlm'>
                    <infs:WclComboBox runat="server" ID="cmbMapTenant" DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="cmbMapTenant_SelectedIndexChanged"
                      Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"  AutoPostBack="true" OnDataBound="cmbMapTenant_DataBound">
                    </infs:WclComboBox>
                </div>
                <div class='sxlb'>
                    <span class='cptn'>Regulatory Entity Type</span>
                </div>
                <div class='sxlm'>
                    <infs:WclComboBox runat="server" ID="cmbMapRegulatoryEntity" DataTextField="Name" DataValueField="ID" 
                       Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnDataBound="cmbMapRegulatoryEntity_DataBound">
                    </infs:WclComboBox>
                </div>
                <div class='sxlb'>
                    <span class='cptn'>D&A Document</span><span class="reqd">*</span>
                </div>
                <div class='sxlm'>
                    <infs:WclComboBox runat="server" ID="cmbMapDRDocuments" DataTextField="Name" DataValueField="ID"
                       Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" ValidationGroup="grpFormEdit" OnDataBound="cmbMapDRDocuments_DataBound">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvDRDocuments" ControlToValidate="cmbMapDRDocuments"
                            InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="D&A Document is required." ValidationGroup="grpFormEdit" />
                    </div>
                </div>
                <div class='sxroend'>
                </div>
            </div>
            <div class='sxro sx3co'>
                <div class='sxlb' title="Click the link and select a node to restrict search results to the selected node">
                    <span class='cptn'>Institution Hierarchy</span>
                </div>
                <div class='sxlm m2spn'>
                    <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                    <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                    <div class="vldx">
                        <span id="lblInstHierarchyError" class="errmsg"></span>
                    </div>
                </div>
                <div class='sxroend'>
                </div>
            </div>
        </asp:Panel>
    </div>
    <infsu:CommandBar ID="fsucDRDocsMapping" runat="server" GridMode="true" DefaultPanel="pnlDRDocsMapping" GridInsertText="Save" GridUpdateText="Save"
        ValidationGroup="grpFormEdit" ExtraButtonIconClass="icnreset" />
</div>
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />