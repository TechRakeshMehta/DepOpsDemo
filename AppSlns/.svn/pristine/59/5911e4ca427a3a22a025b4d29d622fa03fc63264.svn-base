<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageInsturctorsPreceptors.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.ManageInsturctorsPreceptors" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxManageInsturctorsPreceptors">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/ClientContact.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script type="text/javascript">
    $page.add_pageLoaded(function () {
        if (Telerik.Web.UI.RadAsyncUpload != null && Telerik.Web.UI.RadAsyncUpload != undefined) {
            Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
            Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
        }
    });

    function pageLoad() {

        //UAT-2447
        //MaskedUnmaskedPhone($jQuery(".PhoneCheck")[0]);
    }


    //UAt-2447
    function MaskedUnmaskedPhone(ID) {
        if (ID == undefined) return;
        if (!ID.checked) {
            $jQuery("[id$=txtInternationalPhone]").hide();
            ValidatorEnable($jQuery("[id$=rfvTxtInternationalPhn]")[0], false);
            ValidatorEnable($jQuery("[id$=revTxtMobilePrmyNonMasking]")[0], false);
            ValidatorEnable($jQuery("[id$=rfvPhone]")[0], true);
            $jQuery("[id$=rfvPhone]").hide();

            jQuery(ID).hide();
            setTimeout(function () { jQuery(ID).show(); }, 0);
            $jQuery("[id$=txtPhone]").show();
        }
        else {
            $jQuery("[id$=txtPhone]").hide();
            ValidatorEnable($jQuery('[id$=rfvPhone]')[0], false);
            jQuery(ID).hide();
            setTimeout(function () { jQuery(ID).show(); }, 0);

            $jQuery("[id$=txtInternationalPhone]").show();
            ValidatorEnable($jQuery("[id$=rfvTxtInternationalPhn]")[0], true);
            ValidatorEnable($jQuery("[id$=revTxtMobilePrmyNonMasking]")[0], true);
            $jQuery("[id$=rfvTxtInternationalPhn]").hide();
        }
    }

    function ValidateAvailableDays(sender, args) {
        var idlist = '';
        var tpStartTimeMonday = $jQuery("[id$=tpStartTimeMonday]")[0].control.get_timeView().getTime();
        var tpEndTimeMonday = $jQuery("[id$=tpEndTimeMonday]")[0].control.get_timeView().getTime();

        var rfvAvailableDays = $find($jQuery("[id$=cmbAvailableDays]")[0].id);

        rfvAvailableDays.get_checkedItems().forEach(function (item) {
            idlist = idlist.concat(item.get_value(), ',');
        });
        if ((idlist == null || idlist.trim() == "")) { // && (tpEndTime != null || tpStartTime != null)) {
            sender.innerText = 'Availability (Days) is required. ';
            //if (tpStartTime == null) {
            //    $jQuery("[id$=rfvStartTime]")[0].enabled = true;
            //}
            //if (tpEndTime == null) {
            //    $jQuery("[id$=rfvEndTime]")[0].enabled = true;
            //}
            args.IsValid = false;
        }
        //if ((idlist != null && idlist.trim() != "") && (tpStartTime == null || tpEndTime == null)) {
        //    if (tpStartTime == null) {
        //        $jQuery("[id$=rfvStartTime]")[0].enabled = true;
        //    }
        //    if (tpEndTime == null) {
        //        $jQuery("[id$=rfvEndTime]")[0].enabled = true;
        //    }
        //    sender.innerText = "";
        //    args.IsValid = false;
        //}
        //if (idlist == null && tpStartTime == null && tpEndTime == null) {
        //    $jQuery("[id$=rfvStartTime]")[0].enabled = false;
        //    $jQuery("[id$=rfvEndTime]")[0].enabled = false;
        //    sender.innerText = "";
        //}
    }

    function OnClientSelectedIndexChanged(sender, args) {
        if (areThereAnyChangesAtTheSelection(sender)) {
            __doPostBack('cmbAvailableDays', '');
        }
        else {
            return false;
        }
    }
    var oldSelectedIdList = [];

    function radComboBoxSelectedIdList(sender) {
        var selectedIdList = [];
        var combo = sender;
        var items = combo.get_items();
        var checkedIndices = items._parent._checkedIndices;
        var checkedIndicesCount = checkedIndices.length;
        for (var itemIndex = 0; itemIndex < checkedIndicesCount; itemIndex++) {
            var item = items.getItem(checkedIndices[itemIndex]);
            selectedIdList.push(item._properties._data.value);
        }
        return selectedIdList;
    }

    function areThereAnyChangesAtTheSelection(sender) {
        var hdnPreviousAgencyValues = $jQuery("[id$=hdnPreviousAvailabilityDaysValues]");
        if (hdnPreviousAgencyValues.val() != "" && hdnPreviousAgencyValues.val() != null && hdnPreviousAgencyValues.val() != undefined) {
            oldSelectedIdList = hdnPreviousAgencyValues.val().split(',');
        }
        var selectedIdList = radComboBoxSelectedIdList(sender);
        hdnPreviousAgencyValues.val(selectedIdList.join(","));
        var isTheCountOfEachSelectionEqual = (selectedIdList.length == oldSelectedIdList.length);
        if (isTheCountOfEachSelectionEqual == false)
            return true;

        var oldIdListMINUSNewIdList = $jQuery(oldSelectedIdList).not(selectedIdList).get();
        var newIdListMINUSOldIdList = $jQuery(selectedIdList).not(oldSelectedIdList).get();

        if (oldIdListMINUSNewIdList.length != 0 || newIdListMINUSOldIdList.length != 0)
            return true;

        return false;
    }

    //UAT-4120
    function OpenInstructorView(navUrl) {
        var win = window.open(navUrl, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        }
    }
</script>
<style type="text/css">

    .setLineHeight
    {
      line-height:11px !important;
      display:contents !important;
    }
</style>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Instructor/Preceptors
            </h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
        </div>
    </div>
    <div class="row bgLightGreen">
        <telerik:RadCaptcha ID="radCpatchaPassword" runat="server" CaptchaImage-TextChars="LettersAndNumbers"
            CaptchaImage-TextLength="10" Visible="false" Display="Dynamic" />
        <asp:Panel runat="server" ID="pnlTenant">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>
                            <asp:Label ID="lblTenant" runat="server" Text="Institution"></asp:Label></span><span
                                class="reqd">*</span>
                        <infs:WclComboBox ID="cmbTenant" runat="server" OnDataBound="cmbTenant_DataBound"
                            DataTextField="TenantName" DataValueField="TenantID" CausesValidation="false" AutoPostBack="true"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnSelectedIndexChanged="cmbTenant_SelectedIndexChanged"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                        <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="cmbTenant" InitialValue="--Select--" 
                                    Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                            </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the Account Activation">
                        <label id="Label2" class="cptn">Account Activated</label>
                        <asp:RadioButtonList ID="rbAccountActivated" runat="server" RepeatDirection="Horizontal"
                            CssClass="radio_list">
                            <asp:ListItem Text="Yes" Value="1" />
                            <asp:ListItem Text="No" Value="0" />
                            <asp:ListItem Text="All" Value="2" Selected="True" />
                        </asp:RadioButtonList>
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</div>

<div class="row">&nbsp;</div>
<div class="row">
    <div class="col-md-12">
        <div class="form-group col-md-12">
            <div class="row text-center">
                <infsu:CommandBar runat="server" ID="fsucManageInstructorCmdBar" ButtonPosition="Center" DisplayButtons="Submit,Extra,Cancel"
                    AutoPostbackButtons="Submit,Extra,Cancel" SubmitButtonText="Search" SubmitButtonIconClass="rbSearch"
                    CancelButtonText="Cancel" ExtraButtonText="Reset" ExtraButtonIconClass="rbUndo" DefaultPanel="pnlSearchFilters" ValidationGroup="grpFormSubmit"
                    OnSubmitClick="fsucManageInstructorCmdBar_SubmitClick" OnExtraClick="fsucManageInstructorCmdBar_ExtraClick"
                    OnCancelClick="fsucManageInstructorCmdBar_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                </infsu:CommandBar>
            </div>
        </div>
    </div>
</div>
<div class="container-fluid">
    <div class="row">
        <infs:WclGrid runat="server" ID="grdInstrctrPreceptr" AllowPaging="True" AutoGenerateColumns="False"
            AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="false" CellSpacing="0"
            EnableDefaultFeatures="True" ShowAllExportButtons="False" ShowExtraButtons="true"
            GridLines="Both" NonExportingColumns="EditCommandColumn,DeleteColumn" OnNeedDataSource="grdInstrctrPreceptr_NeedDataSource"
            OnInsertCommand="grdInstrctrPreceptr_InsertCommand" OnUpdateCommand="grdInstrctrPreceptr_UpdateCommand"
            OnDeleteCommand="grdInstrctrPreceptr_DeleteCommand"
            OnItemCommand="grdInstrctrPreceptr_ItemCommand" OnItemDataBound="grdInstrctrPreceptr_ItemDataBound"
            EnableLinqExpressions="false">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                HideStructureColumns="false" Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm">
                <Excel AutoFitImages="true" />
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ClientContactID,UserID,TokenID,IsRegistered"
                PagerStyle-Position="TopAndBottom">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Instructor/Preceptors"
                    ShowExportToPdfButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true"
                    ShowRefreshButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter Institution column"
                        HeaderText="Institution" SortExpression="TenantName" UniqueName="TenantName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column"
                        HeaderText="Name" SortExpression="Name" UniqueName="Name" AllowFiltering="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column"
                        HeaderText="Email" SortExpression="Email" UniqueName="Email">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Phone" FilterControlAltText="Filter Phone column"
                        HeaderText="Phone" SortExpression="Phone" UniqueName="Phone">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AccountActivated" FilterControlAltText="Filter AccountActivated column" AllowFiltering="true"
                        HeaderText="Account Activated" SortExpression="AccountActivated" UniqueName="AccountActivated">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ResendActivationLink" HeaderStyle-Width="15%">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnReSendActivationInvitationLink" CssClass="setLineHeight" ButtonType="LinkButton" CommandName="ResendActivationLink"
                                runat="server" Text="Resend Activation Link" ToolTip="Click here to resend activation invitation to instructor/preceptor.">
                            </telerik:RadButton>
                              <telerik:RadButton ID="btnResetInstructorPassword" CssClass="setLineHeight" ButtonType="LinkButton" CommandName="ResetInstructorPassword"
                                runat="server" Text="Reset Password" ToolTip="Click here to reset instructor/preceptor password.">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="LoginAsInstructor" >
                        <ItemTemplate>
                            <telerik:RadButton ID="btnLoginAsInstructor" CssClass="setLineHeight" ButtonType="LinkButton" CommandName="LoginAsInstructor"
                                runat="server" Text="Instructor/Preceptor Login" ToolTip="Click here to Login as instructor/preceptor." >
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this contact?"
                        Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                        EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" />
                    </telerik:GridEditCommandColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div runat="server" id="divEditBlock" visible="true">
                            <div class="col-md-12">
                                <h2 class="header-color">
                                    <asp:Label ID="lblEHAttr" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Instructor/Preceptors" : "Update Instructor/Preceptors" %>'
                                        runat="server" /></h2>
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="msgbox">
                                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel runat="server" ID="pnlInstructorPreceptor">
                                <div class="col-md-12">
                                    <div class="row bgLightGreen">
                                        <div class='form-group col-md-3'>
                                            <span class='cptn'>Name</span><span class="reqd">*</span>
                                            <infs:WclTextBox runat="server" ID="txtName" Text='<%# Eval("Name") %>'
                                                MaxLength="100" Width="100%" CssClass="form-control">
                                            </infs:WclTextBox>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                                    class="errmsg" Display="Dynamic" ErrorMessage="Name is required." ValidationGroup='grpInstrctrPerceptr' />
                                            </div>
                                        </div>
                                        <div class='form-group col-md-3'>
                                            <span class='cptn'>Email</span><span class="reqd">*</span>
                                            <infs:WclTextBox runat="server" ID="txtEmail" Text='<%# Eval("Email") %>'
                                                MaxLength="100" Width="100%" CssClass="form-control">
                                            </infs:WclTextBox>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="txtEmail"
                                                    class="errmsg" Display="Dynamic" ErrorMessage="Email is required." ValidationGroup='grpInstrctrPerceptr' />
                                                <asp:RegularExpressionValidator runat="server" ID="revEmailAddress" ControlToValidate="txtEmail"
                                                    Display="Dynamic" CssClass="errmsg" ErrorMessage="Email Address is not valid."
                                                    ValidationGroup="grpInstrctrPerceptr"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                            </div>
                                        </div>
                                        <div class='form-group col-md-3'>
                                            <span class='cptn'>Phone</span><span class="reqd">*</span>
                                            <div class="row">
                                                <div class="form-group col-md-11" style="padding-right: 0px;">
                                                    <infs:WclMaskedTextBox ID="txtPhone" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("Phone") %>'
                                                        Mask="(###)-###-####" Width="100%" CssClass="form-control" Visible='<%# (Container is GridEditFormInsertItem) ? false: !Convert.ToBoolean(Eval("IsInternationalPhone")) %>' />
                                                    <infs:WclTextBox ID="txtInternationalPhone" runat="server" MaxLength="15" Width="100%" CssClass="form-control" Visible='<%# (Container is GridEditFormInsertItem) ? false: Convert.ToBoolean(Eval("IsInternationalPhone")) %>'
                                                        Text='<%# (Container is GridEditFormInsertItem) ? String.Empty: Eval("Phone") %>' />
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvPhone" ControlToValidate="txtPhone"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Phone is required." ValidationGroup='grpInstrctrPerceptr' />

                                                        <asp:RequiredFieldValidator runat="server" ID="rfvTxtInternationalPhn" ControlToValidate="txtInternationalPhone"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Phone is required." ValidationGroup='grpInstrctrPerceptr' />
                                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revTxtMobilePrmyNonMasking" runat="server"
                                                            ValidationGroup="grpInstrctrPerceptr" CssClass="errmsg" ErrorMessage="Invalid phone number. This field only contains +, - and numbers." ControlToValidate="txtInternationalPhone"
                                                            ValidationExpression="(\d?)+(([-+]+?\d+)?)+([-+]?)+" />
                                                    </div>
                                                </div>
                                                <div class="form-group col-md-1">
                                                    <infs:WclCheckBox runat="server" ID="chkInternationalPhone" ToolTip="Check this box if you do not have an US Number." AutoPostBack="true" OnCheckedChanged="chkInternationalPhone_CheckedChanged"
                                                        CssClass="PhoneCheck" Checked='<%# (Container is GridEditFormInsertItem) ? false: Convert.ToBoolean(Eval("IsInternationalPhone")) %>'></infs:WclCheckBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='form-group col-md-3'>
                                            <span class='cptn'>Client Contact Type</span><span class="reqd">*</span>
                                            <infs:WclComboBox ID="cmbUserType" AutoPostBack="false" OnDataBound="cmbUserType_DataBound"
                                                DataTextField="Name" DataValueField="ClientContactTypeID" runat="server"
                                                ClientIDMode="Static" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                            </infs:WclComboBox>
                                            <div class="vldx">
                                                <asp:RequiredFieldValidator runat="server" ID="rfvUserPoints" ControlToValidate="cmbUserType"
                                                    InitialValue="--Select--"
                                                    Display="Dynamic" ValidationGroup="grpInstrctrPerceptr" CssClass="errmsg" Text="Client Contact Type is required."
                                                    Enabled="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row bgLightGreen">
                                        <div class='form-group col-md-3'>
                                            <span class='cptn'>Availability (Days)</span>
                                            <%-- <infs:WclComboBox ID="cmbAvailableDays" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                AutoPostBack="true"  
                                                DataTextField="Name" DataValueField="WeekDayID" runat="server" EnableViewState="true"
                                                ClientIDMode="Static" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                            </infs:WclComboBox>

                                            <div class='vldx'>
                                                <asp:CustomValidator ID="rfvAvailableDays" ErrorMessage=""
                                                    ClientValidationFunction="ValidateAvailableDays"
                                                    CssClass="errmsg"
                                                    Display="Dynamic" ClientIDMode="Static" ValidationGroup="grpInstrctrPerceptr" runat="server" SetFocusOnError="True"></asp:CustomValidator>
                                            </div>--%>

                                            <infs:WclComboBox ID="cmbAvailableDays" runat="server" AutoPostBack="false" DataTextField="Name" EnableCheckAllItemsCheckBox="true"
                                                OnClientDropDownClosed="OnClientSelectedIndexChanged"
                                                DataValueField="WeekDayID" OnItemDataBound="cmbAvailableDays_ItemDataBound"
                                                Filter="None" OnClientKeyPressing="openCmbBoxOnTab"
                                                Width="100%" OnSelectedIndexChanged="cmbAvailableDays_SelectedIndexChanged"
                                                CssClass="form-control" Skin="Silk"
                                                AutoSkinMode="false" CheckBoxes="true" CausesValidation="false" EmptyMessage="--Select--"
                                                CheckedItemsTexts="DisplayAllInInput">
                                            </infs:WclComboBox>
                                            <div class='vldx'>
                                                <%-- <asp:CustomValidator ID="rfvAvailableDays" ErrorMessage=""
                                                    ClientValidationFunction="ValidateAvailableDays"
                                                    CssClass="errmsg"
                                                    Display="Dynamic" ClientIDMode="Static" ValidationGroup="grpInstrctrPerceptr" runat="server" SetFocusOnError="True"></asp:CustomValidator>--%>
                                            </div>
                                            <%--  <asp:RequiredFieldValidator runat="server" ID="rfvcmbAvailableDays" ControlToValidate="cmbAvailableDays"
                                                Display="Dynamic" ValidationGroup="grpInstrctrPerceptr" CssClass="errmsg" Text="Days is required." Enabled="true" />--%>
                                        </div>
                                        <%--<div style="display: none" class='form-group col-md-3'>
                                            <span class='cptn'>Availability (Time)</span>
                                            <infs:WclTimePicker AutoPostBack="true" ID="tpStartTime" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="Start Time"
                                                runat="server"
                                                Width="100%" CssClass="form-control">
                                                <TimeView Interval="00:30:00"></TimeView>
                                            </infs:WclTimePicker>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvStartTime" ControlToValidate="tpStartTime"
                                                    Enabled="false"
                                                    class="errmsg" Display="Dynamic" ErrorMessage="Start time is required." ValidationGroup='grpInstrctrPerceptr' />
                                            </div>
                                            <div class="gclrPad"></div>
                                            <infs:WclTimePicker AutoPostBack="true" ID="tpEndTime" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="End Time"
                                                ClientEvents-OnPopupOpening="SetMinTime" runat="server"
                                                Width="100%" CssClass="form-control">
                                                <TimeView Interval="00:30:00"></TimeView>
                                            </infs:WclTimePicker>
                                            <div class='vldx'>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvEndTime" ControlToValidate="tpEndTime"
                                                    Enabled="false"
                                                    class="errmsg" Display="Dynamic" ErrorMessage="End time is required." ValidationGroup='grpInstrctrPerceptr' />
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                                <div runat="server" id="dvAvailabilityTime" class="col-md-12" visible="false">
                                    <div class="row bgLightGreen">
                                        <table class="table">

                                            <tbody>
                                                <tr>
                                                    <td><span class='cptn' style="margin-left: 1%">Days</span></td>
                                                    <td><span class='cptn' style="margin-left: 1%">Availability (Time)</span><span class="reqd">*</span></td>
                                                </tr>
                                                <div runat="server" id="dvMonday">
                                                    <tr>
                                                        <td><span class='cptn'>Monday</span></td>
                                                        <td>
                                                            <div class='form-group col-md-3'>
                                                                <infs:WclTimePicker AutoPostBack="false" ID="tpStartTimeMonday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="Start Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpStartTimeMonday" ControlToValidate="tpStartTimeMonday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="Start time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class="gclrPad"></div>
                                                                <infs:WclTimePicker AutoPostBack="false" ID="tpEndTimeMonday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="End Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpEndTimeMonday" ControlToValidate="tpEndTimeMonday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="End time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class='vldx'>
                                                                    <asp:Label ID="lblMondayTime_CCA_ID" runat="server" Visible="false" />
                                                                </div>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </div>
                                                <div runat="server" id="dvTuesday">
                                                    <tr>
                                                        <td><span class='cptn'>Tuesday</span></td>
                                                        <td>
                                                            <div class='form-group col-md-3'>
                                                                <infs:WclTimePicker AutoPostBack="false" ID="tpStartTimeTuesday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="Start Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpStartTimeTuesday" ControlToValidate="tpStartTimeTuesday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="Start time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class="gclrPad"></div>
                                                                <infs:WclTimePicker AutoPostBack="false" ID="tpEndTimeTuesday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="End Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpEndTimeTuesday" ControlToValidate="tpEndTimeTuesday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="End time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class='vldx'>
                                                                    <asp:Label ID="lblTuesdayTime_CCA_ID" runat="server" Visible="false" />
                                                                </div>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </div>
                                                <div runat="server" id="dvWednesday">
                                                    <tr>
                                                        <td><span class='cptn'>Wednesday</span></td>
                                                        <td>
                                                            <div class='form-group col-md-3'>
                                                                <infs:WclTimePicker AutoPostBack="false" ID="tpStartTimeWednesday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="Start Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpStartTimeWednesday" ControlToValidate="tpStartTimeWednesday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="Start time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class="gclrPad"></div>
                                                                <infs:WclTimePicker AutoPostBack="false" ID="tpEndTimeWednesday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="End Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpEndTimeWednesday" ControlToValidate="tpEndTimeWednesday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="End time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class='vldx'>
                                                                    <asp:Label ID="lblWednesdayTime_CCA_ID" runat="server" Visible="false" />
                                                                </div>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </div>
                                                <div runat="server" id="dvThr">
                                                    <tr>
                                                        <td><span class='cptn'>Thursday</span></td>
                                                        <td>
                                                            <div class='form-group col-md-3'>
                                                                <infs:WclTimePicker AutoPostBack="false" ID="tpStartTimeThrusday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="Start Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpStartTimeThrusday" ControlToValidate="tpStartTimeThrusday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="Start time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class="gclrPad"></div>
                                                                <infs:WclTimePicker AutoPostBack="false" ID="tpEndTimeThrusday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="End Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpEndTimeThrusday" ControlToValidate="tpEndTimeThrusday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="End time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class='vldx'>
                                                                    <asp:Label ID="lblThursdayTime_CCA_ID" runat="server" Visible="false" />
                                                                </div>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </div>
                                                <div runat="server" id="dvFriday">
                                                    <tr>
                                                        <td><span class='cptn'>Friday</span></td>
                                                        <td>
                                                            <div class='form-group col-md-3'>
                                                                <infs:WclTimePicker AutoPostBack="false" ID="tpStartTimeFriday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="Start Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpStartTimeFriday" ControlToValidate="tpStartTimeFriday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="Start time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class="gclrPad"></div>
                                                                <infs:WclTimePicker ID="tpEndTimeFriday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="End Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpEndTimeFriday" ControlToValidate="tpEndTimeFriday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="End time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class='vldx'>
                                                                    <asp:Label ID="lblFridayTime_CCA_ID" runat="server" Visible="false" />
                                                                </div>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </div>
                                                <div runat="server" id="dvSat">
                                                    <tr>
                                                        <td><span class='cptn'>Sataurday</span></td>
                                                        <td>
                                                            <div class='form-group col-md-3'>
                                                                <infs:WclTimePicker ID="tpStartTimeSat" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="Start Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpStartTimeSat" ControlToValidate="tpStartTimeSat"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="Start time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class="gclrPad"></div>
                                                                <infs:WclTimePicker ID="tpEndTimeSat" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="End Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpEndTimeSat" ControlToValidate="tpEndTimeSat"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="End time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class='vldx'>
                                                                    <asp:Label ID="lblSaturdayTime_CCA_ID" runat="server" Visible="false" />
                                                                </div>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </div>
                                                <div runat="server" id="dvSunday">
                                                    <tr>
                                                        <td><span class='cptn'>Sunday</span></td>
                                                        <td>
                                                            <div class='form-group col-md-3'>
                                                                <infs:WclTimePicker ID="tpStartTimeSunday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="Start Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpStartTimeSunday" ControlToValidate="tpStartTimeSunday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="Start time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class="gclrPad"></div>
                                                                <infs:WclTimePicker ID="tpEndTimeSunday" EnableScreenBoundaryDetection="true" DateInput-EmptyMessage="End Time"
                                                                    runat="server"
                                                                    Width="100%" CssClass="form-control">
                                                                    <TimeView Interval="00:30:00"></TimeView>
                                                                </infs:WclTimePicker>
                                                                <div class='vldx'>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvtpEndTimeSunday" ControlToValidate="tpEndTimeSunday"
                                                                        Enabled="false"
                                                                        class="errmsg" Display="Dynamic" ErrorMessage="End time is required." ValidationGroup='grpInstrctrPerceptr' />
                                                                </div>
                                                                <div class='vldx'>
                                                                    <asp:Label ID="lblSundayTime_CCA_ID" runat="server" Visible="false" />
                                                                </div>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </div>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="msgbox">
                                            <asp:Label ID="lblUploadDocumentsMsg" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <h2 class="header-color">Upload Document
                                    </h2>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                        <infs:WclGrid runat="server" ID="grdUploadDocuments" AllowPaging="false"
                                            PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                                            ShowClearFiltersButton="false"
                                            GridViewMode="AutoAddOnly" NonExportingColumns="EditCommandColumn, DeleteColumn"
                                            OnItemDataBound="grdUploadDocuments_ItemDataBound"
                                            OnNeedDataSource="grdUploadDocuments_NeedDataSource" OnItemCommand="grdUploadDocuments_ItemCommand"
                                            ShowAllExportButtons="false">
                                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="DocumentID,TempDocumentID,DocumentPath"
                                                AllowFilteringByColumn="false">
                                                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Upload New Document"
                                                    ShowRefreshButton="false"
                                                    ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="DocumentTypeName" FilterControlAltText="Filter DocumentTypeName column"
                                                        HeaderText="Document Type" SortExpression="DocumentTypeName" UniqueName="DocumentTypeName">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter DocumentName column"
                                                        HeaderText="Document Name" SortExpression="FileName" UniqueName="DocumentName">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                                                        HeaderText="Description" SortExpression="Description" UniqueName="Description">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                                        ConfirmText="Are you sure you want to delete this document?"
                                                        Text="Delete" UniqueName="DeleteColumn" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif">
                                                        <HeaderStyle CssClass="tplcohdr" />
                                                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridTemplateColumn UniqueName="DownloadDocument">
                                                        <ItemTemplate>
                                                            <a id="lnkDoc" runat="server" class="linkView">View Document</a>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <EditFormSettings EditFormType="Template">
                                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                    </EditColumn>
                                                    <FormTemplate>
                                                        <div class="col-md-12">
                                                            <h2 class="header-color">
                                                                <asp:Label ID="lblNewHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Upload New Document" : "Edit Document"%>'
                                                                    runat="server"></asp:Label>
                                                            </h2>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="msgbox">
                                                                    <asp:Label ID="lblUploadDocuments" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <asp:Panel runat="server" ID="pnlMNew">
                                                            <div class="col-md-12">
                                                                <div class="row bgLightGreen">
                                                                    <div class='form-group col-md-3'>
                                                                        <asp:Label ID="lblUserName" runat="server" Text="Document Type" CssClass="cptn"> </asp:Label><span
                                                                            class="reqd">*</span>
                                                                        <infs:WclComboBox ID="cmbDocumentType" OnDataBound="cmbDocumentType_DataBound" runat="server"
                                                                            AutoPostBack="false"
                                                                            DataTextField="Name" MaxHeight="200px" DataValueField="SharedSystemDocTypeID"
                                                                            Width="100%"
                                                                            CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                                        </infs:WclComboBox>
                                                                        <div class='vldx'>
                                                                            <asp:RequiredFieldValidator runat="server" ID="rfvDocType" ControlToValidate="cmbDocumentType"
                                                                                class="errmsg" ValidationGroup="grpUploadDoc" Display="Dynamic" ErrorMessage="Document Type is required."
                                                                                InitialValue="--Select--" />
                                                                        </div>
                                                                    </div>
                                                                    <div class='form-group col-md-3'>
                                                                        <asp:Label ID="lblDocUpload" runat="server" Text="Select Document" CssClass="cptn"></asp:Label><span
                                                                            class="reqd">*</span>
                                                                        <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="silk"
                                                                            MaxFileInputsCount="1"
                                                                            MultipleFileSelection="Disabled" OnClientFileSelected="clientFileSelected" OnClientValidationFailed="upl_OnClientValidationFailed"
                                                                            OnClientFileUploading="OnClientFileUploading"
                                                                            OnClientFileUploaded="OnClientFileUploaded"
                                                                            AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT">
                                                                            <Localization Select="Browse" />
                                                                        </infs:WclAsyncUpload>
                                                                    </div>
                                                                    <div class='form-group col-md-3'>
                                                                        <asp:Label ID="ldlDescription" runat="server" Text="Description" CssClass="cptn"> </asp:Label>
                                                                        <infs:WclTextBox runat="server" ID="txtDocDescription" MaxLength="500" Width="100%"
                                                                            CssClass="form-control">
                                                                        </infs:WclTextBox>
                                                                        <div class='vldx'>
                                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbDocumentType"
                                                                                class="errmsg" ValidationGroup="grpUploadDoc" Display="Dynamic" ErrorMessage="Document Type is required."
                                                                                InitialValue="--SELECT--" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                        </asp:Panel>
                                                        <div class="col-md-12 text-center">
                                                            <infsu:CommandBar ID="fsucCmdBarNew" runat="server" GridMode="true" ValidationGroup="grpUploadDoc"
                                                                GridInsertText="Add" GridUpdateText="Save" DefaultPanel="pnlMNew"
                                                                UseAutoSkinMode="False" ButtonSkin="Silk" />
                                                        </div>
                                                    </FormTemplate>
                                                </EditFormSettings>
                                            </MasterTableView>
                                        </infs:WclGrid>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-md-12 text-center">
                            <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlAttr"
                                GridInsertText="Save" GridUpdateText="Save"
                                ValidationGroup="grpInstrctrPerceptr" ExtraButtonIconClass="icnreset" UseAutoSkinMode="False"
                                ButtonSkin="Silk" />
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
</div>



<asp:HiddenField runat="server" ID="hdnPreviousAvailabilityDaysValues" />
