<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.OrderQueue"
    CodeBehind="OrderQueue.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<%@ Register TagName="InstituteHierarchy" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/InstitutionHierarchyMultipleNodeSearch.ascx" %>--%>



<style type="text/css">
    .buttonHidden {
        display: none;
    }

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

    .chkAllResults {
        width: 10%;
        float: left;
        margin-top: 10px;
        padding-left: 7px;
    }

    .sxcbar {
        padding-left: 0px !important;
        width: 89%;
    }
</style>

<infs:WclResourceManagerProxy runat="server" ID="rprxOrderQueue">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/OrderQueue.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color" tabindex="0">
                <asp:Label ID="lblOrderQueue" runat="server" Text=""></asp:Label></h2>
        </div>
    </div>
    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel runat="server" ID="pnlShowFilters">
            <div class="col-md-12">
                <div class="row">
                    <div id="divTenant" runat="server" visible="false">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class='cptn'>Institution</span><span class="reqd">*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" AutoPostBack="true" OnItemSelected="ddlTenantName_ItemSelected"
                                DataTextField="TenantName" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_ItemSelected"
                                CausesValidation="false" OnDataBound="ddlTenantName_DataBound"
                                Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" Width="100%" CssClass="form-control"
                                Skin="Silk" AutoSkinMode="false" EnableAriaSupport="true">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName"
                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>
                    <%--<infs:WclDropDownList ID="ddlTenantName" runat="server" AutoPostBack="true" OnItemSelected="ddlTenantName_ItemSelected"
                                DataTextField="TenantName" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_ItemSelected"
                                CausesValidation="false" OnDataBound="ddlTenantName_DataBound">
                            </infs:WclDropDownList>--%>


                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <label for="<%= txtFirstName.ClientID %>" class="cptn">Applicant First Name</label>
                        <infs:WclTextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <label for="<%= txtLastName.ClientID %>" class="cptn">Applicant Last Name</label>
                        <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div class="row">
                    <div class='col-md-12' title="Click the link and select a node to restrict search results to the selected node">
                        <label class="cptn">Institution Hierarchy</label>
                        <a href="#" id="instituteHierarchy" onclick="openPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                        <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-12">&nbsp;</div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered Order ID">
                        <label for="<%= txtOrderNumber.ClientID %>" class="cptn">Order ID</label>
                        <infs:WclTextBox ID="txtOrderNumber" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Select one or more Payment Statuses to restrict search results to those statuses">
                        <label for="<%= chkOrderStatus.ClientID %>_Input" class="cptn">Payment Status(s)</label>
                        <infs:WclComboBox ID="chkOrderStatus" runat="server" CheckBoxes="true" DataTextField="Name"
                            DataValueField="Code" EmptyMessage="--Select--" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" EnableAriaSupport="true" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select one or more Payment Types to restrict search results to those types">
                        <label for="<%= chkPaymentType.ClientID %>_Input" class="cptn">Payment Type(s)</label>
                        <infs:WclComboBox ID="chkPaymentType" runat="server" CheckBoxes="true" DataTextField="Name"
                            DataValueField="Code" EmptyMessage="--Select--" Width="100%" CssClass="form-control" EnableAriaSupport="true"
                            Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Enter a range of dates to restrict search results to orders placed within that range">
                        <label for="<%= cmbOrderType.ClientID %>_Input" class="cptn">Order Type</label>
                        <infs:WclComboBox ID="cmbOrderType" runat="server" CheckBoxes="true" EmptyMessage="--Select--" EnableAriaSupport="true"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class='form-group col-md-3' title="Enter a range of dates to restrict search results to orders placed within that range">
                        <span class="cptn">Order Created Date Range</span>
                        <label for="<%= dpOdrCrtFrm.ClientID %>_dateInput" class="sr-only">Order Created Date Range From</label>
                        <infs:WclDatePicker ID="dpOdrCrtFrm" runat="server" DateInput-EmptyMessage="Select a date (From)" DateInput-EnableAriaSupport="true"
                            Calendar-EnableAriaSupport="true" DateInput-IncrementSettings-InterceptArrowKeys="true" ClientEvents-OnPopupClosing="OnCalenderClosing" DateInput-SelectionOnFocus="CaretToBeginning"
                            ClientEvents-OnDateSelected="CorrectFrmToCrtdDate" EnableAriaSupport="true" Width="100%" CssClass="form-control">
                        </infs:WclDatePicker>
                        <div class="gclrPad"></div>
                        <label for="<%= dpOdrCrtTo.ClientID %>_dateInput" class="sr-only">Order Created Date Range To</label>
                        <infs:WclDatePicker ID="dpOdrCrtTo" runat="server" DateInput-EmptyMessage="Select a date (To)" Calendar-EnableAriaSupport="true" EnableAriaSupport="true"
                            DateInput-EnableAriaSupport="true" ClientEvents-OnPopupOpening="SetMinDateCrtd" Width="100%" CssClass="form-control" ClientEvents-OnPopupClosing="OnCalenderClosing" DateInput-SelectionOnFocus="CaretToBeginning">
                        </infs:WclDatePicker>
                    </div>
                    <div class='form-group col-md-3' title="Enter a range of dates to restrict search results to orders where the payment falls within that range">
                        <span class="cptn">Order Paid Date Range</span>
                        <label for="<%= dpOdrPaidFrm.ClientID %>_dateInput" class="sr-only">Order Paid Date Range From</label>
                        <infs:WclDatePicker ID="dpOdrPaidFrm" runat="server" DateInput-EmptyMessage="Select a date (From)" EnableAriaSupport="true" DateInput-EnableAriaSupport="true"
                            ClientEvents-OnDateSelected="CorrectFrmToPaidDate" Width="100%" CssClass="form-control" Calendar-EnableAriaSupport="true" ClientEvents-OnPopupClosing="OnCalenderClosing" DateInput-SelectionOnFocus="CaretToBeginning">
                        </infs:WclDatePicker>
                        <div class="gclrPad"></div>
                        <label for="<%= dpOdrPaidTo.ClientID %>_dateInput" class="sr-only">Order Paid Date Range To</label>
                        <infs:WclDatePicker ID="dpOdrPaidTo" runat="server" DateInput-EmptyMessage="Select a date (To)" EnableAriaSupport="true" DateInput-EnableAriaSupport="true"
                            ClientEvents-OnPopupOpening="SetMinDatePaid" Width="100%" CssClass="form-control" Calendar-EnableAriaSupport="true" ClientEvents-OnPopupClosing="OnCalenderClosing" DateInput-SelectionOnFocus="CaretToBeginning">
                        </infs:WclDatePicker>
                    </div>
                    <!--UAT-2193: Remove Rush Orders check box-->
                    <%-- <div class='form-group col-md-3' title="Select the checkbox to restrict search results to only rush orders">
                        <span class='cptn'>Show only Rush Orders</span>
                        <asp:CheckBox ID="chkShowRushOrders" runat="server" Checked="false" Width="100%"
                            CssClass="form-control" />
                    </div>--%>
                    <div id="divSSN" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered SSN or ID Number">
                            <label for="<%= txtSSN.ClientID %>" class="cptn">SSN/ID Number</label>
                            <infs:WclMaskedTextBox ID="txtSSN" runat="server" MaxLength="10" Mask="aaa-aa-aaaa" EnableAriaSupport="true"
                                Width="100%" CssClass="form-control">
                            </infs:WclMaskedTextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div id="dvRecords" runat="server" visible="false">
                <div class="col-md-12">
                    <div class="row">
                        <div class='form-group col-md-3' title="Select a radio button to restrict the search results to either active or archived orders">
                            <label for="<%= rblOrderState.ClientID %>" class="cptn">Records</label>
                            <asp:RadioButtonList ID="rblOrderState" runat="server" RepeatDirection="Horizontal"
                                DataTextField="As_Name" DataValueField="AS_Code" CssClass="radio_list form-control"
                                Width="100%">
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-2">
                <div class="row">
                    <asp:CheckBox ID="chkSelectAllResults" Text="Select All Results" runat="server"
                        OnCheckedChanged="chkSelectAllResults_CheckedChanged" AutoPostBack="true" Width="100%" CssClass="form-control" />
                </div>
            </div>
            <div class="form-group col-md-9">
                <div class="row text-center">
                    <infsu:CommandBar ID="fsucOrderCmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                        AutoPostbackButtons="Submit,Save,Cancel" SaveButtonIconClass="rbSearch" SaveButtonText="Search"
                        CancelButtonText="Cancel" SubmitButtonText="Reset" SubmitButtonIconClass="rbUndo"
                        OnSubmitClick="CmdBarReset_Click" ValidationGroup="grpFormSubmit" OnSaveClick="CmdBarSearch_Click"
                        OnCancelClick="CmdBarCancel_Click" UseAutoSkinMode="false" ButtonSkin="Silk">
                        <%--DefaultPanel="pnlShowFilters" DefaultPanelButton="Save"--%>
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdOrderDetails" AutoGenerateColumns="False" AllowSorting="True"
            AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" NonExportingColumns="ViewDetail,AssignItems,ApplicantSSN"
            ShowClearFiltersButton="false" GridLines="Both" OnNeedDataSource="grdOrderDetails_NeedDataSource"
            ShowAllExportButtons="False" OnItemDataBound="grdOrderDetails_ItemDataBound" EnableAriaSupport="true"
            OnItemCommand="grdOrderDetails_ItemCommand" AllowCustomPaging="true" OnSortCommand="grdOrderDetails_SortCommand"
            OnInit="grdOrderDetails_Init">
            <ClientSettings EnableRowHoverStyle="true" ClientEvents-OnGridCreated="onGridCreated">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrderId,OrderNumber" AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                    ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="AssignItems" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAllOrderQueue(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelectItem" runat="server" Visible='<%#Convert.ToBoolean(Eval("IsInvoiceApproval"))?true:Convert.ToBoolean(Eval("IsCardWithApproval")) %>'
                                onclick="UnCheckOrderQueueHeader(this)" OnCheckedChanged="chkSelectItem_CheckedChanged"
                                Enabled='<%#(!Convert.ToBoolean(Eval("IsInvoiceApprovalInitiated"))) %>' />
                            <asp:Label ID="lblIsInvoiceApproval" runat="server" Text='<%#Eval("IsInvoiceApproval") %>'
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblIsCardWithApproval" runat="server" Text='<%#Eval("IsCardWithApproval") %>'
                                Visible="false"></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridNumericColumn DataField="OrderNumber" FilterControlAltText="Filter OrderId column"
                        HeaderText="Order ID" SortExpression="OrderNumber" DataType="System.Int32" UniqueName="OrderId"
                        DecimalDigits="0" HeaderStyle-Width="8%" HeaderTooltip="This column displays the order number for each record in the grid">
                    </telerik:GridNumericColumn>
                    <telerik:GridBoundColumn DataField="LastName" FilterControlAltText="Filter LastName column"
                        HeaderText="Applicant Last Name" SortExpression="LastName" UniqueName="LastName"
                        HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FirstName" FilterControlAltText="Filter FirstName column"
                        HeaderText="Applicant First Name" SortExpression="FirstName" UniqueName="FirstName"
                        HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid"
                        UniqueName="ApplicantSSN">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="SSN" HeaderText="SSN/ID Number" SortExpression="SSN"
                        Display="false"
                        HeaderTooltip="This column displays the applicant's SSN or ID Number for each record in the grid"
                        UniqueName="_ApplicantSSN">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstituteHierarchy" FilterControlAltText="Filter InstituteHierarchy column"
                        AllowFiltering="false" HeaderText="Institution Hierarchy" SortExpression="InstituteHierarchy"
                        UniqueName="InstituteHierarchy" HeaderStyle-Width="20%" HeaderTooltip="This column displays the institution hierarchy for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="DateOfBirth" HeaderText="Date of Birth" SortExpression="DateOfBirth"
                        UniqueName="DateOfBirth" DataFormatString="{0:MM/dd/yyyy}" HeaderTooltip="This column displays the applicant's date of birth for each record in the grid">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="PaymentType" FilterControlAltText="Filter PaymentType column"
                        AllowSorting="false"
                        HeaderText="Payment Type" UniqueName="PaymentType"
                        HeaderTooltip="Payment Type This column displays the Payment Type for each order">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrderStatusName" FilterControlAltText="Filter OrderStatusName column"
                        AllowSorting="false"
                        HeaderText="Payment Status" UniqueName="OrderStatusName"
                        HeaderStyle-Width="15%" HeaderTooltip="Payment Status This column displays the current Payment Status for each order">
                    </telerik:GridBoundColumn>
                    <%--<telerik:GridBoundColumn DataField="RushOrderStatus" FilterControlAltText="Filter RushOrderStatus column"
                        HeaderText="Rush Order" SortExpression="RushOrderStatus" UniqueName="RushOrderStatus"
                        HeaderStyle-Width="15%" HeaderTooltip="This column displays the Rush Order, if any, for each record in the grid">
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridDateTimeColumn DataField="OrderDate" HeaderText="Order Date" SortExpression="OrderDate"
                        UniqueName="OrderDate" DataFormatString="{0:MM/dd/yyyy}" HeaderTooltip="This column displays the Order Date for each order">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridDateTimeColumn DataField="Amount" HeaderText="Fee" SortExpression="Amount"
                        UniqueName="Amount" DataFormatString="{0:c}" HeaderTooltip="This column displays the Fee for each order">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="CancelledBy" FilterControlAltText="Filter CancelledBy column"
                        HeaderText="Cancelled By" SortExpression="CancelledBy" UniqueName="CancelledBy"
                        HeaderTooltip="This column displays the name of user who fully cancelled the order">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CancelledOn" FilterControlAltText="Filter CancelledOn column"
                        HeaderText="Cancelled On" SortExpression="CancelledOn" UniqueName="CancelledOn" DataFormatString="{0:MM/dd/yyyy}"
                        HeaderTooltip="This column displays the date and time when the order was fully cancelled">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="ViewDetail" OrderNumber='<%#Eval("OrderNumber") %>'
                                ToolTip="Click here to view details or to review/approve payments or cancellation requests"
                                runat="server" Text="Detail">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Auto Renewal" UniqueName="AutoRenewal" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnAutoRenewal" runat="server" Visible="false"
                                OnClientClick="return ResetAutoRenewalStatus(this);">
                            </asp:LinkButton>
                            <asp:HiddenField ID="hfOrderID" runat="server" Value='<%#Eval("OrderId") %>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="tplcohdr" />
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>


    <div id="divInvoiceApproval" runat="server">
        <asp:Panel ID="pnlApprove" runat="server" DefaultButton="btnInvoiceApproval">
            <div class="row text-center" id="trailingText">

                <infs:WclButton ID="btnInvoiceApproval" ToolTip="Click to Approve Payment(s)" runat="server"
                    Icon-PrimaryIconCssClass="rbOk" Text="Approve Payment" ButtonPosition="Center"
                    OnClick="btnInvoiceApproval_Click"
                    Skin="Silk" AutoSkinMode="false">
                </infs:WclButton>
                &nbsp;&nbsp;
                    <infs:WclTextBox ID="txtReferenceText" runat="server" EmptyMessage="Enter Reference Number" EnableAriaSupport="true"
                        ToolTip="Enter Reference Number" Skin="Silk" AutoSkinMode="false">
                    </infs:WclTextBox>
            </div>
        </asp:Panel>
    </div>
</div>

<div id="confirmSave" class="confirmProfileSave" runat="server" style="display: none">
    <p style="text-align: left"><asp:literal runat="server" ID="ltrlConfrmProfileSave">Do you want to continue?</asp:literal></p>

</div>

<div id="ConfirmationMsgPopup" title="Complio" runat="server" style="display: none">
    <p style="text-align: left;">Some or all of the selected orders are set up as School Approval Only and they need approval from applicant's school. Do you still want to continue?</p>
</div>


<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
<%--<asp:HiddenField ID="hfTenantId" runat="server" />--%>
<asp:HiddenField ID="hfCurrentUserID" runat="server" />
<asp:HiddenField ID="hdnIsPagePostBack" runat="server" Value="" />
<asp:HiddenField ID="hdnConfirmSave" runat="server" Value="0" />
<label ID="lblPopUpText" runat="server" style="display:none"></label>
<%--<asp:Label ID="lblPopUpText" runat="server" Value="" Visible="false" />--%>
<asp:HiddenField ID="hdnPopUpText" runat="server" Value="" />
<asp:HiddenField ID="hdnConfirmApproveOrderPayment" runat="server" Value="0" />
<asp:HiddenField ID="hdnConfirmApproveOrderPaymentPopUpText" runat="server" Value="" />
<asp:HiddenField ID="hdnScreenName" runat="server" />
<script type="text/javascript">
    var winopen = false;
    var minDate = new Date("01/01/1980");

    function ShowCCOrdersRequirApprovalConfirmationPopup() {
        //debugger;
        var dialog = $window.showDialog($jQuery("[id$=ConfirmationMsgPopup]").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Proceed and Approve Payment", click: function () {

                    $jQuery("[id$=hdnConfirmApproveOrderPayment]").val(1);
                    $jQuery("#<%=btnInvoiceApproval.ClientID %>").trigger('click');
                }
            }, closeBtn: {
                autoclose: true, text: "Cancel", click: function () {
                    $jQuery("[id$=hdnConfirmApproveOrderPayment]").val(0);
                    return false;
                }
            }
        }, 550, 'Alert');
    }


    function confirmClick() {
    
        var popUpText = $jQuery("[id$=hdnPopUpText]").val();
        $jQuery(".confirmProfileSave p")[0].innerHTML= popUpText;
        var dialog = $window.showDialog($jQuery(".confirmProfileSave").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Proceed and Approve Payment", click: function () {

                    $jQuery("[id$=hdnConfirmSave]").val(1);
                    $jQuery("#<%=btnInvoiceApproval.ClientID %>").trigger('click');
                }
            }, closeBtn: {
                autoclose: true, text: "Cancel", click: function () {
                    $jQuery("[id$=hdnConfirmSave]").val(0);
                    return false;
                }
            }
        }, 475, 'Alert');
    }


    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = $jQuery("[id$=hdnScreenName]").val();
        //var tenantId = 2;
        var tenantId = $jQuery("[id$=hdnTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            //UAT-1923
            $jQuery("[id$=hdnIsPagePostBack]").val("Focus Set");
            $jQuery("[id$=instituteHierarchy]").focusout();
            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
                $jQuery("[id$=hdnDepartmntPrgrmMppng]").val(arg.DepPrgMappingId);
                $jQuery("[id$=hdnHierarchyLabel]").val(arg.HierarchyLabel);
                $jQuery("[id$=hdnInstitutionNodeId]").val(arg.InstitutionNodeId);
                __doPostBack("<%= btnDoPostBack.ClientID %>", "");
              }
              winopen = false;
              //UAT-1923
              setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
          }
      }

      function CorrectFrmToCrtdDate(picker) {
          var date1 = $jQuery("[id$=dpOdrCrtFrm]")[0].control.get_selectedDate();
          var date2 = $jQuery("[id$=dpOdrCrtTo]")[0].control.get_selectedDate();
          if (date1 != null && date2 != null) {
              if (date1 > date2)
                  $jQuery("[id$=dpOdrCrtTo]")[0].control.set_selectedDate(null);
          }
      }

      function SetMinDateCrtd(picker) {
          var date = $jQuery("[id$=dpOdrCrtFrm]")[0].control.get_selectedDate();
          if (date != null) {
              picker.set_minDate(date);
          }
          else {
              picker.set_minDate(minDate);
          }
      }

      function CorrectFrmToPaidDate(picker) {
          var date1 = $jQuery("[id$=dpOdrPaidFrm]")[0].control.get_selectedDate();
          var date2 = $jQuery("[id$=dpOdrPaidTo]")[0].control.get_selectedDate();
          if (date1 != null && date2 != null) {
              if (date1 > date2)
                  $jQuery("[id$=dpOdrPaidTo]")[0].control.set_selectedDate(null);
          }
      }

      function SetMinDatePaid(picker) {
          var date = $jQuery("[id$=dpOdrPaidFrm]")[0].control.get_selectedDate();
          if (date != null) {
              picker.set_minDate(date);
          }
          else {
              picker.set_minDate(minDate);
          }
      }

      function grd_rwDbClick(s, e) {
          var _id = "btnEdit";
          var b = e.get_gridDataItem().findControl(_id);
          if (b && typeof (b.click) != "undefined") { b.click(); }
      }


      function CheckAllOrderQueue(id) {
          var masterTable = $find("<%= grdOrderDetails.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (masterTable.get_dataItems()[i].findElement("chkSelectItem") != null && !(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectItem").checked = isChecked; // for checking the checkboxes
            }
        }
    }

    function UnCheckOrderQueueHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdOrderDetails.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (masterTable.get_dataItems()[i].findElement("chkSelectItem") != null && !(masterTable.get_dataItems()[i].findElement("chkSelectItem").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectItem").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    //UAT-796 -Function to turn on/off Automatic renewal of an applicant.
    function ResetAutoRenewalStatus(sender, eventArgs) {
        var btnID = sender.id;
        if (sender.attributes.Enabled != undefined && sender.attributes.Enabled.value == "false") {
            return false;
        }
        var containerID = btnID.substr(0, btnID.indexOf("btnAutoRenewal"));
        var tenantId = $jQuery("#<%= hdnTenantId.ClientID %>").val();
        var orderID = $jQuery("[id$=" + containerID + "hfOrderID]").val();
        var currentUserID = $jQuery("#<%= hfCurrentUserID.ClientID %>").val();
        var urltoPost = "/ComplianceOperations/Default.aspx/ResetAutoRenewalStatus";

        var dataString = "tenantID : '" + tenantId + "', orderID : '" + orderID + "', currentUserID : '" + currentUserID + "', buttonid : '" + btnID + "'";//, autoRenewalCurrentValue : '" + autoRenewalCurrentValue +"'";
        $jQuery.ajax
            (
            {
                type: "POST",
                url: urltoPost,
                data: "{ " + dataString + " }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var data = JSON.parse(result.d);
                    if (data.orderRenwalStatus == "True") {
                        $jQuery('#' + data.buttonid).html("ON");
                        $jQuery('#' + data.buttonid)[0].title = "Click to Turn Off Auto Renewal"
                    }
                    else if (data.orderRenwalStatus == "False") {
                        $jQuery('#' + data.buttonid).html("OFF");
                        $jQuery('#' + data.buttonid)[0].title = "Click to Turn On Auto Renewal"
                    }
                }
            });
        return false;
    }

    function pageLoad() {



        SetDefaultButtonForSection("divSearchPanel", "fsucOrderCmdBar_btnSave", true);

        //UAT-1955
        var combobox = $jQuery("[id$=chkOrderStatus]");
        combobox[0].setAttribute("role", "combobox");

        var combobox = $jQuery("[id$=chkPaymentType]");
        combobox[0].setAttribute("role", "combobox");

        var combobox = $jQuery("[id$=cmbOrderType]");
        combobox[0].setAttribute("role", "combobox");

        //UAT-1923
        var hdnHierarchyLabel = $jQuery("[id$=hdnHierarchyLabel]");
        var hdnIsPagePostBack = $jQuery("[id$=hdnIsPagePostBack]");
        if (hdnHierarchyLabel.val() != "" && hdnIsPagePostBack.val() == "Focus Set") {
            setTimeout(function () { $jQuery("[id$=instituteHierarchy]").focus(); }, 100);
            hdnIsPagePostBack.val("");
        }
        //UAT-1995
        $jQuery(".rgHeader").each(function () {
            if (this.innerText == 'Payment Type' || this.innerText == 'Payment Status' || this.innerText == 'Auto Renewal') {
                //debugger;
                this.tabIndex = 0;
            }
        });
    }

    $jQuery(document).ready(function () {

        //Adding title for Search/Reset/Cancel buttton
        $jQuery('[id$=fsucOrderCmdBar_btnSave_input]').attr('title', '<%= searchbtnToolTipText%>');
        $jQuery('[id$=fsucOrderCmdBar_btnSubmit_input]').attr('title', '<%= resetbtnToolTipText%>');
        $jQuery('[id$=fsucOrderCmdBar_btnCancel_input]').attr('title', '<%= cancelbtnToolTipText%>');

        $jQuery("[id$=instituteHierarchy]").on('keypress', function (e) {
            if (e.which == 13) {
                openPopUp();
            }
        })

    });
</script>
