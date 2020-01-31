<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ADBAdminDataAuditHistory" CodeBehind="ADBAdminDataAuditHistory.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style type="text/css">
    .RadToolTip {
        border: 1px solid Gray !important;
        background-color: Gray !important;
    }

    .RadGrid .rgHoveredRow {
        background: #dadada !important;
        border: none none none none !important;
        border-color: #dadada !important;
    }
</style>
<div class="section">
    <h1 class="mhdr">Multiple Institutions Admin Data Audit History</h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlShowFilters">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbTenant" Width="100%" runat="server" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            AutoPostBack="false" DataTextField="TenantName" DataValueField="TenantID" EmptyMessage="--SELECT--" Skin="Silk"
                            EnableCheckAllItemsCheckBox="true" CheckBoxes="true" ValidationGroup="vgAuditHistory">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:CustomValidator ID="rfvTenants" runat="server" ErrorMessage="Institution is required." ValidateEmptyText="true"
                                ClientValidationFunction="ValidateCheckBoxSelection" ControlToValidate="cmbTenant"
                                CssClass="errmsg" Display="Dynamic" ValidationGroup="vgAuditHistory">
                            </asp:CustomValidator>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Package</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtPackageName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Category</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtCategoryName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Item</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtItemName" runat="server">
                        </infs:WclTextBox>
                    </div>

                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Applicant First Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtFirstName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Applicant Last Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtLastName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Admin First Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtAdminFirstName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Admin Last Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtAdminLastName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Updated From</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDatePicker ID="dpTmStampFromDate" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnDateSelected="CorrectFrmToCrtdDate">
                        </infs:WclDatePicker>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Updated To</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDatePicker ID="dpTmStampToDate" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinDate">
                        </infs:WclDatePicker>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                 <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Role</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="cmbRole" runat="server" DataTextField="Value" DataValueField="Key" Filter="Contains" ChangeTextOnKeyBoardNavigation="true" OnClientKeyPressing="openCmbBoxOnTab" 
                                EmptyMessage="--Select--" EnableCheckAllItemsCheckBox="true"  CheckBoxes="true" AllowCustomText="true">
                            </infs:WclComboBox>
                        </div>                        
                        <div class='sxroend'>
                        </div>
                    </div>
                <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                    AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset"
                    SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
                    OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" CauseValidationOnCancel="false"
                    OnCancelClick="CmdBarCancel_Click">
                </infsu:CommandBar>
            </asp:Panel>
        </div>
        <div>
            <infs:WclGrid runat="server" ID="grdApplicantDataAudit" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="False" AutoSkinMode="True" CellSpacing="0" NonExportingColumns="ChangeValue"
                GridLines="None" ShowAllExportButtons="false" OnNeedDataSource="grdApplicantDataAudit_NeedDataSource"
                OnSortCommand="grdApplicantDataAudit_SortCommand" OnPreRender="grdApplicantDataAudit_PreRender"
                OnItemCommand="grdApplicantDataAudit_ItemCommand" AllowCustomPaging="true" ShowClearFiltersButton="false">
                <ClientSettings EnableRowHoverStyle="false">
                    <Selecting AllowRowSelect="false"></Selecting>
                </ClientSettings>
                <GroupingSettings CaseSensitive="false" />
                <ExportSettings Pdf-PageWidth="350mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    OpenInNewWindow="true" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <MasterTableView AllowFilteringByColumn="false" CommandItemDisplay="Top" DataKeyNames="ApplicantDataAuditMultiTenantID,ApplicantDataAuditID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                        ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter TenantName column"
                            AllowFiltering="false" HeaderText="Tenant Name" SortExpression="TenantName"
                            UniqueName="TenantName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ApplicantName" FilterControlAltText="Filter ApplicantName column"
                            AllowFiltering="false" HeaderText="Applicant Name" SortExpression="ApplicantName"
                            UniqueName="ApplicantName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                            AllowFiltering="false" HeaderText="Package Name" SortExpression="PackageName"
                            UniqueName="PackageName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter CategoryName column"
                            HeaderText="Category Name" SortExpression="CategoryName" UniqueName="CategoryName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemName" FilterControlAltText="Filter ItemName column"
                            AllowFiltering="false" HeaderText="Item Name" SortExpression="ItemName" UniqueName="ItemName">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="TimeStampValue" FilterControlAltText="Filter TimeStampValue column"
                            AllowFiltering="false" HeaderText="Updated On" SortExpression="TimeStampValue"
                            UniqueName="TimeStampValue" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="AdminName" FilterControlAltText="Filter AdminName column"
                            AllowFiltering="false" HeaderText="Updated By " SortExpression="AdminName" UniqueName="AdminName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ChangeValue" FilterControlAltText="Filter Change column"
                            AllowFiltering="false" HeaderText="Change" UniqueName="ChangeTemp" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Change" UniqueName="ChangeValue" FilterControlAltText="Filter ChangeValue column"
                            AllowFiltering="false">
                            <ItemTemplate>
                                <asp:Label ID="lblChangeValue" runat="server" Text='<%# Convert.ToString(Eval("ChangeValue")).Length > 100 ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ChangeValue")).Substring(0, 100)) + "...." : INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString( Eval("ChangeValue")))%>'></asp:Label>
                                <infs:WclToolTip runat="server" ID="tltpChangeValue" TargetControlID="lblChangeValue"
                                    Width="300px" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString( Eval("ChangeValue"))) %>' ManualClose="false"
                                    RelativeTo="Element" Position="TopCenter" Visible='<%# Eval("ChangeValue").ToString().Trim()==String.Empty ? false : Convert.ToString(Eval("ChangeValue")).Length > 100?true:false %>'>
                                </infs:WclToolTip>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
            <div class="gclr">
            </div>
        </div>
    </div>
</div>
<div id="dialog-form" title="Change Value">
    <div id="divContent" style="text-align: left;">
    </div>
</div>
<script type="text/javascript">
    var minDate = new Date("01/01/1980");
    function CorrectFrmToCrtdDate(picker) {
        var date1 = $jQuery("[id$=dpTmStampFromDate]")[0].control.get_selectedDate();
        var date2 = $jQuery("[id$=dpTmStampToDate]")[0].control.get_selectedDate();
        if (date1 != null && date2 != null) {
            if (date1 > date2)
                $jQuery("[id$=dpTmStampToDate]")[0].control.set_selectedDate(null);
        }
    }

    function SetMinDate(picker) {
        var date = $jQuery("[id$=dpTmStampFromDate]")[0].control.get_selectedDate();
        if (date != null) {
            picker.set_minDate(date);
        }
        else {
            picker.set_minDate(minDate);
        }
    }

    function ValidateCheckBoxSelection(source, args) {
        var cntrlToValidate = $find($jQuery("[id$=cmbTenant]").attr("id"));
        var check = 0;
        if (cntrlToValidate) {
            var cntrlItems = cntrlToValidate.get_items();
            for (var i = 0; i <= cntrlItems.get_count() - 1; i++) {
                var cntrlItem = cntrlItems.getItem(i);
                if (cntrlItem.get_checked()) {
                    check = 1;
                }
            }
        }
        if (check)
            args.IsValid = true;
        else
            args.IsValid = false;
    }

</script>
