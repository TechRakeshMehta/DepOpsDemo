<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MangeSystemEntityUserPermissions.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.MangeSystemEntityUserPermissions" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">Manage System Entity User Permissions
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Select the Institution whose data you want to view">
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbTenant" runat="server" AutoPostBack="false " DataTextField="TenantName"
                            DataValueField="TenantID" OnDataBound="cmbTenant_DataBound"
                            Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="cmbTenant"
                                InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Choose Entity</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbEntityType" runat="server" AutoPostBack="false" DataTextField="SE_Name"
                            DataValueField="SE_ID" OnDataBound="cmbEntityType_DataBound"
                            Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvEntityType" ControlToValidate="cmbEntityType"
                                InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Entity type is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <%-- <div class='sxlb' title="Restrict search results to the entered User ID">
                        <span class="cptn">User ID</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtUserID" MaxValue="2147483647"
                            runat="server" InvalidStyleDuration="100" MinValue="1">
                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                        </infs:WclNumericTextBox>
                    </div>--%>
                    <div class='sxlb' title="Restrict search results to the entered first name">
                        <span class="cptn">User's First Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtFirstName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb' title="Restrict search results to the entered last name">
                        <span class="cptn">User's Last Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtLastName" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset"
            SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel" ValidationGroup="grpFormSubmit"
            OnSubmitClick="CmdBarSearch_ResetClick" OnSaveClick="CmdBarSearch_SearchClick" OnCancelClick="CmdBarSearch_CancelClick">
        </infsu:CommandBar>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdUserPermissions" AllowCustomPaging="true" AllowPaging="True"
                AutoGenerateColumns="False" AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True"
                CellSpacing="0" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowClearFiltersButton="false"
                GridLines="Both" NonExportingColumns="EditCommandColumn,DeleteColumn" OnItemCreated="grdUserList_ItemCreated"
                OnNeedDataSource="grdUserPermissions_NeedDataSource" OnItemCommand="grdUserPermissions_ItemCommand"
                OnSortCommand="grdUserList_SortCommand" OnItemDataBound="grdUserPermissions_ItemDataBound" EnableLinqExpressions="false">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="false" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="SEUP_ID, EntityPermissionId, OrganizationUserId,HierarchyNodeID" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New User Permission"
                        ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="UserFirstName" HeaderText="User First Name"
                            SortExpression="UserFirstName" UniqueName="UserFirstName" HeaderTooltip="This column displays the user's first name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UserLastName" HeaderText="User Last Name"
                            SortExpression="UserLastName" UniqueName="UserLastName" HeaderTooltip="This column displays the user's last name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress"
                            UniqueName="EmailAddress" ItemStyle-Width="180px" HeaderTooltip="This column displays the user's email address for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PermissionName" FilterControlAltText="Filter Name column"
                            HeaderText="Permission" SortExpression="PermissionName" UniqueName="Permission">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HierarchyNodeLabel" FilterControlAltText="Filter HierarchyNodeLabel column"
                            HeaderText="Permission Hierarchy" SortExpression="HierarchyNodeLabel" UniqueName="HierarchyNodeLabel">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Record?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" runat="server" id="divEditBlock" visible="true">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHAttr" Text='<%# (Container is GridEditFormInsertItem) ? "Add New User Permission" : "Update User Permission" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAttr">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Select User</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbUsers" runat="server" AutoPostBack="false" DataTextField="UserName"
                                                        DataValueField="UserID" EmptyMessage="--Select--"
                                                        Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvUserName" ControlToValidate="cmbUsers"
                                                            Display="Dynamic" CssClass="errmsg" Text="User is required." ValidationGroup="grpPermission" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Permission</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:RadioButtonList runat="server" ID="rblPermissionList" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    </asp:RadioButtonList>
                                                    <infs:WclComboBox ID="cmbPermissionList" runat="server" OnClientItemChecked="checkNonePermission" CheckBoxes="true" EmptyMessage="--SELECT--" AutoPostBack="false">
                                                    </infs:WclComboBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvPermissionList" ControlToValidate="rblPermissionList"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Permission is required." ValidationGroup="grpPermission" />
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvCmbPermissionList" ControlToValidate="cmbPermissionList"
                                                            Display="Dynamic" ValidationGroup="grpPermission" CssClass="errmsg"
                                                            Text="Permission is required." />
                                                    </div>
                                                </div>
                                                <div title="Click the link to select a node." id="dvInstHierarchy" runat="server" visible="false">
                                                    <div class='sxlb' title="">
                                                        <span class="cptn">Hierarchy Nodes</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <a style="color: blue;" href="javascript:void(0)" id="lnkInstitutionHierarchyPB" runat="server" class="">Select Institution Hierarchy</a><br />
                                                        <asp:Label ID="lblInstitutionHierarchyPB" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarPermission" runat="server" GridMode="true" DefaultPanel="pnlAttr" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpPermission" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
        <asp:HiddenField runat="server" ID="hdnNoneOptionValue" Value="0" />
        <asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
        <asp:HiddenField ID="hdnInstitutionHierarchyPBLbl" runat="server" Value="" />
        <asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
        <asp:HiddenField ID="hdnSelectedTenantID" runat="server" Value="" />
        <asp:HiddenField ID="hdnInstNodeLabel" runat="server" Value="" />
    </div>
</div>


<script type="text/javascript">
    function checkNonePermission(sender, args) {
        var nonePermissionId = $jQuery("#" + "<%= hdnNoneOptionValue.ClientID %>").val();
        var cmbPermissionList = $find($jQuery("[id$=cmbPermissionList]")[0].id);
        var items = cmbPermissionList.get_items();

        if (args.get_item().get_value() == nonePermissionId) {
            for (var i = 0; i < cmbPermissionList.get_items().get_count(); i++) {

                if (args._item.get_checked()) {

                    if (items._array[i].get_value() != nonePermissionId) {
                        items._array[i].set_checked(false);
                        items.getItem(i).set_cssClass("rcbDisabled");
                        items.getItem(i).set_enabled(false);
                    }

                }
                else {
                    items.getItem(i).set_enabled(true);
                    items.getItem(i).set_cssClass("rcbDisabled");
                    items.getItem(i).set_cssClass("rcbItem");
                }
            }
        }


    }

    $jQuery(document).ready(function () {
        $jQuery(document).on('click', '[id$=lnkInstitutionHierarchyPB]', function (e) {
            if (!$jQuery(this).hasClass('disabled')) {
                //var win = $page.get_window();
                //var IsRequestFromAddRotationScreen = 'Yes';
                //var TempDIV = top.$window.get_radManager().getActiveWindow();//.get_popupElement();//.css('z-index','90001 !important');
                //if (TempDIV._popupElement && Sys.UI.DomElement.containsCssClass(TempDIV._popupElement, "rwMaximizedWindow")) {
                //    IsParentzIndex = TempDIV._popupElement.style.zIndex;
                //    IsParentMaximizedWindow = true;
                //}
                //else {
                //    IsParentMaximizedWindow = false;
                //    IsParentzIndex = 0;
                //}
                OpenInstitutionHierarchyPopupInsideGrid();
            }
        });
    });
    //UAT-4257
    var winopen = false;

    function OpenInstitutionHierarchyPopupInsideGrid() {

        //UAT-1843: Bug Fix: Institution Hierarchy popup not shown on top if this popup maximized


        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        var ScreenNameForPermission = "ScreenNameForPermissionReadOnly";

        var tenantId = $jQuery("[id$=hdnSelectedTenantID]").val();
        if (tenantId != "0" && tenantId != "") {
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmentProgmapNew]").val();
            //var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&DelemittedDeptPrgMapIds=" + DelemittedDeptPrgMapIds + "&ScreenName=" + screenName + "&ScreenNameForPermission=" + ScreenNameForPermission);
            //UAT-2364
            url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionHierarchyList.aspx?TenantId="
                + tenantId + "&DepartmentProgramId=" + DepartmentProgramId);

            var popupHeight = $jQuery(window).height() * (100 / 100);
            var win = $window.createPopup(url, {
                size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move,
                name: composeScreenWindowName, onclose: OnHierarhyClientClose
            });
            //if (IsParentMaximizedWindow && parseInt(IsParentzIndex) > 0) {
            //    if (parseInt(win._popupElement.style.zIndex) < parseInt(IsParentzIndex)) {
            //        win._popupElement.style.zIndex = parseInt(IsParentzIndex) + 3;
            //    }
            //}
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnHierarhyClientClose(oWnd, args) {
        //debugger;
        oWnd.remove_close(OnHierarhyClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {

                // debugger;
                $jQuery("[id$=hdnDepartmentProgmapNew]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnInstitutionHierarchyPBLbl]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstNodeIdNew]").val(arg.InstitutionNodeId);
                $jQuery("[id$=lblInstitutionHierarchyPB]")[0].innerHTML = arg.HierarchyLabel;

                if ($jQuery("[id$=hdnInstNodeLabel]").length > 0) {
                    $jQuery("[id$=hdnInstNodeLabel]").val(arg.HierarchyLabel);
                }

                ////Settings for Add/Edit Rotation (Services --> Manage Rotation)
                //if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnInstitutionNodeIds]").length > 0) {
                //    $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnInstitutionNodeIds]").val(arg.DepPrgMappingId);
                //}

                //if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnAgencyNodeId]").length > 0) {
                //    $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnAgencyNodeId]").val('');
                //}

                //if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_lblAgencyHierarchy]").length > 0) {
                //    $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_lblAgencyHierarchy]").text('');
                //}

                //if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnselectedRootNodeId]").length > 0) {
                //    $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnselectedRootNodeId]").val('');
                //}

                //if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnAgencyName]").length > 0) {
                //    $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnAgencyName]").val('');
                //}

                //if ($jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnHierarchyLabel]").length > 0) {
                //    $jQuery("[id$=ucAgencyHierarchyAddRotationMultiple_hdnHierarchyLabel]").val('');
                //}






                $jQuery("[id$=spnInsPre]").attr('class', 'reqd controlHidden');
            }
            winopen = false;
        }
    }
</script>
