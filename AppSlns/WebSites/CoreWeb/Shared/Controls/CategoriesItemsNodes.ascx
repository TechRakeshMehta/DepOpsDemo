<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoriesItemsNodes.ascx.cs" Inherits="CoreWeb.Shell.Views.CategoriesItemsNodes" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
        <div class="sxlb" style="width: 51.5%; max-height: 150px; overflow: auto">
            <asp:Repeater ID="rptNodes" runat="server">
                <HeaderTemplate>
                    <table>
                        <tr>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# ((INTSOF.UI.Contract.ComplianceManagement.NodesContract)Container.DataItem).DPM_Label %>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table><br />
                </FooterTemplate>
            </asp:Repeater>
        </div>
    
    <div class='sxroend'>
    </div>
