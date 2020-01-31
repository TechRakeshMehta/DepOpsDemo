<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.IntsofSecurityModel.Views.SysXSecurityModelDefault"
    Title="Default" MasterPageFile="~/Shared/DefaultMaster.master" ErrorPage="~/IntsofSecurityModel/Default.aspx?args=DF319F553C8300A9BB59BD06DBE2FA8C150C156C45F1020C653A22C5253E82E7C0D6667C4F41C33578BE33B63CC4FA8D99520939F5192D2F" Codebehind="Default.aspx.cs" %>
 <%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <contenttemplate>
            <asp:PlaceHolder runat="server" ID="plcDynamic"></asp:PlaceHolder>
        </contenttemplate>
</asp:Content>
