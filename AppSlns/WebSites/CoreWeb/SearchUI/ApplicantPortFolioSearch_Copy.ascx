<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Search.Views.ApplicantPortFolioSearch_Copy" Codebehind="ApplicantPortFolioSearch_Copy.ascx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearch.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="uc" %>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblApplicantSearch" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlSearch" CssClass="sxpnl" runat="server">
                <div class='sxro sx3co'>
                    <div id="divTenant" runat="server">
                        <div class='sxlb'>
                            <span class="cptn">Institution</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" DataValueField="TenantID"
                                Enabled="false" EmptyMessage="--Select--" OnDataBound="ddlTenantName_DataBound" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged">
                            </infs:WclComboBox>
                        </div>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">User Group</span></div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID"
                            OnDataBound="ddlUserGroup_DataBound">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Search Mode</span></div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbSearchType" runat="server">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <%-- <uc:CustomAttributeLoaderSearch ID="ucCustomAttributeLoaderSearch" runat="server" />--%>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">User ID</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtUserID" MaxValue="2147483647"
                            runat="server" InvalidStyleDuration="100" MinValue="1">
                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                        </infs:WclNumericTextBox>
                    </div>
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
                        <span class="cptn">Email Address</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtEmail" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">SSN/ID Number</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclMaskedTextBox ID="txtSSN" runat="server" MaxLength="10" Mask="###-##-####">
                        </infs:WclMaskedTextBox>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Date of Birth</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date"
                            DateInput-DateFormat="MM/dd/yyyy">
                        </infs:WclDatePicker>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset"
            SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
            OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click">
        </infsu:CommandBar>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdApplicantSearchData" AllowCustomPaging="true"
                AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" ShowClearFiltersButton="false"
                OnNeedDataSource="grdApplicantSearchData_NeedDataSource" OnItemCommand="grdApplicantSearchData_ItemCommand"
                OnSortCommand="grdApplicantSearchData_SortCommand" EnableLinqExpressions="false" OnItemDataBound="grdApplicantSearchData_ItemDataBound">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId,TenantId" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="OrganizationUserId" HeaderText="User ID" SortExpression="OrganizationUserId"
                            UniqueName="UserID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="Applicant First Name"
                            SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Applicant Last Name"
                            SortExpression="ApplicantLastName" UniqueName="ApplicantLastName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="InstituteName" HeaderText="Institution" 
                            SortExpression="InstituteName" UniqueName="InstituteName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress"
                            UniqueName="EmailAddress" ItemStyle-Width="180px">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="DateOfBirth" HeaderText="Date of Birth" SortExpression="DateOfBirth"
                            UniqueName="DateOfBirth" DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="75px"
                            ItemStyle-Width="88px">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                            UniqueName="SSN" ItemStyle-Width="130px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="InstitutionHierarchy" HeaderText="Institution Hierarchy"
                            ItemStyle-Width="250px" AllowSorting="false" SortExpression="InstitutionHierarchy"
                            UniqueName="InstitutionHierarchy">
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="ViewDetail" Text="View Portfolio"
                            ItemStyle-Width="80px">
                        </telerik:GridButtonColumn>
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

