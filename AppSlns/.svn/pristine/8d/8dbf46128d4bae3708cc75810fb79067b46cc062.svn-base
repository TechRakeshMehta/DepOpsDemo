<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyUserProfile.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.AgencyUserProfile" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxAgencyUserProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid">

    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblTitle" Text="" runat="server" />
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAgencyUserProfile">

            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="First Name of this Agency user">
                        <span class="cptn">First Name</span><span class="reqd">*</span>
                        <infs:WclTextBox runat="server" ID="txtFirstName" MaxLength="256" Enabled="true" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                                Display="Dynamic" CssClass="errmsg" Text="First name is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Middle Name of this Agency user">
                        <span class="cptn">Middle Name</span>
                        <infs:WclTextBox runat="server" ID="txtMiddleName" MaxLength="50" Enabled="true" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Last Name of this Agency user">
                        <span class="cptn">Last Name</span><span class="reqd">*</span>
                        <infs:WclTextBox runat="server" ID="txtLastName" MaxLength="50" Enabled="true" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                Display="Dynamic" CssClass="errmsg" Text="Last name is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Email Address of this Agency user">
                        <span class="cptn">Email Address</span>
                        <infs:WclTextBox runat="server" ID="txtEmail" MaxLength="50" Enabled="false" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div runat="server" id="dvPermission" visible="false">
                        <div class='form-group col-md-3' title="Immunization/Compliance Information of this Agency user">
                            <asp:Label ID="lblImmunizationCompliancePermission" runat="server" Text="Immunization/Compliance Information"
                                CssClass="cptn"></asp:Label><span class="reqd">*</span>
                            <infs:WclComboBox ID="cmbCompliancePermissions" runat="server" DataValueField="SharedInfoTypeID"
                                DataTextField="SharedInfoType" EmptyMessage="--Select--" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvImmunizationCompliancePermission" ControlToValidate="cmbCompliancePermissions"
                                    Display="Dynamic" ValidationGroup="grpValdManageAgencyUsers" CssClass="errmsg" Text="Immunization/Compliance Information is required." Enabled="true" />
                            </div>
                        </div>
                        <div class='form-group col-md-3' title="Background Screening Information of this Agency user">
                            <asp:Label ID="lblBackgroundPermissions" runat="server" Text="Background Screening Information" CssClass="cptn"></asp:Label>
                            <infs:WclComboBox ID="cmbBackgroundPermissions" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="false"
                                DataValueField="SharedInfoTypeID" DataTextField="SharedInfoType" EmptyMessage="--Select--" Filter="None" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                        </div>
                    </div>
                    <div runat="server" id="dvRotationPermission" visible="false">
                        <div class='form-group col-md-3' title="Agency Orientation Information of this Agency user">
                            <asp:Label ID="lblRotationPermission" runat="server" Text="Agency Orientation Information" CssClass="cptn">
                            </asp:Label><span class="reqd">*</span>
                            <infs:WclComboBox ID="cmbRotationPermissions" runat="server" DataValueField="SharedInfoTypeID" DataTextField="SharedInfoType"
                                EmptyMessage="--Select--" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvRotationPermission" ControlToValidate="cmbRotationPermissions"
                                    Display="Dynamic" ValidationGroup="grpValdManageAgencyUsers" CssClass="errmsg" Text="Agency Orientation Information is required." Enabled="true" />
                            </div>
                        </div>                                                
                    </div>
                    <div runat="server">
                        <div class='form-group col-md-3' title="Change Password">
                            <div class='sxlb'>
                                <span class='cptn'>Change Password</span>
                            </div>
                            <div class='sxlm'>
                                <asp:HyperLink ID="lnkChangePassword" runat="server" CssClass="user" Text="Change Password"> </asp:HyperLink>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <infsu:CommandBar ID="fsucCmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Save,Cancel,Clear" DefaultPanel="pnlAgencyUserProfile"
        AutoPostbackButtons="Save,Cancel,Clear" CancelButtonText="Cancel" CancelButtonIconClass="rbCancel" OnCancelClick="fsucCmdBar_CancelClick" ValidationGroup="grpFormSubmit"
        SaveButtonText="Update Profile" OnSaveClick="fsucCmdBar_SaveClick" UseAutoSkinMode="false" ButtonSkin="Silk"
        ClearButtonText="Link Account" OnClearClick="fsucCmdBar_ClearClick">
    </infsu:CommandBar>
</div>



