<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataReconciliationQueue.ascx.cs"
    Inherits="CoreWeb.ComplianceOperations.Views.DataReconciliationQueue" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    .RadPicker {
        float: left !important;
        vertical-align: middle;
    }
</style>

<div class="section">
    <h1 class="mhdr">Data Reconciliation Queue</h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlShowFilters">
                <div class='sxro sx2co'>
                    <div class='sxlb' title="Select the Institution whose data you want to view">
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" EmptyMessage="--Select--"
                            CausesValidation="false" DataValueField="TenantID" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            <Localization CheckAllString="All" />
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>

        <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Save,Submit,Cancel"
            AutoPostbackButtons="Save,Submit,Cancel" SubmitButtonText="Reset" CancelButtonText="Cancel" OnSubmitClick="CmdBarSearch_SubmitClick" SubmitButtonIconClass="rbRefresh"
            SaveButtonText="Search" SaveButtonIconClass="rbSearch" ValidationGroup="grpFormSubmit" OnSaveClick="CmdBarSearch_SaveClick" OnCancelClick="CmdBarSearch_CancelClick">
        </infsu:CommandBar>


        <div id="dvReconciliationQueue" runat="server" style="margin-top: 10px;" class="swrap">
            <infs:WclGrid runat="server" ID="grdReconciliationQueue" AllowCustomPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False"
                PageSize="50" NonExportingColumns="EditCommandColumn,DeleteColumn"
                OnNeedDataSource="grdReconciliationQueue_NeedDataSource" OnItemCommand="grdReconciliationQueue_ItemCommand"
                OnSortCommand="grdReconciliationQueue_SortCommand"
                OnInit="grdReconciliationQueue_Init">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True" HideStructureColumns="false"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="FlatComplianceItemReconciliationDataID">
                    <CommandItemSettings ShowAddNewRecordButton="false"
                        ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ApplicantName" FilterControlAltText="Filter Applicant_Name column"
                            HeaderText="Applicant Name" SortExpression="ApplicantName" UniqueName="ApplicantName"
                            HeaderTooltip="This column displays the applicant's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemName" FilterControlAltText="Filter Item_Name column"
                            HeaderText="Item Name" SortExpression="ItemName" UniqueName="ItemName"
                            HeaderTooltip="This column displays the Item's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter Category_Name column" 
                            HeaderText="Category Name" SortExpression="CategoryName" UniqueName="CategoryName"
                            HeaderTooltip="This column displays the Category's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter Package_Name column"
                            HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName"
                            HeaderTooltip="This column displays the Package's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="SubmissionDate" FilterControlAltText="Filter Submission_Date column"
                            HeaderText="Submission Date" DataFormatString="{0:d}" SortExpression="SubmissionDate" UniqueName="SubmissionDate"
                            FilterControlWidth="120px" HeaderTooltip="This column displays the date the applicant submitted the Item for review">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="Reviewers" FilterControlAltText="Filter Reviewers column"
                            HeaderText="Reviewers" SortExpression="Reviewers" UniqueName="Reviewers"
                            HeaderTooltip="This column displays the Reviewer's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="InstitutionName" FilterControlAltText="Filter Institution column"
                            HeaderText="Institution Name" SortExpression="InstitutionName" UniqueName="InstitutionName"
                            HeaderTooltip="This column displays the Institution name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnViewDetail" ButtonType="LinkButton" CommandName="ViewDetail"
                                    ToolTip="Click to open the reconciliation details" runat="server" Font-Underline="true"
                                    BackColor="Transparent" BorderStyle="None" ForeColor="Black"
                                    Text="Detail">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="TriPanelNav">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnTriPanel" ButtonType="LinkButton" CommandName="TriPanelNav" Width="100px"
                                    ToolTip="Click to open the verification details" runat="server" Font-Underline="true"
                                    BackColor="Transparent" BorderStyle="None" ForeColor="Black"
                                    Text="Verification Detail">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
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
<script type="text/javascript">
    function grd_rwDbClick(s, e) {
        var _id = "btnViewDetail";
        var _btnViewDetail = e.get_gridDataItem().findControl(_id);
        if (_btnViewDetail && typeof (_btnViewDetail.click) != "undefined") { _btnViewDetail.click(); }
    }
</script>
