<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ViewPackageDetail" CodeBehind="ViewPackageDetail.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style>
    .bullet ul {
        margin-left: 10px;
        padding-left: 10px !important;
    }

    .bullet li {
        list-style-position: inside;
        list-style: disc;
    }

    .bullet ol {
        list-style-type: decimal;
        margin-left: 10px;
        padding-left: 10px;
    }

        .bullet ol li {
            list-style: decimal;
        }
</style>
<div class="section">
    <h1 class="mhdr">Compliance Package Detail</h1>
    <div class="content">
        <infs:WclTreeList ID="treeListPackageDetail" runat="server" DataTextField="Value"
            ParentDataKeyNames="ParentNodeID,ParentDataID,ParentNodeCode" DataKeyNames="NodeID,DataID,NodeCode"
            OnNeedDataSource="treeListPackageDetail_NeedDataSource" DataMember="Assigned"
            OnItemCreated="treeListPackageDetail_ItemCreated" AutoGenerateColumns="false"
            DataValueField="UICode" OnPreRender="treeListPackageDetail_PreRender">
            <Columns>
                <telerik:TreeListBoundColumn DataField="Value" UniqueName="Value" HeaderText="Name" />
                <telerik:TreeListBoundColumn DataField="Description" ItemStyle-CssClass="bullet" UniqueName="Description" HeaderText="Description" />
            </Columns>
        </infs:WclTreeList>
    </div>
</div>
<br />
<infsu:CommandBar ID="fsucCmdBarPackageDetail" runat="server" DisplayButtons="Submit"
    ButtonPosition="Center" SubmitButtonText="Continue Order" AutoPostbackButtons="Submit"
    OnSubmitClick="fsucCmdBarAssignment_SubmitClick">
</infsu:CommandBar>
