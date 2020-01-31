<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClinicalRotationMapping.aspx.cs" MasterPageFile="~/Shared/ChildPage.master" Inherits="CoreWeb.ComplianceOperations.Views.ClinicalRotationMapping" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxClinicalRotationMappingPopup">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="../Resources/Generic/popup.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotation.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <script type="text/javascript">

        $page.add_pageLoad(function () {
            var $ = $jQuery;
            $(".grdCmdBar .RadButton").each(function () {
                if ($(this).text().toLowerCase() == "add new rotation") {
                    $(this).attr("title", "Click to add a new rotation");
                }
            });
        });

       

    </script>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Rotation Mapping
                </h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="msgbox">
                    <asp:Label ID="lblMessage" runat="server" CssClass="info">
                    </asp:Label>
                </div>
            </div>
        </div>
        <div id="DivGridRotations" runat="server" class="row allowscroll" style="height: auto">
            <infs:WclGrid runat="server" ID="grdRotations" AllowCustomPaging="true"
                AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
                OnNeedDataSource="grdRotations_NeedDataSource" OnItemCommand="grdRotations_ItemCommand" OnInit="grdRotations_Init"
                OnSortCommand="grdRotations_SortCommand" OnItemDataBound="grdRotations_ItemDataBound"
                OnItemCreated="grdRotations_ItemCreated" NonExportingColumns="EditCommandColumn,DeleteColumn" EnableLinqExpressions="false">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                    <%--   <Scrolling AllowScroll="true" UseStaticHeaders="true" />--%>
                    <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="RotationID,IsEditableByClientAdmin"
                    AllowFilteringByColumn="true">

                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Rotation"
                        ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />

                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="AssignItems" HeaderTooltip="Click this box to select all rotations on the active page"
                            AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="30">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectItem" runat="server"
                                    onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectItem_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency" SortExpression="AgencyName"
                            HeaderTooltip="This column displays the Agency name for each record in the grid" HeaderStyle-Width="150"
                            UniqueName="AgencyName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HierarchyNodes" HeaderText="Hierarchy" SortExpression="HierarchyNodes"
                            HeaderTooltip="This column displays the Hierarchy for each record in the grid" HeaderStyle-Width="150"
                            UniqueName="HierarchyNodes">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ComplioID" HeaderText="Complio ID" SortExpression="RotationID" HeaderStyle-Width="120"
                            UniqueName="ComplioID" HeaderTooltip="This column displays the Complio ID for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RotationName" HeaderText="Rotation ID/Name" SortExpression="RotationName" HeaderStyle-Width="150"
                            UniqueName="RotationName" HeaderTooltip="This column displays the Location for each record in the grid">
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="RotationReviewStatusName" HeaderText="Rotation Review Status" SortExpression="RotationReviewStatusName" HeaderStyle-Width="150"
                            UniqueName="RotationReviewStatusName" HeaderTooltip="This column displays the Rotation Review Status for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TypeSpecialty" HeaderText="Type/Specialty" SortExpression="TypeSpecialty" HeaderStyle-Width="120"
                            UniqueName="TypeSpecialty" HeaderTooltip="This column displays the Type/Specialty for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Department" HeaderText="Department" SortExpression="Department" HeaderStyle-Width="120"
                            UniqueName="Department" HeaderTooltip="This column displays the department name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Program" HeaderText="Program" HeaderStyle-Width="120"
                            SortExpression="Program" UniqueName="Program" HeaderTooltip="This column displays the program name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Course" HeaderText="Course" SortExpression="Course" HeaderStyle-Width="120"
                            UniqueName="Course" HeaderTooltip="This column displays the course for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Term" HeaderText="Term" SortExpression="Term" HeaderStyle-Width="120"
                            UniqueName="Term" HeaderTooltip="This column displays the term for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UnitFloorLoc" HeaderText="Unit/Floor" SortExpression="UnitFloorLoc" HeaderStyle-Width="140"
                            UniqueName="UnitFloorLoc" HeaderTooltip="This column displays the Unit/Floor for each record in the grid"
                            DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                        </telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="Students" HeaderText="# of Students" DataType="System.Decimal"
                            SortExpression="Students" HeaderStyle-Width="120" FilterControlWidth="80"
                            UniqueName="Students" HeaderTooltip="This column displays the # of Students for each record in the grid">
                        </telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="RecommendedHours" HeaderText="# of Recommended Hours" DataType="System.Decimal"
                            SortExpression="RecommendedHours" HeaderStyle-Width="120" FilterControlWidth="80"
                            UniqueName="RecommendedHours" HeaderTooltip="This column displays the # of Recommended Hours for each record in the grid">
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="DaysName" HeaderText="Days" AllowSorting="false" HeaderStyle-Width="150"
                            UniqueName="DaysName" HeaderTooltip="This column displays days for each record in the grid">
                            <ItemStyle Wrap="true" Width="150px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Shift" HeaderText="Shift" SortExpression="Shift" HeaderStyle-Width="120"
                            UniqueName="Shift" HeaderTooltip="This column displays shift for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Time" HeaderText="Time" AllowSorting="false" HeaderStyle-Width="120px"
                            UniqueName="Time" HeaderTooltip="This column displays the time for each record in the grid">
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Created Date"  HeaderStyle-Width="120px"
                            AllowSorting="true" SortExpression="CreatedDate" DataFormatString="{0:MM/dd/yyyy}"
                            UniqueName="CreatedDate" HeaderTooltip="This column displays the Created Date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="StartDate" HeaderText="Start Date" HeaderStyle-Width="120"
                            AllowSorting="true" SortExpression="StartDate" DataFormatString="{0:MM/dd/yyyy}"
                            UniqueName="StartDate" HeaderTooltip="This column displays the Start Date for each record in the grid">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridDateTimeColumn DataField="EndDate" HeaderText="End Date" HeaderStyle-Width="120"
                            AllowSorting="true" SortExpression="EndDate" DataFormatString="{0:MM/dd/yyyy}"
                            UniqueName="EndDate" HeaderTooltip="This column displays the End Date for each record in the grid">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="ContactNames" HeaderText="Instructor/Preceptor"
                            AllowSorting="false" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Justify"
                            UniqueName="ContactName" HeaderTooltip="This column displays the Instructor/Preceptor for each record in the grid">
                        </telerik:GridBoundColumn>                        
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="container-fluid" style="display: none;">
                                <div class="row">
                                    <div class="col-md-12">
                                        <h2 class="header-color">
                                            <asp:Label ID="lblRotation" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Rotation" : "Update Rotation" %>'
                                                runat="server" />

                                        </h2>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <asp:Panel ID="pnlEditForm" runat="server" Visible="false">
                                    <div class="row bgLightGreen">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3 col-sm-3' title="Select the Institution whose data you want to view">
                                                    <span class="cptn">Institution</span><span class='reqd'>*</span>

                                                    <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                                        DataValueField="TenantID" Filter="None" OnClientKeyPressing="openCmbBoxOnTab"
                                                        OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                                                        Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                                            InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                            Text="Institution is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3' title="Select a Agency whose data you want to view.">
                                                    <span class="cptn lineHeight">Agency</span><span class="reqd">*</span>
                                                    <infs:WclComboBox ID="ddlAgency" runat="server" DataTextField="AgencyName" DataValueField="AgencyID"
                                                        AutoPostBack="false" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAgency" ControlToValidate="ddlAgency"
                                                            InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Agency is required." />
                                                    </div>
                                                </div>
                                                <div id="dvComplioId" runat="server">
                                                    <div class='form-group col-md-3 col-sm-3'>
                                                        <span class="cptn">Complio ID</span>
                                                        <infs:WclTextBox CssClass="form-control" Width="100%" ID="txtComplioId" runat="server"
                                                            Text='<%# Eval("ComplioID") %>' ReadOnly="true">
                                                        </infs:WclTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-12'>
                                                    <span class="cptn">Hierarchy Nodes</span><span class='reqd'>*</span>
                                                    <a style="color: blue;" href="#" id="lnkInstitutionHierarchyPB" onclick="OpenInstitutionHierarchyPopupInsideGrid(true);">Select Institution Hierarchy</a><br />
                                                    <asp:Label ID="lblInstitutionHierarchyPB" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn">Rotation ID/Name</span>

                                                    <infs:WclTextBox ID="txtClassification" runat="server" Text='<%# Eval("RotationName") %>'
                                                        Width="100%" CssClass="form-control">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn">Type/Specialty</span>
                                                    <infs:WclTextBox ID="txtTypeSpecialty" runat="server" MaxLength="256" Text='<%# Eval("TypeSpecialty") %>'
                                                        Width="100%" CssClass="form-control">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn">Department</span><span class="reqd">*</span>
                                                    <infs:WclTextBox ID="txtDepartment" runat="server" Text='<%# Eval("Department") %>'
                                                        Width="100%" CssClass="form-control">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDepartment" ControlToValidate="txtDepartment"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Department is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn">Program</span><span class="reqd">*</span>
                                                    <infs:WclTextBox ID="txtProgram" runat="server" Text='<%# Eval("Program") %>' Width="100%"
                                                        CssClass="form-control">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvProgram" ControlToValidate="txtProgram"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Program is required." />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn">Course</span><span class="reqd">*</span>
                                                    <infs:WclTextBox ID="txtCourse" runat="server" Text='<%# Eval("Course") %>' Width="100%"
                                                        CssClass="form-control">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCourse" ControlToValidate="txtCourse"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Course is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3' title="Restrict search results to the selected term">
                                                    <span class="cptn">Term</span>
                                                    <infs:WclTextBox ID="txtTerm" Text='<%# Eval("Term") %>' runat="server" Width="100%"
                                                        CssClass="form-control">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn">Unit/Floor or Location</span>
                                                    <infs:WclTextBox ID="txtUnit" runat="server" Text='<%# Eval("UnitFloorLoc") %>' Width="100%"
                                                        CssClass="form-control">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn"># of Students</span>
                                                    <infs:WclNumericTextBox ID="txtStudents" runat="server"
                                                        Text='<%# Eval("Students") %>' NumberFormat-DecimalDigits="2" Width="100%"
                                                        CssClass="form-control">
                                                    </infs:WclNumericTextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn"># of Recommended Hours</span>
                                                    <infs:WclNumericTextBox ID="txtRecommendedHrs" runat="server"
                                                        Text='<%# Eval("RecommendedHours") %>' NumberFormat-DecimalDigits="2" Width="100%"
                                                        CssClass="form-control">
                                                    </infs:WclNumericTextBox>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn lineHeight">Days</span>
                                                    <infs:WclComboBox ID="ddlDays" runat="server" CheckBoxes="true" DataValueField="WeekDayID"
                                                        DataTextField="Name"
                                                        Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn">Shift</span>
                                                    <infs:WclTextBox ID="txtShift" runat="server" Text='<%# Eval("Shift") %>' Width="100%"
                                                        CssClass="form-control">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn lineHeight">Instructor/Preceptor</span>
                                                    <infs:WclComboBox ID="ddlInstructor" runat="server" CheckBoxes="true" DataValueField="ClientContactID"
                                                        DataTextField="Name"
                                                        Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn">Time</span>
                                                    <infs:WclTimePicker ID="tpStartTime" runat="server" DateInput-EmptyMessage="Start Time"
                                                        TimeView-Height="300px" Width="100%" CssClass="form-control">
                                                        <TimeView Interval="00:30:00"></TimeView>
                                                    </infs:WclTimePicker>
                                                    <div class="gclrPad"></div>
                                                    <infs:WclTimePicker ID="tpEndTime" DateInput-EmptyMessage="End Time" runat="server"
                                                        Width="100%" CssClass="form-control">
                                                        <TimeView Interval="00:30:00"></TimeView>
                                                    </infs:WclTimePicker>
                                                    <div class="vldx">
                                                        <asp:CustomValidator ID="cstStartFrm" runat="server" ErrorMessage="EndTime Is Required."
                                                            ValidationGroup="grpFormSubmit"
                                                            CssClass="errmsg" Display="Dynamic" ClientValidationFunction="ValidateStartEndTime"
                                                            ClientIDMode="Static" SetFocusOnError="True">
                                                        </asp:CustomValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group col-md-3 col-sm-3">
                                                    <span class="cptn">Start Date</span><span class="reqd">*</span>

                                                    <infs:WclDatePicker ID="dpStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                                                        ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate"
                                                        Width="100%" CssClass="form-control">
                                                    </infs:WclDatePicker>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="dpStartDate"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="Start Date is required." />
                                                    </div>
                                                </div>
                                                <div class="form-group col-md-3 col-sm-3">
                                                    <span class="cptn">End Date</span><span class="reqd">*</span>

                                                    <infs:WclDatePicker ID="dpEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                                                        ClientEvents-OnPopupOpening="SetMinEndDate" Width="100%" CssClass="form-control">
                                                    </infs:WclDatePicker>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="dpEndDate"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                            Text="End Date is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn lineHeight">Syllabus Document</span>

                                                    <div class="bx_uploader" title="Click this button to upload syllabus">
                                                        <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                                                            MultipleFileSelection="Disabled" MaxFileInputsCount="1" OnClientFileSelected="onClientFileSelected"
                                                            OnClientFileUploaded="onFileUploaded" OnClientValidationFailed="upl_OnClientValidationFailed"
                                                            Localization-Select="Browse" Width="100%" CssClass="form-control marginTop2"
                                                            AutoSkinMode="true"
                                                            AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT" />
                                                    </div>
                                                    <div style="clear: both; float: left; position: relative;">
                                                        <asp:Label ID="lblUploadFormName" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblUploadFormPath" runat="server" Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" Visible="false" OnClick="lnkRemove_Click"
                                                            ToolTip="Click this button to remove document"></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h2 class="header-color">Nag Notification Settings</h2>
                                        </div>
                                    </div>
                                    <div class="row bgLightGreen">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn lineHeight">Days Before</span>

                                                    <infs:WclNumericTextBox ID="txtDaysBefore" runat="server" MaxLength="3" Text='<%# Eval("DaysBefore") %>'
                                                        Width="100%" CssClass="form-control">
                                                        <NumberFormat AllowRounding="false" DecimalDigits="0" />
                                                    </infs:WclNumericTextBox>
                                                </div>
                                                <div class='form-group col-md-3 col-sm-3'>
                                                    <span class="cptn lineHeight">Frequency</span>

                                                    <infs:WclNumericTextBox ID="txtFrequency" runat="server" MaxLength="3" Text='<%# Eval("Frequency") %>'
                                                        Width="100%" CssClass="form-control">
                                                        <NumberFormat AllowRounding="false" DecimalDigits="0" />
                                                    </infs:WclNumericTextBox>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <infsu:CommandBar ID="fsucCmdBarRotation" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                    GridInsertText="Save" GridUpdateText="Save"
                                    ValidationGroup="grpFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                        Position="TopAndBottom" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="row">
            <div class="col-md-12">
                &nbsp;
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group col-md-9 pull-right">
                    <div class="row text-right" id="trailingText">
                        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Right" DisplayButtons="Submit,Save,Cancel"
                            AutoPostbackButtons="Save" SaveButtonText="Assign" SubmitButtonText="Unassign" SaveButtonIconClass="rbAssign" OnSubmitClientClick="ConfirmationMessage"
                            SubmitButtonIconClass="rbUnassign" CancelButtonText="Cancel" OnSubmitClick="CmdBarUnassign_Click" OnSaveClick="CmdBarAssign_Click"
                            OnCancelClientClick="ClosePopup" UseAutoSkinMode="false" ButtonSkin="Silk">
                        </infsu:CommandBar>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--UAT-4147--%>
<div id="divExistingRotationMembers" class="acknowledgeMessagePopup" title="Complio" runat="server" style="display: none">
    <p style="text-align: left;">One or more of the selected Applicant(s) is already added as Instructor/Preceptor. Please review your selection: </p>
    <div>&nbsp;</div>
    <asp:Panel ID="pnlExistingRotationMembers" runat="server">
    </asp:Panel>
</div>

    <%--UAT-4323--%>
<div id="divRotationApplicantLimitNotificationPopup" class="rotationApplicantLimitNotificationPopup" title="Complio" runat="server" style="display: none">
    <p style="text-align: left;">You cannot add Applicant(s) to a Rotation more than the limit specified in '# of Students' field. Please review your selection: </p>
    <div>&nbsp;</div>
    <asp:Panel ID="pnlRotationApplicantLimit" runat="server">
    </asp:Panel>
</div>
    <asp:Button runat="server" Style="display: none;" OnClick="btnRelod_Click" ID="btnRelod" />

    <asp:HiddenField ID="hdnFileRemoved" runat="server" />
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
    <asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnSelectedTenantId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnNeedToShowRotSaveMsg" Value="0" runat="server" />
    <div class="approvepopup" runat="server" style="display: none">
        <div style="float:left;width:50px" ><img src="../../Resources/Themes/Default/images/info.png" /></div>
        <div>By deleting the student(s) from the rotation, any data previously submitted for this rotation will be erased.</div>  
    </div>
    <script type="text/javascript">

        function ValidateStartEndTime(sender, args) {
            var tpStartTime = $jQuery("[id$=tpStartTime]")[0].control.get_timeView().getTime();
            var tpEndTime = $jQuery("[id$=tpEndTime]")[0].control.get_timeView().getTime();
            if (tpEndTime != null && tpStartTime == null) {
                sender.innerText = 'Rotation "Start Time" is required.'
                args.IsValid = false;
            }
            if (tpStartTime != null && tpEndTime == null) {
                sender.innerText = 'Rotation "End Time" is required.'
                args.IsValid = false;
            }
        }

        function CheckAll(id) {
            var masterTable = $find("<%= grdRotations.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            var isChecked = false;
            if (id.checked == true) {
                var isChecked = true;
            }
            for (var i = 0; i < row.length; i++) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled == true)) {
                    masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
                }
            }
        }

        function UnCheckHeader(id) {
            var checkHeader = true;
            var masterTable = $find("<%= grdRotations.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            for (var i = 0; i < row.length; i++) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                    if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                        checkHeader = false;
                        break;
                    }
                }
            }
            $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
        }


        function show_progress_OnSubmit() {
            Page.showProgress('Processing...');
        }

        //function to get current popup window
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        //Function to close popup window
        function ClosePopup() {
            var oArg = {};
            oArg.Action = "Cancel";
            top.$window.get_radManager().getActiveWindow().close();
        }

        //Function to redirect to parent 
        function RedirectToParent() {
            var oArg = {};
            oArg.Action = "Submit";
            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }

        function OpenAddEditRotationPopup(screenMode) {

            var win = $page.get_window();
            if (win) {
                win.restore();
            }

            var tenantID = $jQuery("[id$=hdnSelectedTenantId]").val();
            if (parseInt(tenantID) > 0) {
                var popupWindowName = "Add Rotation";
                //UAT-2364
                var popupHeight = $jQuery(window).height() * (100 / 100);

                var url = $page.url.create("/ComplianceOperations/Pages/AddEditRotation.aspx?TenantID=" + tenantID + "&ScreenMode=" + screenMode);
                var win = $window.createPopup(url, { size: "1000,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnCloseAddEditRotationPopup }
                   );
            }
            else {
                alert("Please select institue");
            }
            return false;
        }

        function OnCloseAddEditRotationPopup(oWnd, args) {
            oWnd.remove_close(OnCloseAddEditRotationPopup);
            var arg = args.get_argument();
            //if (arg) {
            //    if (arg.Action == "Submit") {
            //masterTable.rebind();
            //    }
            //}
        }

        function refreshRotationMappingGrid() {
            $jQuery("[id$=hdnNeedToShowRotSaveMsg]").val('1');
            $jQuery("[id$=btnRelod]").click();
        }

        //UAT-2225
        function ConfirmationMessage(sender, args) {            
            $window.showDialog($jQuery(".approvepopup").clone().show(), {
                continuebtn: {
                    autoclose: true, text: "Continue", click: function () {
                        __doPostBack('<%= fsucCmdBarButton.SubmitButton.UniqueID %>', '');
                    }
                 }, closeBtn: {
                    autoclose: true, text: "Cancel"
                }
            }, 475, 'Complio');
        }
        //UAT-4147
        function ShowExistingRotationMembers() {
            // debugger;
            var dialog = $window.showDialog($jQuery("[id$=divExistingRotationMembers]").clone().show(), {
                approvebtn: {
                    autoclose: true, text: "Ok", click: function () {
                        return false;
                    }
                }
            }, 550, 'Notice');
        }

        //UAT-4323
        function ShowRotationApplicantLimitViolationNotification() {
            // debugger;
            var dialog = $window.showDialog($jQuery("[id$=divRotationApplicantLimitNotificationPopup]").clone().show(), {
                approvebtn: {
                    autoclose: true, text: "Ok", click: function () {
                        return false;
                    }
                }
            }, 550, 'Complio');
        }
    </script>

</asp:Content>

