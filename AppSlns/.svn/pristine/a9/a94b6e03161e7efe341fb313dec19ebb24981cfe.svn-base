<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RotationDetailForm.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.RotationDetailForm" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/SharedUserRotationDetails.ascx" TagName="RotationDetails"
    TagPrefix="uc" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearchMultipleNodes.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="ucNodeSearch" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxRotationDetails">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/ManageRotation.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>
<style type="text/css">
    #modcmd_bar {
        position: relative !important;
    }

    #dvSection {
        padding-top: 5px !important;
        padding-bottom: 0px !important;
        margin-bottom: 0px !important;
    }

    .RadMenu .rmItem .rmTemplate {
        background-color: #4382c2 !important;
    }

    /*#trailingText #menuDiv {
        margin: 0 auto;
        width: 50%;
        padding-top: 5px;
        margin: 0 auto;
        position: absolute;
        right: 0;
        top: 4px;
    }*/

    .center .sxcmds {
        float: right;
    }

    #menuDiv {
        margin: 0 auto;
        width: 50%;
        padding-top: 3px;
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

    .checkboxCptn::after {
        content: '' !important;
    }

    div#pnlCollection.sxcbar.c {
        float: left;
    }

    .instructorPreseptorRow {
        background-color: #61FFFF !important;
        height: 31px;
    }

        .instructorPreseptorRow:hover {
            background: none;
            font-weight: bold;
        }

        .instructorPreseptorRow.rgRow.rgSelectedRow {
            background: none;
            font-weight: bold;
            color: #333;
        }

        .instructorPreseptorRow.rgRow > .rgSorted {
            background-color: transparent;
        }

        .instructorPreseptorRow.rgAltRow > .rgSorted {
            background-color: transparent;
        }
</style>

<asp:UpdatePanel ID="pnlErrorSchuduleInv" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="msgbox" id="pageMsgBoxSchuduleInv" style="overflow-y: auto; max-height: 400px">
            <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblError"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<div class="container-fluid">
    <div id="dvSection">
        <div class="row">
            <div class="col-md-12">
                <div id="modcmd_bar">
                    <div id="vermod_cmds">
                        <asp:LinkButton Text="Back to Search" runat="server" ID="lnkGoBack" OnClick="lnkGoBack_click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<uc:RotationDetails ID="ucRotationDetails" runat="server"></uc:RotationDetails>

<div class="col-md-12 text-center">
    <infs:WclButton ID="btnPkgDetail" runat="server" ButtonType="StandardButton" Skin="Silk"
        AutoSkinMode="false" AutoPostBack="true" OnClick="btnPkgDetail_Click" CssClass="redBtn" Text="View Rotation Requirement Package Detail">
    </infs:WclButton>
</div>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Rotation Requirement Packages</h2>
        </div>
    </div>

    <div class="row bgLightGreen">

        <asp:Panel ID="pnlPackages" CssClass="" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class="cptn">Package Currently Assigned</span>
                        <infs:WclTextBox runat="server" ID="txtAssignedRotationPkg" ReadOnly="true" Width="100%"
                            CssClass="form-control">
                        </infs:WclTextBox>
                        <asp:Label ID="lblAssignedRotationPkg" runat="server" class="form-control"></asp:Label>
                    </div>
                </div>
            </div>

            <div id="dvAddUpdatePackage" runat="server">
                <div class='col-md-12'>
                    <div class="row">
                        <div class='form-group col-md-3' title="Select a package.">
                            <span class="cptn">Select Package</span><span class="reqd">*</span>

                            <infs:WclComboBox ID="cmbPackage" runat="server" DataTextField="RequirementPackageName"
                                DataValueField="RequirementPackageID"
                                AutoPostBack="true" OnDataBound="cmbPackage_DataBound" OnItemDataBound="cmbPackage_ItemDataBound"
                                OnSelectedIndexChanged="cmbPackage_SelectedIndexChanged"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvPackage" ControlToValidate="cmbPackage"
                                    InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAssignPackage"
                                    Text="Package is required." />
                            </div>
                        </div>
                        <div class='form-group col-md-2'>
                            <span class="cptn" style="color: transparent !important; display: block;"></span>
                            <infs:WclButton ID="btnAssignPackage" ButtonType="StandardButton" runat="server"
                                AutoPostBack="true" OnClick="btnAssignPackage_Click"
                                Text="Assign Package to Rotation" ButtonPosition="Center" ValidationGroup="grpAssignPackage"
                                CssClass="redBtn">
                            </infs:WclButton>
                        </div>
                        <div id="dvEditPackage" runat="server" class='form-group col-md-2 text-center' style="display: none">

                            <span class="cptn" style="color: transparent !important;"></span>
                            <asp:LinkButton Text="Edit Package" runat="server" ID="lnkEditPackage" CssClass="form-control blueText"
                                OnClick="lnkEditPackage_click" />

                        </div>
                        <div class='form-group col-md-2 text-center' style="display: none">
                            <span class="cptn" style="color: transparent !important;"></span>
                            <asp:LinkButton Text="Add New Package" runat="server" ID="lnkAddNewPackage" CssClass="form-control blueText"
                                OnClick="lnkAddNewPackage_click" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="dvShowPackage" runat="server" visible="false">
                <div class='col-md-12'>
                    <div class="row">
                        <div class='form-group col-md-3'>
                            <span class="cptn">Package</span>

                            <infs:WclTextBox runat="server" ID="txtPackageName" ReadOnly="true" Width="100%"
                                CssClass="form-control">
                            </infs:WclTextBox>
                            <asp:HiddenField ID="hdnRequirementPackageID" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

</div>

<div class="container-fluid" style="margin-top: 1%;">
    <div class="row">
        <div runat="server" id="dv_InstructorPreceptorPkgDetail" class="col-md-12 text-center">
            <infs:WclButton ID="btn_InstructorPreceptorPkgDetail" runat="server" ButtonType="StandardButton" Skin="Silk"
                AutoSkinMode="false" AutoPostBack="true" OnClick="btn_InstructorPreceptorPkgDetail_Click" CssClass="redBtn" Text="View Instructor/Preceptor Rotation Requirement Package Detail">
            </infs:WclButton>
        </div>

    </div>
</div>

<div class="container-fluid" id="dvInstrüctorPkg" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Instructor/Preceptor Rotation Package</h2>

        </div>
    </div>
    <div class="row bgLightGreen">

        <asp:Panel ID="Panel1" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class="cptn">Package Currently Assigned</span>
                        <infs:WclTextBox runat="server" ID="txtAssignedInstructorpkg" ReadOnly="true" Width="100%"
                            CssClass="form-control">
                        </infs:WclTextBox>
                        <asp:Label ID="lblAssignedInstructorpkg" runat="server" class="form-control" CssClass="form-control"></asp:Label>
                    </div>
                </div>
            </div>

            <div id="dvAddUpdateInstPackage" runat="server">
                <div class='col-md-12'>
                    <div class="row">
                        <div class='form-group col-md-3' title="Select a package.">
                            <span class="cptn">Select Package</span><span class="reqd">*</span>

                            <infs:WclComboBox ID="cmbInstPackage" runat="server" DataTextField="RequirementPackageName"
                                DataValueField="RequirementPackageID" OnItemDataBound="cmbInstPackage_ItemDataBound" OnSelectedIndexChanged="cmbInstPackage_SelectedIndexChanged"
                                AutoPostBack="true" OnDataBound="cmbInstPackage_DataBound" Width="100%" CssClass="form-control"
                                Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvInstPackage" ControlToValidate="cmbInstPackage"
                                    InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpAssignInstPackage"
                                    Text="Package is required." />
                            </div>
                        </div>
                        <div class='form-group col-md-2'>
                            <span class="cptn" style="color: transparent !important; display: block;"></span>
                            <infs:WclButton ID="btnAssignInstPkg" ButtonType="StandardButton" runat="server"
                                AutoPostBack="true" OnClick="btnAssignInstPkg_Click"
                                Text="Assign Package to Rotation" ButtonPosition="Center" CssClass="redBtn"
                                ValidationGroup="grpAssignInstPackage">
                            </infs:WclButton>
                        </div>
                        <div id="dvEditInstPackage" class='form-group col-md-2 text-center' runat="server" style="display: none">
                            <span class="cptn" style="color: transparent !important;"></span>
                            <asp:LinkButton Text="Edit Package" runat="server" ID="btnEditInstPkg" CssClass="form-control blueText"
                                OnClick="btnEditInstPkg_Click" />

                        </div>
                        <div class='form-group col-md-2 text-center' style="display: none">
                            <span class="cptn" style="color: transparent !important;"></span>
                            <asp:LinkButton Text="Add New Package" runat="server" ID="btnAddInstPkg" CssClass="form-control blueText"
                                OnClick="btnAddInstPkg_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="dvShowInstPackage" runat="server" visible="false">
                <div class='col-md-12'>
                    <div class="row">
                        <div class='form-group col-md-3'>
                            <span class="cptn">Package</span>

                            <infs:WclTextBox runat="server" ID="txtInstPackage" ReadOnly="true" Width="100%"
                                CssClass="form-control">
                            </infs:WclTextBox>
                            <asp:HiddenField ID="hdnInstRequirementPackageID" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

</div>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">Rotation Members</h1>
        </div>
    </div>


    <div class="row bgLightGreen">

        <div class="col-md-12">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdRotationMembers" AllowCustomPaging="false"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            EnableLinqExpressions="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            ShowClearFiltersButton="false"
            NonExportingColumns="RemoveItems,SSN,CustomAttributes,InvitationReviewStatus,DocumentView,Detail,SchoolCompliance,AgencyCompliance" ValidationGroup="grpValdManageAgencyUsers"
            OnNeedDataSource="grdRotationMembers_NeedDataSource" OnItemCommand="grdRotationMembers_ItemCommand"
            OnItemDataBound="grdRotationMembers_ItemDataBound">
            <ExportSettings Pdf-PageWidth="400mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grdRotationMembers_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ClinicalRotationMemberId,SchoolPackageSubscriptionID,SchoolCompliancePackageID,RotationMemberDetail.OrganizationUserId,SchoolCompliance,RequirementPackageID,RequirementSubscriptionId,AgencyCompliance,RotationMemberDetail.InvitationReviewStatus,IsInstructor,RotationMemberRowIndex"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                    ShowExportToPdfButton="true" ShowExportToCsvButton="true"></CommandItemSettings>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="RemoveItems" HeaderTooltip="Click this box to remove all members"
                        AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkRemoveAll" runat="server" onclick="CheckRemoveAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRemoveItem" runat="server" onclick="UnCheckRemoveAllHeader(this)" OnCheckedChanged="chkRemoveItem_CheckedChanged"
                                Enabled='<%#Convert.ToString(Eval("IsDropped")).ToLower() == "false" ? true : false %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column"
                        HeaderText="First Name" SortExpression="RotationMemberDetail.ApplicantFirstName"
                        UniqueName="ApplicantFirstName" HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column"
                        HeaderText="Last Name" SortExpression="RotationMemberDetail.ApplicantLastName"
                        UniqueName="ApplicantLastName" HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.EmailAddress" FilterControlAltText="Filter EmailAddress column"
                        HeaderText="Email Address" SortExpression="RotationMemberDetail.EmailAddress" HeaderStyle-Width="170px"
                        UniqueName="EmailAddress" HeaderTooltip="This column displays the applicant's email address for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.DateOfBirth" FilterControlAltText="Filter DateOfBirth column"
                        HeaderText="Date Of Birth" SortExpression="RotationMemberDetail.DateOfBirth"
                        UniqueName="DateOfBirth" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid"
                        DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.SSN" FilterControlAltText="Filter AGU_TenantName column"
                        HeaderText="SSN/ID Number" SortExpression="RotationMemberDetail.SSN" UniqueName="SSN"
                        HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.SSN" HeaderText="SSN/ID Number"
                        SortExpression="RotationMemberDetail.SSN" Display="false"
                        UniqueName="_SSN" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.UserGroups" FilterControlAltText="Filter UserGroups column"
                        HeaderText="User Group" SortExpression="RotationMemberDetail.UserGroups" UniqueName="UserGroups"
                        HeaderTooltip="This column displays any user group(s) to which the applicant belongs for each record in the grid">
                    </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RotationMemberDetail.PhoneNumber" FilterControlAltText="Filter PhoneNumber column"
                        HeaderText="Phone Number" SortExpression="RotationMemberDetail.PhoneNumber" UniqueName="PhoneNumber"
                        HeaderTooltip="This column displays the applicant's Phone Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.CustomAttributes" FilterControlAltText="Filter CustomAttributes column"
                        HeaderText="Custom Attributes" SortExpression="RotationMemberDetail.CustomAttributes"
                        UniqueName="CustomAttributes" HeaderTooltip="This column displays the custom attribute(s) for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.CustomAttributes" AllowFiltering="false"
                        HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="300px"
                        UniqueName="CustomAttributesTemp" Display="false">
                    </telerik:GridBoundColumn>
                     <%------New GridBoundColumns added------%>
                    <telerik:GridBoundColumn DataField="SchoolCompliance" HeaderTooltip="School Compliance" UniqueName="SchoolComplianceTemp"
                        AllowFiltering="false" HeaderText="School Compliance" ShowFilterIcon="false" Display="false">                       
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AgencyCompliance" HeaderTooltip="Agency Compliance" UniqueName="AgencyComplianceTemp"
                        AllowFiltering="false" HeaderText="Agency Compliance" ShowFilterIcon="false" Display="false">
                    </telerik:GridBoundColumn>
                    <%------New GridBoundColumns added------%>
                    <telerik:GridTemplateColumn UniqueName="SchoolCompliance" HeaderTooltip="School Compliance"
                        AllowFiltering="false" HeaderText="School Compliance" ShowFilterIcon="false">
                        <ItemTemplate>
                            <a href="javascript:void(0)" runat="server" onclick="ShowSchoolStatusPopUp(this);" id="lnkSchoolCompliance"></a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="AgencyCompliance" HeaderTooltip="Agency Compliance"
                        AllowFiltering="false" HeaderText="Agency Compliance" ShowFilterIcon="false">
                        <ItemTemplate>
                            <a href="javascript:void(0)" runat="server" onclick="ShowAgencyStatusPopUp(this);" id="lnkAgencyCompliance"></a>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.Notes" FilterControlAltText="Filter Notes column"
                        HeaderText="Review Notes" SortExpression="RotationMemberDetail.Notes"
                        UniqueName="Notes" HeaderTooltip="This column displays the review status/notes for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.StudentDroppedDate" HeaderText="Dropped Date" FilterControlAltText="Filter StudentDroppedDate column"
                        SortExpression="RotationMemberDetail.StudentDroppedDate" DataFormatString="{0:MM/dd/yyyy}"
                        HeaderTooltip="This column displays the dropped date of student for each record in the grid"
                        UniqueName="StudentDroppedDate">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.ShareStatus" FilterControlAltText="Filter Share Status column"
                        HeaderText="Share Status" SortExpression="RotationMemberDetail.ShareStatus"
                        UniqueName="ShareStatus" HeaderTooltip="This column displays the Share Status for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RotationMemberDetail.InvitationReviewStatus" FilterControlAltText="Filter Notes column"
                        HeaderText="InvitationReviewStatus" SortExpression="RotationMemberDetail.InvitationReviewStatus"
                        UniqueName="InvitationReviewStatus" HeaderTooltip="This column displays the review status/notes for each record in the grid" Display="false">
                    </telerik:GridBoundColumn>

                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="DocumentView" HeaderText="Document" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnView" ButtonType="LinkButton" CommandName="DocumentView" Visible="false"
                                ToolTip="Click here to view documents of rotation member." runat="server" Text="View">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Detail" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnDetail" ButtonType="LinkButton" CommandName="Detail" Visible="false"
                                ToolTip="Click here to view the requirement verification details." runat="server" Text="Detail">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <%-- <telerik:GridBoundColumn DataField="SchoolCompliance" FilterControlAltText="Filter SchoolCompliance column"
                        HeaderText="School Compliance" SortExpression="SchoolCompliance" UniqueName="SchoolCompliance">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AgencyCompliance" FilterControlAltText="Filter AgencyCompliance column"
                        HeaderText="Agency Compliance" SortExpression="AgencyCompliance" UniqueName="AgencyCompliance">
                    </telerik:GridBoundColumn>--%>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                    Position="TopAndBottom" />
            </MasterTableView>
        </infs:WclGrid>

    </div>
    <div class="row">
        <div class="col-md-12">
            &nbsp;
        </div>
    </div>
    <div class="row bgLightGreen">
        <div class='form-group col-md-12' style="padding-left: 8px;">
            <span class="cptn checkboxCptn">
                <infs:WclCheckBox ID="chkSendNotificationToSchoolAdmin" runat="server" Checked="true" Text="I would like to receive a confirmation email when the rotation is approved/rejected." />
            </span>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            &nbsp;
        </div>
    </div>
    <div class="row" id="trailingText">
        <div style="width: 775px !important; margin: auto !important;">
            <infsu:CommandBar ID="fsucCmdBarButtons" width="590px" ClientIDMode="Static" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                AutoPostbackButtons="Submit,Cancel" SubmitButtonIconClass="" SaveButtonIconClass=""
                SubmitButtonText="Profile Share" SaveButtonText="Remove from Rotation" OnSaveClientClick="ConfirmationMessage"
                OnSaveClick="fsucCmdBarButtons_RemoveMembersClick" OnSubmitClick="fsucCmdBarButtons_ProfileShareClick"
                CancelButtonText="Download Rotation Document(s)" OnCancelClick="fsucCmdBarButtons_CancelClick" CancelButtonIconClass=""
                UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
            <div id="menuDiv" style="float: left; text-align: left; width: 176px;">
                <infs:WclMenu ID="cmd" runat="server" Skin="Default" AutoSkinMode="false">
                    <Items>
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
                                <telerik:RadMenuItem Text="RotationDetails">
                                    <ItemTemplate>
                                        <infs:WclButton runat="server" Text="Notify Agency User(s) about Rotation Information" ID="btnRotationDetailsNotification" CssClass="btn" Icon-PrimaryIconCssClass="btnPlane"
                                            Skin="Silk" AutoSkinMode="false" ButtonPosition="Center" OnClick="btnRotationDetailsNotification_Click">
                                        </infs:WclButton>
                                    </ItemTemplate>
                                </telerik:RadMenuItem>
                            </Items>
                        </telerik:RadMenuItem>
                    </Items>
                </infs:WclMenu>
            </div>
        </div>
    </div>
</div>

<div id="dvAssignToRotation" runat="server" class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">Assign to Rotation</h1>
        </div>
    </div>

    <div class="row bgLightGreen">
        <div id="divStudentSearchPanel">
            <asp:Panel ID="pnlSearch" runat="server">
                <div class='col-md-12'>
                    <div class="row">
                        <div class='form-group col-md-3' title="Select a User Group. The selected group's members' checkboxes will be marked in the grid below">
                            <span class="cptn">User Group</span>

                            <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID"
                                AutoPostBack="false" OnDataBound="ddlUserGroup_DataBound" Width="100%" CssClass="form-control"
                                Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                        </div>
                        <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                            <span class="cptn">Applicant First Name</span>

                            <infs:WclTextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                        </div>
                        <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                            <span class="cptn">Applicant Last Name</span>

                            <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                        </div>
                        <div class='form-group col-md-3' title="Restrict search results to the entered email address">
                            <span class="cptn">Email Address</span>

                            <infs:WclTextBox ID="txtEmail" runat="server" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                        </div>
                    </div>
                </div>

                <div class='col-md-12'>
                    <div class="row">

                        <div id="divSSN" runat="server">
                            <div class='form-group col-md-3' title="Restrict search results to the entered SSN or ID Number">
                                <span class="cptn">SSN/ID Number</span>

                                <infs:WclMaskedTextBox runat="server" ID="txtSSN" Mask="aaa-aa-aaaa" Width="100%"
                                    CssClass="form-control" />
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
                        <div>
                            <div class='form-group col-md-3' style="padding-left: 0px;">
                                <ucNodeSearch:CustomAttributeLoaderSearch ID="ucCustomAttributeLoaderSearch" IsCustomAttributesHide="true" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class='col-md-12'>
            <div class="form-group col-md-2">
                <div class="row">
                    <%--UAT-2887--%>
                    <asp:CheckBox ID="chkSelectAllResults" Text="Select All Results" runat="server" Width="100%" AutoPostBack="true" CssClass="form-control" OnCheckedChanged="chkSelectAllResults_CheckedChanged" />
                </div>
            </div>
        </div>
        <div class='col-md-12 text-center'>
            <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel,Clear"
                AutoPostbackButtons="Submit,Save,Cancel,Clear" SubmitButtonIconClass="rbUndo"
                SubmitButtonText="Reset" SaveButtonText="Search" SaveButtonIconClass="rbSearch"
                ClearButtonIconClass="" ClearButtonText="Assign to Rotation"
                CancelButtonText="Cancel" OnSaveClick="fsucCmdBarButton_SearchClick" OnSubmitClick="fsucCmdBarButton_ResetClick"
                OnCancelClick="fsucCmdBarButton_CancelClick"
                OnClearClick="fsucCmdBarButton_AddToRotationClick" ValidationGroup="grpFormSubmit"
                UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>

        </div>
    </div>
    <div class='row'>

        <infs:WclGrid runat="server" ID="grdAddToRotation" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            EnableLinqExpressions="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            ShowClearFiltersButton="false"
            OnNeedDataSource="grdAddToRotation_NeedDataSource" OnItemCommand="grdAddToRotation_ItemCommand"
            OnSortCommand="grdAddToRotation_SortCommand" OnItemDataBound="grdAddToRotation_ItemDataBound"
            NonExportingColumns="AssignItems,SSN">
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
                            <asp:CheckBox ID="chkSelectItem" runat="server" CssClass="uncheck" OnCheckedChanged="chkSelectItem_CheckedChanged"
                                onclick='<% # "UnCheckHeader(this,\"" + Eval("OrganizationUserId") + "\" );"%>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
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
                        UniqueName="DateOfBirth" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid"
                        DataFormatString="{0:MM/dd/yyyy}" FilterControlWidth="100px">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="UserGroups" HeaderText="User Group" AllowSorting="false"
                        SortExpression="UserGroups" UniqueName="UserGroups" HeaderTooltip="This column displays any user group(s) to which the applicant belongs for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        UniqueName="SSN" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        Display="false"
                        UniqueName="_SSN" HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid">
                    </telerik:GridBoundColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                    Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>

    <div class='col-md-12 text-center'>
        <infsu:CommandBar ID="fsucCmdBarButton_AssignRotation" runat="server" ButtonPosition="Center"
            DisplayButtons="Clear" AutoPostbackButtons="Clear" ClearButtonIconClass=""
            ClearButtonText="Assign to Rotation" OnClearClick="fsucCmdBarButton_AddToRotationClick"
            ValidationGroup="grpFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk">
        </infsu:CommandBar>
        <div class='col-md-12'>&nbsp;</div>
    </div>
</div>
<div class="approvepopup" runat="server" style="display: none">
    <div style="float: left; width: 50px">
        <img src="../../Resources/Themes/Default/images/info.png" />
    </div>
    <div>By deleting the student(s) from the rotation, any data previously submitted for this rotation will be erased.</div>
</div>
<%--UAT-4147--%>
<div id="divExistingRotationMembers" class="acknowledgeMessagePopup" title="Complio" runat="server" style="display: none">
    <p style="text-align: left;">One or more of the selected Applicant(s) is already added as Instructor/Preceptor. Please review your selection: </p>
    <div>&nbsp;</div>
    <asp:Panel ID="pnlExistingRotationMembers" runat="server">
    </asp:Panel>
</div>

<%--UAT-4323--%>
<div id="divRotationApplicantLimitNotificationPopup" class="rotationApplicantLimitNotificationPopup" title="Complio" runat="server" style="display: none">
    <p style="text-align: left;">You cannot add Applicant(s) to this Rotation more than the specified limit of <asp:Label runat="server" ID="lblNoOfStudents" />. Please review your selection.</p>
    <div>&nbsp;</div>
    <asp:Panel ID="pnlRotationApplicantLimit" runat="server">
    </asp:Panel>
</div>

<asp:HiddenField ID="hdnTotalUsersAssigned" runat="server" />
<asp:HiddenField ID="hdnErrorMessage" runat="server" />
<asp:HiddenField ID="hdnOrganizationUserId" runat="server" />

<iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>

<script type="text/javascript">

    var editStudentIds = [];
    function CheckAll(id) {
        //var masterTable = $find(FSObject.$("[id$=grdAddToRotation]")[0].id).get_masterTableView();
        var masterTable = $find("<%= grdAddToRotation.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
                var studentId = row[i].getDataKeyValue("OrganizationUserId");
                AddStudentIDsInList(isChecked, studentId);
            }
        }
    }


    function UnCheckHeader(id, studentId) {
        var checkHeader = true;
        //var masterTable = $find(FSObject.$("[id$=grdAddToRotation]")[0].id).get_masterTableView();
        var masterTable = $find("<%= grdAddToRotation.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        AddStudentIDsInList(id.checked, studentId);
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

    //function to get StudentIds of all checked items and storing them in array.
    function AddStudentIDsInList(isChecked, studentId) {
        if (isChecked) {
            if (editStudentIds.indexOf(studentId) < 0) {
                editStudentIds.push(studentId);
            }
        }
        else {
            editStudentIds = $jQuery.grep(editStudentIds, function (value) {
                return value != studentId;
            });
        }
        $jQuery('[id$="hdnOrganizationUserId"]').val(editStudentIds);
    }

    function CheckRemoveAll(id) {
        //var masterTable = $find(FSObject.$("[id$=grdRotationMembers]")[0].id).get_masterTableView();
        var masterTable = $find("<%= grdRotationMembers.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkRemoveItem").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkRemoveItem").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckRemoveAllHeader(id) {
        var checkHeader = true;
        //var masterTable = $find(FSObject.$("[id$=grdRotationMembers]")[0].id).get_masterTableView();
        var masterTable = $find("<%= grdRotationMembers.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkRemoveItem").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkRemoveItem").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkRemoveAll]')[0].checked = checkHeader;
    }

    function pageLoad() {
        SetDefaultButtonForSection("divStudentSearchPanel", "fsucCmdBarButton_btnSave", true);
    }

    function ShowSchoolStatusPopUp(obj) {
        var args = $jQuery(obj).attr('args');
        if (args != null && args != "") {
            //var fromScreenName = "BkgOrderSearchQueue";
            //var communicationTypeId = 'CT01';
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ClinicalRotation/Pages/CompliancePackageDetail.aspx?args=" + args);
            var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize });
            return false;
        }
    }

    //function OnSchoolStatusPopUpClose(oWnd, args) {
    //    oWnd.remove_close(OnSchoolStatusPopUpClose);
    //}


    function ShowAgencyStatusPopUp(obj) {
        var args = $jQuery(obj).attr('args');
        if (args != null && args != "") {
            //var fromScreenName = "BkgOrderSearchQueue";
            //var communicationTypeId = 'CT01';
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);
            var rotationDetailScreenName = "Agency Compliance";
            url = $page.url.create("~/ApplicantRotationRequirement/Pages/RotationRequirementDataEntryPopup.aspx?args=" + args);
            var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize }, function () { this.set_title(rotationDetailScreenName) });
            return false;
        }
    }

    //function OnAgencyStatusPopUpClose(oWnd, args) {
    //    oWnd.remove_close(OnAgencyStatusPopUpClose);
    //    var arg = args.get_argument();
    //    if (arg) {
    //        if (arg.MessageSentStatus == "sent") {
    //        }
    //    }
    //}

    function OpenPopup(sender, eventArgs) {
        var composeScreenWindowName = "composeScreen";
        var fromScreenName = "RotationDetailForm";
        var communicationTypeId = 'CT01';
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/Messaging/Pages/WriteMessage.aspx?cType=" + communicationTypeId + "&SName=" + fromScreenName);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnMessagePopupClose });
        return false;
    }

    //This event fired when Send Message popup closed.
    function OnMessagePopupClose(oWnd, args) {
        //debugger;
        oWnd.remove_close(OnMessagePopupClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.MessageSentStatus == "sent") {
                ShowSuccessMessage("Message has been sent successfully.", "sucs", true)
            }
        }
    }

    function ShowSuccessMessage(msg, msgtype, overriderErrorPanel) {
        /// <summary>Shows message box on the page</summary>
        /// <param name="msg" type="String">Message to be displayed</param>
        /// <param name="msgtype" type="$page.msgTypes">Type of message box</param>

        if (typeof (msg) == "undefined") return;
        var c = typeof (msgtype) != "undefined" ? msgtype : "";
        if ($jQuery(".no_error_panel").length > 0 || overriderErrorPanel) {
            $jQuery("#pageMsgBox").children("span").text(msg).attr("class", msgtype);
            if (c == 'sucs') {
                c = "Success";
            }
            else (c = c.toUpperCase());

            $jQuery("#pnlError").hide();

            $window.showDialog($jQuery("#pageMsgBox").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 400, c);
        }
        else {
            $jQuery("#pageMsgBox").fadeIn().children("span").text(msg).attr("class", msgtype);
        }
    }

    //UAT-2225
    function ConfirmationMessage(sender, args) {
        $window.showDialog($jQuery(".approvepopup").clone().show(), {
            continuebtn: {
                autoclose: true, text: "Continue", click: function () {
                    __doPostBack('<%= fsucCmdBarButtons.SaveButton.UniqueID %>', '');
                }
            }, closeBtn: {
                autoclose: true, text: "Cancel"
            }
        }, 475, 'Complio');
    }


    function OpenRequirementPackagePopup(ClinicalRotationID, tenantID, IsStudentPackage) {
        var popupWindowName = "Rotation Requirement Package Detail";
        if (IsStudentPackage == "False" || IsStudentPackage == "false") {
            popupWindowName = "Instructor/Preceptor Rotation Requirement Package Detail";
        }

        winopen = true;
        var url = "ClinicalRotation/Pages/RotationPackageCategoryDetailPopUp.aspx?RotationID=" + ClinicalRotationID + "&TenantId=" + tenantID + "&IsStudentPackage=" + IsStudentPackage;

        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url, { size: "1100," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Modal }, function () { this.set_title(popupWindowName); this.set_destroyOnClose(true); });
        return false;
    }
    //UAT-2887
    function AddStudentIdsInArray() {
        var studentIds = $jQuery('[id$="hdnOrganizationUserId"]').val();
        if (studentIds != null && studentIds != "") {
            editStudentIds = studentIds.split(',');
        }
    }
    function ResetSelectedUsers() {
        editStudentIds = [];
    }

    //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
    function ShowCallBackMessage(msg, noteMsg, msgClass) {
        //if (docMessage != '') {
        //    alert(docMessage);
        //}
        // debugger;
        if (typeof (msg) == "undefined") return;
        var c = typeof (msgClass) != "undefined" ? msgClass : "";
        if ($jQuery(".approvepopup").length > 0) {
            $jQuery("#pageMsgBox").children("span").text(msg).attr("class", msgClass);
            if (noteMsg != "undefined" && $jQuery("#pageMsgBox")[0].innerHTML != "undefined" && noteMsg != "" && noteMsg != null) {
                $jQuery("#pageMsgBox")[0].innerHTML = $jQuery("#pageMsgBox")[0].innerHTML + "<span class='info' id='lblNotes'>Note : " + noteMsg + "</span>";
            }
            if (c == 'sucs') {
                c = "Success";
            }
            else (c = c.toUpperCase());

            $jQuery("#pnlError").hide();

            $window.showDialog($jQuery("#pageMsgBox").clone().show(), {
                
                closeBtn: {
                    autoclose: true, text: "Ok", click: function () {
                        $jQuery("#pageMsgBox").children("span").text('').attr("class", "");
                        $jQuery("#pnlError").show();
                    }
                }
            }, 400, c);
        }
    }


    //UAT:-3049
    function grdRotationMembers_rwDbClick(s, e) {
        var _id = "btnDetail";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    function RemoveDisplayNoneFrompnlErrorDiv() {
        $jQuery("#pnlError").show();
    }

    //UAT-4147
    function ShowExistingRotationMembers() {
        // debugger;
        var dialog = $window.showDialog($jQuery("[id$=divExistingRotationMembers]").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Ok", click: function () {                    
                    return false;
                }
            }            
        }, 550, 'Notice');
    }

    //UAT-4323
    function ShowRotationApplicantLimitViolationNotification() {
        // debugger;
        var dialog = $window.showDialog($jQuery("[id$=divRotationApplicantLimitNotificationPopup]").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Ok", click: function () {
                    return false;
                }
            }
        }, 550, 'Complio');
    }
</script>
