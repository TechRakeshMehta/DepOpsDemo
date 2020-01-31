<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisclosureAndRelease.ascx.cs" Inherits="CoreWeb.BkgOperations.UserControl.Views.DisclosureAndRelease" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Repeater ID="rptDisclosureAndRelease" runat="server" OnItemDataBound="rptDisclosureAndRelease_OnItemDataBound">
    <ItemTemplate>
        <div runat="server" style="height: 500px;">
            <iframe id="iframePdfDocViewer" runat="server" width="100%" height="100%"></iframe>
        </div>
        <center>
            <div>
        <%-- Uncomment for display the checkboxes after each Pdf.   16-July-2014--%>
        <%--<asp:CheckBox ID="chkDisclosureAndRelease" runat="server" Text=" I have read and agree to the above Disclosure & Authorization form" 
            CssClass="dandrDisclosureCheckbox" />--%>
       </div>
                 </center>
        <br />
    </ItemTemplate>
</asp:Repeater>
