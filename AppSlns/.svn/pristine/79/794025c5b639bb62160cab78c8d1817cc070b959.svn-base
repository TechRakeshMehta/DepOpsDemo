<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageRequest.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.ManageRequest" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>
<%@ Register Src="~/ClinicalRotation/UserControl/SharedUserCustomAttributeForm.ascx" TagPrefix="ucCustom" TagName="CustomAttributes" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxManageRequest">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    /*.buttonHidden {
        display: none;
    }*/

    .lnkBtn {
        background-color: transparent !important;
        padding-top: 0px !important;
    }

        .lnkBtn.rbLinkButton:hover {
            background-color: transparent !important;
        }

    .section, .tabvw {
        padding-bottom: 25px;
    }

    .top3 {
        top: 3px !important;
    }

    .rmVertical.rmGroup.rmLevel1 {
        border: none;
    }

    .btn {
        width: 100%;
        text-align: left;
    }

    .RadMenu .rmGroup .rmText {
        padding: 0px;
        margin: 0px;
    }
</style>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Request
            </h2>
        </div>
    </div>
    <div class="row">&nbsp;&nbsp;</div>

    <asp:Panel ID="pnlSearch" runat="server">
        <div class="row bgLightGreen">
        </div>
        <div class='col-md-12'>
            <div class="col-md-12">&nbsp;</div>
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view.">
                            <span class="cptn">Institution</span>

                            <%--   <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" EmptyMessage="--Select--"
                                AllowCustomText="true" CausesValidation="false" DataValueField="TenantID" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                                <Localization CheckAllString="All" />
                            </infs:WclComboBox>--%>
                            <asp:RadioButtonList ID="rblInstitutionAvailability" runat="server" RepeatDirection="Horizontal" DataTextField="IAT_Name" DataValueField="IAT_Code">
                                <%--      OnSelectedIndexChanged="rblInstitutionAvailability_SelectedIndexChanged" AutoPostBack="true">--%>
                            </asp:RadioButtonList>
                        </div>
                        <%--need to clarify if it should be radiobutton list as opportunities will be for associated institution or all institutions.--%>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="row">
                    <div class="form-group col-md-3">
                    </div>
                    <div class="section" id="sectionPanel" runat="server">
                        <h1 class="mhdr" id="hdrAdvancedSearch" runat="server" onclick="AdvancedSearchPanelClick()">Advanced Search
                        </h1>
                        <div class="content" id="contentPanel" runat="server">
                            <div class='col-md-12'>
                                <div class="row">
                                    <%-- <div class='form-group col-md-3' title="Select a Agency whose data you want to view.">
                                        <uc:AgencyHierarchyMultiple ID="ucAgencyHierarchyMultiple" runat="server" />
                                    </div--%>
                                    <div class='form-group col-md-3' title="Restrict search results to the entered Location">
                                        <span class="cptn">Location</span>
                                        <infs:WclComboBox ID="ddlLocation" runat="server" DataTextField="Location" EmptyMessage="--Select--"
                                            DataValueField="AgencyLocationID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" AutoPostBack="false">
                                        </infs:WclComboBox>
                                    </div>
                                    <div class='form-group col-md-3' title="Restrict search results to the entered department">
                                        <span class="cptn">Department</span>
                                        <infs:WclComboBox ID="ddlDepartment" runat="server" DataTextField="Name" EmptyMessage="--Select--"
                                            DataValueField="DepartmentID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" AutoPostBack="false">
                                        </infs:WclComboBox>
                                    </div>
                                    <div class="form-group col-md-3" title="Restrict search results to the entered start date">
                                        <span class="cptn">Start Date</span>
                                        <infs:WclDatePicker ID="dpStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                                            Width="100%" CssClass="form-control" ClientEvents-OnDateSelected="CorrectStartToEndDate" ClientEvents-OnPopupOpening="SetMinDate">
                                        </infs:WclDatePicker>

                                    </div>
                                    <div class="form-group col-md-3" title="Restrict search results to the entered end date">
                                        <span class="cptn">End Date</span>
                                        <infs:WclDatePicker ID="dpEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                                            Width="100%" CssClass="form-control" ClientEvents-OnPopupOpening="SetMinEndDate">
                                        </infs:WclDatePicker>
                                    </div>
                                </div>
                            </div>
                            <div class='col-md-12'>
                                <div class="row">
                                    <div class='form-group col-md-3' title="Restrict search results to the entered Specialty">
                                        <span class='cptn'>Specialty</span>
                                        <infs:WclComboBox ID="ddlSpecialty" runat="server" DataTextField="Name" EmptyMessage="--Select--"
                                            AllowCustomText="true" CausesValidation="false" DataValueField="SpecialtyID"
                                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                                        </infs:WclComboBox>
                                    </div>
                                    <div class='form-group col-md-3' title="Restrict search results to the selected student types">
                                        <span class='cptn'>Students Type</span>
                                        <infs:WclComboBox ID="ddlStudentType" runat="server" DataTextField="Name" EmptyMessage="--Select--"
                                            AllowCustomText="true" CausesValidation="false" DataValueField="StudentTypeId" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                                            <Localization CheckAllString="All" />
                                        </infs:WclComboBox>
                                    </div>
                                    <div class='form-group col-md-3' title="Restrict search results to the entered maximum number of students">
                                        <span class='cptn'>Max #</span>
                                        <infs:WclTextBox ID="txtMaxNoOfStudent" runat="server" Width="100%" CssClass="form-control">
                                        </infs:WclTextBox>
                                    </div>

                                </div>
                            </div>
                            <div class='col-md-12'>
                                <div class="row">
                                    <div class='form-group col-md-3' title="Restrict search results to the selected days">
                                        <span class="cptn">Days</span>

                                        <infs:WclComboBox ID="ddlDays" runat="server" CheckBoxes="true" EmptyMessage="--Select--"
                                            DataValueField="WeekDayID" DataTextField="Name" Width="100%" CssClass="form-control"
                                            Skin="Silk" AutoSkinMode="false">
                                        </infs:WclComboBox>
                                    </div>
                                    <div class='form-group col-md-3' title="Restrict search results to the entered shift">
                                        <span class="cptn">Shift</span>

                                        <infs:WclTextBox ID="txtShift" runat="server" Width="100%" CssClass="form-control">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='form-group col-md-3' title="Restrict search results to the selected status type.">
                                        <span class='cptn'>Status</span>
                                        <infs:WclComboBox ID="ddlStatus" runat="server" DataTextField="Name" EmptyMessage="--Select--"
                                            AllowCustomText="true" CausesValidation="false" DataValueField="StatusID"
                                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                                        </infs:WclComboBox>
                                    </div>
                                </div>
                            </div>

                            <div class='col-md-12'>
                                <ucCustom:CustomAttributes ID="caCustomAttributesID" EnableViewState="false" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="col-md-12">
                <div class="row ">
                    <div style="width: 60%; float: left;">
                        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Right" DisplayButtons="Submit,Save,Cancel"
                            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo"
                            SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                            CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_ResetClick" OnSaveClick="fsucCmdBarButton_SearchClick"
                            OnCancelClick="fsucCmdBarButton_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                            <ExtraCommandButtons>
                            </ExtraCommandButtons>
                        </infsu:CommandBar>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <div class="row">
        <infs:WclGrid Width="100%" CssClass="gridhover" runat="server" ID="grdRequest"
            AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GridCode="AAAB"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" Visible="true"
            EnableLinqExpressions="false" ShowClearFiltersButton="false" OnNeedDataSource="grdRequest_NeedDataSource" OnItemCommand="grdRequest_ItemCommand" OnItemDataBound="grdRequest_ItemDataBound">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick_Request" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OpportunityID,RequestID,RequestStatus,StatusCode"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <%-- <AlternatingItemStyle BackColor="#f2f2f2" />
                <ItemStyle BackColor="#ffffff" />--%>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="AssignCheckBox" AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectOpportunity" runat="server" onclick="UnCheckHeader(this)" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="RequestID" HeaderText="Request ID" AllowSorting="false" SortExpression="RequestID"
                        HeaderTooltip="This column displays the Request Id for each record in the grid"
                        UniqueName="RequestID" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstitutionID" HeaderText="Institution ID" AllowSorting="false" SortExpression="InstitutionID"
                        HeaderTooltip="This column displays the institution Id for each record in the grid"
                        UniqueName="InstitutionID" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstitutionName" HeaderText="Institution" AllowSorting="false" SortExpression="InstitutionName"
                        HeaderTooltip="This column displays the Request Id for each record in the grid"
                        UniqueName="InstitutionName">
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
                    <telerik:GridBoundColumn DataField="StudentTypes" HeaderText="Student Type" SortExpression="StudentTypes"
                        HeaderTooltip="This column displays the student Type for each record in the grid"
                        UniqueName="StudentTypes">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Max" HeaderText="Max #" SortExpression="Max"
                        UniqueName="Max" HeaderTooltip="This column displays the maximum number of students for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StartDate" HeaderText="Start Date"
                        AllowSorting="true" SortExpression="StartDate" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="StartDate" HeaderTooltip="This column displays the start Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EndDate" HeaderText="End Date"
                        AllowSorting="true" SortExpression="EndDate" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="EndDate" HeaderTooltip="This column displays the end Date for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Days" HeaderText="Days" AllowSorting="false"
                        UniqueName="Days" HeaderTooltip="This column displays days for each record in the grid">
                        <ItemStyle Wrap="true" Width="10px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Shift" HeaderText="Shift" SortExpression="Shift"
                        UniqueName="Shift" HeaderTooltip="This column displays shift for each record in the grid">
                    </telerik:GridBoundColumn>
                    <%--     <telerik:GridBoundColumn DataField="Groups" HeaderText="Groups" SortExpression="Groups"
                        UniqueName="Groups" HeaderTooltip="This column displays the groups for each record in the grid">
                    </telerik:GridBoundColumn>--%>
                    <%--  <telerik:GridBoundColumn DataField="Status" HeaderText="Status" SortExpression="Status"
                        UniqueName="Status" HeaderTooltip="This column displays the status for each record in the grid">
                    </telerik:GridBoundColumn>--%>

                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Status" ItemStyle-Width="150px" HeaderText="Status" SortExpression="Status">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnStatus" ButtonType="LinkButton" CommandName="Status"
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

<asp:Button ID="btnDoPostback" runat="server" CssClass="buttonHidden" OnClick="btnDoPostback_Click" />
<asp:HiddenField ID="hdnAdvancesearch" runat="server" Value="false" />
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

    function openCmbBoxOnTab(sender, e) {
        if (!sender.get_dropDownVisible()) sender.showDropDown();
    }

    function AdvancedSearchPanelClick() {
        var classValues = $jQuery("[id$=hdrAdvancedSearch]").attr('class');
        var hdnAdvanceSearch = $jQuery("[id$=hdnAdvancesearch]");
        if (classValues == "mhdr colps") {
            hdnAdvanceSearch.val('true');
        }
        else {
            hdnAdvanceSearch.val('false');
        }
    }

    function grd_rwDbClick_Request(s, e) {
        var _id = "btnPreview";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function CorrectStartToEndDate(picker) {
        var date1 = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
        var date2 = $jQuery("[id$=dpEndDate]")[0].control.get_selectedDate();
        if (date1 != null && date2 != null) {
            if (date1 > date2)
                $jQuery("[id$=dpEndDate]")[0].control.set_selectedDate(null);
        }
    }

    var minDate = new Date("01/01/1980");
    function SetMinDate(picker) {
        picker.set_minDate(minDate);
    }

    function SetMinEndDate(picker) {
        picker.set_minDate(minDate);
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
        //debugger;
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

                $jQuery("#<%=btnDoPostback.ClientID %>").trigger('click');

            }
            winopen = false;
        }

    }

</script>
