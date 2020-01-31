<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementDataAuditHistory.ascx.cs" Inherits="CoreWeb.ClinicalRotation.RequirementDataAuditHistory" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:wclresourcemanagerproxy runat="server" id="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:wclresourcemanagerproxy>





<div id="dvTop" class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Admin Requirement Data Audit History</h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div id="divTenant">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class="reqd">*</span>
                            <infs:wclcombobox id="ddlTenantName" runat="server" datatextfield="TenantName" emptymessage="--Select--" onselectedindexchanged="ddlTenantName_SelectedIndexChanged"
                                datavaluefield="TenantID" filter="Contains" onclientkeypressing="openCmbBoxOnTab" autopostback="true" enabled="false"
                                width="100%" cssclass="form-control" skin="Silk" autoskinmode="false">
                            </infs:wclcombobox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvInstitution" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" ValidationGroup="vgAuditHistory"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected User Group">
                        <span class="cptn">User Group</span>
                        <infs:wclcombobox id="ddlUserGroup" runat="server" datatextfield="UG_Name" datavaluefield="UG_ID"
                            emptymessage="--Select--" autopostback="false" width="100%" cssclass="form-control"
                            skin="Silk" autoskinmode="false" filter="Contains" onclientkeypressing="openCmbBoxOnTab">
                        </infs:wclcombobox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Applicant First Name">
                        <span class="cptn">Applicant First Name</span>
                        <infs:wcltextbox id="txtApplicantFirstName" runat="server" width="100%" cssclass="form-control">
                        </infs:wcltextbox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Applicant Last Name">
                        <span class='cptn'>Applicant Last Name</span>
                        <infs:wcltextbox id="txtApplicantLastName" runat="server" width="100%" cssclass="form-control">
                        </infs:wcltextbox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the selected requirement package type">
                        <span class="cptn">Package Type</span>
                         <infs:wclcombobox id="cmbPackageType" runat="server" datatextfield="Name" emptymessage="--Select--" onselectedindexchanged="cmbPackageType_SelectedIndexChanged"
                                datavaluefield="ID" filter="Contains" onclientkeypressing="openCmbBoxOnTab" autopostback="true" 
                                width="100%" cssclass="form-control" skin="Silk" autoskinmode="false">
                            </infs:wclcombobox>
                       <%-- <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvPackageType" ControlToValidate="cmbPackageType"
                                    InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" ValidationGroup="vgAuditHistory"
                                    Text="Package Type is required." />
                            </div>--%>
                       <%-- <infs:wclcombobox id="cmbPackageType" runat="server" datatextfield="Name" emptymessage="--SELECT--" width="100%" cssclass="form-control"
                            datavaluefield="ID" filter="None" OnSelectedIndexChanged="cmbPackageType_SelectedIndexChanged" onclientkeypressing="openCmbBoxOnTab" skin="Silk" autoskinmode="false">
                        </infs:wclcombobox>--%>
                    </div>
                    <%--<div class='form-group col-md-3' title="Restrict search results to the selectd package">
                        <span class="cptn">Package Type</span>
                        <infs:WclComboBox ID="ddlPackageTypes" runat="server" DataTextField="Name" DataValueField="ID" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            EmptyMessage="--Select--" AutoPostBack="false" Width="100%" CssClass="form-control" OnClientItemChecked="OnClientItemChecked" OnClientBlur="OnClientItemChecked" Skin="Silk" AutoSkinMode="false"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnClientDropDownClosed="triggerBtnPkgTypeClick">
                        </infs:WclComboBox>
                        <infs:WclButton ID="btnPackageTypeChange" runat="server" OnClick="btnPackageTypeChange_Click" Style="display: none;"></infs:WclButton>
                    </div>--%>
                    <div class='form-group col-md-3' title="Restrict search results to the selectd package">
                        <span class="cptn">Package</span>
                        <infs:wclcombobox id="ddlPackages" runat="server" datatextfield="RequirementPackageName" datavaluefield="RequirementPackageID" checkboxes="true" enablecheckallitemscheckbox="true"
                            emptymessage="--Select--" autopostback="false" width="100%" cssclass="form-control" onclientitemchecked="OnClientItemChecked" onclientblur="OnClientItemChecked" skin="Silk" autoskinmode="false"
                            filter="Contains" onclientkeypressing="openCmbBoxOnTab" onclientdropdownclosed="triggerBtnClick">
                        </infs:wclcombobox>
                        <infs:wclbutton id="btnPackageChange" runat="server" onclick="btnPackageChange_Click" style="display: none;"></infs:wclbutton>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selectd category">
                        <span class="cptn">Category</span>
                        <infs:wclcombobox id="ddlCategory" runat="server" datatextfield="RequirementCategoryName" datavaluefield="RequirementCategoryID" checkboxes="true" enablecheckallitemscheckbox="true"
                            emptymessage="--Select--" autopostback="false" width="100%" cssclass="form-control" onclientdropdownclosed="triggerCategoryChangeBtnClick"
                            skin="Silk" autoskinmode="false" filter="Contains" onclientkeypressing="openCmbBoxOnTab">
                        </infs:wclcombobox>
                        <infs:wclbutton id="btnCategoryChange" runat="server" onclick="btnCategoryChange_Click" style="display: none;"></infs:wclbutton>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the selected item">
                        <span class="cptn">Item</span>
                        <infs:wclcombobox id="ddlItem" runat="server" datatextfield="RequirementItemName" datavaluefield="RequirementItemID"
                            autopostback="false" width="100%" cssclass="form-control" filter="Contains"
                            skin="Silk" autoskinmode="false" onclientkeypressing="openCmbBoxOnTab">
                        </infs:wclcombobox>
                    </div>
                    <%--UAT-3117--%>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Complio ID">
                        <span class="cptn">Complio ID</span>
                        <infs:wcltextbox id="txtComplioId" runat="server" width="100%" cssclass="form-control">
                        </infs:wcltextbox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Admin First Name">
                        <span class="cptn">Admin First Name</span>
                        <infs:wcltextbox id="txtAdminFirstName" runat="server" width="100%" cssclass="form-control">
                        </infs:wcltextbox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered Admin Last Name">
                        <span class="cptn">Admin Last Name</span>
                        <infs:wcltextbox id="txtAdminLastName" runat="server" width="100%" cssclass="form-control">
                        </infs:wcltextbox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the Updated From">
                        <span class="cptn">Updated From</span>
                        <infs:wcldatepicker id="dpTmStampFromDate" runat="server" width="100%" cssclass="form-control"
                            dateinput-emptymessage="Select a date" clientevents-ondateselected="CorrectFrmToCrtdDate">
                        </infs:wcldatepicker>
                    </div>
                </div>
            </div>

            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Updated To">
                        <span class="cptn">Updated To</span>
                        <infs:wcldatepicker id="dpTmStampToDate" runat="server" dateinput-emptymessage="Select a date"
                            clientevents-onpopupopening="SetMinDate" width="100%" cssclass="form-control">
                        </infs:wcldatepicker>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="col-md-12">&nbsp;</div>
        <div class="col-md-12">
            <div class="row text-center">
                <infsu:commandbar id="fsucCmdBarButton" runat="server" buttonposition="Center" displaybuttons="Submit,Save,Cancel"
                    autopostbackbuttons="Submit,Save,Cancel" submitbuttoniconclass="rbUndo"
                    submitbuttontext="Reset" savebuttontext="Search" savebuttoniconclass="rbSearch"
                    cancelbuttontext="Cancel" onsubmitclick="fsucCmdBarButton_SubmitClick" onsaveclick="fsucCmdBarButton_SaveClick"
                    oncancelclick="fsucCmdBarButton_CancelClick" useautoskinmode="false" buttonskin="Silk">
                </infsu:commandbar>
            </div>
            <div class="col-md-12">&nbsp;</div>
        </div>
    </div>
    <div class="row allowscroll">
        <div id="dvGrdRotations" runat="Server">
            <infs:wclgrid runat="server" id="grdDataAudit" allowcustompaging="true"
                autogeneratecolumns="False" allowsorting="true" allowfilteringbycolumn="false"
                autoskinmode="true" cellspacing="0" gridlines="Both" showallexportbuttons="false"
                onneeddatasource="grdDataAudit_NeedDataSource" onitemcommand="grdDataAudit_ItemCommand"
                onsortcommand="grdDataAudit_SortCommand" onprerender="grdDataAudit_PreRender"
                enabledefaultfeatures="True" nonexportingcolumns=""
                showclearfiltersbutton="false">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                    Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                    ExportOnlyData="true" IgnorePaging="true">
                </ExportSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ApplicantRequirementDataAuditID"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false"
                        ShowExportToCsvButton="true"
                        ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                    <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="TimeStampValue" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ApplicantName" HeaderText="Applicant Name" SortExpression="ApplicantName"
                            HeaderTooltip="This column displays the Applicant Name for each record in the grid"
                            UniqueName="ApplicantName">
                        </telerik:GridBoundColumn>
                        <%-- UAT-3117--%>
                        <telerik:GridBoundColumn DataField="ComplioId" HeaderText="Complio ID" SortExpression="ComplioId"
                            HeaderTooltip="This column displays the Complio ID for each record in the grid"
                            UniqueName="ComplioId">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                            AllowFiltering="false" HeaderText="Package Name" SortExpression="PackageName"
                            UniqueName="PackageName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter CategoryName column"
                            HeaderText="Category Name" SortExpression="CategoryName" UniqueName="CategoryName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ItemName" FilterControlAltText="Filter ItemName column"
                            AllowFiltering="false" HeaderText="Item Name" SortExpression="ItemName" UniqueName="ItemName">
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="TimeStampValue" FilterControlAltText="Filter TimeStampValue column"
                            AllowFiltering="false" HeaderText="Updated On" SortExpression="TimeStampValue"
                            UniqueName="TimeStampValue" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                        </telerik:GridDateTimeColumn>
                        <telerik:GridBoundColumn DataField="ChangeBY" FilterControlAltText="Filter ChangeBy column"
                            AllowFiltering="false" HeaderText="Updated By " SortExpression="ChangeBy" UniqueName="ChangeBy">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ChangeValue" FilterControlAltText="Filter Change column"
                            AllowFiltering="false" HeaderText="Change" UniqueName="ChangeTemp" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Change" UniqueName="ChangeValue" FilterControlAltText="Filter ChangeValue column"
                            AllowFiltering="false">
                            <ItemTemplate>
                                <asp:Label ID="lblChangeValue" runat="server" Text='<%# Convert.ToString(Eval("ChangeValue")).Length > 100 ?  INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString( Eval("ChangeValue")).Substring(0, 100)) + "...." : INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ChangeValue")))%>'></asp:Label>
                                <infs:WclToolTip runat="server" ID="tltpChangeValue" TargetControlID="lblChangeValue"
                                    Width="300px" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ChangeValue"))) %>' ManualClose="false"
                                    RelativeTo="Element" Position="TopCenter" Visible='<%# Eval("ChangeValue").ToString().Trim()==String.Empty ? false : Convert.ToString(Eval("ChangeValue")).Length > 100?true:false %>'>
                                </infs:WclToolTip>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                        Position="TopAndBottom" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:wclgrid>
        </div>
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

    function SetMinDate(picker) {
        var date = $jQuery("[id$=dpTmStampFromDate]")[0].control.get_selectedDate();
        if (date != null) {
            picker.set_minDate(date);
        }
        else {
            picker.set_minDate(minDate);
        }
    }

    function OnClientItemChecked(sender, args) {
        if (sender.get_checkedItems().length == 0) {
            sender.clearSelection();
            sender.set_emptyMessage("--Select--");
        }
    }

    function triggerBtnClick() {
        $jQuery("[id$=btnPackageChange]").click();
    }


    function triggerBtnPkgTypeClick() {
        $jQuery("[id$=btnPackageTypeChange]").click();
    }



    function triggerCategoryChangeBtnClick() {
        $jQuery("[id$=btnCategoryChange]").click();
    }

</script>
