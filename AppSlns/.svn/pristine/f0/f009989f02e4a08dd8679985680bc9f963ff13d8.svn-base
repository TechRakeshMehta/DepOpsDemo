<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgRuleExpressionObject.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.BkgRuleExpressionObject" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class='sxro sx3co'>
    <div class='sxlb'>
        <asp:Label ID="objName" runat="server" Text="" CssClass="cptn"></asp:Label>
    </div>
    <div class='sxlm m3spn'>
        <infs:WclDropDownList ID="ddlObjectType" runat="server"
            OnSelectedIndexChanged="ddlObjectType_SelectedIndexChanged" AutoPostBack="true">
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlServices" runat="server" Visible="false"
            ToolTip="">
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlAttributegrp" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlAttributegrp_SelectedIndexChanged"
            ToolTip="">
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlAttribute" runat="server" Visible="false"
            ToolTip="">
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlConstantGroup" runat="server" Visible="false" AutoPostBack="true"
            ToolTip="" OnSelectedIndexChanged="ddlConstantGroup_SelectedIndexChanged">
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlConstantType" runat="server" Visible="false" AutoPostBack="true"
            ToolTip="" OnSelectedIndexChanged="ddlConstantType_SelectedIndexChanged">
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlConstantData" runat="server" Visible="false" DropDownHeight="200px"
            ToolTip="">
        </infs:WclDropDownList>
        <asp:CheckBox ID="chkCurrent" runat="server" Visible="false" Text="Current" OnCheckedChanged="chkCurrent_CheckedChanged"></asp:CheckBox>
        <infs:WclTextBox ID="txtConstant" runat="server" Visible="false"></infs:WclTextBox>
    </div>
</div>
