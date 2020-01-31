<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ItemsDataVerificationQueue" CodeBehind="ItemsDataVerificationQueue.ascx.cs" %>
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
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    /*body {
        background-color:#fff;
    }*/
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
    /*.setPosition {
        display: inline-block;
    }*/

    .rgFooter {
        background-color: #f7f8f8 !important;
    }

    .RadGrid_Default .rgFooter td {
        border-bottom-style: none !important;
    }
    /*  tfoot td.rgPagerCell {
        border:none !important;
    }*/
</style>

<script type="text/javascript">
    function CheckAll(id) {
        var masterTable = $find("<%= grdVerificationItemData.ClientID %>").get_masterTableView();
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
        $jQuery('[id$=chkSelectAll]')[0].checked = isChecked;
        $jQuery('[id$=chkSelectAllFooter]')[0].checked = isChecked;
    }
    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdVerificationItemData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader
        $jQuery('[id$=chkSelectAllFooter]')[0].checked = checkHeader;
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
<div class="approvepopup" runat="server" style="display: none">
    <asp:Label ID="lblConfirmMessage" runat="server"></asp:Label>
</div>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblVerificationQueue" runat="server" Text=""></asp:Label></h2>
        </div>
    </div>

    <div class="row bgLightGreen">
        <asp:Panel runat="server" ID="pnlShowFilters">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class='cptn'>Institution</span>
                            <%--<infs:WclDropDownList ID="ddlTenantName" runat="server" AutoPostBack="true" OnItemSelected="ddlTenantName_ItemSelected" Enabled="false"
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
                            AutoPostBack="true" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
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
                    <div runat="server" id="dvIncmplt">
                        <div class='form-group col-md-3'>
                            <span class='cptn'>Include Incomplete Items</span>
                            <asp:CheckBox ID="chkShowIncompleteItems" runat="server" Checked="false" Width="100%" CssClass="form-control" />
                        </div>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Show only Rush Orders</span>
                        <asp:CheckBox ID="chkShowRushOrders" runat="server" Checked="false" Width="100%" CssClass="form-control" />
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
                    <asp:CheckBox ID="chkSelectAllResults" Visible="false" Text="Select All Results" runat="server" OnCheckedChanged="chkSelectAllResults_CheckedChanged"
                        AutoPostBack="true" Width="100%" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group col-md-9">
                <div class="row text-center">
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
    <asp:UpdatePanel ID="pnlVerError" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="msgbox" id="VerMsgBox">
                <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblVerError"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <input type="button" onclick="return false;" />
    <div id="pnlVerification" visible="false" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">
                    <asp:Label Text="Data Verification Queue" runat="server" ID="lblPageHdr" /></h2>
            </div>
        </div>
        <div class="row allowscroll" id="linkHover">
            <infs:WclGrid runat="server" ID="grdVerificationItemData" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                NonExportingColumns="AssignItems,ViewDetail,CustomAttributes,IsUiRulesViolate" GridLines="Both" OnNeedDataSource="grdVerificationItemData_NeedDataSource"
                OnItemDataBound="grdVerificationItemData_ItemDataBound" ShowAllExportButtons="False"
                OnItemCommand="grdVerificationItemData_ItemCommand" AllowCustomPaging="true"
                OnSortCommand="grdVerificationItemData_SortCommand" OnInit="grdVerificationItemData_Init">
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" OnFilterMenuShowing="filterMenuShowing"/>
                    <Selecting AllowRowSelect="true"></Selecting>
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
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" ToolTip="Click this box to select all users on the active page" />
                            </HeaderTemplate>
                            <FooterTemplate>
                                <label style="width: 100px;">
                                    <asp:CheckBox ID="chkSelectAllFooter" runat="server" onclick="CheckAll(this)" ToolTip="Click this box to select all users on the active page" />
                                    Select All
                                </label>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectItem" runat="server" onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectVerItem_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridBoundColumn DataField="ApplicantName" FilterControlAltText="Filter ApplicantName column"
                            HeaderText="Applicant Name" SortExpression="ApplicantName" UniqueName="ApplicantName"
                            HeaderTooltip="This column displays the applicant's name for each record in the grid">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column"
                            HeaderText="Applicant First Name" SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName"
                            HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column"
                            HeaderText="Applicant Last Name" SortExpression="ApplicantLastName" UniqueName="ApplicantLastName"
                            HeaderTooltip="This column displays the applicant's last name for each record in the grid">
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
                            HeaderText="Verification Status" SortExpression="VerificationStatus" UniqueName="VerificationStatus" AllowSorting ="false" AllowFiltering="false"
                            HeaderTooltip="This column displays the applicant's overall compliance status for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ReviewLevel" FilterControlAltText="Filter ReviewLevel column" AllowSorting ="true"
                            HeaderText="Review Level" SortExpression="ReviewLevel" UniqueName="ReviewLevel" HeaderTooltip="This column displays the Review Level for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SystemStatus" FilterControlAltText="Filter SystemStatus column"
                            HeaderText="System Status" SortExpression="SystemStatus" UniqueName="SystemStatus" AllowSorting ="false" AllowFiltering="false"
                            HeaderTooltip="This column displays the system suggested Item Compliance, if a compliance rule has been applied at the Item level">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RushOrderStatus" FilterControlAltText="Filter RushOrderStatus column"
                            HeaderText="Rush Order" SortExpression="RushOrderStatus" UniqueName="RushOrderStatus" AllowSorting ="false" AllowFiltering="false"
                            HeaderTooltip="This column displays the Rush Order, if any, for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="AssignedUserName" FilterControlAltText="Filter AssignedUserName column"
                            HeaderText="Assigned To User" SortExpression="AssignedUserName" UniqueName="AssignedUserName"
                            HeaderTooltip="This column displays the user, if any,  who has been assigned to complete the verification for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CustomAttributes" FilterControlAltText="Filter CustomAttributes column"
                            AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="200px"
                            UniqueName="CustomAttributes" HeaderTooltip="This column displays the Custom Attributes for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CustomAttributes" AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="300px"
                            UniqueName="CustomAttributesTemp" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsUiRulesViolate" AllowFiltering="false" HeaderText="IsUiRulesViolate" AllowSorting="false" ItemStyle-Width="300px"
                            UniqueName="IsUiRulesViolate" Display="false">
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfCatId" runat="server" Value='<%# Eval("CategoryId") %>' />
                                <asp:HiddenField ID="hdfPackSubscriptionId" runat="server" Value='<%# Eval("PackageSubscriptionId") %>' />
                                <%--  <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail"
                                    runat="server" Text="Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                </telerik:RadButton>--%>
                                <telerik:RadButton ID="btnEditNew" ButtonType="LinkButton" CommandName="ViewDetailNew"
                                    ToolTip="Click to open the verification screen for this Item" runat="server"
                                    Text="Detail">
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
        <div class="row text-center" runat="server" id="pnlVerShowUsers"
            visible="false">
            <span title="Select a user and click assign to assign verification for all chosen items to that user" class="cptn">Select User</span>
            <infs:WclComboBox ID="ddlVerSelectedUser" runat="server" AutoPostBack="false" Filter="Contains"
                DataTextField="FirstName" DataValueField="OrganizationUserID" OnDataBound="ddlVerSelectedUser_DataBound"
                CssClass="form-control sameLine" Skin="Silk" AutoSkinMode="false">
            </infs:WclComboBox>
            <infs:WclButton ID="btnVerAssignUser" runat="server" AutoPostBack="true" Text="Assign" Style="vertical-align: top !important;"
                OnClick="btnVerAssignUser_Click" ToolTip="Select a user and click assign to assign verification for all chosen items to that user"
                Skin="Silk" AutoSkinMode="false" />
            <infs:WclButton ID="btnAutomaticItemAssigning" runat="server" AutoPostBack="true" Text="Auto Assign" Style="vertical-align: top !important;"
                OnClick="btnAutomaticItemAssigning_Click" ToolTip="Automatic assign to assign verification for all items to ADB admin user(s)"
                Skin="Silk" AutoSkinMode="false" />
        </div>
        <div class="row">&nbsp;</div>
    </div>
</div>
<asp:HiddenField ID="hdnIsMutipleTimesAssignmentAllowed" runat="server" Value="false" />
