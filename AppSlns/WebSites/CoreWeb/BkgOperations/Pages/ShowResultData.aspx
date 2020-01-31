<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowResultData.aspx.cs" Inherits="CoreWeb.BkgOperations.Pages.ShowResultData" MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Repeater ID="rptrServiceResult" runat="server">
        <ItemTemplate>
            <table id="mytable" cellspacing="0" width="100%" align="center">
                <tr>
                    <td style="width: 100%;">
                        <asp:Literal runat="server" ID="litResultData" Text='<%# Container.DataItem %>'></asp:Literal>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
        <SeparatorTemplate>
            <h1 style="width: 90%;">----------------------------------------------------------------------------------------------------------------------------------------</h1>
        </SeparatorTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:Repeater>
    <div style="display: none">
        <infs:WclButton ID="btn" runat="server" Text="dummyButton"></infs:WclButton>
    </div>
</asp:Content>
