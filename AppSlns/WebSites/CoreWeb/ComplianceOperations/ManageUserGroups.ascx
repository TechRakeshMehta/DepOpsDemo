<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ManageUserGroups" CodeBehind="ManageUserGroups.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/ClientContact.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>
<style>
    .rbLinkButton {
        display: inline-block;
        height: 25px !important;
        line-height: none !important;
        position: relative;
        border: 1px solid;
        padding: 0 !important;
        cursor: pointer;
        vertical-align: bottom;
        text-decoration: none;
    }


    .height {
        height: auto !important;
    }

    .RadMenu .rmHorizontal .rmText {
        padding: 0 2px 1px 0;
    }

    .rmVertical.rmGroup.rmLevel1 {
        border: none;
    }

    .RadMenu_Default .rmGroup, .RadMenu_Default .rmMultiColumn, .RadMenu_Default .rmGroup .rmVertical {
    }

    .fa-download {
        margin-left: 8px;
    }
</style>
<style type="text/css">
    #menuDiv ul ul li:first-child {
        border-radius: 10px 10px 0px 0px;
    }

    #menuDiv ul ul {
        border-radius: 10px;
    }

        #menuDiv ul ul li:last-child {
            border-radius: 0px 0px 10px 10px;
        }

        #menuDiv ul ul li.rmFirst.rmLast {
            border-radius: 10px 10px 10px 10px;
        }

    .rmVertical.rmGroup {
        background: none;
        background-color: #4382c2;
        border-radius: 10px;
    }

    .btn {
        width: 100%;
        text-align: left;
    }

    .RadMenu .rmGroup .rmText {
        padding: 0px;
        margin: 0px;
    }

    .setZindex {
        z-index: 9 !important;
    }

    .ColConfigBtn {
        padding-bottom: 7px !important;
    }
    /*.RadMenu .rmItem .rmTemplate, .RadToolTip_Default .rtWrapper .rtWrapperTopRight, .RadToolTip_Default .rtWrapper .rtWrapperBottomLeft, .RadToolTip_Default .rtWrapper .rtWrapperBottomRight, .RadToolTip_Default .rtWrapper .rtWrapperTopCenter, .RadToolTip_Default .rtWrapper .rtWrapperBottomCenter, .RadToolTip_Default table.rtShadow .rtWrapperTopLeft, .RadToolTip_Default table.rtShadow .rtWrapperTopRight, .RadToolTip_Default table.rtShadow .rtWrapperBottomLeft, .RadToolTip_Default table.rtShadow .rtWrapperBottomRight, .RadToolTip_Default table.rtShadow .rtWrapperTopCenter, .RadToolTip_Default table.rtShadow .rtWrapperBottomCenter, .RadToolTip_Default .rtCloseButton {
        background-image: none !important;
    }*/
</style>

<script type="text/javascript">

    $page.add_pageLoad(function () {
        var $ = $jQuery;
        $(".grdCmdBar .RadButton").each(function () {
            if ($(this).text().toLowerCase() == "add new user group") {
                $(this).attr("title", "Click to add a new user group");
            }
        });

        $jQuery("[id$=ddlTenant_Input").attr('role', 'combobox');
        $jQuery('[id$=fsucCmdBarPriceAdjustment_btnGrd_input]').attr('title', 'Click to save/update user group');
        $jQuery('[id$=fsucCmdBarPriceAdjustment_btnCancel_input]').attr('title', 'Click to cancel');

    });
    function pageLoad() {



        //UAT-1955
        $jQuery("[id$=ddlTenant]").attr('tabindex', '0');

        if ($jQuery('[id$=hdnControlToSetFocus]').val() != "") {
            $jQuery('[id$=' + $jQuery('[id$=hdnControlToSetFocus]').val() + ']').attr("tabindex", 0).focus();
            $jQuery('[id$=hdnControlToSetFocus]').val("");
        }
    }

    function CheckAll(id) {
        var masterTable = $find("<%= grdUserGroup.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectUser").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdUserGroup.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }
    function OpenInstitutionHierarchyPopupInsideGrid() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        var tenantId = $find($jQuery("[id$=ddlTenant]").attr("id")).get_value();
        if (tenantId != "0" && tenantId != "") {
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmentProgmapNew]").val();
            var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionNodeHierarchyWithPermissions.aspx?TenantId=" + tenantId + "&ScreenName="
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

    function OpenInstitutionHierarchyPopupInsideGridFilter() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        var tenantId = $find($jQuery("[id$=ddlTenant]").attr("id")).get_value();
        if (tenantId != "0" && tenantId != "") {
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmentProgmapNewFilter]").val();

            //var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionNodeHierarchyWithPermissions.aspx?TenantId=" + tenantId + "&ScreenName="
            //    + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" +
                screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var popupHeight = $jQuery(window).height() * (100 / 100);


            var win = $window.createPopup(url, {
                size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move,
                name: composeScreenWindowName, onclose: OnHierarhyClientCloseFilter
            });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnHierarhyClientCloseFilter(oWnd, args) {
        oWnd.remove_close(OnHierarhyClientCloseFilter);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmentProgmapNewFilter]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
                //  $jQuery("[id$=hdnInstNodeIdNewFilter]").val(arg.InstitutionNodeId);
                // $jQuery("[id$=lblInstitutionHierarchyFilter]")[0].innerHTML = arg.HierarchyLabel;
                __doPostBack("<%= btnDoPostBack.ClientID %>", "");
              }
              winopen = false;
          }
      }
</script>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">Manage User Group
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
    <div class="row bgLightGreen">
        <asp:Panel runat="server" ID="paneTenant">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                        <%--<infs:WclComboBox ID="ddlTenant" EnableAriaSupport="true" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" CausesValidation="false" EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChange"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>--%>
                        <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName"
                            CausesValidation="false" AutoPostBack="true" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenant_SelectedIndexChange"
                            OnDataBound="ddlTenant_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Hierarchy Nodes</span>
                        <a style="color: blue;" href="#" id="lnkInstitutionHierarchyFilter" onclick="OpenInstitutionHierarchyPopupInsideGridFilter(false);">Select Institution Hierarchy</a><br />
                        <asp:Label ID="lblInstitutionHierarchyFilter" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Select &#34All&#34 to view all user groups  or &#34Archived&#34 to view only archived user groups or &#34Active&#34 to view only non archived user groups">
                        <span class="cptn">Archive State</span>
                        <asp:RadioButtonList ID="rbUserGroupState" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbUserGroupState_SelectedIndexChanged"
                            DataTextField="rbText" DataValueField="rbValue" CssClass="radio_list" AutoPostBack="true">
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-3">
                <div class="row">
                    <%--<asp:CheckBox ID="chkSelectAllResults" Text="Select All Results" runat="server" OnCheckedChanged="chkSelectAllResults_CheckedChanged" AutoPostBack="true"
                        Width="100%" CssClass="form-control" />--%>
                </div>
            </div>
            <div class="form-group col-md-9">
                <div class="row" id="trailingText">
                    <%--            <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel,Clear,Extra"
                        AutoPostbackButtons="Submit,Save,Cancel,Clear,Extra" SubmitButtonIconClass="rbUndo" SubmitButtonText="Reset"
                        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
                        OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click"
                        ClearButtonText="Send Message" OnClearClick="btnSendMessage_Click" ExtraButtonText="Passport Report" OnExtraClick="btnViewReport_Click"
                        ClearButtonIconClass="rbEnvelope" ExtraButtonIconClass="rbPassport" UseAutoSkinMode="false" ButtonSkin="Silk">
                                  <ExtraCommandButtons>
                                  <infs:WclButton ID="btnArchieve" runat="server" Text="Archive" Enabled="true" OnClick="btnArchieve_Click" AutoSkinMode="false" Skin="Silk">
                                <Icon PrimaryIconCssClass="rbArchive"></Icon>
                            </infs:WclButton>
                        </ExtraCommandButtons>
                    </infsu:CommandBar>--%>

                    <div id="menuDiv" runat="server">
                        <infs:WclMenu ID="cmd" runat="server" Skin="Default" AutoSkinMode="false" CssClass="setZindex">
                            <Items>
                                <telerik:RadMenuItem Text="Searchmun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Search" ID="btnSearch" Icon-PrimaryIconCssClass="rbSearch" OnClick="CmdBarSearch_Click" ToolTip="Click to search orders per the criteria entered above" Skin="Silk" AutoSkinMode="false" ButtonPosition="Center">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>
                                <telerik:RadMenuItem Text="Resetmun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Reset" ID="btnReset" Icon-PrimaryIconCssClass="rbUndo" OnClick="CmdBarReset_Click" ToolTip="Click to remove all values entered in the search criteria above" Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" CausesValidation="false" CssClass="btn">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>

                                <telerik:RadMenuItem Text="Cancelmun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Cancel" ID="btnCancel" Icon-PrimaryIconCssClass="rbCancel" OnClick="CmdBarCancel_Click" ToolTip="Click to cancel. Any data entered will not be saved" Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" CssClass="btn" CausesValidation="false">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>

                                <telerik:RadMenuItem Text="Archivemun">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Archive" ID="btnArchive" Icon-PrimaryIconCssClass="rbArchive" CssClass="btn"
                                            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnArchieve_Click">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                    <Items>
                                        <telerik:RadMenuItem>
                                            <ItemTemplate>
                                                <infs:WclButton runat="server" Text="UnArchive" ID="btnUnArchive" Icon-PrimaryIconCssClass="rbArchive" CssClass="btn"
                                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnUnArchive_Click">
                                                </infs:WclButton>
                                            </ItemTemplate>
                                        </telerik:RadMenuItem>
                                    </Items>
                                </telerik:RadMenuItem>
                            </Items>
                        </infs:WclMenu>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div id="dvUserGroup" runat="server">
            <infs:WclGrid runat="server" ID="grdUserGroup" AllowPaging="True" PageSize="10" AutoGenerateColumns="False"
                AllowSorting="True" GridLines="Both" ShowAllExportButtons="False" NonExportingColumns="EditCommandColumn, DeleteColumn"
                ValidationGroup="grpValdUserGroup" OnNeedDataSource="grdUserGroup_NeedDataSource"
                OnItemCommand="grdUserGroup_ItemCommand" OnInsertCommand="grdUserGroup_InsertCommand"
                OnUpdateCommand="grdUserGroup_UpdateCommand" OnDeleteCommand="grdUserGroup_DeleteCommand"
                OnItemDataBound="grdUserGroup_ItemDataBound">
                <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true" ClientEvents-OnGridCreated="onGridCreated">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="UG_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New User Group"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="SelectUsers" HeaderTooltip="Click this box to select all users on the active page"
                            AllowFiltering="false" ShowFilterIcon="false" ItemStyle-Width="10px">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectUser" runat="server"
                                    onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectUser_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="UG_Name" FilterControlAltText="Filter UG_Name column"
                            HeaderText="Name" SortExpression="UG_Name" UniqueName="UG_Name" HeaderTooltip="This column displays the name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UG_Description" FilterControlAltText="Filter UG_Description column"
                            HeaderText="Description" SortExpression="UG_Description" UniqueName="UG_Description"
                            HeaderTooltip="This column displays the description for each record in the grid">
                        </telerik:GridBoundColumn>
                        <%-- <telerik:GridBoundColumn HeaderStyle-Width="50 %" DataField="HierarchyNodeLabelList" FilterControlAltText="Filter UG_Description column"
                            HeaderText="Hierarchy Nodes" SortExpression="HierarchyNodeLabelList" UniqueName="HierarchyNodeLabelList"
                            HeaderTooltip="This column displays the description for each record in the grid">
                        </telerik:GridBoundColumn>--%>
                        <%--<telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Wrap="false" UniqueName="ManageQueue">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlManageQueue" Text="Manage Queue" NavigateUrl="#" Visible="false" runat="server"></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif"
                            ConfirmText="Are you sure you want to delete?" Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                            UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div runat="server" id="divEditBlock" visible="true">
                                <div class="col-md-12">
                                    <div class="row bgLightGreen">
                                        <div class="col-md-12">
                                            <h2 class="header-color" tabindex="0">
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
                                <asp:Panel runat="server" ID="pnlPriceAdjustment">
                                    <div class="col-md-12">
                                        <div class="row bgLightGreen">
                                            <div class='form-group col-md-3' title="Enter the desired name for the new user group">
                                                <label id="lblName" class="cptn">Name</label><span class="reqd">*</span>
                                                <infs:WclTextBox ID="txtUserGroupeName" EnableAriaSupport="true" aria-labelledby="lblName" Width="100%" runat="server" Text='<%# Eval("UG_Name") %>'
                                                    MaxLength="50" CssClass="form-control" AutoSkinMode="false" Skin="Silk" aria-describedby="rfvLabel">
                                                </infs:WclTextBox>
                                                <div id="dvLabel" class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvLabel" ControlToValidate="txtUserGroupeName"
                                                        class="errmsg" ErrorMessage="Name is required." ValidationGroup='grpValdUserGroup' SetFocusOnError="true"
                                                        Enabled="true" />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-6' title="Enter a description of the user group">
                                                <label id="lblDescription" class="cptn">Description</label>
                                                <infs:WclTextBox EnableAriaSupport="true" aria-labelledby="lblDescription" Width="100%" ID="txtUserGroupeDescription" runat="server" Text='<%# Eval("UG_Description") %>'
                                                    MaxLength="250" CssClass="form-control" AutoSkinMode="false" Skin="Silk">
                                                </infs:WclTextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
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
                                    <infsu:CommandBar ID="fsucCmdBarPriceAdjustment" runat="server" GridMode="true" DefaultPanel="pnlPriceAdjustment"
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
    </div>
    <asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
    <asp:HiddenField ID="hdnControlToSetFocus" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmentProgmapNewFilter" runat="server" Value="" />
    <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
</div>
