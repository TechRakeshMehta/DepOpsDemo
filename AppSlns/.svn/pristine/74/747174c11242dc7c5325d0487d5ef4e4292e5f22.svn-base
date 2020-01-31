<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RotationMemberSearch.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.RotationMemberSearch" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchy" Src="~/AgencyHierarchy/UserControls/AgencyHierarchySelection.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/RotationMemberSearch.js" ResourceType="JavaScript" />
     <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Rotation Member Search
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                DataValueField="TenantID" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                                OnDataBound="ddlTenant_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                    InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='form-group col-md-3'>&nbsp;</div>
                    <div class='form-group col-md-3' title="Select an Agency whose data you want to view.">
                        <%-- <span class="cptn">Agency</span>
                        <infs:WclComboBox ID="ddlAgency" runat="server" DataTextField="AgencyName" DataValueField="AgencyID"
                            AutoPostBack="false" OnDataBound="ddlAgency_DataBound" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>--%>
                        <uc:AgencyHierarchy ID="ucAgencyHierarchy" runat="server" />
                    </div>
                    <div class='form-group col-md-3' title="Select a User Group.">
                        <span class="cptn">User Group</span>
                        <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            AutoPostBack="false" OnDataBound="ddlUserGroup_DataBound" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
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
            <div class="col-md-12">
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
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered No of Students">
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
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered shift">
                        <span class="cptn">Shift</span>
                        <infs:WclTextBox ID="txtShift" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered time range">
                        <span class="cptn">Time</span>
                        <infs:WclTimePicker ID="tpStartTime" runat="server" TimeView-Height="300px" Width="100%"
                            CssClass="form-control">
                            <timeview cssclass="calanderFontSetting" interval="00:15:00"></timeview>
                        </infs:WclTimePicker>
                        <div class="gclrPad"></div>
                        <infs:WclTimePicker ID="tpEndTime" runat="server" TimeView-Height="300px" Width="100%"
                            CssClass="form-control">
                            <timeview cssclass="calanderFontSetting" interval="00:15:00"></timeview>
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
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select &#34All&#34 to view all rotations per the other parameters or &#34Archived&#34 to view only archived rotations or &#34Active&#34 to view only non archived rotations">
                        <span class="cptn">Rotation Archive Status</span>
                        <asp:RadioButtonList ID="rbArchiveStatus" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbArchiveStatus_SelectedIndexChanged"
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="radio_list" AutoPostBack="true">
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered applicant first name">
                        <span class="cptn">First Name</span>
                        <infs:WclTextBox ID="txtAppFirstName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered applicant last name">
                        <span class="cptn">Last Name</span>
                        <infs:WclTextBox ID="txtAppLastName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected user type">
                        <span class="cptn">User Type</span>
                        <infs:WclComboBox ID="ddlUserType" runat="server" CheckBoxes="true" EmptyMessage="--SELECT--"
                            AutoPostBack="false"  Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" DataTextField="Value" DataValueField="Key">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="col-md-12">&nbsp;</div>
    <div class="row">
        <div class="col-md-12 text-center">
            <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo"
                SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_ResetClick" OnSaveClick="fsucCmdBarButton_SearchClick"
                OnCancelClick="fsucCmdBarButton_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
        </div>
    </div>
    <div class="row allowscroll" id="dvGrdRotationMemebers" runat="Server">
        <infs:WclGrid runat="server" ID="grdRotationMembers" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            OnNeedDataSource="grdRotationMembers_NeedDataSource" OnItemCommand="grdRotationMembers_ItemCommand"
            OnSortCommand="grdRotationMembers_SortCommand" OnItemDataBound="grdRotationMembers_ItemDataBound"
            NonExportingColumns="ViewDetail" EnableLinqExpressions="false" ShowClearFiltersButton="false">
            <clientsettings enablerowhoverstyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick_Rotation" />
                <Selecting AllowRowSelect="true"></Selecting>
            </clientsettings>
            <exportsettings pdf-pagewidth="450mm" pdf-pageheight="230mm" pdf-pageleftmargin="20mm"
                pdf-pagerightmargin="20mm" openinnewwindow="true" hidestructurecolumns="false"
                exportonlydata="true" ignorepaging="true">
            </exportsettings>
            <mastertableview commanditemdisplay="Top" datakeynames="RotationID,OrganizationUserId,IsPackageExistsInRotation,IsApplicant"
                allowfilteringbycolumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="ExportCheckBox" AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectDocument" runat="server" onclick="UnCheckHeader(this)"
                                OnCheckedChanged="chkSelectDocument_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="FirstName" HeaderText="First Name" SortExpression="FirstName"
                        HeaderTooltip="This column displays the first name for each record in the grid"
                        UniqueName="FirstName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LastName" HeaderText="Last Name" SortExpression="LastName"
                        HeaderTooltip="This column displays the last name for each record in the grid"
                        UniqueName="LastName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UserGroup" HeaderText="User Group" SortExpression="UserGroup"
                        HeaderTooltip="This column displays the user group for each record in the grid"
                        UniqueName="UserGroup">
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="UserType" HeaderText="User Type" SortExpression="UserType"
                        HeaderTooltip="This column displays the user type for each record in the grid"
                        UniqueName="UserType">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency" SortExpression="AgencyName"
                        HeaderTooltip="This column displays the Agency name for each record in the grid"
                        UniqueName="AgencyName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ComplioID" HeaderText="Complio ID" SortExpression="ComplioID"
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
                        SortExpression="Students" UniqueName="Students" HeaderTooltip="This column displays the # of Recommended Hours for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RecommendedHours" HeaderText="# of Recommended Hours"
                        SortExpression="RecommendedHours"
                        UniqueName="RecommendedHours" HeaderTooltip="This column displays the # of Recommended Hours for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DaysName" HeaderText="Days" AllowSorting="false"
                        UniqueName="Days" HeaderTooltip="This column displays any Days Name to which the applicant belongs for each record in the grid">
                        <ItemStyle Wrap="true" Width="10px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Shift" HeaderText="Shift" SortExpression="Shift"
                        UniqueName="Shift" HeaderTooltip="This column displays shift for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Time" HeaderText="Time" AllowSorting="false"
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
                        AllowSorting="false"
                        UniqueName="ContactName" HeaderTooltip="This column displays the Instructor/Preceptor for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AgencyComplianceStatus" HeaderText="Agency Compliance Status"
                        AllowSorting="true" SortExpression="AgencyComplianceStatus"
                        UniqueName="AgencyComplianceStatus" HeaderTooltip="This column displays the Rotation Compliance Status for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnViewDetail" ButtonType="LinkButton" CommandName="ViewDetail"
                                ToolTip="Click here to view details of rotation."
                                runat="server" Text="Detail">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%-- <telerik:GridBoundColumn DataField="IsApplicant" HeaderText="Is Applicant" AllowSorting="false"
                            UniqueName="IsApplicant" HeaderTooltip="This column displays the IsApplicant for each record in the grid">
                        </telerik:GridBoundColumn>--%>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                    Position="TopAndBottom" />
            </mastertableview>
            <pagerstyle pagesizecontroltype="RadComboBox"></pagerstyle>
            <filtermenu enableimagesprites="False">
            </filtermenu>
        </infs:WclGrid>
    </div>
    <div class="col-md-12">&nbsp;</div>
    <div class="col-md-12">
        <infsu:CommandBar ID="fsucCmdExport" runat="server" ButtonPosition="Center" DisplayButtons="Extra"
            AutoPostbackButtons="Extra" ExtraButtonText="Export Document(s)" OnExtraClick="btnExport_Click"
            UseAutoSkinMode="false" ButtonSkin="Silk">
        </infsu:CommandBar>
    </div>
    <div class="col-md-12">&nbsp;</div>
    <iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>
</div>

<script type="text/javascript">
    function CheckAll(id) {
        var masterTable = $find("<%= grdRotationMembers.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked = isChecked; // for checking the checkboxes
            }
        }
    }
    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdRotationMembers.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    function grd_rwDbClick_Rotation(s, e) {
        var _idViewDetail = "btnViewDetail";
        var btnViewDetail = e.get_gridDataItem().findControl(_idViewDetail);
        if (btnViewDetail && typeof (btnViewDetail.click) != "undefined") {
            btnViewDetail.click();
        }
    }



</script>
