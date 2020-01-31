<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementVerificationDetail.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.RequirementVerificationDetail" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/CommonControls/UserControl/SharedUserRotationDetails.ascx" TagName="SharedUserRotationDetails"
    TagPrefix="uc" %>
<%@ Register Src="~/ClinicalRotation/UserControl/RequirementVerificationCategoryControl.ascx"
    TagPrefix="uc" TagName="CategoryControl" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/RequirementVerificationDetail.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/verification.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/RotationMemberSearch.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .img_status
    {
        margin-top: 1px;
        position: absolute;
        /*vertical-align: sub;*/
    }
</style>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Applicant Details</h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlApplicantDetails" runat="server">
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Name</span>
                        <infs:WclTextBox runat="server" ID="txtApplicantName" ReadOnly="true" Width="100%"
                            CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Username</span>
                        <infs:WclTextBox runat="server" ID="txtApplicantUserName" ReadOnly="true" Width="100%"
                            CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Date of Birth</span>
                        <infs:WclTextBox runat="server" ID="txtDOB" ReadOnly="true" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Email</span>
                        <infs:WclTextBox runat="server" ID="txtEmail" ReadOnly="true" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-6'>
                        <span class='cptn'>Address</span>
                        <infs:WclTextBox runat="server" ID="txtAddress" ReadOnly="true" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class='cptn' style="width: 100%; float: left;">Overall Compliance Status</span>
                        <span style="width: 100%;"><span class="not">
                            <asp:Label ID="lblComplianceStatus" runat="server" Text=""></asp:Label></span>&nbsp;
                            <asp:Image ID="imgOverallComplianceStatus" runat="server" CssClass="img_status" />
                            &nbsp;&nbsp;</span>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Phone</span>
                        <infs:WclMaskedTextBox ReadOnly="true" ID="txtPhoneNo" runat="server" Mask="(###)-###-####"
                            Width="100%" CssClass="form-control">
                        </infs:WclMaskedTextBox>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</div>
<asp:HiddenField ID="hdfNotApproved" runat="server" />
<asp:HiddenField ID="hdfIncomplete" runat="server" />

<uc:SharedUserRotationDetails ID="ucRotationDetails" runat="server"></uc:SharedUserRotationDetails>

<asp:Panel ID="pnlCategoryContainer" runat="server"></asp:Panel>
<div class="col-md-12">&nbsp;</div>
<div class="col-md-12">
    <div class="row">
        <infsu:CommandBar ID="cmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
            AutoPostbackButtons="Submit,Save,Cancel,Clear"
            SaveButtonIconClass="rbOk" SubmitButtonText="Save all and Next" OnSubmitClientClick="validateItemRejectionReason"
            OnSaveClientClick="validateItemRejectionReason" SaveButtonText="Save all and Return to Queue"
            CancelButtonText="Cancel and Return to Queue" OnSubmitClick="cmdBar_SubmitClick"
            OnSaveClick="cmdBar_SaveClick" OnCancelClick="cmdBar_CancelClick" UseAutoSkinMode="false"
            ButtonSkin="Silk">
        </infsu:CommandBar>
    </div>
</div>
<div class="col-md-12">&nbsp;</div>
