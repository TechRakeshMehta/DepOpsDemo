<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.MapProductFeature" CodeBehind="MapProductFeature.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxMapProductFeature">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/MapBlockfeature.js"
        ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/MapProductFeature.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblMapProductFeature" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlMapProductFeature" CssClass="sxpnl" runat="server">
                        <div class="msgbox">
                            <asp:Label ID="lblErrormessage" runat="server" Text=""></asp:Label>
                        </div>
                        <infs:WclGrid runat="server" ID="grdBlockFeature" AllowPaging="false" AutoGenerateColumns="False"
                            EnableDefaultFeatures="false" GridLines="Both" OnItemCommand="grdMapProductFeature_ItemCommand"
                            OnItemDataBound="grdMapProductFeature_ItemDataBound" OnNeedDataSource="grdMapProductFeature_NeedDataSource" GroupHeaderItemStyle-Font-Bold="true">
                            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True" 
                                Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                            </ExportSettings>
                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="true"></Selecting>
                            </ClientSettings>

                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="SysXBlockId">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                <NestedViewTemplate>
                                    <infs:WclTreeList runat="server" ID="treeListFeature" AllowPaging="false" OnItemDataBound="treeListFeature_ItemDataBound"
                                        DataKeyNames="ProductFeature.ProductFeatureID" ParentDataKeyNames="ProductFeature.ParentProductFeatureID"
                                        OnNeedDataSource="treeListFeature_NeedDataSource" OnInit="treeListFeature_Init"
                                        AutoGenerateColumns="false" AllowMultiItemSelection="true">
                                        <Columns>
                                            <telerik:TreeListTemplateColumn HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkFeature" runat="server" parent='<%# SetParent(DataBinder.Eval(Container.DataItem, "ProductFeature.ParentProductFeatureID"),
                                       DataBinder.Eval(Container.DataItem, "SysXBlockID").ToString())%>'
                                                        fieldIndex='<%#SetParent(DataBinder.Eval(Container.DataItem, "ProductFeature.ProductFeatureID"),
                                      DataBinder.Eval(Container.DataItem,"SysXBlockID").ToString()) %>' />
                                                </ItemTemplate>
                                            </telerik:TreeListTemplateColumn>
                                            <telerik:TreeListBoundColumn DataField="SysXBlockFeatureID" UniqueName="SysXBlockFeatureID"
                                                HeaderStyle-Width="100px" Visible="false" HeaderText="Business Channel Feature ID" />
                                            <telerik:TreeListBoundColumn DataField="ProductFeature.Name" UniqueName="Name" HeaderText="Feature Name" />
                                            <telerik:TreeListBoundColumn DataField="ProductFeature.Description" UniqueName="Description"
                                                HeaderText="Description" />
                                            <telerik:TreeListBoundColumn DataField="ProductFeature.IconImageName" UniqueName="IconImageName"
                                                HeaderText="Icon Image Name" />
                                        </Columns>
                                        <ClientSettings AllowPostBackOnItemClick="false">
                                        </ClientSettings>
                                    </infs:WclTreeList>
                                </NestedViewTemplate>
                                <NestedViewSettings>
                                    <ParentTableRelation>
                                        <telerik:GridRelationFields DetailKeyField="SysXBlockId" MasterKeyField="SysXBlockId" />
                                    </ParentTableRelation>
                                </NestedViewSettings>
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                </ExpandCollapseColumn>
                                <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField HeaderText="Business Channel Type" FieldName="BusinessChannelTypeName" />
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="BusinessChannelTypeID" />
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="User Type Name" HeaderButtonType="TextButton"
                                        DataField="Name">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Description" HeaderButtonType="TextButton" DataField="Description">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn SortExpression="Count" HeaderText="Count" UniqueName="Count">
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            </MasterTableView>
                        </infs:WclGrid>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="gclr">
        </div>
        <infsu:Commandbar ID="CommandbarbtnSaveBottom" runat="server" DisplayButtons="Submit,Cancel"
            SubmitButtonText="Save Mapping" SubmitButtonIconClass="rbSave" CancelButtonIconClass="rbCancel" TabIndexAt="1"
            DefaultPanel="pnlMapProductFeature" OnSubmitClick="btnSave_Click" OnCancelClick="btnCancel_Click" AutoPostbackButtons="Submit,Cancel" />
        <asp:CustomValidator runat="server" ID="cvIsPermissionAssigned" OnServerValidate="cvIsPermissionAssigned_ServerValidate">
        </asp:CustomValidator>
    </div>
</div>
