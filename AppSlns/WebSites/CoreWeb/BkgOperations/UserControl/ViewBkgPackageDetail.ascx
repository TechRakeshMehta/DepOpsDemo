<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewBkgPackageDetail.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.ViewBkgPackageDetail" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">
      Background Package Detail</h1>
    <div class="content">
        <infs:WclTreeList ID="treeListbkgPackageDetail" runat="server" DataTextField="Value"
            ParentDataKeyNames="ParentNodeID,ParentDataID" DataKeyNames="NodeID,DataID"
            OnNeedDataSource="treeListbkgPackageDetail_NeedDataSource" DataMember="Assigned"
            OnItemCreated="treeListbkgPackageDetail_ItemCreated" AutoGenerateColumns="false"
            DataValueField="UICode" OnPreRender="treeListbkgPackageDetail_PreRender">
            <Columns>
                <telerik:TreeListBoundColumn DataField="Value" UniqueName="Value" HeaderText="Display Name" />
                <telerik:TreeListBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description" />
            </Columns>
        </infs:WclTreeList>
    </div>
</div>
<br />
<infsu:CommandBar ID="fsucCmdBarBkgPackageDetail" runat="server" DisplayButtons="Submit"
    ButtonPosition="Center" SubmitButtonText="Continue Order" AutoPostbackButtons="Submit"
    OnSubmitClick="fsucCmdBarBkgPackageDetail_SubmitClick">
</infsu:CommandBar>