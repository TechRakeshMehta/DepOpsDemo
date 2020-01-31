<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.MapBlockFeature" Codebehind="MapBlockFeature.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxMapBlockFeature">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/MapBlockfeature.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblMapLineOfBusiness" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <asp:Panel ID="pnlMapLineOfBusiness" runat="server">
            <infs:WclTreeList runat="server" ID="treeListFeature" AllowPaging="false" DataKeyNames="ProductFeatureID"
                AllowMultiItemSelection="true" ParentDataKeyNames="ProductFeature2.ProductFeatureID"
                AutoGenerateColumns="false" OnNeedDataSource="treeListFeature_NeedDataSource"
                OnItemDataBound="treeListFeature_ItemDataBound" OnPreRender="treeListFeature_PreRender"
                OnItemCreated="treeListFeature_ItemCreated">
                <Columns>
                    <telerik:TreeListTemplateColumn HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkFeature" runat="server" parent='<%#DataBinder.Eval(Container.DataItem, "ParentProductFeatureID")%>'
                                fieldIndex='<%# DataBinder.Eval(Container.DataItem, "ProductFeatureID")%>' />
                        </ItemTemplate>
                    </telerik:TreeListTemplateColumn>
                    <telerik:TreeListBoundColumn DataField="Name" UniqueName="Name" HeaderText="Name" />
                    <telerik:TreeListBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description" />
                    <telerik:TreeListBoundColumn DataField="UIControlID" UniqueName="UIControlID" HeaderText="Web Page Name" />
                    <telerik:TreeListBoundColumn DataField="IconImageName" UniqueName="IconImageName"
                        HeaderText="Icon Image Name" />
                    <telerik:TreeListBoundColumn DataField="NavigationURL" UniqueName="NavigationURL"
                        HeaderText="Navigation URL" Visible="false" />
                </Columns>
            </infs:WclTreeList>
        </asp:Panel>
        <infsu:Commandbar ID="CommandbarbtnSaveBottom" runat="server" DisplayButtons="Submit,Cancel"
            SubmitButtonText="Save Mapping" CancelButtonText="Cancel" SubmitButtonIconClass="rbSave" CancelButtonIconClass="rbCancel" DefaultPanel="pnlMapLineOfBusiness"
            OnSubmitClick="btnSave_Click" OnCancelClick="btnCancel_Click" AutoPostbackButtons="Submit,Cancel" />
    </div>
</div>
