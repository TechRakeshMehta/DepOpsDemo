<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstructorSupportPortalDetail.ascx.cs" Inherits="CoreWeb.SearchUI.Views.InstructorSupportPortalDetail" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantRequirementRotations.ascx" TagPrefix="infsu" TagName="ApplicantRequirementRotations" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantPortfolioProfile.ascx" TagPrefix="infsu"
    TagName="ApplicantPortfolioProfile" %>
<%@ Register Src="~/SearchUI/UserControl/SupportPortalNotes.ascx" TagPrefix="uc" TagName="SupportPortalNotes" %>
<%@ Import Namespace="INTSOF.Utils" %>

<style type="text/css">
    .autoRenewalLink {
        display: inline-block;
        color: Black;
        background-color: #D6D6D6;
        border-style: None;
        text-decoration: none;
        padding: 2px 15px;
    }

    .autoRenewalLinkOffButton {
        display: inline-block;
        color: Black;
        background-color: #D6D6D6;
        border-style: None;
        text-decoration: none;
        padding: 2px 15px;
    }

    a.autoRenewalLink:hover {
        background-color: #D5E5FF;
    }

    .pnlCCNotes {
        margin-top: 10px;
    }
</style>
<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>

<asp:UpdatePanel ID="pnlErrorPkgCompletion" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="msgbox" id="pageMsgBoxPkgCompletion" style="overflow-y: auto; max-height: 400px">
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
                        <asp:LinkButton ID="lnkGoBack" runat="server" OnClick="lnkGoBack_Click" Text="Back to Support Portal Search"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-md-12">&nbsp;</div>
    <div class="row">
        <div class='col-md-12'>
            <div class="form-group col-md-2" style="float: right; text-align: right">
                <infs:WclButton runat="server" ID="btnInstructorLogin" Text="Instructor/Preceptor Login" AutoSkinMode="false" Skin="Silk" OnClick="btnInstructorLogin_Click"></infs:WclButton>

            </div>
        </div>
    </div>


    <asp:Panel ID="pnlInstructorProfile" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Personal Information
                </h2>
            </div>
        </div>

        <div class="col-md-12">
            <div class="row">
                <div class="form-group col-md-3">
                    <span class="cptn">Email Address</span>
                    <infs:WclTextBox runat="server" Width="100%" CssClass="form-control" ID="txtEmail" MaxLength="50" Enabled="false">
                    </infs:WclTextBox>
                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">First Name</span>
                    <infs:WclTextBox runat="server" ID="txtFirstName" MaxLength="256" Width="100%" CssClass="form-control" Enabled="false">
                    </infs:WclTextBox>

                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">Middle Name</span>
                    <infs:WclTextBox runat="server" ID="txtMiddleName" Width="100%" CssClass="form-control" MaxLength="50" Enabled="false">
                    </infs:WclTextBox>
                </div>
                <div class="form-group col-md-3">
                    <span class="cptn">Last Name</span>
                    <infs:WclTextBox runat="server" ID="txtLastName" Width="100%" CssClass="form-control" MaxLength="50" Enabled="false">
                    </infs:WclTextBox>

                </div>

            </div>

            <div class="row">
                  <div class="form-group col-md-3">
                    <span class="cptn">Last 4 digits of SSN</span>
                    <infs:WclMaskedTextBox runat="server" ID="txtSSN" Enabled="false" Mask="####" AutoPostBack="false" Width="100%" />
                      </div>
                </div>

        </div>

    </asp:Panel>


    <asp:Panel ID="Panel2" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Clinical Rotations
                </h2>
            </div>
        </div>
        <div class="row  allowscroll">


            <infs:WclGrid Width="100%" CssClass="gridhover" runat="server" ID="grdRotations" AllowCustomPaging="false"
                AutoGenerateColumns="False" AllowFilteringByColumn="false"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
                OnNeedDataSource="grdRotations_NeedDataSource" OnItemCommand="grdRotations_ItemCommand"
                NonExportingColumns="ViewDetail" EnableLinqExpressions="false" ShowClearFiltersButton="false">

                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="RotationId,PkgSubscriptionId" AllowSorting="false"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="AgencyName" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Agency Name" UniqueName="AgencyName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="HierarchyNodes" HeaderText="Hierarchy" SortExpression="HierarchyNodes"
                            HeaderTooltip="This column displays the Hierarchy for each record in the grid" HeaderStyle-Width="250px"
                            UniqueName="HierarchyNodes">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ComplioID" HeaderText="Complio ID" SortExpression="ComplioID"
                            UniqueName="ComplioID" HeaderTooltip="This column displays the Complio ID for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RotationName" HeaderText="Rotation ID/Name" SortExpression="RotationName"
                            UniqueName="RotationName" HeaderTooltip="This column displays the Location for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TypeSpecialty" HeaderText="Type/Specialty" SortExpression="TypeSpecialty"
                            HeaderStyle-Width="100px"
                            UniqueName="TypeSpecialty" HeaderTooltip="This column displays the Type/Specialty for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Department" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Department" UniqueName="Department">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Program" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Program" UniqueName="Program">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Course" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Course" UniqueName="Course">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Term" HeaderText="Term" SortExpression="Term"
                            UniqueName="Term" HeaderTooltip="This column displays the term for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UnitFloorLoc" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Unit/Floor/Location" UniqueName="UnitFloorLoc">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Students" HeaderText="# of Students"
                            SortExpression="Students" HeaderStyle-Width="100px"
                            UniqueName="Students" HeaderTooltip="This column displays the # of Students for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="RecommendedHours" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="No of Hours" UniqueName="RecommendedHours">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DaysName" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Days" UniqueName="DaysName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Shift" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Shift" UniqueName="Shift">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Time" UniqueName="Time" ItemStyle-Width="140px">
                            <ItemTemplate>
                                <asp:Label ID="lblFromTime" runat="server" Text='<%# 
                             String.IsNullOrEmpty(Convert.ToString(Eval("StartTime"))) ? String.Empty:  DateTime.Parse(Convert.ToString(Eval("StartTime"))).ToShortTimeString() %>' />
                                <span>- </span>
                                <asp:Label ID="lblToTime" runat="server" Text='<%# 
                             String.IsNullOrEmpty(Convert.ToString(Eval("EndTime"))) ? String.Empty:  DateTime.Parse(Convert.ToString(Eval("EndTime"))).ToShortTimeString() %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Start Date" UniqueName="Time">
                            <ItemTemplate>
                                <asp:Label ID="lblStartDate" runat="server" Text='<%# Convert.ToString(Eval("StartDate", "{0:d}"))  %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="End Date" UniqueName="Time">
                            <ItemTemplate>
                                <asp:Label ID="lblEndDate" runat="server" Text='<%# Convert.ToString(Eval("EndDate", "{0:d}"))  %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ContactNames" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="100px" HeaderText="Instructor/Preceptor" UniqueName="ContactNames">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" ItemStyle-Width="130px">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnViewDetails" ButtonType="LinkButton" CommandName="ViewDetail" Visible='<%# 
                             Eval("PkgSubscriptionId") == null ? false: true %>'
                                    ToolTip='<%# String.Concat("Click to view rotation detail, For Complio ID: ", Eval("ComplioID")) %>'
                                    runat="server" Text="Detail" BackColor="Transparent" Font-Underline="true" BorderStyle="None">
                                </telerik:RadButton>
                                <telerik:RadButton ID="btnNotApplicable" ButtonType="LinkButton" Visible='<%#Eval("PkgSubscriptionId") == null ? true: false %>'
                                    ToolTip='<%# String.Concat(" You do not currently have any additional requirements for this rotation. If additional requirements are assigned, you will receive an email notification. For Complio Id: ", Eval("ComplioID")) %>'
                                    runat="server" Text="Not Applicable" BackColor="Transparent" Font-Underline="true" BorderStyle="None" Enabled="false">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Compliance Status">
                            <ItemTemplate>
                                <%# !String.IsNullOrEmpty(Convert.ToString(Eval("RequirementPackageStatusCode"))) 
                            ? String.Compare(RequirementPackageStatus.REQUIREMENT_COMPLIANT.GetStringValue(), Convert.ToString(Eval("RequirementPackageStatusCode"))) == 0 ? "<img src='../../Resources/Mod/Compliance/icons/yes16.png' alt='Complio ID " + Convert.ToString(Eval("ComplioID"))+ " is Compliant' style='vertical-align: text-bottom; width: 12; height: 10' />" :"<img src='../../Resources/Mod/Compliance/icons/no16.png' alt='Complio ID " + Convert.ToString(Eval("ComplioID"))+ " is not Compliant' style='vertical-align: text-bottom; width: 12; height: 10' />"
                            : String.Empty
                                %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Review Status" UniqueName="SharedUserInvitationReviewStatusName" DataField="SharedUserInvitationReviewStatusName">
                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlNotes" runat="server">
        <div class="row ">
            <div class="col-md-12">
                <h2 class="header-color">Notes
                </h2>
            </div>
        </div>
        <div class="row allowscroll">

            <infs:WclGrid runat="server" Width="100%" ID="grdClientContactNotes" AutoGenerateColumns="False" AllowSorting="True" AllowFilteringByColumn="true" AutoSkinMode="True" CellSpacing="0"
                NonExportingColumns="EditCommandColumn,DeleteColumn" ShowClearFiltersButton="false" GridLines="Both" OnNeedDataSource="grdClientContactNotes_NeedDataSource" OnItemCommand="grdClientContactNotes_ItemCommand"
                OnItemDataBound="grdClientContactNotes_ItemDataBound" PagerStyle-Visible="false"
                ShowAllExportButtons="true" EnableAriaSupport="true" AllowCustomPaging="true">
                <GroupingSettings CaseSensitive="false" />
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="NoteId" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" 
                        ShowExportToPdfButton="false" ShowExportToCsvButton="false" AddNewRecordText="Add New Note" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridNumericColumn DataField="NoteId" FilterControlAltText="Filter NoteId column"
                            HeaderText="NoteId" SortExpression="NoteId" DataType="System.Int32" UniqueName="NoteId" Display="false"
                            DecimalDigits="0" HeaderTooltip="This column displays the note id for each record in the grid.">
                        </telerik:GridNumericColumn>

                        <telerik:GridBoundColumn DataField="Notes" FilterControlAltText="Filter Notes column"
                            HeaderText="Notes" SortExpression="Notes" UniqueName="Notes" ItemStyle-Width="150px"
                            HeaderTooltip="This column displays the notes for each record in the grid">
                            <ItemStyle Wrap="true" Width="70%" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="NotesCreatedBy" FilterControlAltText="Filter NotesCreatedBy column"
                            HeaderText="Created By" SortExpression="NotesCreatedBy" UniqueName="NotesCreatedBy"
                            HeaderTooltip="This column displays the notes created by for each record in the grid.">
                        </telerik:GridBoundColumn>

                        <telerik:GridDateTimeColumn DataField="NoteCreatedOn" FilterControlAltText="Filter NoteCreatedOn column"
                            HeaderText="Created On" FilterDateFormat="MM/dd/yyyy" SortExpression="NoteCreatedOn" UniqueName="NoteCreatedOn" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" AllowFiltering="true"
                            HeaderTooltip="This column displays the note created on for each record in the grid.">
                        </telerik:GridDateTimeColumn>

                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle Width="30px" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                        <FormTemplate>
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-12">
                                        <h2 class="header-color">
                                            <asp:Label ID="lblClientContactNotes" Text='<%# (Container is GridEditFormInsertItem) ? "Add Note " : "Update Note" %>'
                                                runat="server" /></h2>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="msgbox">
                                            <asp:Label runat="server" ID="lblName" CssClass="info"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <asp:Panel runat="server" CssClass="sxpnl pnlCCNotes" ID="pnlCCNotes">
                                    <div class="row bgLightGreen">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class='form-group col-md-4'>
                                                    <span class="cptn">Notes</span><span class='reqd'>*</span>
                                                    <infs:WclTextBox ID="txtNotes" MaxLength="1012" runat="server" TextMode="MultiLine" CssClass="borderTextArea" Width="100%" EnableAriaSupport="true">
                                                    </infs:WclTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvtxtNotes" ControlToValidate="txtNotes" SetFocusOnError="true"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Note is required."
                                                            ValidationGroup='grpClientContactNotes' />
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                    </div>
                                </asp:Panel>
                                <div class="col-md-12 text-right">
                                    <infsu:CommandBar ID="fsucCmdBarNode" runat="server" GridMode="true" GridInsertText="Save" GridUpdateText="Save"
                                        ValidationGroup="grpClientContactNotes" ExtraButtonIconClass="icnreset" UseAutoSkinMode="false" ButtonSkin="Silk" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>

                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" Position="TopAndBottom" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>

                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>

        </div>
    </asp:Panel>
</div>
<asp:HiddenField ID="hdnSelectedTenantID" runat="server" />
<asp:HiddenField ID="hdnCurrentloggedInUserId" runat="server" />


<script type="text/javascript">

    function OpenInstructorView(navUrl) {
        var win = window.open(navUrl, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        }
    }


</script>

