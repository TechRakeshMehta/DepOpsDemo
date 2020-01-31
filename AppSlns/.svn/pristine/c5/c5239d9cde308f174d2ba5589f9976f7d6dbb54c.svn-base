<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageOpportunity.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.ManageOpportunity" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>
<%@ Register Src="~/ClinicalRotation/UserControl/SharedUserCustomAttributeForm.ascx" TagPrefix="ucCustom" TagName="CustomAttributes" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxManageOpportunity">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .buttonHidden {
        display: none;
    }

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
            <h2 class="header-color">Manage Opportunities
            </h2>
        </div>
    </div>
    <div class="row">&nbsp;&nbsp;</div>

    <asp:Panel ID="pnlSearch" runat="server">
        <div class="row bgLightGreen">
            <div class='col-md-12'>
                <div class="col-md-12">&nbsp;</div>
                <div class="col-md-12">
                    <div class="row">
                        <div id="divTenant" runat="server">
                            <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                                <span class="cptn">Institution</span>
                                <asp:RadioButtonList ID="rblInstitutionAvailability" runat="server" RepeatDirection="Horizontal" DataTextField="IAT_Name" DataValueField="IAT_Code"
                                    OnSelectedIndexChanged="rblInstitutionAvailability_SelectedIndexChanged" AutoPostBack="true">
                                    <%--     <asp:ListItem Text="Associated Institution(s)" Value="AAAA" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="All Institutions" Value="AAAB"></asp:ListItem>--%>
                                </asp:RadioButtonList>
                            </div>
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
                                        <%--<div class='form-group col-md-3' title="Select a Agency whose data you want to view.">
                                            <uc:AgencyHierarchyMultiple ID="ucAgencyHierarchyMultiple" runat="server" />
                                        </div>--%>
                                        <div class='form-group col-md-3' title="Restrict search results to the selected Location">
                                            <span class="cptn">Location</span>
                                            <infs:WclComboBox ID="ddlLocation" runat="server" DataTextField="Location"
                                                DataValueField="AgencyLocationID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--Select--"
                                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" AutoPostBack="false">
                                            </infs:WclComboBox>
                                        </div>
                                        <div class='form-group col-md-3' title="Restrict search results to the selected department">
                                            <span class="cptn">Department</span>
                                            <infs:WclComboBox ID="ddlDepartment" runat="server" DataTextField="Name"
                                                DataValueField="DepartmentID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--Select--"
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
                                                <%-- <Items>
                                                    <telerik:RadComboBoxItem Text="BSN" Value="AAAA" />
                                                    <telerik:RadComboBoxItem Text="Medical Lab Technician" Value="AAAB" />
                                                    <telerik:RadComboBoxItem Text="Pharmacy" Value="AAAC" />
                                                </Items>--%>
                                            </infs:WclComboBox>
                                        </div>

                                        <div class='form-group col-md-3' title="Restrict search results to the entered maximum number of students">
                                            <span class='cptn'>Max #</span>
                                            <infs:WclNumericTextBox ID="txtMaxNoOfStudent" runat="server" Width="100%" CssClass="form-control" NumberFormat-DecimalDigits="0"></infs:WclNumericTextBox>
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
                                        <%--<div class='form-group col-md-3' title="Restrict search results to the entered group">
                                            <span class="cptn">Group</span>
                                            <infs:WclComboBox ID="ddlGroups" runat="server" DataTextField="GroupName" EmptyMessage="--Select--"
                                                AllowCustomText="true" CausesValidation="false" DataValueField="GroupCode"
                                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Group 1" Value="AAAA" />
                                                    <telerik:RadComboBoxItem Text="Group 2" Value="AAAB" />
                                                    <telerik:RadComboBoxItem Text="Group 3" Value="AAAC" />
                                                    <telerik:RadComboBoxItem Text="Group 4" Value="AAAD" />
                                                    <telerik:RadComboBoxItem Text="Group 5" Value="AAAE" />
                                                </Items>
                                            </infs:WclComboBox>
                                        </div>--%>
                                        <div class='form-group col-md-3' title="Select">
                                            <span class="cptn">Archive Status</span>
                                            <asp:RadioButtonList ID="rbArchiveStatus" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
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

                <div class="col-md-12">&nbsp;</div>
                <div class="col-md-12">
                    <div class="row ">
                        <div style="width: 60%; float: left;">
                            <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Right" DisplayButtons="Submit,Save,Cancel"
                                AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo" OnSubmitClick="fsucCmdBarButton_ResetClick"
                                OnSaveClick="fsucCmdBarButton_SearchClick" OnCancelClick="fsucCmdBarButton_CancelClick"
                                SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                                CancelButtonText="Cancel"
                                UseAutoSkinMode="false" ButtonSkin="Silk">
                                <ExtraCommandButtons>
                                </ExtraCommandButtons>
                            </infsu:CommandBar>
                        </div>

                    </div>
                    <div class="col-md-12">&nbsp;</div>
                </div>
                <div class="col-md-12">&nbsp;</div>
            </div>
        </div>
    </asp:Panel>

    <div class="row allowscroll">
        <infs:WclGrid Width="100%" CssClass="gridhover" runat="server" ID="grdOpportunities"
            AllowCustomPaging="false" GridCode="AAAA"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            OnNeedDataSource="grdOpportunities_NeedDataSource" OnItemCommand="grdOpportunities_ItemCommand" EnableLinqExpressions="false" ShowClearFiltersButton="false">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick_Opportunities" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OpportunityID,StatusCode"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="true" ShowExportToCsvButton="true" AddNewRecordText="Create New Opportunity"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <AlternatingItemStyle BackColor="#f2f2f2" />
                <ItemStyle BackColor="#ffffff" />
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="AssignCheckBox" AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectOpportunity" runat="server" onclick="UnCheckHeader(this)"
                                OnCheckedChanged="chkSelectOpportunity_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="OpportunityID" HeaderText="Opportunity ID" AllowSorting="false" SortExpression="OpportunityID"
                        HeaderTooltip="This column displays the Opportunity Id for each record in the grid"
                        UniqueName="OpportunityID" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Agency" HeaderText="Agency" AllowSorting="false" SortExpression="Agency"
                        HeaderTooltip="This column displays the agency for each record in the grid"
                        UniqueName="Agency">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Location" HeaderText="Location" AllowSorting="false" SortExpression="Location"
                        HeaderTooltip="This column displays the location for each record in the grid"
                        UniqueName="Location">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Department" HeaderText="Department" AllowSorting="false"
                        SortExpression="Department" HeaderTooltip="This column displays the department for each record in the grid" UniqueName="Department">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Specialty" HeaderText="Specialty" SortExpression="Specialty"
                        HeaderTooltip="This column displays the specialty for each record in the grid"
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
                    <telerik:GridBoundColumn DataField="GroupName" HeaderText="Group" SortExpression="GroupName"
                        UniqueName="GroupName" HeaderTooltip="This column displays the group for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Status" HeaderText="Status" SortExpression="Status"
                        UniqueName="Status" HeaderTooltip="This column displays the status for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Preview" ItemStyle-Width="200px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnPreview" ButtonType="LinkButton" CommandName="Preview"
                                ToolTip="Click here to preview opportunity."
                                runat="server" Text="Preview">
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

    <div class="col-md-12">&nbsp;</div>
    <div class="col-md-12">
        <div class="row ">
            <div class="col-md-5"></div>
            <infs:WclMenu ID="cmdArchive" runat="server" Skin="Default" CssClass="top3, form-group col-md-1" AutoSkinMode="false" Visible="false">
                <Items>
                    <telerik:RadMenuItem Text="Archivemun">
                        <ItemTemplate>
                            <infs:WclButton runat="server" Text="Archive" ID="btnArchive" Icon-PrimaryIconCssClass="rbArchive" CssClass="btn"
                                Skin="Silk" ButtonPosition="Center" AutoSkinMode="false" OnClick="btnArchive_Click">
                            </infs:WclButton>
                        </ItemTemplate>
                        <Items>
                            <telerik:RadMenuItem>
                                <ItemTemplate>
                                    <infs:WclButton runat="server" Text="UnArchive" ID="btnUnArchive" Icon-PrimaryIconCssClass="rbUnArchive" CssClass="btn"
                                        Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnUnArchive_Click">
                                    </infs:WclButton>
                                </ItemTemplate>
                            </telerik:RadMenuItem>
                        </Items>
                    </telerik:RadMenuItem>
                </Items>
            </infs:WclMenu>
            <div class="form-group col-md-2">
                <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnDelete_Click" AutoSkinMode="false" ID="btnDelete" Text="Delete" Visible="false" Icon-PrimaryIconCssClass="fa fa-trash-o" Icon-PrimaryIconLeft="15" Icon-PrimaryIconTop="9">
                </infs:WclButton>
            </div>

        </div>
    </div>
</div>
<asp:Button ID="btnDoPostback" runat="server" CssClass="buttonHidden" OnClick="btnDoPostback_Click" />
<asp:HiddenField ID="hdnAdvancesearch" runat="server" Value="false" />
<asp:HiddenField ID="hdnOppportunityId" runat="server" />
<asp:HiddenField ID="hdnIsExistingOpportunity" runat="server" />
<asp:HiddenField ID="hdnIsNewOpportunity" runat="server" />
<asp:HiddenField ID="hdnIsSavedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnIsPublishedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnForAllInstitution" runat="server" />
<asp:HiddenField ID="hdnStatusCode" runat="server" />

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

    function grd_rwDbClick_Opportunities(s, e) {
        var _id = "btnPreview";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function CreateUpdateOpportunityPopUp() {
        // debugger;
        var composeScreenWindowName = "Create Opportunity";
        //var screenName = "CommonScreen";
        var oppportunityId = $jQuery("[id$=hdnOppportunityId]").val();
        var isExistingOpportunity = $jQuery("[id$=hdnIsExistingOpportunity]").val();
        var isNewOpportunity = $jQuery("[id$=hdnIsNewOpportunity]").val();
        var statusCode = $jQuery("[id$=hdnStatusCode]").val();
        //var forAllInstitutions = $jQuery("[id$=hdnForAllInstitution]").val();
        var popupHeight = $jQuery(window).height() * (80 / 100);
        var url = $page.url.create("~/PlacementMatching/Pages/CreateUpdateOpportunityPopup.aspx?OpportunityId=" + oppportunityId + "&IsExistingOpportunity=" + isExistingOpportunity + "&IsNewOpportunity=" + isNewOpportunity + "&StatusCode=" + statusCode);
        var win = $window.createPopup(url, { size: "1000," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                //do button postback to show success message.
                $jQuery("[id$=hdnIsSavedSuccessfully]").val(arg.IsSavedSuccessfully);
                $jQuery("[id$=hdnIsPublishedSuccessfully]").val(arg.IsPublishedSuccessfully);
                // 1
                $jQuery("#<%=btnDoPostback.ClientID %>").trigger('click');
            }
            winopen = false;
        }
    }

    function CheckAll(id) {
        // 2 
        var masterTable = $find("<%= grdOpportunities.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectOpportunity").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectOpportunity").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckHeader(id) {
        var checkHeader = true;
        //3
        var masterTable = $find("<%= grdOpportunities.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectOpportunity").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectOpportunity").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    function CorrectStartToEndDate(picker) {
        //debugger;
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

</script>
