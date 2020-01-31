<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CopyComplainceToClient.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.CopyComplainceToClient" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxMapProductFeature">
    <infs:LinkedResource ResourceType="JavaScript" Path="~/Resources/Mod/Compliance/CopyTree.js" />
</infs:WclResourceManagerProxy>
<div class="section">
    <h1 class="mhdr">
        <asp:Label Text="" runat="server" ID="lblClientName" />Mapped Packages</h1>
    <div class="content">
        <infs:WclTreeList ID="treeListPackages" runat="server" DataTextField="Value" ParentDataKeyNames="ParentNodeID"
            DataKeyNames="NodeID" OnNeedDataSource="treeListPackages_NeedDataSource"
            DataMember="Assigned" AutoGenerateColumns="false" DataValueField="UICode"
            OnItemDataBound="treeListPackages_ItemDataBound" AllowLoadOnDemand="true" OnChildItemsDataBind="treeListPackages_ChildItemsDataBind">
            <%-- OnPreRender="treeListPackages_PreRender" --%>
            <Columns>
                <telerik:TreeListBoundColumn DataField="UICode" UniqueName="UICode" Visible="false" />
                <telerik:TreeListTemplateColumn HeaderText="Name">
                    <HeaderStyle Width="" />
                    <ItemTemplate>
                        <asp:CheckBox ID="chkFeature" runat="server" fieldIndex='<%# Eval("NodeID") %>' parent='<%# Eval("ParentNodeID") %>' />
                        <asp:HiddenField ID="hdfUICode" runat="server" Value='<%# Eval("UICode") %>' />
                        <asp:HiddenField ID="hdfIsAssigned" runat="server" Value='<%# Eval("Associated") %>' />
                        <asp:Label ID="Label1" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Value"))) %>' runat="server" />
                    </ItemTemplate>
                </telerik:TreeListTemplateColumn>
                <telerik:TreeListBoundColumn DataField="Value" UniqueName="Name" HeaderText="Name"
                    Display="false" />
            </Columns>
            <NoRecordsTemplate>
                No package available
            </NoRecordsTemplate>
        </infs:WclTreeList>
    </div>
</div>
<infsu:Commandbar ID="fsucCmdBarItem" runat="server" DefaultPanel="pnlItem" DisplayButtons="Save, Cancel"
    SaveButtonText="Save Mapping" SubmitButtonIconClass="rbEdit" AutoPostbackButtons="Save, Cancel"
    OnSaveClick="btnSave_Click" OnCancelClick="btnCancel_Click">
</infsu:Commandbar>
<asp:HiddenField ID="hdn" runat="server" />
<asp:HiddenField ID="hdnPackageNode" runat="server" />
