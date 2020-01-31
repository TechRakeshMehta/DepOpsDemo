<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyUserPlacementDashboard.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.AgencyUserPlacementDashboard" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxPlacementDashboard">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/ProfileSharing/RotationWidget.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>



<style type="text/css">
    .hdrtext {
        font-size: x-large;
        margin: 0px 0px 5px 30px;
    }

    .dvcss {
        text-align: left;
        margin-right: -3px;
        width: 100%;
        height: 14.7%;
    }

    .statusText {
        font-size: medium;
        margin: 8px 0px 0px 20px;
        float: left;
        width: 75%;
    }

    .StatusCounttext {
        width: 20%;
        margin: 8px 0px 0px -35px;
        font-size: 30px;
        font-weight: 600;
    }



    .allBlue {
        background-image: url(../Resources/Mod/ProfileSharing/images/Blue.png) !important;
        background-repeat: no-repeat;
        background-size: 100% 100%;
    }

    .BlueOrange {
        background-image: url(../Resources/Mod/ProfileSharing/images/BlueOrange.png) !important;
        background-repeat: no-repeat;
        background-size: 100% 100%;
    }

    .BlueGreen {
        background-image: url(../Resources/Mod/ProfileSharing/images/BlueGreen.png) !important;
        background-repeat: no-repeat;
        background-size: 100% 100%;
    }

    .pieChart {
        margin: 0 auto;
    }

    .Droppedyellow {
        color: #ecd424;
    }


    .UserGuide {
        display: inline-block;
        text-align: center;
        position: absolute;
        margin-left: 2.9%;
        /*color: #000 !important*/
    }

    .stylink {
        font-size: 13px;
        color: blue;
    }

        .stylink:hover {
            color: #8C1921;
        }

    .rsDateBox {
        background-color: transparent !important;
        background-image: none !important;
        border-bottom: none !important;
    }

    .rsAptDelete {
        display: none;
    }

    .rsMonthView .rsTodayCell {
        background-color: #CCFF00;
        color: #000;
    }

    .rsContentScrollArea {
        overflow-x: auto;
    }

    .grayBg {
        background-color: lightgray;
    }

    .rsContent.rsMonthView > table {
        float: left;
    }

    .rsHeader > h2 {
        margin: 0 !important;
    }

    .rsHeader > p {
        margin: 0 !important;
    }

    .header {
        font-weight: bold;
        font-size: 22px;
        font-family: Calibri;
    }

    .subheader {
        font-weight: bold;
        font-size: 20px;
        font-family: Calibri;
    }

    .bodytext {
        font-size: 14px;
        color: #C0C0C0;
        text-wrap: normal;
    }

    .h1Rtn {
        text-align: left;
        margin: 0px !important;
    }

    .rtnDate {
        font-weight: normal;
        text-align: left;
        margin: 0px !important;
    }

    .pRtnInfo {
        font-size: 11px;
        margin: 2px 0 !important;
        text-align: left;
    }

    .spnRtnInfo {
        font-size: 11px;
        margin: 2px 0 !important;
        font-weight: bold;
        text-align: left;
    }

    .pRtnAgency {
        text-align: left;
        font-size: 14px;
        margin: 0px !important;
    }

    .spnRtnAgency {
        text-align: left;
        font-size: 14px;
        margin: 0px !important;
        font-weight: bold;
    }

    .pRtnStatus {
        margin-top: 0px !important;
        margin-right: 0px !important;
        margin-bottom: 10px;
        margin-left: 0px !important;
        text-align: left;
        font-size: 14px;
    }

    .spnRtnStatus {
        text-align: left;
        font-size: 14px;
        font-style: italic;
    }

    .divRtnInfo {
        width: 100%;
        float: left;
        margin: 10px 0;
    }

    .dataPager {
        float: left;
        background-color: transparent;
        border-style: solid none none;
        border-width: 2px 0px 0px;
        margin-top: 5px;
    }

    .dataPager2 {
        float: left;
        background-color: transparent;
        border-style: solid none none;
        border-width: 0px 0px 0px;
    }

        .dataPager2 .rdpWrap {
            padding: 0px !important;
        }

    .dvdesign {
        border-radius: 2px;
    }

    .RadDataPager.dataPager2 {
        padding: 0px !important;
    }

    html .RadButton_Silk.rbSkinnedButton {
        background-color: #8C1921 !important;
        background-image: none !important;
        border: medium none !important;
        border-radius: 7px !important;
        color: #fff !important;
        height: 30px !important;
        line-height: 30px !important;
        padding: 0 12px !important;
    }

    .downloadRight {
        position: relative;
    }

    .btnGoCptn::after {
        content: "" !important;
    }

    .padding0 {
        padding: 0 !important;
    }

    .hipaacompliant {
        background-image: url("/Resources/Mod/Shared/images/login/hipaa_compliant_logo.png");
        background-repeat: no-repeat;
        float: right;
        height: 55px;
        text-align: right;
        width: 101px;
    }

    .pci_logo {
        background-image: url("/Resources/Mod/Shared/images/login/pci_logo.png");
        background-repeat: no-repeat;
        float: right;
        height: 55px;
        width: 67px;
    }

    .rdpPageNext {
        width: 11px !important;
    }

    .clsPci_logo {
        padding-top: 10px;
    }

    a.list-group-item, button.list-group-item {
        color: #555;
    }

    .list-group-item {
        position: relative;
        display: block;
        padding: 10px 15px;
        margin-bottom: -1px;
        background-color: #fff;
        border: 1px solid #ddd;
    }
</style>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Placement Dashboard
            </h2>
        </div>
    </div>

    <div class="row">&nbsp;&nbsp;</div>

    <div>
        <div class="row">
            <div class='col-md-12'>
                <div id="dvRequestStatus" class="col-md-3" style="height: 300px">
                    <%--<h2 style="font-size: x-large; margin: 0px 0px 5px 30px;">Request Status</h2>--%>

                    <div class="panel panel-default">
                        <div class="panel-heading" style="background-color: #8C1921; color: #fff">
                            <h2 class="text-center">Request Status</h2>
                        </div>
                        <!-- /.panel-heading -->
                        <div class="panel-body">
                            <div class="list-group">
                                <a href="javascript:void(0)" id="ancPendingReview" runat="server" class="list-group-item" style="background-color: #F9E79F">
                                    <i class="fa fa-tasks fa-fw" style="font-style: normal"></i>PENDING REVIEW
                                    <span class="pull-right text-muted large bold"><em id="emPendingReview" runat="server"></em>
                                    </span>
                                </a>
                                <a href="javascript:void(0)" id="ancModified" runat="server" class="list-group-item" style="background-color: #b1e1f9">
                                    <i class="fa fa-pencil-square fa-fw" style="font-style: normal"></i>MODIFIED
                                    <span class="pull-right text-muted large bold"><em id="emModified" runat="server"></em>
                                    </span>
                                </a>
                                <a href="javascript:void(0)" id="ancApproved" runat="server" class="list-group-item" style="background-color: #ceffba">
                                    <i class="fa fa-check fa-fw" style="font-style: normal"></i>APPROVED
                                    <span class="pull-right text-muted large bold"><em id="emApproved" runat="server"></em>
                                    </span>
                                </a>
                                <a href="javascript:void(0)" id="ancRejected" runat="server" class="list-group-item" style="background-color: #ffc27e">
                                    <i class="fa fa-times fa-fw" style="font-style: normal"></i>REJECTED
                                    <span class="pull-right text-muted large bold"><em id="emRejected" runat="server"></em>
                                    </span>
                                </a>
                                <a href="javascript:void(0)" id="ancArchived" runat="server" class="list-group-item" style="background-color: #bababa"><%--d6e9c6   #FA8603--%>
                                    <i class="fa fa-archive fa-fw" style="font-style: normal"></i>ARCHIVED
                                    <span class="pull-right text-muted large bold"><em id="emArchived" runat="server"></em>
                                    </span>
                                </a>
                                <a href="javascript:void(0)" id="ancCancelled" runat="server" class="list-group-item" style="background-color: #ebccd1"><%--#FFA07A--%>
                                    <i class="fa fa-ban fa-fw" style="font-style: normal"></i>CANCELLED
                                    <span class="pull-right text-muted large bold"><em id="emCancelled" runat="server"></em>
                                    </span>
                                </a>
                                <a href="javascript:void(0)" id="ancConflicts" runat="server" class="list-group-item" style="background-color: #fffbac"><%--#EB5863--%>
                                    <i class="fa fa-exclamation fa-fw" style="font-style: normal"></i>CONFLICTS
                                    <span class="pull-right text-muted large bold"><em id="emConflicts" runat="server"></em>
                                    </span>
                                </a>

                            </div>
                            <!-- /.list-group -->
                            <%--   <a href="#" class="btn btn-default btn-block">View All Alerts</a>--%>
                        </div>
                        <!-- /.panel-body -->
                    </div>

                </div>
                <div class="col-md-9" id="dvCalender">
                    <div class="panel panel-default bgCalander">
                        <div class="panel-body" style="padding-left: 0px; padding-top: 0px; padding-bottom: 0px;">
                            <div id="rotationWidget">
                                <asp:UpdatePanel ID="upl" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <div id="divRadScheduler" class="col-md-7">
                                            <div class="row">
                                                <div class="demo-container no-bg">
                                                    <infs:WclCalendar ID="clndrPlacement" ShowChooser="true" runat="server" ShowRowHeaders="false" AutoPostBack="true" EnableNavigation="true"
                                                        EnableMultiSelect="false" EnableMonthYearFastNavigation="true" OnDayRender="clndrPlacement_DayRender" UseColumnHeadersAsSelectors="false" Height="300px" Width="100%" ShowOtherMonthsDays="true">
                                                        <FastNavigationSettings EnableTodayButtonSelection="true"></FastNavigationSettings>
                                                    </infs:WclCalendar>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div id="dvIntitutionProfile">
                                                <h2 class="header-color">Placement Allocation</h2>
                                                <div class="gridBorder" style="float: left;">
                                                    <telerik:RadHtmlChart Style="float: left; margin-top: 10px" runat="server" CssClass="pieChart" ID="RdPieChartInstitutionProfile" Transitions="true" Height="300px" Width="430px" EnableViewState="true">
                                                        <Legend>
                                                            <Appearance Visible="true" Position="Bottom"></Appearance>
                                                        </Legend>
                                                        <PlotArea>
                                                            <Series>
                                                                <telerik:PieSeries StartAngle="0" DataFieldY="RecordsPercentage" NameField="TenantName" ColorField="pieColor" ExplodeField="IsExploded">
                                                                    <LabelsAppearance Position="Circle" DataFormatString="{0}%">
                                                                    </LabelsAppearance>
                                                                    <TooltipsAppearance Visible="true" ClientTemplate="#=dataItem.TenantName# (#=dataItem.RecordsPercentage#%)" />
                                                                    <%--<Items>
                                                                        <telerik:SeriesItem BackgroundColor="#58D68D" Exploded="true" Name="Jefferson University" YValue="10" />
                                                                        <telerik:SeriesItem BackgroundColor="#AED6F1" Exploded="true" Name="Memorial Health System" YValue="55" />
                                                                        <telerik:SeriesItem BackgroundColor="#DAF7A6" Exploded="true" Name="CCSD" YValue="35" />
                                                                    </Items>--%>
                                                                </telerik:PieSeries>
                                                            </Series>
                                                        </PlotArea>
                                                    </telerik:RadHtmlChart>
                                                    <div class="col-md-12" style="padding-left: 5px; padding-right: 0px; padding-top: 5px;">
                                                        <infs:WclComboBox ID="ddlPieChartFilters" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPieChartFilters_SelectedIndexChanged"
                                                            Filter="None" OnClientKeyPressing="openCmbBoxOnTab" Width="50%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                        </infs:WclComboBox>
                                                    </div>
                                                    <div id="dvRangeFilters" runat="server">
                                                        <div class="col-md-12" style="padding-left: 5px; padding-right: 0px; padding-top: 5px;">
                                                            <div class="row">
                                                                <div class='form-group col-md-5' title="">
                                                                    <span class="cptn">Date From</span>
                                                                    <infs:WclDatePicker ID="dpFromDate" runat="server" DateInput-EmptyMessage="Select a date"
                                                                        ClientEvents-OnDateSelected="VerifyFromDateWithToFilterDate" Width="100%" CssClass="form-control">
                                                                    </infs:WclDatePicker>
                                                                </div>
                                                                <div class='form-group col-md-5' title="">
                                                                    <span class="cptn">Date To</span>
                                                                    <infs:WclDatePicker ID="dpToDate" runat="server" DateInput-EmptyMessage="Select a date"
                                                                        ClientEvents-OnPopupOpening="SetMinFromDateForPieFilter" Width="100%" CssClass="form-control">
                                                                    </infs:WclDatePicker>
                                                                </div>
                                                                <div class='form-group col-md-2' title="" style="padding-left: 0px; padding-top: 20px;">
                                                                    <span class="cptn btnGoCptn">&nbsp;</span>
                                                                    <infs:WclButton ID="btnGo_RangeFilter" runat="server" AutoPostBack="true" Text="Go"
                                                                        OnClick="btnGo_RangeFilter_Click" ToolTip="Clicks to filter to institutions by profiles by date range."
                                                                        AutoSkinMode="false" Skin="Silk" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12" style="padding-left: 5px; padding-right: 0px; padding-top: 5px;">
                                                        <div class="row">
                                                            <div class='form-group col-md-12' title="">
                                                                <span class="rgWrap rgInfoPart" style="color: #656565; font-size: 12px;" runat="server" id="spnPieInformation"></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="dvGrid" class="row">
        <infs:WclGrid Width="100%" CssClass="gridhover" runat="server" ID="grdPlacementRequest"
            AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GridCode="AAAB"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" Visible="true"
            EnableLinqExpressions="false" ShowClearFiltersButton="false" OnNeedDataSource="grdPlacementRequest_NeedDataSource" OnItemCommand="grdPlacementRequest_ItemCommand" OnItemDataBound="grdPlacementRequest_ItemDataBound">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick_PlacementRequest" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="RequestID,RequestStatus,StatusCode,OpportunityID"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false" ShowRefreshButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <%-- <AlternatingItemStyle BackColor="#f2f2f2" />
                <ItemStyle BackColor="#ffffff" />--%>
                <Columns>
                    <telerik:GridBoundColumn DataField="OpportunityID" HeaderText="PLID" AllowSorting="false" SortExpression="OpportunityID"
                        HeaderTooltip="This column displays the placement request id for each record in the grid"
                        UniqueName="OpportunityID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency" AllowSorting="false" SortExpression="Agency"
                        HeaderTooltip="This column displays the agency for each record in the grid"
                        UniqueName="AgencyName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Location" HeaderText="Location" AllowSorting="false" SortExpression="Location"
                        HeaderTooltip="This column displays the location for each record in the grid"
                        UniqueName="Location">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Department" HeaderText="Department" AllowSorting="false"
                        SortExpression="Department" HeaderTooltip="This column displays the department for each record in the grid" UniqueName="Department">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Specialty" HeaderText="Specialty" SortExpression="Specialty"
                        HeaderTooltip="This column displays the Specialty for each record in the grid"
                        UniqueName="Specialty">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StudentType" HeaderText="Student Type" SortExpression="StudentType"
                        HeaderTooltip="This column displays the student Type for each record in the grid"
                        UniqueName="StudentType">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="MaxNoOfStudents" HeaderText="Max #" SortExpression="MaxNoOfStudents"
                        UniqueName="MaxNoOfStudents" HeaderTooltip="This column displays the maximum number of students for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Course" HeaderText="Max #" SortExpression="Course"
                        UniqueName="Course" HeaderTooltip="This column displays the course for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StartDate" HeaderText="Start Date"
                        AllowSorting="true" SortExpression="StartDate" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="StartDate" HeaderTooltip="This column displays the start Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EndDate" HeaderText="End Date"
                        AllowSorting="true" SortExpression="EndDate" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="EndDate" HeaderTooltip="This column displays the end Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LastUpdateDate" HeaderText="Last Updated" SortExpression="LastUpdateDate" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="LastUpdateDate" HeaderTooltip="This column displays the last updated date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LastUpdateByName " HeaderText="Updated By" AllowSorting="false" SortExpression="LastUpdateByName"
                        HeaderTooltip="This column displays the updated by for each record in the grid"
                        UniqueName="LastUpdateByName">
                    </telerik:GridBoundColumn>

                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Status" ItemStyle-Width="200px" HeaderText="Status" SortExpression="Status">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnDetails" ButtonType="LinkButton" CommandName="ViewDetails"
                                ToolTip="Click here to view details of request."
                                runat="server" Text='<%# Eval("RequestStatus")%>'>
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                    Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="false">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>
<asp:HiddenField ID="hdnSelectedStatusCode" runat="server" />
<asp:Button CssClass="buttonHidden" ID="btnDoPostBack" runat="server" OnClick="btnDoPostBack_Click" />
<asp:HiddenField ID="hdnRequestId" runat="server" />
<asp:HiddenField ID="hdnPageRequested" runat="server" />
<asp:HiddenField ID="hdnRequestStatusCode" runat="server" />
<asp:HiddenField ID="hdnOpportunityID" runat="server" />

<asp:HiddenField ID="hdnRequestSavedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestCancelledSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestRejectedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestArchivedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestApprovedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestPublishedSuccessfully" runat="server" />


<script type="text/javascript">
    function pageLoad() {

        $jQuery('#divRadScheduler .rcPrev').removeClass().empty().addClass('fa fa-arrow-circle-left arrow-color font22');
        $jQuery('#divRadScheduler .rcNext').removeClass().empty().addClass('fa fa-arrow-circle-right arrow-color font22');
    }

    function statusClick(status) {
        debugger;
        $jQuery("[id$=hdnSelectedStatusCode]").val(status);
        __doPostBack("<%= btnDoPostBack.ClientID %>", "");

    }

    function grd_rwDbClick_PlacementRequest(s, e) {
        var _id = "btnDetails";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function openCmbBoxOnTab(sender, e) {
        if (!sender.get_dropDownVisible()) sender.showDropDown();
    }

    function VerifyFromDateWithToFilterDate(picker) {

        var datefrom = $jQuery("[id$=dpFromDate]")[0].control.get_selectedDate();
        var dateTo = $jQuery("[id$=dpToDate]")[0].control.get_selectedDate();
        if (datefrom != null && dateTo != null) {
            if (datefrom > dateTo)
                $jQuery("[id$=dpToDate]")[0].control.set_selectedDate(null);
        }
    }

    function SetMinFromDateForPieFilter(picker) {
        var date = $jQuery("[id$=dpFromDate]")[0].control.get_selectedDate();
        if (date != null) {
            picker.set_minDate(date);
        }
        else {
            picker.set_minDate(minDate);
        }
    }

    function RequestDetailsPop() {
        //debugger;
        var composeScreenWindowName = "Request Details";
        //var screenName = "CommonScreen";
        var requestId = $jQuery("[id$=hdnRequestId]").val();
        var RequestedPage = $jQuery("[id$=hdnPageRequested]").val();
        var RequestStatusCode = $jQuery("[id$=hdnRequestStatusCode]").val();
        var OpportunityID = $jQuery("[id$=hdnOpportunityID]").val();
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/PlacementMatching/Pages/CreateRequestPopup.aspx?RequestId=" + requestId + "&RequestedPage=" + RequestedPage + "&RequestStatusCode=" + RequestStatusCode + "&OpportunityId=" + OpportunityID);
        var win = $window.createPopup(url, { size: "1000," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function OnClientClose(oWnd, args) {
        // debugger;
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                //do button postback to show success message.
                $jQuery("[id$=hdnRequestSavedSuccessfully]").val(arg.RequestSavedSuccessfully);
                $jQuery("[id$=hdnRequestCancelledSuccessfully]").val(arg.RequestCancelledSuccessfully);
                $jQuery("[id$=hdnRequestRejectedSuccessfully]").val(arg.RequestRejectedSuccessfully);
                $jQuery("[id$=hdnRequestArchivedSuccessfully]").val(arg.RequestArchivedSuccessfully);
                $jQuery("[id$=hdnRequestApprovedSuccessfully]").val(arg.RequestApprovedSuccessfully);
                $jQuery("[id$=hdnRequestPublishedSuccessfully]").val(arg.RequestPublishedSuccessfully);

                $jQuery("#<%=btnDoPostBack.ClientID %>").trigger('click');
            }
            winopen = false;
        }

    }

</script>
