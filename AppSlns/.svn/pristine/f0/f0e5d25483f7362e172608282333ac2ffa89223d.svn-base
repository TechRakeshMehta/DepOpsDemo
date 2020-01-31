<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Search.Views.ApplicantSubscriptionSearch" CodeBehind="ApplicantSubscriptionSearch.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblApplicantSubscriptionSearch" runat="server" Text="Applicant Subscription Search"></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel ID="pnlSearch" CssClass="sxpnl" runat="server">
                <div class='sxro sx3co'>
                    <div id="divTenant" runat="server">
                        <div class='sxlb' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName"
                                CausesValidation="false" AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='sxlb' title="Select a Package to restrict items in the grid to the selected Package">
                        <span class='cptn'>Package</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclDropDownList ID="ddlPackage" runat="server" DataTextField="PackageName"
                            DataValueField="CompliancePackageID" OnDataBound="ddlPackage_DataBound">
                        </infs:WclDropDownList>
                    </div>
                </div>
                <div class='sxroend'>
                </div>
                <div class='sxro sx3co'>
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
        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset"
            SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
            OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click">
        </infsu:CommandBar>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdApplicantSearchData" AllowCustomPaging="True"
                AutoGenerateColumns="False" AllowSorting="True" AllowFilteringByColumn="True"
                AutoSkinMode="True" CellSpacing="0" GridLines="Both" ShowAllExportButtons="False"
                ShowClearFiltersButton="false" OnNeedDataSource="grdApplicantSearchData_NeedDataSource"
                OnItemCommand="grdApplicantSearchData_ItemCommand" OnSortCommand="grdApplicantSearchData_SortCommand"
                OnInit="grdApplicantSearchData_Init" EnableLinqExpressions="false" NonExportingColumns="">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId,PackageSubscriptionId" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ApplicantName" FilterControlAltText="Filter ApplicantName column"
                            HeaderText="Applicant Name" SortExpression="ApplicantName" UniqueName="ApplicantName"
                            HeaderTooltip="This column displays the applicant's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="DateOfBirth" FilterControlAltText="Filter DateOfBirth column"
                            HeaderText="Date of Birth" SortExpression="DateOfBirth" UniqueName="DateOfBirth"
                            HeaderTooltip="This column displays the applicant's date of birth for each record in the grid"
                            DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                            HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName"
                            HeaderTooltip="This column displays the name of the package for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="ExpirationDate" FilterControlAltText="Filter ExpirationDate column"
                            HeaderText="Subscription Expiration Date" SortExpression="ExpirationDate" UniqueName="ExpirationDate"
                            HeaderTooltip="This column displays the Subscription Expiration Date for each record in the grid"
                            DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                        </telerik:GridDateTimeColumn>
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
    
</script>
