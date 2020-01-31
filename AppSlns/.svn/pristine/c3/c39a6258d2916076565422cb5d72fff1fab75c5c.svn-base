<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.BkgSetup.Views.UploadDisclosureDocuments" CodeBehind="UploadDisclosureDocuments.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<script type="text/javascript">
</script>
<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceDataEntry/Scripts/UploadDocuments.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="section">
    <div class="content">
        <div class="sxform auto">
            <div class='sxro sx3co'>
                <div class='sxlb'>
                    <span class="cptn">Upload D&A Document(s)</span>
                </div>

                <div class='sxlm'>
                    <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                        MultipleFileSelection="Automatic" OnClientFileSelected="onClientFileSelectedDandADocument" OnClientFileUploaded="onClientFileUploaded" OnClientFileUploadRemoved="onFileRemoved" OnClientFileUploading="OnClientFileUploading"
                        AllowedFileExtensions="pdf,PDF"
                        OnClientValidationFailed="upl_OnClientValidationFailed" ToolTip="Click here to select files to upload from your computer">
                        <Localization Select="Browse" />
                    </infs:WclAsyncUpload>
                </div>
                <div class='sxroend'>
                </div>
            </div>
        </div>
    </div>
</div>
<asp:HiddenField ID="hdfIgnoreAlreadyUploadedDoc" Value="true" runat="server" />
<infs:WclButton ID="btnUploadAll" runat="server" Text="Upload All" OnClick="btnUploadAll_Click" />
<infs:WclButton ID="btnUploadCancel" runat="server" Text="Cancel" OnClick="btnUploadCancel_Click" />

