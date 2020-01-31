<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RotationStudentSearch.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.RotationStudentSearch" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/SharedUserRotationDetails.ascx" TagName="SharedUserRotationDetails" TagPrefix="uc" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/ColumnsConfiguration.ascx" TagPrefix="infsu" TagName="ColumnsConfiguration" %>

<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>

<infs:WclResourceManagerProxy runat="server" ID="rprxRotationStudentSearch">
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotation.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Generic/ColumnsConfiguration.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    /*.lnkBtn {
        background-color: transparent !important;
        padding-top: 0px !important;
    }

        .lnkBtn.rbLinkButton:hover {
            background-color: transparent !important;
        }*/

    /*UAT-3211 Rotation Tab*/
    .section, .tabvw {
        padding-bottom: 27px;
    }
    ul.rwControlButton {
        width: auto !important;
    }
</style>
<script>
    //UAT-3211 Rotation tab updates(Advanced Search)
    function AdvancedSearchPanelClick() {
        var classValues = $jQuery("[id$=mhdrPanel]").attr('class');
        var hdnAdvanceSearch = $jQuery("[id$=hdnAdvancesearch]");
        if (classValues == "mhdr colps") {
            hdnAdvanceSearch.val('true');
        }
        else {
            hdnAdvanceSearch.val('false');
        }
    }

    function OpenPopup(sender, eventArgs) {
        //debugger;
        var composeScreenWindowName = "composeScreen";
        var fromScreenName = "RotationStudentSearch";
        var communicationTypeId = 'CT01';
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/Messaging/Pages/WriteMessage.aspx?cType=" + communicationTypeId + "&SName=" + fromScreenName);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnMessagePopupClose });

        parent.$jQuery('.rwMaximizeButton').on('click', function () {
            setTimeout(function () {
                parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
            }, 50);
        });

        setTimeout(function () {
            parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
        }, 1000);

        parent.$jQuery('.rwTitlebarControls').on('click', function () {
            setTimeout(function () {
                parent.$jQuery('ul.rwControlButtons').attr('style', 'width: auto !important;');
            }, 40);
        });
        return false;
    }

    function OnMessagePopupClose(oWnd, args) {
        //debugger;
        oWnd.remove_close(OnMessagePopupClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.MessageSentStatus == "sent") {
                ShowSuccessMessage("Message has been sent successfully.", "sucs", true)
            }
        }
    }

    function ShowSuccessMessage(msg, msgtype, overriderErrorPanel) {
        /// <summary>Shows message box on the page</summary>
        /// <param name="msg" type="String">Message to be displayed</param>
        /// <param name="msgtype" type="$page.msgTypes">Type of message box</param>

        if (typeof (msg) == "undefined") return;
        var c = typeof (msgtype) != "undefined" ? msgtype : "";
        if ($jQuery(".no_error_panel").length > 0 || overriderErrorPanel) {
            $jQuery("#pageMsgBox").children("span").text(msg).attr("class", msgtype);
            if (c == 'sucs') {
                c = "Success";
            }
            else (c = c.toUpperCase());

            $jQuery("#pnlError").hide();

            $window.showDialog($jQuery("#pageMsgBox").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 400, c);
        }
        else {
            $jQuery("#pageMsgBox").fadeIn().children("span").text(msg).attr("class", msgtype);
        }
    }

    function CheckRemoveAll(id) {
       // debugger;
        var masterTable = $find("<%= grdRotations.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkRemoveItem").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkRemoveItem").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckRemoveAllHeader(id) {
       // debugger;
        var checkHeader = true;
        var masterTable = $find("<%= grdRotations.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkRemoveItem").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkRemoveItem").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkRemoveAll]')[0].checked = checkHeader;
    }


</script>
<div class="container-fluid">
    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Rotation Student Search</h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <div class="col-md-12">
            <div class="col-md-12">&nbsp;</div>
            <div class="col-md-12">
                <div class="row">
                    <div class="form-group col-md-3">
                        <span class="cptn">Institution</span>
                        <%--<span class='reqd'>*</span>--%>
                        <infs:WclComboBox ID="cmbTenant" Width="100%" CssClass="form-control" runat="server" CausesValidation="false"
                            OnClientKeyPressing="openCmbBoxOnTab" Filter="Contains" DataTextField="TenantName"
                            DataValueField="TenantID" OnDataBound="cmbTenant_DataBound" Skin="Silk" AutoSkinMode="false" ValidationGroup="grpFormSubmit"
                             EnableCheckAllItemsCheckBox="true" CheckBoxes="true" EmptyMessage="--SELECT--" >
                        </infs:WclComboBox>
                        <%--           <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="cmbTenant"
                            InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />--%>
                    </div>
                    <div class="form-group col-md-3">
                        <uc:AgencyHierarchyMultiple ID="ucAgencyHierarchyMultiple" runat="server" />
                        <asp:HiddenField ID="hdnSelectedAgencyIDs" Value="" runat="server" />
                        <asp:HiddenField ID="hdnSelectedNodeIDs" Value="" runat="server" />
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered First Name">
                        <span class="cptn">First Name</span>
                        <infs:WclTextBox ID="txtFirstName" Width="100%" CssClass="form-control" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class="form-group col-md-3">
                        <span class="cptn">Last Name</span>
                        <infs:WclTextBox ID="txtLastName" Width="100%" CssClass="form-control" runat="server">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="row">
                    <div class="form-group col-md-3">
                    </div>
                    <div class="section" id="sectionPanel" runat="server">
                        <h1 class="mhdr" id="mhdrPanel" runat="server" onclick="AdvancedSearchPanelClick()">Advanced Search
                        </h1>
                        <div class="content" id="contentPanel" runat="server">
                            <div class="swrap">

                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Complio ID</span>
                                            <infs:WclTextBox ID="txtComplioId" Width="100%" CssClass="form-control" runat="server">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Rotation ID/Name</span>
                                            <infs:WclTextBox ID="txtRotationName" Width="100%" CssClass="form-control" runat="server">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <span class='cptn'>Type/Specialty</span>
                                            <infs:WclTextBox ID="txtTypeSpecialty" Width="100%" CssClass="form-control" runat="server">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Department</span>
                                            <infs:WclTextBox ID="txtDepartment" Width="100%" CssClass="form-control" runat="server">
                                            </infs:WclTextBox>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Program</span>
                                            <infs:WclTextBox ID="txtProgram" Width="100%" CssClass="form-control" runat="server">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Course</span>
                                            <infs:WclTextBox ID="txtCourse" Width="100%" CssClass="form-control" runat="server">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Term</span><infs:WclTextBox ID="txtTerm" Width="100%" CssClass="form-control"
                                                runat="server">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Unit/Floor or Location</span>
                                            <infs:WclTextBox ID="txtUnit" CssClass="form-control" Width="100%" runat="server">
                                            </infs:WclTextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="form-group col-md-3">
                                            <span class="cptn"># of Recommended Hours</span>
                                            <infs:WclNumericTextBox Width="100%"
                                                ID="txtRecommendedHrs" runat="server" NumberFormat-DecimalDigits="2">
                                            </infs:WclNumericTextBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Days</span>
                                            <infs:WclComboBox ID="ddlDays" runat="server" Width="100%" CssClass="form-control"
                                                CheckBoxes="true" EmptyMessage="--SELECT--" Skin="Silk" AutoSkinMode="false"
                                                DataValueField="WeekDayID" DataTextField="Name">
                                            </infs:WclComboBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Shift</span><infs:WclTextBox CssClass="form-control" Width="100%"
                                                ID="txtShift" runat="server">
                                            </infs:WclTextBox>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Time</span>
                                            <infs:WclTimePicker ID="tpStartTime" runat="server" CssClass="form-control" Width="100%"
                                                TimeView-Height="300px">
                                                <TimeView Interval="00:15:00"></TimeView>
                                            </infs:WclTimePicker>
                                            <div class="gclrPad"></div>
                                            <infs:WclTimePicker ID="tpEndTime" runat="server" CssClass="form-control" Width="100%"
                                                TimeView-Height="300px">
                                                <TimeView Interval="00:15:00"></TimeView>
                                            </infs:WclTimePicker>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="form-group col-md-3">
                                            <span class="cptn">Date Range</span>
                                            <infs:WclDatePicker CssClass="form-control" Width="100%"
                                                ID="dpStartDate" runat="server" DateInput-EmptyMessage="Select a date (Start)"
                                                ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate">
                                            </infs:WclDatePicker>
                                            <div class="gclrPad"></div>
                                            <infs:WclDatePicker Width="100%" CssClass="form-control"
                                                ID="dpEndDate" runat="server" DateInput-EmptyMessage="Select a date (End)"
                                                ClientEvents-OnPopupOpening="SetMinEndDate">
                                            </infs:WclDatePicker>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12" id="trailingText">
                <infsu:CommandBar ID="fsucCmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                    AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo" CancelButtonText="Cancel"
                    SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch" UseAutoSkinMode="false" ButtonSkin="Silk"
                    OnCancelClick="fsucCmdBar_CancelClick" OnSaveClick="fsucCmdBar_SearchClick" OnSubmitClick="fsucCmdBar_ResetClick" ValidationGroup="grpFormSubmit">
                </infsu:CommandBar>
            </div>
            <div class="col-md-12">&nbsp;</div>
        </div>
    </div>
    <div id="dvColumnsConfiguration" style="display: none">
        <infsu:ColumnsConfiguration runat="server" ID="ColumnsConfiguration" />
    </div>
    <div class="row" id="linkHover">
        <infs:WclGrid Width="100%" GridCode="AAAN" CssClass="gridhover containsColumnsConfiguration" runat="server" ID="grdRotations" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            OnNeedDataSource="grdRotations_NeedDataSource" OnSortCommand="grdRotations_SortCommand" OnItemDataBound="grdRotations_ItemDataBound"
            OnItemCommand="grdRotations_ItemCommand"
            NonExportingColumns="ViewDetail" EnableLinqExpressions="false" ShowClearFiltersButton="false">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="RotationID, SlctdAgencyID, OrganizationUserId, TenantID"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="RemoveItems" HeaderTooltip="Click this box to remove all members"
                        AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkRemoveAll" runat="server" onclick="CheckRemoveAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRemoveItem" runat="server" onclick="UnCheckRemoveAllHeader(this)" OnCheckedChanged="chkRemoveItem_CheckedChanged" />
                            <%--   Enabled='<%#Convert.ToString(Eval("IsDropped")).ToLower() == "false" ? true : false %>' --%>
                            <%--/>--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="Student First Name" SortExpression="ApplicantFirstName"
                        HeaderTooltip="This column displays the student's first name for each record in the grid"
                        UniqueName="ApplicantFirstName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Student Last Name" SortExpression="ApplicantLastName"
                        HeaderTooltip="This column displays the student's last name for each record in the grid"
                        UniqueName="ApplicantLastName">
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
                    <telerik:GridBoundColumn DataField="TypeSpecialty" HeaderText="Type/Specialty" SortExpression="TypeSpecialty" HeaderStyle-Width="100px"
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
                        UniqueName="unit" HeaderTooltip="This column displays the Unit/Floor for each record in the grid" DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RecommendedHours" HeaderText="# of Recommended Hours" SortExpression="RecommendedHours"
                        UniqueName="RecommendedHours" HeaderTooltip="This column displays the # of Recommended Hours for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DaysName" HeaderText="Days" AllowSorting="false"
                        UniqueName="Days" HeaderTooltip="This column displays any user group(s) to which the applicant belongs for each record in the grid">
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

                    <telerik:GridBoundColumn DataField="ApplicantSSN" HeaderText="SSN/ID Number" SortExpression="ApplicantSSN"
                        HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid"
                        UniqueName="ApplicantSSN">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="DateOfBirth" HeaderText="Date of Birth" SortExpression="DateOfBirth"
                        UniqueName="DateOfBirth" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid" DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnDetails" ButtonType="LinkButton" CommandName="ViewDetail"
                                ToolTip="Click here to view details of rotation." runat="server" Text="Detail">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>

    <div class="row text-center" style="padding-top: 10px;">
        <infs:WclButton runat="server" Text="Send Message" ID="btnsendmail" Icon-PrimaryIconCssClass="rbEnvelope2"
            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnsendmail_Click" CssClass="btn" Visible="false">
        </infs:WclButton>
    </div>

    <asp:HiddenField ID="hdnAdvancesearch" runat="server" Value="false" />
     <asp:HiddenField ID="hdnDefaultTenantIDs" Value="" runat="server" />
     <asp:HiddenField ID="hdnSelectedTenantIds" ClientIDMode="Static" runat="server" Value="" />
</div>
<script src="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>
