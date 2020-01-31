<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageMultipleSubscriptionsPopup.aspx.cs" MasterPageFile="~/Shared/ChildPage.master" Inherits="CoreWeb.SearchUI.Pages.Views.ManageMultipleSubscriptions" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxManageMultipleSubscriptionsPopup">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" /> 
        <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />    
</infs:WclResourceManagerProxy>  
    <%-- <style type="text/css">
    .rgRefresh
    {
        background-image: url('/Resources/Mod/Dashboard/images/refresh.png') !important;
        background-repeat: no-repeat !important;
        background-position: 1px 1px !important;
    }

      
</style>--%>
    <script type="text/javascript">
        function UnCheckHeader(id) {
            //debugger;
            var checkHeader = true;
            var masterTable = $find("<%= grdArchiveMultipleSubscriptions.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            for (var i = 0; i < row.length; i++) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectSubscription").disabled)) {
                    if (!(masterTable.get_dataItems()[i].findElement("chkSelectSubscription").checked)) {
                        checkHeader = false;
                        break;
                    }
                }
            }
            $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
        }

        ////var editSubscptionIDs = [];
        //function AddBSE_IDsInList(isChecked, BSE_IDs) {
        //    if (isChecked) {
        //        if (editBSEIDs.indexOf(BSE_IDs) < 0) {
        //            editBSEIDs = [];
        //            editBSEIDs.push(BSE_IDs);
        //        }
        //    }
        //    else {
        //        editBSEIDs = $jQuery.grep(editBSEIDs, function (value) {
        //            return value != BSE_IDs;
        //        });
        //    }
        //    var temp = [];
        //    var oldIds = $jQuery('[id$="hdnBSE_ID"]').val();
        //    if (oldIds != "" && oldIds != undefined) {
        //        temp.push(oldIds);
        //    }
        //    temp.push(editBSEIDs[0]);


        //    $jQuery('[id$="hdnBSE_ID"]').val(temp);
        //}

        function CheckAll(id) {
            var masterTable = $find("<%= grdArchiveMultipleSubscriptions.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            var isChecked = false;
            if (id.checked == true) {
                var isChecked = true;
            }
            for (var i = 0; i < row.length; i++) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectSubscription").disabled == true)) {
                    masterTable.get_dataItems()[i].findElement("chkSelectSubscription").checked = isChecked; // for checking the checkboxes
                }
            }
        }

        //function pageLoad() {
        //    CheckRadGridAllCheckBox('chkSelectSubscription', 'chkSelectAll');
        //}

        ////Function to check headerbox checkbox on page-load when all items are checked.
        //function CheckRadGridAllCheckBox(chkBoxRadGridItemID, chkBoxSelectAllRadGridHeaderItemID) {
        //    var chkBoxRadGridItemList = $jQuery('input[type=checkbox][id$="' + chkBoxRadGridItemID + '"]');
        //    var chkBoxRadGridItemUncheckedList = $jQuery('input[type=checkbox][id*= "' + chkBoxRadGridItemID + '"]:not(:checked)');
        //    if (chkBoxRadGridItemUncheckedList.length == 0 && chkBoxRadGridItemList.length > 0) {
        //        $jQuery('input[type=checkbox][id$="' + chkBoxSelectAllRadGridHeaderItemID + '"]').attr('checked', 'checked');
        //    }
        //}

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
        function ClosePopup()
        {
            var oArg = {};
            oArg.Action = "Cancel";
            top.$window.get_radManager().getActiveWindow().close();
        }

        //Function to redirect to parent 
        function RedirectToParent()
        {
            var oArg = {};
            oArg.Action = "Submit";
            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }
    </script>

    <div class="container-fluid">  <div class="row"><div class="col-md-12">
        <h2 class="header-color">
            <asp:Label ID="lblMultipleSubscriptionsPopup" runat="server" Text="Manage Multiple Subscriptions For Archival"></asp:Label>
        </h2></div></div>
        <div class="row">
            <asp:Panel ID="pnlMultipleSubscriptions" runat="server">
                <div class="sxform auto">
                    <div class="msgbox">
                        <asp:Label ID="lblMessage" runat="server"> </asp:Label>
                    </div>
                    <infs:WclGrid runat="server" ID="grdArchiveMultipleSubscriptions" AllowPaging="true" AutoGenerateColumns="False" AllowCustomPaging="false"
                        AllowSorting="False" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" EnableViewState="true"
                        GridLines="Both" EnableDefaultFeatures="true" ShowAllExportButtons="False" ShowExtraButtons="false" ShowClearFiltersButton="false"
                        NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdArchiveMultipleSubscriptions_NeedDataSource"
                        OnItemDataBound="grdArchiveMultipleSubscriptions_ItemDataBound">
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="PackageSubscriptionID" ClientDataKeyNames="PackageSubscriptionID">
                            <CommandItemSettings ShowAddNewRecordButton="false"
                                ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                                ShowRefreshButton="true" />
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="SelectSubscriptions" HeaderTooltip="Click this box to select all Subscriptions on the active page"
                                    AllowFiltering="false" ShowFilterIcon="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelectSubscription" runat="server" 
                                            onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectSubscription_CheckedChanged" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="PackageSubscriptionID" UniqueName="PackageSubscriptionID"
                                    Display="false" AllowSorting="false" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FirstName" HeaderText="Applicant First Name" AllowSorting="false" AllowFiltering="false"
                                    UniqueName="FirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LastName" HeaderText="Applicant Last Name" AllowSorting="false" AllowFiltering="false"
                                    SortExpression="LastName" UniqueName="LastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name" AllowSorting="false" AllowFiltering="false"
                                    UniqueName="InstituteName" HeaderTooltip="This column displays the applicant's User Name for each record in the grid">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UserGroup" HeaderText="User Group" AllowSorting="false" AllowFiltering="false"
                                    UniqueName="UserGroup" ItemStyle-Width="180px" HeaderTooltip="This column displays the applicant's User Group  for each record in the grid">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PackageName" HeaderText="Package Name" AllowSorting="false" AllowFiltering="false"
                                    UniqueName="PackageName" ItemStyle-Width="180px" HeaderTooltip="This column displays the applicant's Package Name  for each record in the grid">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="InstitutionHierarchy" HeaderText="Institution Hierarchy" AllowSorting="false" AllowFiltering="false"
                                    UniqueName="InstitutionHierarchy" ItemStyle-Width="180px" HeaderTooltip="This column displays the Institution Hierarchy  for each record in the grid">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                        </MasterTableView>
                        <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </infs:WclGrid>
                    <div class="gclr">
                    </div>
                </div>
                <div id="divButton" runat="server">
                    <infsu:CommandBar ID="cmdBarMultipleSubscriptions" ButtonSkin="Silk" UseAutoSkinMode="false" runat="server" GridMode="false" DefaultPanel="pnlMultipleSubscriptions"
                        DisplayButtons="Submit,Cancel" AutoPostbackButtons="Submit,Cancel"
                        SubmitButtonText="Archive Selected Subscriptions" SubmitButtonIconClass="" OnSubmitClick="cmdBarMultipleSubscriptions_SubmitClick" OnSubmitClientClick="show_progress_OnSubmit"
                        CancelButtonText="Cancel" OnCancelClientClick="ClosePopup">
                    </infsu:CommandBar>
                </div>
            </asp:Panel>
        </div>
    </div>

    <asp:HiddenField ID="hdnSelectedSubscriptions" runat="server" />

</asp:Content>

