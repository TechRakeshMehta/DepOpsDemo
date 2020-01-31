<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.CategoryInfo"
    Title="CategoryInfo" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="CategoryInfo.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemsListng" Src="~/ComplianceAdministration/UserControl/ItemsListing.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<%@ Register TagPrefix="uc2" TagName="CategoriesItemsNodes" Src="~/Shared/Controls/CategoriesItemsNodes.ascx" %>

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

        function RefrshTreeOnDissociate(data) {
            if (data != undefined && data != '') {
                var hdnStoredData = $jQuery('[id$=hdnStoredData]', $jQuery(parent.theForm))
                if (hdnStoredData != undefined) {
                    hdnStoredData.val(data);
                }
            }
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
        var minDate = new Date("01/01/1980");

        function ValdateFrmToDate(sender, args) {
            var endDate = $jQuery("[id$=dpEndTo]")[0].control.get_selectedDate();
            var startDate = $jQuery("[id$=dpStartFrm]")[0].control.get_selectedDate();
            if (endDate != null && startDate == null) {
                sender.innerText = 'Compliance Required "From Date" is required.'
                args.IsValid = false;
            }
            if (startDate != null && endDate == null) {
                sender.innerText = 'Compliance Required "To Date" is required.'
                args.IsValid = false;
            }
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
                            <infs:WclTextBox runat="server" ID="txtCategoryName" MaxLength="100">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvCategoryName" ControlToValidate="txtCategoryName"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Category Name is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Category Label</span><%--<span class="reqd">*</span>--%>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtCategoryLabel" MaxLength="100">
                            </infs:WclTextBox>
                            <%--<div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvCategoryLabel" ControlToValidate="txtCategoryLabel"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Category Label is required." />
                            </div>--%>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Screen Label</span><%--<span class="reqd">*</span>--%>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtScreenLabel" MaxLength="100">
                            </infs:WclTextBox>
                            <%--  <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvScreenLabel" ControlToValidate="txtScreenLabel"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Screen Label is required." />
                            </div>--%>
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
                        <div class='sxlb'>
                            <span class="cptn">Display Order</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtDisplayOrder" MaxValue="2147483647"
                                runat="server" MinValue="0" MaxLength="4" InvalidStyleDuration="100" NumberFormat-DecimalDigits="0">
                            </infs:WclNumericTextBox>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Send Item Document on Approval</span>
                        </div>
                        <div class='sxlm'>
                            <uc1:IsActiveToggle runat="server" ID="chkSendItemDocApp" IsActiveEnable="true" IsAutoPostBack="false" />
                        </div>
                        <%-- <div class='sxlb'>
                            <span class="cptn">Universal Category</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbUniversalCategory" runat="server" ToolTip="Select universal category to map"
                                DataTextField="UC_Name" DataValueField="UC_ID"
                                AutoPostBack="false">
                            </infs:WclComboBox>
                        </div>--%>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class="sxgrp" runat="server" id="divComplianceRequired" visible="false">
                            <div class='sxlb'>
                                <span class="cptn">Compliance Required</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rblComplianceRequired" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes " Value="True" Selected="True" />
                                    <asp:ListItem Text="No" Value="False" />
                                </asp:RadioButtonList>
                            </div>
                            <div class='sxlb' title="Enter a range of dates for compliance required.">
                                <span class='cptn'>Compliance Required Date Range</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclDatePicker ID="dpStartFrm" AutoPostBack="false" runat="server" DateInput-EmptyMessage="Select a date (From)">
                                    <DateInput DateFormat="MM/dd" DisplayDateFormat="MM/dd"></DateInput>
                                </infs:WclDatePicker>
                                <infs:WclDatePicker ID="dpEndTo" AutoPostBack="false" runat="server" DateInput-EmptyMessage="Select a date (To)">
                                    <DateInput DateFormat="MM/dd" DisplayDateFormat="MM/dd"></DateInput>
                                </infs:WclDatePicker>
                                <div class="vldx">
                                    <asp:CustomValidator ID="cstStartFrm" runat="server" ErrorMessage="End Date Is Required." ValidationGroup="grpFormSubmit"
                                        CssClass="errmsg" Display="Dynamic" ClientValidationFunction="ValdateFrmToDate" ClientIDMode="Static" SetFocusOnError="True">
                                    </asp:CustomValidator>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class="sxro sx3co">
                        <div runat="server" id="dvTriggerComplianceCheck" visible="false">
                            <div class='sxlb'>
                                <span class="cptn">Trigger Compliance Check For All Categories</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rblTriggerComplianceCheck" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes " Value="True" />
                                    <asp:ListItem Text="No" Value="False" Selected="True" />
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div id="dvMappingHierarchy" runat="server" visible="false" class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Category Nodes</span>
                            </div>
                            <uc2:CategoriesItemsNodes runat="server" ID="ucCategoriesItemsNodes" />
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Description</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclEditor ID="rdEditorDescription" ClientIDMode="Static" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                Height="150px">
                            </infs:WclEditor>
                            <div class='vldx'>
                                <asp:CustomValidator runat="server" ID="cstValEditorDescription" ControlToValidate="rdEditorDescription" ClientValidationFunction="ValidateLength"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 500 characters." />
                            </div>
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

                    <%-- <div class='sxro sx3co monly'>
                        <div class='sxlb'>
                            <span class="cptn">Explanatory Notes</span>
                        </div>

                      
                    </div>
                    <div class='sxro sx1co'>
                        <infs:WclEditor ID="rdEditorEcplanatoryNotes" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml"  Width="99.7%" EnableResize="false"
                            Height="100px">
                        </infs:WclEditor>
                    </div>--%>
                </asp:Panel>
            </div>
            <div class="sxpnl">
                <div style="float: right;">
                    <infsu:CommandBar ID="fsucCmdBarCat" runat="server" DefaultPanel="pnlCategory" SaveButtonText="Save"
                        SubmitButtonText="Edit" SubmitButtonIconClass="rbEdit" OnSaveClick="fsucCmdBarCat_SaveClick"
                        OnSubmitClick="fsucCmdBarCat_SubmitClick" OnCancelClick="fsucCmdBarCat_CancelClick"
                        AutoPostbackButtons="Submit,Save,Cancel">
                    </infsu:CommandBar>
                </div>
                <%--  <div style="float: right; padding-top: 10px;">
                    <telerik:RadButton ID="btnDissociateCategory" ToolTip="Click here to dissociate category" runat="server" Text="Dissociate" Visible="false" OnClick="btnDissociateCategory_Click">
                    </telerik:RadButton>
                </div>--%>
                <%-- UAT-1862-Move Disassociate button from right side to left side of screen.--%>
                <div id="dvDisassociate" runat="server" style="padding-top: 10px;">
                    <div class='sxro sx2co' style="clear: none">
                        <div class='sxlb' style="border-color: transparent">
                            <span class="cptn">Dissociate With</span>
                        </div>
                        <div class='sxlm' style="border-color: transparent">
                            <infs:WclComboBox ID="cmbAssociatedPackages" runat="server" ToolTip="Select Packages to Disassociate the category" EmptyMessage="--Select--"
                                DataTextField="PackageName" DataValueField="CompliancePackageID" CheckBoxes="true" Width="70%"
                                AutoPostBack="false">
                            </infs:WclComboBox>
                            <telerik:RadButton ID="btnDissociateCategory" ToolTip="Click here to dissociate category" runat="server" Text="Dissociate" Visible="false" OnClick="btnDissociateCategory_Click">
                            </telerik:RadButton>
                        </div>
                    </div>
                </div>
            </div>

            <%--<div style="clear: both"></div>--%>
            <%--<infsu:CommandBar ID="fsucCmdBarCat" runat="server" DefaultPanel="pnlCategory" SaveButtonText="Save"
                SubmitButtonText="Edit" SubmitButtonIconClass="rbEdit" OnSaveClick="fsucCmdBarCat_SaveClick"
                OnSubmitClick="fsucCmdBarCat_SubmitClick" OnCancelClick="fsucCmdBarCat_CancelClick"
                AutoPostbackButtons="Submit,Save,Cancel">
            </infsu:CommandBar>--%>
        </div>
    </div>
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblTitle" runat="server" Text="Items"></asp:Label>
        </h1>
        <infsu:ItemsListng ID="ItemsListing" runat="server"></infsu:ItemsListng>
    </div>
</asp:Content>
