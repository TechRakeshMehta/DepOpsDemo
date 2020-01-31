<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.ManagePolicy" Codebehind="MapRolePolicy.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManagePolicy" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <asp:Panel ID="pnlgrdPolicy" runat="server">
                <infs:WclGrid runat="server" ID="grdPolicy" EnableDefaultFeatures="false" AutoGenerateColumns="False" ShowAllExportButtons ="false"
                    AllowSorting="True" GridLines="Both" OnItemCommand="grdPolicy_ItemCommand" OnNeedDataSource="grdPolicy_NeedDataSource">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="ControlPath,RegisterUserControlID">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="User Control Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblUCName" Text= '<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("DisplayName")) )%>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <NestedViewTemplate>
                            <div class="swrap">
                                <infs:WclGrid runat="server" ID="grdControl" EnableDefaultFeatures="false" AllowPaging="True"
                                    PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                                    OnItemDataBound="grdControl_ItemDataBound">
                                    <ClientSettings EnableRowHoverStyle="true">
                                        <Selecting AllowRowSelect="true"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView CommandItemDisplay="Top">
                                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Control">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="HdnControlID" runat="server" Value= <%# Bind("ControlDisplayName")%>'/>
                                                    <asp:Label runat="server" ID="lblControl" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ControlID")) )%>' ></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Control Type">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblControlType" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ControlType")) )%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Policy">
                                                <ItemTemplate>
                                                    <asp:CheckBoxList runat="server" ID="chkList">
                                                    </asp:CheckBoxList>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </infs:WclGrid>
                            </div>
                            <div class="gclr">
                            </div>
                        </NestedViewTemplate>
                    </MasterTableView>
                </infs:WclGrid>
            </asp:Panel>
        </div>
        <div class="gclr">
        </div>
        <infsu:Commandbar ID="CommandbarbtnSaveBottom" runat="server" DisplayButtons="Submit,Cancel"
            SubmitButtonText="Save Mapping" CancelButtonText="Cancel" CancelButtonIconClass="rbCancel" SubmitButtonIconClass="rbSave" DefaultPanel="pnlMapRoleFeature"
            OnSubmitClick="btnSave_Click" OnCancelClick="btnCancel_Click" AutoPostbackButtons="Submit,Cancel" />
    </div>
</div>
