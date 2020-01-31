<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.VerificationDetailsUnassignedDocumentConrol" CodeBehind="VerificationDetailsUnassignedDocumentConrol.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style type="text/css">
    .hidedocs .trAssignDocCls {
        display: none !important;
        font-size: 28;
    }
</style>

<asp:UpdatePanel ID="updpnlDocumentControl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hdnTenantIdInDocument" runat="server" />
        <asp:HiddenField ID="hdnMergingCompletedDocStatusID" runat="server" />
        <div class="msgbox">
            <asp:Label ID="lblMessage" runat="server">
                             
            </asp:Label>
        </div>
        <div id="uploadControlDiv" runat="server" itemdataid='<%=ComplianceItemId%>' clientidmode="Static">
            <table style="width: 100%;">             
                <tr>    
                    <td class="elm">         
                        <div id="mappedDocument" itemdataid='<%=ComplianceItemId%>'>
                            <asp:Repeater ID="rptrDocuments" runat="server" 
                                >
                                <ItemTemplate>
                                    <table width="95%">
                                        <tr>
                                            <td style="width:40% ">  
                                                <asp:Label ID="lblDocumentName" Text='<%# (!(String.IsNullOrEmpty(Convert.ToString(Eval("DocumentName")))) && (Convert.ToString(Eval("DocumentName")).Length>33))? Convert.ToString(Eval("DocumentName")).Substring(0,33)+"...":Eval("DocumentName") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"DocumentName")%>'
                                                    runat="server"></asp:Label>
                                            </td>
                                            <td style="width:20%">
                                                <asp:Label ID="lblDocumentDescription" Text='<%# DataBinder.Eval(Container.DataItem,"DocumentDescription") %>' runat="server" 
                                                    ToolTip='<%# DataBinder.Eval(Container.DataItem,"DocumentDescriptionToolTip") %>' Font-Italic="true"></asp:Label>
                                            </td>
                                            <td id="tdPrint" style="width: 28%" class="Class1">
                                                <a id="lnkbtnView" href="#" onclick="itemclickedToView(this)" doctypeid='<%# DataBinder.Eval(Container.DataItem,"DocumentType") %>'
                                                    documentid='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                                    unifieddocumentstartpageid='<%# DataBinder.Eval(Container.DataItem,"UnifiedDocumentStartPageID") %>'
                                                    applicantdocumentmergingstatusid='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentMergingStatusID") %>'
                                                    title="Click here to view this document">View</a>                                                                                    
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>     
            </table>
        </div>
        <asp:HiddenField ID="hdnScrDocTypeId" runat="server" />
        <asp:HiddenField ID="hdnIsEdPrevAcpt" runat="server" />
        <asp:HiddenField ID="hdnEmployementDiscTypeCode" runat="server" />
        <asp:HiddenField ID="hdnOrgUsrId" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    $jQuery(document).ready(function () {
        var documentDiv = $jQuery("[id=selectUnselectDocument]");
        var documentDivMapped = $jQuery("[id=mappedDocument]");
        var commandBar = $jQuery("[id$=CommandBarSaveCancel]");
        commandBar.hide();
        documentDiv.hide();
        documentDivMapped.show();
    });

    //UAT-878 - Function to download document
    function PrintDocument(sender) {
        scrollPosition = $jQuery(".vdatapn-top")[0].scrollTop
        var btnID = sender.id;
        var containerID = btnID.substr(0, btnID.indexOf("lnkbtnPrint"));
        var tenantId = $jQuery("#<%= hdnTenantIdInDocument.ClientID %>").val();
        var btnPrint = $jQuery(sender).closest("#tdPrint").find("[id*=" + containerID + "lnkbtnPrint]")[0];
        var applicantDocumentID = btnPrint.attributes["documentid"].value;
        var url = $page.url.create("~/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId=" + applicantDocumentID + " &tenantId=" + tenantId);
        location.href = url;
        return false;
    }

    var selectedFileIndex;

    function clientFileSelected(sender, args) {
        selectedFileIndex = args.get_rowIndex();

    }
    function ShowHiddenDiv() {
        var documentDiv = $jQuery("[id=selectUnselectDocument]");
        var documentDivMapped = $jQuery("[id=mappedDocument]");
        var uploader = $jQuery("[id=uploader]");
        var CommandBarSaveCancel = $jQuery("[id=CommandBarSaveCancel]");
        uploader.show();
        CommandBarSaveCancel.hide();
        documentDivMapped.show();
        documentDiv.hide();

    }

    function itemclickedToView(obj) {

        var documentId = $jQuery(obj).attr('documentId');
        var tenantId = $jQuery("input[id$=hdnTenantIdInDocument]").val();

        var _crntDocTypId = $jQuery(obj).attr('doctypeid');
        var _scrngtDocTypId = $jQuery("input[id$=hdnScrDocTypeId]").val();

        var hdnIsEdPrevAcpt = $jQuery("input[id$=hdnIsEdPrevAcpt]").val();
        var documentType = $jQuery("input[id$=hdnEmployementDiscTypeCode]").val();
        var orgUsrId = $jQuery("input[id$=hdnOrgUsrId]").val();

        if (_scrngtDocTypId == _crntDocTypId) {
            if (orgUsrId != "" && hdnIsEdPrevAcpt != "true") {
                //client admin
                OpenEmployerDisclosureDocument(tenantId, orgUsrId, documentId, documentType)
            }
            else {
                ViewScreeningDocument(tenantId, documentId);
            }
        }
        else {
            var unifiedDocumentStartPageID = $jQuery(obj).attr('UnifiedDocumentStartPageID');
            var applicantDocumentMergingStatusID = $jQuery(obj).attr('applicantdocumentmergingstatusid');

            //UAT-1538
            var selectedDocumentViewType = $jQuery("[id$=rdbLstViewType]").find('input:radio:checked').val();
            //AAAC: is for Unified Document View Type and AAAD: is for Single Document view type
            if (selectedDocumentViewType == "AAAC") {
                //var tenantId = $jQuery("[id$=hdnTenantIdInDocument]").val();
                //var mergingCompletedDocStatusID = $jQuery("[id$=hdnMergingCompletedDocStatusID").val();

                var mergingCompletedDocStatusID = $jQuery("input[id$=hdnMergingCompletedDocStatusID]").val();
                //if documentStatusID is not 3 i.e. not Merging Completed
                if (applicantDocumentMergingStatusID != "" && applicantDocumentMergingStatusID != mergingCompletedDocStatusID) {
                    var IsOKClicked = confirm('There was an error in merging this document. Do you want to view this document individually?');
                    if (IsOKClicked)
                        PageMethods.GetDataForFailedUnifiedDocument(documentId, tenantId, GetFailedUnifiedDocument_CallBack);
                }
                else {
                    if (unifiedDocumentStartPageID == "") {
                        PageMethods.GetDataForUnifiedDocument(documentId, tenantId, get_document_callback);
                    }
                    else {
                        ChangePdfDocVwrScroll(unifiedDocumentStartPageID);
                        return false;
                    }
                }
            }
            else {
                $jQuery("[id$=hdnCurrentDocID]").val(documentId);
                PageMethods.GetSingleDocumentForPDFViewer(documentId, tenantId, GetSingleDocument_CallBack);
            }
        }
    }

    function ShowHideDocuments(obj) {
        var itemdataid = $jQuery(obj).attr('itemdataid');
        var documentDiv = $jQuery("[id=selectUnselectDocument][itemdataid=" + itemdataid + "]");
        var documentDivMapped = $jQuery("[id=mappedDocument][itemdataid=" + itemdataid + "]");
        var CommandBarSaveCancel = $jQuery("[id=CommandBarSaveCancel][itemdataid=" + itemdataid + "]");
        var uploader = $jQuery("[id=uploader][itemdataid=" + itemdataid + "]");
        documentDiv.show();
        CommandBarSaveCancel.hide();
        uploader.hide();
        documentDivMapped.hide();
    }


    function onFileRemoved(obj) {
        var itemdataid = getItemDataIdForUploader(obj);
        var filecount = $jQuery(obj)[0]._selectedFilesCount;
        if (filecount == 0) {
            var commandBar = $jQuery("[id=CommandBarSaveCancel][itemdataid=" + itemdataid + "]");
            commandBar.hide();
        }
    }

    function onFileUploaded(obj, args) {
        var fileSize = args.get_fileInfo().ContentLength;
        //var selectedFileIndex = args.get_fileInfo().Index;
        //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0
        if (fileSize > 0) {
            var itemdataid = getItemDataIdForUploader(obj);
            var filecount = $jQuery(obj)[0]._selectedFilesCount;
            if (filecount == 1) {
                var commandBar = $jQuery("[id=CommandBarSaveCancel][itemdataid=" + itemdataid + "]");
                commandBar.show();
            }
        }
        else {
            obj.deleteFileInputAt(selectedFileIndex);
            isDuplicateFile = false;
            obj.updateClientState();
            alert("File size should be more than 0 byte.");
            return;
        }
    }

    function getItemDataIdForUploader(obj) {
        return $jQuery(obj._element).parents('#uploader').attr('itemdataid');
    }

    var upl_OnClientValidationFailed = function (s, a) {
        var error = false;
        var errorMsg = "";
        var extn = a.get_fileName().substring(a.get_fileName().lastIndexOf('.') + 1, a.get_fileName().length);

        if (a.get_fileName().lastIndexOf('.') != -1) {
            if (s.get_allowedFileExtensions().indexOf(extn) == -1) {
                error = true;
                errorMsg = "! Error: Unsupported File Format";
            }
            else {
                error = true;
                errorMsg = "! Error: File size exceeds 20 MB";
            }
        }
        else {
            error = true;
            errorMsg = "! Error: Unrecognized File Format";
        }

        if (error) {
            var row = a.get_row();
            smsg = document.createElement("span");

            smsg.innerHTML = errorMsg;
            smsg.setAttribute("class", "ruFileWrap");
            smsg.setAttribute("style", "color:red;padding-left:10px;");

            row.appendChild(smsg);
        }
    }

    function abc(obj) {

    }

    function OnClientFileUploading(sender, args) {
        if (args.get_fileName().length > 100) {
            args.set_cancel(true);
        }
        else {
            args.set_cancel(false);
        }
    }
    function ShowCallBackMessage(docMessage) {
        if (docMessage != '') {
            alert(docMessage);
        }
    }



</script>
