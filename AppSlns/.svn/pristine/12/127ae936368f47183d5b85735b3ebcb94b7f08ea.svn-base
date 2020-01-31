<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicantComprehensiveSearch.ascx.cs" Inherits="CoreWeb.SearchUI.Views.ApplicantComprehensiveSearch" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblApplicantSearch" runat="server" Text="Manage Applicant Comprehensive Search"></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlSearch" CssClass="sxpnl" runat="server">
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Select the Institution whose data you want to view">
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbTenantName" runat="server" DataTextField="TenantName"
                            CausesValidation="false" DataValueField="TenantID" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            <Localization CheckAllString="All" />
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="cmbTenantName"
                                Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Restrict search results to the entered User ID">
                        <span class="cptn">User ID</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtUserID" MaxValue="2147483647"
                            runat="server" InvalidStyleDuration="100" MinValue="1">
                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                        </infs:WclNumericTextBox>
                    </div>
                    <div class='sxlb' title="Restrict search results to the entered first name">
                        <span class="cptn">Applicant First Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtFirstName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb' title="Restrict search results to the entered last name">
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
                    <div class='sxlb' title="Restrict search results to the entered email address">
                        <span class="cptn">Email Address</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtEmail" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div id="divSSN" runat="server">
                        <div class='sxlb' title="Restrict search results to the entered SSN or ID Number">
                            <span class="cptn">SSN/ID Number</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclMaskedTextBox ID="txtSSN" runat="server" MaxLength="10" Mask="aaa-aa-aaaa">
                            </infs:WclMaskedTextBox>
                        </div>
                    </div>
                    <div id="divDOB" runat="server">
                        <div class='sxlb' title="Restrict search results to the entered Date of Birth">
                            <span class="cptn">Date of Birth</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date"
                                DateInput-DateFormat="MM/dd/yyyy">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset"
        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel" ValidationGroup="grpFormSubmit"
        OnSubmitClick="CmdBarSearch_ResetClick" OnSaveClick="CmdBarSearch_SearchClick" OnCancelClick="CmdBarSearch_CancelClick">
    </infsu:CommandBar>
    <div class="swrap">
        <infs:WclGrid runat="server" ID="grdApplicantComprehensiveSearchData" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            ShowClearFiltersButton="false" OnNeedDataSource="grdApplicantSearchData_NeedDataSource"
            OnItemCommand="grdApplicantSearchData_ItemCommand" OnSortCommand="grdApplicantSearchData_SortCommand"
            OnItemDataBound="grdApplicantSearchData_ItemDataBound" EnableLinqExpressions="false"
            NonExportingColumns="PortFolioDetail,SSN">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId" ClientDataKeyNames="OrganizationUserId" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="OrganizationUserId" HeaderText="User ID" SortExpression="OrganizationUserId"
                        UniqueName="UserID" HeaderTooltip="This column displays the User ID for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="Applicant First Name"
                        SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Applicant Last Name"
                        SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstituteName" HeaderText="Institution"
                        SortExpression="InstituteName" UniqueName="InstituteName" HeaderTooltip="This column displays the Institution for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress"
                        UniqueName="EmailAddress" ItemStyle-Width="180px" HeaderTooltip="This column displays the applicant's email address for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="DateOfBirth" HeaderText="Date of Birth" SortExpression="DateOfBirth"
                        UniqueName="DateOfBirth" DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="75px"
                        ItemStyle-Width="88px" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        UniqueName="SSN" ItemStyle-Width="130px" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                     Display="false"   UniqueName="_SSN" ItemStyle-Width="130px" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TenantID" HeaderText="TenantID" Display="false"
                        ItemStyle-Width="250px" AllowSorting="false" UniqueName="TenantID">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="PortFolioDetail" HeaderStyle-Width="10%">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnPortFolioDetail" ButtonType="LinkButton" CommandName="PortFolioDetail"
                                runat="server" Text="View Portfolio Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
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

<script type="text/javascript">
    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        //debugger;
        var _id = "btnPortFolioDetail";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }
</script>
