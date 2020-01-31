<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageInvoiceGroups.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ManageInvoiceGroups" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" ResourceType="JavaScript" />
    
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script>
  

    function grdInvoiceGroups_rwDbClick(s, e) {
        var _id = "btnEdit";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }
</script>
<div id="dvTop" class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Invoice Groups
            </h2>
        </div>
    </div>
    <div class="row allowscroll">
        <div id="dvGrdInvoiceGroups" runat="Server">
            <infs:WclGrid runat="server" ID="grdInvoiceGroups" AllowCustomPaging="false" AutoGenerateColumns="False"
                AllowSorting="true" AllowFilteringByColumn="true" AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
                OnNeedDataSource="grdInvoiceGroups_NeedDataSource" OnItemDataBound="grdInvoiceGroups_ItemDataBound"
                OnItemCreated="grdInvoiceGroups_ItemCreated" OnItemCommand="grdInvoiceGroups_ItemCommand" EnableLinqExpressions="false"
                ShowClearFiltersButton="true">
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grdInvoiceGroups_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>

                <MasterTableView CommandItemDisplay="Top" DataKeyNames="InvoiceGroupID,InstitutionIDs,InstitutionHierarchyIDs,ReportColumnIDs"
                    AllowFilteringByColumn="true">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Invoice Group" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="InvoiceGroupName" HeaderText="Group Name" SortExpression="InvoiceGroupName"
                            HeaderTooltip="This column displays the Group name for each record in the grid"
                            UniqueName="InvoiceGroupName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="InstitutionNames" HeaderText="Institution(s)" SortExpression="InstitutionNames"
                            UniqueName="InstitutionNames" HeaderTooltip="This column displays the Institution(s) for each record in the grid">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="InstitutionHierarchyLabels" HeaderText="Hierarchy Nodes" SortExpression="InstitutionHierarchyLabels"
                            UniqueName="InstitutionHierarchyLabels" HeaderTooltip="This column displays the Hierarchy Nodes for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ReportColumnNames" HeaderText="Report Columns" SortExpression="ReportColumnNames"
                            UniqueName="ReportColumnNames" HeaderTooltip="This column displays the Report Columns for each record in the grid">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif"
                            CommandName="Delete" ConfirmText="Are you sure you want to delete this invoice group?"
                            Text="Delete" UniqueName="DeleteGroup">
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                            UniqueName="EditGroup">
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
                                            <asp:Label ID="lblInvoiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Invoice Group" : "Update Invoice Group" %>'
                                                runat="server" />
                                        </h2>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlEditForm" runat="server">
                                    <div class="row bgLightGreen">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Invoice Group Name</span><span class='reqd'>*</span>
                                                    <infs:WclTextBox ID="txtInvoiceGroupeName" runat="server" Text='<%# Eval("InvoiceGroupName") %>'
                                                        Width="100%" CssClass="form-control">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvGroupName" ControlToValidate="txtInvoiceGroupeName"
                                                            Display="Dynamic" CssClass="errmsg"
                                                            Text="Invoice Group Name is required" ValidationGroup="grpFormSubmit" />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn">Invoice Group Description</span>
                                                    <infs:WclTextBox ID="txtInvoiceGroupDescription" runat="server" Text='<%# Eval("InvoiceGroupDescription") %>'
                                                        Width="100%" CssClass="form-control">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='form-group col-md-3' title="Select Report Columns">
                                                    <span class="cptn lineHeight">Report Column(s)</span><span class="reqd">*</span>
                                                    <infs:WclComboBox ID="cmbReportColumn" runat="server" DataTextField="RC_Name" DataValueField="RC_ID"
                                                        AutoPostBack="false" Width="100%" CssClass="form-control" EmptyMessage="--SELECT--" Skin="Silk" AutoSkinMode="false"
                                                        EnableCheckAllItemsCheckBox="true" CheckBoxes="true">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:CustomValidator ID="rfvReportColumns" ErrorMessage="Report Column is required" ValidateEmptyText="true"
                                                            ClientValidationFunction="ValidateCheckBoxSelectionReportColumns" ControlToValidate="cmbReportColumn"
                                                            CssClass="errmsg"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" runat="server">
                                                        </asp:CustomValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3' title="Select the Institution">
                                                    <span class="cptn">Institution(s)</span><span class="reqd">*</span>
                                                    <infs:WclComboBox ID="cmbTenant" Width="100%" CssClass="form-control" runat="server" DataTextField="TenantName"
                                                        DataValueField="TenantID" EmptyMessage="--SELECT--" Skin="Silk" AutoSkinMode="false"
                                                        EnableCheckAllItemsCheckBox="true" CheckBoxes="true" AutoPostBack="false">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:CustomValidator ID="rfvTenants" ErrorMessage="Institution is required" ValidateEmptyText="true"
                                                            ClientValidationFunction="ValidateCheckBoxSelection" ControlToValidate="cmbTenant"
                                                            CssClass="errmsg"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" runat="server"></asp:CustomValidator>
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class="cptn" style="color: transparent !important; display: block;"></span>
                                                    <infs:WclButton ID="btnLoadNodes" ButtonType="StandardButton" runat="server"
                                                        AutoPostBack="true" OnClick="btnLoadNodes_Click"
                                                        Text="Load Nodes" ButtonPosition="Center" ValidationGroup=""
                                                        CssClass="redBtn">
                                                    </infs:WclButton>
                                                </div>
                                                <div class='form-group col-md-6' title="Select Hierarchy Nodes">
                                                    <span class="cptn lineHeight">Hierarchy Node(s)</span>
                                                    <infs:WclComboBox ID="cmbNode" runat="server" DataTextField="NodeLabel" DataValueField="NodeValue"
                                                        AutoPostBack="false" Width="100%" CssClass="form-control" EmptyMessage="--SELECT--" Skin="Silk" AutoSkinMode="false"
                                                        EnableCheckAllItemsCheckBox="true" CheckBoxes="true">
                                                    </infs:WclComboBox>
                                                    <%--<div class="vldx">
                                                        <asp:CustomValidator ID="rfvNodes" ErrorMessage="Hierarchy Node is required" ValidateEmptyText="true"
                                                            ClientValidationFunction="ValidateCheckBoxSelectionNodes" ControlToValidate="cmbNode"
                                                            CssClass="errmsg" Display="Dynamic" ValidationGroup="grpFormSubmit" runat="server"></asp:CustomValidator>
                                                    </div>--%>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <infsu:CommandBar ID="fsucCmdBarRotation" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                    GridInsertText="Save" GridUpdateText="Save"
                                    ValidationGroup="grpFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                        Position="TopAndBottom" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>
</div>

<script type="text/javascript">

    function ValidateCheckBoxSelection(source, args) {
        var cntrlToValidate = $find($jQuery("[id$=cmbTenant]").attr("id"));
        var check = 0;
        if (cntrlToValidate) {
            var cntrlItems = cntrlToValidate.get_items();
            for (var i = 0; i <= cntrlItems.get_count() - 1; i++) {
                var cntrlItem = cntrlItems.getItem(i);
                if (cntrlItem.get_checked()) {
                    check = 1;
                }
            }
        }
        if (check)
            args.IsValid = true;
        else
            args.IsValid = false;
    }

    function ValidateCheckBoxSelectionNodes(source, args) {
        var cntrlToValidate = $find($jQuery("[id$=cmbNode]").attr("id"));
        var check = 0;
        if (cntrlToValidate) {
            var cntrlItems = cntrlToValidate.get_items();
            for (var i = 0; i <= cntrlItems.get_count() - 1; i++) {
                var cntrlItem = cntrlItems.getItem(i);
                if (cntrlItem.get_checked()) {
                    check = 1;
                }
            }
        }
        if (check)
            args.IsValid = true;
        else
            args.IsValid = false;
    }

    function ValidateCheckBoxSelectionReportColumns(source, args) {
        var cntrlToValidate = $find($jQuery("[id$=cmbReportColumn]").attr("id"));
        var check = 0;
        if (cntrlToValidate) {
            var cntrlItems = cntrlToValidate.get_items();
            for (var i = 0; i <= cntrlItems.get_count() - 1; i++) {
                var cntrlItem = cntrlItems.getItem(i);
                if (cntrlItem.get_checked()) {
                    check = 1;
                }
            }
        }
        if (check)
            args.IsValid = true;
        else
            args.IsValid = false;
    }
</script>
