<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.VerificationDocumentControlReadOnlyMode" CodeBehind="VerificationDocumentControlReadOnlyMode.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:UpdatePanel ID="updpnlDocumentControl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hdnTenantIdReadOnly" runat="server" />
        <asp:HiddenField ID="hdnMergingCompletedDocmntStatusID" runat="server" />
        <div id="mappedDocument">
            <asp:Repeater ID="rptrDocuments" runat="server">
                <ItemTemplate>
                    <table style="width: 95%;">
                        <tr>
                            <td style="width: 40%;">
                                <asp:Label ID="lblDocumentName" Text='<%# (!(String.IsNullOrEmpty(Convert.ToString(Eval("DocumentName")))) && (Convert.ToString(Eval("DocumentName")).Length>33))? Convert.ToString(Eval("DocumentName")).Substring(0,33)+"...":Eval("DocumentName") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"DocumentName")%>'
                                    runat="server"></asp:Label>
                            </td>
                            <td style="width: 20%">
                                <asp:Label ID="lblDocumentDescription" Text='<%# DataBinder.Eval(Container.DataItem,"DocumentDescription") %>' runat="server"
                                    ToolTip='<%# DataBinder.Eval(Container.DataItem,"DocumentDescriptionToolTip") %>' Font-Italic="true"></asp:Label>
                            </td>
                            <td style="width: 20%" class="Class1">
                                <a id="lnkbtnView" href="#" onclick="itemclickedToView(this)" doctypeid='<%# DataBinder.Eval(Container.DataItem,"DocumentType") %>' documentid='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                    unifieddocumentstartpageid='<%# DataBinder.Eval(Container.DataItem,"UnifiedDocumentStartPageID") %>'
                                    applicantdocumentmergingstatusid='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentMergingStatusID") %>'
                                    title="Click here to view this document">View</a>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <asp:HiddenField ID="hdnScrDocTypeId" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    function itemclickedToView(obj) {

        var documentId = $jQuery(obj).attr('documentId');
        var tenantId = $jQuery("input[id$=hdnTenantIdReadOnly]").val();

        var _crntDocTypId = $jQuery(obj).attr('doctypeid');
        var _scrngtDocTypId = $jQuery("input[id$=hdnScrDocTypeId]").val();

        if (_scrngtDocTypId == _crntDocTypId) {
            ViewScreeningDocument(tenantId, documentId);
        }
        else {
            var unifiedDocumentStartPageID = $jQuery(obj).attr('UnifiedDocumentStartPageID');
            var applicantDocumentMergingStatusID = $jQuery(obj).attr('ApplicantDocumentMergingStatusID');


            //UAT-1538
            var selectedDocumentViewType = $jQuery("[id$=rdbLstViewType]").find('input:radio:checked').val();
            //AAAC: is for Unified Document View Type and AAAD: is for Single Document view type
            if (selectedDocumentViewType == "AAAC") {
                var mergingCompletedDocStatusID = $jQuery("input[id$=hdnMergingCompletedDocmntStatusID]").val();
                //if documentStatusID is not 3 i.e. not Merging Completed
                if (applicantDocumentMergingStatusID != "" && applicantDocumentMergingStatusID != mergingCompletedDocStatusID) {
                    var IsOKClicked = confirm('There was an error in merging this document. Do you want to view this document individually?');
                    if (IsOKClicked)
                        PageMethods.GetDataForFailedUnifiedDocument(documentId, tenantId, GetFailedUnifiedDocument_CallBack);
                }
                else {
                    if (unifiedDocumentStartPageID > 0) {
                        ChangePdfDocVwrScroll(unifiedDocumentStartPageID);
                    }
                }
            }
            else {
                //var tenantId = $jQuery("input[id$=hdnTenantIdInDocument]").val();
                $jQuery("[id$=hdnCurrentDocID]").val(documentId);
                PageMethods.GetSingleDocumentForPDFViewer(documentId, tenantId, GetSingleDocument_CallBack);
            }
            return false;
        }
    }

</script>
