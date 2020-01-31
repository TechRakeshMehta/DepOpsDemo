<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEntryQueue.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.DataEntryQueue" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxDataEntryQueue">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/DataEntryQueue.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<%--To reduce string columns filters : Start--%>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var column = null;
        function MenuShowing(sender, args) {
            if (column == null) return;
            var menu = sender; var items = menu.get_items();
            //column.get_uniqueName() == "uniqueName"
            if (column.get_dataType() == "System.String") {
                var i = 0;
                while (i < items.get_count()) {
                    if (!(items.getItem(i).get_value() in { 'NoFilter': '', 'Contains': '' })) {
                        var item = items.getItem(i);
                        if (item != null)
                            item.set_visible(false);
                    }
                    else {
                        var item = items.getItem(i);
                        if (item != null)
                            item.set_visible(true);
                    } i++;
                }
            }      
             if (columnName == "AssignToUserName") { 
                var i = 0;
                while (i < items.get_count()) {
                    if (!(items.getItem(i).get_value() in { 'NoFilter': '', 'Contains': '', 'IsEmpty': '' })) {
                        var item = items.getItem(i);
                        if (item != null) {
                            item.set_visible(false);
                        }
                    }
                    else {
                        var item = items.getItem(i);
                        if (item != null) {
                            item.set_visible(true);
                        }
                    } i++;
                }
            }
            column = null;
            columnName = null;
            menu.repaint();
        }
        function filterMenuShowing(sender, eventArgs) {
            column = eventArgs.get_column();
            columnName = eventArgs.get_column().get_uniqueName();
        }
    </script>
</telerik:RadCodeBlock>
<%--To reduce string columns filters : End--%>
<style>
    .rgFooter {
        background-color: #eee !important;
    }

    .RadGrid_Default .rgFooter td {
        border-bottom-style: none !important;
    }

    tfoot td.rgPagerCell {
        border: none !important;
    }
    .buttonHidden {
            display: none;
        }
</style>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblDataEntryQueue" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlFilters">
                <div class='sxro sx2co'>
                    <div class='sxlb' title="Select the Institution whose data you want to view">
                        <span class="cptn">Institution</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbTenantName" runat="server" DataTextField="TenantName" EmptyMessage="--Select--"
                            AllowCustomText="true" OnClientBlur="checkTenantName_SelectedIndexChanged" OnClientDropDownClosing="checkTenantName_SelectedIndexChanged"
                            CausesValidation="false" DataValueField="TenantID" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            <localization checkallstring="All" />
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="cmbTenantName"
                                Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div runat="server" id="dvMultipleInstitutionHierarchy" visible="false" class='sxro sx2co'>
                    <div class='sxlb' title="Select the Institution whose data you want to view">
                        <span class="cptn">Institution Hierarchy</span>
                    </div>
                    <div class='sxlm'>
                        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
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
    SaveButtonText="Search" SaveButtonIconClass="rbSearch" OnSaveClick="CmdBarSearch_SaveClick" OnCancelClick="CmdBarSearch_CancelClick">
</infsu:CommandBar>
<div class="section" runat="server" id="pnlDataEntry">
    <h1 class="mhdr">
        <asp:Label Text="" runat="server" ID="lblPageHdr" /></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdDataEntry" AllowPaging="true" AutoGenerateColumns="false" NonExportingColumns="SelectDocuments,ViewDetail"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="true" CellSpacing="0"
                GridLines="Both" OnNeedDataSource=" grdDataEntry_NeedDataSource" OnItemCommand="grdDataEntry_ItemCommand"
                ShowAllExportButtons="False" OnSortCommand="grdDataEntry_SortCommand" OnInit="grdDataEntry_Init"
                OnItemDataBound="grdDataEntry_ItemDataBound"
                AllowCustomPaging="true">
                <clientsettings enablerowhoverstyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" OnFilterMenuShowing="filterMenuShowing" />
                    <Selecting AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="false" />
                </clientsettings>
                <exportsettings pdf-pagewidth="350mm" pdf-pageheight="210mm" pdf-pageleftmargin="20mm"
                    pdf-pagerightmargin="20mm">
                </exportsettings>
                <groupingsettings casesensitive="false" />
                <mastertableview commanditemdisplay="Top" allowpaging="true" allowfilteringbycolumn="true" datakeynames="FDEQ_ID,ApplicantDocumentID,ApplicantOrganizationUserID,TenantID,DiscardDocumentCount"
                    clientdatakeynames="FDEQ_ID" showfooter="true">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                        ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="SelectDocuments" HeaderTooltip="Click this box to select all users on the active page"
                            AllowFiltering="false" ShowFilterIcon="false" Visible="false">
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
                                    onclick='<% # "UnCheckHeader(this,\"" + Eval("FDEQ_ID") + "\" );"%>' OnCheckedChanged="chkSelectItem_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter Institution column" AllowFiltering="true"
                            HeaderText="Institution" SortExpression="TenantName" UniqueName="TenantName"
                            HeaderTooltip="This column displays the Institution's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DocumentName" FilterControlAltText="Filter DocumentName column" AllowFiltering="true"
                            HeaderText="Document Name" SortExpression="DocumentName" UniqueName="DocumentName"
                            HeaderTooltip="This column displays the Document's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DocumentStatusName" FilterControlAltText="Filter DocumentStatusName column" AllowSorting="true"
                            HeaderText="Status" SortExpression="DocumentStatusName" UniqueName="DocumentStatusName"
                            HeaderTooltip="This column displays the Status for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AssignToUserName" FilterControlAltText="Filter AssignToUserName column" AllowSorting="true"
                            HeaderText="Assigned To" SortExpression="AssignToUserName" UniqueName="AssignToUserName" Visible="false"
                            HeaderTooltip="This column displays the user, if any,  who has been assigned to complete the data entry for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="DateUploaded" FilterControlAltText="Filter DateUploadDatePart column" AllowSorting="true"
                            HeaderText="Date Uploaded(with timestamp)" SortExpression="DateUploaded" UniqueName="DateUploadDatePart" EnableTimeIndependentFiltering="true"
                            FilterControlWidth="100px" HeaderTooltip="This column displays the date the applicant submitted the document">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnEditNew" ButtonType="LinkButton" CommandName="ViewDetail"
                                    ToolTip="Click to open the Data Entry screen for this Item" runat="server"
                                    Text="Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </mastertableview>
                <pagerstyle pagesizecontroltype="RadComboBox"></pagerstyle>
                <filtermenu enableimagesprites="False" OnClientShowing="MenuShowing">
                </filtermenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>

    </div>
    <div style="padding: 10px; text-align: center" runat="server" id="pnlShowUsers" visible="false">
        <span title="Select a user and click assign to assign verification for all chosen items to that user">Select User</span>
        <infs:WclComboBox ID="cmbVerSelectedUser" runat="server" AutoPostBack="false" Filter="Contains"
            DataTextField="FirstName" DataValueField="OrganizationUserID">
        </infs:WclComboBox>
        <infs:WclButton ID="btnAssignUser" runat="server" AutoPostBack="true" Text="Assign" OnClick="btnAssignUser_Click"
            ToolTip="Select a user and click assign to assign data entry for all chosen documents to that user" />
    </div>
</div>
<asp:HiddenField ID="hdnFDEQ_IDs" runat="server" />
<asp:HiddenField ID="hdnSelectedTenantID" runat="server" />
<asp:HiddenField ID="hdnApplicantUserID" runat="server" />
<asp:HiddenField ID="hdnApplicantDocumentID" runat="server" />
<asp:HiddenField ID="hdnPackageSubscriptionID" runat="server" />
<asp:HiddenField ID="hdnFDEQ_ID" runat="server" />
<asp:HiddenField ID="hdnDiscardDocumentCount" runat="server" />
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />

<div style="display: none">
    <infs:WclButton ID="btnRedirect" ClientIDMode="Static" runat="server" AutoPostBack="true" OnClick="btnRedirect_Click" />
    <infs:WclButton ID="btnPostback" ClientIDMode="Static" runat="server" AutoPostBack="true" OnClick="btnPostback_Click" />
</div>
<script type="text/javascript">

    var editFDEQ_IDs = [];

    //function to check all items on Header checked click.
    function CheckAll(id) {
        var masterTable = $find("<%= grdDataEntry.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
                var FDEQ_ID = row[i].getDataKeyValue("FDEQ_ID");
                FlatVeriFicationDetailIDsInList(isChecked, FDEQ_ID);
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = isChecked;
        $jQuery('[id$=chkSelectAll]')[1].checked = isChecked;
    }

    //function to uncheck header check-box when any of the item is unchecked.
    function UnCheckHeader(id, FDEQ_ID) {
        var checkHeader = true;
        var masterTable = $find("<%= grdDataEntry.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        FlatVeriFicationDetailIDsInList(id.checked, FDEQ_ID);
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
    function FlatVeriFicationDetailIDsInList(isChecked, FDEQ_ID) {
        if (isChecked) {
            if (editFDEQ_IDs.indexOf(FDEQ_ID) < 0) {
                editFDEQ_IDs.push(FDEQ_ID);
            }
        }
        else {
            editFDEQ_IDs = $jQuery.grep(editFDEQ_IDs, function (value) {
                return value != FDEQ_ID;
            });
        }
        $jQuery('[id$="hdnFDEQ_IDs"]').val(editFDEQ_IDs);
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
        editFDEQ_IDs = [];
    }


    function ShowProgressBar() {
        Page.showProgress('Processing...');
    }
    //click on link button while double click on any row of grid.
    function grd_rwDbClick(s, e) {
        var _id = "btnEditNew";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function checkTenantName_SelectedIndexChanged(sender, args) {
        if (sender.get_checkedItems().length == 0) {
            sender.clearSelection();
            sender.set_emptyMessage("--Select--");
            $jQuery("[id$=dvMultipleInstitutionHierarchy]").css('display', 'none');
        }
        else if (sender.get_checkedItems().length == 1) {
            ShowProgressBar();
            __doPostBack("<%= btnPostback.UniqueID %>", "");
        }
        else {
            $jQuery("[id$=dvMultipleInstitutionHierarchy]").css('display', 'none');
        }
}

var winopen = false;
function openPopUp() {
    var composeScreenWindowName = "Institution Hierarchy";
    var tenantId = $jQuery("[id$=hdnSelectedTenantID]").val();
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
</script>

