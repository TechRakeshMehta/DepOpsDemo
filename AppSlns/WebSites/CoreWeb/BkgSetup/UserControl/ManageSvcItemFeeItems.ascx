<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageSvcItemFeeItems.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageSvcItemFeeItems" %>


<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <div class="content">
        <div class="page_cmd" style="float: right">
            <infs:WclButton runat="server" ID="btnAddFeeItem" Text="+ Add Fee Item" OnClick="btnAddFeeItem_Click"
                ButtonType="LinkButton" Height="30px">
            </infs:WclButton>
        </div>
    </div>
</div>
<div id="divFeeItem" runat="server" visible="false">
    <div class="content">
        <h1 class="mhdr">
            <asp:Label ID="lblEHAttr" Text="Add New Fee Item"
                runat="server" /></h1>
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlReviewer">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Fee Item Name</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtFeeItemName" Text='<%# Eval("PSIF_Name") %>'
                            MaxLength="256">
                        </infs:WclTextBox>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvtxtFeeItemName" ControlToValidate="txtFeeItemName"
                                class="errmsg" ValidationGroup="grpFeeItem" Display="Dynamic" ErrorMessage="Fee Item Name is required." />
                        </div>
                    </div>
                    <%--<div class='sxlb'>
                        <span class="cptn">Fee Item Label</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox runat="server" ID="txtFeeItemLabel" Text='<%# Eval("PSIF_Label") %>'
                            MaxLength="256">
                        </infs:WclTextBox>
                    </div>--%>
                    <div class='sxlb'>
                        <span class="cptn">Select Fee Item Type</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="ddlFeeItemType" runat="server"
                            DataTextField="SIFT_Name" DataValueField="SIFT_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlFeeItemType_SelectedIndexChanged">
                        </infs:WclComboBox>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvCustomFormConfigAttrGroup" ControlToValidate="ddlFeeItemType" InitialValue="--Select--"
                                class="errmsg" ValidationGroup="grpFeeItem" Display="Dynamic" ErrorMessage="Fee Item Type is required." />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Description</span>
                    </div>
                    <div class='sxlm m2spn'>
                        <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("PSIF_Description") %>'
                            MaxLength="1024">
                        </infs:WclTextBox>
                    </div>
                    <div id="divFixedTypeAmount" runat="server" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Amount</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ShowSpinButtons="True" Type="Currency" ID="txtFixedTypeAmount"
                                MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                MinValue="-2147483647">
                                <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                            </infs:WclNumericTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvFixedTypeAmount" ControlToValidate="txtFixedTypeAmount"
                                    class="errmsg" ValidationGroup="grpFeeItem" Display="Dynamic" ErrorMessage="Fee Item Amount is required." />
                            </div>
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
                ValidationGroup="grpFeeItem">
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
        <asp:Label ID="lblFeeItem" runat="server" Text="Fee Items"></asp:Label>
    </h1>
    <div class="content">
        <infs:WclGrid runat="server" ID="grdManageSvcItemFeeItems" AllowPaging="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
            AllowFilteringByColumn="false" AllowSorting="True" EnableDefaultFeatures="false"
            ShowAllExportButtons="False" ShowClearFiltersButton="false" OnNeedDataSource="grdManageSvcItemFeeItems_NeedDataSource"
             OnDeleteCommand="grdManageSvcItemFeeItems_DeleteCommand">
            <ClientSettings EnableRowHoverStyle="true">
                <%--OnUpdateCommand="grdManageSvcItemFeeItems_UpdateCommand" OnItemCreated="grdManageSvcItemFeeItems_ItemCreated"
            OnInsertCommand="grdManageSvcItemFeeItems_InsertCommand"--%>
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="PSIF_ID" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridBoundColumn DataField="FeeItemName" HeaderText="Fee Item Name"
                        SortExpression="FeeItemName" UniqueName="FeeItemName">
                    </telerik:GridBoundColumn>
                    <%-- <telerik:GridBoundColumn DataField="PSIF_Label" HeaderText="Fee Item Label"
                        SortExpression="PSIF_Label" UniqueName="PSIF_Label">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn DataField="FeeItemType" HeaderText="Fee Item Type" SortExpression="FeeItemType" UniqueName="FeeItemType">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FeeItemDescription" HeaderText="Fee Item Description"
                        SortExpression="FeeItemDescription" UniqueName="FeeItemDescription">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FeeItemAmount"  HeaderText="Amount($)" DataType="System.Decimal" DataFormatString="{0:c}"
                        SortExpression="FeeItemAmount" UniqueName="FeeItemAmount">
                    </telerik:GridBoundColumn>
                   <%-- <telerik:GridTemplateColumn HeaderText="Amount" UniqueName="SIFRAmount" SortExpression="ServiceItemFeeRecords.SIFR_Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblServiceItemFeeRecordAmount" runat="server"></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Fee Item ?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>
