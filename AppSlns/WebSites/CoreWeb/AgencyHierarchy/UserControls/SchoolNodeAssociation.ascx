<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SchoolNodeAssociation.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.UserControls.SchoolNodeAssociation" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rmpHierarchyControls">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~//Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Scripts/bootstrap.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid" tabindex="-1" id="dvschoolNodeAssociation" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">School Node Association</h2>
        </div>
    </div>
    <div class="row">
        <%--<div class="col-md-12">--%>
            <infs:WclGrid runat="server" ID="grdSchoolNodeAss" AllowCustomPaging="false" AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" NonExportingColumns="EditCommandColumn,DeleteColumn"
                OnDeleteCommand="grdSchoolNodeAss_DeleteCommand" OnNeedDataSource="grdSchoolNodeAss_NeedDataSource" OnItemCommand="grdSchoolNodeAss_ItemCommand"
                OnItemDataBound="grdSchoolNodeAss_ItemDataBound" EnableLinqExpressions="false" ShowClearFiltersButton="false" EnableAriaSupport="true">
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="TenantID, AgencyHierarchyID,CommaSeparatedDpmIds"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add School" ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="TenantName" HeaderText="School Name" SortExpression="TenantName"
                            UniqueName="TenantName" HeaderTooltip="This column displays the School Name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CommaSeparatedDpmlabel" HeaderText="Institution Node" SortExpression="CommaSeparatedDpmlabel"
                            UniqueName="CommaSeparatedDpmlabel" HeaderTooltip="This column displays the Institution Node for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsStudentShare" HeaderText="Is Student Share" SortExpression="IsStudentShare"
                            UniqueName="IsStudentShare" HeaderTooltip="This column displays the Profile share permission for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsAdminShare" HeaderText="Is Admin Share" SortExpression="IsAdminShare"
                            UniqueName="IsAdminShare" HeaderTooltip="This column displays the Profile share permission for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif" ItemStyle-Width="1%"
                            CommandName="Delete" ConfirmText="Are you sure you want to delete this node association?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" ItemStyle-Width="2%">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
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
                                            <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add School Node Association" : "Update School Node Association" %>'
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

                                <asp:Panel ID="pnlEditForm" runat="server">
                                    <div class="row bgLightGreen">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3' title="Select an Institution.">
                                                    <span class="cptn">Institution</span><span class="reqd">*</span>
                                                    <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" AutoPostBack="false"
                                                        DataValueField="TenantID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnClientSelectedIndexChanged="ResetHierarchyNodeSelection"
                                                        Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                                            InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmitSNA" CssClass="errmsg"
                                                            Text="Institution is required." />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-5'>
                                                    <span class="cptn">Hierarchy Nodes</span><span class='reqd'>*</span>
                                                    <a style="color: blue;" href="#" id="lnkInstitutionHierarchyPB" onclick="OpenInstitutionHierarchyPopupInsideGrid(false);">Select Institution Hierarchy</a><br />
                                                    <asp:Label ID="lblInstitutionHierarchyPB" runat="server"></asp:Label>
                                                </div>
                                                <div class='form-group col-md-4'>
                                                    <div class="col-md-12">
                                                        <span class="cptn">Allow Profile Share</span>
                                                    </div>
                                                    <div class="col-md-12">
                                                        <div class="col-md-6">
                                                            <asp:CheckBox ID="chkStudentProfileSharingPermission" Text="Student" runat="server" Width="100%" CssClass="form-control" Checked='<%# (Container is GridEditFormInsertItem) ? true: Convert.ToBoolean(Eval("IsStudentShare"))%>' />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:CheckBox ID="chkAdminProfileSharingPermission" Text="Admin" runat="server" Checked='<%#(Container is GridEditFormInsertItem) ? true: Convert.ToBoolean(Eval("IsAdminShare"))%>'
                                                                Width="100%" CssClass="form-control" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <infsu:CommandBar ID="fsucCmdBarAssociation" runat="server" GridMode="true"
                                    GridInsertText="Save" GridUpdateText="Save" SaveButtonIconClass="rbSave"
                                    ValidationGroup="grpFormSubmitSNA" UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
       <%-- </div>--%>
    </div>
</div>
<asp:Label runat="server" TabIndex="-1" ID="lblFocus"></asp:Label>
<asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
<asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
<asp:HiddenField ID="hdnDeptLabel" runat="server" Value="" />

<script type="text/javascript">

    var winopen = false;

    function OpenInstitutionHierarchyPopupInsideGrid(IsMappingScreen) {
        //UAT-1843: Bug Fix: Institution Hierarchy popup not shown on top if this popup maximized
        if (IsMappingScreen) {
            var win = $page.get_window();
            if (win) {
                win.restore();
            }
        }

        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        var tenantId = $find($jQuery("[id$=ddlTenant]").attr("id")).get_value();
        if (tenantId != "0" && tenantId != "") {
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmentProgmapNew]").val();
            //var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName="
            //                            + screenName + "&DepartmentProgramId=" + DepartmentProgramId);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName="
                                   + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var win = $window.createPopup(url, {
                size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move,
                name: composeScreenWindowName, onclose: OnHierarhyClientClose
            });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
            setTimeout(function () { $jQuery("[id$=dvschoolNodeAssociation]").focus() }, 100);
        }
        return false;
    }

    //function OnHierarhyClientClose(oWnd, args) {
    //    oWnd.remove_close(OnHierarhyClientClose);
    //    if (winopen) {
    //        var arg = args.get_argument();
    //        if (arg) {

    //            $jQuery("[id$=dvschoolNodeAssociation]").focus();
    //            //alert(arg.DepPrgMappingId);
    //            //alert(arg.InstitutionNodeId);

    //            $jQuery("[id$=hdnDepartmentProgmapNew]").val(arg.DepPrgMappingId);
    //            //$jQuery("[id$=hdnInstitutionHierarchyPBLbl]").val(arg.HierarchyLabel);
    //            $jQuery("[id$=hdnInstNodeIdNew]").val(arg.InstitutionNodeId);
    //            $jQuery("[id$=lblInstitutionHierarchyPB]")[0].innerHTML = arg.HierarchyLabel;
    //            $jQuery("[id$=hdnDeptLabel]").val(arg.HierarchyLabel);
    //        }
    //        winopen = false;
    //    }
    //}

    function ResetHierarchyNodeSelection() {
        $jQuery("[id$=hdnDepartmentProgmapNew]").val('');
        //$jQuery("[id$=hdnInstitutionHierarchyPBLbl]").val(arg.HierarchyLabel);
        $jQuery("[id$=hdnInstNodeIdNew]").val('');
        $jQuery("[id$=lblInstitutionHierarchyPB]")[0].innerHTML = '';
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
    function InstitutionHierarchyLabel() {
        setTimeout(function () {
            var InstNodeLabel = $jQuery("[id$=hdnInstitutionHierarchyPBLbl]").val();
            if (InstNodeLabel != '') {
                $jQuery($jQuery("[id$=lblInstitutionHierarchyPB]")[0]).text(InstNodeLabel);
            }
        }, 1000);
    }
</script>
