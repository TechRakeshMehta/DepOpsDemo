<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StudentBucketAssignment.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.StudentBucketAssignment" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="CustomAttributeLoaderSearch" Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearchMultipleNodes.ascx" %>
<%@ Register Src="~/ClinicalRotation/UserControl/SharedUserCustomAttributeForm.ascx"
    TagPrefix="ucRotation" TagName="CustomAttributes" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchy" Src="~/AgencyHierarchy/UserControls/AgencyHierarchySelection.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotation.js" ResourceType="JavaScript" />
    
<infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    .RadMenu .rmItem .rmTemplate {
        background-color: #4382c2 !important;
    }

    #menuDiv {
        margin: 0 auto;
        width: 50%;
        padding-top: 5px;
    }

        #menuDiv ul ul li:first-child {
            border-radius: 10px 10px 0px 0px;
        }

        #menuDiv ul ul {
            border-radius: 10px;
        }

            #menuDiv ul ul li:last-child {
                border-radius: 0px 0px 10px 10px;
            }

            #menuDiv ul ul li.rmFirst.rmLast {
                border-radius: 10px 10px 10px 10px;
            }

    .btn {
        width: 100%;
        text-align: left;
    }

    .RadMenu .rmGroup .rmText {
        padding: 0px;
        margin: 0px;
    }
    /*.RadMenu .rmItem .rmTemplate, .RadToolTip_Default .rtWrapper .rtWrapperTopRight, .RadToolTip_Default .rtWrapper .rtWrapperBottomLeft, .RadToolTip_Default .rtWrapper .rtWrapperBottomRight, .RadToolTip_Default .rtWrapper .rtWrapperTopCenter, .RadToolTip_Default .rtWrapper .rtWrapperBottomCenter, .RadToolTip_Default table.rtShadow .rtWrapperTopLeft, .RadToolTip_Default table.rtShadow .rtWrapperTopRight, .RadToolTip_Default table.rtShadow .rtWrapperBottomLeft, .RadToolTip_Default table.rtShadow .rtWrapperBottomRight, .RadToolTip_Default table.rtShadow .rtWrapperTopCenter, .RadToolTip_Default table.rtShadow .rtWrapperBottomCenter, .RadToolTip_Default .rtCloseButton {
        background-image: none !important;
    }*/
</style>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Student Bulk Assignment
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel runat="server" ID="pnlSearch">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" DataTextField="TenantName" DataValueField="TenantID" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged" OnDataBound="ddlTenantName_DataBound"
                                Enabled="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Select a User Group. The selected group's members' checkboxes will be marked in the grid below">
                        <span class="cptn">User Group</span>
                        <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID"
                            AutoPostBack="true" OnDataBound="ddlUserGroup_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" 
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
            <uc:CustomAttributeLoaderSearch ID="ucCustomAttributeLoaderSearch" runat="server" />
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered User ID">
                        <span class="cptn">User ID</span>
                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Number" ID="txtUserID" MaxValue="2147483647"
                            runat="server" InvalidStyleDuration="100" MinValue="1" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                        </infs:WclNumericTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <span class="cptn">Applicant First Name</span>
                        <infs:WclTextBox ID="txtFirstName" runat="server" AutoSkinMode="false" Width="100%" CssClass="form-control" Skin="Silk">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <span class="cptn">Applicant Last Name</span>
                        <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered email address">
                        <span class="cptn">Email Address</span>
                        <infs:WclTextBox ID="txtEmail" runat="server" Skin="Silk" AutoSkinMode="false" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div id="divSSN" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered SSN or ID Number">
                            <span class="cptn">SSN/ID Number</span>
                            <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="aaa-aa-aaaa" Skin="Silk" AutoSkinMode="false" Width="100%" CssClass="form-control" />
                        </div>
                    </div>
                    <div id="divDOB" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered Date of Birth">
                            <span class="cptn">Date of Birth</span>
                            <infs:WclDatePicker ID="dpkrDOB" runat="server" DateInput-EmptyMessage="Select a date"
                                DateInput-DateFormat="MM/dd/yyyy" Width="100%" CssClass="form-control">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <%--    <div class='form-group col-md-3' title="Select &#34All&#34 to view all applicants per the other parameters or &#34Selected User Group&#34 to view only the applicants who are in the chosen user group per the other parameters">
                        <span class="cptn">Result</span>
                        <asp:RadioButtonList ID="rbtnResults" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="false" Skin="Silk" AutoSkinMode="false" Width="100%" CssClass="form-control">
                            <asp:ListItem Text="All" Value="false" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Selected User Group" Value="true"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="form-group col-md-3">
                        <span class="cptn">Order Created From</span>
                        <infs:WclDatePicker ID="dpOrderCreatedFrom" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnDateSelected="CorrectFrmToCrtdDate" Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>--%>
                </div>
            </div>
            <%--         <div class="col-md-12">
                <div class="row">
                    <div class="form-group col-md-3">
                        <span class="cptn">Order Created To</span>
                        <infs:WclDatePicker ID="dpOrderCreatedTo" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinDate" Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Select &#34All&#34 to view all subscriptions per the other parameters or &#34Archived&#34 to view only archived subscriptions or &#34Active&#34 to view only non archived subscriptions">
                        <span class="cptn">Subscription Archive State</span>
                        <asp:RadioButtonList ID="rbSubscriptionState" runat="server" RepeatDirection="Horizontal"
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="form-control"
                            AutoPostBack="true" Width="100%">
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>--%>

            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Select a Agency whose data you want to view.">
                        <%--<span class="cptn">Agency</span>
                        <infs:WclComboBox ID="ddlAgency" runat="server" DataTextField="AgencyName" DataValueField="AgencyID"
                            AutoPostBack="false" OnDataBound="ddlAgency_DataBound" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>--%>
                        <div style="margin-top:5%">
                            <uc:AgencyHierarchy ID="ucAgencyHierarchy" runat="server" />
                        </div>
                        
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Complio ID">
                        <span class="cptn">Complio ID</span>

                        <infs:WclTextBox ID="txtComplioId" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered rotation name">
                        <span class="cptn">Rotation ID/Name</span>

                        <infs:WclTextBox ID="txtRotationName" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Type/Specialty">
                        <span class='cptn'>Type/Specialty</span>

                        <infs:WclTextBox ID="txtTypeSpecialty" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered department">
                        <span class="cptn">Department</span>

                        <infs:WclTextBox ID="txtDepartment" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered program">
                        <span class="cptn">Program</span>

                        <infs:WclTextBox ID="txtProgram" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered course">
                        <span class="cptn">Course</span>

                        <infs:WclTextBox ID="txtCourse" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the selected term">
                        <span class="cptn">Term</span>

                        <infs:WclTextBox ID="txtTerm" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered Unit/Floor or Location">
                        <span class="cptn">Unit/Floor or Location</span>

                        <infs:WclTextBox ID="txtUnit" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Students">
                        <span class="cptn"># of Students</span>

                        <infs:WclNumericTextBox ID="txtStudents" runat="server" NumberFormat-DecimalDigits="2"
                            Width="100%" CssClass="form-control">
                        </infs:WclNumericTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Recommended Hours">
                        <span class="cptn"># of Recommended Hours</span>

                        <infs:WclNumericTextBox ID="txtRecommendedHrs" runat="server" NumberFormat-DecimalDigits="2"
                            Width="100%" CssClass="form-control">
                        </infs:WclNumericTextBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the selected days">
                        <span class="cptn">Days</span>

                        <infs:WclComboBox ID="ddlDays" runat="server" CheckBoxes="true" EmptyMessage="--SELECT--"
                            DataValueField="WeekDayID" DataTextField="Name" Width="100%" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>

                    <div class='form-group col-md-3' title="Restrict search results to the entered shift">
                        <span class="cptn">Shift</span>

                        <infs:WclTextBox ID="txtShift" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>


                </div>

            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered time range">
                        <span class="cptn">Time</span>

                        <infs:WclTimePicker ID="tpStartTime" runat="server" TimeView-Height="300px" Width="100%"
                            CssClass="form-control">
                            <TimeView CssClass="calanderFontSetting" Interval="00:15:00"></TimeView>
                        </infs:WclTimePicker>
                        <div class="gclrPad"></div>
                        <infs:WclTimePicker ID="tpEndTime" runat="server" TimeView-Height="300px" Width="100%"
                            CssClass="form-control">
                            <TimeView CssClass="calanderFontSetting" Interval="00:15:00"></TimeView>
                        </infs:WclTimePicker>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered start date">
                        <span class="cptn">Start Date</span>

                        <infs:WclDatePicker ID="dpStartDate" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinDate" ClientEvents-OnDateSelected="CorrectStartToEndDate"
                            Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class="form-group col-md-3" title="Restrict search results to the entered end date">
                        <span class="cptn">End Date</span>

                        <infs:WclDatePicker ID="dpEndDate" runat="server" DateInput-EmptyMessage="Select a date"
                            ClientEvents-OnPopupOpening="SetMinEndDate" Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected Instructor/Preceptor">
                        <span class="cptn">Instructor/Preceptor</span>

                        <infs:WclComboBox ID="ddlContacts" runat="server" EmptyMessage="--SELECT--" CheckBoxes="true"
                            DataValueField="ClientContactID" DataTextField="Name" Width="100%" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <ucRotation:CustomAttributes ID="caRotationCustomAttributesID" EnableViewState="false" runat="server" />
            </div>
        </asp:Panel>
    </div>
    <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel" DefaultPanel="pnlSearch" DefaultPanelButton="Save"
        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonText="Reset" SubmitButtonIconClass="rbUndo" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
        CancelButtonText="Cancel" OnSubmitClick="fsucCmdBarButton_SubmitClick" OnSaveClick="fsucCmdBarButton_SaveClick"
        OnCancelClick="fsucCmdBarButton_CancelClick"
        ClearButtonIconClass="" UseAutoSkinMode="false" ButtonSkin="Silk">
    </infsu:CommandBar>
    <div class="row allowscroll">
        <infs:WclGrid runat="server" ID="grdApplicantSearchData" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            OnNeedDataSource="grdApplicantSearchData_NeedDataSource" OnItemCommand="grdApplicantSearchData_ItemCommand"
            OnSortCommand="grdApplicantSearchData_SortCommand" EnableLinqExpressions="false"
            NonExportingColumns="AssignItems,SSN" OnItemDataBound="grdApplicantSearchData_ItemDataBound"
            ShowClearFiltersButton="false">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrganizationUserId"
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
                            <asp:CheckBox ID="chkSelectItem" runat="server"
                                onclick="UnCheckHeader(this)" OnCheckedChanged="chkSelectItem_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="OrganizationUserId" HeaderText="User ID" SortExpression="OrganizationUserId"
                        HeaderTooltip="This column displays the User ID for each record in the grid" ItemStyle-Width="3%"
                        UniqueName="UserID">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" HeaderText="Applicant First Name" ItemStyle-Width="4%"
                        SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" HeaderText="Applicant Last Name" ItemStyle-Width="4%"
                        SortExpression="ApplicantLastName" UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstituteName" HeaderText="Institution" AllowSorting="false" ItemStyle-Width="4%"
                        SortExpression="InstituteName" UniqueName="Institution" HeaderTooltip="This column displays the Institution for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress" ItemStyle-Width="4%"
                        UniqueName="EmailAddress" HeaderTooltip="This column displays the applicant's email address for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="DateOfBirth" HeaderText="Date of Birth" SortExpression="DateOfBirth" ItemStyle-Width="4%"
                        UniqueName="DateOfBirth" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid" DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridDateTimeColumn>

                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN" ItemStyle-Width="4%"
                        UniqueName="SSN" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN" Display="false"
                        UniqueName="_SSN" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ArchiveStatus" HeaderText="Archive Status"
                        AllowSorting="false" SortExpression="ArchiveStatus"
                        ItemStyle-Width="6%" UniqueName="ArchiveStatus" HeaderTooltip="This column displays the Archive Status for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationNameAssigned" HeaderText="Rotation Name Assigned" ItemStyle-Width="6%"
                        AllowSorting="false" SortExpression="RotationNameAssigned"
                        UniqueName="RotationNameAssigned" HeaderTooltip="This column displays the Rotation Name Assigned for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UserGroups" HeaderText="User Group" AllowSorting="false" ItemStyle-Width="10%"
                        SortExpression="UserGroups" UniqueName="UserGroups" HeaderTooltip="This column displays any user group(s) to which the applicant belongs for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstitutionHierarchy" HeaderText="Institution Hierarchy" ItemStyle-Width="10%"
                        AllowSorting="false" SortExpression="InstitutionHierarchy"
                        UniqueName="InstitutionHierarchy" HeaderTooltip="This column displays the institution hierarchy for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationNotification" HeaderText="Rotation Notification" ItemStyle-Width="10%"
                        AllowSorting="false" SortExpression="RotationNotification"
                        UniqueName="RotationNotification" HeaderTooltip="This column displays the Rotation Notification for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ImmunizationComplianceStatus" HeaderText="Immunization compliance Status"
                        ItemStyle-Width="10%" AllowSorting="false" SortExpression="ImmunizationComplianceStatus"
                        UniqueName="ImmunizationComplianceStatus" HeaderTooltip="This column displays the Immunization compliance Status for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="BackgroundServiceGroupStatus" HeaderText="Background Service Group Status"
                        ItemStyle-Width="10%" AllowSorting="false" SortExpression="BackgroundServiceGroupStatus"
                        UniqueName="BackgroundServiceGroupStatus" HeaderTooltip="This column displays the Background Service Group Status for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationComplianceStatus" HeaderText="Rotation Compliance Status"
                        ItemStyle-Width="10%" AllowSorting="false" SortExpression="RotationComplianceStatus"
                        UniqueName="RotationComplianceStatus" HeaderTooltip="This column displays the Rotation Compliance Status for each record in the grid">
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
<div class="col-md-12" id="trailingText">
    <div id="menuDiv">
        <infs:WclMenu ID="cmd" runat="server" Skin="Default" AutoSkinMode="false" OnItemDataBound="cmd_ItemDataBound">
            <Items>
                <telerik:RadMenuItem Text="UserGroup">
                    <ItemTemplate>
                        <infs:WclButton runat="server" Text="User Group" ID="btnUserGroup" Icon-PrimaryIconCssClass="rbUsers"
                            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" AutoPostBack="false">
                        </infs:WclButton>
                    </ItemTemplate>
                    <Items>
                        <telerik:RadMenuItem>
                            <ItemTemplate>
                                <infs:WclButton runat="server" Text="Assign" ID="btnAssign" Icon-PrimaryIconCssClass="rbAssign" CssClass="btn"
                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnUserGroupAssign_Click">
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem>
                            <ItemTemplate>
                                <infs:WclButton runat="server" Text="Unassign" ID="btnUnassign" Icon-PrimaryIconCssClass="rbUnassign" CssClass="btn"
                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnUserGroupUnassign_Click">
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Rotation">
                    <ItemTemplate>
                        <infs:WclButton runat="server" Text="Rotation" ID="btnRotation" Icon-PrimaryIconCssClass="rbRefresh2"
                            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" AutoPostBack="false">
                        </infs:WclButton>
                    </ItemTemplate>
                    <Items>
                        <telerik:RadMenuItem>
                            <ItemTemplate>
                                <infs:WclButton runat="server" Text="Assign" ID="btnRotationAssign" Icon-PrimaryIconCssClass="rbAssign" CssClass="btn"
                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnRotationAssign_Click">
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem>
                            <ItemTemplate>
                                <infs:WclButton runat="server" Text="Unassign" ID="btnRotationUnassign" Icon-PrimaryIconCssClass="rbUnassign" CssClass="btn"
                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnRotationUnassign_Click">
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Archivemun">
                    <ItemTemplate>
                        <infs:WclButton runat="server" Text="Archive" ID="btnArchivemun" AutoPostBack="false" Icon-PrimaryIconCssClass="rbArchive"
                            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center">
                        </infs:WclButton>
                    </ItemTemplate>
                    <Items>
                        <telerik:RadMenuItem>
                            <ItemTemplate>
                                <infs:WclButton runat="server" Text="Archive" ID="btnArchive" Icon-PrimaryIconCssClass="rbArchive" CssClass="btn"
                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnArchive_Click">
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem>
                            <ItemTemplate>
                                <infs:WclButton runat="server" Text="UnArchive" ID="btnUnArchive" Icon-PrimaryIconCssClass="rbArchive" CssClass="btn"
                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnUnArchive_Click">
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="SendMessage">
                    <ItemTemplate>
                        <infs:WclButton runat="server" Text="Send Message" ID="btnsendmail" Icon-PrimaryIconCssClass="rbEnvelope2" AutoPostBack="false"
                            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center">
                        </infs:WclButton>
                    </ItemTemplate>
                    <Items>
                        <telerik:RadMenuItem>
                            <ItemTemplate>
                                <infs:WclButton runat="server" Text="Send Custom Message" ID="btnCustomMessage" CssClass="btn" Icon-PrimaryIconCssClass="btnPlane"
                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnCustomMessage_Click">
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="ScheduleRotation">
                            <ItemTemplate>
                                <infs:WclButton runat="server" Text="Notify of Schedule Rotation" ID="btnScheduleRotation" CssClass="btn" Icon-PrimaryIconCssClass="btnPlane"
                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnScheduleRotation_Click">
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:RadMenuItem>
                        <telerik:RadMenuItem Text="ScheduleRequirements">
                            <ItemTemplate>
                                <infs:WclButton runat="server" Text="Notify of Rotation Schedule & Requirements" ID="btnScheduleRequirements" CssClass="btn" Icon-PrimaryIconCssClass="btnPlane"
                                    Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnScheduleRequirements_Click">
                                </infs:WclButton>
                            </ItemTemplate>
                        </telerik:RadMenuItem>
                    </Items>
                </telerik:RadMenuItem>
            </Items>
        </infs:WclMenu>
    </div>
</div>
<asp:HiddenField ID="hdnSelectedTenantID" runat="server" />
<asp:HiddenField ID="hdMessageSent" runat="server" Value="new" />
<script type="text/javascript">
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

    function OpenUserGroupMappingPopup(screenMode) {
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
        var popupWindowName = "User Group Mapping";
        var url = $page.url.create("~/ComplianceOperations/Pages/UserGroupMapping.aspx?TenantID=" + tenantID + "&ScreenMode=" + screenMode);
        var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose }
           );
        return false;
    }

    function OpenRotationMappingPopup(screenMode) {
        var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
        var popupWindowName = "Rotation Mapping";
        var widht = (window.screen.width) * (90 / 100);
        var height = (window.screen.height) * (80 / 100);
        var popupsize = widht + ',' + height;
        var url = $page.url.create("~/ComplianceOperations/Pages/ClinicalRotationMapping.aspx?TenantID=" + tenantID + "&ScreenMode=" + screenMode + "&popupHeight=" + parseInt(height));
        var win = $window.createPopup(url, { size: popupsize, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Reload | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose }
           );
        return false;
    }

    function OnClose(oWnd, args) {
        oWnd.remove_close(OnClose);
        var arg = args.get_argument();
        //if (arg) {
        //    if (arg.Action == "Submit") {
        var masterTable = $find("<%= grdApplicantSearchData.ClientID %>").get_masterTableView();
        masterTable.rebind();
        //    }
        //}
    }

  

    function OpenPopup(sender, eventArgs) {
        var composeScreenWindowName = "composeScreen";
        var fromScreenName = "StudentBucketAssignment";
        var communicationTypeId = 'CT01';
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/Messaging/Pages/WriteMessage.aspx?cType=" + communicationTypeId + "&SName=" + fromScreenName);
        var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnMessagePopupClose });
        return false;
    }

    //This event fired when Send Message popup closed.
    function OnMessagePopupClose(oWnd, args) {
        oWnd.remove_close(OnMessagePopupClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.MessageSentStatus == "sent") {
                $jQuery("[id$=hdMessageSent]").val("sent");
                var masterTable = $find("<%= grdApplicantSearchData.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        }
    }
    function ValidateStartEndTime(sender, args) {
        var tpStartTime = $jQuery("[id$=tpStartTime]")[0].control.get_timeView().getTime();
        var tpEndTime = $jQuery("[id$=tpEndTime]")[0].control.get_timeView().getTime();
        if (tpEndTime != null && tpStartTime == null) {
            sender.innerText = 'Rotation "Start Time" is required.'
            args.IsValid = false;
        }
        if (tpStartTime != null && tpEndTime == null) {
            sender.innerText = 'Rotation "End Time" is required.'
            args.IsValid = false;
        }
    }

    var minDate = new Date("01/01/1980");

    function SetMinDate(picker) {
        picker.set_minDate(minDate);
    }
</script>
