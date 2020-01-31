<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageRequirementApprovalNotificationDocument.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.UserControls.ManageRequirementApprovalNotificationDocument" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rmpHierarchyControls">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~//Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <%--<infs:LinkedResource Path="~//Resources/Mod/Dashboard/Scripts/bootstrap.min.js" ResourceType="JavaScript" />--%>
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceDataEntry/Scripts/UploadDocuments.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/upload.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<div class="container-fluid" tabindex="-1" id="dvRequirementApprovalNotificationDocument" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Requirement Approval Notification Document</h2>
        </div>
    </div>
    <div id="dvUploadDocument" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="upload-box-header">
                    <h1>Upload Document
                    </h1>
                    Click browse button to select files.
                </div>
                <div class="upload-box">
                    <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay" OnClientFileUploaded="onClientFileUploaded"
                        MultipleFileSelection="Disabled" OnClientFileSelected="clientFileSelected" OnClientFileUploadRemoved="onFileRemoved"
                        OnClientFileUploading="OnClientFileUploading" Width="100%" MaxFileInputsCount="1"
                        AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT"
                        OnClientValidationFailed="upl_OnClientValidationFailed" ToolTip="Click here to select files to upload from your computer">
                        <Localization Select="Browse" />
                    </infs:WclAsyncUpload>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <infs:WclButton ID="btnUploadAll" CssClass="rbSave" ClientIDMode="Static" runat="server" Text="Upload" AutoSkinMode="false" Skin="Silk" OnClick="btnUploadAll_Click" />
                <infs:WclButton ID="btnUploadCancel" runat="server" CssClass="rbCancel" AutoSkinMode="false" Skin="Silk" Text="Cancel" OnClick="btnUploadCancel_Click" />

                <%-- <infsu:CommandBar ID="fsucCmdBar" runat="server" ButtonPosition="Right" DisplayButtons="Submit, Cancel" UseAutoSkinMode="false" ButtonSkin="Silk"
                AutoPostbackButtons="Submit, Cancel" OnSubmitClick="btnUploadAll_Click" SubmitButtonIconClass="rbUpload" SubmitButtonText="Save" ValidationGroup="grpFormSubmit"
                CancelButtonIconClass="rbCancel" OnCancelClientClick="btnUploadCancel_Click" CancelButtonText="Cancel">
            </infsu:CommandBar>--%>
            </div>
        </div>
    </div>
    <div id="dvUploadedDocuments" runat="server" style="margin-bottom: 30px;">
        <div class="row">
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class="cptn">Document Name</span>

                    </div>
                    <div class='form-group col-md-2'>
                        <span class="cptn">Document Size (KB)</span>

                    </div>
                    <div class='form-group col-md-4'>
                        <span class="cptn">Description</span>

                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Actions</span>
                    </div>
                </div>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <asp:Label runat="server" ID="lblDocumentName"></asp:Label>
                    </div>
                    <div class='form-group col-md-2'>
                        <asp:Label runat="server" ID="lblDocumentSize"></asp:Label>
                    </div>
                    <div class='form-group col-md-4'>
                        <asp:Label runat="server" Style="word-wrap: break-word;" ID="lblDocumentDesc"></asp:Label>
                    </div>
                    <div class='form-group col-md-3'>
                        <asp:LinkButton runat="server" ID="lnkViewDocument" Text="View" OnClick="lnkViewDocument_Click"></asp:LinkButton>
                        <span style="padding-left: 3px; padding-right: 3px;">|</span>
                        <asp:LinkButton runat="server" ID="lnkDelete" Text="Delete" OnClientClick="return DeleteConfirmation();"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkDeleteDocument" Style="display: none;" Text="Delete" OnClick="lnkDeleteDocument_Click"></asp:LinkButton>
                        <iframe runat="server" id="iFrameViewDoc" style="display: none;"></iframe>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<asp:HiddenField ID="hdfIgnoreAlreadyUploadedDoc" runat="server" Value="true" />

<script type="text/javascript">
    function DeleteConfirmation() {
        var message = "Are you sure, you want to delete document?"
        $confirm(message, function (res) {
            if (res) {
                __doPostBack("<%= lnkDeleteDocument.UniqueID %>", "");
                return true;
            }
            else {
                return false;
            }
        }, 'Complio', true);

        return false;
    }

</script>
