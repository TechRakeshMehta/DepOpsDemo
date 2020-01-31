<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceFormMappingTemplate.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ServiceFormMappingTemplate" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="content">
    <div class="sxform auto">
        <div class="msgbox">
            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
        </div>
        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlDRDocsMapping">
            <div class='sxro sx3co'>
                <div class='sxlb'>
                    <span class='cptn'>Service Attached Form</span><span class="reqd">*</span>
                </div>
                <div class='sxlm'>
                    <!---->
                    <infs:WclComboBox runat="server" ID="cmbServiceForm" OnDataBound="cmbServiceForm_DataBound" CausesValidation="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                        DataTextField="SF_Name" DataValueField="SF_ID" AutoPostBack="true" OnSelectedIndexChanged="cmbServiceForm_SelectedIndexChanged">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvServiceForm" ControlToValidate="cmbServiceForm"
                            InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Service Attached Form is required." ValidationGroup="grpFormEdit" />
                    </div>
                </div>
                <div class='sxlb'>
                    <span class='cptn'>Service</span><span class="reqd">*</span>
                </div>
                <div class='sxlm'>
                    <infs:WclComboBox runat="server" ID="cmbService" OnDataBound="cmbService_DataBound" DataTextField="BSE_Name" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" DataValueField="BSE_ID">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvService" ControlToValidate="cmbService"
                            InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Service is required." ValidationGroup="grpFormEdit" />
                    </div>
                </div>
                <div class='sxroend'>
                </div>
            </div>
            <div class='sxro sx3co' id="divHierarchy" runat="server" visible="false">
                <div class='sxlb' title="Click the link and select a node to restrict search results to the selected node">
                    <span class='cptn'>Institution Hierarchy</span><span class="reqd">*</span>
                </div>
                <div class='sxlm m2spn'>
                    <a href="#" id="instituteHierarchyChild" onclick="openPopUp(true);">Select Institution Hierarchy</a>&nbsp;&nbsp
                    <asp:Label ID="lblChildinstituteHierarchy" runat="server"></asp:Label>
                    <div class="vldx">
                        <asp:Label ID="lblHierarchyVld" runat="server" CssClass="errmsg"></asp:Label>
                    </div>
                </div>
                <div class='sxroend'>
                </div>
            </div>
            <div class='sxro sx3co' runat="server" id="divDispatchedMode">
                <div class='sxlb'>
                    <span class="cptn">Dispatch Type</span>
                </div>
                <div class='sxlm' runat="server" id="divDispatchedModeRadioButtons">
                    <asp:RadioButtonList ID="rblDispatchMode" runat="server" RepeatDirection="Horizontal"
                        CssClass="radio_list" AutoPostBack="false">
                        <asp:ListItem Text="Manual" Value="0" />
                        <asp:ListItem Text="Automatic" Value="1" />
                    </asp:RadioButtonList>
                </div>
                <div class='sxroend'>
                </div>

            </div>
        </asp:Panel>
    </div>
    <infsu:CommandBar ID="fsucDRDocsMapping" runat="server" GridMode="true" DefaultPanel="pnlDRDocsMapping" GridInsertText="Save" GridUpdateText="Save"
        ValidationGroup="grpFormEdit" ExtraButtonIconClass="icnreset" />
</div>
<asp:HiddenField ID="hdnChildTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnChildDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnChildHierarchyLabel" runat="server" Value="" />



