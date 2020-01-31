<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationDepartment.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.LocationDepartment" %>

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

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">Map Department(s)</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
        </div>
    </div>

    <div id="dvLocationDepartment" class="row" style="margin-left: 10px; margin-right: 10px; width: auto; height: auto">
        <infs:WclGrid runat="server" ID="grdLocationDepartment" AllowPaging="true" AutoGenerateColumns="false" CssClass="gridhover"
            AllowSorting="true" AllowFilteringByColumn="true" AutoSkinMode="true" CellSpacing="0" GridLines="Both" EnableDefaultFeatures="true"
            ShowAllExportButtons="false" ShowExtraButtons="true" NonExportingColumns="DeleteColumn"
            OnNeedDataSource="grdLocationDepartment_NeedDataSource" OnItemCommand="grdLocationDepartment_ItemCommand" OnItemDataBound="grdLocationDepartment_ItemDataBound">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents />
                <%--OnRowDblClick="grd_rwDbClick"--%>
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="AgencyLocationDepartmentID" AllowFilteringByColumn="true">
                <CommandItemSettings ShowExportToExcelButton="false"
                    ShowExportToPdfButton="false" ShowExportToCsvButton="false" AddNewRecordText="Add New Department" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter Department column"
                        HeaderText="Department" SortExpression="Department" UniqueName="Department"
                        HeaderTooltip="This column displays the name of the department for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StudentTypes" FilterControlAltText="Filter StudentTypes column"
                        HeaderText="Student Types" SortExpression="StudentTypes" UniqueName="StudentTypes"
                        HeaderTooltip="This column displays the student types for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif"
                        CommandName="Delete" ConfirmText="Are you sure you want to delete this mapping ?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="~/Resources/Mod/Dashboard/images/editGrid.gif"
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
                                        <asp:Label ID="lblTitleLocationDepartmentMapping" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Department" : "Update Department" %>'
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
                            <asp:Panel runat="server" CssClass="editForm" ID="pnlAgencyLocationDepartmentMapping">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="form-group col-md-3">
                                                <span class="cptn">Department</span><span class='reqd'>*</span>
                                                <infs:WclComboBox ID="ddlDepartment" runat="server" DataTextField="DP_Name"
                                                    DataValueField="DP_ID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" 
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" AutoPostBack="false">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvDepartment" ControlToValidate="ddlDepartment"
                                                        InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                        Text="Department is required." />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Student Type</span><span class='reqd'>*</span>
                                                <infs:WclComboBox ID="cmbStudentType" runat="server" CheckBoxes="true" DataValueField="ST_ID" EnableCheckAllItemsCheckBox="true"
                                                    DataTextField="ST_Name" EmptyMessage="--SELECT--"
                                                    Filter="Contains" OnClientBlur="OnClientItemChecked" OnClientItemChecked="OnClientItemChecked" OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="false"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    <Localization CheckAllString="--Select All--"/>
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvStudentType" ControlToValidate="cmbStudentType"
                                                        Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit"
                                                        Text="Student type is required." />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="col-md-12 text-right">
                                <infsu:CommandBar ID="fsucCmdBarNode" runat="server" GridMode="true" GridInsertText="Save" GridUpdateText="Save"
                                    ValidationGroup="grpFormSubmit" ExtraButtonIconClass="icnreset" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False"></FilterMenu>
        </infs:WclGrid>
    </div>
</div>


<script type="text/javascript">

    //function grd_rwDbClick(s, e) {
    //    var _id = "btnEdit";
    //    var b = e.get_gridDataItem().findControl(_id);
    //    if (b && typeof (b.click) != "undefined") { b.click(); }
    //}
    function OnClientItemChecked(sender, args) {
        if (sender.get_checkedItems().length == 0) {
            sender.clearSelection();
            sender.set_emptyMessage("--SELECT--");
        }
    }
    function openCmbBoxOnTab(sender, e) {
        if (!sender.get_dropDownVisible()) sender.showDropDown();
    }
</script>

