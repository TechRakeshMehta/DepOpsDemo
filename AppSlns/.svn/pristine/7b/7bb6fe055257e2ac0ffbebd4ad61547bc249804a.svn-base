<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.ItemList"
    Title="ItemList" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="ItemList.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemsListng" Src="~/ComplianceAdministration/UserControl/ItemsListing.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <style type="text/css">
        .reEditorModes a {
            display: none;
        }

        .reToolZone {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
        //13-02-2014  Changes done for - "Item listing screen show splash screen on save".
        function SaveClick(sender, args) {

            var i;
            var selecteditem = $find($jQuery("[id$=cmbMaster]")[0].id).get_selectedItem().get_value();
            if (selecteditem == "" || selecteditem == "0") {
                if (Page_Validators != undefined && Page_Validators != null) {
                    for (i = 0; i < Page_Validators.length; i++) {
                        var val = Page_Validators[i];
                        if (!val.isvalid) {
                            return
                        }
                    }
                }
            }
            Page.showProgress("Processing...");
            args.set_cancel(false);
        }

        function HideShowPanel(args, isPageReady) {
            var divAmount = $jQuery('[id$=divAmount]');
            var txtAmount = $jQuery('[id$=txtAmount]');
            var rfvAmount = $jQuery("[id$=rfvAmount]");
            var isChecked;

            if (isPageReady != undefined && isPageReady == "true") {
                var chkPaymentType = $jQuery('[id$=chkPaymentType]');
                if (chkPaymentType.length > 0) {
                    isChecked = chkPaymentType[0].checked;
                }

            }
            else {
                isChecked = args.checked;
            }

            if (isChecked) {
                divAmount[0].style.display = "block";
                ValidatorEnable(rfvAmount[0], true);
                rfvAmount.hide();
            }
            else {
                txtAmount.val("");
                divAmount[0].style.display = "none";
                ValidatorEnable(rfvAmount[0], false);
            }
        }
    </script>
    <asp:XmlDataSource ID="xdtsItems" runat="server" DataFile="~/App_Data/DB.xml" XPath="//MasterCompliance/MasterItems/*"></asp:XmlDataSource>
    <div class="page_cmd">
        <infs:WclButton runat="server" ID="btnAdd" Text="+ Add an Item" OnClick="btnAdd_Click"
            Height="30px" ButtonType="LinkButton">
        </infs:WclButton>
    </div>
    <div class="section" id="divAddForm" runat="server" visible="false">
        <h1 class="mhdr">Add Item</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlItem" DefaultButton="btnSave">
                    <div class="sxgrp" id="divSelect" runat="server" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Select Item</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbMaster" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbMaster_SelectedIndexChanged"
                                    ToolTip="Select from a master list OR create new">
                                </infs:WclComboBox>
                            </div>
                            <div class='sxlm'>
                                <infs:WclButton runat="server" ID="btnCreate" Text="Create New" Visible="false">
                                    <Icon PrimaryIconCssClass="rbAdd" />
                                </infs:WclButton>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class="sxgrp" runat="server" id="divCreate" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Item Name</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtName" MaxLength="100">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvItemName" ControlToValidate="txtName"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Item Name is required." />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Item Label</span><%--<span class="reqd">*</span>--%>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtLabel" MaxLength="100">
                                </infs:WclTextBox>
                                <%--     <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvLabel" ControlToValidate="txtLabel"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Item Label is required." /></div>--%>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Screen Label</span><%--<span class="reqd">*</span>--%>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtScreenLabel" MaxLength="100">
                                </infs:WclTextBox>
                                <%-- <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvScreenLabel" ControlToValidate="txtScreenLabel"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Screen Label is required." /></div>--%>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Is Active</span>
                            </div>
                            <div class='sxlm'>
                                <%--<infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                    AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>--%>
                                <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />
                            </div>
                            <div id="divEffectiveDate" runat="server">
                                <div class='sxlb'>
                                    <span class="cptn">Effective Date</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclDatePicker ID="dpkrEffectiveDate" runat="server" DateInput-EmptyMessage="Select a date">
                                        <%--<Calendar>
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today" ItemStyle-CssClass="rcToday" />
                                        </SpecialDays>
                                    </Calendar>--%>
                                    </infs:WclDatePicker>
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Display Order</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtDisplayOrder"
                                    MaxValue="2147483647" runat="server" MinValue="0" InvalidStyleDuration="100"
                                    NumberFormat-DecimalDigits="0" MaxLength="4">
                                </infs:WclNumericTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Is a Payment Item</span>
                            </div>
                            <div class='sxlm'>
                                <asp:CheckBox ID="chkPaymentType" runat="server" Text="" onclick="HideShowPanel(this);" />
                            </div>
                            <div id="divAmount" runat="server" style="display: none;">
                                <div class='sxlb'>
                                    <span class="cptn">Amount</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclNumericTextBox ID="txtAmount" Type="Currency" runat="server" NumberFormat-DecimalDigits="2" MinValue="0" MaxLength="9">
                                    </infs:WclNumericTextBox>
                                    <div class='vldx'>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvAmount" ControlToValidate="txtAmount" ValidationGroup="grpFormSubmit"
                                            class="errmsg" Display="Dynamic" ErrorMessage="Amount is required." />
                                    </div>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                       <%-- <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Universal Item</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbUniversalItem" runat="server" ToolTip="Select universal category to map"
                                    DataTextField="UI_Name" DataValueField="UI_ID"
                                    AutoPostBack="false">
                                </infs:WclComboBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>--%>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Description</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclEditor ID="rdEditorDescription" ClientIDMode="Static" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                    Height="150px">
                                </infs:WclEditor>
                            </div>
                            <div class='vldx'>
                                <asp:CustomValidator runat="server" ID="cstValEditorDescription" ControlToValidate="rdEditorDescription" ClientValidationFunction="ValidateLength"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 500 characters." />
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Details</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclEditor ID="rdEditorDetails" ClientIDMode="Static" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                    Height="150px">
                                </infs:WclEditor>
                            </div>
                            <div class='vldx'>
                                <asp:CustomValidator runat="server" ID="cstValEditorDetails" ControlToValidate="rdEditorDetails" ClientValidationFunction="ValidateDetailsLength"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 500 characters." />
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Explanatory Notes</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclEditor ID="rdEditorEcplanatoryNotes" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                    Height="150px">
                                </infs:WclEditor>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <%-- <div class='sxro sx1co'>--%>
                        <%-- <infs:WclEditor ID="rdEditorEcplanatoryNotes" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml"  Width="99.3%" EnableResize="false"
                                Height="100px">
                            </infs:WclEditor>
                            <div class='sxroend'>
                            </div>--%>
                        <%-- </div>--%>
                    </div>
                </asp:Panel>
            </div>
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <%--//13-02-2014  Changes done for - "Item listing screen show splash screen on save".--%>
                    <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="btnSubmit_Click"
                        OnClientClicking="SaveClick" ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="fsucCmdBarItem_CancelClick">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
            <%--<infsu:CommandBar ID="fsucCmdBarItem" runat="server" DisplayButtons="Save,Cancel" DefaultPanel="www"
                OnSaveClick="btnSubmit_Click" AutoPostbackButtons="Save,Cancel" OnCancelClick="fsucCmdBarItem_CancelClick"
                ValidationGroup="grpFormSubmit">
            </infsu:CommandBar>--%>
        </div>
    </div>
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblTitle" runat="server" Text="Items"></asp:Label>
        </h1>
        <div class="content">
            <div class="swrap">
                <infsu:ItemsListng ID="ucItemsListing" runat="server"></infsu:ItemsListng>
            </div>
            <div class="gclr">
            </div>
        </div>
    </div>
</asp:Content>
