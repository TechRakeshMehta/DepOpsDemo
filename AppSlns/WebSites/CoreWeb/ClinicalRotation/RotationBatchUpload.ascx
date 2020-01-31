<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RotationBatchUpload.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.RotationBatchUpload" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxBulkOrderUpload">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/ComplianceOperations/ManageBulkRotation.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/AgencyHierarchy/AgencyHierarchyMultipleSelection.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script type="text/javascript">

    //$page.add_pageLoaded(function () {
    //    if (Telerik.Web.UI.RadAsyncUpload != null && Telerik.Web.UI.RadAsyncUpload != undefined) {
    //        Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
    //        Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
    //    }
    //});

  

    function BindInstitutionLabel() {
        setTimeout(function () {
            var InstNodeLabel = $jQuery("[id$=hdnInstNodeLabel]").val();
            $jQuery($jQuery("[id$=lblInstitutionHierarchyPB]")[0]).text(InstNodeLabel);
        }, 1100);
    }
    $jQuery(document).ready(function () {
        $jQuery(document).on('click', '[id$=lnkInstitutionHierarchyPB]', function (e) {
            if (!$jQuery(this).hasClass('disabled')) {
                OpenInstitutionHierarchyPopupInsideGrid(false);
            }
        });
    });

    function ResetAgencyHierarchySelection() {
        $jQuery($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_lblAgencyHierarchy")[0]).text('');
        $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_AgencyHierarchyNodeIds").val('');
        $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnSelectedAgecnyIds").val('');
        $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnSelectedRootNodeId").val('');
    }
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
                <asp:Label ID="lblBulkRotationUpload" runat="server" Text="Batch Rotation Upload"></asp:Label>
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlInstitution" runat="server">
            <div class="col-md-12">
                <div class="row">
                     <div class='form-group col-md-3'>
                        <span class='cptn'>Institution</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="ddlTenant" runat="server" CssClass="form-control" CausesValidation="true" AutoPostBack="true"
                            DataTextField="TenantName" OnDataBound="ddlTenant_DataBound" DataValueField="TenantID" Width="100%" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Filter="None" OnClientKeyPressing="openCmbBoxOnTab" AutoSkinMode="false" Skin="Silk">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvClient" ControlToValidate="ddlTenant"
                                ValidationGroup="grpSubmit" InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Select a Agency whose data you want to view.">
                        <div style="margin-top: 5%">
                            <uc:AgencyHierarchyMultiple ID="ucAgencyHierarchyMultipleToUploadRotation" runat="server" />
                             <asp:Label CssClass="errmsg" ID="lblAgencyErr" Text="Agency is required." Visible="false" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                   
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

                    <div class='form-group col-md-2'>
                        <span class="cptn" style="color: transparent !important; display: block;"></span>
                        <infs:WclButton ID="btnDownloadTemplate" ButtonType="StandardButton" runat="server"
                            AutoPostBack="true" OnClick="btnDownloadTemplate_Click"
                            Text="Download Template" ButtonPosition="Center" ValidationGroup=""
                            CssClass="redBtn">
                        </infs:WclButton>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <asp:UpdatePanel ID="udpnlPersonalAlias" runat="server">
        <ContentTemplate>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownloadTemplate" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered rotation name">
                        <span class="cptn">Rotation ID/Name</span>

                        <infs:WclTextBox ID="txtRotationName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered start date">
                        <span class="cptn">Start Date</span>

                        <infs:WclDatePicker ID="dpStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered end date">
                        <span class="cptn">End Date</span>

                        <infs:WclDatePicker ID="dpEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Select &#34Pending&#34 to view all the &#34Pending&#34 rotation(s) or select &#34Successful&#34 to view all the &#34Successful&#34 rotation(s) or select &#34Failed&#34 to view all the &#34Failed&#34 rotation(s).">
                        <span class="cptn">Upload Status</span>
                        <asp:RadioButtonList ID="rbUploadStatus" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Pending" Value="AAAA" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Successful" Value="AAAB"></asp:ListItem>
                            <asp:ListItem Text="Failed" Value="AAAC"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="col-md-12">&nbsp;</div>
    <div class="col-md-12">
        <div id="dvCommandBar1" runat="server" class="row text-center">
            <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo" OnSaveClick="fsucCmdBarButton_SaveClick"
                OnCancelClick="fsucCmdBarButton_CancelClick" OnSubmitClick="fsucCmdBarButton_SubmitClick"
                SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch" ValidationGroup="grpSubmit"
                CancelButtonText="Cancel" UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
        </div>
    </div>



    <div class="row">
        <infs:WclGrid runat="server" ID="grdRotationBatchUpload" AllowCustomPaging="false"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            OnNeedDataSource="grdRotationBatchUpload_NeedDataSource" OnItemCommand="grdRotationBatchUpload_ItemCommand" EnableLinqExpressions="false"
            NonExportingColumns="" ShowClearFiltersButton="false" Visible="true">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames=""
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="Agency" HeaderText="Agency" SortExpression="AgencyName"
                        HeaderTooltip="This column displays the Agency name for each record in the grid"
                        UniqueName="AgencyName">
                    </telerik:GridBoundColumn>
                    <%-- <telerik:GridBoundColumn DataField="HierarchyNodes" HeaderText="Hierarchy" SortExpression="HierarchyNodes"
                        HeaderTooltip="This column displays the Hierarchy for each record in the grid" HeaderStyle-Width="250px"
                        UniqueName="HierarchyNodes">
                    </telerik:GridBoundColumn>--%>
                    <%--  <telerik:GridBoundColumn DataField="ComplioID" HeaderText="Complio ID" SortExpression="ComplioID"
                        UniqueName="ComplioID" HeaderTooltip="This column displays the Complio ID for each record in the grid">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn DataField="Rotation_Name" HeaderText="Rotation ID/Name" SortExpression="RotationName"
                        UniqueName="RotationName" HeaderTooltip="This column displays the Rotation ID/Name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Rotation_Review_Status" HeaderText="Rotation Review Status" SortExpression="RotationReviewStatusName"
                        UniqueName="RotationReviewStatusName" HeaderTooltip="This column displays the Rotation Uploaded Status for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Type_Specialty" HeaderText="Type/Specialty" SortExpression="TypeSpecialty"
                        HeaderStyle-Width="100px"
                        UniqueName="TypeSpecialty" HeaderTooltip="This column displays the Type/Specialty for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Department" HeaderText="Department" SortExpression="Department"
                        HeaderStyle-Width="100px"
                        UniqueName="Department" HeaderTooltip="This column displays the department name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Program" HeaderText="Program"
                        SortExpression="Program" UniqueName="Program" HeaderTooltip="This column displays the program name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Course" HeaderText="Course" SortExpression="Course"
                        UniqueName="Course" HeaderTooltip="This column displays the course for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Term" HeaderText="Term" SortExpression="Term"
                        UniqueName="Term" HeaderTooltip="This column displays the term for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Unit_Floor" HeaderText="Unit/Floor" SortExpression="UnitFloorLoc"
                        UniqueName="unit" HeaderTooltip="This column displays the Unit/Floor for each record in the grid"
                        DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Students" HeaderText="# of Students"
                        SortExpression="Students" HeaderStyle-Width="100px"
                        UniqueName="Students" HeaderTooltip="This column displays the # of Students for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Recommended_Hours" HeaderText="# of Recommended Hours"
                        SortExpression="RecommendedHours" HeaderStyle-Width="100px"
                        UniqueName="RecommendedHours" HeaderTooltip="This column displays the # of Recommended Hours for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Days" HeaderText="Days" AllowSorting="false"
                        UniqueName="Days" HeaderTooltip="This column displays days for each record in the grid">
                        <ItemStyle Wrap="true" Width="10px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Shift" HeaderText="Shift" SortExpression="Shift"
                        UniqueName="Shift" HeaderTooltip="This column displays shift for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Time" HeaderText="Time" AllowSorting="false"
                        HeaderStyle-Width="100px"
                        UniqueName="Time" HeaderTooltip="This column displays the time for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CreatedOn" HeaderText="Upload Date"
                        AllowSorting="true" SortExpression="UploadDate" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="UploadDate" HeaderTooltip="This column displays the Upload Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StartDate" HeaderText="Start Date"
                        AllowSorting="true" SortExpression="StartDate" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="StartDate" HeaderTooltip="This column displays the Start Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EndDate" HeaderText="End Date"
                        AllowSorting="true" SortExpression="EndDate" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="EndDate" HeaderTooltip="This column displays the End Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Instructor_Preceptor" HeaderText="Instructor/Preceptor"
                        AllowSorting="false" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Justify"
                        UniqueName="ContactName" HeaderTooltip="This column displays the Instructor/Preceptor for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="BatchRotationErrorMessage" HeaderText="Notes" AllowSorting="false"
                        HeaderStyle-Width="100px"
                        UniqueName="Notes" HeaderTooltip="This column displays the notes for each record in the grid">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
    <div id="dvCreateButton" style="display: none" runat="server" class='col-md-12'>
        <div class="row">
            <div class='form-group col-md-5'>
            </div>
            <div class='form-group col-md-2'>
                <span class="cptn" style="color: transparent !important; display: block;"></span>
                <infs:WclButton ID="btnCreate" ButtonType="StandardButton" runat="server"
                    AutoPostBack="true" OnClick="btnCreate_Click"
                    Text="Create Rotation(s)" ButtonPosition="Center" ValidationGroup=""
                    CssClass="redBtn">
                </infs:WclButton>
            </div>
        </div>
    </div>
</div>


<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnTenantIdNew" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
<asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
<asp:HiddenField ID="hdnCurrentRotStartDate" runat="server" Value="" />
<asp:HiddenField ID="hdnInstNodeLabel" runat="server" Value="" />
