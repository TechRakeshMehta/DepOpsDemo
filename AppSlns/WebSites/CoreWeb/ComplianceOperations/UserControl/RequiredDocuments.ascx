<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequiredDocuments.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.RequiredDocuments" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    .msgbox .info {
        background-color: #fffef0;
        background-image: url("../../Resources/Themes/Default/images/info.png");
        background-position: 10px 8px;
        background-repeat: no-repeat;
        color: #3071cd !important;
        border-radius: 4px;
        margin-top: 10px;
    }
</style>

<div>
    <div runat="server" id="dvMessage" class="msgbox">
        <span class="info" id="lblError">No records found.</span>
    </div>
    <div runat="server" id="dvRequiredDocs">
        <infs:WclTreeList runat="server" ID="treeListRequiredDocuments" OnItemCreated="treeListRequiredDocuments_ItemCreated"
            DataTextField="DataValue" DataMember="Assigned" OnPreRender="treeListRequiredDocuments_PreRender" OnItemDataBound="treeListRequiredDocuments_ItemDataBound"
            DataKeyNames="DataID" ParentDataKeyNames="ParentID" AutoGenerateColumns="false" OnNeedDataSource="treeListRequiredDocuments_NeedDataSource" DataValueField="Code"
            PagerStyle-Position="top">
            <Columns>
                <telerik:TreeListBoundColumn DataField="DataValue" UniqueName="DataValue" HeaderText="Package Name/Category Name" />
                <telerik:TreeListTemplateColumn HeaderStyle-Width="500px" HeaderText="" >
                    <ItemTemplate>
                     <%--   <asp:LinkButton ID="lnkVisitRequiredDocument" runat="server" CssClass="headerName" Text="Click Here"></asp:LinkButton>--%>
                        <div id="divLnkVisitRequiredDocument" runat="server"></div>
                    </ItemTemplate>
                </telerik:TreeListTemplateColumn>

            </Columns>
        </infs:WclTreeList>
    </div>
    <div class="col-md-12">&nbsp;</div>
    <div runat="server" id="dvRotReqdDocs">
        <infs:WclTreeList runat="server" ID="tlstRotationReqDocuments" OnItemCreated="tlstRotationReqDocuments_ItemCreated"
            DataTextField="DataValue" DataMember="Assigned" OnPreRender="tlstRotationReqDocuments_PreRender" OnItemDataBound="tlstRotationReqDocuments_ItemDataBound"
            DataKeyNames="DataID" ParentDataKeyNames="ParentID" AutoGenerateColumns="false" OnNeedDataSource="tlstRotationReqDocuments_NeedDataSource" DataValueField="Code"
            PagerStyle-Position="top">
            <Columns>
                <telerik:TreeListBoundColumn DataField="DataValue" UniqueName="DataValue" HeaderText="Package Name/Category Name" />
                <telerik:TreeListTemplateColumn HeaderStyle-Width="500px" HeaderText="" >
                    <ItemTemplate>
                        <%--<asp:LinkButton ID="lnkVisitRotationReqDocuments" runat="server" CssClass="headerName" Text="More Information"></asp:LinkButton>--%>
                        <div id="divLnkVisitRotRequiredDocument" runat="server"></div>
                     
                    </ItemTemplate>
                </telerik:TreeListTemplateColumn>
            </Columns>
        </infs:WclTreeList>
    </div>
    <asp:TextBox ID="hdnPostbacksource" class="postbacksource" runat="server" Style="display: none;" />
</div>

