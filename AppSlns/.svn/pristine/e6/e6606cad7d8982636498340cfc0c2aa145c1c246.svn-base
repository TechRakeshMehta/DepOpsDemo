<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateUpdateOpportunityPopup.aspx.cs" Inherits="CoreWeb.PlacementMatching.Views.CreateUpdateOpportunityPopup"
    MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxCreateNewOppotunityPopup">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>


    <script type="text/javascript">

        function openCmbBoxOnTab(sender, e) {
            if (!sender.get_dropDownVisible()) sender.showDropDown();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function returnToParent() {
            //debugger;
            var hdnIsSavedSuccessfully = $jQuery("[id$=hdnIsSavedSuccessfully]")[0].value;
            var hdnIsPublishedSuccessfully = $jQuery("[id$=hdnIsPublishedSuccessfully]")[0].value;

            var oArg = {};

            oArg.IsSavedSuccessfully = hdnIsSavedSuccessfully;
            oArg.IsPublishedSuccessfully = hdnIsPublishedSuccessfully;
            //get a reference to the current RadWindow
            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }

        function HideShowOnCheckChange(sender) {
            debugger;
            var selectedValue = $jQuery("[id$=rbtnContainsFloatArea] [type='radio']:checked").val();
            var dvFloatArea = $jQuery("[id$=dvFloatArea]");

            if (selectedValue != null && selectedValue == "true") {
                dvFloatArea[0].style.display = "block";
            }
            else {
                dvFloatArea[0].style.display = "none";
            }
        }

        var minDate = new Date("01/01/1980");
        function SetMinDate(picker) {
            picker.set_minDate(minDate);
        }

        function CorrectStartToEndDate(picker) {
            var date1 = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
            var date2 = $jQuery("[id$=dpEndDate]")[0].control.get_selectedDate();
            if (date1 != null && date2 != null) {
                if (date1 > date2)
                    $jQuery("[id$=dpEndDate]")[0].control.set_selectedDate(null);
            }
        }

        function SetMinEndDate(picker) {
            var date = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
            if (date != null) {
                picker.set_minDate(date);
            }
            else {
                picker.set_minDate(minDate);
            }
        }
    </script>


    <div class="container-fluid" style="padding-top: 20px; height: auto;">
        <div class="row">
            <div class="col-md-12 col-sm-12">
                <div class='vldx'>
                    <asp:Label ID="lblError" Font-Size="Medium" runat="server" Visible="false" CssClass="errmsg" Text="Atleast one shift details are required."></asp:Label>
                </div>
                <h2 class="header-color" id="hdrPopUp" runat="server"></h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12">&nbsp;&nbsp;</div>
        </div>

        <div class="row bgLightGreen">
            <asp:Panel ID="pnlAddOpportunity" runat="server">
                <div class="col-md-12 col-sm-12">&nbsp;</div>
                <div class='col-md-12 col-sm-12'>
                    <div class="row">
                        <div class='form-group col-md-5 col-sm-5' title="Select the Institution">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <asp:RadioButtonList ID="rblInstitutionAvailability" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblInstitution_SelectedIndexChanged"
                                DataTextField="IAT_Name" DataValueField="IAT_Code" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </div>
                        <div class='form-group col-md-5 col-sm-5' title="Select Agency" style="display: none">
                            <uc:AgencyHierarchyMultiple ID="ucAgencyHierarchyMultiple" runat="server" />
                        </div>
                    </div>
                </div>
                <div class='col-md-12 col-sm-12'>
                    <div class="row">
                        <div class='form-group col-md-3 col-sm-3' title="Enter the location">
                            <span class="cptn">Location</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlLocation" runat="server" DataTextField="Location"
                                DataValueField="AgencyLocationID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--SELECT--"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvLocation" ControlToValidate="ddlLocation"
                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Location is required." />
                            </div>
                        </div>
                        <div class='form-group col-md-3 col-sm-3' title="Enter unit for opportunity">
                            <span class="cptn">Department</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlDepartment" runat="server" DataTextField="Name"
                                DataValueField="DepartmentID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--SELECT--"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvDepartment" ControlToValidate="ddlDepartment"
                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Department is required." />
                            </div>
                        </div>

                        <div class='form-group col-md-3 col-sm-3' title="Select type of Student for opportunity">
                            <span class='cptn'>Students Type</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlStudentType" runat="server" DataTextField="Name" EmptyMessage="--Select--"
                                AllowCustomText="true" CausesValidation="false" DataValueField="StudentTypeId" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                                <Localization CheckAllString="All" />
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvStudentType" ControlToValidate="ddlStudentType"
                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Student type(s) is required." />
                            </div>
                        </div>

                        <div class='form-group col-md-3 col-sm-3' title="Enter unit for opportunity">
                            <span class="cptn">Unit</span>
                            <infs:WclTextBox ID="txtUnit" runat="server" Width="100%" CssClass="form-control" MaxLength="512">
                            </infs:WclTextBox>
                        </div>

                        <%-- <div class='form-group col-md-3 col-sm-3' title="Select days for opportunity.">
                                <span class="cptn">Groups</span>
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
                    </div>
                </div>
                <div class='col-md-12 col-sm-12'>
                    <div class="row">
                        <div class='form-group col-md-3 col-sm-3' title="Select available dates for opportunity">
                            <span class="cptn">Start Date</span><span class='reqd'>*</span>
                            <infs:WclDatePicker ID="dpStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                                Width="100%" CssClass="form-control" ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate">
                            </infs:WclDatePicker>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="dpStartDate"
                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Start date is required." />
                            </div>
                        </div>

                        <div class="form-group col-md-3 col-sm-3" title="Restrict search results to the entered end date">
                            <span class="cptn">End Date</span><span class='reqd'>*</span>
                            <infs:WclDatePicker ID="dpEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                                Width="100%" CssClass="form-control" ClientEvents-OnPopupOpening="SetMinEndDate">
                            </infs:WclDatePicker>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="dpEndDate"
                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="End date is required." />
                            </div>
                        </div>

                        <div class='form-group col-md-3 col-sm-3' title="Enter speciality for opportunity">
                            <span class="cptn">Specialty</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlSpecialty" runat="server" DataTextField="Name" EmptyMessage="--Select--"
                                AllowCustomText="true" CausesValidation="false" DataValueField="SpecialtyID"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvSpecialty" ControlToValidate="ddlSpecialty"
                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Specialty is required." />
                            </div>
                        </div>

                        <%-- <div class='form-group col-md-3 col-sm-3' title="Enter maximum number for opportunity" visible="false" style="display: none">
                            <span class="cptn">Max #</span><span class='reqd'>*</span>
                            <infs:WclNumericTextBox ID="txtMax" NumberFormat-DecimalDigits="0" Type="Number" runat="server" Width="100%" CssClass="form-control">
                            </infs:WclNumericTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvMax" ControlToValidate="txtMax"
                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Max. number of students is required." />
                            </div>
                        </div>--%>
                    </div>
                </div>
                <div class='col-md-12 col-sm-12'>
                    <div class="row">
                        <div class='form-group col-md-3 col-sm-3' title="Select Yes, if is preceptionship?">
                            <span class="cptn">Is Preceptionship</span>
                            <asp:RadioButtonList ID="rbtnIsPreceptionship" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>

                        </div>
                        <%-- <div class='form-group col-md-3 col-sm-3' title="Enter course for opportunity">
                                <span class="cptn">Number Of Students</span>

                                <infs:WclNumericTextBox ID="txtNoOfStudents" runat="server" Width="100%" CssClass="form-control">
                                </infs:WclNumericTextBox>
                            </div>--%>
                        <div class='form-group col-md-3 col-sm-3' title="">
                            <span class="cptn">Contains Float Area</span>

                            <asp:RadioButtonList ID="rbtnContainsFloatArea" runat="server" RepeatDirection="Horizontal" AutoPostBack="false" onclick="HideShowOnCheckChange(this);">
                                <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>

                            <%-- <infs:WclButton ID="chkContainsFloatArea" runat="server" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false" OnClientCheckedChanged="HideShowOnCheckChange">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>--%>
                        </div>
                        <div class='form-group col-md-3 col-sm-3' title="Enter float area for opportunity" id="dvFloatArea" style="display: none;" runat="server">
                            <span class="cptn">Float Area</span>
                            <infs:WclTextBox ID="txtFloatArea" runat="server" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                        </div>
                    </div>
                </div>

                 <div id="dvCustomAttr" runat="server" class='col-md-12 col-sm-12'></div>

                <div class="col-md-12 col-sm-12">&nbsp;</div>
                <div class='col-md-12 col-sm-12'>
                    <div class="row">
                        <div class='form-group col-md-9 col-sm-9' title="Enter shift for opportunity">
                            <span class="header-color fa-underline" style="font-size: small; font-weight: bold;">Add Shift Detail</span>
                        </div>
                        <%--<div class='form-group col-md-3 col-sm-3'>
                            <asp:LinkButton ID="btnAddNewRecord" runat="server" OnClick="btnAddNewRecord_Click" Text="Add Shift" ToolTip="Add a Shift"
                                CausesValidation="true" ValidationGroup="grpShift"
                                CssClass="form-control blueText" />
                        </div>--%>
                    </div>
                </div>
                <%--<div class='col-md-12 col-sm-12'>--%>
                <%--       <asp:Repeater runat="server" ID="rptrShiftDetails" Visible="true" OnItemCommand="rptrShiftDetails_ItemCommand" OnItemDataBound="rptrShiftDetails_ItemDataBound">
                    <ItemTemplate>


                        <div class="col-md-12 col-sm-12">
                            <div class="row">
                                <asp:HiddenField ID="hdnIsEditClick" runat="server" Value='<%# Eval("IsEditClick")%>' />
                                <div class='form-group col-md-3 col-sm-3'>
                                    <span class='cptn'>Shift</span><span class='reqd'>*</span>
                                    <asp:Label runat="server" ID="lblShift" Visible="false" Width="100%" CssClass="form-control"> </asp:Label>
                                    <infs:WclTextBox runat="server" CssClass="form-control" ID="txtShift" Width="100%">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvShift" ControlToValidate="txtShift"
                                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Shift name is required."
                                            ValidationGroup="grpShift" />
                                    </div>
                                </div>
                                <div class='form-group col-md-3 col-sm-3'>
                                    <span class='cptn'>Shift From</span><span class='reqd'>*</span>
                                    <asp:Label runat="server" ID="lblShiftFrom" Width="100%" CssClass="form-control" Visible="false"></asp:Label>
                                    <infs:WclTimePicker ID="tpShiftFrom" runat="server" TimeView-Height="300px" Width="100%"
                                        CssClass="form-control">
                                        <TimeView CssClass="calanderFontSetting" Interval="01:00:00"></TimeView>
                                    </infs:WclTimePicker>
                                    <div class="vldx vlMiddelName">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvShiftFrom" ControlToValidate="tpShiftFrom"
                                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Shift from is required." ValidationGroup="grpShift" />
                                    </div>
                                </div>
                                <div class='form-group col-md-3 col-sm-3'>
                                    <span class='cptn'>Shift To</span><span class='reqd'>*</span>
                                    <asp:Label runat="server" ID="lblShiftTo" Width="100%" CssClass="form-control" Visible="false"></asp:Label>

                                    <infs:WclTimePicker ID="tpShiftTo" runat="server" TimeView-Height="300px" Width="100%"
                                        CssClass="form-control">
                                        <TimeView CssClass="calanderFontSetting" Interval="01:00:00"></TimeView>
                                    </infs:WclTimePicker>

                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvShiftTo" ControlToValidate="tpShiftTo"
                                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Shift to is required."
                                            ValidationGroup="grpShift" />
                                        <asp:CompareValidator ID="CfvShiftTo" CssClass="errmsg" ControlToValidate="tpShiftTo" ControlToCompare="tpShiftFrom" runat="server"
                                            ErrorMessage="Shift to time should be greater than shift from time." Operator="GreaterThan" ValidationGroup="grpFormSubmit"></asp:CompareValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12">
                            <div class="row">
                                <div class='form-group col-md-3 col-sm-3'>
                                    <span class="cptn">Days</span><span class='reqd'>*</span>
                                    <asp:Label runat="server" ID="lblDays" Width="100%" CssClass="form-control" Visible="false"></asp:Label>

                                    <infs:WclComboBox ID="ddlDays" runat="server" CheckBoxes="true" EmptyMessage="--SELECT--"
                                        DataValueField="WeekDayID" DataTextField="Name" Width="100%" CssClass="form-control"
                                        Skin="Silk" AutoSkinMode="false">
                                    </infs:WclComboBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDays" ControlToValidate="ddlDays"
                                            Display="Dynamic" ValidationGroup="grpShift" CssClass="errmsg"
                                            Text="Day(s) is required." />
                                    </div>
                                </div>
                                <div class='form-group col-md-5 col-sm-5'>
                                    <span class='cptn'>Number of Students</span><span class='reqd'>*</span>
                                    <asp:Label runat="server" ID="lblNumberOfStudents" Width="100%" CssClass="form-control" Visible="false"></asp:Label>

                                    <infs:WclNumericTextBox ID="txtNoOfStudents" NumberFormat-DecimalDigits="0" Type="Number" runat="server" Width="100%"
                                        CssClass="form-control">
                                    </infs:WclNumericTextBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvNoOfStudents" ControlToValidate="txtNoOfStudents"
                                            Display="Dynamic" CssClass="errmsg" ErrorMessage="Number of students is required."
                                            ValidationGroup="grpShift" />
                                    </div>
                                </div>
                                <div class='form-group col-md-3 col-sm-3' id="divButtons" runat="server">
                                    <div class='col-md-3 col-sm-3' id="dvEdit" runat="server">
                                        <span class="cptn" style="color: transparent !important;"></span>
                                        <asp:LinkButton CommandName="Edit" runat="server" Text="Edit" ID="btnEdit" CausesValidation="true"
                                            ValidationGroup="grpShift" CssClass="form-control blueText"></asp:LinkButton>
                                    </div>
                                    <div class="col-md-3 col-sm-3" id="dvDelete" runat="server">
                                        <span class="cptn" style="color: transparent !important;"></span>
                                        <asp:LinkButton ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete the Shift ?')"
                                            runat="server" CommandName="delete" Text="Delete" ValidationGroup="grpShift"
                                            CausesValidation="false" CssClass="form-control blueText" />
                                    </div>
                                    <div class='col-md-3 col-sm-3' id="dvSave" runat="server">
                                        <span class="cptn" style="color: transparent !important;"></span>
                                        <asp:LinkButton CommandName="Save" runat="server" Text="Save" ID="btnSave" CausesValidation="true"
                                            ValidationGroup="grpShift" CssClass="form-control blueText"></asp:LinkButton>
                                    </div>
                                    <div class='col-md-3 col-sm-3' id="dvCancel" runat="server">
                                        <span class="cptn" style="color: transparent !important;"></span>
                                        <asp:LinkButton CommandName="Cancel" runat="server" Text="Cancel" ID="btnCancel" CausesValidation="true"
                                            CssClass="form-control blueText"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </ItemTemplate>
                </asp:Repeater>--%>

                <infs:WclGrid Width="100%" CssClass="gridhover" runat="server" ID="grdShiftDetails"
                    AllowCustomPaging="false" GridCode="AAAA"
                    AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
                    AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
                    OnNeedDataSource="grdShiftDetails_NeedDataSource" OnItemCommand="grdShiftDetails_ItemCommand" OnItemDataBound="grdShiftDetails_ItemDataBound" EnableLinqExpressions="false" ShowClearFiltersButton="false">
                    <ClientSettings EnableRowHoverStyle="true">
                        <%--<ClientEvents OnRowDblClick="grd_rwDbClick_Opportunities" />--%>
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                        Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                        ExportOnlyData="true" IgnorePaging="true">
                    </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="ClinicalInventoryShiftID,ClinicalInventoryID"
                        AllowFilteringByColumn="false">
                        <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Shift" ShowRefreshButton="false" />
                        <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <AlternatingItemStyle BackColor="#f2f2f2" />
                        <ItemStyle BackColor="#ffffff" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="ClinicalInventoryID" HeaderText="ClinicalInventoryID" AllowSorting="false" SortExpression="ClinicalInventoryID"
                                HeaderTooltip="This column displays the Opportunity Id for each record in the grid"
                                UniqueName="ClinicalInventoryID" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Shift" HeaderText="Shift" AllowSorting="false" SortExpression="Shift"
                                HeaderTooltip="This column displays the shift for each record in the grid"
                                UniqueName="Shift">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ShiftFromString" HeaderText="Shift From"
                                AllowSorting="true" SortExpression="ShiftFrom"
                                UniqueName="ShiftFrom" HeaderTooltip="This column displays the shift start time for each record in the grid">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ShiftToString" HeaderText="Shift to"
                                AllowSorting="true" SortExpression="ShiftTo"
                                UniqueName="ShiftTo" HeaderTooltip="This column displays the shift end time for each record in the grid">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Days" HeaderText="Days" AllowSorting="false"
                                UniqueName="Days" HeaderTooltip="This column displays days for each record in the grid">
                                <ItemStyle Wrap="true" Width="10px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="NumberOfStudents" HeaderText="# Students" AllowSorting="false"
                                SortExpression="NumberOfStudents" HeaderTooltip="This column displays the number of students for each record in the grid" UniqueName="NumberOfStudents">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif"
                                CommandName="Delete" ConfirmText="Are you sure you want to delete this rotation?"
                                Text="Delete" UniqueName="DeleteColumn">
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Resources/Mod/Dashboard/images/editGrid.gif"
                                UniqueName="EditCommandColumn">
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridEditCommandColumn>
                        </Columns>
                        <EditFormSettings EditFormType="Template">
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                            <FormTemplate>
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h2 class="header-color">
                                                <asp:Label ID="lblShift" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Shift" : "Update Shift" %>'
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

                                    <asp:Panel ID="pnlEditForm" CssClass="editForm" runat="server">
                                        <div class="row bgLightGreen">
                                            <div class="col-md-12">
                                                <div class="row">
                                                    <div class='form-group col-md-3 col-sm-3' title="Enter shift name.">
                                                        <span class='cptn'>Shift</span><span class='reqd'>*</span>

                                                        <infs:WclTextBox runat="server" CssClass="form-control" ID="txtShift" Width="100%">
                                                        </infs:WclTextBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvShift" ControlToValidate="txtShift"
                                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Shift name is required."
                                                                ValidationGroup="grpShift" />
                                                        </div>
                                                    </div>
                                                    <div class='form-group col-md-3 col-sm-3'>
                                                        <span class='cptn'>Shift From</span><span class='reqd'>*</span>

                                                        <infs:WclTimePicker ID="tpShiftFrom" runat="server" TimeView-Height="300px" Width="100%"
                                                            CssClass="form-control">
                                                            <TimeView CssClass="calanderFontSetting" Interval="01:00:00"></TimeView>
                                                        </infs:WclTimePicker>
                                                        <div class="vldx vlMiddelName">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvShiftFrom" ControlToValidate="tpShiftFrom"
                                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Shift from is required." ValidationGroup="grpShift" />
                                                        </div>
                                                    </div>
                                                    <div class='form-group col-md-3 col-sm-3'>
                                                        <span class='cptn'>Shift To</span><span class='reqd'>*</span>


                                                        <infs:WclTimePicker ID="tpShiftTo" runat="server" TimeView-Height="300px" Width="100%"
                                                            CssClass="form-control">
                                                            <TimeView CssClass="calanderFontSetting" Interval="01:00:00"></TimeView>
                                                        </infs:WclTimePicker>

                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvShiftTo" ControlToValidate="tpShiftTo"
                                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Shift to is required."
                                                                ValidationGroup="grpShift" />
                                                            <asp:CompareValidator ID="CfvShiftTo" CssClass="errmsg" ControlToValidate="tpShiftTo" ControlToCompare="tpShiftFrom" runat="server"
                                                                ErrorMessage="Shift to time should be greater than shift from time." Operator="GreaterThan" ValidationGroup="grpFormSubmit"></asp:CompareValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-12">
                                                <div class="row">
                                                    <div class='form-group col-md-3 col-sm-3'>
                                                        <span class="cptn">Days</span><span class='reqd'>*</span>

                                                        <infs:WclComboBox ID="ddlDays" runat="server" CheckBoxes="true" EmptyMessage="--SELECT--"
                                                            DataValueField="WeekDayID" DataTextField="Name" Width="100%" CssClass="form-control"
                                                            Skin="Silk" AutoSkinMode="false">
                                                        </infs:WclComboBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvDays" ControlToValidate="ddlDays"
                                                                Display="Dynamic" ValidationGroup="grpShift" CssClass="errmsg"
                                                                Text="Day(s) is required." />
                                                        </div>
                                                    </div>
                                                    <div class='form-group col-md-5 col-sm-5'>
                                                        <span class='cptn'>Number of Students</span><span class='reqd'>*</span>

                                                        <infs:WclNumericTextBox ID="txtNoOfStudents" NumberFormat-DecimalDigits="0" Type="Number" runat="server" Width="100%"
                                                            CssClass="form-control">
                                                        </infs:WclNumericTextBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvNoOfStudents" ControlToValidate="txtNoOfStudents"
                                                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Number of students is required."
                                                                ValidationGroup="grpShift" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <infsu:CommandBar ID="fsucCmdBarShiftDetails" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                        GridInsertText="Save" GridUpdateText="Save" SaveButtonIconClass="rbSave"
                                        ValidationGroup="grpShift" UseAutoSkinMode="false" ButtonSkin="Silk" />
                                </div>
                            </FormTemplate>
                        </EditFormSettings>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                            Position="TopAndBottom" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="false">
                    </FilterMenu>
                </infs:WclGrid>

                <%--</div>--%>
                <div class='col-md-12 col-sm-12'>
                    <div class="row">

                        <div class='form-group col-md-9 col-sm-9'>&nbsp;&nbsp;</div>


                    </div>
                </div>
            </asp:Panel>
        </div>

        <div class="row bgLightGreen">
            <asp:Panel ID="pnlPreviewOpportunity" runat="server" Visible="false">

                <%--<div class='col-md-12 col-sm-12'>--%>
                <div class="col-md-12 col-sm-12">&nbsp;</div>
                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class='form-group col-md-6 col-sm-6' title="Selected Institution">
                            <span class="cptn">Available For</span>
                            <asp:Label ID="lblAvailableFor" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                        <div class='form-group col-md-6 col-sm-6' title="Location for opportunity">
                            <span class="cptn">Location</span>
                            <asp:Label ID="lblLocation" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12">
                    <div class="row">

                        <div class='form-group col-md-6 col-sm-6' title="Selected Student type(s) for opportunity">
                            <span class='cptn'>Students Type</span>
                            <asp:Label ID="lblStudentsType" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                        <div class='form-group col-md-6 col-sm-6' title="Department for opportunity">
                            <span class="cptn">Department</span>
                            <asp:Label ID="lblDepartment" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class='form-group col-md-6 col-sm-6' title="Specialty for opportunity">
                            <span class="cptn">Specialty</span>
                            <asp:Label ID="lblSpecialty" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                        <div class='form-group col-md-6 col-sm-6' title="Unit for opportunity">
                            <span class="cptn">Unit</span>
                            <asp:Label ID="lblUnit" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                    </div>
                </div>


                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class='form-group col-md-6 col-sm-6' title="Select available dates for opportunity">
                            <span class="cptn">Start Date</span>
                            <asp:Label ID="lblStartDate" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="form-group col-md-6 col-sm-6" title="Restrict search results to the entered end date">
                            <span class="cptn">End Date</span>
                            <asp:Label ID="lblEndDate" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class='form-group col-md-6 col-sm-6' title="Is preceptionship for opportunity.">
                            <span class="cptn">Is Preceptionship</span>
                            <asp:Label ID="lblIsPreceptionship" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                        <div class='form-group col-md-6 col-sm-6' title="Enter shift for opportunity">
                            <span class="cptn">Shift</span>
                            <asp:Label ID="lblShift" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                        <%--<div class='form-group col-md-6 col-sm-6' title="Group for opportunity">
                            <span class="cptn">Group</span>
                            <asp:Label ID="lblGroup" runat="server" Font-Size="Small"></asp:Label>
                        </div>--%>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class='form-group col-md-6 col-sm-6' title="Select days for opportunity.">
                            <span class="cptn">Days</span>
                            <asp:Label ID="lblDays" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                        <div class='form-group col-md-6 col-sm-6' title="Maximum number of students for opportunity">
                            <span class="cptn">Max #</span>
                            <asp:Label ID="lblMax" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class='form-group col-md-6 col-sm-6' title="">
                            <span class="cptn">Contains Float Area</span>
                            <asp:Label ID="lblContainsFloatArea" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-sm-12">
                    <div class="row">
                        <div class='form-group col-md-12 col-sm-12' title="" id="dvlabelFloatArea" runat="server">
                            <span class="cptn">Float Area</span>
                            <asp:Label ID="lblFloatArea" runat="server" Font-Size="Small"></asp:Label>
                        </div>
                    </div>
                </div>

                <%--  <div class="col-md-12 col-sm-12" style="display: none">
                    <div class="row">
                        <div class='form-group col-md-12 col-sm-12' title="Notes for opportunity">
                            <span class="cptn">Notes</span>
                            <asp:Label ID="lblNotes" runat="server" Visible="false" Font-Size="Small"></asp:Label>
                        </div>
                    </div>
                </div>--%>
                <%--</div>--%>
            </asp:Panel>
        </div>


        <div class="col-md-12 col-sm-12">&nbsp;&nbsp;</div>

        <div class="col-md-12 col-sm-12">
            <div class="row">
                <div class="text-center">
                    <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnPreview_Click" AutoSkinMode="false" ID="btnPreview" Text="Preview" ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="fa fa-eye" PrimaryIconLeft="15" PrimaryIconTop="8" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnEdit_Click" AutoSkinMode="false" ID="btnEdit" Text="Edit" ValidationGroup="grpFormSubmit" Visible="false" Icon-PrimaryIconCssClass="rbEdit">
                    </infs:WclButton>
                    <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnSave_Click" AutoSkinMode="false" ID="btnSave" Text="Save Draft" ValidationGroup="grpFormSubmit" Icon-PrimaryIconCssClass="rbSave">
                    </infs:WclButton>
                    <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnSubmit_Click" AutoSkinMode="false" ID="btnSubmit" Text="Publish" ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="fa fa-share-square-o" PrimaryIconLeft="15" PrimaryIconTop="10" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton CssClass="" runat="server" AutoPostBack="true" Skin="Silk" OnClick="btnCancel_Click" AutoSkinMode="false" ID="btnCancel" Text="Cancel" Icon-PrimaryIconCssClass="rbCancel">
                    </infs:WclButton>

                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnIsSavedSuccessfully" runat="server" />
    <asp:HiddenField ID="hdnIsPublishedSuccessfully" runat="server" />
</asp:Content>
