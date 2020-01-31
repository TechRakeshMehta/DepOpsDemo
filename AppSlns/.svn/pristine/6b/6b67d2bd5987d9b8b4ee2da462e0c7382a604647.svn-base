<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminAuditHistoryDiscardedDocuments.ascx.cs" Inherits="CoreWeb.ComplianceOperations.AdminAuditHistoryDiscardedDocuments" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxDiscardedDocAudit">
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
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">
                <asp:Label ID="lblDiscardedDocAudit" runat="server" Text="Discarded Document Audit"></asp:Label>
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel runat="server" ID="pnlShowFilters">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server" visible="false">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class='cptn'>Institution</span><span class="reqd">*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                DataTextField="TenantName" DataValueField="TenantID" EmptyMessage="--Select--"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Enabled="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" ValidationGroup="vgAuditHistory"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered ApplicantFirstName">
                        <label id="lblApplicantFirstName" class="cptn">Applicant First Name</label>
                        <infs:WclTextBox ID="txtApplicantFirstName" aria-labelledby="lblUName" EnableAriaSupport="true" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered ApplicantLastName">
                        <label id="lblApplicantLastName" class="cptn">Applicant Last Name</label>
                        <infs:WclTextBox ID="txtApplicantLastName" aria-labelledby="lblUName" EnableAriaSupport="true" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered UpdatedFrom">
                        <label id="lblDiscardedFrom" class="cptn">Discarded From</label>
                        <infs:WclDatePicker ID="dpTmStampFromDate" runat="server" DateInput-EmptyMessage="Select a date"
                            Width="100%" CssClass="form-control" DateInput-EnableAriaSupport="true" DateInput-SelectionOnFocus="CaretToBeginning"
                            ClientEvents-OnDateSelected="CorrectFrmToCrtdDate" EnableAriaSupport="true"
                            DateInput-DateFormat="MM/dd/yyyy">
                            <Calendar EnableKeyboardNavigation="true" EnableAriaSupport="true"></Calendar>
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered UpdatedTo">
                        <label id="lblDiscardedTo" class="cptn">Discarded To</label>
                        <infs:WclDatePicker ID="dpTmStampToDate" runat="server" DateInput-EmptyMessage="Select a date"
                            Width="100%" CssClass="form-control" DateInput-EnableAriaSupport="true" DateInput-SelectionOnFocus="CaretToBeginning"
                            ClientEvents-OnPopupOpening="SetMinDate" EnableAriaSupport="true"
                            DateInput-DateFormat="MM/dd/yyyy">
                            <Calendar EnableKeyboardNavigation="true" EnableAriaSupport="true"></Calendar>
                        </infs:WclDatePicker>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-12">
                <div class="row text-center">
                    <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbUndo" SubmitButtonText="Reset"
                        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
                        OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" CauseValidationOnCancel="false"
                        OnCancelClick="CmdBarCancel_Click" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>
</div>

<div>
    <infs:WclGrid runat="server" ID="grdApplicantDataAuditDiscadedDocs" AutoGenerateColumns="False"
        AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
        ClientSettings-AllowKeyboardNavigation="true" EnableAriaSupport="true" CssClass="gridhover" GridLines="Both"
        ShowAllExportButtons="false" OnNeedDataSource="grdApplicantDataAuditDiscadedDocs_NeedDataSource"
        OnSortCommand="grdApplicantDataAuditDiscadedDocs_SortCommand" OnPreRender="grdApplicantDataAuditDiscadedDocs_PreRender"
        OnItemCommand="grdApplicantDataAuditDiscadedDocs_ItemCommand" AllowCustomPaging="true">
        <ClientSettings EnableRowHoverStyle="false">
            <Selecting AllowRowSelect="false"></Selecting>
        </ClientSettings>
        <GroupingSettings CaseSensitive="false" />
        <ExportSettings Pdf-PageWidth="350mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
            OpenInNewWindow="true" Pdf-PageRightMargin="20mm">
        </ExportSettings>
        <MasterTableView AllowFilteringByColumn="true" CommandItemDisplay="Top" DataKeyNames="QueueRecordID">
            <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
            </RowIndicatorColumn>
            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
            </ExpandCollapseColumn>
            <Columns>
                <telerik:GridBoundColumn DataField="InstitutionName" FilterControlAltText="Filter InstitutionName column"
                    AllowFiltering="true" HeaderText="Institution" SortExpression="InstitutionName"
                    UniqueName="InstitutionName">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="StudentName" FilterControlAltText="Filter StudentName column"
                    AllowFiltering="true" HeaderText="Student Name" SortExpression="StudentName"
                    UniqueName="StudentName">
                </telerik:GridBoundColumn>
                <telerik:GridDateTimeColumn DataField="DiscardedOn" FilterControlAltText="Filter TimeStampValue column"
                    AllowFiltering="true" HeaderText="Discarded On" SortExpression="DiscardedOn" EnableTimeIndependentFiltering="true"
                    UniqueName="DiscardedOn" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                </telerik:GridDateTimeColumn>
                <telerik:GridBoundColumn DataField="DiscardedBy" FilterControlAltText="Filter DiscardedBy column"
                    AllowFiltering="true" HeaderText="Discarded By" SortExpression="DiscardedBy"
                    UniqueName="DiscardedBy">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DocumentName" FilterControlAltText="Filter DocumentName column"
                    AllowFiltering="true" HeaderText="Document Name" SortExpression="DocumentName"
                    UniqueName="DocumentName">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DiscardedReason" FilterControlAltText="Filter DiscardedReason column"
                    AllowFiltering="true" HeaderText="Discarded Reason" SortExpression="DiscardedReason"
                    UniqueName="DiscardedReason">
                </telerik:GridBoundColumn>
                  <telerik:GridBoundColumn DataField="DiscardedCount" FilterControlAltText="Filter DiscardedCount column"
                    AllowFiltering="true" HeaderText="Discarded Count" SortExpression="DiscardedCount"
                    UniqueName="DiscardedCount">
                </telerik:GridBoundColumn>
                
            </Columns>
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
        </MasterTableView>
        <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
        <FilterMenu EnableImageSprites="False">
        </FilterMenu>
    </infs:WclGrid>
</div>
<script type="text/javascript">
    var minDate = new Date("01/01/1980");
    function CorrectFrmToCrtdDate(picker) {
        var date1 = $jQuery("[id$=dpTmStampFromDate]")[0].control.get_selectedDate();
        var date2 = $jQuery("[id$=dpTmStampToDate]")[0].control.get_selectedDate();
        if (date1 != null && date2 != null) {
            if (date1 > date2)
                $jQuery("[id$=dpTmStampToDate]")[0].control.set_selectedDate(null);
        }
    }

    function SetMinDate(picker) {
        var date = $jQuery("[id$=dpTmStampFromDate]")[0].control.get_selectedDate();
        if (date != null) {
            picker.set_minDate(date);
        }
        else {
            picker.set_minDate(minDate);
        }
    }
</script>
