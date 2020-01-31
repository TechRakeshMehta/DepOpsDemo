<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageRotationByAgency.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.ManageRotationByAgency" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotation.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/AgencyHierarchy/AgencyHierarchyMultipleSelection.js" ResourceType="JavaScript" /> 
</infs:WclResourceManagerProxy>

<script>
    $page.add_pageLoaded(function () {
        if (Telerik.Web.UI.RadAsyncUpload != null && Telerik.Web.UI.RadAsyncUpload != undefined) {
            Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
            Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
        }
    });

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
    function ValidateDaysBeforeFrequency(sender, args) {
        //var txtDaysBefore = $jQuery("[id$=txtDaysBefore]").innerText();
        //var txtFrequency = $jQuery("[id$=txtFrequency]").innerText();

        //if (txtDaysBefore != null && txtFrequency == null) {
        //    sender.innerText = 'Rotation "Frequency " is required.'
        //    args.IsValid = false;
        //}
        //if (txtDaysBefore == null && txtFrequency != null) {
        //    sender.innerText = 'Rotation "Days Before" is required.'
        //    args.IsValid = false;
        //}
    }


</script>


<div id="dvTop" class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Rotation By Agency
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span>

                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName"  EmptyMessage="--SELECT--"
                                DataValueField="TenantID" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Enabled="false"   AutoPostBack="false"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                               <%-- <Localization CheckAllString="All" />--%>
                            </infs:WclComboBox>

                        </div>

                        <div class='form-group col-md-3' title="Select a Agency whose data you want to view.">
                            <%--<span class="cptn">Agency</span>--%>

                            <%--<infs:WclComboBox ID="ddlAgency" runat="server" DataTextField="AgencyName" DataValueField="AgencyID"
                                CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="--SELECT--"
                                AutoPostBack="false"  Width="100%" CssClass="form-control"
                                Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>--%>

                            <div style="margin-top: 5%">
                                <uc:AgencyHierarchyMultiple ID="ucAgencyHierarchyMultipleToSearchRotation" runat="server" />
                            </div>

                        </div>
                    </div>
                </div>

            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Complio ID">
                        <span class="cptn">Complio ID</span>

                        <infs:WclTextBox ID="txtComplioId" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered rotation name">
                        <span class="cptn">Rotation ID/Name</span>

                        <infs:WclTextBox ID="txtRotationName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Type/Specialty">
                        <span class='cptn'>Type/Specialty</span>

                        <infs:WclTextBox ID="txtTypeSpecialty" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered department">
                        <span class="cptn">Department</span>

                        <infs:WclTextBox ID="txtDepartment" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered program">
                        <span class="cptn">Program</span>

                        <infs:WclTextBox ID="txtProgram" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered course">
                        <span class="cptn">Course</span>

                        <infs:WclTextBox ID="txtCourse" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the selected term">
                        <span class="cptn">Term</span>

                        <infs:WclTextBox ID="txtTerm" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered Unit/Floor or Location">
                        <span class="cptn">Unit/Floor or Location</span>

                        <infs:WclTextBox ID="txtUnit" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Students">
                        <span class="cptn"># of Students</span>

                        <infs:WclNumericTextBox ID="txtStudents" runat="server" NumberFormat-DecimalDigits="2"
                            Width="100%" CssClass="form-control">
                        </infs:WclNumericTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Recommended Hours">
                        <span class="cptn"># of Recommended Hours</span>

                        <infs:WclNumericTextBox ID="txtRecommendedHrs" runat="server" NumberFormat-DecimalDigits="2"
                            Width="100%" CssClass="form-control">
                        </infs:WclNumericTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the selected days">
                        <span class="cptn">Days</span>

                        <infs:WclComboBox ID="ddlDays" runat="server" CheckBoxes="true" EmptyMessage="--SELECT--"
                            DataValueField="WeekDayID" DataTextField="Name" Width="100%" CssClass="form-control" 
                            Skin="Silk" AutoSkinMode="false"  Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered shift">
                        <span class="cptn">Shift</span>

                        <infs:WclTextBox ID="txtShift" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>


                </div>

            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered time range">
                        <span class="cptn">Time</span>

                        <infs:WclTimePicker ID="tpStartTime" runat="server" TimeView-Height="300px" Width="100%"
                            CssClass="form-control">
                            <TimeView CssClass="calanderFontSetting" Interval="00:15:00"></TimeView>
                        </infs:WclTimePicker>
                        <div class="gclrPad"></div>
                        <infs:WclTimePicker ID="tpEndTime" runat="server" TimeView-Height="300px" Width="100%"
                            CssClass="form-control">
                            <TimeView CssClass="calanderFontSetting" Interval="00:15:00"></TimeView>
                        </infs:WclTimePicker>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered start date">
                        <span class="cptn">Start Date</span>

                        <infs:WclDatePicker ID="dpStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered end date">
                        <span class="cptn">End Date</span>

                        <infs:WclDatePicker ID="dpEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinEndDate" Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected Instructor/Preceptor">
                        <span class="cptn">Instructor/Preceptor</span>

                        <infs:WclComboBox ID="ddlContacts" runat="server" EmptyMessage="--SELECT--" CheckBoxes="true" 
                            DataValueField="ClientContactID" DataTextField="Name" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false"  Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select &#34All&#34 to view all rotations per the other parameters or &#34Archived&#34 to view only archived rotations or &#34Active&#34 to view only non archived rotations">
                        <span class="cptn">Rotation Archive Status</span>
                        <asp:RadioButtonList ID="rbArchiveStatus" runat="server" RepeatDirection="Horizontal" 
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="radio_list" AutoPostBack="false" Visible="true">
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>

        </asp:Panel>
        <div class="col-md-12">&nbsp;</div>
        <div class="col-md-12">
            <div class="row text-center">
                <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                    AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo"
                    SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch" 
                    CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_ResetClick" OnSaveClick="fsucCmdBarButton_SearchClick"
                    OnCancelClick="fsucCmdBarButton_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                </infsu:CommandBar>
            </div>
            <div class="col-md-12">&nbsp;</div>
        </div>
    </div>
    <div class="row allowscroll">
        <div id="dvGrdRotations" runat="Server">
            <infs:WclGrid runat="server" ID="grdRotations" AllowCustomPaging="true"
                AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
                OnNeedDataSource="grdRotations_NeedDataSource" OnItemCommand="grdRotations_ItemCommand"
                OnSortCommand="grdRotations_SortCommand" OnItemDataBound="grdRotations_ItemDataBound"
                OnItemCreated="grdRotations_ItemCreated"   EnableDefaultFeatures="True"
                NonExportingColumns="ViewDetail,DeleteColumn" 
                ShowClearFiltersButton="false">
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grdRotation_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="RotationID,AgencyID,TenantID"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add New Rotation"
                        ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        
                        <telerik:GridBoundColumn DataField="TenantName" HeaderText="Institution" SortExpression="TenantName"
                            HeaderTooltip="This column displays the Tenant name for each record in the grid"
                            UniqueName="TenantName"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency" SortExpression="AgencyName"
                            HeaderTooltip="This column displays the Agency name for each record in the grid"
                            UniqueName="AgencyName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ComplioID" HeaderText="Complio ID" SortExpression="RotationID"
                            UniqueName="ComplioID" HeaderTooltip="This column displays the Complio ID for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RotationName" HeaderText="Rotation ID/Name" SortExpression="RotationName"
                            UniqueName="RotationName" HeaderTooltip="This column displays the Rotation ID/Name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TypeSpecialty" HeaderText="Type/Specialty" SortExpression="TypeSpecialty"
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
                        <telerik:GridBoundColumn DataField="UnitFloorLoc" HeaderText="Unit/Floor" SortExpression="UnitFloorLoc"
                            UniqueName="unit" HeaderTooltip="This column displays the Unit/Floor for each record in the grid"
                            DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Students" HeaderText="# of Students"
                            SortExpression="Students" HeaderStyle-Width="80px"
                            UniqueName="Students" HeaderTooltip="This column displays the # of Students for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RecommendedHours" HeaderText="# of Recommended Hours"
                            SortExpression="RecommendedHours" HeaderStyle-Width="80px"
                            UniqueName="RecommendedHours" HeaderTooltip="This column displays the # of Recommended Hours for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DaysName" HeaderText="Days" AllowSorting="false"
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
                        <telerik:GridBoundColumn DataField="StartDate" HeaderText="Start Date"
                            AllowSorting="true" SortExpression="StartDate" DataFormatString="{0:MM/dd/yyyy}"
                            UniqueName="StartDate" HeaderTooltip="This column displays the Start Date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EndDate" HeaderText="End Date"
                            AllowSorting="true" SortExpression="EndDate" DataFormatString="{0:MM/dd/yyyy}"
                            UniqueName="EndDate" HeaderTooltip="This column displays the End Date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ContactNames" HeaderText="Instructor/Preceptor"
                            AllowSorting="false" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Justify"
                            UniqueName="ContactName" HeaderTooltip="This column displays the Instructor/Preceptor for each record in the grid">
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                                    ToolTip="Click here to view details of rotation." runat="server" Text="Detail">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--  <telerik:GridButtonColumn ButtonType="LinkButton" HeaderTooltip="Click here to view details of rotation."
                            CommandName="ViewDetail" Text="Detail" 
                            ItemStyle-Width="100px" UniqueName="ViewDetail">
                        </telerik:GridButtonColumn>--%>
                        <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif"
                            CommandName="Delete" ConfirmText="Are you sure you want to delete this rotation?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>

                    </Columns>


                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                        Position="TopAndBottom" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>

    <asp:HiddenField ID="hdnFileRemoved" runat="server" />
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
    <asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
    <asp:HiddenField ID="hdnTenantIdNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
    <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnCurrentRotStartDate" runat="server" Value="" />
</div>
