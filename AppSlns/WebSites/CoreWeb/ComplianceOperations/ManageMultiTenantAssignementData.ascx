<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageMultiTenantAssignementData.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ManageMultiTenantAssignementData" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style>
    .rgFooter {
        background: #eee !important;
    }

    .RadGrid_Default .rgFooter td {
        border-bottom-style: none !important;
    }

    tfoot td.rgPagerCell {
        border: none !important;
    }
</style>

<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblVerificationQueue" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlShowFilters">
                <div class='sxro sx2co'>
                    <div class='sxlb' title="Select the Institution whose data you want to view">
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbTenantName" runat="server" DataTextField="TenantName" EmptyMessage="--Select--"
                            CausesValidation="false" DataValueField="TenantID" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            <Localization CheckAllString="All" />
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="cmbTenantName"
                                Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
<infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Save,Submit,Cancel"
    AutoPostbackButtons="Save,Submit,Cancel" SubmitButtonText="Reset" CancelButtonText="Cancel" OnSubmitClick="CmdBarSearch_SubmitClick"
    SaveButtonText="Search" SaveButtonIconClass="rbSearch" ValidationGroup="grpFormSubmit" OnSaveClick="CmdBarSearch_SaveClick" OnCancelClick="CmdBarSearch_CancelClick">
</infsu:CommandBar>
<asp:UpdatePanel ID="pnlVerError" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="msgbox" id="VerMsgBox">
            <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblVerError"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="section" runat="server" id="pnlVerification">
    <h1 class="mhdr">
        <asp:Label Text="Multiple Institutions Assignment Queue" runat="server" ID="lblPageHdr" /></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdMultiTenantVerificationItemData" AllowPaging="true" AutoGenerateColumns="false" NonExportingColumns="SelectUsers,AdminNote,IsUiRulesViolate"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="true" CellSpacing="0"
                GridLines="Both" OnNeedDataSource=" grdMultiTenantVerificationItemData_NeedDataSource" OnItemCommand="grdMultiTenantVerificationItemData_ItemCommand"
                ShowAllExportButtons="False" OnSortCommand="grdMultiTenantVerificationItemData_SortCommand" OnInit="grdMultiTenantVerificationItemData_Init"
                OnItemDataBound="grdMultiTenantVerificationItemData_ItemDataBound"
                AllowCustomPaging="true">
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="350mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView CommandItemDisplay="Top" AllowPaging="true" AllowFilteringByColumn="true" DataKeyNames="FVDId,ApplicantComplianceItemID,ApplicantId,CategoryId,ItemName,ComplianceItemId,ApplicantComplianceCategoryId,VerificationStatusCode,TenantID,IsDirty"
                    ClientDataKeyNames="FVDId" ShowFooter="true">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                        ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="SelectUsers" HeaderTooltip="Click this box to select all users on the active page"
                            AllowFiltering="false" ShowFilterIcon="false">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <FooterTemplate>
                                <label>
                                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                                    Select All
                                </label>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectItem" runat="server"
                                    onclick='<% # "UnCheckHeader(this,\"" + Eval("FVDId") + "\" );"%>' OnCheckedChanged="chkSelectItem_CheckedChanged" />
                                <%-- Checked='<%#Convert.ToBoolean(Eval("IsUserGroupMatching")) %>'
                                   <asp:Label ID="lblIsUserGroup" runat="server" Text='<%#Eval("IsUserGroupMatching") %>'
                                    Visible="false"></asp:Label>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ApplicantName" FilterControlAltText="Filter ApplicantName column"
                            HeaderText="Applicant Name" SortExpression="ApplicantName" UniqueName="ApplicantName"
                            HeaderTooltip="This column displays the applicant's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="InstitutionName" FilterControlAltText="Filter InstitutionName column"
                            HeaderText="Institution Name" SortExpression="InstitutionName" UniqueName="InstitutionName"
                            HeaderTooltip="This column displays the Institution name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemName" FilterControlAltText="Filter ItemName column"
                            HeaderText="Item Name" SortExpression="ItemName" UniqueName="ItemName" HeaderTooltip="This column displays the name of the Item for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter CategoryName column"
                            HeaderText="Category Name" SortExpression="CategoryName" UniqueName="CategoryName"
                            HeaderTooltip="This column displays the name of the Category for each record">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                            HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName"
                            HeaderTooltip="This column displays the name of the package for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="SubmissionDate" FilterControlAltText="Filter SubmissionDate column"
                            HeaderText="Submission Date" SortExpression="SubmissionDate" UniqueName="SubmissionDate"
                            DataFormatString="{0:d}" FilterControlWidth="100px" HeaderTooltip="This column displays the date the applicant submitted the Item for review">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="VerificationStatus" FilterControlAltText="Filter VerificationStatus column"
                            HeaderText="Verification Status" SortExpression="VerificationStatus" UniqueName="VerificationStatus"
                            HeaderTooltip="This column displays the applicant's overall compliance status for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SystemStatus" FilterControlAltText="Filter SystemStatus column"
                            HeaderText="System Status" SortExpression="SystemStatus" UniqueName="SystemStatus"
                            HeaderTooltip="This column displays the system suggested Item Compliance, if a compliance rule has been applied at the Item level">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ReviewLevel" FilterControlAltText="Filter ReviewLevel column"
                            HeaderText="Review Level" SortExpression="ReviewLevel" UniqueName="ReviewLevel" HeaderTooltip="This column displays the Review Level for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AssignedUserName" FilterControlAltText="Filter AssignedUserName column"
                            HeaderText="Assigned To User" SortExpression="AssignedUserName" UniqueName="AssignedUserName"
                            HeaderTooltip="This column displays the user, if any,  who has been assigned to complete the verification for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AdminNote" FilterControlAltText="Filter AdminNote column"
                            HeaderText="Admin Note" UniqueName="AdminNoteForExport" Display="false" HeaderTooltip="This column displays the Admin Note for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsUiRulesViolate" AllowFiltering="false" HeaderText="IsUiRulesViolate" AllowSorting="false" ItemStyle-Width="300px"
                            UniqueName="IsUiRulesViolate" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Admin Note" UniqueName="AdminNote" FilterControlAltText="Filter AdminNote column">
                            <ItemTemplate>
                                <asp:Label ID="lblChangeValue" runat="server" Text='<%# Convert.ToString(Eval("AdminNote")).Length > 33 ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AdminNote")).Substring(0, 33)) + "...." : INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AdminNote")))%>'></asp:Label>
                                <infs:WclToolTip runat="server" ID="tltpChangeValue" TargetControlID="lblChangeValue"
                                    Width="300px" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AdminNote"))) %>' ManualClose="false"
                                    RelativeTo="Element" Position="TopCenter" Visible='<%# Eval("AdminNote").ToString().Trim()==String.Empty ? false : Convert.ToString(Eval("AdminNote")).Length > 33?true:false %>'>
                                </infs:WclToolTip>
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
        <div class="gclr">
        </div>

    </div>
    <div style="padding: 10px; text-align: center" runat="server" id="pnlVerShowUsers" visible="false">
        <span title="Select a user and click assign to assign verification for all chosen items to that user">Select User</span>
        <infs:WclComboBox ID="cmbVerSelectedUser" runat="server" AutoPostBack="false" Filter="Contains"
            DataTextField="FirstName" DataValueField="OrganizationUserID">
        </infs:WclComboBox>
        <infs:WclButton ID="btnVerAssignUser" runat="server" AutoPostBack="true" Text="Assign" OnClick="btnVerAssignUser_Click"
            ToolTip="Select a user and click assign to assign verification for all chosen items to that user" />

        <infs:WclButton ID="btnAutomaticItemAssigning"  runat="server" AutoPostBack="true" Text="Auto Assign" OnClick="btnAutomaticItemAssigning_Click"
            ToolTip="Select a user and click assign to assign verification for all chosen items to that user" />
    </div>
</div>
<asp:HiddenField ID="hdnEditFVDId" runat="server" />
<asp:HiddenField ID="hdnIsMutipleTimesAssignmentAllowed" runat="server" Value="false" />
<div class="approvepopup" runat="server" style="display: none">
    <asp:Label ID="lblConfirmMessage" runat="server"></asp:Label>
</div>

<script type="text/javascript">

    var editFVDIds = [];

    //function to check all items on Header checked click.
    function CheckAll(id) {
        var masterTable = $find("<%= grdMultiTenantVerificationItemData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
                var FVDId = row[i].getDataKeyValue("FVDId");
                FlatVeriFicationDetailIDsInList(isChecked, FVDId);
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = isChecked;
        $jQuery('[id$=chkSelectAll]')[1].checked = isChecked;

    }

    //function to uncheck header check-box when any of the item is unchecked.
    function UnCheckHeader(id, FVDId) {
        var checkHeader = true;
        var masterTable = $find("<%= grdMultiTenantVerificationItemData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        FlatVeriFicationDetailIDsInList(id.checked, FVDId);
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
        $jQuery('[id$=chkSelectAll]')[1].checked = checkHeader;
    }

    //function to get FlatVerificationDetailIDs of all checked items and storing them in array.
    function FlatVeriFicationDetailIDsInList(isChecked, FVDId) {
        if (isChecked) {
            if (editFVDIds.indexOf(FVDId) < 0) {
                editFVDIds.push(FVDId);
            }
        }
        else {
            editFVDIds = $jQuery.grep(editFVDIds, function (value) {
                return value != FVDId;
            });
        }
        $jQuery('[id$="hdnEditFVDId"]').val(editFVDIds);
    }

    function pageLoad() {
        CheckRadGridAllCheckBox('chkSelectItem', 'chkSelectAll');
    }

    //Function to check headerbox checkbox on page-load when all items are checked.
    function CheckRadGridAllCheckBox(chkBoxRadGridItemID, chkBoxSelectAllRadGridHeaderItemID) {
        var chkBoxRadGridItemList = $jQuery('input[type=checkbox][id$="' + chkBoxRadGridItemID + '"]');
        var chkBoxRadGridItemUncheckedList = $jQuery('input[type=checkbox][id*= "' + chkBoxRadGridItemID + '"]:not(:checked)');
        if (chkBoxRadGridItemUncheckedList.length == 0 && chkBoxRadGridItemList.length > 0) {
            $jQuery('input[type=checkbox][id$="' + chkBoxSelectAllRadGridHeaderItemID + '"]').attr('checked', 'checked');
        }
    }

    function ResetSelectedItems() {
        editFVDIds = [];
    }

    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        var _id = "btnEditNew";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function ShowConfirmationPopUp() {
        $jQuery(".approvepopup").css("display", "block");
        $window.showDialog($jQuery(".approvepopup").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Continue", click: function () {
                    $jQuery("[id$=hdnIsMutipleTimesAssignmentAllowed]").val('true');
                    __doPostBack('<%= btnVerAssignUser.UniqueID %>', '');
                }
            }, closeBtn: {
                autoclose: true, text: "Cancel", click: function () {
                    $jQuery("[id$=hdnIsMutipleTimesAssignmentAllowed]").val('false');

                }
            }
        }, 475, '&nbsp;');
        $jQuery(".approvepopup").css("display", "none");
    }
</script>
