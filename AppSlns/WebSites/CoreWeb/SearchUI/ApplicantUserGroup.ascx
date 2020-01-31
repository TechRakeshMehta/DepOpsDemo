<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Search.Views.ApplicantUserGroup" CodeBehind="ApplicantUserGroup.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearch.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="uc" %>--%>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearchMultipleNodes.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="uc" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style>
    .radio_list label {
        font-size: 11px !important;
    }
</style>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">Manage Applicant User Group Mapping
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel runat="server" ID="pnlSearch">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <label for="<%= ddlTenantName.ClientID %>_Input" class="cptn">Institution</label><span class="reqd">*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                OnDataBound="ddlTenantName_DataBound" Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName" SetFocusOnError="true" role="alert"
                                    InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Select a User Group. The selected group's members' checkboxes will be marked in the grid below">
                        <label id="Label1" for="<%= ddlSearchUserGroup.ClientID %>_Input" class="cptn">Search User Group</label>
                        <infs:WclComboBox ID="ddlSearchUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID"
                            AutoPostBack="false" OnDataBound="ddlSearchUserGroup_DataBound" EmptyMessage="--Select--" Width="100%" OnClientItemChecked="OnClientItemChecked"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CheckBoxes="true" CssClass="form-control" Localization-CheckAllString="Select ALL" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                       <%-- <div class="vldx">
                            <asp:CustomValidator ID="cvUserGroup" CssClass="errmsg" Display="Dynamic" runat="server"
                                ErrorMessage="UserGroup(s) are required." ValidationGroup="grpFormSubmit"
                                ClientValidationFunction="ValidateUserGroup" ControlToValidate="ddlSearchUserGroup" ValidateEmptyText="true">
                            </asp:CustomValidator>

                        </div>--%>
                    </div>
                    <%--<div class='form-group col-md-3'> &nbsp;</div>--%>
                    <div class='form-group col-md-3' title="Select a User Group. The selected group's members' checkboxes will be marked in the grid below">
                        <label id="lblUserGrp" for="<%= ddlUserGroup.ClientID %>_Input" class="cptn">Assign To User Group</label><span class="reqd">*</span>
                        <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID"
                            AutoPostBack="true" OnDataBound="ddlUserGroup_DataBound" OnSelectedIndexChanged="ddlUserGroup_SelectedIndexChanged" Width="100%"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvUserGroup" ControlToValidate="ddlUserGroup" role="alert"
                                InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpFormSubmit" SetFocusOnError="true"
                                Text="User Group is required." />
                        </div>
                    </div>
                </div>
            </div>
            <uc:CustomAttributeLoaderSearch ID="ucCustomAttributeLoaderSearch" runat="server" />
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered User ID">
                        <label id="lblUserID" class="cptn">User ID</label>
                        <infs:WclNumericTextBox EnableAriaSupport="true" aria-labelledby="lblUserID" ShowSpinButtons="false" Type="Number" ID="txtUserID" MaxValue="2147483647"
                            runat="server" InvalidStyleDuration="100" MinValue="1" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                        </infs:WclNumericTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <label id="lblFName" class="cptn">Applicant First Name</label>
                        <infs:WclTextBox ID="txtFirstName" EnableAriaSupport="true" aria-labelledby="lblFName" runat="server" AutoSkinMode="false" Width="100%" CssClass="form-control" Skin="Silk">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <label id="lblLName" class="cptn">Applicant Last Name</label>
                        <infs:WclTextBox ID="txtLastName" EnableAriaSupport="true" aria-labelledby="lblLName" runat="server" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered email address">
                        <label id="lblEmailAddress" class="cptn">Email Address</label>
                        <infs:WclTextBox EnableAriaSupport="true" aria-labelledby="lblEmailAddress" ID="txtEmail"  runat="server"  Skin="Silk" AutoSkinMode="false" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                        <div class="vldx">
                        <asp:RegularExpressionValidator Display = "Dynamic" CssClass="errmsg" ControlToValidate = "txtEmail" ID="RegularExpressionValidator2" ValidationExpression = "^[\s\S]{3,}$" runat="server" ErrorMessage="Minimum 3 characters required." ValidationGroup="grpFormSubmit" SetFocusOnError="true"></asp:RegularExpressionValidator>
                       </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div id="divSSN" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered SSN or ID Number">
                            <label id="lblSSN" class="cptn">SSN/ID Number</label>
                            <infs:WclMaskedTextBox runat="server" ID="txtSSN" EnableAriaSupport="true" aria-labelledby="lblSSN" Mask="aaa-aa-aaaa" Skin="Silk" AutoSkinMode="false" Width="100%" CssClass="form-control" />
                        </div>
                    </div>
                    <div id="divDOB" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered Date of Birth">
                            <label for="<%= dpkrDOB.ClientID %>_dateInput" class="cptn">Date of Birth</label>
                            <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date" EnableAriaSupport="true"
                                DateInput-DateFormat="MM/dd/yyyy" Width="100%" CssClass="form-control">
                                <Calendar EnableAriaSupport="true" EnableKeyboardNavigation="true"></Calendar>
                                <ClientEvents OnPopupClosing="OnCalenderClosing" />
                                <DateInput EnableAriaSupport="true" SelectionOnFocus="CaretToBeginning"></DateInput>
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    
                    <div class="form-group col-md-3">
                        <label for="<%= dpOrderCreatedFrom.ClientID %>_dateInput" class="cptn">Order Created From</label>
                        <infs:WclDatePicker ID="dpOrderCreatedFrom" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnDateSelected="CorrectFrmToCrtdDate" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                            <Calendar EnableAriaSupport="true" EnableKeyboardNavigation="true"></Calendar>
                            <ClientEvents OnPopupClosing="OnCalenderClosing" />
                            <DateInput EnableAriaSupport="true" SelectionOnFocus="CaretToBeginning"></DateInput>
                        </infs:WclDatePicker>
                    </div>
                    <div class="form-group col-md-3">
                        <label for="<%= dpOrderCreatedTo.ClientID %>_dateInput" class="cptn">Order Created To</label>
                        <infs:WclDatePicker ID="dpOrderCreatedTo" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinDate" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                            <Calendar EnableAriaSupport="true" EnableKeyboardNavigation="true"></Calendar>
                            <ClientEvents OnPopupClosing="OnCalenderClosing" />
                            <DateInput EnableAriaSupport="true" SelectionOnFocus="CaretToBeginning"></DateInput>
                        </infs:WclDatePicker>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Select &#34All&#34 to view all subscriptions per the other parameters or &#34Archived&#34 to view only archived subscriptions or &#34Active&#34 to view only non archived subscriptions">
                        <%--<span class="cptn">Subscription Archive State</span>--%>
                        <label id="lblArchiveState" class="cptn">Subscription Archive State</label>
                        <asp:RadioButtonList ID="rbSubscriptionState" runat="server" RepeatDirection="Horizontal"
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="form-control" Width="100%">
                        </asp:RadioButtonList>
                    </div>
                     <div class='form-group col-md-5' title="Select &#34All&#34 to view all applicants per the other parameters or &#34Selected User Group&#34 to view only the applicants who are in the chosen user group per the other parameters">
                        <%--<span id="spnResult" class="cptn">Result</span>--%>
                        <label id="lblResult" class="cptn">Result</label>
                        <asp:RadioButtonList ID="rbtnResults" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="false" Skin="Silk" AutoSkinMode="false" Width="100%" CssClass="form-control">
                            <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Selected User Group" Value="1"></asp:ListItem>
                            <asp:ListItem Text="User Group Not Assigned" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                </div>
            </div>
        </asp:Panel>
    </div>
    <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel,Clear"
        AutoPostbackButtons="Submit,Save,Cancel,Clear" SubmitButtonText="Reset" SubmitButtonIconClass="rbUndo" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
        CancelButtonText="Cancel" OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click"
        OnCancelClick="CmdBarCancel_Click" ClearButtonText="Confirm Updates" OnClearClick="btnAssign_Click"
        ClearButtonIconClass="" UseAutoSkinMode="false" ButtonSkin="Silk">
    </infsu:CommandBar>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdApplicantSearchData" AllowCustomPaging="true" EnableAriaSupport="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            OnNeedDataSource="grdApplicantSearchData_NeedDataSource" OnItemCommand="grdApplicantSearchData_ItemCommand"
            OnSortCommand="grdApplicantSearchData_SortCommand" EnableLinqExpressions="false"
            NonExportingColumns="AssignItems,SSN" OnItemDataBound="grdApplicantSearchData_ItemDataBound"
            ShowClearFiltersButton="false">
            <ClientSettings EnableRowHoverStyle="true" ClientEvents-OnGridCreated="onGridCreated">
                <Selecting AllowRowSelect="true"></Selecting>
                <ClientEvents OnGridCreated="GridCreated" />
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId,IsUserGroupMatching"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="AssignItems" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectItem" runat="server" Checked='<%#Convert.ToBoolean(Eval("IsUserGroupMatching")) %>' UserID='<%# Eval("OrganizationUserId") %>'
                                onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectVerItem_CheckedChanged" />
                            <asp:Label ID="lblIsUserGroup" runat="server" Text='<%#Eval("IsUserGroupMatching") %>'
                                Visible="false"></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="OrganizationUserId" HeaderText="User ID" SortExpression="OrganizationUserId"
                        HeaderTooltip="This column displays the User ID for each record in the grid"
                        UniqueName="UserID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="Applicant First Name"
                        SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Applicant Last Name"
                        SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstituteName" HeaderText="Institution" AllowSorting="false"
                        SortExpression="InstituteName" UniqueName="Institution" HeaderTooltip="This column displays the Institution for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress"
                        UniqueName="EmailAddress" HeaderTooltip="This column displays the applicant's email address for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="DateOfBirth" HeaderText="Date of Birth" SortExpression="DateOfBirth"
                        UniqueName="DateOfBirth" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid" DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="UserGroups" HeaderText="User Group"
                        SortExpression="UserGroups" UniqueName="UserGroups" HeaderTooltip="This column displays any user group(s) to which the applicant belongs for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        UniqueName="SSN" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN" Display="false"
                        UniqueName="_SSN" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstitutionHierarchy" HeaderText="Institution Hierarchy"
                        ItemStyle-Width="250px" AllowSorting="false" SortExpression="InstitutionHierarchy"
                        UniqueName="InstitutionHierarchy" HeaderTooltip="This column displays the institution hierarchy for each record in the grid">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>


<asp:HiddenField ID="hdnTotalUsersAssigned" runat="server" />
<asp:HiddenField ID="hdnTotalUsersUnassigned" runat="server" />
<script type="text/javascript">
    //    $jQuery(document).ready(function () {
    //        
    //    });

    function CheckAll(id) {
        var masterTable = $find("<%= grdApplicantSearchData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdApplicantSearchData.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    var minDate = new Date("01/01/1980");
    function CorrectFrmToCrtdDate(picker) {
        //debugger;
        var date1 = $jQuery("[id$=dpOrderCreatedFrom]")[0].control.get_selectedDate();
        var date2 = $jQuery("[id$=dpOrderCreatedTo]")[0].control.get_selectedDate();
        if (date1 != null && date2 != null) {
            if (date1 > date2)
                $jQuery("[id$=dpOrderCreatedTo]")[0].control.set_selectedDate(null);
        }
    }

    function SetMinDate(picker) {
        //debugger;
        var date = $jQuery("[id$=dpOrderCreatedFrom]")[0].control.get_selectedDate();
        if (date != null) {
            picker.set_minDate(date);
        }
        else {
            picker.set_minDate(minDate);
        }
    }

    function GridCreated(sender, eventArgs) {
        var totalUsersAssigned = $jQuery("[id$=hdnTotalUsersAssigned]").val();
        var IsUnAssigned = $jQuery("[id$=hdnTotalUsersUnassigned]").val();
        var pagerDiv = $jQuery('.rgWrap.rgInfoPart');
        if (totalUsersAssigned != "NA") {
            //UAT-2759
            if (IsUnAssigned == "UnAssigned") {
                if (pagerDiv != undefined && pagerDiv != null && pagerDiv.length > 0) {
                    pagerDiv.parent().append("<div id='divUsers' tabindex='0' class='rgWrap rgInfoPart' style = 'float : right;font-weight:bold;'>"
                        + totalUsersAssigned + " User(s) unassigned</div>");
                    $jQuery('#divUsers').append("<div style = 'padding-left :15px;float : right;font-weight:normal'>|</div>");
                }
            } //Ends UAT-2759
            else {
                if (pagerDiv != undefined && pagerDiv != null && pagerDiv.length > 0) {
                    pagerDiv.parent().append("<div id='divUsers' tabindex='0' class='rgWrap rgInfoPart' style = 'float : right;font-weight:bold;'>"
                        + totalUsersAssigned + " User(s) assigned</div>");
                    $jQuery('#divUsers').append("<div style = 'padding-left :15px;float : right;font-weight:normal'>|</div>");
                }
            }
        }
    }
    function pageLoad() { 

        //UAT-1955
        //Starting Accessibility Code
        $jQuery("[id$=ddlTenantName]").attr("tabindex", "0");

        var rbtnResults = '<%=rbtnResults.UniqueID%>';
        $jQuery("input[name='" + rbtnResults + "']:radio").each(function (index) {
            $jQuery(this).attr('aria-describedby', 'lblResult');
        });

        var rbSubscriptionState = '<%=rbSubscriptionState.UniqueID%>';
        $jQuery("input[name='" + rbSubscriptionState + "']:radio").each(function (index) {
            $jQuery(this).attr('aria-describedby', 'lblArchiveState');
        });

        //$jQuery("[id$=grdApplicantSearchData]").find("th").each(function (element) {
        //    if ($jQuery(this).text() != "" && $jQuery(this).text() != undefined && $jQuery(this).text().length > 1) {
        //        $jQuery(this).attr("tabindex", "0");
        //    }
        //});

        $jQuery(".rgHeader").each(function () {
            if (this.innerText == 'Institution' || this.innerText == 'User Group' || this.innerText == 'Screening Packages' || this.innerText == 'Institution Hierarchy') {
                //debugger;
                this.tabIndex = 0;
            }
        });


        //UAT-1946
        $jQuery("[id$=ddlTenantName_Input]").attr('role', 'combobox');
        $jQuery("[id$=ddlUserGroup_Input]").attr('role', 'combobox');


        $jQuery("div[id*=ucCustomAttributeLoaderSearch] .col-md-3 input:visible").each(function (i, e) {
            var input = $jQuery(this);
            var relatedSpan = $jQuery(input).parents('.col-md-3').children(0);
            var relatedSpanId = $jQuery(relatedSpan).attr('id');

            if ($jQuery(input).attr('type') != 'radio') {
                $jQuery(input).attr('aria-labelledBy', relatedSpanId);
            }
        });

        $jQuery('[id$=fsucCmdBarButton_btnSave_input]').attr('title', 'Click to search users per the criteria entered above');
        $jQuery('[id$=fsucCmdBarButton_btnSubmit_input]').attr('title', 'Click to remove all values entered in the search criteria above');
        $jQuery('[id$=fsucCmdBarButton_btnCancel_input]').attr('title', 'Click to cancel. Any data entered will not be saved');

        //Adding title to Select All checkbox
        $jQuery('[id$=chkSelectAll]').attr('title', 'Select all users on all active page(s)');

        //Adding title to Grid checkbox(s):
        $jQuery('[id$=chkSelectItem]').each(function () {
            var userID = $jQuery(this).parent().attr('UserID');
            $jQuery(this).attr('title', 'Click to select user, For user ID ' + userID);
        });

        //END Accessibility Code
    }

    function ValidatorUpdateDisplay(val) {
        if (typeof (val.display) == "string") {
            if (val.display == "None") {
                return;
            }
            if (val.display == "Dynamic") {
                val.style.display = val.isvalid ? "none" : "inline";
                if (!val.isvalid) {
                    $jQuery("[id$=" + val.controltovalidate + "_Input]").focus();
                }
                return;
            }
        }
        val.style.visibility = val.isvalid ? "hidden" : "visible";
        if (val.isvalid) {
            document.getElementById(val.controltovalidate).style.border = '1px solid #333';
        }
        else {
            document.getElementById(val.controltovalidate).style.border = '1px solid red';
        }
    }

    function OnClientItemChecked(sender, args) {
        if (sender.get_checkedItems().length == 0) {
            sender.clearSelection();
            sender.set_emptyMessage("--SELECT--");
        }
    }
</script>
