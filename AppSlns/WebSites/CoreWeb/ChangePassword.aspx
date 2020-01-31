<%@ Page Language="C#" AutoEventWireup="true" Inherits="ChangePassword"
    StylesheetTheme="NoTheme" MasterPageFile="~/Shared/PublicPageMaster.master" Codebehind="ChangePassword.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="IntsofSecurityModel/ChangePassword.ascx" TagName="ChangePassword" TagPrefix="changePasswordControl" %>
<%@ Import Namespace="INTSOF.Utils" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeadContent" runat="Server">
    <title>
        <%--<%#SysXUtils.GetMessage(ResourceConst.SECURITY_CHANGE_PASSWORD) %>--%>
        <%=Resources.Language.CHGEPASSWRD %>
    </title>      
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" runat="Server">     
    <h1 class="page_header">
        <%=Resources.Language.CHGEPASSWRD %></h1>    
    <changePasswordControl:ChangePassword ID="changePassword" runat="server" IsFirstTimeLogin="true" />
      <asp:HiddenField ID="hdnLanguageCode" runat="server" />
    <asp:Literal ID="litFooter" runat="server" Visible="false"></asp:Literal>
</asp:Content>
