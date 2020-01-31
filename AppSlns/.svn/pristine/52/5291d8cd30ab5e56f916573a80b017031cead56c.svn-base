<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.CategoryList"
    Title="CategoryList" MasterPageFile="~/Shared/ChildPage.master" MaintainScrollPositionOnPostback="true" CodeBehind="CategoryList.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/CategoryListing.ascx" TagPrefix="uc" TagName="CategoryList" %>
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

        .bullet ul {
            margin-left: 10px;
            padding-left: 10px !important;
        }

        .bullet li {
            list-style-position: inside;
            list-style: disc;
        }

        .bullet ol {
            list-style-type: decimal;
            margin-left: 10px;
            padding-left: 10px;
        }

            .bullet ol li {
                list-style: decimal;
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

        //13-02-2014  Changes Done for -"Category listing screen save performance improvement and Add Splash screen on save click".
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
        <infs:WclButton runat="server" ID="btnAdd" Text="+ Add a Category" OnClick="btnAdd_Click"
            Height="30px" ButtonType="LinkButton">
        </infs:WclButton>
    </div>
    <div class="section" id="divAddForm" runat="server" visible="false">
        <h1 class="mhdr">Add Category</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCategory" DefaultButton="btnSave">
                    <div class="sxgrp" id="divSelect" runat="server" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Select Category</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbMaster" runat="server" ToolTip="Select from a master list OR create new"
                                    DataTextField="CategoryName" DataValueField="ComplianceCategoryID" OnSelectedIndexChanged="cmbMaster_SelectedIndexChanged"
                                    AutoPostBack="true" OnDataBound="cmbMaster_DataBound">
                                </infs:WclComboBox>
                            </div>
                            <%--  <div class='sxlm'>
                                <infs:WclButton runat="server" ID="btnCreate" Text="Create New">
                                    <Icon PrimaryIconCssClass="rbAdd" />
                                </infs:WclButton>
                            </div>--%>
                            <div runat="server" id="divComplianceRequired" visible="false">
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
                                    <infs:WclDatePicker ID="dpStartFrm" runat="server" DateInput-EmptyMessage="Select a date (From)">
                                        <DateInput DateFormat="MM/dd" DisplayDateFormat="MM/dd"></DateInput>
                                    </infs:WclDatePicker>
                                    <infs:WclDatePicker ID="dpEndTo" runat="server" DateInput-EmptyMessage="Select a date (To)">
                                        <DateInput DateFormat="MM/dd" DisplayDateFormat="MM/dd"></DateInput>
                                    </infs:WclDatePicker>
                                    <div class="vldx">
                                        <asp:CustomValidator ID="cstStartFrm" runat="server" ErrorMessage="End Date Is Required." ValidationGroup="grpFormSubmit"
                                            CssClass="errmsg" Display="Dynamic" ClientValidationFunction="ValdateFrmToDate" ClientIDMode="Static" SetFocusOnError="True">
                                        </asp:CustomValidator>
                                    </div>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <div class="sxgrp" runat="server" id="divCreate" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Category Name</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtCategoryName" MaxLength="100">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvCategoryName" ControlToValidate="txtCategoryName"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Category Name is required." />
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
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Category Label is required." />
                                </div>--%>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Screen Label</span><%--<span class="reqd">*</span>--%>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtScreenLabel" MaxLength="100">
                                </infs:WclTextBox>
                                <%--<div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvScreenLabel" ControlToValidate="txtScreenLabel"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Screen Label is required." />
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
                                <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtDisplayOrder"
                                    MaxValue="2147483647" runat="server" MinValue="0" InvalidStyleDuration="100"
                                    NumberFormat-DecimalDigits="0" MaxLength="4">
                                </infs:WclNumericTextBox>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Send Item Document on Approval</span>
                            </div>
                            <div class='sxlm'>
                                <uc1:IsActiveToggle runat="server" ID="chkSendItemDocApp" IsActiveEnable="true" IsAutoPostBack="false" />
                            </div>
                            <%--                            <div class='sxlb'>
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
                        <div class="sxro sx3co">
                            <div runat="server" id="dvTriggerComplianceCheck" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Trigger Compliance Check For All Categories</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:RadioButtonList ID="rblTriggerComplianceCheck" runat="server"
                                        RepeatLayout="Flow" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Yes " Value="True" />
                                        <asp:ListItem Text="No" Value="False" Selected="True" />
                                    </asp:RadioButtonList>
                                </div>
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
                                    Height="150px">
                                </infs:WclEditor>
                                <div class='vldx'>
                                    <asp:CustomValidator runat="server" ID="cstValEditorDescription" ControlToValidate="rdEditorDescription" ClientValidationFunction="ValidateLength"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please don't enter more than 500 characters." />
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
                        <%--  <div class='sxro sx3co monly'>
                            <div class='sxlb'>
                                <span class="cptn">Explanatory Notes</span>
                            </div>
                        </div>
                        <div class='sxro sx1co'>
                            <infs:WclEditor ID="rdEditorEcplanatoryNotes" runat="server" ToolsFile="~/ComplianceAdministration/Data/Tools.xml"  Width="99.3%" EnableResize="false"
                                Height="100px">
                            </infs:WclEditor>
                            <div class='sxroend'>
                            </div>
                        </div>--%>
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
            <%-- <infsu:CommandBar ID="fsucCmdBarPackage" runat="server" DefaultPanel="pnlPackage"
                DisplayButtons="Save,Cancel" OnSaveClick="btnSave_Click" OnCancelClick="btnCancel_click"
                AutoPostbackButtons="Save,Cancel" ValidationGroup="grpFormSubmit">
            </infsu:CommandBar>--%>
        </div>
    </div>
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblTitle" runat="server" Text="Categories"></asp:Label>
        </h1>
        <div class="content">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdCategory" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false"
                    GridLines="None" OnNeedDataSource="grdCategory_NeedDataSource" OnDeleteCommand="grdCategory_DeleteCommand">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="CPC_ID">
                        <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add new Category" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="ComplianceCategory.CategoryName" FilterControlAltText="Filter CategoryName column"
                                HeaderText="Category Name" SortExpression="ComplianceCategory.CategoryName" UniqueName="CategoryName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceCategory.CategoryLabel" FilterControlAltText="Filter CategoryLabel column"
                                HeaderText="Category Label" SortExpression="ComplianceCategory.CategoryLabel"
                                UniqueName="CategoryLabel">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceCategory.ScreenLabel" FilterControlAltText="Filter ScreenLabel column"
                                HeaderText="Screen Label" SortExpression="ComplianceCategory.ScreenLabel" UniqueName="ScreenLabel">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceCategory.Description" ItemStyle-CssClass="bullet" FilterControlAltText="Filter Description column"
                                HeaderText="Description" SortExpression="ComplianceCategory.Description" UniqueName="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="ComplianceCategory.IsActive" FilterControlAltText="Filter IsActive column" DataType="System.Boolean"
                                HeaderText="Is Active" SortExpression="ComplianceCategory.IsActive" UniqueName="IsActive">
                                <ItemTemplate>
                                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("ComplianceCategory.IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                    <asp:HiddenField ID="hdnfComplianceCategoryID" runat="server" Value='<%#Eval("ComplianceCategory.ComplianceCategoryID")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="CPC_DisplayOrder" FilterControlAltText="Filter DisplayOrder column" DataType="System.Int32"
                                HeaderText="Display Order" SortExpression="CPC_DisplayOrder" UniqueName="DisplayOrder">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ComplianceCategory.TenantName" FilterControlAltText="Filter TenantName column"
                                HeaderText="Tenant" SortExpression="ComplianceCategory.TenantName" UniqueName="TenantName">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                Text="Delete" UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <EditFormSettings EditFormType="Template">
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
            <div class="gclr">
            </div>
        </div>
    </div>
    <%--  <div>
    <uc:CategoryList runat="server" ID="grdCategorylist"/>
    </div>--%>
</asp:Content>
