<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageSvcItemCustomForms.ascx.cs" Inherits="CoreWeb.BkgSetup.UserControl.ManageSvcItemCustomForms" %>


<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <div class="content">
        <div class="page_cmd" style="float: right">
            <infs:WclButton runat="server" ID="btnAddCustomForm" Text="+ Map Custom Form" OnClick="btnAddCustomForm_Click"
                ButtonType="LinkButton" Height="30px">
            </infs:WclButton>
        </div>
    </div>
</div>
<div id="divCustomForm" runat="server" visible="false">
    <div class="content">
        <h1 class="mhdr">
            <asp:Label ID="lblEHAttr" Text="Map New Custom Form"
                runat="server" /></h1>
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlReviewer">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Select Custom Form</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbCustomForm" runat="server"
                            DataTextField="CF_Name" DataValueField="CF_ID">
                        </infs:WclComboBox>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvCustomForm" ControlToValidate="cmbCustomForm" InitialValue="--SELECT--"
                                class="errmsg" ValidationGroup="grpCustomForm" Display="Dynamic" ErrorMessage="Custom Form is required." />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
<div id="divButtonSave" runat="server" visible="false">
    <div class="sxcbar">
        <div class="sxcmds" style="text-align: right">
            <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                ValidationGroup="grpCustomForm">
                <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                    PrimaryIconWidth="14" />
            </infs:WclButton>
            <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
                <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                    PrimaryIconWidth="14" />
            </infs:WclButton>
        </div>
    </div>
</div>

<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblCustomForm" runat="server" Text="Custom Forms"></asp:Label>
    </h1>
    <div class="content">
        <infs:WclGrid runat="server" ID="grdManageSvcItemCustomForms" AllowPaging="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
            AllowFilteringByColumn="false" AllowSorting="True" EnableDefaultFeatures="false"
            ShowAllExportButtons="False" ShowClearFiltersButton="false" OnNeedDataSource="grdManageSvcItemCustomForms_NeedDataSource"
            OnDeleteCommand="grdManageSvcItemCustomForms_DeleteCommand">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="BSIFM_ID,CF_ID" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridBoundColumn DataField="CF_Title" HeaderText="Title" SortExpression="CF_Title" UniqueName="CF_Title">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CF_Name" HeaderText="Name" SortExpression="CF_Name" UniqueName="CF_Name">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CF_Description" HeaderText="Description" SortExpression="CF_Description" UniqueName="CF_Description">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CF_Sequence" HeaderText="Sequence" SortExpression="CF_Sequence" UniqueName="CF_Sequence">                       
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CFT_Name" HeaderText="Custom Form Type" SortExpression="CFT_Name" UniqueName="CFT_Name">                       
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this record?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>
