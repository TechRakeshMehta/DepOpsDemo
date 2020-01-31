<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserGroupMapping.aspx.cs" MasterPageFile="~/Shared/ChildPage.master" Inherits="CoreWeb.ComplianceOperations.Views.UserGroupMapping" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxUserGroupMappingPopup">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="../Resources/Generic/popup.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <script type="text/javascript">

        $page.add_pageLoad(function () {
            var $ = $jQuery;
            $(".grdCmdBar .RadButton").each(function () {
                if ($(this).text().toLowerCase() == "add new user group") {
                    $(this).attr("title", "Click to add a new user group");
                }
            });
        });

    </script>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">User Group Mapping
                </h2>
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
        <div class="row allowscroll">
            <infs:WclGrid runat="server" ID="grdUserGroup" AllowPaging="True" PageSize="10" AutoGenerateColumns="False"
                AllowSorting="True" GridLines="Both" ShowAllExportButtons="False" NonExportingColumns="EditCommandColumn, DeleteColumn"
                ValidationGroup="grpValdUserGroup" AllowFilteringByColumn="false" ShowClearFiltersButton="true" OnNeedDataSource="grdUserGroup_NeedDataSource"
                OnItemCommand="grdUserGroup_ItemCommand" OnInsertCommand="grdUserGroup_InsertCommand"
                OnItemDataBound="grdUserGroup_ItemDataBound" Height="70%">
                <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                    <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="UG_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New User Group"
                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false" ShowRefreshButton="true"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="AssignItems" HeaderTooltip="Click this box to select all user groups on the active page"
                            AllowFiltering="false" ShowFilterIcon="false">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectItem" runat="server"
                                    onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectItem_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="UG_Name" FilterControlAltText="Filter UG_Name column"
                            HeaderText="Name" SortExpression="UG_Name" UniqueName="UG_Name" HeaderTooltip="This column displays the name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UG_Description" FilterControlAltText="Filter UG_Description column"
                            HeaderText="Description" SortExpression="UG_Description" UniqueName="UG_Description"
                            HeaderTooltip="This column displays the description for each record in the grid">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div runat="server" id="divEditBlock" visible="true">
                                <div class="col-md-12">
                                    <div class="row bgLightGreen">
                                        <div class="col-md-12">
                                            <h2 class="header-color">
                                                <asp:Label ID="lblTitlePriceAdjustment" Text='<%# (Container is GridEditFormInsertItem) ? "Add New User Group" : "Update User Group" %>'
                                                    runat="server" /></h2>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                        <infs:WclTextBox runat="server" Text='<%# Eval("UG_ID") %>' ID="txtUserGroupeId"
                                            Visible="false" CssClass="form-control" AutoSkinMode="false" Skin="Silk" Width="100%">
                                        </infs:WclTextBox>
                                    </div>
                                </div>
                                <asp:Panel runat="server" ID="pnlUserGroup">
                                    <div class="col-md-12">
                                        <div class="row bgLightGreen">
                                            <div class='form-group col-md-3 col-sm-3' title="Enter the desired name for the new user group">
                                                <span class="cptn">Name</span><span class="reqd">*</span>
                                                <infs:WclTextBox ID="txtUserGroupeName" Width="100%" runat="server" Text='<%# Eval("UG_Name") %>'
                                                    MaxLength="50" CssClass="form-control" AutoSkinMode="false" Skin="Silk">
                                                </infs:WclTextBox>
                                                <div id="dvLabel" class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvLabel" ControlToValidate="txtUserGroupeName"
                                                        class="errmsg" ErrorMessage="Name is required." ValidationGroup='grpValdUserGroup'
                                                        Enabled="true" />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-6 col-sm-6' title="Enter a description of the user group">
                                                <span class="cptn">Description</span>
                                                <infs:WclTextBox Width="100%" ID="txtUserGroupeDescription" runat="server" Text='<%# Eval("UG_Description") %>'
                                                    MaxLength="250" CssClass="form-control" AutoSkinMode="false" Skin="Silk">
                                                </infs:WclTextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <%--UAT-3381--%>
                                        <div class="row">
                                            <div class='form-group col-md-12'>
                                                <span class="cptn">Hierarchy Nodes</span><span class='reqd'>*</span>
                                                <a style="color: blue;" href="#" id="lnkInstitutionHierarchyPB" onclick="OpenInstitutionHierarchyPopupInsideGrid(false);">Select Institution Hierarchy</a><br />
                                                <asp:Label ID="lblInstitutionHierarchyPB" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="col-md-12">
                                <div class="row">&nbsp;</div>
                                <div class="row">
                                    <infsu:CommandBar ID="fsucCmdBarUserGroup" runat="server" GridMode="true" DefaultPanel="pnlUserGroup"
                                        GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpValdUserGroup" UseAutoSkinMode="False" ButtonSkin="Silk">
                                    </infsu:CommandBar>
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div class="row">
            <div class="col-md-12">
                &nbsp;
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group col-md-9 pull-right">
                    <div class="row text-right" id="trailingText">
                        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Right" DisplayButtons="Submit,Save,Cancel"
                            AutoPostbackButtons="Submit,Save" SaveButtonText="Assign" SubmitButtonText="Unassign" SaveButtonIconClass="rbAssign"
                            SubmitButtonIconClass="rbUnassign" CancelButtonText="Cancel" OnSubmitClick="CmdBarUnassign_Click" OnSaveClick="CmdBarAssign_Click"
                            OnCancelClientClick="ClosePopup" UseAutoSkinMode="false" ButtonSkin="Silk">
                        </infsu:CommandBar>
                    </div>
                </div>
            </div>
        </div>
        <%--UAT-3381--%>
        <asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
        <asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
        <asp:HiddenField ID="hdnControlToSetFocus" runat="server" Value="" />
    </div>

    <script type="text/javascript">

        function CheckAll(id) {
            var masterTable = $find("<%= grdUserGroup.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            var isChecked = false;
            if (id.checked == true) {
                var isChecked = true;
            }
            for (var i = 0; i < row.length; i++) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled == true)) {
                    masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
                }
            }
        }

        function UnCheckHeader(id) {
            var checkHeader = true;
            var masterTable = $find("<%= grdUserGroup.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            for (var i = 0; i < row.length; i++) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                    if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                        checkHeader = false;
                        break;
                    }
                }
            }
            $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
        }


        function show_progress_OnSubmit() {
            Page.showProgress('Processing...');
        }

        //function to get current popup window
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        //Function to close popup window
        function ClosePopup() {
            var oArg = {};
            oArg.Action = "Cancel";
            top.$window.get_radManager().getActiveWindow().close();
        }

        //Function to redirect to parent 
        function RedirectToParent() {
            var oArg = {};
            oArg.Action = "Submit";
            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }


        //UAT-3381
        function OpenInstitutionHierarchyPopupInsideGrid() {
            var composeScreenWindowName = "Institution Hierarchy";
            var screenName = "CommonScreen";
            var tenantId = '<%= SelectedTenantID %> ';
            if (tenantId != "0" && tenantId != "") {
                var DepartmentProgramId = $jQuery("[id$=hdnDepartmentProgmapNew]").val();
                var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionNodeHierarchyWithPermissions.aspx?TenantId=" + tenantId + "&ScreenName="
                                            + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);

                var popupHeight = $jQuery(window).height() * (100 / 100);

                var win = $window.createPopup(url, {
                    size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move,
                    name: composeScreenWindowName, onclose: OnHierarhyClientClose
                });
                winopen = true;
            }
            else {
                $alert("Please select Institution.");
            }
            return false;
        }

        function OnHierarhyClientClose(oWnd, args) {
            oWnd.remove_close(OnHierarhyClientClose);
            if (winopen) {
                var arg = args.get_argument();
                if (arg) {
                    $jQuery("[id$=hdnDepartmentProgmapNew]").val(arg.DepPrgMappingId);
                    $jQuery("[id$=hdnInstitutionHierarchyPBLbl]").val(arg.HierarchyLabel);
                    $jQuery("[id$=hdnInstNodeIdNew]").val(arg.InstitutionNodeId);
                    $jQuery("[id$=lblInstitutionHierarchyPB]")[0].innerHTML = arg.HierarchyLabel;
                }
                winopen = false;
            }
        }


    </script>

</asp:Content>
