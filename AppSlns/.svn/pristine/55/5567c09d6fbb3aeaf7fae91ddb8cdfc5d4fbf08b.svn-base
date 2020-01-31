<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageDocumentUploadOrderSearch.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ManageDocumentUploadOrderSearch" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/ManageBulkArchive.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotation.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>



<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Document Upload Order Search
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                        <div id="divTenant" runat="server">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" AutoPostBack="true" Skin="Silk" AutoSkinMode="false"
                                DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged" Width="100%" CssClass="form-control"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Select one or more Order Types to restrict search results to those types">
                        <span class='cptn'>Order Type</span>
                        <infs:WclComboBox ID="cmbOrderType" runat="server" CheckBoxes="true" EmptyMessage="--Select--"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false"  Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Upload Document</span>
                        <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay" MaxFileInputsCount="1" CssClass="form-control" AutoSkinMode="true"
                            MultipleFileSelection="Disabled" OnClientFileSelected="onClientFileSelected" OnClientFileUploaded="onFileUploadedZeroSize" OnClientFileUploadRemoved="onFileRemoved" UploadedFilesRendering="BelowFileInput"
                            AllowedFileExtensions="xls,xlsx" ToolTip="Click here to select files to upload from your computer">
                            <Localization Select="Browse" />
                        </infs:WclAsyncUpload>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <div class="col-md-12">
            &nbsp;
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo" SubmitButtonText="Reset" DefaultPanel="pnlSearch" DefaultPanelButton="Save"
                SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel" ValidationGroup="grpFormSubmit"
                OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click" UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Order Details
            </h2>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdMatchedApplicantOrder" AutoGenerateColumns="False"
            AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="true"
            ShowAllExportButtons="false" ShowExtraButtons="False" AllowCustomPaging="false" ShowClearFiltersButton="false"
            GridLines="Both" OnNeedDataSource="grdMatchedApplicantOrder_NeedDataSource">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="OrderId" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridNumericColumn DataField="OrderNumber" FilterControlAltText="Filter OrderId column"
                        HeaderText="Order ID" SortExpression="OrderNumber" DataType="System.Int32" UniqueName="OrderId"
                        DecimalDigits="0" HeaderStyle-Width="8%" HeaderTooltip="This column displays the order number for each record in the grid">
                    </telerik:GridNumericColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column" HeaderStyle-Width="300" AllowFiltering="false"
                        HeaderText="First Name" SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" ReadOnly="true" HeaderTooltip="This column contains the Applicant First name">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column" HeaderStyle-Width="300" AllowFiltering="false"
                        HeaderText="Last Name" SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" ReadOnly="true" HeaderTooltip="This column contains the Applicant Last name">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="ApplicantDOB" FilterControlAltText="Filter ApplicantDOB column" HeaderStyle-Width="200"
                        AllowFiltering="false" HeaderText="DOB" SortExpression="ApplicantDOB" HeaderTooltip="This column contains the Applicant Date of Birth"
                        UniqueName="ApplicantDOB" DataFormatString="{0:MM/dd/yyyy}">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="InstitutionHierarchy" FilterControlAltText="Filter InstitutionHierarchy column" HeaderStyle-Width="450" AllowFiltering="false"
                        HeaderText="Institution Hierarchy" SortExpression="InstitutionHierarchy" UniqueName="InstitutionHierarchy" ReadOnly="true" HeaderTooltip="This column contains the Institution Hierarchy">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column" HeaderStyle-Width="300" AllowFiltering="false"
                        HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName" ReadOnly="true" HeaderTooltip="This column contains the Package Name">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrderStatusName" FilterControlAltText="Filter OrderStatusName column" AllowSorting="false"
                        HeaderText="Payment Status" UniqueName="OrderStatusName"
                        HeaderStyle-Width="15%" HeaderTooltip="This column displays the current Payment Status for each order">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>

    <div id="mainUnMatchedApplicant" runat="server" visible="false">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Un-Matched Applicants
                </h2>
            </div>
        </div>
        <div class="row">
            <infs:WclGrid runat="server" ID="grdUnMatchedApplicants" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" EnableDefaultFeatures="true"
                ShowAllExportButtons="false" ShowExtraButtons="False" AllowCustomPaging="false" ShowClearFiltersButton="false"
                GridLines="Both" OnNeedDataSource="grdUnMatchedApplicants_NeedDataSource">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column" HeaderStyle-Width="350" AllowFiltering="false"
                            HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName" ReadOnly="true" HeaderTooltip="This column contains the Applicant First name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column" HeaderStyle-Width="350" AllowFiltering="false"
                            HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName" ReadOnly="true" HeaderTooltip="This column contains the Applicant Last name">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="DOB" FilterControlAltText="Filter DOB column" HeaderStyle-Width="350"
                            AllowFiltering="false" HeaderText="DOB" SortExpression="DOB" HeaderTooltip="This column contains the Applicant Date of Birth"
                            UniqueName="DOB" DataFormatString="{0:MM/dd/yyyy}">
                        </telerik:GridDateTimeColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>
</div>
