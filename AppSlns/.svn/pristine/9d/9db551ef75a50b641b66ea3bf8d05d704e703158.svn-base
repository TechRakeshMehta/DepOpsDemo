<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageFeeRecord.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageFeeRecord" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<div class="section" runat="server" id="divMainSection">
    <h1 class="mhdr">
        <asp:Label ID="lblFeeItem" runat="server"></asp:Label>
        <asp:HiddenField ID="hdnFeeItemName" runat="server" />
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
                        <infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                        </infs:WclDropDownList>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="divGrdFeeRecord" runat="server" visible="false">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdFeeRecord" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                    GridLines="Both" NonExportingColumns="EditCommandColumn,DeleteColumn" OnItemCreated="grdFeeRecord_ItemCreated"
                    OnUpdateCommand="grdFeeRecord_UpdateCommand"
                    OnInsertCommand="grdFeeRecord_InsertCommand" OnNeedDataSource="grdFeeRecord_NeedDataSource"
                    OnDeleteCommand="grdFeeRecord_DeleteCommand"
                    EnableLinqExpressions="false">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="false" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                        Pdf-PageRightMargin="20mm">
                        <Excel AutoFitImages="true" />
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="SIFR_ID">
                        <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Fee Record"
                            ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                            ShowRefreshButton="true" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <%--<telerik:GridBoundColumn DataField="PackageServiceItemFee.PSIF_Name" FilterControlAltText="Filter Fee Item column"
                                HeaderText="Fee Item" SortExpression="PackageServiceItemFee.PSIF_Name" UniqueName="PackageServiceItemFee.PSIF_Name">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="PSIF_Name" FilterControlAltText="Filter FeeItem column"
                                HeaderText="Fee Item" SortExpression="PSIF_Name" UniqueName="PSIF_Name">
                            </telerik:GridBoundColumn>
                            <%--<telerik:GridBoundColumn DataField="SIFR_FieldValue" FilterControlAltText="Filter FieldValue column"
                                HeaderText="Field Value" SortExpression="SIFR_FieldValue" UniqueName="SIFR_FieldValue">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="State" FilterControlAltText="Filter State column"
                                HeaderText="State" SortExpression="State" UniqueName="State" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FieldValue" FilterControlAltText="Filter FieldValue column"
                                HeaderText="Applicable On" SortExpression="FieldValue" UniqueName="FieldValue">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SIFR_Amount" FilterControlAltText="Filter FieldAmount column"
                                HeaderText="Amount($)" DataType="System.Decimal" SortExpression="SIFR_Amount" UniqueName="SIFR_Amount">
                            </telerik:GridBoundColumn>
                            <%-- <telerik:GridTemplateColumn DataField="PSIF_IsGlobal" FilterControlAltText="Filter Is Global column"
                                HeaderText="Is Global" SortExpression="PSIF_IsGlobal" UniqueName="PSIF_IsGlobal">
                                <ItemTemplate>
                                    <asp:Label ID="IsGlobal" runat="server" Text='<%# Convert.ToBoolean(Eval("PSIF_IsGlobal"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" />
                            </telerik:GridEditCommandColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Attribute?"
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
                                        <asp:Label ID="lblEHAttr" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Fee Record" : "Update Fee Record" %>'
                                            runat="server" /></h1>
                                    <div class="content">
                                        <div class="sxform auto">
                                            <div class="msgbox">
                                                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                            </div>
                                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlFeeItem">
                                                <div class='sxro sx3co'>
                                                    <%--<div class='sxlb'>
                                                        <span class="cptn">Fee Item</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="cmbFeeItem" AutoPostBack="true" runat="server" DataTextField="PSIF_Name" OnSelectedIndexChanged="cmbFeeItem_SelectedIndexChanged" OnItemDataBound="cmbFeeItem_ItemDataBound" 
                                                            DataValueField="PSIF_ID">
                                                        </infs:WclComboBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvFeeItem" ControlToValidate="cmbFeeItem"
                                                                InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFeeRecord" CssClass="errmsg"
                                                                Text="Fee Item is required." />
                                                        </div>
                                                    </div>--%>
                                                    <div id="divStateCounty" runat="server" visible="false">
                                                        <div class='sxlb'>
                                                            <span class="cptn">State</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclComboBox ID="cmbState" AutoPostBack="true" runat="server" DataTextField="StateName" OnSelectedIndexChanged="cmbState_SelectedIndexChanged"
                                                                DataValueField="StateID">
                                                            </infs:WclComboBox>
                                                            <div class="vldx">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvState" ControlToValidate="cmbState"
                                                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFeeRecord" CssClass="errmsg"
                                                                    Text="State is required." />
                                                            </div>
                                                        </div>
                                                        <div id="divCounty" runat="server" visible="false">
                                                            <div class='sxlb'>
                                                                <span class="cptn">County</span><span class="reqd">*</span>
                                                            </div>
                                                            <div class='sxlm'>
                                                                <infs:WclComboBox ID="cmbCounty" runat="server" DataTextField="CountyName"
                                                                    DataValueField="CountyID">
                                                                </infs:WclComboBox>
                                                                <div class="vldx">
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvCounty" ControlToValidate="cmbCounty"
                                                                        InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFeeRecord" CssClass="errmsg"
                                                                        Text="County is required." />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="divCountry" runat="server" visible="false">
                                                          <div class='sxlb'>
                                                            <span class="cptn">Country</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclComboBox ID="cmbCountry" AutoPostBack="false" runat="server" >
                                                            </infs:WclComboBox>
                                                            <div class="vldx">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvCountry" ControlToValidate="cmbCountry"
                                                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFeeRecord" CssClass="errmsg"
                                                                    Text="Country is required." />
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div id="divShipping" runat="server" visible="false">
                                                          <div class='sxlb'>
                                                            <span class="cptn">Mailing Option</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclComboBox ID="cmbMailingOption" AutoPostBack="false" runat="server" >
                                                            </infs:WclComboBox>
                                                            <div class="vldx">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvMailingOption" ControlToValidate="cmbMailingOption"
                                                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFeeRecord" CssClass="errmsg"
                                                                    Text="Mailing Option is required." />
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div id="divAdditionalService" runat="server" visible="false">
                                                          <div class='sxlb'>
                                                            <span class="cptn">Additional Service</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclComboBox ID="cmbAdditionalService" AutoPostBack="false" runat="server" >
                                                            </infs:WclComboBox>
                                                            <div class="vldx">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvAdditionalService" ControlToValidate="cmbAdditionalService"
                                                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFeeRecord" CssClass="errmsg"
                                                                    Text="Additional Service is required." />
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Amount</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Currency" ID="ntxtAmount" Text='<%# Eval("SIFR_Amount") %>'
                                                            MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                                            MinValue="1">
                                                            <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                                        </infs:WclNumericTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvAmount" ControlToValidate="ntxtAmount"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Amount is required."
                                                                ValidationGroup='grpFeeRecord' />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <infsu:CommandBar ID="fsucCmdBarFeeItem" runat="server" GridMode="true" DefaultPanel="pnlFeeItem" GridInsertText="Save" GridUpdateText="Save"
                                            ValidationGroup="grpFeeRecord" />
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
<div style="width: 100%; text-align: center" id="dvShowBackLink" runat="server">
    <infs:WclButton runat="server" ID="btnGoBack" Text="Go Back To Fee Item" OnClick="CmdBarCancel_Click">
    </infs:WclButton>
</div>
