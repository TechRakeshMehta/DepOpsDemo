<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminCreateOrderSearch.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.AdminCreateOrderSearch" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxAdminCreateOrderSearch">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">
                <asp:Label runat="server" ID="lblOrderSearch" CssClass="page-heading"></asp:Label>
            </h2>
        </div>
    </div>

    <div class="row bgLightGreen">
        <asp:Panel runat="server" ID="pnlSearchFilters">
            <div class="col-md-12">
                <div class="row">
                    <div id="dvTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the institution whose data you want to view">
                            <span class='cptn'>Institution</span><span class="reqd">*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" AutoPostBack="true"
                                DataTextField="TenantName" DataValueField="TenantID"
                                OnDataBound="ddlTenantName_DataBound" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control"
                                Skin="Silk" AutoSkinMode="false" EnableAriaSupport="true">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" InitialValue="--SELECT--"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <label for="<%= txtFirstName.ClientID %>" class="cptn">First Name</label>
                        <infs:WclTextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <label for="<%= txtLastName.ClientID %>" class="cptn">Last Name</label>
                        <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='col-md-12' title="Click the link and select a node to restrict search results to the selected node">
                        <label class="cptn">Institution Hierarchy</label>
                        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-12">&nbsp;</div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Order ID">
                        <label for="<%= txtOrderNumber.ClientID %>" class="cptn">Order ID</label>
                        <infs:WclTextBox ID="txtOrderNumber" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered SSN or ID Number">
                        <label for="<%= txtSSN.ClientID %>" class="cptn">SSN/ID Number</label>
                        <infs:WclMaskedTextBox ID="txtSSN" runat="server" MaxLength="10" Mask="aaa-aa-aaaa" EnableAriaSupport="true"
                            Width="100%" CssClass="form-control">
                        </infs:WclMaskedTextBox>
                    </div>
                    <div id="divDOB" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered dob">
                            <label for="<%= dpDob.ClientID %>_dateInput" class="cptn">DOB</label>
                            <infs:WclDatePicker ID="dpDob" runat="server" DateInput-EmptyMessage="Select a dob" EnableAriaSupport="true"
                                ClientEvents-OnPopupClosing="OnCalenderClosing" DateInput-SelectionOnFocus="CaretToBeginning"
                                Calendar-EnableKeyboardNavigation="true" Calendar-EnableAriaSupport="true" DateInput-EnableAriaSupport="true"
                                Width="100%" CssClass="form-control">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <label id="lblTransmit" class="cptn">Ready to Transmit</label>
                        <asp:RadioButtonList ID="rdbtnTransmitType" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" CssClass="form-control">
                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="No" Value="No"> </asp:ListItem>
                            <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                        <div class='vldx'>
                            <%--<asp:RequiredFieldValidator runat="server" ID="rfvOrderDateType" ControlToValidate="orderDateType"
                                Enabled="false" SetFocusOnError="true"
                                class="errmsg" Display="Dynamic" ErrorMessage="Order date type is required."
                                ValidationGroup='grpOrderSearch' />--%>
                        </div>
                    </div>
                </div>
        </asp:Panel>
    </div>

    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-12">
                <div class="row text-center">
                    <infsu:CommandBar ID="fsucAdminOrderCmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Extra,Cancel" 
                        AutoPostbackButtons="Submit,Extra,Cancel" SubmitButtonText="Search" SubmitButtonIconClass="rbSearch"
                        CancelButtonText="Cancel" ExtraButtonIconClass="rbUndo" ExtraButtonText="Reset" DefaultPanel="pnlSearchFilters"
                        OnSubmitClick="fsucAdminOrderCmdBar_SearchClick"  OnExtraClick="fsucAdminOrderCmdBar_ResetClick" ValidationGroup="grpFormSubmit"
                        OnCancelClick="fsucAdminOrderCmdBar_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">    
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>
</div>
<div>
    <infs:WclGrid runat="server" ID="grdAdminOrderDetails" AutoGenerateColumns="False" AllowSorting="True" CssClass="gridhover"
        AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" OnNeedDataSource="grdAdminOrderDetails_NeedDataSource"
        OnSortCommand="grdAdminOrderDetails_SortCommand" EnableLinqExpressions="false" ClientSettings-AllowKeyboardNavigation="true"
        ShowClearFiltersButton="false" GridLines="Both" ShowAllExportButtons="False" EnableAriaSupport="true" AllowCustomPaging="true"
        OnItemCommand="grdAdminOrderDetails_ItemCommand" OnItemDataBound="grdAdminOrderDetails_ItemDataBound"
        NonExportingColumns="AssignItems,ViewDetail,EditCommandColumn"
        OnDeleteCommand="grdAdminOrderDetails_DeleteCommand">
        <ClientSettings EnableRowHoverStyle="true">
            <ClientEvents OnRowDblClick="grdAdminOrderDetails_rwDbClick" />
            <Selecting AllowRowSelect="true"></Selecting>
        </ClientSettings>
        <GroupingSettings CaseSensitive="false" />
        <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
            Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
            ExportOnlyData="true" IgnorePaging="true">
        </ExportSettings>
        <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrderID,OrderNumber" AllowFilteringByColumn="false">
            <CommandItemSettings ShowExportToExcelButton="true"
                ShowExportToPdfButton="true" ShowExportToCsvButton="true" AddNewRecordText="Add New Order" />
            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
            </RowIndicatorColumn>
            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
            </ExpandCollapseColumn>
            <Columns>
                <telerik:GridTemplateColumn UniqueName="AssignItems" HeaderTooltip="Click this box to select all users on the active page"
                    AllowFiltering="false" ShowFilterIcon="false">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAllOrderQueue(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelectOrders" runat="server" onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectOrders_CheckedChanged" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridNumericColumn DataField="OrderNumber" FilterControlAltText="Filter OrderId column"
                    HeaderText="Order ID" SortExpression="OrderNumber" DataType="System.Int32" UniqueName="OrderId"
                    DecimalDigits="0" HeaderTooltip="This column displays the order number for each record in the grid">
                </telerik:GridNumericColumn>

                <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column"
                    HeaderText="Applicant First Name" SortExpression="FirstName" UniqueName="FirstName"
                    HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column"
                    HeaderText="Applicant Last Name" SortExpression="LastName" UniqueName="LastName"
                    HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                    HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid"
                    UniqueName="ApplicantSSN">
                </telerik:GridBoundColumn>

                <%--<telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        Display="false"
                        HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid"
                        UniqueName="_ApplicantSSN">
                    </telerik:GridBoundColumn>--%>

                <telerik:GridDateTimeColumn DataField="DOB" HeaderText="Date of Birth" SortExpression="DOB"
                    UniqueName="DOB" DataFormatString="{0:MM/dd/yyyy}" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid">
                </telerik:GridDateTimeColumn>

                <telerik:GridDateTimeColumn DataField="EntryDate" HeaderText="Draft Date" SortExpression="EntryDate"
                    UniqueName="EntryDate" DataFormatString="{0:MM/dd/yyyy}" HeaderTooltip="This column displays the created Date for each order">
                </telerik:GridDateTimeColumn>

                <telerik:GridBoundColumn DataField="OrderHierarchy" FilterControlAltText="Filter OrderHierarchy column"
                    AllowFiltering="false" HeaderText="Order Hierarchy" SortExpression="OrderHierarchy"
                    UniqueName="OrderHierarchy" HeaderTooltip="This column displays the order hierarchy for each record in the grid">
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="IsReadyToTransmit" FilterControlAltText="Filter TransmitType column"
                    SortExpression="IsReadyToTransmit" HeaderText="Ready to Transmit" UniqueName="IsReadyToTransmit"
                    HeaderTooltip="This column displays the whether the order is ready to transmit or not.">
                </telerik:GridBoundColumn>

                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Order?"
                    Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                    <HeaderStyle Width="30px" />
                </telerik:GridButtonColumn>
                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                            ToolTip="Click here to view details of Order." runat="server" Text="Detail">
                        </telerik:RadButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                    UniqueName="EditCommandColumn">
                    <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                </telerik:GridEditCommandColumn>

                <%-- <telerik:GridTemplateColumn HeaderText="Auto Renewal" UniqueName="AutoRenewal" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnAutoRenewal" runat="server" Visible="false"
                                OnClientClick="return ResetAutoRenewalStatus(this);">
                            </asp:LinkButton>
                            <asp:HiddenField ID="hfOrderID" runat="server" Value='<%#Eval("OrderId") %>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="tplcohdr" />
                    </telerik:GridTemplateColumn>--%>
            </Columns>
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"  Position="TopAndBottom"/>
        </MasterTableView>
        <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
        <FilterMenu EnableImageSprites="False">
        </FilterMenu>
    </infs:WclGrid>
</div>

<div class="row">&nbsp;</div>
<div class="row">
    <div class="col-md-12">
        <div class="form-group col-md-12">
            <div class="row text-center">
                <infsu:CommandBar ID="CmdBarTransmit" runat="server" ButtonPosition="Center" DisplayButtons="Submit"
                    AutoPostbackButtons="Submit" SubmitButtonText="Transmit" SubmitButtonIconClass="rbSave"
                    OnSubmitClick="CmdBarTransmit_SubmitClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                </infsu:CommandBar>
            </div>
        </div>
    </div>
</div>

<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<script type="text/javascript">
  
    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            // debugger;
            var popupHeight = $jQuery(window).height() * (100 / 100);
            //debugger;
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionHierarchyList.aspx?TenantId=" + tenantId + "&DepartmentProgramId=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }
    function OnClientClose(oWnd, args) {
        //debugger;
        oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
                __doPostBack("<%= btnDoPostBack.ClientID %>", "");
            }
            winopen = false;
            setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
        }
    }


    //function CorrectFrmToCrtdDate(picker) {

    //    var date1 = $jQuery("[id$=dpOdrCrtFrm]")[0].control.get_selectedDate();
    //    var date2 = $jQuery("[id$=dpOdrCrtTo]")[0].control.get_selectedDate();
    //    if (date1 != null && date2 != null) {
    //        if (date1 > date2)
    //            $jQuery("[id$=dpOdrCrtTo]")[0].control.set_selectedDate(null);
    //    }
    //}

    function grdAdminOrderDetails_rwDbClick(s, e) {
        var _id = "btnEdit";
        var b = e.get_gridDataItem().findElement(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }


    function CheckAllOrderQueue(id) {
        //debugger;
        var masterTable = $find("<%= grdAdminOrderDetails.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (masterTable.get_dataItems()[i].findElement("chkSelectOrders") != null && !(masterTable.get_dataItems()[i].findElement("chkSelectOrders").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectOrders").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckHeader(id) {
        // debugger;
        var checkHeader = true;
        var masterTable = $find("<%= grdAdminOrderDetails.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectOrders").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectOrders").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    function UnCheckOrderQueueHeader(id) {
        //debugger;
        var checkHeader = true;
        var masterTable = $find("<%= grdAdminOrderDetails.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (masterTable.get_dataItems()[i].findElement("chkSelectItem") != null && !(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

</script>
