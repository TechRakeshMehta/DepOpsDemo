<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceAttachedForm.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ServiceAttachedForm" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="~/Resources/Mod/WebSite/Scripts/WebSiteSetup.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/BkgSetup/ManageServiceAttachedForm.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    <%--    <infs:LinkedResource Path="~/Resources/Mod/Templates/TemplatesMaintenance.js" ResourceType="JavaScript" />  --%>
</infs:WclResourceManagerProxy>

<style type="text/css">
    .reEditorModes a {
        display: none;
    }

    .reToolZone {
        display: none;
    }

    .bullet ul {
        margin-left: 10px;
        padding-left: 10px !important;
    }

    .bullet li {
        list-style-position: inside;
        list-style: disc;
    }

    .bullet ol {
        list-style-type: decimal;
        margin-left: 10px;
        padding-left: 10px;
    }

        .bullet ol li {
            list-style: decimal;
        }
</style>


<div class="msgbox">
    <asp:Label ID="lblMessage" runat="server" CssClass="info">
    </asp:Label>
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="Label1" runat="server" CssClass="info">
        </asp:Label>
    </h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdServiceAttachedForm" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0" ShowClearFiltersButton="false"
                GridLines="Both" EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
                NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdServiceAttachedForm_NeedDataSource"
                OnItemCommand="grdServiceAttachedForm_ItemCommand" OnInsertCommand="grdServiceAttachedForm_InsertCommand"
                OnDeleteCommand="grdServiceAttachedForm_DeleteCommand" OnUpdateCommand="grdServiceAttachedForm_UpdateCommand"
                OnItemCreated="grdServiceAttachedForm_ItemCreated">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="SF_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Service Form"
                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                        ShowRefreshButton="false" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="FormName" FilterControlAltText="Filter FormName column"
                            HeaderText="Service Form Name" SortExpression="FormName" UniqueName="FormName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ParentFormName" FilterControlAltText="Filter ParentFormName column"
                            HeaderText="Parent Service Form Name" SortExpression="ParentFormName" UniqueName="ParentFormName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ServiceFormDispatchType" FilterControlAltText="Filter ServiceFormDispatchType column"
                            HeaderText="Service Form Dispatch Type" SortExpression="ServiceFormDispatchType" UniqueName="ServiceFormDispatchType">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this User?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle Width="30px" />
                        </telerik:GridButtonColumn>

                        <%--<telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" Visible="false" ConfirmText="Are you sure you want to delete this Service Form?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>--%>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Service Form" : "Update Service Form" %>'
                                        runat="server" />
                                </h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceGroup">
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Service Form Type</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <%--<asp:RadioButtonList runat="server" ID="rbtnListFormType" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Form Version" Value="1" Selected="True" Enabled="true" onclick="OnClickFormType(this);">&nbsp;</asp:ListItem>
                                                        <asp:ListItem Text="New Form" Value="2" Selected="False" Enabled="true" onclick="OnClickFormType(this);">&nbsp;</asp:ListItem>
                                                    </asp:RadioButtonList>--%>
                                                    <asp:RadioButton ID="rbtnFormVer" runat="server" GroupName="grpFormType" Text="Form Version" Value="0"
                                                        Checked="true" onclick="OnClickFormType(this);" />
                                                    <asp:RadioButton ID="rbtnNewForm" runat="server" GroupName="grpFormType" Text="New Form" Value="1"
                                                        onclick="OnClickFormType(this);" />
                                                </div>
                                                <div class="dvDispatchType" runat="server" style="display: none;" id="dvDispatchType">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Service Form Dispatch Type</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <asp:RadioButtonList ID="rblDispatchType" runat="server" RepeatDirection="Horizontal"
                                                            CssClass="radio_list" onclick="ManageServiceFormDispatchMode(this);">
                                                        </asp:RadioButtonList>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvDispatchType" ControlToValidate="rblDispatchType"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Service Form Dispatch Type is required." Enabled="false"
                                                                ValidationGroup='grpFormSubmit' />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Service Form Name</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox runat="server" ID="txtFormName"
                                                        MaxLength="256">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvFormName" ControlToValidate="txtFormName"
                                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Service Form Name is required." />
                                                    </div>
                                                </div>
                                                <div class="dvParentForm" id="dvParentForm" runat="server">
                                                    <div class='sxlb'>
                                                        <span class="cptn">Parent Service Form</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="ddlParentForm" runat="server" DataTextField="SF_Name" DataValueField="SF_ID">
                                                        </infs:WclComboBox>
                                                        <%--<infs:WclDropDownList ID="ddlParentForm" runat="server" CausesValidation="false" AutoPostBack="false"
                                                            DataTextField="SF_Name" DataValueField="SF_ID">
                                                        </infs:WclDropDownList>--%>
                                                        <div class="vldx">
                                                            <%--  <asp:RequiredFieldValidator runat="server" ID="rfvParentForm" ControlToValidate="ddlParentForm"
                                                                Display="Dynamic" InitialValue="--SELECT--" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                                                ErrorMessage="Parent Form is required." Enabled="false" />--%>
                                                            <asp:CustomValidator ID="rfvParentForm" ErrorMessage="Parent Service Form is required." ValidateEmptyText="true"
                                                                ClientValidationFunction="OnCustomValidatorParentForm" ControlToValidate="ddlParentForm" CssClass="errmsg"
                                                                Display="Dynamic" ValidationGroup="grpFormSubmit" runat="server"></asp:CustomValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx2co' id="dvUploadSvcForm" runat="server">
                                                <div class='sxlb'>
                                                    <span class="cptn">Upload Service Form</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay" MaxFileInputsCount="1"
                                                        MultipleFileSelection="Disabled" OnClientFileSelected="onClientFileSelected" OnClientFileUploaded="onFileUploadedZeroSize"
                                                        OnClientFileUploadRemoved="onFileRemoved" UploadedFilesRendering="BelowFileInput"
                                                        AllowedFileExtensions="PDF,pdf" ToolTip="Click here to select files to upload from your computer.">
                                                        <Localization Select="Browse" />
                                                    </infs:WclAsyncUpload>
                                                    <div style="clear: both; float: left; position: relative;">
                                                        <asp:Label ID="lblUploadFormName" runat="server" Visible="false"></asp:Label>
                                                    </div>
                                                    <div class='vldx'>
                                                        <asp:Label ID="lblUploadFormMsg" class="errmsg" runat="server" Visible="false">Upload Service Form is required.</asp:Label>
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class="sxro sx2co" style="display:block">
                                                <div class='sxlb'>
                                                    <asp:Label ID="Label4" Text="Template language" runat="server" AssociatedControlID="cmbTemplateLanguage" CssClass="cptn" /><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbTemplateLanguage" runat="server" DataTextField="LAN_Name" AutoPostBack="true"
                                                        DataValueField="LAN_ID">
                                                    </infs:WclComboBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvcmbTemplateLanguage" ControlToValidate="cmbTemplateLanguage"
                                                            class="errmsg" ValidationGroup="grpFormSubmit"
                                                            Display="Dynamic" ErrorMessage="Template language is required." />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class="dvSrvcFormTemplate" runat="server" id="dvSrvcFormTemplate" style="display: none;">
                                                <h1 class="mhdr">
                                                    <asp:Label ID="Label2" Text='<%# (Container is GridEditFormInsertItem) ? "Add Notification Template" : "Update Notification Template" %>'
                                                        runat="server" />
                                                </h1>
                                                <div class='sxro sx2co'>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Template Name</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox ID="txtTemplateName" runat="server" MaxLength="50">
                                                        </infs:WclTextBox>
                                                        <asp:HiddenField ID="hdnSvcFormCommunicationTemplateID" runat="server"></asp:HiddenField>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvTemplateName" ControlToValidate="txtTemplateName" Enabled="false"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Template Name is required." ValidationGroup="grpFormSubmit" />
                                                            <asp:RegularExpressionValidator runat="server" ID="rgvTemplateName" ControlToValidate="txtTemplateName"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Only alphanumeric is allowed." Enabled="false"
                                                                ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="grpFormSubmit" />
                                                        </div>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Subject</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox ID="txtTemplateSubject" runat="server" MaxLength="255">
                                                        </infs:WclTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvSubject" ControlToValidate="txtTemplateSubject" Enabled="false"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Subject is required." ValidationGroup="grpFormSubmit" />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                                <div class='sxro sx2co'>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Template Content</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm m2spn'>
                                                        <infs:WclEditor ID="ecTemplateContent" ClientIDMode="Static" runat="server" Width="99.5%" OnClientLoad="OnClientLoad"
                                                            ToolsFile="~/Templates/Data/Tools.xml" EnableResize="false" OnClientCommandExecuting="OnClientCommandExecuting"
                                                            Height="130px">
                                                        </infs:WclEditor>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvContent" ControlToValidate="ecTemplateContent"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Template content is required." Enabled="false"
                                                                ValidationGroup="grpFormSubmit" />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="dvSrvcFormReminderTemplate" runat="server" id="dvSrvcFormReminderTemplate" style="display: none;">
                                                <h1 class="mhdr">
                                                    <asp:Label ID="Label3" Text='<%# (Container is GridEditFormInsertItem) ? "Add Reminder Template" : "Update Reminder Template" %>'
                                                        runat="server" />
                                                </h1>
                                                <div class='sxro sx2co'>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Template Name</span></span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox ID="txtReminderTemplateName" runat="server" MaxLength="50">
                                                        </infs:WclTextBox>
                                                        <asp:HiddenField ID="hdnReminderCommunicationTemplateID" runat="server"></asp:HiddenField>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvReminderTemplateName" ControlToValidate="txtReminderTemplateName" Enabled="false"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Template Name is required." ValidationGroup="grpFormSubmit" />
                                                            <asp:RegularExpressionValidator runat="server" ID="rgvReminderTemplateName" ControlToValidate="txtReminderTemplateName"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Only alphanumeric is allowed." Enabled="false"
                                                                ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="grpFormSubmit" />
                                                        </div>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Subject</span></span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox ID="txtReminderSubject" runat="server" MaxLength="255">
                                                        </infs:WclTextBox>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvReminderSubject" ControlToValidate="txtReminderSubject" Enabled="false"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Subject is required." ValidationGroup="grpFormSubmit" />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                                <div class='sxro sx2co'>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Template Content</span></span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm m2spn'>
                                                        <infs:WclEditor ID="ecReminderContent" ClientIDMode="Static" runat="server" Width="99.5%" OnClientLoad="OnClientLoad"
                                                            ToolsFile="~/Templates/Data/Tools.xml" EnableResize="false" OnClientCommandExecuting="OnClientCommandExecuting"
                                                            Height="130px">
                                                        </infs:WclEditor>
                                                        <div class='vldx'>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvReminderContent" ControlToValidate="ecReminderContent"
                                                                class="errmsg" Display="Dynamic" ErrorMessage="Template content is required." Enabled="false"
                                                                ValidationGroup="grpFormSubmit" />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlServiceGroup"
                                        ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
    <div style="display: none">
        <infs:WclAsyncUpload runat="server" ID="uploadControlTemp" HideFileInput="true" Skin="Hay" MaxFileInputsCount="1"
            MultipleFileSelection="Disabled"
            AllowedFileExtensions="PDF,pdf" ToolTip="Click here to select files to upload from your computer.">
            <Localization Select="Browse" />
        </infs:WclAsyncUpload>
    </div>
</div>
