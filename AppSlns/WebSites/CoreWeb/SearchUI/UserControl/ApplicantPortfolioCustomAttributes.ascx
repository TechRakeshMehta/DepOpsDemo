<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Search.Views.ApplicantPortfolioCustomAttributes" Codebehind="ApplicantPortfolioCustomAttributes.ascx.cs" %>
<%@ Register TagPrefix="infsu" TagName="CustomAttribute" Src="~/ComplianceAdministration/UserControl/CustomAttributeLoader.ascx" %>
<div class="section">
    <h1 class="mhdr">
       <%-- Custom Attributes--%><asp:Label ID="lblApplicantPortFolioSearch" runat="server" Text="Custom Attributes" title="The data entered for institute-specific fields is displayed in this section"></asp:Label>
    </h1>
    <div class="content">
        <div id="NoRecords" runat="server" visible="true" style="width: 98%; background-color: White;
            border: 1px; border-color: Gray; border-style: solid; font-size: 12px; font-family: Segoe UI,Arial,Helvetica,sans-serif;
            padding-top: 3px; padding-bottom: 4px; padding-left: 7px; padding-right: 7px;">
            <asp:Label ID="lblMessage" runat="server" BackColor="White" ForeColor="#33333">
            </asp:Label>
        </div>
        <div id="divcontent" runat="server">
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                    <asp:Repeater ID="rptrCustomAttribute" runat="server" OnItemDataBound="rptrCustomAttribute_ItemDataBound" OnLoad="rptrCustomAttribute_Load">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="content">
                                <div class="sxform auto">
                                    <infsu:CustomAttribute ID="customAttribute" Title='<%# INTSOF.Utils.Extensions.HtmlEncode( Convert.ToString(Eval("DPM_Label"))) %>' MappingRecordId='<%#Eval("DPM_InstitutionNodeID") %>'
                                        ValueRecordId='<%# Eval("DPM_ID") %>' runat="server" />
                                    <div class='sxroend'>
                                    </div>
                                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </div>
        </div>
    </div>
</div>
