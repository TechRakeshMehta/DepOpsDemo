<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.VerificationDetailsDocumentConrol" CodeBehind="VerificationDetailsDocumentConrol.ascx.cs" %>
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
                <tr class="trAssignDocCls" runat="server" id="divAssignDocCls">
                    <td class="lbl"></td>
                    <td>
                        <a id="lnkbtnViewAll" href="#" onclick="ShowHideDocuments(this)" itemdataid='<%=ComplianceItemId%>'
                            title="Click here to select one or more previously uploaded documents to associate with this Item">Assign More Documents</a>
                    </td>
                </tr>


                <tr>
                    <td class="lbl"></td>
                    <td class="elm">
                        <div id="selectUnselectDocument" itemdataid='<%=ComplianceItemId%>' style="display: none">
                            <asp:Repeater ID="rptrAllDocuments" runat="server" OnItemDataBound="rptrAllDocuments_ItemDataBound">
                                <ItemTemplate>
                                    <div style="width: auto;">
                                        <asp:CheckBox ID="chkIsMapped" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsChecked")%>' OnClick="checkchanged(this);"
                                            Text='<%# DataBinder.Eval(Container.DataItem,"DocumentName") %>' />
                                        <asp:HiddenField ID="hdnDocumentId" Value='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                            runat="server" />
                                        <asp:Label ID="lblErrorMessage" runat="server" Style="color: red; padding-left: 10px;"></asp:Label>
                                    </div>
                                    <br />
                                </ItemTemplate>
                            </asp:Repeater>
                            <%--Commented Related to UAT-2768: <infsu:CommandBar ID="comandSaveMapping" runat="server" DisplayButtons="Save,Cancel" Visible="false"
                                AutoPostbackButtons="Save,Cancel" SaveButtonText="Assign" OnSaveClientClick="ShowHiddenDiv"
                                OnSaveClick="comandSaveMapping_SubmitClick" />--%>
                            <infsu:CommandBar ID="comandSaveMapping" runat="server" DisplayButtons="Cancel"
                                AutoPostbackButtons="Cancel" />
                        </div>
                        <div id="mappedDocument" itemdataid='<%=ComplianceItemId%>'>
                            <asp:Repeater ID="rptrDocuments" runat="server" OnItemCommand="rptrDocuments_ItemCommand"
                                OnItemDataBound="rptrDocuments_ItemDataBound">
                                <ItemTemplate>
                                    <table width="95%">
                                        <tr>
                                            <td style="width: 40%">
                                                <asp:Label ID="lblDocumentName" Text='<%# (!(String.IsNullOrEmpty(Convert.ToString(Eval("DocumentName")))) && (Convert.ToString(Eval("DocumentName")).Length>33))? Convert.ToString(Eval("DocumentName")).Substring(0,33)+"...":Eval("DocumentName") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"DocumentName")%>'
                                                    runat="server"></asp:Label>
                                            </td>
                                            <td style="width: 20%">
                                                <asp:Label ID="lblDocumentDescription" Text='<%# DataBinder.Eval(Container.DataItem,"DocumentDescription") %>' runat="server"
                                                    ToolTip='<%# DataBinder.Eval(Container.DataItem,"DocumentDescriptionToolTip") %>' Font-Italic="true"></asp:Label>
                                            </td>
                                            <td id="tdPrint" style="width: 28%" class="Class1">
                                                <a id="lnkbtnView" href="#" onclick="itemclickedToView(this)" doctypeid='<%# DataBinder.Eval(Container.DataItem,"DocumentType") %>'
                                                    documentid='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                                    unifieddocumentstartpageid='<%# DataBinder.Eval(Container.DataItem,"UnifiedDocumentStartPageID") %>'
                                                    applicantdocumentmergingstatusid='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentMergingStatusID") %>'
                                                    title="Click here to view this document">View</a>
                                                <asp:Literal ID="litSymbol" runat="server" Text="|"></asp:Literal>
                                                <asp:LinkButton ID="lnkbtnDelete" OnClientClick="if (this.className != 'aspNetDisabled') { if (!confirm('Are you sure you want to remove this document?')) { return false; } }"
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                                    CommandName="remove" runat="server">Remove</asp:LinkButton>
                                                <asp:Literal ID="litSymbol2" runat="server" Text="|"></asp:Literal>
                                                <asp:LinkButton ID="lnkbtnPrint" documentid='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                                    runat="server" OnClientClick="PrintDocument(this)">Print</asp:LinkButton>
                                                <%--<asp:HiddenField ID="hdnCurrentDocumentID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>' />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <div class='sxro sx3co' runat="server" id="dvBrowse">
                            <div class='sxlm' style="overflow: inherit;">
                                <div itemdataid='<%=ComplianceItemId %>' id="uploader">
                                    <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                                        AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT"
                                        MultipleFileSelection="Disabled" OnClientFileSelected="clientFileSelected" OnClientFileUploaded="onFileUploaded" ToolTip="Click here to upload and associate one or more documents to this Item"
                                        OnClientFileUploadRemoved="onFileRemoved" OnClientValidationFailed="upl_OnClientValidationFailed" OnClientFileUploading="OnClientFileUploading">
                                        <Localization Select="Browse" />
                                    </infs:WclAsyncUpload>
                                </div>
                            </div>
                            <div id="CommandBarSaveCancel" itemdataid='<%=ComplianceItemId %>' style="display: none;">
                                <infsu:CommandBar ID="fsucCmdBarMUser" runat="server" DisplayButtons="Save,Cancel"
                                    AutoPostbackButtons="Save,Cancel" OnSaveClick="fsucCmdBar1_SubmitClick" />
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnScrDocTypeId" runat="server" />
        <asp:HiddenField ID="hdnIsEdPrevAcpt" runat="server" />
        <asp:HiddenField ID="hdnEmployementDiscTypeCode" runat="server" />
        <asp:HiddenField ID="hdnOrgUsrId" runat="server" />
        <asp:HiddenField ID="hdnIsRestrictedFileTypeChecked" runat="server" />
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

    function checkchanged(obj) {       
        var selectedAssignDocs = $jQuery("[id*=" + "selectUnselectDocument" + "] input:checkbox")
        var IsRestrictedFileTypeExist = false;  
        if (selectedAssignDocs != null && selectedAssignDocs != "" && selectedAssignDocs.length > 0) {
            for (var i = 0; i < selectedAssignDocs.length; i++) {
                if (selectedAssignDocs[i].checked) {
                    var btnID = selectedAssignDocs[i].id;
                    var containerID = btnID.replace("chkIsMapped", "lblErrorMessage");
                    var IsRestrictedType = $jQuery("[id=" + containerID + "]").text()
                    if (IsRestrictedType != "" && IsRestrictedType != null) {
                        IsRestrictedFileTypeExist = "true";
                        break;
                    }
                }
            }
        }
        if (IsRestrictedFileTypeExist != null && IsRestrictedFileTypeExist != "" && IsRestrictedFileTypeExist == "true") {
            $jQuery("[id$=hdnIsRestrictedFileTypeChecked]").val("true");
        }
        else { $jQuery("[id$=hdnIsRestrictedFileTypeChecked]").val("false"); }
    }

</script>
