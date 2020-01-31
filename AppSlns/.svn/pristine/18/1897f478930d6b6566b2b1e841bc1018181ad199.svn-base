<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.ItemForm" CodeBehind="ItemForm.ascx.cs" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/AttributeControl.ascx" %>
<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/RowControl.ascx" %>
<%@ Register TagName="UploadDocuments" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/UploadDocuments.ascx" %>
<%--<div style="border-top: 1px solid #adadad; border-bottom: 1px solid #efefef; margin: 10px 0;">
</div>--%>
<style>
    .box__dragndrop,
    .box__uploading,
    .box__success,
    .box__error {
        display: none;
    }

    .issue-drop-zone {
        /*border: 1px dashed #ccc;*/
        border-top-color: rgb(204, 204, 204);
        border-right-color: rgb(204, 204, 204);
        border-bottom-color: rgb(204, 204, 204);
        border-left-color: rgb(204, 204, 204);
        border-radius: 0;
        padding: 7px;
        padding-left: 7px;
    }

    .issue-drop-zoneItemForm {
        border: 1px dashed #ccc;
        border-top-color: rgb(204, 204, 204);
        border-right-color: rgb(204, 204, 204);
        border-bottom-color: rgb(204, 204, 204);
        border-left-color: rgb(204, 204, 204);
        border-radius: 0;
        background-color: white;
        padding: 7px;
        transition: background-color .01s linear .01s;
        position: relative;
        margin-right: -10.6%;
    }

    .issue-drop-zone__drop-icon {
        position: relative;
    }

    .issue-drop-zone__drop-icon {
        position: relative;
    }

    .adg3 #attachmentmodule .issue-drop-zone .issue-drop-zone__text {
        width: 53%;
        font-family: -apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,Oxygen,Ubuntu,'Fira Sans','Droid Sans','Helvetica Neue',sans-serif;
        color: #172b4d;
        font-size: 14px;
        font-weight: 400;
        font-style: normal;
        line-height: 20px;
    }

    .issue-drop-zone__text {
        text-align: center;
    }

    .issue-drop-zone:not(.mod-content) {
        text-align: center;
    }

    .issue-drop-zoneItemForm {
        border: 1px dashed #ccc;
        border-top-color: rgb(204, 204, 204);
        border-right-color: rgb(204, 204, 204);
        border-bottom-color: rgb(204, 204, 204);
        border-left-color: rgb(204, 204, 204);
        border-radius: 0;
        background-color: white;
        padding: 7px;
        transition: background-color .01s linear .01s;
        position: relative;
        margin-right: -10.6%;
    }

    .issue-drop-zone__drop-icon {
        position: relative;
    }

    .issue-drop-zone button {
        color: #0052cc;
    }

    .issue-drop-zone__button {
        position: relative;
        cursor: pointer;
        color: #3572b0;
        background: transparent;
        padding: 0;
        border: 0;
        font-family: inherit;
        font-size: inherit;
    }

        .issue-drop-zone__button:hover {
            text-decoration: underline;
        }
</style>

<asp:Panel ID="pnlForm" runat="server">
    <div id="errorMessageBox" class="msgbox" runat="server">
        <asp:Label ID="lblError" runat="server" CssClass="error" Text="">
        </asp:Label>
    </div>
    <%-- <div style="margin-bottom: 10px; text-decoration: underline; padding-left: 5px;">
        Please fill the form below for
        <asp:Label runat="server" ID="lblItemName" /></div>--%>
    <div runat="server" id="dvItemPayment" visible="false">
        <div class='sxro sx2co'>
            <div class='sxlb'>
                <span class='cptn'>Amount</span>
            </div>
            <div class='sxlm'>
                <asp:Literal ID="litLabel" runat="server">0$</asp:Literal>
                <div class="vldx">
                    <span class="errmsg" id="spnItemPaymentValidation" style="display: inline;"></span>
                </div>
            </div>
            <div id="dvItemPaymentOrderNumber" style="display: none">
                <div class='sxlb'>
                    <span class='cptn'>Order Number</span>
                </div>
                <div class='sxlm'>
                    <asp:Label runat="server" ID="lblItemPaymentDisplayOrderNumber"></asp:Label>
                </div>
            </div>
            <div id="dvCreateItemPayment" class='sxlm'>
                <asp:LinkButton ID="lnkItemPayment" OnClick="lnkItemPayment_Click" ForeColor="Blue" runat="server">Click here to pay</asp:LinkButton>
            </div>
            <div id="dvComplteItemPayment" style="display: none" class='sxlm'>
                <a href='javascript:void(0)' style='color: #0000ff;' onclick="CompleteItemPaymentClick()">Complete Your Order</a>
            </div>
        </div>
        <div class='sxro sx2co'>
            <div id="dvOrderStatus" style="display: none">
                <div class='sxlb'>
                    <span class='cptn'>Payment Status</span>
                </div>
                <div class='sxlm'>
                    <asp:Label runat="server" ID="lblOrderStatus" Text="Paid"></asp:Label>
                </div>
            </div>
        </div>

    </div>

    <asp:Panel ID="pnl" runat="server">
    </asp:Panel>
    <asp:Panel ID="pnl2" runat="server">
        <div id="dvDocumentPreview" class='sxro sx2co' style="display: none" runat="server">
            <div class='sxlb' title="Click to View document Preview">
                <span class='cptn'>Preview Documents</span>
            </div>
            <div class='sxlm m2spn'>
                <asp:Panel ID="pnlDocumentPreview" runat="server">
                </asp:Panel>
            </div>
            <div class='sxroend'>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlItemDocumentUpload" runat="server" Visible="false">
        <div class='sxro sx2co'>
            <div class='sxlb' title="Click Browse to upload additional documents to be associated with this requirement">
                <span class='cptn'>Upload Additional Documents</span>
            </div>
            <div class='sxlm m2spn'>
                <infs:WclAsyncUpload runat="server" ID="fupItemData" HideFileInput="true" ClientName="fupItemDataCName"
                    Skin="Hay" MultipleFileSelection="Automatic" OnClientFileSelected="clientFileSelected" OnClientFileUploading="OnClientFileUploading"
                    OnClientFileUploaded="OnClientFileUploaded" CssClass="complioFileUpload" OnClientFileUploadRemoved="onFileRemoved"
                    OnClientValidationFailed="upl_OnClientValidationFailed" AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,TXT">
                    <Localization Select="Browse" DropZone="" />
                </infs:WclAsyncUpload>
            </div>
            <%--AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,gif,tif,tiff,docx,doc,rtf,pdf,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,TXT"
                                                        MaxFileSize="<%#MaxFileSize %>"--%>

            <%--<div class="upload-box-header">
                   Upload Additional Documents
                </div>
                <div class="upload-box">
                    <infsu:UploadDocuments ID="ucUploadDocuments" runat="server" isDropZoneEnabled="true" DropzoneID="ManageUploadDropZone" IsItemFormScreen="true"></infsu:UploadDocuments>
                </div>--%>
            <div id="<%=DropzoneID%>" class="dvApplicantDocumentDropzone sxro sx2co mod-content issue-drop-zoneItemForm drgndrop_border_class ">
                <div class="issue-drop-zone -dui-type-parsed">
                    <div class="issue-drop-zone__target"></div>
                    <span class="issue-drop-zone__text"><span class="issue-drop-zone__drop-icon"></span>Drop files to attach, or
                    <input id="btnDropZone" runat="server" type="button" class="issue-drop-zone__button" value="Browse" onclick="OpenDialog()" />
                    </span>
                </div>
            </div>


            <div class='sxroend'>
            </div>
        </div>
    </asp:Panel>
    <div id="dvNotes" runat="server" class='sxro monly'>
        <div class='sxlb' title="Use this field to enter any notes that are pertinent to this requirement">
            <span class='cptn'>Note</span>
        </div>
        <asp:Panel ID="pnlNotes" runat="server">
        </asp:Panel>
        <div class='sxroend'>
        </div>
    </div>
    <asp:Label ID="lbl" runat="server"> </asp:Label>
    <asp:HiddenField ID="hdfApplicantItemDataId" runat="server" />
    <asp:HiddenField ID="hdnIsOrderCreated" Value="0" runat="server" />
    <asp:HiddenField ID="hdfInstructionTextCategoryId" runat="server" />
</asp:Panel>

<%-- <asp:HiddenField ID="hdfUpload" runat="server" Value="1" />--%>
<script>

    function OpenDialog() {
        debugger;
        $telerik.$(".complioFileUpload .ruFileInput").click();
    }
</script>
