<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.UploadDocuments" CodeBehind="UploadDocuments.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>

<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceDataEntry/Scripts/UploadDocuments.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .Category label > input {
        display: none;
    }

    .Category label {
        color: black;
        background-color: white;
        padding-left: 10px;
    }

    /*.Items > label {
        padding: 5px 0px 0px 40px !important;
    }*/
    /*.rcbDisabled {
        padding: 5px 0px 0px 40px !important;
    }*/

    #cmbItems_DropDown li:not(.rcbSeparator) {
        padding: 5px 0px 0px 40px !important;
    }

    .mod-content {
        margin-left: 0;
        padding-left: 0;
        margin-top: 5px;
    }

    .issue-drop-zone {
        border-color: #b3bac5;
        border-radius: 3px;
        border-radius: 3px;
        padding: 7px;
        transition: background-color .01s linear .01s;
        position: relative;
        margin-right: 10%;
    }

    .mod-content.issue-drop-zone .issue-drop-zone {
        border: none;
    }

    .issue-drop-zone:not(.mod-content) {
        text-align: center;
    }

    .issue-drop-zone__target {
        position: absolute;
        left: 0;
        top: 0;
        right: 0;
        bottom: 0;
    }

    .issue-drop-zone .issue-drop-zone__text {
        font-family: -apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,Oxygen,Ubuntu,'Fira Sans','Droid Sans','Helvetica Neue',sans-serif;
        color: #172b4d;
        font-size: 14px;
        font-weight: 400;
        font-style: normal;
        line-height: 20px;
    }

    .issue-drop-zone__text {
        text-align: center;
        display: block;
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

    #ManageUploadDropZone {
        margin-top: 0px !important;
        padding-top: 0px !important;
        padding-right: 0px !important;
        margin-right: 0px !important;
    }

    .section {
        margin-bottom: 0px !important;
        padding-bottom: 0px !important;
        padding-left: 2px !important;
        padding-right: 0px !important;
    }

    .chkFileName {
        display: none !important;
    }
</style>
<div class="section">
    <div class="content">
        <div class="">
            <div class='sxro sx3co'>
                <div id="dropdown" class='sxlm' style="display: normal">
                    <infs:WclComboBox ID="cmbItems" runat="server" CheckBoxes="true" AutoSkinMode="true" ClientIDMode="Static" OnClientDropDownClosed="OnDropdownClosed"
                        OnItemDataBound="cmbItems_ItemDataBound" Filter="None" Width="50%" DataValueField="CategoryItemsID" DataTextField="ItemsName" CssClass="form-control"
                        OnClientItemChecked="OnClientItemChecked" OnClientKeyPressing="openCmbBoxOnTab">
                    </infs:WclComboBox>
                </div>
                <div class='sxlb' id="hdrUploadDocument" runat="server">
                    <span class="cptn">Upload Document(s)</span>
                </div>

                <div class='sxlm m2spn' style="max-width: 53% !important; width: auto !important">
                    <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay" OnClientFileUploaded="onClientFileUploaded"
                        MultipleFileSelection="Automatic" OnClientFileSelected="clientFileSelected" OnClientFileUploadRemoved="onFileRemoved" CssClass="complioFileUpload"
                        OnClientFileUploading="OnClientFileUploading" Width="100%"
                        AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,txt,efts,nist,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,TXT,EFTS,NIST"
                        OnClientValidationFailed="upl_OnClientValidationFailed" ToolTip="Click here to select files to upload from your computer">
                        <Localization Select="Browse" DropZone="" />
                    </infs:WclAsyncUpload>

                </div>
                <div class='sxroend'>
                </div>
            </div>
        </div>
    </div>
</div>

<asp:HiddenField ID="hdnIsValidPCNFile" runat="server" value="0"/>

<%if (this.DropzoneID != null && this.DropzoneID != String.Empty)
    {%>
<div id="<%=DropzoneID%>" class="<%=DropzoneCSS%>">
    <div class="issue-drop-zone -dui-type-parsed">
        <div class="issue-drop-zone__target"></div>
        <span class="issue-drop-zone__text"><span class="issue-drop-zone__drop-icon"></span>Drop files to attach, or
             <input type="button" class="issue-drop-zone__button" value="Browse" onclick="OpenDialog()" />
        </span>
    </div>
</div>
<%} %>

<asp:HiddenField ID="hdfOrganizationUserId" runat="server" />
<asp:HiddenField ID="hdnDocumentAssociationSettingEnabled" runat="server" />
<infs:WclButton ID="btnUploadAll" runat="server" Text="Upload All" OnClick="btnUploadAll_Click" />
<infs:WclButton ID="btnUploadCancel" runat="server" Text="Cancel" OnClick="btnUploadCancel_Click" />
<%--<asp:HiddenField ID="hdfSelectedTenantId" runat="server" />--%>

<script>

    //$jQuery('[id$=btnChkFileName]').attr('style', 'display:none;');

    function OpenDialog() {
        $telerik.$(".complioFileUpload .ruFileInput").click();
    }

</script>
