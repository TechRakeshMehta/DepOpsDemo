<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageTrackingAutoAssignmentConfiguration.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ManageTrackingAutoAssignmentConfiguration" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="SysXResourceManagerProxy1">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>


<script type="text/javascript">
   

    var minDate = new Date("01/01/1980");
    function SetMinDate(picker) {
        //debugger;
        picker.set_minDate(minDate);
    }

    function SetMinEndDate(picker) {
        //debugger;
        var date = $jQuery("[id$=dpStartDate]")[0].control.get_selectedDate();
        if (date != null) {
            picker.set_minDate(date);
        }
        else {
            picker.set_minDate(minDate);
        }
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

    function OnKeyPress(sender, args) {
        //debugger;
        if (args.get_keyCode() == 13) {
            args.get_domEvent().preventDefault();
            args.get_domEvent().stopPropagation();
            //$jQuery("[id$=btnSaveAbdReturnToQueue]").click();
        }
    }
    function OnClientSelectedIndexChanged(sender, args) {
        //debugger;
        var hdnIsPostback = $jQuery("[id$=hdnIsPostback]");
        if (areThereAnyChangesAtTheSelection(sender)) {
            hdnIsPostback.val(true);
            __doPostBack('ddlObjects', '');

        }
        else {
            hdnIsPostback.val(false);
            return false;
        }
    }

    function radComboBoxSelectedIdList(sender) {
        // debugger;
        var selectedIdList = [];
        var combo = sender;
        var items = combo.get_items();
        var checkedIndices = items._parent._checkedIndices;
        var checkedIndicesCount = checkedIndices.length;
        for (var itemIndex = 0; itemIndex < checkedIndicesCount; itemIndex++) {
            var item = items.getItem(checkedIndices[itemIndex]);
            selectedIdList.push(item._properties._data.value);
        }
        return selectedIdList;
    }

    function areThereAnyChangesAtTheSelection(sender) {
        //debugger;
        var oldSelectedIdList = '';
        var hdnPreviousObjectValues = $jQuery("[id$=hdnPreviousObjectValues]");
        if (hdnPreviousObjectValues.val() != "" && hdnPreviousObjectValues.val() != null && hdnPreviousObjectValues.val() != undefined) {
            oldSelectedIdList = hdnPreviousObjectValues.val().split(',');
        }
        var selectedIdList = radComboBoxSelectedIdList(sender);
        hdnPreviousObjectValues.val(selectedIdList.join(","));

        var isTheCountOfEachSelectionEqual = (selectedIdList.length == oldSelectedIdList.length);
        if (isTheCountOfEachSelectionEqual == false)
            return true;

        var oldIdListMINUSNewIdList = $jQuery(oldSelectedIdList).not(selectedIdList).get();
        var newIdListMINUSOldIdList = $jQuery(selectedIdList).not(oldSelectedIdList).get();

        if (oldIdListMINUSNewIdList.length != 0 || newIdListMINUSOldIdList.length != 0)
            return true;

        return false;
    }
</script>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">Manage Auto Assignment</h1>
        </div>
    </div>

    <div id="dvAdminConfig" class="row">
        <infs:WclGrid runat="server" ID="grdAdmins" AllowPaging="true" AutoGenerateColumns="false" CssClass="gridhover"
            AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
            GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true" ValidationGroup="grdAdminConfig"
            PageSize="10" NonExportingColumns="EditCommandColumn,DeleteColumn, AdminId" OnNeedDataSource="grdAdmins_NeedDataSource"
            OnItemCommand="grdAdmins_ItemCommand" OnItemDataBound="grdAdmins_ItemDataBound">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="TAC_ID,AdminId" AllowFilteringByColumn="true">
                <CommandItemSettings ShowExportToExcelButton="true"
                    ShowExportToPdfButton="true" ShowExportToCsvButton="true" AddNewRecordText="Add New Auto Assignment Configuration" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <%-- <telerik:GridTemplateColumn UniqueName="AssignItems" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="true" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAllOrderQueue(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectOrders" runat="server" onclick="UnCheckHeader(this)" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>

                    <telerik:GridNumericColumn DataField="AdminId" FilterControlAltText="Filter AdminId column"
                        HeaderText="Admin ID" SortExpression="AdminId" DataType="System.Int32" UniqueName="AdminId" Display="false"
                        DecimalDigits="0" HeaderTooltip="This column displays the admin Id for each record in the grid">
                    </telerik:GridNumericColumn>

                    <telerik:GridBoundColumn DataField="AdminFirstName" FilterControlAltText="Filter AdminFirstName column"
                        HeaderText="First Name" SortExpression="AdminFirstName" UniqueName="AdminFirstName"
                        HeaderTooltip="This column displays the Admin first name for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="AdminLastName" FilterControlAltText="Filter AdminLastName column"
                        HeaderText="Last Name" SortExpression="AdminLastName" UniqueName="AdminLastName"
                        HeaderTooltip="This column displays the admin last name for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridNumericColumn DataField="AssignmentCount" FilterControlAltText="Filter AssignmentCount column" HeaderText="Assignment Count"
                        SortExpression="AssignmentCount" HeaderTooltip="This column displays the assignment count for each record in the grid" DataType="System.Int32"
                        UniqueName="AssignmentCount">
                    </telerik:GridNumericColumn>

                    <%--<telerik:GridDateTimeColumn DataField="DateFrom" HeaderText="Submission Start Date" SortExpression="DateFrom" FilterControlAltText="Filter DateFrom column"
                        UniqueName="DateFrom" DataFormatString="{0:MM/dd/yyyy}" HeaderTooltip="This column displays the submission start date for each record in the grid" FilterControlWidth="40%">
                    </telerik:GridDateTimeColumn>

                    <telerik:GridDateTimeColumn DataField="DateTo" HeaderText="Submission End Date" SortExpression="DateTo" FilterControlAltText="Filter DateTo column"
                        UniqueName="DateTo" DataFormatString="{0:MM/dd/yyyy}" HeaderTooltip="This column displays the submission end date for each order" FilterControlWidth="40%">
                    </telerik:GridDateTimeColumn>--%>
                    <telerik:GridNumericColumn DataField="DaysFrom" FilterControlAltText="Filter DaysFrom column" HeaderText="Days From"
                        SortExpression="DaysFrom" HeaderTooltip="This column displays the days from for each record in the grid" DataType="System.Int32"
                        UniqueName="DaysFrom">
                    </telerik:GridNumericColumn>
                    <telerik:GridNumericColumn DataField="DaysTo" FilterControlAltText="Filter DaysTo column" HeaderText="Days To"
                        SortExpression="DaysTo" HeaderTooltip="This column displays the days to for each record in the grid" DataType="System.Int32"
                        UniqueName="DaysTo">
                    </telerik:GridNumericColumn>

                    <telerik:GridBoundColumn DataField="allObjectsName" FilterControlAltText="Filter allObjectsName column"
                        HeaderText="Compliance Object(s)" SortExpression="allObjectsName" UniqueName="allObjectsName"
                        HeaderTooltip="This column displays the compliance object(s) for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this record?"
                        Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                        <HeaderStyle Width="30px" />
                    </telerik:GridButtonColumn>
                    <%-- <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                            ToolTip="Click here to view details of ." runat="server" Text="Detail">
                        </telerik:RadButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                        UniqueName="EditCommandColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridEditCommandColumn>
                </Columns>

                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                    <FormTemplate>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h2 class="header-color">
                                        <asp:Label ID="lblTitleNode" Text='<%# "Update Auto Assignment Configuration" %>'
                                            runat="server" /></h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="msgbox">
                                        <asp:Label runat="server" ID="lblName" CssClass="info"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAdminConfig">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Admin Name</span>
                                                <infs:WclTextBox runat="server" ID="txtAdminName" Enabled="false">
                                                </infs:WclTextBox>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Assignment Count</span><%--<span class="reqd">*</span>--%>
                                                <infs:WclNumericTextBox ID="txtAssignmentCount" runat="server"
                                                    MaxLength="5" CssClass="form-control" InputType="Number" NumberFormat-DecimalDigits="0" MinValue="0">
                                                </infs:WclNumericTextBox>

                                                <%-- <div id="dvrfv" class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAssignmentCount" ControlToValidate="txtAssignmentCount"
                                                        Display="Dynamic" class="errmsg" ErrorMessage="Assignment count is required." ValidationGroup='grdAdminConfig'
                                                        Enabled="true" />
                                                </div>--%>
                                            </div>

                                            <%-- <div class='form-group col-md-3'>
                                                <span class="cptn">Submission Start Date</span><span class="reqd">* </span>
                                                <infs:WclDatePicker ID="dpStartDate" runat="server" CssClass="form-control"
                                                    ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate">
                                                </infs:WclDatePicker>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="dpStartDate"
                                                        Display="Dynamic" CssClass="errmsg" ValidationGroup="grdAdminConfig"
                                                        Text="Start Date is required." />
                                                </div>
                                            </div>--%>
                                            <%--  <div class='form-group col-md-3'>
                                                <span class="cptn">Submission End Date</span><span class="reqd">* </span>
                                                <infs:WclDatePicker ID="dpEndDate" runat="server" CssClass="form-control" ClientEvents-OnPopupOpening="SetMinEndDate"></infs:WclDatePicker>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="dpEndDate"
                                                        Display="Dynamic" CssClass="errmsg" ValidationGroup="grdAdminConfig"
                                                        Text="End Date is required." />--%>
                                            <%-- <asp:CompareValidator ID="cvEndDate" runat="server" Operator="GreaterThan" Type="Date" ControlToCompare="dpStartDate" ControlToValidate="dpEndDate"
                                                    class="errmsg" ValidationGroup="grdAdminConfig" Display="Dynamic" ErrorMessage="Submission end date should be greater than submission start date." />--%>
                                            <%--</div>
                                            </div>--%>

                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Days From</span><span class="reqd">*</span>
                                                <infs:WclNumericTextBox ID="txtDaysFrom" runat="server"
                                                    MaxLength="5" CssClass="form-control" InputType="Number" NumberFormat-DecimalDigits="0" MinValue="0">
                                                </infs:WclNumericTextBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvDaysFrom" ControlToValidate="txtDaysFrom"
                                                        Display="Dynamic" CssClass="errmsg" ValidationGroup="grdAdminConfig"
                                                        Text="Days from are required." />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Days To</span><span class="reqd">*</span>
                                                <infs:WclNumericTextBox ID="txtDaysTo" runat="server"
                                                    MaxLength="5" CssClass="form-control" InputType="Number" NumberFormat-DecimalDigits="0" MinValue="0">
                                                </infs:WclNumericTextBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvDaysTo" ControlToValidate="txtDaysTo"
                                                        Display="Dynamic" CssClass="errmsg" ValidationGroup="grdAdminConfig"
                                                        Text="Days to are required." />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Compliance Object</span>
                                                <infs:WclComboBox ID="ddlObjects" runat="server" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Filter="None" AutoPostBack="false"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnClientDropDownClosed="OnClientSelectedIndexChanged" CausesValidation="false"
                                                    OnClientKeyPressing="openCmbBoxOnTab" EmptyMessage="--Select--" DataTextField="CPO_Name" DataValueField="CPO_ID"
                                                    OnSelectedIndexChanged="ddlObjects_SelectedIndexChanged"
                                                    CheckedItemsTexts="DisplayAllInInput">
                                                    <Localization CheckAllString="All" />
                                                </infs:WclComboBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div>
                                    <asp:Repeater ID="rptrObjectPriority" runat="server">
                                        <ItemTemplate>
                                            <div class="row bgLightGreen">
                                                <div class="form-group col-md-3">
                                                    <asp:HiddenField ID="hdnObjId" runat="server" Value='<%#Eval("TCOM_ComplianceObjectID") %>' />
                                                    <asp:HiddenField ID="hdnObjMappingId" runat="server" Value='<%#Eval("TCOM_ID") %>' />

                                                    <span class="cptn">Object Name</span>

                                                    <infs:WclTextBox ID="txtObjectName" runat="server" ClientEvents-OnKeyPress="OnKeyPress" ReadOnly="true" Enabled="false" Text='<%#Eval("ObjectName") %>'></infs:WclTextBox>

                                                </div>
                                                <div class="form-group col-md-3">
                                                    <span class="cptn">Priority</span>

                                                    <infs:WclNumericTextBox ID="txtPriority" runat="server" MaxLength="5" NumberFormat-DecimalDigits="0" ClientEvents-OnKeyPress="OnKeyPress"
                                                        Value='<%# (String.IsNullOrEmpty(Convert.ToString(Eval("TCOM_Priority"))))? Convert.ToDouble(Eval("TCOM_Priority"))+1 :Convert.ToDouble(Eval("TCOM_Priority")) %>'>
                                                    </infs:WclNumericTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvPriority" ControlToValidate="txtPriority"
                                                            Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAdminConfig"
                                                            Text="Priority is required." />
                                                        <%--<asp:CompareValidator ID="cvPriority" runat="server" Operator="GreaterThan" Type="Integer" ControlToCompare="" ControlToValidate="txtPriority"
                                                    class="errmsg" ValidationGroup="grdAdminConfig" Display="Dynamic" ErrorMessage="Submission end date should be greater than submission start date." />--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>

                                <div class="col-md-12 text-right">
                                    <infsu:CommandBar ID="fsucCmdBarNode" runat="server" GridMode="true" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grdAdminConfig" ExtraButtonIconClass="icnreset" UseAutoSkinMode="false" ButtonSkin="Silk" />
                                </div>
                            </asp:Panel>

                        </div>
                    </FormTemplate>
                </EditFormSettings>

                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>

<asp:HiddenField ID="hdnPreviousObjectValues" runat="server" />
<asp:HiddenField ID="hdnIsPostback" runat="server" Value="false" />
