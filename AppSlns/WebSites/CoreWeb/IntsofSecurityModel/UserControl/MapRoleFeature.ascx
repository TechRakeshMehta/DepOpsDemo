<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.MapRoleFeature" CodeBehind="MapRoleFeature.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxMapRoleFeature">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/MapBlockfeature.js"
        ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/MapRoleFeature.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblMapRoleFeature" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <div class="msgbox">
                <asp:Label ID="lblErrormessage" runat="server"></asp:Label>
            </div>
            <asp:Panel ID="pnlMapRoleFeature" runat="server">
                <infs:WclGrid runat="server" ID="grdMapRoleFeature" AutoGenerateColumns="False" EnableDefaultFeatures="false"
                    GridLines="Both" DataKeyNames="SysXBlocksFeature.lkpSysXBlock.SysXBlockID" OnItemCommand="grdBlockFeature_ItemCommand"
                    OnItemDataBound="grdBlockFeature_ItemDataBound" OnNeedDataSource="grdBlockFeature_NeedDataSource">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="SysXBlockId" GroupHeaderItemStyle-Font-Bold="true">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <NestedViewTemplate>
                            <infs:WclTreeList runat="server" ID="treeListFeature" AllowPaging="false" AllowSorting="false"
                                MasterKeyField="SysXBlocksFeature.FeatureID" DataKeyNames="SysXBlocksFeature.ProductFeature.ProductFeatureID"
                                ParentDataKeyNames="SysXBlocksFeature.ProductFeature.ParentProductFeatureID"
                                AutoGenerateColumns="false" AllowMultiItemSelection="true" OnNeedDataSource="treeListFeature_NeedDataSource"
                                OnItemDataBound="treeListFeature_ItemDataBound" OnInit="treeListFeature_Init" OnItemCommand="treeListFeature_ItemCommand">
                                <Columns>
                                    <telerik:TreeListTemplateColumn HeaderStyle-Width="30px">
                                        <ItemTemplate>

                                            <asp:CheckBox ID="chkFeature" runat="server" parent='<%#
    SetParent(DataBinder.Eval(Container.DataItem, "SysXBlocksFeature.ProductFeature.ParentProductFeatureID"),
              DataBinder.Eval(Container.DataItem, "SysXBlocksFeature.SysXBlockID").ToString())%>'
                                                fieldIndex='<%#
    SetParent(DataBinder.Eval(Container.DataItem, "SysXBlocksFeature.ProductFeature.ProductFeatureID"),
              DataBinder.Eval(Container.DataItem, "SysXBlocksFeature.SysXBlockID").ToString()) %>' />
                                        </ItemTemplate>
                                    </telerik:TreeListTemplateColumn>
                                    <telerik:TreeListBoundColumn DataField="SysXBlocksFeature.SysXBlockFeatureID" HeaderStyle-Width="100px"
                                        UniqueName="SysXBlockFeatureID" Visible="false" HeaderText="Business Channel Feature ID" />
                                    <telerik:TreeListBoundColumn DataField="SysXBlocksFeature.ProductFeature.Name" UniqueName="Name"
                                        HeaderText="Feature Name" />
                                    <telerik:TreeListBoundColumn DataField="SysXBlocksFeature.ProductFeature.Description"
                                        UniqueName="Description" HeaderText="Description" />
                                    <telerik:TreeListBoundColumn DataField="SysXBlocksFeature.ProductFeature.IconImageName"
                                        UniqueName="IconImageName" HeaderText="Icon Image Name" />
                                    <telerik:TreeListTemplateColumn UniqueName="ManageFields" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                             <asp:HiddenField ID="hdnUIControlID" runat="server" Value='<%#Eval("SysXBlocksFeature.ProductFeature.UIControlID") %>' />
                                            <asp:HiddenField ID="hdnProductFeatureID" runat="server" Value='<%#Eval("SysXBlocksFeature.ProductFeature.ProductFeatureID") %>' />
                                            <asp:HiddenField ID="hdnSysyXBlockFeatureID" runat="server" Value='<%#Eval("SysXBlocksFeature.SysXBlockFeatureID") %>' />
                                            <telerik:RadButton ID="btnFeatureActionList" OnClientClicked="openPopUp" ButtonType="LinkButton" AutoPostBack="false"
                                                runat="server" Text="Manage Feature Action" BackColor="Transparent" Font-Underline="true" BorderStyle="None" Style="display: none;">
                                            </telerik:RadButton>
                                            <%--        <a href="#" id="featureActionList" onclick="openPopUp();">Manage Feature Action</a>
                                            <asp:Label ID="lblfeatureActionList" runat="server"></asp:Label>--%>
                                        </ItemTemplate>
                                    </telerik:TreeListTemplateColumn>
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
                            <telerik:GridBoundColumn SortExpression="Name" HeaderText="User Type Name"
                                HeaderButtonType="TextButton" DataField="Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="Description" HeaderText="Description" HeaderButtonType="TextButton"
                                DataField="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn SortExpression="Count" HeaderText="Count" UniqueName="Count">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </infs:WclGrid>
            </asp:Panel>
        </div>
        <div class="gclr">
        </div>
        <infsu:Commandbar ID="CommandbarbtnSaveBottom" runat="server" DisplayButtons="Submit,Cancel"
            SubmitButtonText="Save Mapping" CancelButtonText="Cancel" SubmitButtonIconClass="rbSave" DefaultPanel="pnlMapRoleFeature"
            OnSubmitClick="btnSave_Click" OnCancelClick="btnCancel_Click" AutoPostbackButtons="Submit,Cancel" />
    </div>
</div>
<asp:HiddenField ID="hdnProductFeatureID" runat="server" Value="" />
<asp:HiddenField ID="hdnSysyXBlockFeatureID" runat="server" Value="" />
<asp:HiddenField ID="hdnRolePermissionProductFeature" runat="server" Value="" />
<script type="text/javascript">
     
</script>
