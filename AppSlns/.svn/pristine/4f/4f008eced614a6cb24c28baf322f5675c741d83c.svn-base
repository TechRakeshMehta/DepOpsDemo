<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageCascadingAttributeDetails.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageCascadingAttributeDetails" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <%--<h1 class="mhdr">Manage Attribute Options</h1>--%>
    <h1 class="mhdr">Attribute Name : <asp:Label runat="server" ID="lblAttrName"></asp:Label></h1>    
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdAttributeOptions" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                GridLines="Both" NonExportingColumns="EditCommandColumn,DeleteColumn"
                OnUpdateCommand="grdAttributeOptions_UpdateCommand" OnNeedDataSource="grdAttributeOptions_NeedDataSource"
                OnInsertCommand="grdAttributeOptions_InsertCommand" OnDeleteCommand="grdAttributeOptions_DeleteCommand"
                
                EnableLinqExpressions="false">
                <ExportSettings ExportOnlyData="false" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="false" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="Id">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Option"
                        ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Value" HeaderText="Value" AllowFiltering="true" AllowSorting="true"
                            FilterControlAltText="" SortExpression="Value" UniqueName="Value">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SourceValue" HeaderText="Source Value" AllowFiltering="true" AllowSorting="true"
                            FilterControlAltText="" SortExpression="SourceValue" UniqueName="SourceValue">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DisplaySequence" HeaderText="Display Sequence" AllowFiltering="true" AllowSorting="true"
                            FilterControlAltText="" SortExpression="DisplaySequence" UniqueName="DisplaySequence">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Attribute Option?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <FormTemplate>
                            <div class="section" runat="server" id="divEditBlock" visible="true">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHAttr" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Option" : "Update Option" %>'
                                        runat="server" />
                                </h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAttrOption">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Option Value</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtValue" Text='<%# Eval("Value") %>' MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvtxtValue" ControlToValidate="txtValue"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Option Value is required." ValidationGroup='grpAttributeOption' />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Source Value</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtSourceValue" Text='<%# Eval("SourceValue") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <%--<div class='sxro sx3co'> 
                                                <div class='sxlb'>
                                                    <span class="cptn">Display Sequence</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" InputType="Number" ID="txtDisplaySequence" Text='<%# Eval("DisplaySequence") %>'>
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvtxtDisplaySequence" ControlToValidate="txtDisplaySequence"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Display Sequence is required." ValidationGroup='grpAttributeOption' />
                                                    </div>
                                                </div>                                                
                                                <div class='sxroend'>
                                                </div>
                                            </div>--%>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlAttrOption"
                                        ValidationGroup="grpAttributeOption" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                </MasterTableView>
            </infs:WclGrid>
        </div>
    </div>
</div>

<div style="width: 100%; text-align: center" id="dvShowBackLink" runat="server">
    <infs:WclButton runat="server" ID="btnGoBack" Text="Go Back To Service Attributes" OnClick="btnGoBack_Click">
    </infs:WclButton>
</div>
