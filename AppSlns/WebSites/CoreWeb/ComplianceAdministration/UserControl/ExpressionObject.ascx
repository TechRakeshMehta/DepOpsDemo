<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.ExpressionObject" Codebehind="ExpressionObject.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class='sxro sx3co'>
    <div class='sxlb'>
        <asp:Label ID="objName" runat="server" Text="" CssClass="cptn"></asp:Label>
    </div>
    <div class='sxlm m3spn'>
        <infs:WclDropDownList ID="ddlCompliance" runat="server" OnSelectedIndexChanged="ddlCompliance_SelectedIndexChanged"  
            AutoPostBack="true">
          <%--  <Items>
                <telerik:DropDownListItem Text="Compliance Value" Value="COMPL" />
                <telerik:DropDownListItem Text="Data Value" Value="DVAL" />
                <telerik:DropDownListItem Text="Defined value" Value="CONST" />
            </Items>--%>
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlObjectType" runat="server"  
          OnSelectedIndexChanged="ddlObjectType_SelectedIndexChanged" AutoPostBack="true" >
           <%-- <Items>
                <telerik:DropDownListItem Text="Compliance Item" Value="2" />
                <telerik:DropDownListItem Text="Item Attribute" Value="3" />
                <telerik:DropDownListItem Text="Constant" Value="3" />
            </Items>--%>
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
            ToolTip="You must first add an category into the package to use here" Visible="false">
        </infs:WclDropDownList>

        <infs:WclDropDownList ID="ddlItem" runat="server"  Visible="false"
        ToolTip="You must first add an item into the category to use here"  OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlAttribute" runat="server"  Visible="false"
            ToolTip="You must first add an attribute into the item in order to use here">
           <%-- <Items>
                <telerik:DropDownListItem Text="Result" Value="1" />
            </Items>--%>
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlConstantGroup" runat="server"  Visible="false"
            ToolTip="" OnSelectedIndexChanged="ddlConstantGroup_SelectedIndexChanged">
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlConstantType" runat="server"  Visible="false"
            ToolTip="" OnSelectedIndexChanged="ddlConstantType_SelectedIndexChanged">
        </infs:WclDropDownList>
        <asp:CheckBox ID="chkCurrent" runat="server" Visible="false" Text="Current" OnCheckedChanged="chkCurrent_CheckedChanged"></asp:CheckBox>
        <infs:WclTextBox ID="txtConstant" runat="server" Visible="false"></infs:WclTextBox>
         <infs:WclDropDownList ID="ddlItemStatus" runat="server" Visible="false"
            ToolTip="You must select item status to use here">
        </infs:WclDropDownList>
        <infs:WclDropDownList ID="ddlItemSubmission" runat="server"  Visible="false"
             ToolTip ="You must first add an item into the category to use here"  OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
        </infs:WclDropDownList>
    </div>
    <div class='sxroend'>
    </div>
</div>
