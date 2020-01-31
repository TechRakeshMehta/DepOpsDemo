<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetupServiceAttributeGroup.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.SetupServiceAttributeGroup" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<style type="text/css">
    .rgMasterTable td 
    {
       word-wrap: break-word ;
    word-break: break-all ;
    }
</style>--%>
<div class="section">
    <h1 class="mhdr">Manage Service Attribute Groups
    </h1>

    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdAttributeGrp" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                GridLines="Both" NonExportingColumns="EditCommandColumn,DeleteColumn" OnUpdateCommand="grdAttributeGrp_UpdateCommand"
                OnItemDataBound="grdAttributeGrp_ItemDataBound"
                OnInsertCommand="grdAttributeGrp_InsertCommand" OnNeedDataSource="grdAttributeGrp_NeedDataSource"
                OnDeleteCommand="grdAttributeGrp_DeleteCommand" OnItemCommand="grdAttributeGrp_ItemCommand"
                EnableLinqExpressions="false" >
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    HideStructureColumns="false" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm">
                    <Excel AutoFitImages="true" />
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BSAD_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Attribute Group"
                        ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BSAD_Name" FilterControlAltText="Filter Service Attribute Group Name column"
                            HeaderText="Service Attribute Group Name" SortExpression="BSAD_Name" UniqueName="BSAD_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BSAD_Description" FilterControlAltText="Filter Service Attribute Group Description column"
                            HeaderText="Service Attribute Group Description" SortExpression="BSAD_Description" UniqueName="BSAD_Description">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridTemplateColumn DataField="BSAD_IsEditable" FilterControlAltText="Filter IsEditable column"
                            HeaderText="Is Editable" SortExpression="BSAD_IsEditable" UniqueName="BSAD_IsEditable" Display="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("BSAD_IsEditable")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <%--<telerik:GridTemplateColumn DataField="BSAD_IsDisplay" FilterControlAltText="Filter IsDisplay column"
                            HeaderText="Is Display" SortExpression="BSAD_IsDisplay" UniqueName="BSAD_IsDisplay">
                            <ItemTemplate>
                                <asp:Label ID="IsDisplay" runat="server" Text='<%# Convert.ToBoolean(Eval("BSAD_IsDisplay"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="MapAttribute" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnMapAttribute" ButtonType="LinkButton" CommandName="MapAttribute"
                                    runat="server" Text="Attribute Mapping" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                                </telerik:RadButton>
                                 <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("BSAD_IsEditable")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Service Attribute Group?"
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
                                    <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Service Attribue Group" : "Update Service Attribue Group" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceGroup">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Service Attribute Group Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtServiceAttrGroupName" Text='<%# Eval("BSAD_Name") %>'
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvServiceAttrGroupName" ControlToValidate="txtServiceAttrGroupName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Service Attribute Group Name is required." />
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
                                                    <infs:WclTextBox runat="server" ID="txtDescription" Text='<%# Eval("BSAD_Description") %>'
                                                        MaxLength="1024">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlServiceGroup"
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
