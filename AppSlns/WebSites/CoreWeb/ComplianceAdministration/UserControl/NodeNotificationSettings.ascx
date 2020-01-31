<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NodeNotificationSettings.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.NodeNotificationSettings" %>

<%@ Register TagPrefix="telerik" Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="Commandbar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<script type="text/javascript">
    $jQuery(document).ready(function () {
        parent.ResetTimer();
        parent.Page.hideProgress();
    });

    function RefrshTree() {

        var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
        btn.click();
    }

</script>
<style type="text/css">
   .chkActive tr td:first-child label {
    /* styles */
    margin-right:2px !important;
}
</style>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblNodeTitle" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="sbsection">
            <h1 class="sbhdr">
                <asp:Label ID="lblDeadlineNotifications" runat="server" Text="Deadline Notifications"></asp:Label>
            </h1>
            <div class="sbcontent" id="divDeadlineNotification" runat="server">
                <div id="dvNodeDeadline" runat="server" class="swrap">
                    <infs:WclGrid runat="server" ID="grdNodeDeadline" AllowPaging="True" PageSize="10"
                        AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" ShowAllExportButtons="False"
                        NonExportingColumns="EditCommandColumn, DeleteColumn" ValidationGroup="grpNodeNotificationSettings"
                        OnNeedDataSource="grdNodeDeadline_NeedDataSource" OnItemDataBound="grdNodeDeadline_ItemDataBound" OnItemCommand="grdNodeDeadline_ItemCommand"
                        OnInsertCommand="grdNodeDeadline_InsertCommand" OnUpdateCommand="grdNodeDeadline_UpdateCommand"
                        OnDeleteCommand="grdNodeDeadline_DeleteCommand" ShowClearFiltersButton="false">
                        <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                            Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                            ExportOnlyData="true" IgnorePaging="true">
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="ND_ID,ND_NodeNotificationMappingId" AllowFilteringByColumn="false">
                            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Node Deadline"
                                ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                                ShowExportToWordButton="false"></CommandItemSettings>
                            <Columns>
                                <telerik:GridBoundColumn DataField="ND_Name" FilterControlAltText="Filter ND_Name column"
                                    HeaderText="Name" SortExpression="ND_Name" UniqueName="ND_Name">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ND_Description" FilterControlAltText="Filter ND_Description column"
                                    HeaderText="Description" SortExpression="ND_Description" UniqueName="ND_Description">
                                </telerik:GridBoundColumn>
                                <telerik:GridDateTimeColumn DataField="ND_DeadlineDate" FilterControlAltText="Filter ND_DeadlineDate column"
                                    HeaderText="Deadline Date" SortExpression="ND_DeadlineDate" ItemStyle-Width="15%"
                                    UniqueName="ND_DeadlineDate" DataFormatString="{0:MM/dd/yyyy}">
                                </telerik:GridDateTimeColumn>
                                <telerik:GridBoundColumn DataField="ND_Frequency" UniqueName="ND_Frequency" FilterControlAltText="Filter ND_Frequency column"
                                    HeaderText="Frequency" SortExpression="ND_Frequency">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ND_DaysBeforeDeadline" UniqueName="ND_DaysBeforeDeadline" FilterControlAltText="Filter ND_DaysBeforeDeadline column"
                                    HeaderText="Days Before Deadline" SortExpression="ND_DaysBeforeDeadline">
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                                    <ItemTemplate>
                                        <telerik:RadButton ID="btnViewDetail" ButtonType="LinkButton" CommandName="ViewDetail"
                                            ToolTip="Click here to manage templates"
                                            runat="server" Text="Manage Templates" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                        </telerik:RadButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                                    <HeaderStyle Width="30px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Node Deadline?"
                                    Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle Width="30px" />
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings EditFormType="Template">
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                                <FormTemplate>
                                    <div class="section" runat="server" id="divEditBlock" visible="true">
                                        <h1 class="mhdr">
                                            <asp:Label ID="lblTitleNodeDeadline" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Node Deadline" : "Update Node Deadline" %>'
                                                runat="server" /></h1>
                                        <div class="content">
                                            <div class="sxform auto">
                                                <div class="msgbox">
                                                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                                </div>
                                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlNodeDeadline">
                                                    <infs:WclTextBox runat="server" Text='<%# Eval("ND_ID") %>' ID="txtNodeDeadlineId"
                                                        Visible="false">
                                                    </infs:WclTextBox>
                                                    <div class='sxro sx3co'>
                                                        <div class='sxlb'>
                                                            <span class="cptn">Name</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclTextBox ID="txtNodeDeadlineName" Width="100%" runat="server" Text='<%# Eval("ND_Name") %>'
                                                                MaxLength="50">
                                                            </infs:WclTextBox>
                                                            <div id="dvLabel" class='vldx'>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvLabel" ControlToValidate="txtNodeDeadlineName"
                                                                    class="errmsg" ErrorMessage="Name is required." ValidationGroup='grpNodeNotificationSettings'
                                                                    Enabled="true" />
                                                            </div>
                                                        </div>
                                                        <div class='sxlb'>
                                                            <span class="cptn">Description</span>
                                                        </div>
                                                        <div class='sxlm '>
                                                            <infs:WclTextBox Width="100%" ID="txtNodeDeadlineDescription" runat="server"
                                                                Text='<%# Eval("ND_Description") %>' MaxLength="250">
                                                            </infs:WclTextBox>
                                                        </div>
                                                        <div class='sxlb'>
                                                            <span class="cptn">Deadline Date</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm '>
                                                            <infs:WclDatePicker ID="dpkrDeadlineDate" runat="server" DateInput-EmptyMessage="Select a date" SelectedDate='<%# (Container is GridEditFormInsertItem) ? null : Eval("ND_DeadlineDate") %>'>
                                                            </infs:WclDatePicker>
                                                            <div class="valdx">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvDeadlineDate" CssClass="errmsg" ControlToValidate="dpkrDeadlineDate"
                                                                    Display="Dynamic" ErrorMessage="Deadline Date is required." ValidationGroup='grpNodeNotificationSettings' />
                                                            </div>
                                                        </div>
                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>

                                                    <div class='sxro sx3co'>
                                                        <div class='sxlb'>
                                                            <span class="cptn">Frequency(days)</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclNumericTextBox ID="txtFrequency" Type="Number" runat="server" MinValue="0" MaxLength="9" Text='<%# Eval("ND_Frequency") %>'>
                                                                <NumberFormat AllowRounding="false" DecimalDigits="0" DecimalSeparator="." />
                                                            </infs:WclNumericTextBox>
                                                            <div class='vldx'>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvFrequency" ControlToValidate="txtFrequency"
                                                                    class="errmsg" Display="Dynamic" ErrorMessage="Frequency is required." ValidationGroup="grpNodeNotificationSettings" />
                                                            </div>
                                                        </div>
                                                        <div class='sxlb'>
                                                            <span class="cptn">Days Before Deadline</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm '>
                                                            <infs:WclNumericTextBox ID="txtDaysBeforeDeadline" Type="Number" runat="server" MinValue="0" MaxLength="9" Text='<%# Eval("ND_DaysBeforeDeadline") %>'>
                                                                <NumberFormat AllowRounding="false" DecimalDigits="0" DecimalSeparator="." />
                                                            </infs:WclNumericTextBox>
                                                            <div class='vldx'>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvDaysBeforeDeadline" ControlToValidate="txtDaysBeforeDeadline"
                                                                    class="errmsg" Display="Dynamic" ErrorMessage="Days Before Deadline are required." ValidationGroup="grpNodeNotificationSettings" />
                                                            </div>
                                                        </div>

                                                        <div class='sxlb'>
                                                            <span class="cptn">User Group</span>
                                                        </div>
                                                        <div class='sxlm '>
                                                            <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID"
                                                                CheckBoxes="true" EmptyMessage="--SELECT--">
                                                            </infs:WclComboBox>
                                                        </div>

                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>

                                                </asp:Panel>
                                            </div>
                                            <infsu:Commandbar ID="fsucCmdBarNodeDeadline" runat="server" GridMode="true" DefaultPanel="pnlNodeDeadline" GridInsertText="Save" GridUpdateText="Save"
                                                ValidationGroup="grpNodeNotificationSettings" ExtraButtonIconClass="icnreset" />
                                        </div>
                                    </div>
                                </FormTemplate>
                            </EditFormSettings>
                            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                        </MasterTableView>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>

            </div>
        </div>

        <div class="sbsection" id="divNagEmailNotfication" runat="server">
            <h1 class="sbhdr">
                <asp:Label ID="lblNagEmailNotifications" runat="server" Text="Nag Email Notifications"></asp:Label>
            </h1>
            <div class="sbcontent">
                <div class="sxform auto">
                    <asp:Panel ID="pnlNagEmailNotifications" CssClass="sxpnl" runat="server">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Frequency(days)</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ID="txtNagFrequency" Type="Number" runat="server" MinValue="0" MaxLength="9">
                                    <NumberFormat AllowRounding="false" DecimalDigits="0" DecimalSeparator="." />
                                </infs:WclNumericTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvFrequencyy" ControlToValidate="txtNagFrequency"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Frequency is required." ValidationGroup="grpNagEmailNotifications" />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">IsActive</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="chkActive" runat="server" RepeatDirection="Horizontal" AutoPostBack="false" CssClass="chkActive">
                                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="false"> </asp:ListItem>
                                </asp:RadioButtonList>
                                <%--<uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />--%>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvChkIsActive" ControlToValidate="chkActive"
                                        class="errmsg" Display="Dynamic" ErrorMessage="IsActive is required." ValidationGroup="grpNagEmailNotifications" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <asp:HiddenField ID="hdnSavedNagFrequency" runat="server" />
                    </asp:Panel>
                </div>
            </div>
            <infsu:Commandbar ID="fsucCmdBarNagEmailNotify" runat="server" DefaultPanel="pnlNagEmailNotifications"
                DisplayButtons="Save,Submit" AutoPostbackButtons="Save,Submit" ValidationGroup="grpNagEmailNotifications"
                ButtonPosition="Right" SaveButtonIconClass="" OnSubmitClick="fsucCmdBarNagEmailNotify_NagEmailTempClick" SubmitButtonText="Nag Email Template" OnSaveClick="fsucCmdBarNagEmailNotify_SaveClick">
            </infsu:Commandbar>
        </div>

        <div class="sbsection">
            <h1 class="sbhdr">
                <asp:Label ID="Label1" runat="server" Text="Contacts"></asp:Label>
            </h1>
            <div class="msgbox">
                <asp:Label ID="lblContactMessage" runat="server" Visible="false"></asp:Label>
            </div>
            <div class="sbcontent">

                <div id="divContacts" runat="server" class="swrap">
                    <infs:WclGrid runat="server" ID="grdContacts" AllowPaging="false" PageSize="10"
                        AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" ShowAllExportButtons="False" AllowFilteringByColumn="false"
                        NonExportingColumns="EditCommandColumn, DeleteColumn" ValidationGroup="grpContacts"
                        OnItemDataBound="grdContacts_ItemDataBound" OnNeedDataSource="grdContacts_NeedDataSource" OnItemCommand="grdContacts_ItemCommand"
                        OnInsertCommand="grdContacts_InsertCommand" OnUpdateCommand="grdContacts_UpdateCommand"
                        OnDeleteCommand="grdContacts_DeleteCommand" OnInit="grdContacts_Init"
                        ShowClearFiltersButton="false" OnItemCreated="grdContacts_ItemCreated" EnableDefaultFeatures="false">
                        <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                            Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                            ExportOnlyData="true" IgnorePaging="true">
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true"></Selecting>
                        </ClientSettings>
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="ICM_ID,InstitutionContact.ICO_ID" AllowFilteringByColumn="false">
                            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add Contact"
                                ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                                ShowExportToWordButton="false"></CommandItemSettings>
                            <Columns>
                                <telerik:GridBoundColumn DataField="InstitutionContact.ICO_FirstName" FilterControlAltText="Filter First Name column"
                                    HeaderText="First Name" SortExpression="InstitutionContact.ICO_FirstName" UniqueName="ICO_FirstName">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="InstitutionContact.ICO_LastName" FilterControlAltText="Filter Last Name column"
                                    HeaderText="Last Name" SortExpression="InstitutionContact.ICO_LastName" UniqueName="ICO_LastName">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="InstitutionContact.ICO_Title" FilterControlAltText="Filter Title column"
                                    HeaderText="Title" SortExpression="InstitutionContact.ICO_Title" UniqueName="ICO_Title">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="InstitutionContact.ICO_PrimaryEmailAddress" FilterControlAltText="Filter Email Address column"
                                    HeaderText="Email Address" SortExpression="InstitutionContact.ICO_PrimaryEmailAddress" UniqueName="ICO_PrimaryEmailAddress">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="InstitutionContact.ICO_PrimaryPhone" FilterControlAltText="Filter Phone column"
                                    HeaderText="Phone" SortExpression="InstitutionContact.ICO_PrimaryPhone" UniqueName="ICO_PrimaryPhone">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="InstitutionContact.ICO_Address1" FilterControlAltText="Filter Address1 column"
                                    HeaderText="Address1" SortExpression="InstitutionContact.ICO_Address1" UniqueName="ICO_Address1">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="InstitutionContact.ICO_Address2" FilterControlAltText="Filter Address column"
                                    HeaderText="Address2" SortExpression="InstitutionContact.ICO_Address2" UniqueName="ICO_Address2">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="InstitutionContact.ICO_ZipCodeID" FilterControlAltText="Filter Zip Code column"
                                    HeaderText="Zip Code" SortExpression="InstitutionContact.ICO_ZipCodeID" UniqueName="ICO_ZipCodeID">
                                </telerik:GridBoundColumn>


                                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                                    <HeaderStyle Width="30px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Contact?"
                                    Text="Delete" UniqueName="DeleteColumn">
                                    <HeaderStyle Width="30px" />
                                </telerik:GridButtonColumn>
                            </Columns>
                            <NestedViewTemplate>


                                <div class="msgbox">
                                    <asp:Label ID="lblNotificationMessage" runat="server" Visible="false"></asp:Label>
                                </div>
                                <h1 class="shdr"><u>Notifications</u>
                                </h1>
                                <div class="swrap">
                                    <infs:WclGrid runat="server" ID="grdNotifications" AllowPaging="false"
                                        PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" ShowClearFiltersButton="false"
                                        GridViewMode="AutoAddOnly" NonExportingColumns="EditCommandColumn, DeleteColumn"
                                        OnItemCreated="grdNotifications_ItemCreated" OnNeedDataSource="grdNotifications_NeedDataSource"
                                        OnInsertCommand="grdNotifications_InsertCommand" OnDeleteCommand="grdNotifications_DeleteCommand"
                                        OnUpdateCommand="grdNotifications_UpdateCommand"
                                        ShowAllExportButtons="false" AllowFilteringByColumn="false" PagerStyle-ShowPagerText="false" EnableDefaultFeatures="false">

                                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="HNM_ID,lkpCopyType.CT_Id,lkpCommunicationSubEvent.CommunicationSubEventID"
                                            AllowFilteringByColumn="false">
                                            <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Notification" ShowRefreshButton="false"
                                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="lkpCopyType.CT_Name" FilterControlAltText="Filter Copy Type column"
                                                    HeaderText="Copy Type" SortExpression="lkpCopyType.CT_Name" UniqueName="CopyTypeName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="lkpCommunicationSubEvent.Name" FilterControlAltText="Filter Sub Event column"
                                                    HeaderText="Sub Event" SortExpression="lkpCommunicationSubEvent.Name" UniqueName="SubEventName">
                                                </telerik:GridBoundColumn>
                                                <%--   <telerik:GridBoundColumn DataField="HNM_IsCommunicationCenter" FilterControlAltText="Filter Communication Center column"
                                                    HeaderText="Is Communication Center" SortExpression="HNM_IsCommunicationCenter" UniqueName="HNM_IsCommunicationCenter">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="HNM_IsEmail" FilterControlAltText="Filter Email column"
                                                    HeaderText="Is Email" SortExpression="HNM_IsEmail" UniqueName="HNM_IsEmail">
                                                </telerik:GridBoundColumn>--%>

                                                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                                                    <HeaderStyle CssClass="tplcohdr" />
                                                    <ItemStyle CssClass="MyImageButton" />
                                                </telerik:GridEditCommandColumn>
                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                                    ConfirmText="Are you sure you want to delete this Notification?"
                                                    Text="Delete" UniqueName="DeleteColumn">
                                                    <HeaderStyle CssClass="tplcohdr" />
                                                    <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                                                </telerik:GridButtonColumn>
                                            </Columns>
                                            <EditFormSettings EditFormType="Template">
                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                </EditColumn>
                                                <FormTemplate>
                                                    <div class="section">
                                                        <h1 class="mhdr">
                                                            <asp:Label ID="lblNewHeading1" Text='<%#(Container is GridEditFormInsertItem) ? "Add New Notification" : "Edit Notification"%>'
                                                                runat="server"></asp:Label></h1>
                                                        <div class="content">
                                                            <div class="sxform auto">
                                                                <div class="msgbox">
                                                                    <asp:Label ID="lblNotificationSuccess" runat="server" Visible="false"></asp:Label>
                                                                </div>
                                                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMNewNotification">
                                                                    <div class='sxro sx2co'>
                                                                        <div id="divUserType" runat="server">
                                                                            <div class='sxlb'>
                                                                                <asp:Label ID="lblCopyType" Text="Copy Type" runat="server"
                                                                                    AssociatedControlID="cmbCopyType" CssClass="cptn" />
                                                                            </div>
                                                                            <div class='sxlm'>
                                                                                <infs:WclComboBox ID="cmbCopyType" runat="server"
                                                                                    DataValueField="CT_Id" DataTextField="CT_Name">
                                                                                </infs:WclComboBox>
                                                                                <div class='vldx'>
                                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvCommunicationType" ControlToValidate="cmbCopyType"
                                                                                        class="errmsg" ValidationGroup="grpValdNotification" InitialValue="--SELECT--"
                                                                                        Display="Dynamic" ErrorMessage="Copy Type is required." />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div id="div10" runat="server">
                                                                            <div class='sxlb'>
                                                                                <asp:Label ID="lblSubEvent" Text="Sub Event" runat="server"
                                                                                    AssociatedControlID="cmbSubEvent" CssClass="cptn" />
                                                                            </div>
                                                                            <div class='sxlm'>
                                                                                <infs:WclComboBox ID="cmbSubEvent" runat="server"
                                                                                    DataValueField="CommunicationSubEventID" DataTextField="Name">
                                                                                </infs:WclComboBox>
                                                                                <div class='vldx'>
                                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvSubEvent" ControlToValidate="cmbSubEvent"
                                                                                        class="errmsg" ValidationGroup="grpValdNotification" InitialValue="--SELECT--"
                                                                                        Display="Dynamic" ErrorMessage="Sub Event is required." />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class='sxroend'>
                                                                        </div>
                                                                </asp:Panel>
                                                            </div>
                                                            <infsu:Commandbar ID="fsucCmdBarNewNotification" runat="server" GridInsertText="Save" GridUpdateText="Save" GridMode="true" ValidationGroup="grpValdNotification"
                                                                DefaultPanel="pnlMNewNotification" />
                                                        </div>
                                                    </div>
                                                </FormTemplate>
                                            </EditFormSettings>

                                        </MasterTableView>
                                    </infs:WclGrid>
                                </div>
                            </NestedViewTemplate>
                            <EditFormSettings EditFormType="Template">
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                                <FormTemplate>
                                    <div class="section">
                                        <h1 class="mhdr">
                                            <asp:Label ID="lblNewHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Add New Contact" : "Edit Contact"%>'
                                                runat="server"></asp:Label></h1>
                                        <div class="content">
                                            <div class="sxform auto">
                                                <div class="msgbox">
                                                    <asp:Label ID="lblContactSuccess" runat="server"></asp:Label>
                                                </div>
                                                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlMNew">
                                                    <div class='sxro sx3co' id="divContact" runat="server">
                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblContactStatus" Text="Select Contact" runat="server"
                                                                CssClass="cptn" /><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>

                                                            <infs:WclComboBox ID="cmbContactStatus" runat="server"
                                                                AutoPostBack="true" DataValueField="ICO_ID" DataTextField="ICO_Name" OnItemDataBound="cmbContactStatus_ItemDataBound" OnSelectedIndexChanged="cmbContactStatus_SelectedIndexChanged">
                                                            </infs:WclComboBox>
                                                            <div class='vldx'>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvContactStatus" ControlToValidate="cmbContactStatus"
                                                                    class="errmsg" ValidationGroup="grpValdContacts" InitialValue="-- SELECT --"
                                                                    Display="Dynamic" ErrorMessage="Contact is required." />

                                                            </div>
                                                        </div>
                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>

                                                    <div class='sxro sx3co'>
                                                        <div id="divUserType1" runat="server">
                                                            <div class='sxlb'>
                                                                <asp:Label ID="lblFirstName" Text="First Name" runat="server"
                                                                    CssClass="cptn" /><span class="reqd">*</span>
                                                            </div>
                                                            <div class='sxlm'>
                                                                <infs:WclTextBox ID="txtFirstName" Width="100%" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("InstitutionContact.ICO_FirstName")  %>'
                                                                    MaxLength="50">
                                                                </infs:WclTextBox>
                                                                <div id="dvLabel" class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtFirstName"
                                                                        Display="Dynamic" class="errmsg" ErrorMessage="First Name is required." ValidationGroup='grpValdContacts' />
                                                                    <asp:RegularExpressionValidator runat="server" ID="revFirstName" ControlToValidate="txtFirstName"
                                                                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$" ValidationGroup='grpValdContacts'
                                                                        ErrorMessage="Invalid character(s)." />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblLastName" Text="Last Name" runat="server"
                                                                CssClass="cptn" /><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclTextBox ID="txtLastName" Width="100%" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("InstitutionContact.ICO_LastName") %>'
                                                                MaxLength="50">
                                                            </infs:WclTextBox>
                                                            <div id="Div1" class='vldx'>
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                                                    Display="Dynamic" class="errmsg" ErrorMessage="Last Name is required." ValidationGroup='grpValdContacts' />
                                                                <asp:RegularExpressionValidator runat="server" ID="rgvLastName" ControlToValidate="txtLastName"
                                                                    Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$" ValidationGroup='grpValdContacts'
                                                                    ErrorMessage="Invalid character(s)." />
                                                            </div>
                                                        </div>

                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblTitle" Text="Title" runat="server"
                                                                CssClass="cptn" />
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclTextBox ID="txtTitle" Width="100%" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("InstitutionContact.ICO_Title") %>'
                                                                MaxLength="50">
                                                            </infs:WclTextBox>
                                                            <div id="Div5" class='vldx'>
                                                                <%-- <asp:RequiredFieldValidator runat="server" ID="rfvTitle" ControlToValidate="txtTitle"
                                                                    Display="Dynamic" class="errmsg" ErrorMessage="Title is required." ValidationGroup='grpValdContacts' />
                                                                <asp:RegularExpressionValidator runat="server" ID="rgvTitle" ControlToValidate="txtTitle"
                                                                    Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$" ValidationGroup='grpValdContacts'
                                                                    ErrorMessage="Invalid character(s)." />--%>
                                                            </div>
                                                        </div>
                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>


                                                    <div class='sxro sx3co'>
                                                        <div id="div2" runat="server">
                                                            <div class='sxlb'>
                                                                <asp:Label ID="lblEmailAddress" Text="Email Address" runat="server"
                                                                    CssClass="cptn" /><span class="reqd">*</span>
                                                            </div>
                                                            <div class='sxlm'>
                                                                <infs:WclTextBox ID="txtPrimaryEmailAddress" Width="100%" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("InstitutionContact.ICO_PrimaryEmailAddress")  %>'
                                                                    MaxLength="50">
                                                                </infs:WclTextBox>
                                                                <div id="Div3" class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPrimaryEmailAddress" ControlToValidate="txtPrimaryEmailAddress"
                                                                        Display="Dynamic" class="errmsg" ErrorMessage="Email Address is required." ValidationGroup='grpValdContacts' />
                                                                    <asp:RegularExpressionValidator runat="server" ID="rgvPrimaryEmailAddress" ControlToValidate="txtPrimaryEmailAddress"
                                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is not valid." ValidationGroup='grpValdContacts'
                                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblPhone" Text="Phone" runat="server"
                                                                CssClass="cptn" />
                                                        </div>
                                                        <div class='sxlm'>
                                                            <%-- <infs:WclTextBox ID="txtPhone" Width="100%" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("InstitutionContact.ICO_PrimaryPhone") %>'
                                                                MaxLength="50">
                                                            </infs:WclTextBox>--%>


                                                            <infs:WclMaskedTextBox Width="99%" ID="txtPhone" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("InstitutionContact.ICO_PrimaryPhone") %>'
                                                                Mask="(###)-###-####" />

                                                            <div id="Div4" class='vldx'>
                                                                <%--   <asp:RequiredFieldValidator runat="server" ID="rfvPhone" ControlToValidate="txtPhone"
                                                                    Display="Dynamic" class="errmsg" ErrorMessage="Phone is required." ValidationGroup='grpValdContacts' />
                                                                <asp:RegularExpressionValidator runat="server" ID="rgvPhone" ControlToValidate="txtPhone"
                                                                    Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$" ValidationGroup='grpValdContacts'
                                                                    ErrorMessage="Invalid character(s)." />--%>
                                                            </div>
                                                        </div>

                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblAddress1" Text="Address1" runat="server"
                                                                CssClass="cptn" />
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclTextBox ID="txtAddress1" Width="100%" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("InstitutionContact.ICO_Address1") %>'
                                                                MaxLength="50">
                                                            </infs:WclTextBox>
                                                            <div id="Div6" class='vldx'>
                                                                <%--   <asp:RequiredFieldValidator runat="server" ID="rfvAddress1" ControlToValidate="txtAddress1"
                                                                    Display="Dynamic" class="errmsg" ErrorMessage="Address1 is required." ValidationGroup='grpValdContacts' />
                                                                <asp:RegularExpressionValidator runat="server" ID="rgvAddress1" ControlToValidate="txtAddress1"
                                                                    Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$" ValidationGroup='grpValdContacts'
                                                                    ErrorMessage="Invalid character(s)." />--%>
                                                            </div>
                                                        </div>
                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>



                                                    <div class='sxro sx3co'>
                                                        <div id="div7" runat="server">
                                                            <div class='sxlb'>
                                                                <asp:Label ID="lblAddress2" Text="Address2" runat="server"
                                                                    CssClass="cptn" />
                                                            </div>
                                                            <div class='sxlm'>
                                                                <infs:WclTextBox ID="txtAddress2" Width="100%" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("InstitutionContact.ICO_Address2") %>'
                                                                    MaxLength="50">
                                                                </infs:WclTextBox>
                                                                <div id="Div8" class='vldx'>
                                                                    <%--  <asp:RequiredFieldValidator runat="server" ID="rfvAddress2" ControlToValidate="txtAddress2"
                                                                        Display="Dynamic" class="errmsg" ErrorMessage="Address2 is required." ValidationGroup='grpValdContacts' />
                                                                    <asp:RegularExpressionValidator runat="server" ID="rgvAddress2" ControlToValidate="txtAddress2"
                                                                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$" ValidationGroup='grpValdContacts'
                                                                        ErrorMessage="Invalid character(s)." />--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class='sxlb'>
                                                            <asp:Label ID="lblZipCode" Text="Zip Code" runat="server"
                                                                CssClass="cptn" />
                                                        </div>
                                                        <div class='sxlm'>
                                                            <%-- <infs:WclTextBox ID="txtZipCode" Width="100%" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("InstitutionContact.ICO_ZipCodeID")  %>'
                                                                MaxLength="50">--%>

                                                            <infs:WclMaskedTextBox ID="txtZipCode" Width="95%" runat="server" MaxLength="5" Mask="#####" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("InstitutionContact.ICO_ZipCodeID")  %>' />


                                                            <div id="Div9" class='vldx'>
                                                                <%--    <asp:RequiredFieldValidator runat="server" ID="rfvZipCode" ControlToValidate="txtZipCode"
                                                                    Display="Dynamic" class="errmsg" ErrorMessage="Zip Code is required." ValidationGroup='grpValdContacts' />
                                                                <asp:RegularExpressionValidator runat="server" ID="rgvZipCode" ControlToValidate="txtZipCode"
                                                                    Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d\s\-\.]{1,50}$" ValidationGroup='grpValdContacts'
                                                                    ErrorMessage="Invalid character(s)." />--%>
                                                            </div>
                                                        </div>
                                                        <div class='sxroend'>
                                                        </div>
                                                    </div>



                                                </asp:Panel>
                                            </div>
                                            <infsu:Commandbar ID="fsucCmdBarNew" runat="server" GridMode="true" ValidationGroup="grpValdContacts" GridInsertText="Save" GridUpdateText="Save"
                                                DefaultPanel="pnlMNew" />
                                        </div>
                                    </div>

                                </FormTemplate>
                            </EditFormSettings>
                            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                        </MasterTableView>
                    </infs:WclGrid>
                </div>
                <div class="gclr">
                </div>
            </div>
        </div>
    </div>
</div>
