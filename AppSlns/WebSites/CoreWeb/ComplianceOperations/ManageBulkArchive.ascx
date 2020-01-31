<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageBulkArchive.ascx.cs"
    Inherits="CoreWeb.ComplianceOperations.Views.ManageBulkArchive" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/ManageBulkArchive.js" ResourceType="JavaScript" />    
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>


<style type="text/css">
    .auto .RadPicker
    {
        width: 120px !important;
    }
</style>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Bulk Archive</h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='form-group col-md-3'>&nbsp;</div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Upload Document</span><span class='reqd'>*</span>
                        <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" MaxFileInputsCount="1"
                            MultipleFileSelection="Disabled" OnClientFileSelected="onClientFileSelected"
                            OnClientFileUploaded="onFileUploadedZeroSize" OnClientFileUploadRemoved="onFileRemoved"
                            UploadedFilesRendering="BelowFileInput"
                            AllowedFileExtensions="xls,xlsx" ToolTip="Click here to select files to upload from your computer"
                            Width="100%" CssClass="form-control">
                            <Localization Select="Browse" />
                        </infs:WclAsyncUpload>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
        ButtonSkin="Silk" UseAutoSkinMode="false"
        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonText="Reset" SubmitButtonIconClass="rbUndo"
        DefaultPanel="pnlSearch" DefaultPanelButton="Save"
        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
        ValidationGroup="grpFormSubmit"
        OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click">
    </infsu:CommandBar>

    <div class="row">
        <div class="col-md-12">
            <h2 id="hdrPkgSub" runat="server" class="header-color">Package Subscriptions</h2>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdMatchedApplicantSubscription" AutoGenerateColumns="False"
            AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
            ShowAllExportButtons="false" ShowExtraButtons="False" AllowCustomPaging="false"
            ShowClearFiltersButton="true" ClearFiltersButtonText="Clear Filters"
            GridLines="Both" OnNeedDataSource="grdMatchedApplicantSubscriptionh_NeedDataSource"
            OnItemDataBound="grdMatchedApplicantSubscription_ItemDataBound">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="PackageSubscriptionID"
                AllowFilteringByColumn="true">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="ArchiveCheckBox" AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectSubscription" runat="server" onclick="UnCheckHeader(this)"
                                OnCheckedChanged="chkSelectSubscription_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column"
                        AllowFiltering="true"
                        HeaderText="First Name" SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName"
                        ReadOnly="true" HeaderTooltip="This column contains the Applicant First name">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column"
                        AllowFiltering="true"
                        HeaderText="Last Name" SortExpression="ApplicantLastName" UniqueName="ApplicantLastName"
                        ReadOnly="true" HeaderTooltip="This column contains the Applicant Last name">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="ApplicantDOB" EnableTimeIndependentFiltering="true" HeaderStyle-Width="250"
                        AllowFiltering="true" HeaderText="DOB" SortExpression="ApplicantDOB" HeaderTooltip="This column contains the Applicant Date of Birth"
                        UniqueName="ApplicantDOB" DataFormatString="{0:MM/dd/yyyy}">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="InstitutionHierarchy" FilterControlAltText="Filter InstitutionHierarchy column"
                        AllowFiltering="true"
                        HeaderText="Institution Hierarchy" SortExpression="InstitutionHierarchy" UniqueName="InstitutionHierarchy"
                        ReadOnly="true" HeaderTooltip="This column contains the Institution Hierarchy">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                        AllowFiltering="true"
                        HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName"
                        ReadOnly="true" HeaderTooltip="This column contains the Package Name">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
    <div class="col-md-12">
        <div class="row text-center">
            <infsu:CommandBar ID="cmdArchiveSubscription" runat="server" ButtonPosition="Center"
                DisplayButtons="Save" ButtonSkin="Silk" UseAutoSkinMode="false"
                AutoPostbackButtons="Save" DefaultPanelButton="Save" SaveButtonText="Archive All Selected"
                SaveButtonIconClass="rbArchive"
                OnSaveClick="CmdArchiveSubscriptionSave_Click">
            </infsu:CommandBar>
        </div>
    </div>
    <div id="mainUnMatchedApplicant" runat="server" visible="false">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Un-Matched Applicants</h2>
            </div>
        </div>
        <div class="row">
            <infs:WclGrid runat="server" ID="grdUnMatchedApplicants" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="true"
                ShowAllExportButtons="false" ShowExtraButtons="False" AllowCustomPaging="false"
                ShowClearFiltersButton="false"
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
                        <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column"
                            HeaderStyle-Width="350" AllowFiltering="false"
                            HeaderText="First Name" SortExpression="FirstName" UniqueName="FirstName" ReadOnly="true"
                            HeaderTooltip="This column contains the Applicant First name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column"
                            HeaderStyle-Width="350" AllowFiltering="false"
                            HeaderText="Last Name" SortExpression="LastName" UniqueName="LastName" ReadOnly="true"
                            HeaderTooltip="This column contains the Applicant Last name">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="DOB" FilterControlAltText="Filter DOB column"
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

<script type="text/javascript">

   

    function CheckAll(id) {
        var masterTable = $find("<%= grdMatchedApplicantSubscription.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectSubscription").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectSubscription").checked = isChecked; // for checking the checkboxes
            }
        }
    }
    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdMatchedApplicantSubscription.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectSubscription").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectSubscription").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }
</script>
