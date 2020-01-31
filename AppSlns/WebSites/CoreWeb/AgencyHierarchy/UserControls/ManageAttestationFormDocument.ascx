<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageAttestationFormDocument.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.ManageAttestationFormDocument" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rmpHierarchyControls">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~//Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceDataEntry/Scripts/UploadDocuments.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/upload.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>

<div class="container-fluid" tabindex="-1" id="dvAttestationFormDocuments" runat="server">
    <div class="col-md-12">
        <div class="row">
            <h2 class="header-color">Specific Attestation Form</h2>
        </div>
    </div>

    <div class="col-md-12">
        <div class="row">
            <div class="form-group col-md-3" >
                <span class="cptn">Agency Attestation Form Setting </span>
                <asp:RadioButtonList ID="rbtnAttestationFormSetting" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                    <asp:ListItem Text="On" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="Off" Value="N"></asp:ListItem>
                    <asp:ListItem Text="Default" Value="D" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </div>

    <div id="dvAttestationUploadDocuments" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="upload-box-header">
                    <h1>Upload Document
                    </h1>
                    Click browse button to select files.
                </div>
                <div class="upload-box">
                    <infs:WclAsyncUpload runat="server" ID="uploadAttestationFormControl" HideFileInput="true" Skin="Hay" 
                        MultipleFileSelection="Disabled" OnClientFileSelected="clientFileSelected" OnClientFileUploadRemoved="onFileRemoved"
                         OnClientFileUploading="OnClientFileUploading"
                        OnClientFileUploaded="OnAttestationFormUploaded" Width="100%" MaxFileInputsCount="1"
                        AllowedFileExtensions="pdf"
                        OnClientValidationFailed="upl_OnClientValidationFailed" ToolTip="Click here to select files to upload from your computer">
                        <Localization Select="Browse" />
                    </infs:WclAsyncUpload>
                    <div class='vldx'> 
                        <asp:CustomValidator ID="cvUploadControl" runat="server" ErrorMessage="Attestation Form required." CssClass="errmsg" Display="Dynamic"
                            ClientValidationFunction="CheckforFileCount" ClientIDMode="Static" ValidationGroup="AttestationSettingForm"></asp:CustomValidator>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="dvAttestationFormUploadedDocuments" runat="server" style="margin-bottom: 30px;"> 
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class="cptn">Document Name</span>

                    </div>
                    <div class='form-group col-md-2'>
                        <span class="cptn">Document Size (KB)</span>

                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Actions</span>
                    </div>
                </div>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <asp:Label runat="server" ID="lblAttestationDocumentName"></asp:Label>
                    </div>
                    <div class='form-group col-md-2'>
                        <asp:Label runat="server" ID="lblAttestationDocumentSize"></asp:Label>
                    </div>
                    <div class='form-group col-md-3'>
                        <asp:LinkButton runat="server" ID="lnkViewAttestationDocument" Text="View" OnClick="lnkViewDocument_Click"></asp:LinkButton>
                        <span style="padding-left: 3px; padding-right: 3px;">|</span>
                        <asp:LinkButton runat="server" ID="lnkAttestationDelete" Text="Delete" OnClientClick="return DeleteAttestationConfirmation();"></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkDeleteAttestationDocument" Style="display: none;" Text="Delete" OnClick="lnkDeleteDocument_Click"></asp:LinkButton>
                        <iframe runat="server" id="iFrameViewDoc" style="display: none;"></iframe>
                    </div>
                </div>
            </div>
       
    </div> 
        <div class="col-md-12">
            <div class="row text-right">
                 <infsu:CommandBar ID="btnSave1" runat="server"  DisplayButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk"
                    AutoPostbackButtons="Submit" OnSubmitClick="btnUploadAll_Click" SubmitButtonIconClass="rbSave" SubmitButtonText="Save" ValidationGroup="AttestationSettingForm">
                </infsu:CommandBar>
        </div>
     </div>
</div>
<asp:HiddenField ID="hdfIgnoreAlreadyUploadedDoc" runat="server" Value="true" />

<script type="text/javascript">
    

    function DeleteAttestationConfirmation() {
        var selectedvalue = $jQuery("[id$=rbtnAttestationFormSetting] [type='radio']:checked").val();
        var message = "";
        if (selectedvalue == "Y") {
            message = "Are you sure, you want to delete this document? Upon deleting this form, Attestation setting of this node will change to Off."
        }
        else {
           message = "Are you sure, you want to delete this document?"
        }
       
        $confirm(message, function (res) {
            if (res) {
                __doPostBack("<%= lnkDeleteAttestationDocument.UniqueID %>", "");
                return true;
            }
            else {
                return false;
            }
        }, 'Complio', true);

        return false;
    }


    function CheckforFileCount(sender, args) {
        args.IsValid = false; 
        var selectedvalue = $jQuery("[id$=rbtnAttestationFormSetting] [type='radio']:checked").val()
        if (selectedvalue == "Y") {
            var uploadcontrol = $find($jQuery("[id$=uploadAttestationFormControl]")[0].id);
            var uploadedFilesCount = $jQuery(uploadcontrol._uploadedFiles).toArray().length;
            if (uploadedFilesCount > 0) {
                args.IsValid = true;
            }
        }
        else {
            args.IsValid = true;
        }
    }

</script>
