<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupportPortalSearch.ascx.cs" Inherits="CoreWeb.SearchUI.Views.SupportPortalSearch" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxSupportPortalSearch">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script type="text/javascript">
    function grd_rwDbClick(s, e) {
        //debugger;
        var _id = "btnSupportPortalDetail";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }     
    }
</script>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">
                <asp:Label ID="lblSupportPortalSearch" runat="server" Text="Manage Support Portal Search"></asp:Label>
            </h2>
        </div>
    </div>


    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel runat="server" ID="pnlSearchFilters">
            <div class="col-md-12">
                <div class="row">
                    <div id="dvTenant">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class='cptn'>Institution</span><span class="reqd">*</span>
                            <infs:WclComboBox ID="cmbTenantName" runat="server" DataTextField="TenantName" DataValueField="TenantID"                               
                                CausesValidation="false" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains" Width="100%"
                                CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnClientKeyPressing="openCmbBoxOnTab">
                                <Localization CheckAllString="All" />
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="cmbTenantName"
                                    Display="Dynamic" CssClass="errmsg" Text="Institution is required." ValidationGroup="grpFormSubmit" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Username">
                        <label id="Label1" class="cptn">Username</label>
                        <infs:WclTextBox ID="txtuserName" aria-labelledby="lblUName" EnableAriaSupport="true" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <label id="lblFName" class="cptn">First Name</label>
                        <infs:WclTextBox ID="txtFirstName" aria-labelledby="lblFName" EnableAriaSupport="true" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <label id="lblLName" class="cptn">Last Name</label>
                        <infs:WclTextBox ID="txtLastName" EnableAriaSupport="true" aria-labelledby="lblLName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered SSN or ID Number">
                        <label id="lblSSN" class="cptn">SSN/ID Number</label>
                        <infs:WclMaskedTextBox EnableAriaSupport="true" aria-labelledby="lblSSN" ID="txtSSN" runat="server" MaxLength="10" Width="100%" CssClass="form-control"
                            Mask="aaa-aa-aaaa">
                        </infs:WclMaskedTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered Date of Birth">
                        <label for="<%= dpkrDOB.ClientID %>_dateInput" class="cptn">Date of Birth</label>
                        <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date"
                            Width="100%" CssClass="form-control" DateInput-EnableAriaSupport="true" DateInput-SelectionOnFocus="CaretToBeginning"
                            ClientEvents-OnPopupClosing="OnCalenderClosing" EnableAriaSupport="true"
                            DateInput-DateFormat="MM/dd/yyyy">
                            <Calendar EnableKeyboardNavigation="true" EnableAriaSupport="true"></Calendar>
                        </infs:WclDatePicker>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered Email Address">
                        <label id="lblEmailAddress" class="cptn">Email Address</label>
                        <infs:WclTextBox ID="txtEmailAddress" EnableAriaSupport="true" aria-labelledby="lblEmailAddress" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the selected user type">
                        <label id="lblUserType" class="cptn">User Type</label>
                        <infs:WclComboBox ID="ddlUserType" runat="server" CheckBoxes="true" EmptyMessage="--SELECT--"
                            AutoPostBack="false" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" DataTextField="Value" DataValueField="Key">
                        </infs:WclComboBox>
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

            <%--    <div class="col-md-12">
                <div class="row">
                     <div class='form-group col-md-3' title="Include search results to the Client Admin" style="display:none">
                        <label id="lblClientAdmin" class="cptn">Include Client Admin(s) in search</label>
                        <asp:RadioButtonList ID="rbClientAdmin" runat="server" RepeatDirection="Horizontal"
                            CssClass="radio_list">
                            <asp:ListItem Text="Yes" Value="1" />
                            <asp:ListItem Text="No" Value="0" Selected="True" />                           
                        </asp:RadioButtonList>
                        <div class='sxroend'>
                        </div>
                    </div>
                    </div>
                </div>--%>            
        </asp:Panel>
    </div>

    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-12">
                <div class="row text-center">
                    <infsu:CommandBar runat="server" ID="fsucSupportPortalCmdBar" ButtonPosition="Center" DisplayButtons="Submit,Extra,Cancel"
                        AutoPostbackButtons="Submit,Extra,Cancel" SubmitButtonText="Search" SubmitButtonIconClass="rbSearch"
                        CancelButtonText="Cancel" ExtraButtonText="Reset" ExtraButtonIconClass="rbUndo" DefaultPanel="pnlSearchFilters" ValidationGroup="grpFormSubmit"
                        OnSubmitClick="fsucSupportPortalCmdBar_SubmitClick" OnExtraClick="fsucSupportPortalCmdBar_ExtraClick"
                        OnCancelClick="fsucSupportPortalCmdBar_CancelClick" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>

    </div>

</div>

<div>
    <infs:WclGrid runat="server" ID="grdSupportPortal" AutoGenerateColumns="false" CssClass="gridhover" OnNeedDataSource="grdSupportPortal_NeedDataSource"
        GridLines="Both" AutoSkinMode="true" ShowClearFiltersButton="false" AllowCustomPaging="true" AllowFilteringByColumn="false" AllowSorting="True"
        EnableLinqExpressions="false" ClientSettings-AllowKeyboardNavigation="true" EnableAriaSupport="true" OnItemCommand="grdSupportPortal_ItemCommand" OnSortCommand="grdSupportPortal_SortCommand"
        NonExportingColumns="ViewDetail,UserID,TenantID" OnItemDataBound="grdSupportPortal_ItemDataBound">

        <ClientSettings EnableRowHoverStyle="true">
            <ClientEvents OnRowDblClick="grd_rwDbClick" />
            <Selecting AllowRowSelect="true"></Selecting>
        </ClientSettings>
        <GroupingSettings CaseSensitive="false" />
        <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
            Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
            ExportOnlyData="true" IgnorePaging="true">
        </ExportSettings>
        <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId,TenantID,UserID,ClientContactID" ClientDataKeyNames="OrganizationUserId,TenantID,UserID,UserType,ClientContactID" AllowFilteringByColumn="false">
            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
            <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
            </RowIndicatorColumn>
            <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
            </ExpandCollapseColumn>
            <Columns>
                <telerik:GridBoundColumn DataField="UserID" HeaderText="UserID" Display="false"
                    ItemStyle-Width="250px" AllowSorting="false" UniqueName="UserID">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ClientContactID" HeaderText="ClientContactID" Display="false"
                    ItemStyle-Width="250px" AllowSorting="false" UniqueName="ClientContactID">
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="InstitutionName" HeaderText="Institution"
                    SortExpression="InstitutionName" UniqueName="InstitutionName" HeaderTooltip="This column displays the Institution for each record in the grid">
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="IsInMultipleInstitutions" HeaderText="Additional Institution"
                    SortExpression="IsInMultipleInstitutions" UniqueName="IsInMultipleInstitutions" HeaderTooltip="This column displays whether the applicant is in multiple Institution or not?">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ApplicantUserName" HeaderText="Username"
                    SortExpression="ApplicantUserName" UniqueName="ApplicantUserName" HeaderTooltip="This column displays the Username for each record in the grid">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="First Name"
                    SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the first name for each record in the grid">
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Last Name"
                    SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the last name for each record in the grid">
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="UserType" HeaderText="User Type" SortExpression="UserType"
                    HeaderTooltip="This column displays the user type for each record in the grid"
                    UniqueName="UserType">
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                    HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid"
                    UniqueName="ApplicantSSN">
                </telerik:GridBoundColumn>

                <telerik:GridDateTimeColumn DataField="DOB" HeaderText="Date of Birth" SortExpression="DOB"
                    UniqueName="DOB" DataFormatString="{0:MM/dd/yyyy}" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid">
                </telerik:GridDateTimeColumn>
                <telerik:GridDateTimeColumn DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress"
                    UniqueName="EmailAddress" HeaderTooltip="This column displays the applicant's Email Address for each record in the grid">
                </telerik:GridDateTimeColumn>

                <telerik:GridBoundColumn DataField="ApplicantAccountActivated" HeaderText="Account Activated"
                    SortExpression="ApplicantAccountActivated" UniqueName="ApplicantAccountActivated" HeaderTooltip="This column displays the account of applicant is activated or not?">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TenantID" HeaderText="TenantID" Display="false"
                    ItemStyle-Width="250px" AllowSorting="false" UniqueName="TenantID">
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" HeaderStyle-Width="10%">
                    <ItemTemplate>
                        <telerik:RadButton ID="btnSupportPortalDetail" ButtonType="LinkButton" CommandName="ViewDetail"
                            runat="server" Text="View Detail" ToolTip="Click here to view details.">
                        </telerik:RadButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
        </MasterTableView>
        <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
        <FilterMenu EnableImageSprites="False">
        </FilterMenu>
    </infs:WclGrid>
</div>

