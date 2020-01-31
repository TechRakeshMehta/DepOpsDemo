<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ClinicalRotation.Views.RequirementVerificationDetailsDocumentConrol" CodeBehind="RequirementVerificationDetailsDocumentConrol.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    .hidedocs .trAssignDocCls
    {
        display: none !important;
        font-size: 28px;
    }
</style>

<asp:UpdatePanel ID="updpnlDocumentControl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="msgbox">
            <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false">
            </asp:Label>
        </div>
        <div id="uploadControlDiv" runat="server" itemdataid='<%=RequirementItemId%>' clientidmode="Static">
            <table style="width: 100%;">
                <tr class="trAssignDocCls" runat="server" id="divAssignDocCls">
                    <td class="lbl"></td>
                    <td>
                        <a id="lnkbtnViewAll" href="#" onclick="ShowHideDocuments(this)" itemdataid='<%=RequirementItemId%>'
                            title="Click here to select one or more previously uploaded documents to associate with this Item">Assign More Documents</a>
                    </td>
                </tr>

                <tr>
                    <td class="lbl"></td>
                    <td class="elm">
                        <div id="selectUnselectDocument" itemdataid='<%=RequirementItemId%>' style="display: none">
                            <asp:Repeater ID="rptrAllDocuments" runat="server" OnItemDataBound="rptrAllDocuments_ItemDataBound">
                                <ItemTemplate>
                                    <div style="width: auto;">
                                        <asp:CheckBox ID="chkIsMapped" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsChecked")%>'
                                            Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("DocumentName"))) %>' />
                                        <asp:HiddenField ID="hdnDocumentId" Value='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                            runat="server" />
                                    </div>
                                    <br />
                                </ItemTemplate>
                            </asp:Repeater>
                            <%--  <infsu:CommandBar ID="comandSaveMapping" runat="server" DisplayButtons="Save,Cancel"
                                AutoPostbackButtons="Save,Cancel" SaveButtonText="Assign" OnSaveClientClick="ShowHiddenDiv"
                                OnSaveClick="comandSaveMapping_SubmitClick" OnCancelClick="comandSaveMapping_CancelClick" ValidationGroup="validationNotrequired" />--%>
                            <infsu:CommandBar ID="comandSaveMapping" OnCancelClick="comandSaveMapping_CancelClick" OnCancelClientClick="ShowHideDocDiv" runat="server" DisplayButtons="Cancel"
                                AutoPostbackButtons="Cancel" />
                        </div>
                        <div id="mappedDocument" itemdataid='<%=RequirementItemId%>'>
                            <asp:Repeater ID="rptrDocuments" runat="server" OnItemCommand="rptrDocuments_ItemCommand"
                                OnItemDataBound="rptrDocuments_ItemDataBound">
                                <ItemTemplate>
                                    <table width="95%">
                                        <tr>
                                            <td style="width: 40%;">
                                                <asp:Label ID="lblDocumentName" Text='<%# (!(String.IsNullOrEmpty(Convert.ToString(Eval("FileName")))) && (Convert.ToString(Eval("FileName")).Length>33))? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("FileName")).Substring(0,33))+"...": INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("FileName"))) %>'
                                                    ToolTip='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("FileName")))%>'
                                                    runat="server"></asp:Label>
                                            </td>
                                            <td style="width: 20%;">
                                                <asp:Label ID="lblDocumentDescription" Text='<%# (!(String.IsNullOrEmpty(Convert.ToString(Eval("Description")))) && (Convert.ToString(Eval("Description")).Length>15))? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Description")).Substring(0,15))+"...": INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Description"))) %>'
                                                    ToolTip='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Description"))) %>' runat="server" Font-Italic="true"></asp:Label>
                                            </td>
                                            <td id="tdPrint" style="width: 28%" class="Class1">
                                                <a id="lnkbtnView" href="#" onclick="itemclickedToView(this)" doctypeid='<%# DataBinder.Eval(Container.DataItem,"DocumentType") %>'
                                                    documentid='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                                    title="Click here to view this document">View</a>
                                                <asp:Literal ID="litSymbol" runat="server" Text="|"></asp:Literal>
                                                <asp:LinkButton ID="lnkbtnDelete" OnClientClick="if (this.className != 'aspNetDisabled') { if (!confirm('Are you sure you want to remove this document?')) { return false; } }"
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                                    CommandName="remove" runat="server" CausesValidation="false">Remove</asp:LinkButton>
                                                <asp:Literal ID="litSymbol2" runat="server" Text="|"></asp:Literal>
                                                <asp:LinkButton ID="lnkbtnPrint" documentid='<%# DataBinder.Eval(Container.DataItem,"ApplicantDocumentId") %>'
                                                    runat="server" CommandName="print" OnClientClick="PrintDocument(this)" CausesValidation="false">Print</asp:LinkButton>
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
                            <div class='sxlm'>
                                <div itemdataid='<%=RequirementItemId %>' id="uploader">
                                    <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                                        AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT"
                                        MultipleFileSelection="Disabled" OnClientFileSelected="clientFileSelected" OnClientFileUploaded="onFileUploaded" ToolTip="Click here to upload and associate one or more documents to this Item"
                                        OnClientFileUploadRemoved="onFileRemoved" OnClientValidationFailed="upl_OnClientValidationFailed" OnClientFileUploading="OnClientFileUploading">
                                        <Localization Select="Browse" />
                                    </infs:WclAsyncUpload>
                                </div>
                            </div>
                            <div id="CommandBarSaveCancel" itemdataid='<%=RequirementItemId %>' style="display: none;">
                                <infsu:CommandBar ID="fsucCmdBarMUser" runat="server" DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel"
                                    OnSaveClick="fsucCmdBarMUser_SubmitClick" OnCancelClick="fsucCmdBarMUser_CancelClick" ValidationGroup="validationNotrequired" />
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
        <%-- <div style="display: none">
            <asp:Button runat="server" ID="btnReload" OnClick="btnReload_Click" />
        </div>--%>
        <asp:HiddenField ID="hdnTenantIdInDocument" runat="server" />
        <asp:HiddenField ID="hdnpermissionName" Value="invalid" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    var openDoucmentDivArray = [];
    Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
    Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };

    $jQuery(document).ready(function () {
        var documentDiv = $jQuery("[id=selectUnselectDocument]");
        var documentDivMapped = $jQuery("[id=mappedDocument]");
        var commandBar = $jQuery("[id$=CommandBarSaveCancel]");
        commandBar.hide();
        documentDiv.hide();
        documentDivMapped.show();

        //UAT  2371
        //CheckDocLinkVisible();
    });

    
    function pageLoad() { //UAT-3345
        $jQuery.each(openDoucmentDivArray, function (index, value) {            
            var docDivStatusId = value;
            var documentDiv = $jQuery("[id=selectUnselectDocument][itemdataid=" + docDivStatusId + "]");
            var documentDivMapped = $jQuery("[id=mappedDocument][itemdataid=" + docDivStatusId + "]");

            if (docDivStatusId != null && docDivStatusId != "") {
                documentDiv.show();
                documentDivMapped.hide();
            }
            else {
                documentDiv.hide();
                documentDivMapped.show();
            }
        });


    }

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
    function ShowHideDocDiv() {

        openDoucmentDivArray = [];
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
        var tenantId = $jQuery("#<%= hdnTenantIdInDocument.ClientID %>").val();
        PageMethods.GetSingleDocumentForPDFViewer(documentId, tenantId, GetSingleDocument_CallBack);
    }

    function ViewDocInPDFViewer(documentId) {
        var tenantId = $jQuery("#<%= hdnTenantIdInDocument.ClientID %>").val();
        PageMethods.GetSingleDocumentForPDFViewer(documentId, tenantId, GetSingleDocument_CallBack);
    }

    function ShowHideDocuments(obj) {        
        var permissionVal = $jQuery("#<%= hdnpermissionName.ClientID %>").val();
        if (permissionVal != "none") {
            var itemdataid = $jQuery(obj).attr('itemdataid');
            var documentDiv = $jQuery("[id=selectUnselectDocument][itemdataid=" + itemdataid + "]");
            var documentDivMapped = $jQuery("[id=mappedDocument][itemdataid=" + itemdataid + "]");
            var CommandBarSaveCancel = $jQuery("[id=CommandBarSaveCancel][itemdataid=" + itemdataid + "]");
            var uploader = $jQuery("[id=uploader][itemdataid=" + itemdataid + "]");
            documentDiv.show();            
            openDoucmentDivArray.push(itemdataid);
            CommandBarSaveCancel.hide();
            uploader.hide();
            documentDivMapped.hide();
        }
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

    //[UAT-4276]
    $jQuery(document).ready(function () {
        $jQuery(".vdatapn-top.scroll-box").scroll(function () {
            $jQuery.each($jQuery('.RadComboBox').toArray(), function (index, obj) {
                $find(obj.id).hideDropDown();
            });
        });
    });   

    //function RefeshRequirementItemData() {
    //    debugger;
    //    __doPostBack("<= btnReload.ID%>", "");
    //}
</script>
