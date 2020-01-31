<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.ManageSubscriptionOption" Codebehind="ManageSubscriptionOption.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">
        Manage Subscription Options</h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                        </infs:WclDropDownList>--%>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID"  EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                           Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">  
                        </infs:WclComboBox>  
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="dvSubscriptionOptions" runat="server" class="swrap">
            <infs:WclGrid runat="server" ID="grdSubscriptionOptions" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                NonExportingColumns="EditCommandColumn,DeleteColumn" GridLines="Both" OnNeedDataSource="grdSubscriptionOptions_NeedDataSource"
                OnItemCommand="grdSubscriptionOptions_ItemCommand" OnInsertCommand="grdSubscriptionOptions_InsertCommand"
                OnItemDataBound="grdSubscriptionOptions_ItemDataBound" OnUpdateCommand="grdSubscriptionOptions_UpdateCommand"
                OnDeleteCommand="grdSubscriptionOptions_DeleteCommand"  EnableLinqExpressions="false" >
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="true" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="SubscriptionOptionID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Subscription Option"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Label" FilterControlAltText="Filter Label column"
                            HeaderText="Label" SortExpression="Label" UniqueName="Label">
                        </telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="Year" FilterControlAltText="Filter Year column"
                            HeaderText="Year(s)" SortExpression="Year" UniqueName="Year">
                        </telerik:GridNumericColumn>
                        <telerik:GridNumericColumn DataField="Month" FilterControlAltText="Filter Month column"
                            HeaderText="Month(s)" SortExpression="Month" UniqueName="Month"  DataType="System.Int32" >
                        </telerik:GridNumericColumn>
                        <telerik:GridBoundColumn DataField="IsSystem" FilterControlAltText="Filter IsSystem column"
                            HeaderText="Is System" SortExpression="IsSystem" UniqueName="IsSystem">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Subscription Option?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" runat="server" id="divEditBlock" visible="true">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHSubscriptionOption" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Subscription Option" : "Update Subscription Option" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlSubscriptionOption">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Label</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtLabel" runat="server" Text='<%# Eval("Label") %>' MaxLength="50">
                                                    </infs:WclTextBox>
                                                    <div id="dvLabel" class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvLabel" ControlToValidate="txtLabel"
                                                            class="errmsg" ErrorMessage="Label is required." ValidationGroup='grpManageSubscriptionOption'
                                                            Enabled="true" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Year(s)</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclNumericTextBox ID="ntxtYear" runat="server" Type="Number" Text='<%# Eval("Year") %>'
                                                        MinValue="0" onpaste="return false;" MaxLength="3">
                                                        <NumberFormat AllowRounding="false" DecimalDigits="0" DecimalSeparator="." />
                                                        <ClientEvents OnKeyPress="NumericTextBoxKeyPress" />
                                                    </infs:WclNumericTextBox>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Month(s)</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclNumericTextBox ID="ntxtMonth" runat="server" Type="Number" Text='<%# Eval("Month") %>'
                                                        MinValue="0" onpaste="return false;" MaxLength="3">
                                                        <NumberFormat AllowRounding="false" DecimalDigits="0" DecimalSeparator="." />
                                                        <ClientEvents OnKeyPress="NumericTextBoxKeyPress" />
                                                    </infs:WclNumericTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro' style="width: 100%;">
                                                <div class='sxlb' style="width: 15%;">
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm' style="width: 85%;">
                                                    <infs:WclTextBox ID="txtDescription" runat="server" Text='<%# Eval("Description") %>'
                                                        TextMode="MultiLine" MaxLength="1024" Height="50px">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarSubscriptionOption" runat="server" GridMode="true" GridInsertText="Save" GridUpdateText="Save"
                                        DefaultPanel="pnlSubscriptionOption" ValidationGroup="grpManageSubscriptionOption"
                                        ExtraButtonIconClass="icnreset" />
                                    <%-- <infsu:CommandBar ID="fsucCmdBarSubscriptionOption" runat="server" DefaultPanel="SubscriptionOption"
                                        ValidationGroup='grpManageSubscriptionOption'>
                                        <ExtraCommandButtons>
                                            <infs:WclButton runat="server" ID="btnSaveForm" ValidationGroup="grpManageSubscriptionOption"
                                                Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>
                                                <Icon PrimaryIconCssClass="rbSave" />
                                            </infs:WclButton>
                                            <infs:WclButton runat="server" ID="btnCancelForm" Text="Cancel" CommandName="Cancel">
                                                <Icon PrimaryIconCssClass="rbCancel" />
                                            </infs:WclButton>
                                        </ExtraCommandButtons>
                                    </infsu:CommandBar>--%>
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
        <script language="javascript" type="text/javascript">
            function NumericTextBoxKeyPress(sender, args) {
                var keyCharacter = args.get_keyCharacter();

                if (keyCharacter == sender.get_numberFormat().DecimalSeparator || keyCharacter == sender.get_numberFormat().NegativeSign) {
                    args.set_cancel(true);
                }
            } 
        </script>
    </div>
</div>
