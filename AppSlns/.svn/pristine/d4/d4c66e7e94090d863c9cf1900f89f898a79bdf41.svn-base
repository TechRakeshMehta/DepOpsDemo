<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportClearstarServicesPopup.aspx.cs" Title="Import External Service" MasterPageFile="~/Shared/ChildPage.master" Inherits="CoreWeb.BkgSetup.Pages.Views.ImportClearstarServicesPopup" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .breakword
        {
            word-break: break-all;
        }

        .rgFilterRow
        {
            display: none !important;
        }
    </style>

    <script type="text/javascript">


        function UnCheckHeader(id, BSE_IDs) {
            //debugger;
            var checkHeader = true;
            var masterTable = $find("<%= grdClearStarSvc.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            AddBSE_IDsInList(id.checked, BSE_IDs);
            for (var i = 0; i < row.length; i++) {
                if (!(row[i].findElement("chkSelectItem").disabled)) {
                    if (!(row[i].findElement("chkSelectItem").checked)) {
                        checkHeader = false;
                        break;
                    }
                }
            }
            //$jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
        }

        var editBSEIDs = [];

        function AddBSE_IDsInList(isChecked, BSE_IDs) {
            //debugger;
            if (isChecked) {
                if (editBSEIDs.indexOf(BSE_IDs) < 0) {
                    editBSEIDs = [];
                    editBSEIDs.push(BSE_IDs);
                }
            }
            else {
                editBSEIDs = $jQuery.grep(editBSEIDs, function (value) {
                    return value != BSE_IDs;
                });
            }
            //debugger;
            var temp = [];
            var oldIds = $jQuery('[id$="hdnBSE_ID"]').val();
            if (oldIds != "" && oldIds != undefined) {
                temp.push(oldIds);
            }
            temp.push(editBSEIDs[0]);


            $jQuery('[id$="hdnBSE_ID"]').val(temp);
        }

        function CheckAll(id) {
            //debugger;
            var masterTable = $find("<%= grdClearStarSvc.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            var isChecked = false;
            if (id.checked == true) {
                isChecked = true;
            }
            for (var i = 0; i < row.length; i++) {
                if (!(row[i].findElement("chkSelectItem").disabled == true)) {
                    //debugger;
                    row[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
                    var serviceID = row[i].getDataKeyValue("CSS_ID");
                    AddBSE_IDsInList(isChecked, serviceID);
                }
            }
        }

        function pageLoad() {
            //debugger;
            CheckRadGridAllCheckBox('chkSelectItem', 'chkSelectAll');
        }

        //To Check the header Select All check Box in Rad Grid. Call this fucntion on "pageLoad()" JS function on design page.    
        function CheckRadGridAllCheckBox(chkBoxRadGridItemID, chkBoxSelectAllRadGridHeaderItemID) {
            //  debugger;
            var chkBoxRadGridItemList = $jQuery('input[type=checkbox][id$="' + chkBoxRadGridItemID + '"]');
            var chkBoxRadGridItemUncheckedList = $jQuery('input[type=checkbox][id*= "' + chkBoxRadGridItemID + '"]:not(:checked)');
            if (chkBoxRadGridItemUncheckedList.length == 0 && chkBoxRadGridItemList.length > 0) {
                $jQuery('input[type=checkbox][id$="' + chkBoxSelectAllRadGridHeaderItemID + '"]').attr('checked', 'checked');
            }
        }

        function show_progress_OnSubmit() {
            Page.showProgress('Processing...');
        }

    </script>

    <div class="section">
        <div class="content">
            <asp:Panel runat="server">
                <div class="sxform auto">
                    <div class="msgbox">
                        <asp:Label ID="lblMessage" runat="server"> </asp:Label>
                    </div>

                    <infs:WclGrid runat="server" ID="grdClearStarSvc" AllowPaging="false" AutoGenerateColumns="False"
                        AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" EnableViewState="true"
                        GridLines="Both" EnableDefaultFeatures="true" ShowAllExportButtons="False" ShowExtraButtons="false" ShowClearFiltersButton="false"
                        NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdClearStarSvc_NeedDataSource"
                        OnItemDataBound="grdClearStarSvc_ItemDataBound">
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="CSS_ID" ClientDataKeyNames="CSS_ID">
                            <CommandItemSettings ShowAddNewRecordButton="false"
                                ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                                ShowRefreshButton="true" />
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="ImportServices" AllowFiltering="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" runat="server" Visible="false" onclick="CheckAll(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelectItem" runat="server" onclick='<% # "UnCheckHeader(this,\"" + Eval("CSS_ID") + "\" );"%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="CSS_Name" AllowFiltering="false"
                                    HeaderText="Service Name" SortExpression="CSS_Name" UniqueName="CSS_Name">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="CSS_Number" ItemStyle-CssClass="breakword" AllowFiltering="false"
                                    HeaderText="External Service Code" SortExpression="CSS_Number" UniqueName="CSS_Number">
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
                <%--<div style="width: 100%; text-align: center; margin-top: 10px;" id="importClearServices" runat="server">
                    <infs:WclButton runat="server" ID="btnImportClearServices" Text="Import Services" OnClick="btnImportClearServices_Click">
                    </infs:WclButton>
                </div>--%>

                <div id="divButton" runat="server">
                    <infsu:CommandBar ID="fsucCmdBarImportServices" runat="server" GridMode="false" DefaultPanel="pnlImportServices" SaveButtonText="Import Services"
                        SaveButtonIconClass="" SubmitButtonIconClass="" DisplayButtons="Save,Submit" AutoPostbackButtons="Save,Submit" OnSaveClick="btnImportClearServices_Click"
                        SubmitButtonText="More Services" OnSubmitClick="btnMoreServices_Click" OnSubmitClientClick="show_progress_OnSubmit">
                    </infsu:CommandBar>
                </div>
            </asp:Panel>
        </div>
        <asp:HiddenField runat="server" ID="hdnBSE_ID" />
        <asp:HiddenField runat="server" ID="hdnCSS_ID" />
        <asp:HiddenField runat="server" ID="hdnRefreshIds" />
    </div>
</asp:Content>

