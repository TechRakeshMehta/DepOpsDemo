<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Shell.Views.ClientDropDown" Codebehind="ClientDropDown.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<infs:WclDropDownList ID="ddlTenantName" runat="server" OnDataBound="ddlTenantName_DataBound"
    AutoPostBack="true" Enabled="false">
</infs:WclDropDownList>--%>    
<infs:WclComboBox ID="ddlTenantName" runat="server" OnDataBound="ddlTenantName_DataBound"
    AutoPostBack="true" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" > 
</infs:WclComboBox>
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<script type="text/javascript" language="javascript">
    function Refresh() {
        $jQuery("[id$=btnDoPostBack]").click();
    }
</script>
<%--  OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"--%>
