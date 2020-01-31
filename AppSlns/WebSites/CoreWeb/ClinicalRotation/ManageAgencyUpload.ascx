<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageAgencyUpload.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.ManageAgencyUpload" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/ManageAgencyUpload.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Agency Upload
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <span class="cptn">Upload Document</span><span class='reqd'>*</span>
                <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                    MaxFileInputsCount="1" MultipleFileSelection="Disabled" OnClientFileSelected="onClientFileSelected"
                    OnClientFileUploaded="onFileUploadedZeroSize" OnClientFileUploadRemoved="onFileRemoved"
                    UploadedFilesRendering="BelowFileInput" AllowedFileExtensions="xls,xlsx" ToolTip="Click here to select files to upload from your computer"
                    AutoSkinMode="true" Width="100%" CssClass="form-control marginTop2">
                    <Localization Select="Browse" />
                </infs:WclAsyncUpload>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Save,Cancel"
            AutoPostbackButtons="Save,Cancel"
            SaveButtonText="Add / Update Agency" SaveButtonIconClass="rbSave" CancelButtonText="Cancel"
            ValidationGroup="grpFormSubmit"
            OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click" UseAutoSkinMode="false"
            ButtonSkin="Silk">
            <%--DefaultPanel="pnlSearch" DefaultPanelButton="Save"--%>
        </infsu:CommandBar>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Uploaded Agencies</h2>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdNPIAssociatedAndAgencyCreated" AutoGenerateColumns="False"
            AllowPaging="false"
            AllowSorting="false" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
            ShowAllExportButtons="false" ShowExtraButtons="False" AllowCustomPaging="false"
            ShowClearFiltersButton="false" ClearFiltersButtonText="Clear Filters"
            GridLines="Both" OnNeedDataSource="grdNPIAssociatedAndAgencyCreated_NeedDataSource"
            OnItemDataBound="grdNPIAssociatedAndAgencyCreated_OnItemDataBound">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" AllowFilteringByColumn="true"
                AllowPaging="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="NPINumber" FilterControlAltText="Filter NPINumber column"
                        AllowFiltering="false" FilterControlWidth="100px"
                        HeaderText="NPI Number" SortExpression="NPINumber" UniqueName="NPINumber" ReadOnly="true"
                        AllowSorting="false" HeaderTooltip="This column contains the NPI Number">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ReplacementNPI" FilterControlAltText="Filter ReplacementNPI column"
                        AllowFiltering="false" FilterControlWidth="100px"
                        HeaderText="Replacement NPI" SortExpression="ReplacementNPI" UniqueName="ReplacementNPI"
                        ReadOnly="true" AllowSorting="false" HeaderTooltip="This column contains the Replacement NPI">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AgencyName" FilterControlAltText="Filter AgencyName column"
                        AllowFiltering="false" FilterControlWidth="100px"
                        HeaderText="Agency Name" SortExpression="AgencyName" UniqueName="AgencyName"
                        ReadOnly="true" AllowSorting="false" HeaderTooltip="This column contains the Agency Name">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AgencyAddress1" FilterControlAltText="Filter AgencyAddress1 column"
                        AllowFiltering="false" FilterControlWidth="100px"
                        HeaderText="Address 1" SortExpression="AgencyAddress1" UniqueName="AgencyAddress1"
                        ReadOnly="true" AllowSorting="false" HeaderTooltip="This column contains the Agency Address1">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AgencyAddress2" FilterControlAltText="Filter AgencyAddress2 column"
                        AllowFiltering="false" FilterControlWidth="100px"
                        HeaderText="Address 2" SortExpression="AgencyAddress2" UniqueName="AgencyAddress2"
                        ReadOnly="true" AllowSorting="false" HeaderTooltip="This column contains the Agency Address2">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ZipCode" FilterControlAltText="Filter ZipCode column"
                        AllowFiltering="false" FilterControlWidth="100px"
                        HeaderText="Zip Code" SortExpression="ZipCode" UniqueName="ZipCode" ReadOnly="true"
                        AllowSorting="false" HeaderTooltip="This column contains the Zip Code">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="City" FilterControlAltText="Filter City column"
                        AllowFiltering="false" FilterControlWidth="100px"
                        HeaderText="City" SortExpression="City" UniqueName="City" ReadOnly="true" AllowSorting="false"
                        HeaderTooltip="This column contains the City">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StateAbbreviation" FilterControlAltText="Filter StateAbbreviation column"
                        AllowFiltering="false" FilterControlWidth="100px"
                        HeaderText="State" SortExpression="StateAbbreviation" UniqueName="StateAbbreviation"
                        ReadOnly="true" AllowSorting="false" HeaderTooltip="This column contains the State">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="IsAgencyCreated" FilterControlWidth="500px" UniqueName="IsAgencyCreated"
                        AllowFiltering="false" HeaderText="Action Performed" SortExpression="IsAgencyCreated"
                        AllowSorting="false">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle PageSizeControlType="None" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>

    <div id="divNotUploadedAgencies" runat="server" style="display: none">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Not Uploaded Agencies</h2>
            </div>
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdNotUploadedAgencies" AutoGenerateColumns="False"
                    AllowPaging="false"
                    AllowSorting="false" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                    ShowAllExportButtons="false" ShowExtraButtons="False" AllowCustomPaging="false"
                    ShowClearFiltersButton="false" ClearFiltersButtonText="Clear Filters"
                    GridLines="Both" OnNeedDataSource="grdNotUploadedAgencies_NeedDataSource">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" AllowFilteringByColumn="true"
                        AllowPaging="false">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                            ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="NPINumber" FilterControlAltText="Filter NPINumber column"
                                AllowFiltering="false" FilterControlWidth="100px"
                                HeaderText="NPI Number" SortExpression="NPINumber" UniqueName="NPINumber" ReadOnly="true"
                                AllowSorting="false" HeaderTooltip="This column contains the NPI Number">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ReplacementNPI" FilterControlAltText="Filter ReplacementNPI column"
                                AllowFiltering="false" FilterControlWidth="100px"
                                HeaderText="Replacement NPI" SortExpression="ReplacementNPI" UniqueName="ReplacementNPI"
                                ReadOnly="true" AllowSorting="false" HeaderTooltip="This column contains the Replacement NPI">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AgencyName" FilterControlAltText="Filter AgencyName column"
                                AllowFiltering="false" FilterControlWidth="100px"
                                HeaderText="Agency Name" SortExpression="AgencyName" UniqueName="AgencyName"
                                ReadOnly="true" AllowSorting="false" HeaderTooltip="This column contains the Agency Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AgencyAddress1" FilterControlAltText="Filter AgencyAddress1 column"
                                AllowFiltering="false" FilterControlWidth="100px"
                                HeaderText="Address 1" SortExpression="AgencyAddress1" UniqueName="AgencyAddress1"
                                ReadOnly="true" AllowSorting="false" HeaderTooltip="This column contains the Agency Address1">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AgencyAddress2" FilterControlAltText="Filter AgencyAddress2 column"
                                AllowFiltering="false" FilterControlWidth="100px"
                                HeaderText="Address 2" SortExpression="AgencyAddress2" UniqueName="AgencyAddress2"
                                ReadOnly="true" AllowSorting="false" HeaderTooltip="This column contains the Agency Address2">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ZipCode" FilterControlAltText="Filter ZipCode column"
                                AllowFiltering="false" FilterControlWidth="100px"
                                HeaderText="Zip Code" SortExpression="ZipCode" UniqueName="ZipCode" ReadOnly="true"
                                AllowSorting="false" HeaderTooltip="This column contains the Zip Code">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="City" FilterControlAltText="Filter City column"
                                AllowFiltering="false" FilterControlWidth="100px"
                                HeaderText="City" SortExpression="City" UniqueName="City" ReadOnly="true" AllowSorting="false"
                                HeaderTooltip="This column contains the City">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="StateAbbreviation" FilterControlAltText="Filter StateAbbreviation column"
                                AllowFiltering="false" FilterControlWidth="100px"
                                HeaderText="State" SortExpression="StateAbbreviation" UniqueName="StateAbbreviation"
                                ReadOnly="true" AllowSorting="false" HeaderTooltip="This column contains the State">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <PagerStyle PageSizeControlType="None" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <%--<PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>--%>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>

        </div>
    </div>
</div>

<script type="text/javascript">

    function pageLoad() {
        SetDefaultButtonForSection("divSearchPanel", "CmdBarSearch_btnSave", true);
    }
</script>



