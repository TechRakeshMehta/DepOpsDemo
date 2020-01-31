<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.ManagePriceAjustment" Codebehind="ManagePriceAjustment.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">
        Price Adjustment</h1>
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
                       <%-- <infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
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
        <div id="dvPriceAdjustment" runat="server" class="swrap">
            <infs:WclGrid runat="server" ID="grdPriceAdjustment" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                PageSize="10" NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdPriceAdjustment_NeedDataSource"
                OnItemCommand="grdPriceAdjustment_ItemCommand" OnInsertCommand="grdPriceAdjustment_InsertCommand"
                OnItemDataBound="grdPriceAdjustment_ItemDataBound" OnUpdateCommand="grdPriceAdjustment_UpdateCommand"
                OnDeleteCommand="grdPriceAdjustment_DeleteCommand">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="true" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="PriceAdjustmentID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Price Adjustment"
                        ShowExportToCsvButton="true" ShowExportToExcelButton="true" ShowExportToPdfButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Label" FilterControlAltText="Filter Label column"
                            HeaderText="Label" SortExpression="Label" UniqueName="Label">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" Width="3%" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" Width="3%" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" runat="server" id="divEditBlock" visible="true">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblTitlePriceAdjustment" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Price Adjustment" : "Update Price Adjustment" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPriceAdjustment">
                                            <infs:WclTextBox runat="server" Text='<%# Eval("PriceAdjustmentID") %>' ID="txtPriceAdjustmentId"
                                                Visible="false">
                                            </infs:WclTextBox>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Label</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox ID="txtLabel" Width="100%" runat="server" Text='<%# Eval("Label") %>'
                                                        MaxLength="50">
                                                    </infs:WclTextBox>
                                                    <div id="dvLabel" class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvLabel" ControlToValidate="txtLabel"
                                                            class="errmsg" ErrorMessage="Label is required." ValidationGroup='grpPriceAdjustment'
                                                            Enabled="true" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Description</span>
                                                </div>
                                                <div class='sxlm '>
                                                    <infs:WclTextBox Width="100%" ID="txtDescription" runat="server" Text='<%# Eval("Description") %>'
                                                        TextMode="MultiLine" MaxLength="1024">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarPriceAdjustment" runat="server" GridMode="true" DefaultPanel="pnlPriceAdjustment"
                                        ValidationGroup="grpPriceAdjustment" ExtraButtonIconClass="icnreset" GridInsertText="Save" GridUpdateText="Save" />
                                    <%-- <infsu:CommandBar ID="fsucCmdBarPriceAdjustment" runat="server" DefaultPanel="PriceAdjustment"
                                        ValidationGroup='grpPriceAdjustment'>
                                        <ExtraCommandButtons>
                                            <infs:WclButton runat="server" ID="btnSaveForm" ValidationGroup="grpPriceAdjustment"
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
    </div>
</div>
