<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementVerificationFieldControl.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.RequirementVerificationFieldControl" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%--<div class='form-group col-md-3'>--%>
<div class='sxro sx1co'>
    <div class="sxlb">
        <asp:Label CssClass="cptn" ID="litFieldName" runat="server"></asp:Label>
    </div>
    <div class='sxlm'>
        <asp:Panel ID="pnlFieldControl" runat="server">
            <%--<asp:Repeater ID="rptDocuments" runat="server" Visible="false">
                <ItemTemplate>
                    <table width="100%">
                        <tr>
                            <td style="font-size: 11.5px; padding-left: 5px; text-align: left">
                                <a id="lnkbtnDownloadDoc" class="form-control" onclick="itemclickedToView(<%# CurrentViewContext.SelectedTenantId %>,<%# DataBinder.Eval(Container.DataItem,"Item1") %>)" href="#"><%# DataBinder.Eval(Container.DataItem,"Item2") %></a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>--%>

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
    <div class='sxroend'>
    </div>
</div>
<script type="text/javascript">
    function ViewDocItemClickedToView(tenantId, documentId) {
        PageMethods.GetSingleDocumentForPDFViewer(documentId, tenantId, GetSingleDocument_CallBack);
    }
</script>
