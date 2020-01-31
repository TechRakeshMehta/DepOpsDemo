<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ApplicantRotationRequirement.Views.SharedUserRequirementItemForm" CodeBehind="SharedUserRequirementItemForm.ascx.cs" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ApplicantRotationRequirement/UserControl/RequirementAttributeControl.ascx" %>
<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ApplicantRotationRequirement/UserControl/RequirementRowControl.ascx" %>--%>

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

    .issue-drop-zone__drop-icon {
        position: relative;
    }

    .issue-drop-zone button {
        color: #0052cc;
    }

    .issue-drop-zone__button {
        position: relative;
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
    <div class="container-fluid">

        <div id="errorMessageBox" class="msgbox col-md-12" runat="server">
            <asp:Label ID="lblError" runat="server" CssClass="error" Text="">
            </asp:Label>
        </div>
        <asp:Panel ID="pnl" runat="server">
        </asp:Panel>
        <div class="row">&nbsp;</div>
        <div class="row">
            <asp:Panel ID="pnlItemDocumentUpload" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class='form-group col-md-3' title="Click Browse to upload additional documents to be associated with this requirement">
                            <span class='cptn'>Upload Additional Documents</span>
                            <infs:WclAsyncUpload runat="server" ID="fupItemData" ClientName="fupItemDataCName" CssClass="complioFileUpload"
                                HideFileInput="true" Skin="Hay" MultipleFileSelection="Automatic" OnClientFileSelected="ReqClientFileSelected" OnClientFileUploading="OnClientFileUploadingReq"
                                OnClientFileUploaded="OnClientFileUploadedReq" OnClientFileUploadRemoved="onFileRemoved"
                                OnClientValidationFailed="upl_OnClientValidationFailedReq" AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT">
                                <Localization Select="Browse" DropZone="" />
                            </infs:WclAsyncUpload>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <div id="<%=DropzoneID%>" class="dvApplicantDocumentDropzone sxro sx2co mod-content issue-drop-zoneItemForm drgndrop_border_class ">
                            <div class="issue-drop-zone -dui-type-parsed">
                                <div class="issue-drop-zone__target"></div>
                                <span class="issue-drop-zone__text"><span class="issue-drop-zone__drop-icon"></span>Drop files to attach, or
                                    <input id="btnDropZone" runat="server" type="button" class="issue-drop-zone__button" value="Browse" onclick="OpenDialog()" />
                                </span>
                            </div>
                        </div>
                    </div>

                </div>
            </asp:Panel>
            <%--  <div class='sxro monly'>
        <div class='sxlb' title="Use this field to enter any notes that are pertinent to this requirement">
            <span class='cptn'>Note</span>
        </div>
        <asp:Panel ID="pnlNotes" runat="server">
        </asp:Panel>
        <div class='sxroend'>
        </div>
    </div>--%>
        </div>
    </div>
    <asp:Label ID="lbl" runat="server"> </asp:Label>
    <asp:HiddenField ID="hdfApplicantReqItemDataId" runat="server" />
    <asp:HiddenField ID="hdfInstructionTextCategoryId" runat="server" />
    <asp:HiddenField ID="hdnItemStatusCode" runat="server" />
</asp:Panel>
<div runat="server" id="dvItemPayment">
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
            <asp:LinkButton ID="lnkItemPayment"   OnClick="lnkItemPayment_Click" ForeColor="Blue" runat="server">Click here to pay</asp:LinkButton>
        </div>
        <div id="dvComplteItemPayment" style="display: none" class='sxlm'>
            <a href='javascript:void(0)' style='color: #0000ff;' onclick="CompleteItemPaymentClick(true)">Complete Your Order</a>
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
            <div class='sxroend'>
            </div>
        </div>
    </div>
</div>
   