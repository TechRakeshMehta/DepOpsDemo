<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.ExceptionVerificationQueue" CodeBehind="ExceptionVerificationQueue.ascx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearch.ascx" TagName="CustomAttributeLoaderSearch" TagPrefix="uc1" %>--%>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearchMultipleNodes.ascx" TagName="CustomAttributeLoaderNodeSearch" TagPrefix="uc" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    /*.chkAllResults {
        width: 10%;
        float: left;
        margin-top: 10px;
        padding-left: 7px;
    }

    .sxcbar {
        padding-left: 0px !important;
        width: 89%;
    }*/

    .rgFooter {
        background-color: #f7f8f8 !important;
    }

    .RadGrid_Default .rgFooter td {
        border-bottom-style: none !important;
    }

    tfoot td.rgPagerCell {
        border: none !important;
    }
</style>

<script type="text/javascript">
    function CheckAllExceptions(id) {
        var masterTable = $find("<%= grdExceptionItemData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectedException").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectedException").checked = isChecked; // for checking the checkboxes
            }
        }
        $jQuery('[id$=chkSelectAllExceptions]')[0].checked = isChecked;
        $jQuery('[id$=chkSelectAllExceptionsFooter]')[0].checked = isChecked;
    }
    function UnCheckExpHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdExceptionItemData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectedException").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectedException").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAllExceptions]')[0].checked = checkHeader;
        $jQuery('[id$=chkSelectAllExceptionsFooter]')[0].checked = checkHeader;
    }
    //click on link button while double click on any row of grid.    
    function grd_rwDbClick(s, e) {
        var _id = "btnEdit";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function pageLoad() {

       

        SetDefaultButtonForSection("divSearchPanel", "fsucCmdBar1_btnSave", true);
    }
</script>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
   <script type="text/javascript">
       var column = null;
        var columnName = null;
        function MenuShowing(sender, args) {
            
            if (column == null) return;
            var menu = sender; var items = menu.get_items();
            
            //column.get_uniqueName() == "uniqueName"
            if (columnName == 'PackageName' || columnName == 'CategoryName' || columnName == 'ItemName' 
            ||  columnName == 'AssignedUserName')
            {
              
               var j = 0;
                while (j < items.get_count()) {
                    if (!(items.getItem(j).get_value() in { 'NoFilter': '', 'Contains': '','DoesNotContain':'','IsEmpty':'' })) {
                        var item = items.getItem(j);
                        if (item != null)
                            item.set_visible(false);
                    }  else {
                        var item = items.getItem(j);
                        if (item != null)
                            item.set_visible(true);
                    } j++;
                }
            }
            else if (columnName == 'ReviewLevel')
            {
               
                var K = 0;
                while (K < items.get_count()) {
                    debugger;
                    if (!(items.getItem(K).get_value() in { 'NoFilter': '', 'EqualTo': '' })) {

                        var item = items.getItem(K);
                        if (item != null)
                            item.set_visible(false);
                    } else {
                        var item = items.getItem(K);
                        if (item != null)
                            item.set_visible(true);
                    } K++;
                }
            }
           else if (column.get_dataType() == "System.String" && columnName != 'ReviewLevel') {
                var i = 0;
                while (i < items.get_count()) {
                    if (!(items.getItem(i).get_value() in { 'NoFilter': '', 'Contains': '','IsEmpty':'' })) {
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
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblExceptionQueue" runat="server" Text=""></asp:Label></h2>
        </div>
    </div>

    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel runat="server" ID="pnlShowFilters">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class='cptn'>Institution</span>
                            <%-- <infs:WclDropDownList ID="ddlTenantName" runat="server" AutoPostBack="true" OnItemSelected="ddlTenantName_ItemSelected" Enabled="false"
                                DataTextField="TenantName" DataValueField="TenantID" OnDataBound="ddlTenantName_DataBound">
                            </infs:WclDropDownList>--%>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTenantName_ItemSelected" Enabled="false"
                                DataTextField="TenantName" DataValueField="TenantID" OnDataBound="ddlTenantName_DataBound"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName" ValidationGroup="grpFormSubmit"
                                    InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--<uc1:CustomAttributeLoaderSearch ID="ucCustomAttributeLoader" runat="server" />--%>
            <uc:CustomAttributeLoaderNodeSearch ID="ucCustomAttributeLoaderNodeSearch" runat="server" />
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Select a User Group. The selected group's members' checkboxes will be marked in the grid below">
                        <span class='cptn'>User Group</span>
                        <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID"
                            OnDataBound="ddlUserGroup_DataBound" OnSelectedIndexChanged="ddlUserGroup_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            AutoPostBack="true" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select a Package to restrict items in the grid to the selected Package">
                        <span class='cptn'>Package</span>
                        <infs:WclComboBox ID="ddlPackage" runat="server" AutoPostBack="true" DataTextField="PackageName"
                            DataValueField="CompliancePackageID" OnSelectedIndexChanged="ddlPackage_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            OnDataBound="ddlPackage_DataBound" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select a Category to restrict items in the grid to the selected Category">
                        <span class='cptn'>Category</span>
                        <infs:WclComboBox ID="ddlCategory" runat="server" AutoPostBack="true" DataTextField="CategoryName"
                            DataValueField="ComplianceCategoryID" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            OnDataBound="ddlCategory_DataBound" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Show only Rush Orders</span>
                        <asp:CheckBox ID="chkShowRushOrders" runat="server" Checked="false" OnCheckedChanged="chkShowRushOrders_CheckedChanged"
                            AutoPostBack="true" Width="100%" CssClass="form-control" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-2">
                <div class="row">
                    <asp:CheckBox Visible="false" ID="chkSelectAllResults" Text="Select All Results" runat="server" CssClass="form-control" OnCheckedChanged="chkSelectAllResults_CheckedChanged" AutoPostBack="true" />
                </div>
            </div>
            <div class="form-group col-md-9">
                <div class="row">
                    <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo" SubmitButtonText="Reset"
                        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
                        ValidationGroup="grpFormSubmit" OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click"
                        OnCancelClick="CmdBarCancel_Click" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="pnlExpError" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="msgbox" id="ExpMsgBox">
                <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblExpError"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div runat="server" id="pnlException" visible="false">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">
                    <asp:Label Text="Exception Verification Queue" runat="server" ID="lblExceptionHdr" />
                </h2>
            </div>
        </div>
        <div class="row allowscroll" id="linkHover">
            <infs:WclGrid runat="server" ID="grdExceptionItemData" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                NonExportingColumns="AssignItems,ViewDetail,CustomAttributes" GridLines="Both" OnNeedDataSource="grdExceptionItemData_NeedDataSource"
                OnItemDataBound="grdExceptionItemData_ItemDataBound" ShowAllExportButtons="false"
                OnItemCommand="grdExceptionItemData_ItemCommand" AllowCustomPaging="true" OnSortCommand="grdExceptionItemData_SortCommand"
                OnInit="grdExceptionItemData_Init">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                    <ClientEvents OnRowDblClick="grd_rwDbClick" OnFilterMenuShowing="filterMenuShowing"/>
                </ClientSettings>
                <GroupingSettings CaseSensitive="false" />
                <ExportSettings Pdf-PageWidth="350mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ApplicantComplianceItemID,HierarchyNodeID,ApplicantId
                    ,ItemName,VerificationStatusCode,ComplianceItemId,CategoryId"
                    ShowFooter="true">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                        ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="AssignItems" AllowFiltering="false" ShowFilterIcon="false">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAllExceptions" runat="server" onclick="CheckAllExceptions(this)" ToolTip="Click this box to select all users on the active page" />
                            </HeaderTemplate>
                            <FooterTemplate>
                                <label style="width:100px;">
                                    <asp:CheckBox ID="chkSelectAllExceptionsFooter" runat="server" onclick="CheckAllExceptions(this)" ToolTip="Click this box to select all users on the active page" /> Select All
                                </label>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectedException" runat="server" onclick="UnCheckExpHeader(this)"
                                    OnCheckedChanged="chkSelectExpItem_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridBoundColumn DataField="ApplicantName" FilterControlAltText="Filter ApplicantName column"
                            HeaderText="Applicant Name" SortExpression="ApplicantName" UniqueName="ApplicantName" HeaderTooltip="This column displays the applicant's name for each record in the grid">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column"
                            HeaderText="Applicant First Name" SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column"
                            HeaderText="Applicant Last Name" SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemName" FilterControlAltText="Filter ItemName column"
                            HeaderText="Item Name" SortExpression="ItemName" UniqueName="ItemName" HeaderTooltip="This column displays the name of the Item for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter CategoryName column"
                            HeaderText="Category Name" SortExpression="CategoryName" UniqueName="CategoryName" HeaderTooltip="This column displays the name of the Category for each record">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                            HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName" HeaderTooltip="This column displays the name of the package for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="SubmissionDate" FilterControlAltText="Filter SubmissionDate column"
                            HeaderText="Submission Date" SortExpression="SubmissionDate" UniqueName="SubmissionDate" HeaderTooltip="This column displays the date the applicant submitted the Item for review"
                            DataFormatString="{0:d}" FilterControlWidth="100px">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="VerificationStatus" FilterControlAltText="Filter VerificationStatus column" AllowSorting ="false" AllowFiltering="false"
                            HeaderText="Verification Status" SortExpression="VerificationStatus" UniqueName="VerificationStatus" HeaderTooltip="This column displays the applicant's overall compliance status for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ReviewLevel" FilterControlAltText="Filter ReviewLevel column" AllowSorting ="true" 
                            HeaderText="Review Level" SortExpression="ReviewLevel" UniqueName="ReviewLevel" HeaderTooltip="This column displays the Review Level for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RushOrderStatus" FilterControlAltText="Filter RushOrderStatus column" AllowSorting ="false" AllowFiltering="false"
                            HeaderText="Rush Order" SortExpression="RushOrderStatus" UniqueName="RushOrderStatus" HeaderTooltip="This column displays the Rush Order, if any, for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AssignedUserName" FilterControlAltText="Filter AssignedUserName column"
                            HeaderText="Assigned To User" SortExpression="AssignedUserName" UniqueName="AssignedUserName" HeaderTooltip="This column displays the user, if any,  who has been assigned to complete the verification for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ExceptionReason" FilterControlAltText="Filter ExceptionReason column"
                            HeaderText="Exception Reason" SortExpression="ExceptionReason" UniqueName="ExceptionReason" HeaderTooltip="This column displays the Exception Reason for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CustomAttributes" FilterControlAltText="Filter CustomAttributes column"
                            AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="200px"
                            UniqueName="CustomAttributes" HeaderTooltip="This column displays the Custom Attributes for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CustomAttributes" AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="300px"
                            UniqueName="CustomAttributesTemp" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfCatId" runat="server" Value='<%# Eval("CategoryId") %>' />
                                <asp:HiddenField ID="hdfPackSubscriptionId" runat="server" Value='<%# Eval("PackageSubscriptionId") %>' />
                                <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail" ToolTip="Click to open the verification screen for this Item"
                                    runat="server" Text="Detail">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False" OnClientShowing="MenuShowing">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="row">&nbsp;</div>
        <div class="row text-center" runat="server" id="pnlExpShowUsers" visible="false">
            <span class="cptn" title="Select a user and click assign to assign verification for all chosen items to that user">Select User</span>
            <infs:WclComboBox ID="ddlExpSelectedUser" runat="server" AutoPostBack="false" CssClass="form-control sameLine" Filter="Contains" Width="18%"
                DataTextField="FirstName" Skin="Silk" AutoSkinMode="false" DataValueField="OrganizationUserID" OnDataBound="ddlExpSelectedUser_DataBound">
            </infs:WclComboBox>
            <infs:WclButton ID="btnExpAssignUser" runat="server" AutoPostBack="true" Text="Assign"
                OnClick="btnExpAssignUser_Click" Skin="Silk" AutoSkinMode="false" Style="vertical-align: top !important;"
                ToolTip="Select a user and click assign to assign verification for all chosen items to that user" />
        </div>
        <div class="row">&nbsp;</div>
    </div>
</div>
