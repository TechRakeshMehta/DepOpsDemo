<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorServiceMapping.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.VendorServiceMapping" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">Manage Vendor Service Mapping
    </h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdVendorServiceMapping" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="true" ShowAllExportButtons="False" ShowExtraButtons="true"
                GridLines="Both" OnNeedDataSource="grdVendorServiceMapping_NeedDataSource" OnDeleteCommand="grdVendorServiceMapping_DeleteCommand"
                OnUpdateCommand="grdVendorServiceMapping_UpdateCommand" OnInsertCommand="grdVendorServiceMapping_InsertCommand"
                OnItemCommand="grdVendorServiceMapping_ItemCommand" OnItemCreated="grdVendorServiceMapping_ItemCreated" OnItemDataBound="grdVendorServiceMapping_ItemDataBound"
                EnableLinqExpressions="false" NonExportingColumns="EditCommandColumn,DeleteColumn,AttributeMapping">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BSESM_ID, IsEditable">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Vendor Service Mapping"
                        ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BSE_Name" FilterControlAltText="Filter Background Service Name column"
                            HeaderText="Background Service Name" SortExpression="BSE_Name" UniqueName="BSE_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EVE_Name" FilterControlAltText="Filter Vendor Name column"
                            HeaderText="Vendor Name" SortExpression="EVE_Name" UniqueName="EVE_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EBS_Name" FilterControlAltText="Filter External Service Name column"
                            HeaderText="External Service Name" SortExpression="EBS_Name" UniqueName="EBS_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EBS_ExternalCode" FilterControlAltText="Filter External Service Code column"
                            HeaderText="External Service Code" SortExpression="EBS_ExternalCode" UniqueName="EBS_ExternalCode">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="AttributeMapping">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnAttributeMapping" ButtonType="LinkButton" CommandName="AttributeMapping"
                                    runat="server" Text="Manage Attribute Mapping" ToolTip="Manage Attribute Mapping" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Vendor Service Mapping?"
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
                                    <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Vendor Service Mapping" : "Update Vendor Service Mapping" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceGroup">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Background Service</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlBackgroundService" runat="server" DataValueField="BSE_ID" DataTextField="BSE_Name" EmptyMessage="--Select--">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvBackgroundService" ControlToValidate="ddlBackgroundService"
                                                            Display="Dynamic" CssClass="errmsg" Text="Background service is required." ValidationGroup="grpFormSubmit" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Vendor</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlExternalVendor" runat="server" DataValueField="EVE_ID" DataTextField="EVE_Name" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlExternalVendor_SelectedIndexChanged" EmptyMessage="--Select--">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvExternalVendor" ControlToValidate="ddlExternalVendor"
                                                            Display="Dynamic" CssClass="errmsg" Text="Vendor is required." ValidationGroup="grpFormSubmit" />
                                                    </div>
                                                </div>

                                                <div class='sxlb'>
                                                    <span class="cptn">External Service</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlExternalService" runat="server" DataValueField="EBS_ID" DataTextField="EBS_Name"
                                                        AutoPostBack="true" EmptyMessage="--Select--" OnSelectedIndexChanged="ddlExternalService_SelectedIndexChanged">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvExternalService" ControlToValidate="ddlExternalService"
                                                            Display="Dynamic" CssClass="errmsg" Text="External service is required." ValidationGroup="grpFormSubmit" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">External Service Code</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <asp:Label ID="lblExtSvcCode" runat="server"></asp:Label>
                                                </div>
                                                <div class='sxroend'>
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
