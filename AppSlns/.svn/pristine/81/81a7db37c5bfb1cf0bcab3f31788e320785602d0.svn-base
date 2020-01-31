<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComplianceDocumentSearch.ascx.cs"
    Inherits="CoreWeb.Search.Views.ComplianceDocumentSearch" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxComplianceDocumentSearch">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>


<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Compliance Document Search
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Select a User Group. ">
                        <span class="cptn">User Group</span>
                        <infs:WclComboBox ID="ddlUserGroup" runat="server" CheckBoxes="true" EmptyMessage="All Applicants"
                            EnableCheckAllItemsCheckBox="true" Localization-CheckAllString="Select All User Groups"
                            DataTextField="UG_Name" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            DataValueField="UG_ID" AutoPostBack="false" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-12' title="Click the link and select a node to restrict search results to the selected node">
                        <span class="cptn">Institution Hierarchy</span>
                        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Select a Compliance Item. ">
                        <span class="cptn">Compliance Item</span>
                        <infs:WclComboBox ID="chkComplianceItem" runat="server" CheckBoxes="true" EmptyMessage="--Select--"
                            EnableCheckAllItemsCheckBox="true" DataTextField="Name" DataValueField="ComplianceItemID"
                            Localization-CheckAllString="ALL" AutoPostBack="false" Width="100%" CssClass="form-control"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
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
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class="form-group col-md-3" title="Restrict search result to the entered document uploaded From Date.">
                        <span class="cptn">Upload From Date</span>
                        <infs:WclDatePicker ID="dpFromDate" runat="server" CssClass="form-control" Width="100%"
                            EnableAriaSupport="true" ClientEvents-OnPopupClosing="OnCalenderClosing" DateInput-SelectionOnFocus="CaretToBeginning"
                            Calendar-EnableKeyboardNavigation="true" Calendar-EnableAriaSupport="true" DateInput-EnableAriaSupport="true">
                        </infs:WclDatePicker>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search result to the entered document uploaded To Date.">
                        <span class="cptn">Upload To Date</span>
                        <infs:WclDatePicker ID="dpToDate" runat="server" CssClass="form-control" Width="100%"
                            EnableAriaSupport="true" ClientEvents-OnPopupClosing="OnCalenderClosing" DateInput-SelectionOnFocus="CaretToBeginning"
                            Calendar-EnableKeyboardNavigation="true" Calendar-EnableAriaSupport="true" DateInput-EnableAriaSupport="true">
                        </infs:WclDatePicker>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Clear,Cancel"
                AutoPostbackButtons="Submit,Save,Clear,Cancel" SubmitButtonIconClass="rbUndo"
                SubmitButtonText="Reset" DefaultPanel="pnlSearch" DefaultPanelButton="Save"
                SaveButtonText="Search" SaveButtonIconClass="rbSearch" ClearButtonText="Export Document(s)"
                CancelButtonText="Cancel" ValidationGroup="grpFormSubmit"
                OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnClearClick="CmdBarSearch_ExportClick"
                OnCancelClick="CmdBarCancel_Click" ClearButtonIconClass="" UseAutoSkinMode="false"
                ButtonSkin="Silk">
            </infsu:CommandBar>
        </div>
    </div>
    <asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
    <asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
    <asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
    <asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
    <asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
    <div class="row">
        <infs:WclGrid runat="server" ID="grdComplianceDocSearch" AutoGenerateColumns="False"
            AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
            ShowAllExportButtons="false" ShowExtraButtons="False" AllowCustomPaging="true"
            ShowClearFiltersButton="false"
            GridLines="Both" OnNeedDataSource="grdComplianceDocSearch_NeedDataSource" OnItemDataBound="grdComplianceDocSearch_ItemDataBound"
            OnItemCommand="grdComplianceDocSearch_ItemCommand" OnSortCommand="grdComplianceDocSearch_SortCommand">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="ApplicantDocumentID,ID,ApplicantID"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="ExportCheckBox" AllowFiltering="false" ShowFilterIcon="false"
                        HeaderStyle-Width="2%" ItemStyle-Width="2%">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectDocument" runat="server" onclick="UnCheckHeader(this)"
                                OnCheckedChanged="chkSelectDocument_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="DocumentPath" FilterControlAltText="Filter DocumentPath column"
                        HeaderText="DocumentPath" SortExpression="DocumentPath" UniqueName="DocumentPath"
                        Display="false">
                        <ItemTemplate>
                            <asp:Label ID="lblDocumentPath" runat="server" Text='<%# Convert.ToString(Eval("DocumentPath")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="FileName" FilterControlAltText="Filter DocumentPath column"
                        HeaderText="FileName" SortExpression="FileName" UniqueName="DocumentName" Display="false">
                        <ItemTemplate>
                            <asp:Label ID="lblFileName" runat="server" Text='<%# Convert.ToString(Eval("FileName")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="ApplicantDocumentID" FilterControlAltText="Filter ApplicantDocumentID column"
                        HeaderText="ID" SortExpression="ApplicantDocumentID" UniqueName="ApplicantDocumentID"
                        Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantName" FilterControlAltText="Filter ApplicantName column"
                        HeaderStyle-Width="350" AllowFiltering="false"
                        HeaderText="Applicant Name" SortExpression="ApplicantName" UniqueName="ApplicantName"
                        ReadOnly="true" HeaderTooltip="This column contains the name of Applicant">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ItemName" FilterControlAltText="Filter ItemName column"
                        HeaderStyle-Width="350" AllowFiltering="false"
                        HeaderText="Item Name" SortExpression="ItemName" UniqueName="ItemName" ReadOnly="true"
                        HeaderTooltip="This column contains the name of Item">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter FileName column"
                        HeaderStyle-Width="350" AllowFiltering="false"
                        HeaderText="File Name" SortExpression="FileName" UniqueName="FileName" ReadOnly="true"
                        HeaderTooltip="This column contains the name of each uploaded document">
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="SubmissionDate" FilterControlAltText="Filter SubmissionDate column"
                        HeaderStyle-Width="350" AllowFiltering="false" DataFormatString="{0:d}"
                        HeaderText="Submission Date" SortExpression="SubmissionDate" UniqueName="SubmissionDate" ReadOnly="true"
                        HeaderTooltip="This column contains the submission date of each uploaded document">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageDocument">
                        <HeaderStyle Width="110" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <a runat="server" id="ancManageDocument" title="Click here to view the document">View
                                Document</a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
    <div class="col-md-12">
        <div class="row text-center">
            <infsu:CommandBar ID="fsucCmdExport" runat="server" ButtonPosition="Center" DisplayButtons="Extra"
                AutoPostbackButtons="Extra" ExtraButtonText="Export Document(s)" OnExtraClick="btnExport_Click"
                UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
            <iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>
        </div>
    </div>
</div>
<script type="text/javascript">
    var winopen = false;
    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        //var tenantId = 2;
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            var DelemittedDeptPrgMapIds = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&DelemittedDeptPrgMapIds=" + DelemittedDeptPrgMapIds);
            var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
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
                __doPostBack("<%= btnDoPostBack.ClientID %>", "");
            }
            winopen = false;
        }
    }
    //click on link button while double click on any row of grid.   
    function grd_rwDbClick(s, e) {
        var _id = "ancManageDocument";
        var b = e.get_gridDataItem().findElement(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
        //findElement findControl
    }

    function CheckAll(id) {
        var masterTable = $find("<%= grdComplianceDocSearch.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked = isChecked; // for checking the checkboxes
            }
        }
    }
    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdComplianceDocSearch.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    } 
</script>
