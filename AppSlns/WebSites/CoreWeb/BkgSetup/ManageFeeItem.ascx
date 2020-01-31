<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageFeeItem.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageFeeItem" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<div class="section" runat="server" id="divMainSection">
    <h1 class="mhdr">Manage Global Fee Item
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant" Visible="false">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="divGrdFeeItem" runat="server" visible="false">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdFeeItems" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                    GridLines="Both" NonExportingColumns="EditCommandColumn,DeleteColumn" OnItemCreated="grdFeeItems_ItemCreated"
                    OnUpdateCommand="grdFeeItems_UpdateCommand"
                    OnInsertCommand="grdFeeItems_InsertCommand" OnNeedDataSource="grdFeeItems_NeedDataSource"
                    OnDeleteCommand="grdFeeItems_DeleteCommand"  OnItemCommand="grdFeeItems_ItemCommand"
                    EnableLinqExpressions="false">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="false" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                        Pdf-PageRightMargin="20mm">
                        <Excel AutoFitImages="true" />
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="PSIF_ID,lkpServiceItemFeeType.SIFT_Code">
                        <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Fee Item"
                            ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                            ShowRefreshButton="true" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="PSIF_Name" FilterControlAltText="Filter FeeItemName column"
                                HeaderText="Fee Item Name" SortExpression="PSIF_Name" UniqueName="PSIF_Name">
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="PSIF_Label" FilterControlAltText="Filter FeeItemLabel column"
                                HeaderText="Fee Item Label" SortExpression="PSIF_Label" UniqueName="PSIF_Label">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="PSIF_Description" FilterControlAltText="Filter Description column"
                                HeaderText="Description" SortExpression="PSIF_Description" UniqueName="PSIF_Description">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="lkpServiceItemFeeType.SIFT_Name" FilterControlAltText="Filter FeeItemType column"
                                HeaderText="Fee Item Type" SortExpression="lkpServiceItemFeeType.SIFT_Name" UniqueName="FeeItemType">
                            </telerik:GridBoundColumn>
                           <%-- <telerik:GridTemplateColumn DataField="PSIF_IsGlobal" FilterControlAltText="Filter Is Global column"
                                HeaderText="Is Global" SortExpression="PSIF_IsGlobal" UniqueName="PSIF_IsGlobal" Visible="false" Display="false">
                                <ItemTemplate>
                                    <asp:Label ID="IsGlobal" runat="server" Text='<%# Convert.ToBoolean(Eval("PSIF_IsGlobal"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageFeeRecord" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnManageFeeRecord" ButtonType="LinkButton" CommandName="ManageFeeRecord"
                                    runat="server" Text="Manage Fee Record" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" />
                            </telerik:GridEditCommandColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Global Fee Item?"
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
                                        <asp:Label ID="lblEHAttr" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Fee Item" : "Update Fee Item" %>'
                                            runat="server" /></h1>
                                    <div class="content">
                                        <div class="sxform auto">
                                            <div class="msgbox">
                                                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                            </div>
                                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlFeeItem">
                                                <div class='sxro sx3co'>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Fee Item Name</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox runat="server" ID="txtFeeItemName" Text='<%# Eval("PSIF_Name") %>' MaxLength="50">
                                                        </infs:WclTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvFeeItemName" ControlToValidate="txtFeeItemName"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Fee Item Name is required." ValidationGroup='grpFeeItem' />
                                                        </div>
                                                    </div>
                                                    <%--<div class='sxlb'>
                                                        <span class="cptn">Fee Item Label</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox runat="server" ID="txtFeeItemLabel" Text='<%# Eval("PSIF_Label") %>'
                                                            MaxLength="200">
                                                        </infs:WclTextBox>
                                                    </div>--%>
                                                      <div class='sxlb'>
                                                        <span class="cptn">Description</span>
                                                    </div>
                                                    <div class='sxlm m2spn'>
                                                        <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("PSIF_Description") %>'
                                                            MaxLength="200">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                                <div class='sxro sx3co'>
                                                   <%-- <div class='sxlb'>
                                                        <span class="cptn">Is Global</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclButton runat="server" ID="chkIsGlobal" ToggleType="CheckBox" ButtonType="ToggleButton"
                                                            Checked='<%# (Container is GridEditFormInsertItem)? false : Eval("PSIF_IsGlobal") %>'
                                                            AutoPostBack="false">
                                                            <ToggleStates>
                                                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                                                <telerik:RadButtonToggleState Text="No" Value="False" />
                                                            </ToggleStates>
                                                        </infs:WclButton>
                                                    </div>--%>
                                                   <div class='sxlb'>
                                                        <span class="cptn">Fee Item Type</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="cmbFeeItemType" runat="server"  DataTextField="SIFT_Name"
                                                            DataValueField="SIFT_ID">
                                                        </infs:WclComboBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvFeeItemType" ControlToValidate="cmbFeeItemType"
                                                                InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFeeItem" CssClass="errmsg"
                                                                Text="Fee Item Type is required." />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <infsu:CommandBar ID="fsucCmdBarFeeItem" runat="server"  GridMode="true" DefaultPanel="pnlFeeItem" GridInsertText="Save" GridUpdateText="Save"
                                            ValidationGroup="grpFeeItem" />
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
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
