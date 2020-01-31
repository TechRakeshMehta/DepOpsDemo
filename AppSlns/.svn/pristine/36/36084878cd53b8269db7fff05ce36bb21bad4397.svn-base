<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorServiceAttributeMapping.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.VendorServiceAttributeMapping" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">Manage Vendor Service Attribute Mapping
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="sxpnl">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Background Service Name</span>
                    </div>
                    <div class='sxlm'>
                        <asp:Label ID="lblBSEName" CssClass="ronly" runat="server"></asp:Label>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Vendor Name</span>
                    </div>
                    <div class='sxlm'>
                        <asp:Label ID="lblEVEName" CssClass="ronly" runat="server"></asp:Label>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">External Service Name</span>
                    </div>
                    <div class='sxlm'>
                        <asp:Label ID="lblEBSName" CssClass="ronly" runat="server"></asp:Label>
                    </div>
                    <div class="sxroend">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdVendorServiceAttributeMapping" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" ShowClearFiltersButton="false"
                EnableDefaultFeatures="true" ShowAllExportButtons="False" ShowExtraButtons="true"
                GridLines="Both" OnNeedDataSource="grdVendorServiceAttributeMapping_NeedDataSource" OnDeleteCommand="grdVendorServiceAttributeMapping_DeleteCommand"
                OnUpdateCommand="grdVendorServiceAttributeMapping_UpdateCommand" OnInsertCommand="grdVendorServiceAttributeMapping_InsertCommand"
                OnItemCommand="grdVendorServiceAttributeMapping_ItemCommand" OnItemCreated="grdVendorServiceAttributeMapping_ItemCreated"
                OnItemDataBound="grdVendorServiceAttributeMapping_ItemDataBound"
                EnableLinqExpressions="false" NonExportingColumns="EditCommandColumn,DeleteColumn">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Pdf PageWidth="450mm" PageHeight="210mm" PageLeftMargin="20mm" PageRightMargin="20mm"></Pdf>

                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ESAM_ID, ESAM_ExternalBkgSvcAttributeID, ESAM_ServiceMappingId, IsComplex" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Vendor Service Attribute Mapping"
                        ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BSAD_Name" FilterControlAltText="Filter Attribute Group Name column"
                            HeaderText="Attribute Group Name" SortExpression="BSAD_Name" UniqueName="BSAD_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BSA_Name" FilterControlAltText="Filter Attribute Name column"
                            HeaderText="Attribute Name" SortExpression="BSA_Name" UniqueName="BSA_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsRequired" FilterControlAltText="Filter IsRequired Name column"
                            HeaderText="Is Required" SortExpression="IsRequired" UniqueName="IsRequired">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EBSA_FieldID" FilterControlAltText="Filter Field ID column"
                            HeaderText="External Field ID" SortExpression="EBSA_FieldID" UniqueName="EBSA_FieldID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EBSA_Label" FilterControlAltText="Filter Field Label column"
                            HeaderText="External Field Label" SortExpression="EBSA_Label" UniqueName="EBSA_Label">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EBSA_LocationField" FilterControlAltText="Filter Location Field Name column"
                            HeaderText="External Location Field Name" SortExpression="EBSA_LocationField" UniqueName="EBSA_LocationField">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EBSA_DefaultValue" FilterControlAltText="Filter Default Value column"
                            HeaderText="Default Value" SortExpression="EBSA_DefaultValue" UniqueName="EBSA_DefaultValue">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ExternalIsRequired" FilterControlAltText="Filter ExternalIsRequired Value column"
                            HeaderText="External Is Required" SortExpression="ExternalIsRequired" UniqueName="ExternalIsRequired">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Vendor Service Attribute Mapping?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" visible="true" id="divEditFormBlock" runat="server">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Vendor Service Attribute Mapping" : "Update Vendor Service Attribute Mapping" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceGroup">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Mapping Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:RadioButtonList ID="rdoMappingType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnSelectedIndexChanged="rdoMappingType_SelectedIndexChanged">
                                                        <asp:ListItem Text="Simple " Value="false" />
                                                        <asp:ListItem Text="Composite" Value="true" />
                                                    </asp:RadioButtonList>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvMappingType" ControlToValidate="rdoMappingType"
                                                            CssClass="errmsg" Display="Dynamic" ErrorMessage="Mapping type is required."
                                                            ValidationGroup='grpFormSubmit' />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">External Service Attribute</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlExtServiceFields" runat="server" DataValueField="ExtSvcAttributeID" DataTextField="ExtSvcAttributeName" EmptyMessage="--Select--">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvExtServiceFields" ControlToValidate="ddlExtServiceFields"
                                                            Display="Dynamic" CssClass="errmsg" Text="External service attribute is required." ValidationGroup="grpFormSubmit" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Background Service Attribute</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlBkgServiceFields" runat="server" DataValueField="BSAGM_ID" DataTextField="BSA_Name" EmptyMessage="--Select--">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvExternalVendor" ControlToValidate="ddlBkgServiceFields"
                                                            Display="Dynamic" CssClass="errmsg" Text="Background service attribute is required." ValidationGroup="grpFormSubmit" />
                                                    </div>
                                                </div>
                                                <div id="divDelimeter" runat="server" style="display: none">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Delimiter</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox Width="99%" MaxLength="50" ID="txtDelimeter" runat="server" Text="\" />
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvDelimeter" ControlToValidate="txtDelimeter"
                                                                Display="Dynamic" CssClass="errmsg" Text="Delimiter is required." ValidationGroup="grpFormSubmit" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>

                                            <div class='sxro sx3co'>
                                                <div id="divCompositeFields" runat="server" style="display: none">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Composite Fields</span>
                                                    </div>
                                                    <div class='sxlm m2spn '>
                                                        <asp:Repeater ID="rptCompositeFields" runat="server" OnItemDataBound="rptCompositeFields_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <table style="padding: 5px !important; width: 100%" align="left">
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="width: 40%;">
                                                                        <span><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("BkgSvcAttributeName"))) %></span>
                                                                        <asp:Label ID="lblBkgAttrID" Text='<%# Eval("BkgSvcAttributeGroupMappingID") %>' Visible="false" runat="server"></asp:Label>
                                                                        <asp:Label ID="lblFormatTypeID" Text='<%# Eval("FormatTypeID") %>' Visible="false" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 25%;">
                                                                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="ntxtFieldSequence" Text='<%# Eval("FieldSequence") %>'
                                                                            MaxValue="10" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number"
                                                                            MinValue="1">
                                                                            <NumberFormat AllowRounding="false" DecimalDigits="0" DecimalSeparator="." GroupSizes="2" />
                                                                        </infs:WclNumericTextBox>
                                                                    </td>
                                                                    <td style="width: 25%;">
                                                                        <infs:WclComboBox ID="ddlFormatType" runat="server" DataValueField="FTY_ID" DataTextField="FTY_Name" EmptyMessage="--Select--">
                                                                        </infs:WclComboBox>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                        ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
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
<div style="width: 100%; text-align: center" id="dvShowBackLink" runat="server">
    <infs:WclButton runat="server" ID="btnGoBack" Text="Go Back To Vendor Service" OnClick="CmdBarCancel_Click">
    </infs:WclButton>
</div>
