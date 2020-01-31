<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminDataAuditHistory.ascx.cs" Inherits="CoreWeb.ComplianceOperations.AdminDataAuditHistory" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    .RadToolTip {
        border: 1px solid Gray !important;
        background-color: Gray !important;
    }

    .RadGrid .rgHoveredRow {
        background: #dadada !important;
        border: none none none none !important;
        border-color: #dadada !important;
    }
</style>
<div class="section">
    <h1 class="mhdr">Admin Document Audit History</h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <div class="sxform auto" id="divSearchPanel">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlShowFilters">
                    <div class='sxro sx3co'>
                        <div id="divTenant" runat="server" visible="false">
                            <div class='sxlb'>
                                <span class="cptn">Institution</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="ddlTenantName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                    DataTextField="TenantName" DataValueField="TenantID" EmptyMessage="--Select--"
                                    Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Enabled="false">
                                </infs:WclComboBox>
                                <div class="vldx">
                                    <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                        InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" ValidationGroup="vgAuditHistory"
                                        Text="Institution is required." />
                                </div>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Document Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtDocumentName" runat="server"></infs:WclTextBox>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Applicant First Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtApplicantFirstName" runat="server">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Applicant Last Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtApplicantLastName" runat="server">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Admin First Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtAdminFirstName" runat="server">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Admin Last Name</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox ID="txtAdminLastName" runat="server">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Updated From</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclDatePicker ID="dpTmStampFromDate" runat="server" DateInput-EmptyMessage="Select a date"
                                ClientEvents-OnDateSelected="CorrectFrmToCrtdDate">
                            </infs:WclDatePicker>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Updated To</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclDatePicker ID="dpTmStampToDate" runat="server" DateInput-EmptyMessage="Select a date"
                                ClientEvents-OnPopupOpening="SetMinDate">
                            </infs:WclDatePicker>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Action Type</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox ID="ddlActionType" runat="server" 
                                DataTextField="LDEDS_Name" DataValueField="LDEDS_ID" EmptyMessage="--Select--" Filter="Contains" >
                            </infs:WclComboBox>
                        </div>
                         <div class='sxlb'>
                            <span class="cptn">Discard Reason</span>
                        </div>
                        <div class='sxlm'>
                             <infs:WclComboBox ID="ddlDiscardReason" runat="server" AutoPostBack="false"
                                    DataTextField="DDR_Name" DataValueField="DDR_ID" EmptyMessage="--Select--"
                                    Filter="Contains">
                                </infs:WclComboBox>

                        </div>
                    </div>
                    <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                        AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset"
                        SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel"
                        OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" CauseValidationOnCancel="false"
                        OnCancelClick="CmdBarCancel_Click">
                    </infsu:CommandBar>
                </asp:Panel>
            </div>
        </div>
        <div>
            <infs:WclGrid runat="server" ID="grdApplicantDataAudit" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="False" AutoSkinMode="True" CellSpacing="0" NonExportingColumns="Changes"
                GridLines="None" ShowAllExportButtons="false" OnNeedDataSource="grdApplicantDataAudit_NeedDataSource"
                OnSortCommand="grdApplicantDataAudit_SortCommand" OnPreRender="grdApplicantDataAudit_PreRender"
                OnItemCommand="grdApplicantDataAudit_ItemCommand" AllowCustomPaging="true" ShowClearFiltersButton="false">
                <ClientSettings EnableRowHoverStyle="false">
                    <Selecting AllowRowSelect="false"></Selecting>
                </ClientSettings>
                <GroupingSettings CaseSensitive="false" />
                <ExportSettings Pdf-PageWidth="350mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                    OpenInNewWindow="true" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <MasterTableView AllowFilteringByColumn="false" CommandItemDisplay="Top" DataKeyNames="QueueRecordID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                        ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>

                         <telerik:GridBoundColumn DataField="DocumentName" FilterControlAltText="Filter DocumentName column"
                            AllowFiltering="false" HeaderText="Document Name" SortExpression="DocumentName"
                            UniqueName="DocumentName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantName column"
                            AllowFiltering="false" HeaderText="Applicant First Name" SortExpression="ApplicantFirstName"
                            UniqueName="ApplicantFirstName">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantName column"
                            AllowFiltering="false" HeaderText="Applicant Last Name" SortExpression="ApplicantLastName"
                            UniqueName="ApplicantLastName">
                        </telerik:GridBoundColumn>

                         <telerik:GridBoundColumn DataField="School" FilterControlAltText="Filter School column"
                            AllowFiltering="false" HeaderText="School" SortExpression="School"
                            UniqueName="School">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="AdminFirstName" FilterControlAltText="Filter Admin First Name column"
                            AllowFiltering="false" HeaderText="Admin Name" SortExpression="AdminFirstName"
                            UniqueName="AdminFirstName">
                        </telerik:GridBoundColumn>

                         <telerik:GridBoundColumn DataField="AssignToName" FilterControlAltText="Filter AssignTo Name column"
                            AllowFiltering="false" HeaderText="Assigned To" SortExpression="AssignToName"
                            UniqueName="AssignToName">
                        </telerik:GridBoundColumn>

                         <telerik:GridBoundColumn DataField="AssignByName" FilterControlAltText="Filter AssignTo Name column"
                            AllowFiltering="false" HeaderText="Assigned By" SortExpression="AssignByName"
                            UniqueName="AssignByName">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="AssignOn" FilterControlAltText="Filter AssignOn Name column"
                            AllowFiltering="false" HeaderText="Assigned On" SortExpression="AssignOn"
                            UniqueName="AssignOn">
                        </telerik:GridBoundColumn>

                       <%-- <telerik:GridBoundColumn DataField="ActionType" FilterControlAltText="Filter Action Type column"
                            AllowFiltering="false" HeaderText="Action Type" SortExpression="ActionType"
                            UniqueName="ActionType">
                        </telerik:GridBoundColumn>--%>

                        <%-- <telerik:GridBoundColumn DataField="DiscardReason" FilterControlAltText="Filter discard reason column"
                            AllowFiltering="false" HeaderText="Discard Reason" SortExpression="ActionType"
                            UniqueName="DiscardReason">
                        </telerik:GridBoundColumn>--%>

                       
                        <telerik:GridDateTimeColumn DataField="CreatedOn" FilterControlAltText="Filter TimeStampValue column"
                            AllowFiltering="false" HeaderText="Updated On" SortExpression="CreatedOn"
                            UniqueName="CreatedOn" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                        </telerik:GridDateTimeColumn>

                       <%-- <telerik:GridBoundColumn DataField="ChangeBY" FilterControlAltText="Filter ChangeBy column"
                            AllowFiltering="false" HeaderText="Updated By " SortExpression="ChangeBy" UniqueName="ChangeBy">
                        </telerik:GridBoundColumn>--%>

                        <telerik:GridBoundColumn DataField="Changes" FilterControlAltText="Filter Change column"
                            AllowFiltering="false" HeaderText="Change" UniqueName="ChangeTemp" Display="false">
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderText="Change" UniqueName="Changes" FilterControlAltText="Filter ChangeValue column"
                            AllowFiltering="false">
                            <ItemTemplate>
                                <asp:Label ID="lblChangeValue" runat="server" Text='<%# Convert.ToString(Eval("Changes")).Length > 100 ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Changes")).Substring(0, 100)) + "...." : INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Changes")))%>'></asp:Label>
                                <infs:WclToolTip runat="server" ID="tltpChangeValue" TargetControlID="lblChangeValue"
                                    Width="300px" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Changes"))) %>' ManualClose="false"
                                    RelativeTo="Element" Position="TopCenter" Visible='<%# Eval("Changes").ToString().Trim()==String.Empty ? false : Convert.ToString(Eval("Changes")).Length > 100?true:false %>'>
                                </infs:WclToolTip>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
            <div class="gclr">
            </div>
        </div>
    </div>
</div>
<asp:HiddenField ID="hdnIsPackageSelected" runat="server" />
<asp:HiddenField ID="hdnIsCategorySelected" runat="server" />
<asp:HiddenField runat="server" ID="hdnPreviousSelectedPackageValues" />
<div id="dialog-form" title="Change Value">
    <div id="divContent" style="text-align: left;">
    </div>
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

    var oldSelectedIdList = [];

    function OnPackageClosed(sender, eventArgs) {
        if (areThereAnyChangesAtTheSelection(sender)) {
            $jQuery("[id$=hdnIsPackageSelected]").val("1");
            $jQuery("[id$=hdnIsCategorySelected]").val("0");
            __doPostBack('ddlPackage', '');
        }
    }

    function areThereAnyChangesAtTheSelection(sender) {
        var hdnPreviousSelectedPackageValues = $jQuery("[id$=hdnPreviousSelectedPackageValues]");
        if (hdnPreviousSelectedPackageValues.val() != "" && hdnPreviousSelectedPackageValues.val() != null && hdnPreviousSelectedPackageValues.val() != undefined) {
            oldSelectedIdList = hdnPreviousSelectedPackageValues.val().split(',');
        }
        var selectedIdList = radComboBoxSelectedIdList(sender);

        var oldIdListMINUSNewIdList = $jQuery(oldSelectedIdList).not(selectedIdList).get();
        var newIdListMINUSOldIdList = $jQuery(selectedIdList).not(oldSelectedIdList).get();
        if (oldIdListMINUSNewIdList.length != 0 || newIdListMINUSOldIdList.length != 0)
            return true;

        return false;
    }

    function radComboBoxSelectedIdList(sender) {
        var selectedIdList = [];
        var combo = sender;
        combo.get_checkedItems().forEach(function (item) {
            selectedIdList.push(item.get_value());
        });

        return selectedIdList;
    }
    function OnCategoryClosed(sender, eventArgs) {
        if (sender.get_items()._array.length != 0) {
            $jQuery("[id$=hdnIsPackageSelected]").val("0");
            $jQuery("[id$=hdnIsCategorySelected]").val("1");
            __doPostBack('ddlCategory', '');
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
