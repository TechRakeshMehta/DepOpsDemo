<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.PackagePrice"
    Title="PackagePrice" MasterPageFile="~/Shared/ChildPage.master" Codebehind="PackagePrice.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
    </script>
    <%-- <div class="section" id="divShowMessage" runat="server" visible="false">
        <h1 class="mhdr">
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </h1>
    </div>--%>
    <div class="section" id="divPrice" runat="server">
        <div class="content">
            <div class="sxform auto">
                <asp:Panel ID="pnlPrice" CssClass="sxpnl" runat="server">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtName" runat="server" Enabled="false">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Price</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ID="txtPrice" Type="Currency" runat="server" MinValue="0" MaxLength ="9">
                            </infs:WclNumericTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPrice" ControlToValidate="txtPrice"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Price is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Total Price</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ID="txtTotalPrice" Type="Currency" runat="server" MinValue="0"
                                Enabled="false">
                            </infs:WclNumericTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Rush Order Price</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ID="txtRushOrderAdditionalPrice" Type="Currency" runat="server"
                                MinValue="0" MaxLength="9">
                            </infs:WclNumericTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divButton" runat="server">
                <infsu:CommandBar ID="fsucCmdBarPrice" runat="server" DefaultPanel="pnlPrice" DisplayButtons="Save,Cancel"
                    AutoPostbackButtons="Save,Cancel" OnSaveClick="fsucCmdBarPrice_SaveClick" OnCancelClick="fsucCmdBarPrice_CancelClick"
                    ValidationGroup="grpFormSubmit">
                </infsu:CommandBar>
            </div>
        </div>
    </div>
    <div id="divContent" runat="server">
        <div class="section">
            <h1 class="mhdr">
                <asp:Label ID="lblTitle" runat="server" Text="Price Adjustments"></asp:Label>
            </h1>
            <div class="content">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdPriceAdjustmentData" AllowPaging="True" AutoGenerateColumns="False"
                        AllowSorting="True" AllowCustomPaging="True" AllowFilteringByColumn="True" AutoSkinMode="True"
                        CellSpacing="0" GridLines="None" ShowAllExportButtons="False" EnableDefaultFeatures="True"
                        OnNeedDataSource="grdPriceAdjustmentData_NeedDataSource" OnItemCreated="grdPriceAdjustmentData_ItemCreated"
                        OnItemCommand="grdPriceAdjustmentData_ItemCommand" OnInsertCommand="grdPriceAdjustmentData_InsertCommand"
                        OnUpdateCommand="grdPriceAdjustmentData_UpdateCommand" OnInit="grdPriceAdjustmentData_Init" OnItemDataBound="grdPriceAdjustmentData_ItemDataBound">
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="ID,PriceAdjustmentID">
                            <CommandItemSettings ShowAddNewRecordButton="True" AddNewRecordText="Add New Price Adjustment"
                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                                ShowRefreshButton="true" />
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <Columns>
                                <%--<telerik:GridTemplateColumn UniqueName="TreeNodeType" AllowFiltering="false" ShowFilterIcon="false">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnTreeNodeType" runat="server" Value='<%#Eval("TreeNodeType")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                                <telerik:GridBoundColumn DataField="PriceAdjustmentLabel" FilterControlAltText="Filter PriceAdjustmentLabel column"
                                    HeaderText="Price Adjustment" SortExpression="PriceAdjustmentLabel" UniqueName="PriceAdjustmentLabel">
                                </telerik:GridBoundColumn>
                                <telerik:GridNumericColumn DataField="PriceAdjustmentValue" FilterControlAltText="Filter PriceAdjustmentValue column"
                                    HeaderText="Price Adjustment Value ($)" SortExpression="PriceAdjustmentValue"
                                    UniqueName="PriceAdjustmentValue" DataType="System.Decimal" DecimalDigits="2"
                                    DataFormatString="{0:###,##0.00}">
                                </telerik:GridNumericColumn>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                                    <HeaderStyle CssClass="tplcohdr" />
                                    <ItemStyle CssClass="MyImageButton" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Price Adjustment?"
                                    Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle CssClass="tplcohdr" />
                                    <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings EditFormType="Template">
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                                <FormTemplate>
                                    <div class="section">
                                        <h1 class="mhdr">
                                            <asp:Label ID="lblPriceAdjustmentEdit" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Price Adjustment" : "Update Price Adjustment" %>'
                                                runat="server" /></h1>
                                        <div class="content">
                                            <!-- Note: Please donot insert anything here. There should be nothing between content and form divs -->
                                            <div class="sxform auto">
                                                <div class="msgbox">
                                                    <asp:Label ID="lblProgramEditMsg" runat="server" CssClass="info"></asp:Label>
                                                </div>
                                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                                                    <!-- MD: Added for ID -->
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox runat="server" Text='<%# (Container is GridEditFormInsertItem) ? null:Eval("ID") %>'
                                                            ID="txtID" Visible="false">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class='sxro sx3co'>
                                                        <div class='sxlb'>
                                                            <span class="cptn">Price Adjustment</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclComboBox ID="ddlPriceAdjustment" runat="server" DataTextField="Label" DataValueField="PriceAdjustmentID"
                                                                OnDataBound="ddlPriceAdjustment_DataBound">
                                                            </infs:WclComboBox>
                                                            <div class='vldx'>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvPriceAdjustment" ControlToValidate="ddlPriceAdjustment"
                                                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Price Adjustment."
                                                                    InitialValue="--SELECT--" />
                                                            </div>
                                                        </div>
                                                        <div class='sxlb'>
                                                            <span class="cptn">Price Adjustment Value</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclNumericTextBox ID="txtPriceAdjustmentValue" Type="Currency" runat="server" MaxLength ="9"
                                                                Text='<%# (Container is GridEditFormInsertItem) ? null: Eval("PriceAdjustmentValue")%>'>
                                                            </infs:WclNumericTextBox>
                                                            <div class='vldx'>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvPriceAdjustmentValue" ControlToValidate="txtPriceAdjustmentValue"
                                                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Price Adjustment Value is required." />
                                                            </div>
                                                        </div>
                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                            <infsu:CommandBar ID="fsucCmdBar1" runat="server" GridMode="true" DefaultPanel="pnlName1" GridInsertText="Save" GridUpdateText="Save"
                                                ValidationGroup="grpFormSubmit" />
                                        </div>
                                    </div>
                                </FormTemplate>
                            </EditFormSettings>
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
        </div>
    </div>
</asp:Content>
