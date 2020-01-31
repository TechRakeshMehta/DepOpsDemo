<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BulkOrderUpload.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.BulkOrderUpload" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxBulkOrderUpload">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/ComplianceOperations/ManageBulkArchive.js" ResourceType="JavaScript" />   
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script type="text/javascript">

    $page.add_pageLoaded(function () {
        if (Telerik.Web.UI.RadAsyncUpload != null && Telerik.Web.UI.RadAsyncUpload != undefined) {
            Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
            Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
        }
    });

    



</script>

<div class='col-md-12'>
    <div class="row">
        <div class="msgbox" id="divSuccessMsg">
            <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
        </div>
    </div>
</div>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblBulkOrderUpload" runat="server" Text="Batch Order Upload"></asp:Label>
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Institution</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="ddlTenantName" runat="server" CssClass="form-control" CausesValidation="true" AutoPostBack="false"
                            DataTextField="TenantName" OnDataBound="ddlTenantName_DataBound" DataValueField="TenantID" Width="100%"
                            Filter="None" OnClientKeyPressing="openCmbBoxOnTab" AutoSkinMode="false" Skin="Silk">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvClient" ControlToValidate="ddlTenantName"
                                ValidationGroup="grpSubmit" InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-2'>
                        <span class="cptn" style="color: transparent !important; display: block;"></span>
                        <infs:WclButton ID="btnDownloadTemplate" ButtonType="StandardButton" runat="server"
                            AutoPostBack="true" OnClick="btnDownloadTemplate_Click"
                            Text="Download Template" ButtonPosition="Center" ValidationGroup=""
                            CssClass="redBtn">
                        </infs:WclButton>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Upload Document</span>
                        <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" MaxFileInputsCount="1"
                            MultipleFileSelection="Disabled" OnClientFileSelected="onClientFileSelected"
                            OnClientFileUploaded="onFileUploadedZeroSize" OnClientFileUploadRemoved="onFileRemoved"
                            UploadedFilesRendering="BelowFileInput"
                            AllowedFileExtensions="xls,xlsx" ToolTip="Click here to select files to upload from your computer"
                            Width="100%" CssClass="form-control">
                            <Localization Select="Browse" />
                        </infs:WclAsyncUpload>
                    </div>
                    <div class='form-group col-md-2'>
                        <span class="cptn" style="color: transparent !important; display: block;"></span>
                        <infs:WclButton ID="btnUploadTemplate" ButtonType="StandardButton" runat="server"
                            AutoPostBack="true" OnClick="btnUploadTemplate_Click"
                            Text="Upload Template" ButtonPosition="Center" ValidationGroup="grpSubmit"
                            CssClass="redBtn">
                        </infs:WclButton>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <%--<div class="col-md-12">
        <div class="row text-center">
            <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel" DefaultPanel="pnlSearch" DefaultPanelButton="Save"
                AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonText="Reset" SubmitButtonIconClass="rbUndo" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_ResetClick" OnSaveClick="fsucCmdBarButton_SearchClick"
                OnCancelClick="fsucCmdBarButton_CancelClick" ClearButtonIconClass="" UseAutoSkinMode="false" ButtonSkin="Silk" ValidationGroup="grpSubmit">
            </infsu:CommandBar>
        </div>
    </div>--%>
    <asp:UpdatePanel ID="udpnlPersonalAlias" runat="server">
        <ContentTemplate>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownloadTemplate" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="row">
        <infs:WclGrid runat="server" ID="grdApplicantSearchData" AllowCustomPaging="false"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            OnNeedDataSource="grdApplicantSearchData_NeedDataSource" OnItemCommand="grdApplicantSearchData_ItemCommand" EnableLinqExpressions="false"
            NonExportingColumns="" ShowClearFiltersButton="false" Visible="true">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <%--<telerik:GridBoundColumn DataField="InstituteName" HeaderText="Institution" AllowSorting="false" ItemStyle-Width="4%"
                        SortExpression="InstituteName" UniqueName="Institution" HeaderTooltip="This column displays the Institution for each record in the grid">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="Applicant First Name" ItemStyle-Width="4%"
                        SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Applicant Last Name" ItemStyle-Width="4%"
                        SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress" ItemStyle-Width="4%"
                        UniqueName="EmailAddress" HeaderTooltip="This column displays the applicant's email address for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrganizationUserId" HeaderText="User ID" SortExpression="OrganizationUserId"
                        HeaderTooltip="This column displays the User ID for each record in the grid" ItemStyle-Width="3%"
                        UniqueName="UserID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PackageID" HeaderText="Package ID" SortExpression="PackageID" ItemStyle-Width="4%"
                        UniqueName="PackageID" HeaderTooltip="This column displays the Package ID for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrderNodeID" HeaderText="Order Node ID" SortExpression="OrderNodeID" ItemStyle-Width="4%"
                        UniqueName="OrderNodeID" HeaderTooltip="This column displays the Order Node ID for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StartDate" HeaderText="Start Date"
                        AllowSorting="true" SortExpression="StartDate" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="4%"
                        UniqueName="StartDate" HeaderTooltip="This column displays the Start Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EndDate" HeaderText="End Date"
                        AllowSorting="true" SortExpression="EndDate" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="4%"
                        UniqueName="EndDate" HeaderTooltip="This column displays the End Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Interval" HeaderText="Interval"
                        SortExpression="Interval" ItemStyle-Width="4%"
                        UniqueName="Interval" HeaderTooltip="This column displays the Interval for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Account Exists?" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <%# String.IsNullOrEmpty(Convert.ToString(Eval("OrganizationUserId"))) 
                            ? "No"
                            : "Yes"
                            %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="BulkOrderStatusName" HeaderText="Status"
                        SortExpression="BulkOrderStatusName" ItemStyle-Width="4%"
                        UniqueName="BulkOrderStatusName" HeaderTooltip="This column displays the Status for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Notes" HeaderText="Notes"
                        SortExpression="Notes" ItemStyle-Width="4%"
                        UniqueName="Notes" HeaderTooltip="This column displays the Notes for each record in the grid">
                    </telerik:GridBoundColumn>

                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>
