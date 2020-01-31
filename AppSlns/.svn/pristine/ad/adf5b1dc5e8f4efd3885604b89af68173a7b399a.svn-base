<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchOpportunities.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.SearchOpportunities" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/ColumnsConfiguration.ascx" TagPrefix="infsu" TagName="ColumnsConfiguration" %>
<%@ Register TagPrefix="uc" TagName="AgencyHierarchyMultiple" Src="~/AgencyHierarchy/UserControls/AgencyHierarchyMultipleSelection.ascx" %>
<%@ Register Src="~/ClinicalRotation/UserControl/SharedUserCustomAttributeForm.ascx" TagPrefix="ucCustom" TagName="CustomAttributes" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxSearchOpportunities">
    <infs:LinkedResource Path="~/Resources/Mod/AgencyHierarchy/AgencyHierarchyMultipleSelection.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Generic/ColumnsConfiguration.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>


<style type="text/css">
    .lnkBtn {
        background-color: transparent !important;
        padding-top: 0px !important;
    }

        .lnkBtn.rbLinkButton:hover {
            background-color: transparent !important;
        }

    /*UAT-3211 Rotation Tab*/
    .section, .tabvw {
        padding-bottom: 27px;
    }

    .fa-download {
        margin-left: 8px;
    }

    /*Need to Remove*/
    .left {
        float: left !important;
    }

    .width {
        width: auto;
    }

    .padding {
        padding-top: 8px !important;
        padding-right: 15px !important;
    }
    /*Remove upto here*/
</style>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblSearchOpportunities" runat="server" Text="Search Opportunities"></asp:Label>
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel runat="server" ID="pnlOpportunitiesSearch">
            <div class="col-md-12">
                <div class="col-md-12">&nbsp;</div>
                <div class="col-md-12">
                    <div class="row">
                        <div class="form-group col-md-3">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" AutoPostBack="true"
                                DataTextField="TenantName" DataValueField="TenantID"
                                OnDataBound="ddlTenantName_DataBound" OnSelectedIndexChanged="ddlTenantName_SelectedIndexChanged"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control"
                                Skin="Silk" AutoSkinMode="false" EnableAriaSupport="true">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" InitialValue="--SELECT--"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <%--<div class="form-group col-md-3" style="margin-top: 20px;display:none">
                            <uc:AgencyHierarchyMultiple ID="ucAgencyHierarchyMultiple" runat="server" Visible="false" />
                        </div>--%>
                        <div class="form-group col-md-3">
                            <span class="cptn">Location</span>
                            <infs:WclComboBox ID="ddlLocation" runat="server" DataTextField="Location" EmptyMessage="--Select--" AutoPostBack="true"
                                AllowCustomText="true" CausesValidation="false" DataValueField="AgencyLocationID" 
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                        </div>
                        <div class="form-group col-md-3">
                            <span class="cptn">Department</span>
                            <infs:WclComboBox ID="ddlDepartment" runat="server" DataTextField="Name" EmptyMessage="--Select--"
                                AllowCustomText="true" CausesValidation="false" DataValueField="DepartmentID"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                        </div>
                        <div class="form-group col-md-3">
                            <span class="cptn">Start Date</span>
                            <infs:WclDatePicker ID="dtStartDate" Width="100%" CssClass="form-control" runat="server">
                            </infs:WclDatePicker>
                        </div>
                        <div class="form-group col-md-3">
                            <span class="cptn">End Date</span>
                            <infs:WclDatePicker ID="dtEndDate" Width="100%" CssClass="form-control" runat="server">
                            </infs:WclDatePicker>
                        </div>
                    </div>

                    <div class="row">
                        <div class="form-group col-md-3">
                            <span class="cptn">Specialty</span>
                            <%--<infs:WclTextBox ID="txtSpecialty" Width="100%" CssClass="form-control" runat="server">
                            </infs:WclTextBox>--%>
                            <infs:WclComboBox ID="ddlSpecialty" runat="server" DataTextField="Name" EmptyMessage="--Select--"
                                AllowCustomText="true" CausesValidation="false" DataValueField="SpecialtyID"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false" EnableCheckAllItemsCheckBox="true">
                            </infs:WclComboBox>
                        </div>
                        <div class='form-group col-md-3' title="Restrict search results to the selected student types">
                            <span class='cptn'>Students Type</span>
                            <infs:WclComboBox ID="ddlStudentType" runat="server" DataTextField="Name" EmptyMessage="--Select--"
                                AllowCustomText="true" CausesValidation="false" DataValueField="StudentTypeId" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                        </div>

                        <div class="form-group col-md-3">
                            <span class="cptn">Max #</span>
                            <infs:WclNumericTextBox ID="txtMax" Width="100%" CssClass="form-control" runat="server" NumberFormat-DecimalDigits="0">
                            </infs:WclNumericTextBox>
                        </div>
                        
                    </div>
                    <div class="row">
                        <div class="form-group col-md-3">
                            <span class="cptn">Days</span>
                            <infs:WclComboBox ID="ddlDays" runat="server" Width="100%" CssClass="form-control"
                                CheckBoxes="true" EmptyMessage="--SELECT--" Skin="Silk" AutoSkinMode="false" Filter="Contains"
                                DataValueField="WeekDayID" DataTextField="Name">
                            </infs:WclComboBox>
                        </div>
                        <div class="form-group col-md-3">
                            <span class="cptn">Shift</span>

                            <infs:WclComboBox ID="ddlShift" runat="server" DataTextField="Shift" EmptyMessage="--Select--"
                                AllowCustomText="true" CausesValidation="false" DataValueField="ClinicalInventoryShiftID" CheckBoxes="false" EnableCheckAllItemsCheckBox="false"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>

                        </div>
                    </div>

                    <div class="col-md-12">
                        <ucCustom:CustomAttributes ID="caCustomAttributesID" EnableViewState="false" runat="server" />
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
                    <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Extra,Cancel"
                        AutoPostbackButtons="Submit,Extra,Cancel" SubmitButtonText="Search" SubmitButtonIconClass="rbSearch"
                        CancelButtonText="Cancel" ExtraButtonIconClass="rbUndo" ExtraButtonText="Reset" DefaultPanel="pnlOpportunitiesSearch"
                        OnSubmitClick="fsucCmdBarButton_SearchClick" OnExtraClick="fsucCmdBarButton_ResetClick" ValidationGroup="grpFormSubmit"
                        OnCancelClick="fsucCmdBarButton_CancelClick" UseAutoSkinMode="False" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>


    <div id="dvColumnsConfiguration" style="display: none">
        <infsu:ColumnsConfiguration runat="server" ID="ColumnsConfiguration" />
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdSearchOpportunities" AllowPaging="true" AutoGenerateColumns="false" CssClass="gridhover containsColumnsConfiguration"
            AllowSorting="true" AllowFilteringByColumn="false" AutoSkinMode="true" CellSpacing="0" GridLines="Both" GridCode="AAAK"
            ShowAllExportButtons="false" ShowExtraButtons="false" ValidationGroup="grdSearchOpportunities" AllowCustomPaging="false"
            PageSize="10" OnNeedDataSource="grdSearchOpportunities_NeedDataSource" OnItemCommand="grdSearchOpportunities_ItemCommand" OnSortCommand="grdSearchOpportunities_SortCommand"
            OnItemDataBound="grdSearchOpportunities_ItemDataBound">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OpportunityID" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <AlternatingItemStyle BackColor="#f2f2f2" />

                <Columns>
                    <%--  <telerik:GridNumericColumn DataField="Id" FilterControlAltText="Filter CPO_ID column"
                        HeaderText="Id" SortExpression="Id" DataType="System.Int32" UniqueName="Id"
                        HeaderTooltip="This column displays the Id">
                    </telerik:GridNumericColumn>--%>
                    <telerik:GridNumericColumn DataField="Agency" FilterControlAltText="Filter Agency column"
                        HeaderText="Agency" SortExpression="Agency" UniqueName="Agency"
                        HeaderTooltip="This column displays the Agency Name">
                    </telerik:GridNumericColumn>

                    <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                        HeaderText="Location" SortExpression="Location" UniqueName="Location"
                        HeaderTooltip="This column displays the Location for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter Department column"
                        HeaderText="Dept." SortExpression="Department" UniqueName="Department"
                        HeaderTooltip="This column displays the Department for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="Specialty" FilterControlAltText="Filter Specialty column"
                        HeaderText="Speciality" SortExpression="Specialty" UniqueName="Specialty"
                        HeaderTooltip="This column displays the Specialty for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="StudentTypes" FilterControlAltText="Filter StudentType column"
                        HeaderText="Student Type" SortExpression="StudentTypes" UniqueName="StudentTypes"
                        HeaderTooltip="This column displays the Student Type for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Max" FilterControlAltText="Filter Max column"
                        HeaderText="Max #" SortExpression="Max" UniqueName="Max"
                        HeaderTooltip="This column displays the Max for each record in the grid">
                    </telerik:GridBoundColumn>
                    <%--  <telerik:GridBoundColumn DataField="Max" FilterControlAltText="Filter Max column"
                        HeaderText="Max #" SortExpression="Max" UniqueName="Max"
                        HeaderTooltip="This column displays the Max for each record in the grid">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridBoundColumn DataField="Days" FilterControlAltText="Filter Days column"
                        HeaderText="Days" SortExpression="Days" UniqueName="Days"
                        HeaderTooltip="This column displays the Days for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Shift" FilterControlAltText="Filter Shift column"
                        HeaderText="Shift" SortExpression="Shift" UniqueName="Shift"
                        HeaderTooltip="This column displays the Shift for each record in the grid">
                    </telerik:GridBoundColumn>
                    <%--<telerik:GridBoundColumn DataField="GroupName" FilterControlAltText="Filter Groups column"
                        HeaderText="Groups" SortExpression="GroupName" UniqueName="GroupName"
                        HeaderTooltip="This column displays the Groups for each record in the grid">
                    </telerik:GridBoundColumn>--%>

                    <telerik:GridButtonColumn ButtonType="PushButton" CommandName="CreateDraft"
                        FilterControlAltText="Filter DeleteColumn column" HeaderText="Draft Request" Text="Create Draft Request"
                        UniqueName="CreateDraft" Resizable="false">
                        <HeaderStyle CssClass="rgHeader ButtonColumnHeader"></HeaderStyle>
                        <ItemStyle CssClass="ButtonColumn" />
                    </telerik:GridButtonColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" Position="TopAndBottom" />
            </MasterTableView>
        </infs:WclGrid>
    </div>
</div>

<asp:Button ID="btnDoPostback" runat="server" CssClass="buttonHidden" OnClick="btnDoPostback_Click" />
<asp:HiddenField runat="server" ID="hdnOpportunityId" />
<asp:HiddenField ID="hdnPageRequested" runat="server" />
<asp:HiddenField ID="hdnIsSearchClicked" runat="server" />
<asp:HiddenField ID="hdnSelectedTenantID" runat="server" />

<asp:HiddenField ID="hdnRequestSavedSuccessfully" runat="server" />
<asp:HiddenField ID="hdnRequestPublishedSuccessfully" runat="server" />

<script type="text/javascript">

    //$page.add_pageLoad(function () {

    //    AddColumnConfiguration();
    //    //on clicking column configuration we are dispaying column configuration div.
    //    $jQuery('.containsColumnsConfigurationTest .ColConfigBtn').on('click', function () {
    //        var ExportDiv = $jQuery(".containsColumnsConfigurationTest .WclGrid-ExportOptions");
    //        if (ExportDiv.length == 0 || ExportDiv.length == undefined) {
    //            ExportDiv.empty();
    //            var ColumnConfig = $jQuery("[id$=dvColumnsConfigurationTest]").css("display", "block");

    //            $jQuery(".containsColumnsConfigurationTest .grdCmdBar").after(ColumnConfig);

    //        }
    //    });
    //});

    //function AddColumnConfiguration() {
    //    //debugger;
    //    //$jQuery(".containsColumnsConfiguration .grdCmdGrp").append("");
    //    $jQuery(".containsColumnsConfigurationTest .grdCmdGrp").append
    //        ("<span class='ColConfigBtn rbText rbPrimary' style='cursor:pointer; padding-left:6px; padding-bottom:2px;'><span class='fa fa-cog Configuration-Color'></span>Column(s) Configuration</span>");
    //}


    function SearchOpportunityPopUp() {

        var composeScreenWindowName = "Create Draft";
        //var screenName = "CommonScreen";
        var opportunityId = $jQuery("[id$=hdnOpportunityId]").val();
        var RequestedPage = $jQuery("[id$=hdnPageRequested]").val();
        var SelectedTenantID = $jQuery("[id$=hdnSelectedTenantID]").val();

        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/PlacementMatching/Pages/CreateRequestPopup.aspx?OpportunityId=" + opportunityId + "&RequestedPage=" + RequestedPage + "&SelectedTenantID=" + SelectedTenantID);
        var win = $window.createPopup(url, { size: "1000," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function OnClientClose(oWnd, args) {

        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                //do button postback to show success message.
                $jQuery("[id$=hdnRequestSavedSuccessfully]").val(arg.RequestSavedSuccessfully);
                $jQuery("[id$=hdnRequestPublishedSuccessfully]").val(arg.RequestPublishedSuccessfully);
                $jQuery("#<%=btnDoPostback.ClientID %>").trigger('click');
            }
            winopen = false;
        }
    }
</script>

