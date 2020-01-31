<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SeriesUnMappedAttributes.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.SeriesUnMappedAttributes" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    th.thHeader {
        font-size: 13px;
        font-weight: bold;
        padding: 2px 41px 4px 52px;
        text-align: center;
    }

    th.thStateAbb {
        font-size: 13px;
        font-weight: bold;
        padding: 2px 41px 4px 52px;
    }

    td.tdItem {
        border-color: #dedede;
        background-color: #eeeeee;
        color: black;
        border-right-width: 0px;
        padding: 3px 5px 4px;
        border-style: solid;
        width: 35%;
        line-height: 10px !important;
    }

    td.tdStateAbb {
        border-color: #dedede;
        background-color: #eeeeee;
        color: black;
        text-align: center;
        border-style: solid;
        line-height: 10px !important;
    }
</style>

<div class="section" id="divAttributes" runat="server" visible="false">
    <h1 class="mhdr">Unmapped Attributes</h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
            </div>
            <asp:Panel ID="pnlAttributeContainer" runat="server" CssClass="sxpnl">
                <asp:Repeater ID="rptUnMappedAttributes" runat="server" OnItemDataBound="rptUnMappedAttributes_ItemDataBound">
                    <HeaderTemplate>
                        <table id="tblUnMappedAttributes" style="padding-top: 5px; width: 99.5%;">
                            <tr>
                                <hr />
                            </tr>
                            <tr>
                                <th class="thHeader" style="padding-bottom: 10px !important; background-color: #C6C5C5; border: solid 1px Black;">Item Name</th>
                                <th class="thHeader" style="padding-bottom: 10px !important; background-color: #C6C5C5; border: solid 1px Black;">Attribute Name</th>
                                <th class="thHeader" style="padding-bottom: 10px !important; background-color: #C6C5C5; border: solid 1px Black;">Attribute Type</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="tdItem" style="border: solid 1px Black !important;">
                                <asp:Label ID="lblItemName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CmpItemName")))%>' runat="server"></asp:Label>
                            </td>
                            <td class="tdItem" style="border: solid 1px Black !important;">
                                <asp:Label ID="lblAttributeName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CmpAttributeName")))%>' runat="server"></asp:Label>
                            </td>
                            <td class="tdItem" style="border: solid 1px Black !important;">
                                <asp:Panel ID="pnlAttributes" runat="server">
                                </asp:Panel>
                            </td>
                            <asp:HiddenField ID="hdnItemID" Value='<%# Eval("CmpItemId") %>' runat="server" />
                            <asp:HiddenField ID="hdnAttributeID" Value='<%# Eval("CmpAttributeId") %>' runat="server" />
                            <asp:HiddenField ID="hdnAttrDatatypeCode" Value='<%# Eval("CmpAttributeDatatypeCode") %>' runat="server" />
                            <asp:HiddenField ID="hdnItemSeriesItemID" Value='<%# Eval("CmpItemSeriesItemId") %>' runat="server" />
                            <asp:HiddenField ID="hdnItemSeriesItemAttributeValueID" Value='<%# Eval("CmpItemSeriesItemAttributeValueId") %>' runat="server" />
                        </tr>
                        <%-- <tr style="height: 3px !important;">
                            <td colspan="3"></td>
                        </tr>--%>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBar" runat="server" DefaultPanel="pnlAttributeContainer"
            DisplayButtons="Save" OnSaveClick="fsucCmdBar_SaveClick" SaveButtonText="Save Attribute(s)"
            AutoPostbackButtons="Save" ValidationGroup="grpSaveAttributes">
        </infsu:CommandBar>
    </div>
</div>
