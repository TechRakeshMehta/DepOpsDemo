<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageServiceAttributeGroup.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ManageServiceAttributeGroup" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblServiceName" runat="server"></asp:Label></h1>
    <div class="content">
        <infs:WclGrid runat="server" ID="grdManageSvcAttributegrp" AllowPaging="True" PageSize="10"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"
            AllowFilteringByColumn="false" AllowSorting="True"
            ShowAllExportButtons="False" ShowClearFiltersButton="false" OnNeedDataSource="grdManageSvcAttributegrp_NeedDataSource"
            OnItemCommand="grdManageSvcAttributegrp_ItemCommand" OnItemCreated="grdManageSvcAttributegrp_ItemCreated"
            OnItemDataBound="grdManageSvcAttributegrp_ItemDataBound" OnPreRender="grdManageSvcAttributegrp_PreRender">
            <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ServiceattGrpID,AttributeID" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Mapping"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"></CommandItemSettings>
                <Columns>
                    <telerik:GridBoundColumn DataField="ServiceAttGrpName" HeaderText="Attribute Group"
                        SortExpression="ServiceAttGrpName" UniqueName="ServiceAttGrpName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AttributeName" HeaderText="Attribute"
                        SortExpression="AttributeName" UniqueName="AttributeName">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="IsEditable" FilterControlAltText="Filter IsActive column"
                        HeaderText="" SortExpression="IsEditable" UniqueName="IsEditable" Visible="false">
                        <ItemTemplate>
                            <%-- <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("IsEditable"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>--%>
                            <asp:HiddenField ID="hdnfIsEditable" runat="server" Value='<%#Eval("IsEditable")%>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" VerticalAlign="Bottom" />
                    </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Attribute Group?"
                        Text="Delete Attribute Group" UniqueName="DeleteColumn">
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
                                <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Mapping" : "Update Mapping" %>'
                                    runat="server" /></h1>
                            <div class="content">
                                <div class="sxform auto">
                                    <div class="msgbox">
                                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceGroup">
                                        <div class='sxro sx3co'>
                                            <div class='sxlb'>
                                                <span class="cptn">Attribute Groups</span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclComboBox ID="ddlAttributegrps" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAttributegrps_SelectedIndexChanged"
                                                    DataTextField="BSAD_Name" DataValueField="BSAD_ID" EmptyMessage="--Select--">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAttributegrps" ControlToValidate="ddlAttributegrps"
                                                        Display="Dynamic" CssClass="errmsg" Text="Attribute group is required." ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>

                                            <div class='sxlb'>
                                                <span class="cptn">Attributes</span><span class="reqd">*</span>
                                            </div>
                                            <div class='sxlm'>
                                                <infs:WclComboBox ID="ddlAttributes" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                    DataTextField="BSA_Name" DataValueField="BSA_ID" EmptyMessage="--Select--">
                                                    <Localization CheckAllString="Select All" />
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAttributes" ControlToValidate="ddlAttributes"
                                                        Display="Dynamic" CssClass="errmsg" Text="Attributes is required." ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                            <div class='sxroend'>
                                            </div>
                                        </div>

                                    </asp:Panel>
                                </div>
                                <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlCategory" GridInsertText="Save" GridUpdateText="Save"
                                    ValidationGroup="grpFormSubmit" ExtraButtonIconClass="icnreset" />
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>

                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
        </infs:WclGrid>
        <div class="gclr">
        </div>
    </div>
</div>

<div style="width: 100%; text-align: center" id="dvShowBackLink" runat="server">
    <infs:WclButton runat="server" ID="btnGoBack" Text="Go Back To Service Queue" OnClick="CmdBarCancel_Click">
    </infs:WclButton>
</div>

