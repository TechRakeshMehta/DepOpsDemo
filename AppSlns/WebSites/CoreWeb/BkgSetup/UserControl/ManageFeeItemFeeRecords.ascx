<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageFeeItemFeeRecords.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageFeeItemFeeRecords" %>


<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div id="divAddFeeRecords" runat="server">
    <div class="section">
        <div class="content">
            <div class="page_cmd" style="float: right">
                <infs:WclButton runat="server" ID="btnAddFeeRecord" Text="+ Add Fee Record" OnClick="btnAddFeeRecord_Click"
                    ButtonType="LinkButton" Height="30px">
                </infs:WclButton>
            </div>
        </div>
    </div>
    <div id="divFeeRecord" runat="server" visible="false">
        <div class="content">
            <h1 class="mhdr">
                <asp:Label ID="lblEHAttr" Text="Add New Fee Record"
                    runat="server" /></h1>
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlReviewer">
                    <div class='sxro sx3co'>
                        <div id="divStateCounty" runat="server" visible="true">
                            <div class='sxlb'>
                                <span class="cptn">State</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbState" AutoPostBack="true" runat="server" DataTextField="StateName"
                                    DataValueField="StateID" OnSelectedIndexChanged="cmbState_SelectedIndexChanged">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvState" ControlToValidate="cmbState"
                                        InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFeeRecord" CssClass="errmsg"
                                        Text="State is required." />
                                </div>
                            </div>
                        </div>
                        <div id="divCounty" runat="server" visible="false">
                            <div class='sxlb'>
                                <span class="cptn">County</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbCounty" runat="server" DataTextField="CountyName" AutoPostBack="true"
                                    DataValueField="CountyID" OnSelectedIndexChanged="cmbCounty_SelectedIndexChanged">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvCounty" ControlToValidate="cmbCounty"
                                        InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFeeRecord" CssClass="errmsg"
                                        Text="County is required." />
                                </div>
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Amount</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ShowSpinButtons="True" Type="Currency" ID="txtAmount"
                                MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                MinValue="-2147483647">
                                <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                            </infs:WclNumericTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvAmount" ControlToValidate="txtAmount"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Amount is required."
                                    ValidationGroup='grpFeeRecord' />
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Global Fee Amount</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtGlobalFee" runat="server" Text="" Enabled="false"></infs:WclTextBox>

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
                    ValidationGroup="grpFeeRecord">
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
            <asp:Label ID="lblFeeRecord" runat="server" Text="Fee Records"></asp:Label>
        </h1>
        <div class="content">
            <infs:WclGrid runat="server" ID="grdFeeRecord" AllowPaging="false"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
                AllowFilteringByColumn="false" AllowSorting="True" EnableDefaultFeatures="false"
                ShowAllExportButtons="False" ShowClearFiltersButton="false"
                OnNeedDataSource="grdFeeRecord_NeedDataSource" OnDeleteCommand="grdFeeRecord_DeleteCommand"
                EnableLinqExpressions="false">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="LocalSFRID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowExportToExcelButton="false" ShowExportToCsvButton="false"
                        ShowRefreshButton="true" />
                    <Columns>
                        <%--OnItemCreated="grdFeeRecord_ItemCreated"--%>
                        <%-- <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="State" UniqueName="State">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server"></asp:Label>
                            </ItemTemplate>
                     </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="County" UniqueName="County">
                            <ItemTemplate>
                                <asp:Label ID="lblCounty" runat="server"></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridBoundColumn DataField="StateName" HeaderText="State" UniqueName="State"
                            SortExpression="StateName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CountyName" HeaderText="County" UniqueName="County"
                            SortExpression="CountyName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LocalSFRAmount" HeaderText="Amount($)" DataType="System.Decimal"
                            SortExpression="LocalSFRAmount" UniqueName="LocalSFRAmount" DataFormatString="{0:c}">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="GlobalFeeAmount" HeaderText="Global Fee Amount($)"
                            SortExpression="GlobalFeeAmount" UniqueName="GlobalFeeAmount" DataFormatString="{0:c}">
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Record?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>
</div>
