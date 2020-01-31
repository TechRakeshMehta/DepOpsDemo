<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.ManageShotSeries"
    Title="ManageShotSeries" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="ManageShotSeries.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ShotSeriesListing" Src="~/ComplianceAdministration/UserControl/ShotSeriesListing.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CategoryPackageListing" Src="~/ComplianceAdministration/UserControl/CategoryPackageListing.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
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

        function SaveClick(sender, args) {
            //Commented code for firing Compliance Required validations.
            //var selecteditem = $find($jQuery("[id$=cmbMaster]")[0].id).get_selectedItem().get_value();
            //if (selecteditem == "" || selecteditem == "0") {
            if (Page_Validators != undefined && Page_Validators != null) {
                var i;
                for (i = 0; i < Page_Validators.length; i++) {
                    var val = Page_Validators[i];
                    if (!val.isvalid) {
                        return
                    }
                }
            }
            //}
            Page.showProgress("Processing...");
            args.set_cancel(false);
        }

        function ValidateSeriesDescritptionLength(sender, args) {
            var maxContentLength = 1000;
            var editor = $jQuery("[id$=rdSeriesEditorDescription]")[0];
            text = editor.control.get_text();
            text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
            var textLength = text.length;
            if (text != "" && textLength > maxContentLength)
                return args.IsValid = false;
            else
                return args.IsValid = true;
        }

        function ValidateSeriesDetailsLength(sender, args) {
            var maxContentLength = 1000;
            var editor = $jQuery("[id$=rdSeriesEditorDetails]")[0];
            text = editor.control.get_text();
            text = text.replace(/(?:\\[rn]|[\r\n]+)+/g, "");
            var textLength = text.length;
            if (text != "" && textLength > maxContentLength)
                return args.IsValid = false;
            else
                return args.IsValid = true;
        }

    </script>
    <div class="page_cmd">
        &nbsp;
    </div>
    <div class="section">

        <h1 class="mhdr">Category Information</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblSuccess" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCategory">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Category Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtCategoryName" MaxLength="100" Enabled="false">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Category Label</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtCategoryLabel" MaxLength="100" Enabled="false">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Screen Label</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtScreenLabel" MaxLength="100" Enabled="false">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Is Active</span>
                        </div>
                        <div class='sxlm'>
                            <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="false" IsAutoPostBack="false" />
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Description</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclEditor ID="rdEditorDescription" ClientIDMode="Static" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                Height="150px" EditModes="Preview">
                            </infs:WclEditor>
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
                                Height="150px" EditModes="Preview">
                            </infs:WclEditor>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

    <div class="page_cmd">
        <div class="sxcmds" style="text-align: right">
            <infs:WclButton runat="server" ID="btnAdd" Text="+ Add a Series" OnClick="btnAdd_Click"
                Height="30px" ButtonType="LinkButton">
            </infs:WclButton>
        </div>
    </div>

    <div class="section" id="divAddForm" runat="server" visible="false">
        <h1 class="mhdr">Add Series</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="Label1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="Panel1" DefaultButton="btnSave">
                    <div class="sxgrp" runat="server" id="divCreate" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Series Name</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtSeriesName" MaxLength="100">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvSeriesName" ControlToValidate="txtSeriesName"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Series Name is required." />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Series Label</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtSeriesLabel" MaxLength="100">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Is Active</span>
                            </div>
                            <div class='sxlm'>
                                <uc1:IsActiveToggle runat="server" ID="IsSeriesActiveToggle" IsActiveEnable="true" IsAutoPostBack="false" />
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Allow entry after all items are approved</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rbtnAlloEntry" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Selected="True" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Rule Execution Order</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtExecutionOrder" MinValue="1" 
                                    MaxValue="99" runat="server" InvalidStyleDuration="100" NumberFormat-DecimalDigits="0">
                                </infs:WclNumericTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Description</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclEditor ID="rdSeriesEditorDescription" ClientIDMode="Static" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                    Height="150px">
                                </infs:WclEditor>
                                <div class='vldx'>
                                    <asp:CustomValidator runat="server" ID="cstValSeriesEditorDescription" ControlToValidate="rdSeriesEditorDescription" ClientValidationFunction="ValidateSeriesDescritptionLength" ValidationGroup="grpFormSubmit"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 1000 characters." />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Details</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclEditor ID="rdSeriesEditorDetails" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                    Height="150px">
                                </infs:WclEditor>
                                <div class='vldx'>
                                    <asp:CustomValidator runat="server" ID="cstSeriesEditorDetails" ControlToValidate="rdSeriesEditorDetails" ClientValidationFunction="ValidateSeriesDetailsLength" ValidationGroup="grpFormSubmit"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 1000 characters." />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <%-- //13-02-2014  Changes Done for -"Category listing screen save performance improvement and Add Splash screen on save click". --%>
                    <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="grpFormSubmit"
                        OnClientClicking="SaveClick">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_click">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
        </div>
    </div>

    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblTitle" runat="server" Text="Series"></asp:Label>
        </h1>
        <infsu:ShotSeriesListing ID="ShotSeriesListing" runat="server"></infsu:ShotSeriesListing>
    </div>
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblTitlePackage" runat="server" Text="Packages"></asp:Label>
        </h1>
        <infsu:CategoryPackageListing ID="CategoryPackageListing" runat="server"></infsu:CategoryPackageListing>
    </div>
</asp:Content>

