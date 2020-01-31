<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentUrlInfo.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.DocumentUrlInfo" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="~/Resources/Mod/Shared/DocumentUrlInfo.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div id="divDocumentUrl" runat="server">
    <asp:Repeater runat="server" ID="rptrDocumentUrl" OnItemCommand="rptrDocumentUrl_ItemCommand" OnItemDataBound="rptrDocumentUrl_ItemDataBound">
        <ItemTemplate>
            <div class='sxro sx3co'>
                <div class='sxlb'>
                    <asp:Label class='cptn' runat="server" ID="rptrLable1">URL for More Information</asp:Label>
                </div>
                <div class='sxlm'>
                    <asp:Label runat="server" ID="lblSampleDocFormURL"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("SampleDocFormURL"))) %> </asp:Label>
                    <infs:WclTextBox runat="server" ID="txtSampleDocFormURL1" Text='<%# Eval("SampleDocFormURL") %>' Enabled="false" Visible="false" MaxLength="250"></infs:WclTextBox>
                    <infs:WclTextBox runat="server" ID="txtSampleDocFormURL" Text='<%# Eval("SampleDocFormURL") %>' Visible="false" MaxLength="250"></infs:WclTextBox>
                    <div id="rptrDivErrorMessage" runat="server" visible="false">
                        <asp:Label ID="rptrLblErrorMsg" runat="server" Text="" CssClass="errmsg"></asp:Label>
                    </div>
                    <div class='vldx'>
                        <asp:RegularExpressionValidator ID="revRptrSampleDocFormURL" runat="server" ControlToValidate="txtSampleDocFormURL"
                            class="errmsg" ValidationGroup="grpFormSave" Display="Dynamic" ErrorMessage="Please enter a valid Url."
                            ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$">
                        </asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvRptrSampleDocFormURL" ControlToValidate="txtSampleDocFormURL"
                            class="errmsg" ValidationGroup="grpFormSave" Display="Dynamic" ErrorMessage="URL For More Information is required." />
                    </div>
                </div>
                <div runat="server" id="dvrptrURLDisplaylabel">
                    <div class='sxlb'>
                        <asp:Label class='cptn' runat="server" ID="rptrURLLable1">URL Label</asp:Label>
                    </div>
                    <div class='sxlm'>
                        <asp:Label runat="server" ID="lblSampleDocFormURLDisplayLabel"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("SampleDocFormUrlDisplayLabel"))) %> </asp:Label>
                        <infs:WclTextBox runat="server" ID="txtSampleDocFormURLDisplayLabel1" Text='<%# Eval("SampleDocFormUrlDisplayLabel") %>' Enabled="false" Visible="false" MaxLength="250"></infs:WclTextBox>
                        <infs:WclTextBox runat="server" ID="txtSampleDocFormURLDisplayLabel" Text='<%# Eval("SampleDocFormUrlDisplayLabel") %>' Visible="false" MaxLength="250"></infs:WclTextBox>
                    </div>
                </div>
                <div runat="server" id="rptrDisplayUrlLabelSection">
                    <div class='sxlb'>
                        <asp:Label class='cptn' runat="server" ID="rptrLable2">More Information Label</asp:Label>
                    </div>
                    <div class='sxlm'>
                        <asp:Label runat="server" ID="lblSampleDocFormURLLabel"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("SampleDocFormURLLabel"))) %> </asp:Label>
                        <infs:WclTextBox runat="server" ID="txtSampleDocFormURLLabel1" Text='<%# Eval("SampleDocFormURLLabel") %>' Enabled="false" Visible="false" MaxLength="100"></infs:WclTextBox>
                        <infs:WclTextBox runat="server" CssClass="helloAni" ID="txtSampleDocFormURLLabel" Text='<%# Eval("SampleDocFormURLLabel") %>' Visible="false" MaxLength="100"></infs:WclTextBox>
                        <infs:WclTextBox runat="server" ID="hdnDocUrlId1" Text='<%# Eval("ID") %>' Visible="false"></infs:WclTextBox>
                    </div>

                </div>
                <div id="divButtons" runat="server" style="text-align: left; padding-right: 5px;">
                    <asp:LinkButton CommandName="edit" runat="server" Text="Edit" ID="btnEdit" ValidationGroup="grpFormSave" Style="color: blue"></asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete the Document Url ?')" runat="server" CommandName="delete" Text="Delete" CausesValidation="false" Style="color: blue" />
                </div>
        </ItemTemplate>
    </asp:Repeater>
    <div id="divFooter" runat="server">
        <div class='sxro sx3co' runat="server">
            <div class='sxlb'>
                <asp:Label class='cptn' runat="server" ID="divFooterLable1">URL for More Information</asp:Label>
            </div>
            <div class='sxlm'>
                <infs:WclTextBox runat="server" ID="txtNewSampleDocFormURL" MaxLength="250"></infs:WclTextBox>
                <div id="divErrorMessage" runat="server" visible="false">
                    <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="errmsg"></asp:Label>
                </div>
                <div class='vldx'>
                    <asp:RegularExpressionValidator ID="revNewSampleDocFormURL" runat="server" ControlToValidate="txtNewSampleDocFormURL"
                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter a valid Url."
                        ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$">
                    </asp:RegularExpressionValidator>

                </div>
            </div>
            <div runat="server" id="footerURLDisplaylabel">
                <div class='sxlb'>
                    <asp:Label class='cptn' runat="server" ID="divFooterURLLabel1">URL Label</asp:Label>
                </div>
                <div class='sxlm'>
                    <infs:WclTextBox runat="server" ID="txtNewSampleDocFormUrlDisplayLabel" MaxLength="250"></infs:WclTextBox>
                </div>
            </div>
            <div id="footerDisplayUrlLabelSection" runat="server">
                <div class='sxlb'>
                    <asp:Label class='cptn' runat="server" ID="divFooterLable2">More Information Label</asp:Label>
                </div>
                <div class='sxlm'>
                    <infs:WclTextBox runat="server" ID="txtNewSampleDocFormURLLabel" MaxLength="100"></infs:WclTextBox>

                    <infs:WclTextBox runat="server" ID="hdnDocUrlId" Text='<%# Eval("SampleDocFormURLLabel") %>' Visible="false"></infs:WclTextBox>

                </div>
            </div>
            <div style="text-align: left; padding-right: 5px;">
                <asp:LinkButton ID="btnAddNewRecord" runat="server" OnClick="OnAddRecord" Text="Add" ValidationGroup="grpFormSubmit" Style="color: blue" />
            </div>
        </div>

    </div>


</div>
<div class='sxroend'>
</div>
