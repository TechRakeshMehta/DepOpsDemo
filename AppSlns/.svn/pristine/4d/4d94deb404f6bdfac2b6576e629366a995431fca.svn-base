<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminCreateOrderDetails.ascx.cs"
    Inherits="CoreWeb.BkgOperations.Views.AdminCreateOrderDetails" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/NewLocationInfo.ascx" TagPrefix="infsu"
    TagName="Location" %>
<%@ Register TagPrefix="uc" TagName="PersonAlias" Src="~/Shared/Controls/NewPersonAliasInfo.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/NewCustomAttributeLoaderSearch.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="uc" %>
<%@ Register TagPrefix="uc" TagName="PrevResident" Src="~/ComplianceOperations/UserControl/NewPreviousAddressControl.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxBkgPackagesOrdered">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/BkgOperations/BkgOperations.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/BkgOperations/BkgOperationForSupplemental.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    #dvPrimaryPhone span.RadInput {
        width: 85% !important;
    }

    .chkIsMaskingRequired input[type="checkbox"] {
        margin-top: 28px;
        position: absolute;
        right: 0;
        top: 2px;
    }

    #dvPrimaryPhone .vldx {
        margin-left: -180px !important;
    }

    #dvSecondaryPhone span.RadInput {
        width: 85% !important;
    }

    div#dvPrimaryPhone, div#dvSecondaryPhone {
        position: relative;
    }
</style>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblOrderDetail" CssClass="page-heading" runat="server" Text=""></asp:Label>
            </h2>
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlApplicantDetails">
        <div class="row bgLightGreen">
            <div class='col-md-12'>
                <div class="row">
                    <div class="col-md-12">
                        <h2 class="header-color">Applicant Details</h2>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-3">
                        <infs:WclCheckBox runat="server" ID="chkMiddleNameRequired" onclick="MiddleNameEnableDisable(this)"></infs:WclCheckBox>
                        <asp:Label ID="lblChkMiddleName" runat="server">I don't have a Middle Name.</asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <label class="cptn">First Name</label><span class="reqd">*</span>
                        <infs:WclTextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvFirstName" ControlToValidate="txtFirstName"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="First Name is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <label class="cptn">Middle Name</label><span class="reqd" id="spnMiddleName" runat="server">*</span>
                        <infs:WclTextBox ID="txtMiddleName" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvMiddleName" ControlToValidate="txtMiddleName"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Middle Name is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <label class="cptn">Last Name</label><span class="reqd">*</span>
                        <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Last Name is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                </div>
                <div style="display: none" id="dvPersonalAlias" runat="server">
                    <uc:PersonAlias ID="ucPersonAlias" runat="server" Visible="true" IsEditProfile="true"
                        IsLabelMode="true"></uc:PersonAlias>
                </div>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <label class="cptn">Gender</label><span class="reqd">*</span>
                        <infs:WclComboBox ID="cmbGender" runat="server" DataTextField="GenderName" Skin="Silk" OnClientKeyPressing="openCmbBoxOnTab"
                            AutoSkinMode="false" Width="100%" CssClass="form-control" DataValueField="GenderID" EmptyMessage="--Select--">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvGender" ControlToValidate="cmbGender"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Gender is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <label class="cptn">Date of Birth</label><span class="reqd">*</span>
                        <infs:WclDatePicker ID="dpkrDOB" Width="100%" EnableAriaSupport="true" runat="server"
                            DateInput-EmptyMessage="Select a date" DateInput-EnableAriaSupport="true">
                            <Calendar EnableKeyboardNavigation="true" EnableAriaSupport="true"></Calendar>
                        </infs:WclDatePicker>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvDOB" ControlToValidate="dpkrDOB"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Date of birth is required." ValidationGroup="grpFormSubmit" />
                            <asp:RangeValidator ID="rngvDOB" runat="server" ControlToValidate="dpkrDOB" Type="Date"
                                ValidationGroup="grpFormSubmit" Display="Dynamic" CssClass="errmsg" Text="Date of birth should not be less than a year.">
                            </asp:RangeValidator>
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <label class="cptn">Email</label><span class="reqd">*</span>
                        <infs:WclTextBox ID="txtEmail" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="txtEmail"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Email is required." ValidationGroup="grpFormSubmit" />
                            <asp:RegularExpressionValidator runat="server" ID="revEmailAddress" ControlToValidate="txtEmail"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is not valid." ValidationGroup="grpFormSubmit"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <label class="cptn">Phone Number</label><span class="reqd">*</span>
                        <%--<infs:WclMaskedTextBox ID="txtPhoneNumber" runat="server" Width="95%" CssClass="form-control" EnableAriaSupport="true"
                            Mask="(###)-###-####">
                        </infs:WclMaskedTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvPhoneNumber" ControlToValidate="txtPhoneNumber"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Phone number is required." ValidationGroup="grpFormSubmit" />
                        </div>--%>
                        <div id="dvMaskedPrimaryPhone" runat="server">
                            <infs:WclMaskedTextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" Width="95%" Mask="(###)-###-####">
                            </infs:WclMaskedTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobile" runat="server" CssClass="errmsg"
                                    ErrorMessage="Phone is required." ControlToValidate="txtPhoneNumber" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobile" runat="server" ValidationGroup="grpFormSubmit"
                                    CssClass="errmsg" ErrorMessage="Format is (###)-###-####" ControlToValidate="txtPhoneNumber"
                                    ValidationExpression="\(\d{3}\)-\d{3}-\d{4}" />
                            </div>
                        </div>
                        <div id="dvUnmaskedPrimaryPhone" runat="server" style="display: none;">
                            <infs:WclTextBox ID="txtUnmaskedPrimaryPhone" CssClass="form-control" Width="95%" runat="server" MaxLength="15"></infs:WclTextBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator Display="Dynamic" ID="rfvTxtMobileUnmasked" runat="server" CssClass="errmsg"
                                    ErrorMessage="Phone is required." ControlToValidate="txtUnmaskedPrimaryPhone" ValidationGroup="grpFormSubmit"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobilePrmyNonMasking" runat="server" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    ErrorMessage="Invalid phone number. This field only contains +, - and numbers."
                                    ControlToValidate="txtUnmaskedPrimaryPhone" ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                            </div>
                        </div>
                        <infs:WclCheckBox runat="server" CssClass="chkIsMaskingRequired" ID="chkPrimaryPhone" ToolTip="Check this box if you do not have a US number." onclick="MaskedUnmaskedPhone(this)"></infs:WclCheckBox>
                    </div>
                </div>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <label class="cptn">SSN</label><span class="reqd">*</span>
                        <div id="dvSSNMask">
                            <div class='form-group col-md-10'>
                                <div class="row">
                                    <infs:WclMaskedTextBox ID="txtSSN" runat="server" Width="95%" CssClass="form-control" EnableAriaSupport="true"
                                        Mask="###-##-####">
                                    </infs:WclMaskedTextBox>
                                </div>
                            </div>
                            <div class='form-group col-md-2'>
                                <div class='row'>
                                    <infs:WclButton runat="server" ID="chkAutoFillSSN" OnClientCheckedChanged="AutoFillSSN" ToolTip="Check this box if you do not have an SSN" ToggleType="CheckBox" ButtonType="ToggleButton" AutoPostBack="false" Visible="true">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Value="True" />
                                            <telerik:RadButtonToggleState Value="False" />
                                        </ToggleStates>
                                    </infs:WclButton>
                                </div>
                            </div>
                        </div>
                        <div class="vldx row form-group col-md-10">
                            <asp:RequiredFieldValidator runat="server" ID="rfvSSN" ControlToValidate="txtSSN"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="SSN is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class="form-group col-md-3">
                        <label class="cptn">Address 1</label><span class="reqd">*</span>
                        <infs:WclTextBox runat="server" ID="txtAddress1" MaxLength="256" CssClass="form-control" Width="100%" EnableAriaSupport="true" >
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvAddress1" ControlToValidate="txtAddress1"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Address1 is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class="form-group col-md-3">
                        <label class="cptn">Address 2</label>
                        <infs:WclTextBox runat="server" ID="txtAddress2" MaxLength="256" CssClass="form-control" Width="100%" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                </div>
                <div class='row bgLightGreen'>
                    <infsu:Location ID="locationTenant" ZipTabIndex="6" CityTabIndex="7" runat="server"
                        IsReverselookupControl="true"
                        NumberOfColumn="Four" ValidationGroup="grpFormSubmit" ControlsExtensionId="AEP" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <div>&nbsp;</div>
    <div>&nbsp;</div>

    <div runat="server" id="dvResHistory" style="display: none">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Residential History</h2>
            </div>
        </div>
        <uc:PrevResident ID="PrevResident" runat="server" class="PrevResident" IsEditProfile="true" />
    </div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <asp:Panel ID="pnlterminalNodeSelection" runat="server">
        <div class="row bgLightGreen">
            <div class='col-md-12'>
                <div class="row">
                    <div class="col-md-12">
                        <h2 class="header-color">Terminal Node</h2>
                    </div>
                </div>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <label class="cptn">Institution Hierarchy</label>
                        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                        <div class="vldx">
                        </div>
                    </div>
                    <%--<uc:CustomAttributeLoaderSearch ID="ucCustomAttributeLoaderSearch" runat="server"
                                IsCustomAttributesHide="true" />--%>
                    <div class='form-group col-md-3'>
                        <label class="cptn">Package</label>
                        <infs:WclComboBox ID="cmbBkgPackages" runat="server" DataTextField="BPAName" DataValueField="BPHMId" Skin="Silk" EnableCheckAllItemsCheckBox="true"
                            OnClientKeyPressing="openCmbBoxOnTab" OnTextChanged="cmbBkgPackages_TextChanged" CheckBoxes="true" Filter="Contains" OnClientDropDownClosed="MVRPkgIsChecked"
                            AutoSkinMode="false" Width="100%" CssClass="form-control" EmptyMessage="--Select--" AutoPostBack="false">
                        </infs:WclComboBox>
                        <div class="vldx">
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <label class="cptn">Attest to FCRA Previsions</label>
                        <asp:CheckBox ID="chkAttestToFCRA" runat="server" Checked="false" AutoPostBack="false" />
                        <div class="vldx">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <asp:Panel ID="pnlMvrInfo" runat="server" Visible="false">
        <div class="row bgLightGreen ">
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-12">
                        <h2 class="header-color">Motor Vehicle Record</h2>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-3">
                        <label class="cptn">Do you have a valid Driver's License?</label><span class="reqd">*</span>
                        <infs:WclButton runat="server" ID="chkIsMVRInfo" ToggleType="CheckBox" ButtonType="ToggleButton" Checked="true"
                            AutoPostBack="false" OnClientCheckedChanged="EnableDisableLicenseValidators" CausesValidation="false">
                            <ToggleStates>
                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                <telerik:RadButtonToggleState Text="No" Value="False" />
                            </ToggleStates>
                        </infs:WclButton>
                    </div>
                    <div class="form-group col-md-3" id="dvDriverLicenseState" runat="server" visible="false">
                        <label class="cptn">Driver License State</label><span id="spnLicenseState" class="reqd" runat="server">*</span>
                        <infs:WclComboBox ID="cmbState" runat="server" Skin="Silk" AutoSkinMode="false" CheckBoxes="false" Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" CssClass="form-control"
                            DataTextField="StateName" DataValueField="StateID" EmptyMessage="--Select--" MarkFirstMatch="true" OnClientBlur="onMVRStateBlur" Enabled="true">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvLicenseState" CssClass="errmsg" ControlToValidate="cmbState"
                                Display="Dynamic" ErrorMessage="License State is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                    <div class="form-group col-md-3" id="dvDriverLicenseNo" runat="server" visible="false">
                        <label class="cptn">Driver License Number</label><span id="spnLicenseNumber" class="reqd" runat="server">*</span>
                        <infs:WclTextBox ID="txtLicenseNO" runat="server" MaxLength="256" CssClass="form-control" Enabled="true">
                        </infs:WclTextBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvtxtLicenseNO" CssClass="errmsg" ControlToValidate="txtLicenseNO"
                                Display="Dynamic" ErrorMessage="License Number is required." ValidationGroup="grpFormSubmit" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div>&nbsp;</div>
    <div>&nbsp;</div>
    <asp:Panel ID="pnlUploadDocument" runat="server" Visible="false">
        <div class="row bgLightGreen">
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-12">
                        <h2 class="header-color">Upload Documents</h2>
                    </div>
                </div>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <label class="cptn">Upload D&A Document</label>
                        <infs:WclAsyncUpload runat="server" ID="uploadDandA" Style="float: left; display: block;" HideFileInput="true" Skin="Hay" ClientIDMode="Static" Width="100%"
                            AllowedFileExtensions="pdf"
                            ToolTip="Click here to select files to upload from your computer" MultipleFileSelection="Automatic">
                            <Localization Select="Browse" />
                        </infs:WclAsyncUpload>
                    </div>
                </div>
                <div class="row" style="text-align: center">
                    <infs:WclButton ID="btnUploadDandA" runat="server" Text="Upload" Icon-PrimaryIconCssClass="rbUpload" AutoSkinMode="false" Skin="Silk" OnClick="btnUploadDandA_Click"></infs:WclButton>
                </div>
                <div>&nbsp;</div>
                <div class="row" style="display: none">
                    <div class='form-group col-md-3'>
                        <label class="cptn">Upload Additional Document(s)</label>
                        <infs:WclAsyncUpload runat="server" ID="uploadAdditional" Style="float: left; display: block;" HideFileInput="true" Skin="Hay" ClientIDMode="Static" Width="100%"
                            AllowedFileExtensions="pdf"
                            ToolTip="Click here to select files to upload from your computer" MultipleFileSelection="Automatic" Visible="false">
                            <Localization Select="Browse" />
                        </infs:WclAsyncUpload>
                    </div>
                </div>
                <div class="row" style="text-align: center; display: none">
                    <infs:WclButton ID="btnUploadAdditional" runat="server" Text="Upload" Icon-PrimaryIconCssClass="rbUpload" AutoSkinMode="false" Skin="Silk" OnClick="btnUploadAdditional_Click" Visible="false"></infs:WclButton>
                </div>
                <div>&nbsp;</div>
            </div>
        </div>
    </asp:Panel>

    <div class="row">
        <div class="col-md-12">
            <infs:WclGrid runat="server" ID="grdApplicantDoc" AllowPaging="True" AutoGenerateColumns="false"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="true" CellSpacing="0"
                EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="False"
                OnNeedDataSource="grdApplicantDoc_NeedDataSource" Visible="false">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView EditMode="InPlace" DataKeyNames="ApplicantDocumentID">
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <Columns>
                        <%-- <telerik:GridTemplateColumn UniqueName="AssignItems" ItemStyle-Width="2%" HeaderTooltip="Click this box to select all Document on the active page"
                            AllowFiltering="false" ShowFilterIcon="false" Visible="true">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectItem" runat="server"
                                    onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectItem_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridBoundColumn DataField="ApplicantDocumentID" FilterControlAltText="Filter Applicant Document ID column"
                            HeaderText="ID" SortExpression="ApplicantDocumentID" UniqueName="ApplicantDocumentID"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="FileName" ItemStyle-Width="15%" FilterControlAltText="Filter FileName column"
                            HeaderText="File Name" SortExpression="FileName" UniqueName="FileName" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DocumentTypeName" ItemStyle-Width="15%" FilterControlAltText="Filter DocumentTypeName column"
                            HeaderText="File Type" SortExpression="DocumentTypeName" UniqueName="DocumentTypeName" ReadOnly="true">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </div>
    <div>&nbsp;</div>
    <div>&nbsp;</div>

    <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel,Extra"
        AutoPostbackButtons="Submit,Save,Extra" ExtraButtonIconClass="rbCancel" ValidationGroup="grpFormSubmit"
        SubmitButtonText="Save" SaveButtonText="Transmit" SubmitButtonIconClass="rbSave" OnSubmitClick="fsucCmdBarButton_SubmitClick" OnSaveClick="fsucCmdBarButton_SaveClick"
        CancelButtonText="Delete" ExtraButtonText="Cancel" UseAutoSkinMode="false" ButtonSkin="Silk" OnExtraClick="fsucCmdBarButton_ExtraClick" OnCancelClick="fsucCmdBarButton_DeleteClick" OnCancelClientClick="getConfirmation">
        <ExtraCommandButtons>
            <infs:WclButton ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" AutoSkinMode="false" Skin="Silk" ValidationGroup="grpFormSubmit"></infs:WclButton>
        </ExtraCommandButtons>
    </infsu:CommandBar>

    <div class='row bgLightGreen'>
        <asp:Panel ID="pnlCustomFormLoad" Visible="false" runat="server">
            <iframe id="iframe" runat="server" width="100%" height="500px;"></iframe>
        </asp:Panel>
        <asp:Panel ID="pnlCustomFormReadMode" Visible="false" runat="server">
            <iframe id="iframeCustomFormReadMode" runat="server" width="100%" height="500px;"></iframe>
        </asp:Panel>
    </div>
</div>
<asp:Button ID="btnRefeshPage" Style="display: none" runat="server" OnClick="btnRefeshPage_Click"></asp:Button>
<asp:Button ID="btnBindPackage" runat="server" OnClick="btnBindPackage_Click" />
<asp:Button ID="btnCheckIsMvrPkgChecked" runat="server" OnClick="btnCheckIsMvrPkgChecked_Click" />
<asp:HiddenField ID="hdnTenantId" runat="server" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" />
<asp:HiddenField ID="hdnIsCustomDataSaved" runat="server" Value="false" />
<asp:HiddenField ID="hdnNoMiddleNameText" Value="----" runat="server" />

<script type="text/javascript">
   

    function getConfirmation() {
        $confirm('Are you sure you want to delete this order?', function (res) {
            if (res) {
                __doPostBack('<%= fsucCmdBarButton.CancelButton.UniqueID %>', '');
            }
        }, 'Complio', true);
    }

    function MVRPkgIsChecked() {
        //    //debugger;
        $jQuery("[id$=btnCheckIsMvrPkgChecked]").click();
        //    //PageMethods.IsMVrPkgChecked();
    }

    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "CommonScreen";
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            $jQuery("[id$=instituteHierarchy]").focusout();
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            var popupHeight = $jQuery(window).height() * (100 / 100);
            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionHierarchyList.aspx?TenantId=" + tenantId + "&DepartmentProgramId=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function ShowCallBackMessage(docMessage) {
        if (docMessage != '') {
            alert(docMessage);
        }
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
                $jQuery("[id$=btnBindPackage]").click();
            }
            winopen = false;
            //UAT-1923
            setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
        }
    }

    function AutoFillSSN(sender, args) {
        if (args.get_checked()) {
            //setPostBackSourceEP();
            $find($jQuery("[id$=txtSSN]")[0].id).set_value('111111111');
        }
        else {
            $find($jQuery("[id$=txtSSN]")[0].id).set_value('')
        }
    }

    //function setPostBackSourceEP() {
    //    $jQuery('.postbacksource').val('EP');
    //    window.DashboardChildClick = 1;
    //}

    function MiddleNameEnableDisable(ID) {
        if (ID.checked) {
            var noMiddleNameText = "----";
            $find($jQuery("[id$=txtMiddleName]")[0].id).set_value(noMiddleNameText);
            $find($jQuery("[id$=txtMiddleName]")[0].id).disable();
            ValidatorEnable($jQuery('[id$=rfvMiddleName]')[0], false);
            $jQuery('[id$=spnMiddleName]').hide();
        }
        else {
            $find($jQuery("[id$=txtMiddleName]")[0].id).set_value('');
            $find($jQuery("[id$=txtMiddleName]")[0].id).enable();
            ValidatorEnable($jQuery('[id$=rfvMiddleName]')[0], true);
            $jQuery('[id$=rfvMiddleName]').hide();
            $jQuery('[id$=spnMiddleName]').show();
        }
    }







    // UAT-2447
    function MaskedUnmaskedPhone(ID, MaskedTextBoxId, PlainTextBoxID, revPlainTextBox, revMaskedPhoneID, regPlainTextID) {
        //debugger;
        if (ID != undefined && ID != '') {
            var chckedBox = $jQuery("[id$=" + ID + "]")[0];
            var isDisicionFieldEnabled = chckedBox.getAttribute('isEnabled');
            if (chckedBox.checked) {
                //debugger;
                var MaskedTextBox = $jQuery("[id$=" + MaskedTextBoxId + "]")[0];
                MaskedTextBox.style.display = "none";
                var PlainTextBoxValidator = ($jQuery("[id$=" + revPlainTextBox + "]")[0]);
                if (PlainTextBoxValidator != "" && PlainTextBoxValidator != undefined && (isDisicionFieldEnabled == undefined || isDisicionFieldEnabled == "true")) {
                    ValidatorEnable(PlainTextBoxValidator, true);
                    //PlainTextBoxValidator.hide();
                    $jQuery("[id$=" + revPlainTextBox + "]").hide();
                }
                var MaskedtextBoxValidator = ($jQuery("[id$=" + revMaskedPhoneID + "]")[0]);
                if (MaskedtextBoxValidator != "" && MaskedtextBoxValidator != undefined) {
                    ValidatorEnable(MaskedtextBoxValidator, false);
                }
                var PlainTExtBoxRegularExp = ($jQuery("[id$=" + regPlainTextID + "]")[0]);
                if (PlainTExtBoxRegularExp != "" && PlainTExtBoxRegularExp != undefined && (isDisicionFieldEnabled == undefined || isDisicionFieldEnabled == "true")) {
                    ValidatorEnable(PlainTExtBoxRegularExp, true);
                    $jQuery("[id$=" + regPlainTextID + "]").hide();
                }

                var PlainTextBox = $jQuery("[id$=" + PlainTextBoxID + "]")[0];
                PlainTextBox.style.display = "block";
            }
            else {
                var MaskedTextBox = $jQuery("[id$=" + MaskedTextBoxId + "]")[0];
                MaskedTextBox.style.display = "block";
                var PlainTextBox = $jQuery("[id$=" + PlainTextBoxID + "]")[0];
                PlainTextBox.style.display = "none";
                var PlainTextBoxValidator = ($jQuery("[id$=" + revPlainTextBox + "]")[0]);
                if (PlainTextBoxValidator != "" && PlainTextBoxValidator != undefined) {
                    ValidatorEnable(PlainTextBoxValidator, false);
                }
                var MaskedtextBoxValidator = ($jQuery("[id$=" + revMaskedPhoneID + "]")[0]);
                if (MaskedtextBoxValidator != "" && MaskedtextBoxValidator != undefined && (isDisicionFieldEnabled == undefined || isDisicionFieldEnabled == "true")) {
                    ValidatorEnable(MaskedtextBoxValidator, true);
                    //MaskedtextBoxValidator.hide();
                    $jQuery("[id$=" + revMaskedPhoneID + "]").hide();
                }
                var PlainTExtBoxRegularExp = ($jQuery("[id$=" + regPlainTextID + "]")[0]);
                if (PlainTExtBoxRegularExp != "" && PlainTExtBoxRegularExp != undefined) {
                    ValidatorEnable(PlainTExtBoxRegularExp, false);
                }
            }
        }
    }

    //UAT-2447
    function ShowHidePhoneOnLoad(instanceid, isEnabled) {
        $jQuery(".dvPhone").each(function (i) {
            //debugger;
            i = i + 1;
            var chkInternationalPhone = $jQuery("[id$=dvPhone_" + i + "] input[type='checkbox']")[0];
            var CurrentinstanceID = $jQuery("[id$=dvPhone_" + i + "] input[type='text']")[1].id.split('Text')[1].split('_')[1];
            var AtrributeGroupMappingId = $jQuery("[id$=dvPhone_" + i + "] input[type='text']")[1].id.split('Text')[1].split('_')[3];
            //debugger;
            if (chkInternationalPhone == undefined)
                return;
            if (instanceid != undefined && isEnabled != undefined && instanceid == CurrentinstanceID) {
                $jQuery("[id$=dvPhone_" + i + "] input[type='checkbox']").attr('isEnabled', isEnabled);
            }
            if (!chkInternationalPhone.checked) {
                var MaskedPhoneElement = $jQuery("[id$=dvMaskedText_" + i + "]")[0];
                MaskedPhoneElement.style.display = "block";

                var PalinPhoneElement = $jQuery("[id$=dvPlainText_" + i + "]")[0];
                PalinPhoneElement.style.display = "none";

                if (CurrentinstanceID != undefined && CurrentinstanceID != '' && AtrributeGroupMappingId != undefined && AtrributeGroupMappingId != '') {
                    var refPlaintextBoxID = "rfv_Text_Plain_" + CurrentinstanceID + "_" + AtrributeGroupMappingId;

                    var reqplainText = $jQuery("[id$=dvPhone_" + i + "]").find("[id$=" + refPlaintextBoxID + "]")[0]
                    if (reqplainText != undefined) {
                        ValidatorEnable(reqplainText, false);
                    }
                }
            }
            else {
                var MaskedPhoneElement = $jQuery("[id$=dvMaskedText_" + i + "]")[0];
                MaskedPhoneElement.style.display = "none";

                var PalinPhoneElement = $jQuery("[id$=dvPlainText_" + i + "]")[0];
                PalinPhoneElement.style.display = "block";

                if (CurrentinstanceID != undefined && CurrentinstanceID != '' && AtrributeGroupMappingId != undefined && AtrributeGroupMappingId != '') {
                    var refMaskedtextBox = "rfv_Text_" + CurrentinstanceID + "_" + AtrributeGroupMappingId;
                    var reqMaskText = $jQuery("[id$=dvPhone_" + i + "]").find("[id$=" + refMaskedtextBox + "]")[0];
                    if (reqMaskText != undefined) {
                        ValidatorEnable(reqMaskText, false);
                    }
                }
            }
        });
    }
    function ValidatePage(s, e) {
        var validation = $jQuery(".errmsg");
        if (validation.length > 0) {
            var isValid = false;
            var groupIds = $jQuery("[id$=hdnGroupId]");
            if (groupIds.length > 0) {
                for (i = 0; i < groupIds.length; i++) {
                    isValid = Page_ClientValidate('submitForm' + groupIds[i].value);
                    if (!isValid)
                    { return isValid; }
                }
            }
            if (isValid && s != undefined && e != undefined) {
                DelayButtonClick(s, e);
            }
            return isValid;
        }
        if (s != undefined && e != undefined) {
            DelayButtonClick(s, e);
        }
        return true;
    }

    var submitclicked = false;
    function DelayButtonClick(s, e) {
        if (submitclicked == false) {
            submitclicked = true;
            s.set_autoPostBack(false);
            window.setTimeout(function () {
                s.set_autoPostBack(true);
                s.click();
                submitclicked = false;
            }, 200, s);
        }
    }

    function EnableValidator(id) {
        if ($jQuery('#' + id)[0] != undefined) {
            ValidatorEnable($jQuery('#' + id)[0], true);
            $jQuery('#' + id).hide();
        }
    }

    function DisableValidator(id) {
        if ($jQuery('#' + id)[0] != undefined) {
            ValidatorEnable($jQuery('#' + id)[0], false);
        }
    }

    function EnableDisableLicenseValidators(sender, args) {
        //debugger;
        var rfvLicenseState = $jQuery("[id$=rfvLicenseState]");
        var rfvtxtLicenseNO = $jQuery("[id$=rfvtxtLicenseNO]");
        var spnLicenseState = $jQuery("[id$=spnLicenseState]");
        var spnLicenseNumber = $jQuery("[id$=spnLicenseNumber]");
        var cmbState = $jQuery("[id$=cmbState]");
        var txtLicenseNO = $jQuery("input[id$=txtLicenseNO]");
        if (sender._checked) {
            if (rfvLicenseState.length > 0 && spnLicenseState.length > 0) {
                EnableValidator($jQuery("[id$=rfvLicenseState]")[0].id);
                $jQuery("[id$=spnLicenseState]")[0].style.display = "inline";
            }
            if (rfvtxtLicenseNO.length > 0 && spnLicenseNumber.length > 0) {
                EnableValidator($jQuery("[id$=rfvtxtLicenseNO]")[0].id);
                $jQuery("[id$=spnLicenseNumber]")[0].style.display = "inline";
            }
            if (cmbState.length > 0) {
                $find(cmbState.attr("id")).enable();
            }
            if (txtLicenseNO.length > 0) {
                $find(txtLicenseNO.attr("id")).clear();
                $find(txtLicenseNO.attr("id")).enable();
            }
        }
        else {
            if (rfvLicenseState.length > 0 && spnLicenseState.length > 0) {
                DisableValidator($jQuery("[id$=rfvLicenseState]")[0].id);
                $jQuery("[id$=spnLicenseState]")[0].style.display = "none";
            }
            if (rfvtxtLicenseNO.length > 0 && spnLicenseNumber.length > 0) {
                DisableValidator($jQuery("[id$=rfvtxtLicenseNO]")[0].id);
                $jQuery("[id$=spnLicenseNumber]")[0].style.display = "none";
            }
            if (cmbState.length > 0) {
                $find(cmbState.attr("id")).disable();
                $find(cmbState.attr("id")).clearSelection();
            }
            if (txtLicenseNO.length > 0) {
                $find(txtLicenseNO.attr("id")).set_textBoxValue('N/A');
                $find(txtLicenseNO.attr("id")).disable();
            }
        }
    }

    function onMVRStateBlur(sender, args) {
        if (sender.get_highlightedItem() != null && (sender.get_originalText() != null && sender.get_originalText() != sender.get_highlightedItem().get_text()))
            sender.get_highlightedItem().select();
        else
            sender.set_text("");
    }

    function MaskedUnmaskedPhone(ID) {
        if (ID.checked) {
            //$jQuery("[id$=dvMaskedPrimaryPhone]").css('visibility', 'hidden');
            $jQuery("[id$=dvMaskedPrimaryPhone]").css('display', 'none');
            $jQuery("[id$=dvUnmaskedPrimaryPhone]").css('display', 'block');
            ValidatorEnable($jQuery('[id$=rfvTxtMobile]')[0], false);
            ValidatorEnable($jQuery('[id$=revTxtMobile]')[0], false);
            ValidatorEnable($jQuery('[id$=rfvTxtMobileUnmasked]')[0], true);
            ValidatorEnable($jQuery('[id$=revTxtMobilePrmyNonMasking]')[0], true);
            $jQuery("[id$=rfvTxtMobileUnmasked]").hide();
            $jQuery("[id$=revTxtMobilePrmyNonMasking]").hide();
        }
        else {
            $jQuery("[id$=dvMaskedPrimaryPhone]").css('display', 'block');
            $jQuery("[id$=dvUnmaskedPrimaryPhone]").css('display', 'none');
            ValidatorEnable($jQuery('[id$=rfvTxtMobileUnmasked]')[0], false);
            ValidatorEnable($jQuery('[id$=revTxtMobilePrmyNonMasking]')[0], false);
            ValidatorEnable($jQuery('[id$=rfvTxtMobile]')[0], true);
            ValidatorEnable($jQuery('[id$=revTxtMobile]')[0], true);
            $jQuery("[id$=rfvTxtMobile]").hide();
            $jQuery("[id$=revTxtMobile]").hide();
        }
    }
</script>
