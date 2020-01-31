<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapServicesToClient.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.MapServicesToClient" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    .rgFilterRow
    {
        display: none !important;
    }
</style>

<script type="text/javascript">
    function UnCheckHeader(id, BSE_IDs) {
        //debugger;
        var checkHeader = true;
        var masterTable = $find("<%= grdService.ClientID %>").get_masterTableView();
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
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    var editBSEIDs = [];

    function AddBSE_IDsInList(isChecked, BSE_IDs) {
        // debugger;
        if ($jQuery('[id$="hdnRefreshIds"]').val() == "true") {
            editBSEIDs = [];
            $jQuery('[id$="hdnRefreshIds"]').val("false")
        }
        if (isChecked) {
            if (editBSEIDs.indexOf(BSE_IDs) < 0) {
                editBSEIDs.push(BSE_IDs);
            }
        }
        else {
            editBSEIDs = $jQuery.grep(editBSEIDs, function (value) {
                return value != BSE_IDs;
            });
        }

        $jQuery('[id$="hdnBSE_ID"]').val(editBSEIDs);
    }

    function CheckAll(id) {
        // debugger;
        var masterTable = $find("<%= grdService.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(row[i].findElement("chkSelectItem").disabled == true)) {
                //debugger;
                row[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
                var serviceID = row[i].getDataKeyValue("BSE_ID");
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

</script>
<div class="section">
    <h1 class="mhdr">Map Services To Client
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server"> </asp:Label><%--CssClass="info"--%>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx3co'>
                    <%-- <div class='sxlb'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                    </div>--%>
                    <div class='sxlb' title="Select the institution whose data you want to view">
                        <span class='cptn'>Institution</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            DataValueField="TenantID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"   >
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:Label Text="Institution is required." CssClass="errmsg" Visible="false" ID="lblInstitution" runat="server" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenant" ControlToValidate="ddlTenant" InitialValue="--Select--"
                                Display="Dynamic" CssClass="errmsg" Text="Institution is required."  />
                        </div>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblSvcId" runat="server" Text="Service Id" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclNumericTextBox runat="server" ID="txtSvcIDs" MinValue="1" NumberFormat-DecimalDigits="0"
                            NumberFormat-AllowRounding="false" MaxLength="9">
                        </infs:WclNumericTextBox>
                        <div class="vldx">
                            <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtSvcIDs"
                                Display="Dynamic" CssClass="errmsg" Text="Service Id is required." />--%>
                        </div>
                    </div>
                    <div class='sxlb'>
                        <asp:Label ID="lblName" runat="server" Text="Service Name" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtName" runat="server"></infs:WclTextBox>
                        <div class="vldx">
                            <%-- <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                Display="Dynamic" CssClass="errmsg" Text="Name is required." />--%>
                        </div>
                    </div>
                    <div class='sxlb'>
                        <asp:Label ID="lblCode" runat="server" Text="External Code" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtCode" runat="server"></infs:WclTextBox>
                        <div class="vldx">
                            <%-- <asp:RequiredFieldValidator runat="server" ID="rfvCode" ControlToValidate="txtCode"
                                Display="Dynamic" CssClass="errmsg" Text="External Code is required." />--%>
                        </div>
                    </div>
                </div>
                <div class='sxroend'>
                </div>

            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucSearchCmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel" 
            AutoPostbackButtons="Submit,Save,Cancel" SaveButtonIconClass="rbSearch" SaveButtonText="Search"
            CancelButtonText="Cancel" SubmitButtonText="Reset" SubmitButtonIconClass="rbRefresh"
            OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click"
            OnCancelClick="CmdBarCancel_Click">
        </infsu:CommandBar>

        <div class='sxroend'>
        </div>
    </div>
    <div class="swrap" runat="server" id="dvServiceGroup">
        <infs:WclGrid runat="server" ID="grdService" AllowPaging="True" AutoGenerateColumns="False"
            AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
            GridLines="Both" EnableDefaultFeatures="true" ShowAllExportButtons="False" ShowExtraButtons="false" ShowClearFiltersButton="false"
            NonExportingColumns="EditCommandColumn,DeleteColumn"
            OnItemCommand="grdService_ItemCommand" OnNeedDataSource="grdService_NeedDataSource" OnItemDataBound="grdService_ItemDataBound">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="BSE_ID" ClientDataKeyNames="BSE_ID">
                <CommandItemSettings ShowAddNewRecordButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                    ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="AlreadyMappedServices" AllowFiltering="false" HeaderText="Already Mapped Services">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectItem" runat="server" onclick='<% # "UnCheckHeader(this,\"" + Eval("BSE_ID") + "\" );"%>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn DataField="BSE_ID" AllowFiltering="false"
                        HeaderText="Service ID" SortExpression="BSE_ID" UniqueName="BSE_ID">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="BSE_Name" AllowFiltering="false"
                        HeaderText="Service Name" SortExpression="BSE_Name" UniqueName="BSE_Name">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EBS_ExternalCode" AllowFiltering="false"
                        HeaderText="External Code" SortExpression="EBS_ExternalCode" UniqueName="EBS_ExternalCode">
                    </telerik:GridBoundColumn>

                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DeactivateMappings">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnDeactivate" ButtonType="LinkButton" CommandName="DeactivateMapping" Visible="false"
                                runat="server" Text="Deactivate Mapping" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                            </telerik:RadButton>
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
<asp:HiddenField runat="server" ID="hdnBSE_ID" />
<asp:HiddenField runat="server" ID="hdnRefreshIds" />
<asp:HiddenField runat="server" ID="hdnPreMappedIDs" />
<infsu:CommandBar ID="fsucCmdBar1" runat="server" DisplayButtons="Submit" AutoPostbackButtons="Submit"
    SubmitButtonText="Save Mapping" ButtonPosition="Center"
    DefaultPanel="pnlTenant" OnSubmitClick="fsucCmdBar1_SubmitClick" Visible="false" />
