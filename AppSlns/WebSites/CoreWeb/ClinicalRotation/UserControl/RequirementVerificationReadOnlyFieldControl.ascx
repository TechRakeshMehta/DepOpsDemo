<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementVerificationReadOnlyFieldControl.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.RequirementVerificationReadOnlyFieldControl" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div id="dvMain" runat="server">
    <div class='form-group col-md-3'>
        <div class="cptn">
            <asp:Literal ID="litFieldName" runat="server"></asp:Literal>
        </div>
        <asp:Panel ID="pnlFieldControl" runat="server">
            <asp:Repeater ID="rptDocuments" runat="server" Visible="false">
                <ItemTemplate>
                    <table width="100%">
                        <tr>
                            <td style="font-size: 11.5px; padding-left: 5px; text-align: left">
                                <a id="lnkbtnDownloadDoc" onclick="ViewDocument(<%# CurrentViewContext.SelectedTenantId %>,<%# DataBinder.Eval(Container.DataItem,"Item1") %>)"
                                    href="#"><%# DataBinder.Eval(Container.DataItem,"Item2") %></a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
            <asp:HiddenField ID="hdfFieldTypeCode" runat="server" />
            <asp:HiddenField ID="hdfApplicantFieldDataId" runat="server" />
            <asp:HiddenField ID="hdfFieldId" runat="server" />
            <asp:HiddenField ID="hdfDocType" runat="server" />
        </asp:Panel>
        <div class='vldx'>
            <asp:Panel ID="pnlValidation" runat="server">
            </asp:Panel>
        </div>
    </div>
</div>
