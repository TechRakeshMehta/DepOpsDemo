<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RotationVerificationUserWorkQueue.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.RotationVerificationUserWorkQueue" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchy" Src="~/AgencyHierarchy/UserControls/AgencyHierarchySelection.ascx" %>
<%--UAT-3245--%>

<infs:WclResourceManagerProxy runat="server" ID="rprxRotationVerificationUserWorkQueue">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 runat="server" id="header" class="header-color">Requirement Verification User Work Queue</h2>
        </div>
    </div>
    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" AutoPostBack="true" ChangeTextOnKeyBoardNavigation="true"
                                DataValueField="TenantID" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" OnClientDropDownClosed="TenantDropDownClosed"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CausesValidation="true"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" EmptyMessage="--SELECT--">
                            </infs:WclComboBox>
                            <asp:HiddenField ID="hdnPreviousTenantIds" Value="" runat="server" />
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                        <%--UAT-3245--%>
                        <div class='form-group col-md-3' title="Select a Agency whose data you want to view.">
                            <div style="margin-top: 5%">
                                <uc:AgencyHierarchy ID="ucAgencyHierarchy" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <%-- <div class='form-group col-md-3' title="Select a Agency whose data you want to view.">
                        <span class="cptn">Agency</span>
                        <infs:WclComboBox ID="cmbAgency" runat="server" DataTextField="AG_Name" DataValueField="AG_ID"
                            AutoPostBack="false" OnDataBound="cmbAgency_DataBound" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>--%>  <%--UAT-3245--%>
                    <div class='col-md-12' title="Click the link and select a node to restrict search results to the selected node" id="dvInstHierarchy" runat="server">
                        <label class="cptn">Institution Hierarchy</label>
                        <a href="#" id="instituteHierarchy" onclick="openInstitutionPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <span class="cptn">Applicant First Name</span>
                        <infs:WclTextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <span class="cptn">Applicant Last Name</span>
                        <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Requirement Package" runat="server" id="divPackage">
                        <span class="cptn">Package Name</span>
                        <infs:WclTextBox ID="txtPackageName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered Complio ID">
                        <%--UAT-3245--%>
                        <span class="cptn">Complio ID</span>
                        <infs:WclTextBox ID="txtComplioID" runat="server" Width="100%" CssClass="form-control"></infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class="form-group col-md-3">
                        <span class="cptn">Rotation Date Range</span>
                        <infs:WclDatePicker ID="dpRotationStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                            DateInput-DateFormat="MM/dd/yyyy" ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate"
                            Width="100%" CssClass="form-control" ToolTip="Restrict search results to the entered rotation start date">
                        </infs:WclDatePicker>
                        <div class="gclrPad"></div>
                        <infs:WclDatePicker ID="dpRotationEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                            DateInput-DateFormat="MM/dd/yyyy" ClientEvents-OnPopupOpening="SetMinEndDate" ToolTip="Restrict search results to the entered rotation end date"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered submission date">
                        <span class="cptn">Submission Date</span>
                        <infs:WclDatePicker ID="dpSubmissionDate" runat="server" DateInput-EmptyMessage="Select a date"
                            DateInput-DateFormat="MM/dd/yyyy" ClientEvents-OnPopupOpening="SetMinSubmissionDate"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected Requirement Package Type" runat="server" id="divRequirementPackageType">
                        <span class="cptn">Requirement Package Type</span>
                        <infs:WclComboBox ID="cmbRequirementPackageType" runat="server" DataValueField="ID" CheckBoxes="true" EmptyMessage="--SELECT--"
                            DataTextField="Name" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the Current Rotation records">
                        <span class="cptn">Current Rotation Only</span>
                        <asp:CheckBox runat="server" ID="chkIsCurrent" />
                    </div>
                    <%--UAT-3245--%>
                </div>
            </div>
            <%-- <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the Current Rotation records">
                        <span class="cptn">Current Rotation Only</span>
                        <asp:CheckBox runat="server" ID="chkIsCurrent" />
                    </div>
                </div>
            </div>--%> <%--UAT-3245--%>
        </asp:Panel>
        <div class="col-md-12 text-center">
            <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo"
                SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_SubmitClick" OnCancelClick="fsucCmdBarButton_CancelClick"
                OnSaveClick="fsucCmdBarButton_SaveClick" UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>

        </div>
        <div class="col-md-12">&nbsp;</div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdAssignRotationVerificationQueue" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="true"
            EnableLinqExpressions="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            ShowClearFiltersButton="true" OnInit="grdAssignRotationVerificationQueue_Init"
            OnNeedDataSource="grdAssignRotationVerificationQueue_NeedDataSource" OnItemCommand="grdAssignRotationVerificationQueue_ItemCommand"
            OnSortCommand="grdAssignRotationVerificationQueue_SortCommand" NonExportingColumns="ViewDetail">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" ShowFooter="true" DataKeyNames="FlatVerificationDataID,TenantID,OrganizationUserID,ClinicalRotationID,RequirementPackageSubscriptionID,RequirementPackageTypeID,RequirementItemId,ApplicantRequirementItemId"
                AllowFilteringByColumn="true">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="Applicant First Name" AllowFiltering="true" FilterControlAltText="Filter ApplicantFirstName column"
                        SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Applicant Last Name" AllowFiltering="true" FilterControlAltText="Filter ApplicantLastName column"
                        SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency" SortExpression="AgencyName" AllowFiltering="true" FilterControlAltText="Filter AgencyName column"
                        HeaderTooltip="This column displays the Agency name for each record in the grid"
                        UniqueName="AgencyName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RequirementPackageName" HeaderText="Package Name" SortExpression="RequirementPackageName" AllowFiltering="true" FilterControlAltText="Filter PackageName column"
                        HeaderTooltip="This column displays the Requirement Package Name for each record in the grid"
                        UniqueName="RequirementPackageName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RequirementCategoryName" HeaderText="Category Name" SortExpression="RequirementCategoryName" AllowFiltering="true" FilterControlAltText="Filter RequirementCategoryName column"
                        HeaderTooltip="This column displays the Requirement Category Name for each record in the grid"
                        UniqueName="RequirementCategoryName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RequirementItemName" HeaderText="Item Name" SortExpression="RequirementItemName" AllowFiltering="true" FilterControlAltText="Filter RequirementItemName column"
                        HeaderTooltip="This column displays the Requirement Item Name for each record in the grid"
                        UniqueName="RequirementItemName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ComplioID" HeaderText="Complio ID" SortExpression="ComplioID" AllowFiltering="true" UniqueName="ComplioID" FilterControlAltText="Filter ComplioID column" HeaderTooltip="This Column displays the Complio ID for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="RotationStartDate" HeaderText="Rotation Start Date" AllowFiltering="true" FilterControlAltText="Filter RotationStartDate column"
                        SortExpression="RotationStartDate"
                        UniqueName="RotationStartDate" HeaderTooltip="This column displays the Rotation Start Date for each record in the grid"
                        DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="RotationEndDate" HeaderText="Rotation End Date" AllowFiltering="true" FilterControlAltText="Filter RotationEndDate column"
                        SortExpression="RotationEndDate"
                        UniqueName="RotationEndDate" HeaderTooltip="This column displays the Rotation End Date for each record in the grid"
                        DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="SubmissionDate" HeaderText="Submission Date" AllowFiltering="true" FilterControlAltText="Filter SubmissionDate column"
                        SortExpression="SubmissionDate"
                        UniqueName="SubmissionDate" HeaderTooltip="This column displays the Submission Date for each record in the grid"
                        DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridDateTimeColumn>
                   
                    <telerik:GridBoundColumn DataField="ReqReviewByDesc" HeaderText="Review By" SortExpression="ReqReviewByDesc" AllowFiltering="true" FilterControlAltText="Filter ReqReviewBy column"
                        HeaderTooltip="This column displays the Requirement Package Review By for each record in the grid"
                        UniqueName="ReqReviewByDesc">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-ForeColor="Red" UniqueName="IsCurrentRotation">
                        <ItemTemplate>
                            <%#  Convert.ToBoolean(Eval("IsCurrentRotation")) == true ? "Current Rotation" : string.Empty %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfReqCatId" runat="server" Value='<%# Eval("RequirementCategoryID") %>' />
                            <asp:HiddenField ID="hdfReqPackSubscriptionId" runat="server" Value='<%# Eval("RequirementPackageSubscriptionID") %>' />
                            <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                                ToolTip="Click here to view details of verification."
                                runat="server" Text="Detail">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                    Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
    <div style="display: none">
        <infs:WclButton runat="server" ID="btnDummy" OnClick="btnDummy_Click"></infs:WclButton>
    </div>
    <%--UAT-3245--%>
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
    <asp:HiddenField ID="hdnInstHierarchyLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnTenantID" runat="server" Value="" />
    <asp:HiddenField ID="hdnIsPagePostBack" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstitutionNodeID" runat="server" Value="" />
</div>
<script type="text/javascript">

    function TenantDropDownClosed() {
        var comboBoxTenant = $find($jQuery("[id$=ddlTenantName]")[0].id);
        if (IsAnyChangesInTenantSelectionForInvitation(comboBoxTenant)) {
            $jQuery("[id$=btnDummy]").click();
        }
    }

    function IsAnyChangesInTenantSelectionForInvitation(sender) {
        var oldTenantIdList = [];
        var hdnPreviousTenantIds = $jQuery("[id$=hdnPreviousTenantIds]");
        if (hdnPreviousTenantIds.val() != "" && hdnPreviousTenantIds.val() != null && hdnPreviousTenantIds.val() != undefined) {
            oldTenantIdList = hdnPreviousTenantIds.val().split(',');
        }
        var selectedIdList = ComboBoxSelectedIdList(sender);
        hdnPreviousTenantIds.val(selectedIdList.join(","));
        var isTheCountOfEachSelectionEqual = (selectedIdList.length == oldTenantIdList.length);
        if (isTheCountOfEachSelectionEqual == false)
            return true;

        var oldIdListMINUSNewIdList = $(oldTenantIdList).not(selectedIdList).get();
        var newIdListMINUSOldIdList = $(selectedIdList).not(oldTenantIdList).get();

        if (oldIdListMINUSNewIdList.length != 0 || newIdListMINUSOldIdList.length != 0)
            return true;
        return false;
    }

    function ComboBoxSelectedIdList(sender) {
        var selectedTenantIdList = [];
        var combo = sender;
        var checkeditems = combo.get_checkedItems();
        for (i = 0; i < checkeditems.length; i++) {
            selectedTenantIdList.push(checkeditems[i].get_value());
        }
        return selectedTenantIdList;
    }

    var minDate = new Date("01/01/1900");

    function SetMinDate(picker) {
        picker.set_minDate(minDate);
    }

    function SetMinEndDate(picker) {
        picker.set_minDate(minDate);
    }

    function SetMinSubmissionDate(picker) {
        picker.set_minDate(minDate);
    }

    function CorrectStartToEndDate(picker) {
        var date1 = $jQuery("[id$=dpRotationStartDate]")[0].control.get_selectedDate();
        var date2 = $jQuery("[id$=dpRotationEndDate]")[0].control.get_selectedDate();
        if (date1 != null && date2 != null) {
            if (date1 > date2)
                $jQuery("[id$=dpRotationEndDate]")[0].control.set_selectedDate(null);
        }
    }

    function grd_rwDbClick(s, e) {
        var _id = "btnEdit";
        var b = e.get_gridDataItem().findElement(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function pageLoad() {
        //UAT-3245
        var hdnHierarchyLabel = $jQuery("[id$=hdnHierarchyLabel]");
        var hdnIsPagePostBack = $jQuery("[id$=hdnIsPagePostBack]");
        if (hdnHierarchyLabel.val() != "" && hdnIsPagePostBack.val() == "Focus Set") {
            setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
            hdnIsPagePostBack.val("");
        }
    }

    //UAT-3245
    function openInstitutionPopUp() {
      
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        //var tenantId = 2;
        var tenantId = $jQuery("[id$=hdnTenantID]").val();
        if (tenantId != "0" && tenantId != "" && tenantId > 0) {
            $jQuery("[id$=hdnIsPagePostBack]").val("Focus Set");
            $jQuery("[id$=instituteHierarchy]").focusout();
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();

            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientPopUpClose });
            winopen = true;
        }
        else {
          
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClientPopUpClose(oWnd, args) {
        debugger;
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnInstHierarchyLabel]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeID]").val(arg.InstitutionNodeId);
                __doPostBack("<%= btnDoPostBack.ClientID %>", "");
            }
            winopen = false;

            setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
        }
    }

</script>
