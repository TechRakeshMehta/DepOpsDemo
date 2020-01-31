<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagePackageBundle.ascx.cs" Inherits="CoreWeb.CommonOperations.Views.ManagePackageBundle" %>

<%@ Register Src="~/CommonOperations/ManageInstituteHierarchyPackage.ascx" TagPrefix="uc" TagName="ManageInstituteHierarchyPackage" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/CommonOperations/ManageInstituteHierarchyPackage.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/CommonOperations/InstituteHierarchyPackage.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script type="text/javascript">
    var winopen = false;
    var minDate = new Date("01/01/1980");

    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "OrderQueue";
        //var tenantId = 2;
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var win = $window.createPopup(url, {
                size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move,
                name: composeScreenWindowName, onclose: OnClientClose
            });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
                $jQuery("[id$=lblInstituteHierarchyName]")[0].innerHTML = arg.HierarchyLabel;
            }
            winopen = false;
        }
    }

    //for Adding new Bundle------------------------------------
    function OpenInstitutionHierarchyPopupForAddPackageBundle() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "OrderQueue";
        var tenantId = $find($jQuery("[id$=ddlTenantNameNew]").attr("id")).get_value();
        if (tenantId != "0" && tenantId != "") {
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmentProgmapNew]").val();
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName="
                                        + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var win = $window.createPopup(url, {
                size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move,
                name: composeScreenWindowName, onclose: OnHierarhyClientClose
            });
            winopen = true;
        }
        else {
            $alert("Please select Institution for adding a Bundle.");
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
    function InstitutionHierarchyLabel() {
        setTimeout(function () {
            var InstNodeLabel = $jQuery("[id$=hdnInstitutionHierarchyPBLbl]").val();
            if (InstNodeLabel != '') {
                $jQuery($jQuery("[id$=lblInstitutionHierarchyPB]")[0]).text(InstNodeLabel);
            }
        }, 1000);
    }


</script>
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnTenantIdNew" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmentProgmapNew" runat="server" Value="" />
<asp:HiddenField ID="hdnInstNodeIdNew" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionHierarchyPBLbl" runat="server" Value="" />

<div class="section">
    <h1 class="mhdr">Manage Package Bundle
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Select the Institution whose data you want to view.">
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                    </div>

                    <div class='sxlm'>
                        <infs:WclComboBox ID="ddlTenantName" runat="server" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="true">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="ddlTenantName"
                                Display="Dynamic" InitialValue="--Select--" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Bundle Name</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtBundleSrch" runat="server">
                        </infs:WclTextBox>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Hierarchy Nodes</span>
                    </div>
                    <div class='sxlm m2spn'>
                        <a href="#" id="lnkInstituteHierarchySearch" onclick="openPopUp();">Select Institution Hierarchy</a><br />
                        <asp:Label ID="lblInstituteHierarchyName" runat="server"></asp:Label>
                        <div class="vldx">
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset" OnSubmitClick="Submit_Click"
            SaveButtonText="Search" SaveButtonIconClass="rbSearch" OnCancelClick="btnCancel_Click" OnSaveClick="Search_Click" CancelButtonText="Cancel"
            ValidationGroup="grpFormSubmit" DefaultPanelButton="Save" DefaultPanel="pnlTenant">
        </infsu:CommandBar>
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdPackageBundle" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0" OnNeedDataSource="grdPackageBundle_NeedDataSource"
                EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false" OnItemDataBound="grdPackageBundle_ItemDataBound"
                OnInsertCommand="grdPackageBundle_InsertCommand" OnDeleteCommand="grdPackageBundle_DeleteCommand" OnUpdateCommand="grdPackageBundle_UpdateCommand"
                GridLines="both" EnableLinqExpressions="false">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BundleId" AllowFilteringByColumn="True">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Bundle" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <%--<telerik:GridBoundColumn DataField="BundleId" FilterControlAltText="Filter ItemName column"
                            HeaderText="BundleId" SortExpression="Name" UniqueName="Name">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="BundleName" FilterControlAltText="Filter BundleName column"
                            HeaderText="Bundle Name" SortExpression="BundleName" UniqueName="BundleName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TrackingPackage" ItemStyle-CssClass="breakword" FilterControlAltText="Filter TrackingPackage column"
                            HeaderText="Immunization Package" SortExpression="TrackingPackage" UniqueName="TrackingPackage">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AdministrativePackage" ItemStyle-CssClass="breakword" FilterControlAltText="Filter AdministrativePackage column"
                            HeaderText="Administrative Package" SortExpression="AdministrativePackage" UniqueName="AdministrativePackage">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ScreeningPackage" ItemStyle-CssClass="breakword" FilterControlAltText="Filter ScreeningPackage column"
                            HeaderText="Screening Package(s)" SortExpression="ScreeningPackage" UniqueName="ScreeningPackage">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HierarchyNodes" FilterControlAltText="Filter HierarchyNodes column"
                            HeaderStyle-Width="30%"
                            HeaderText="Institute Hierarchy" SortExpression="HierarchyNodes" UniqueName="HierarchyNodes">
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
                            <div class="section" id="divAddForm" runat="server" visible="true">
                                <h1 class="mhdr">Add Bundle</h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlItem">
                                            <div class="msgbox">
                                                <asp:Label ID="lblGridMessage" runat="server">
                                                </asp:Label>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Institution</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlTenantNameNew" runat="server" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                        DataTextField="TenantName" AutoPostBack="true" DataValueField="TenantID"
                                                        OnSelectedIndexChanged="ddlTenantNameNew_SelectedIndexChanged">
                                                    </infs:WclComboBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="ddlTenantNameNew"
                                                            Display="Dynamic" InitialValue="--Select--" CssClass="errmsg" Text="Institution is required."
                                                            ValidationGroup="grpBundleSubmit" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Package Bundle Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtBundleNameNew" MaxLength="100">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvItemName" ControlToValidate="txtBundleNameNew"
                                                            class="errmsg" Display="Dynamic" ValidationGroup="grpBundleSubmit" ErrorMessage="Bundle Name is required." />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Package Bundle Label</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtLabel" MaxLength="100">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Is Available For Order</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:RadioButtonList ID="rdAvailableforOrder" RepeatDirection="Horizontal" runat="server">
                                                        <asp:ListItem Value="True">Yes</asp:ListItem>
                                                        <asp:ListItem Value="False">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Hierarchy Nodes</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm m3spn'>
                                                    <a style="color: blue;" href="#" id="lnkInstitutionHierarchyPB" onclick="OpenInstitutionHierarchyPopupForAddPackageBundle();">Select Institution Hierarchy</a><br />
                                                    <asp:Label ID="lblInstitutionHierarchyPB" runat="server"></asp:Label>

                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Immunization Package</span>
                                                </div>
                                                <div class='sxlm m3spn'>
                                                    <%-- <infs:WclComboBox ID="ddlTrackingPackages" runat="server" AutoPostBack="false">
                                                    </infs:WclComboBox>--%>
                                                    <uc:ManageInstituteHierarchyPackage runat="server" ID="ucManageInstituteHierarchyImmunizationPackage" />
                                                </div>

                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Administrative Package</span>
                                                </div>
                                                <div class='sxlm m3spn'>
                                                    <%--           <infs:WclComboBox ID="ddlAdmPackage" runat="server" AutoPostBack="false">
                                                    </infs:WclComboBox>--%>
                                                    <uc:ManageInstituteHierarchyPackage runat="server" ID="ucManageInstituteHierarchyAdministrativePackage" />
                                                </div>

                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Screening Package(s)</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <%--                   <infs:WclComboBox ID="ddlscreeningpackage" CheckBoxes="true" runat="server" AutoPostBack="false"
                                                        EmptyMessage="--Select--">
                                                    </infs:WclComboBox>--%>
                                                    <uc:ManageInstituteHierarchyPackage runat="server" ID="ucManageInstituteHierarchyScreeningPackage" />
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <infs:WclTextBox runat="server" ID="txtDescription" Height="60" MaxLength="500">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='vldx'>
                                                    <asp:CustomValidator runat="server" ID="cstValEditorDescription" ControlToValidate="txtDescription"
                                                        class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 200 characters." />
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Explanatory Notes</span>
                                                </div>
                                                <div class='sxlm m2spn'>
                                                    <div class="content" style="height: 405px;">
                                                        <infs:WclEditor ID="rdEditorNotes" runat="server" ClientIDMode="Static" Width="99.5%" StripFormattingOptions="All"
                                                            ToolsFile="~/Templates/Data/Tools.xml" EnableResize="false" OnClientLoad="OnClientLoad">
                                                        </infs:WclEditor>
                                                    </div>
                                                    <div class='vldx'>
                                                        <asp:CustomValidator runat="server" ID="cstValEditorNotes" ControlToValidate="rdEditorNotes"
                                                            ClientValidationFunction="ValidatePackageBundleDetailLength" ValidationGroup="grpBundleSubmit"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 500 characters." />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarPermission" runat="server" GridMode="true" DefaultPanel="pnlItem" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpBundleSubmit" />
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
    </div>
</div>

<script type="text/javascript" language="javascript">
    function OnClientLoad(editor, args) {
        $jQuery('ul.reToolbar').width('auto');
    }
</script>
