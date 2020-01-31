<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplementAutomationConfigurationQueue.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.SupplementAutomationConfigurationQueue" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="msgbox">
    <asp:Label ID="lblMessage" runat="server" CssClass="info">
    </asp:Label>
</div>
<asp:HiddenField runat="server" ID="hdnIsQueueConfigInEditMode" />
<div class="section">
    <div class="content">
        <div class="section">
            <h1 class="mhdr">
                Supplement Automation Configuration
            </h1>
            <div class="content">
                <div class="sxform auto">
                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlConfiguration">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class='cptn'>Institution</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="ddlTenantName" runat="server" EnableCheckAllItemsCheckBox="true"
                                    DataTextField="TenantName" DataValueField="TenantID" CheckBoxes="true"
                                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                                </infs:WclComboBox>
                            </div>
                            <div class='sxlb'>
                                <span class='cptn'>Description</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox ID="txtDescription" runat="server">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxlb'>
                                <span class='cptn'>Record Percentage</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ID="txtPercentage" runat="server" ShowSpinButtons="false" Type="Number" MinValue="0" MaxValue="100">
                                </infs:WclNumericTextBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPercentage" ControlToValidate="txtPercentage"
                                        Display="Dynamic" CssClass="errmsg" Text="Record Percentage is required." ValidationGroup="grpQueueConfiguration" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </asp:Panel>
                    <infsu:CommandBar ID="fscuSupplementAutomation" runat="server" AutoPostbackButtons="Save,Submit,Cancel" SubmitButtonText="Edit Configuration"
                        OnSaveClick="fscuSupplementAutomation_SaveClick" CancelButtonText="Cancel" CauseValidationOnCancel="false" SubmitButtonIconClass="rbEdit"
                        OnCancelClick="fscuSupplementAutomation_CancelClick" OnSubmitClick="fscuSupplementAutomation_SubmitClick"
                        DefaultPanel="pnlConfiguration" ValidationGroup="grpSubmit">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>
</div>

