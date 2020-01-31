<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderSearchQueue.ascx.cs"
    Inherits="CoreWeb.BkgOperations.Views.BkgOrderSearchQueue" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="InstituteHierarchy" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/NewComplianceInstitutionHierarchy.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/CustomAttributeLoaderSearchMultipleNodes.ascx"
    TagName="CustomAttributeLoaderSearch" TagPrefix="uc" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxBkgOrderSearchQueue">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" /> 
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .RadMenu .rmGroup .rmText {
        padding: 0px;
        margin: 0px;
    }

    .rmSlide ul {
        border: none !important;
    }

    .archiveBtnStyle {
        float: right !important;
        top: -34px !important;
        right: 23.4% !important;
        /*float: left;*/
    }

    .btn {
        text-align: left;
    }

    .top3 {
        top: 3px !important;
    }
</style>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color"  tabindex="0">
                <asp:Label ID="lblOrderQueue" CssClass="page-heading" runat="server" Text=""></asp:Label>
            </h2>
        </div>
    </div>
    <div class="row bgLightGreen" id="divSearchPanel">
        <asp:Panel runat="server" ID="pnlShowFilters">
            <div class='col-md-12'>
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the institution whose data you want to view">
                            <%--<span class='cptn'>Institution</span><span class="reqd">*</span>--%>
                            <%--<infs:WclDropDownList ID="ddlTenantName" runat="server" AutoPostBack="true"
                                DataTextField="TenantName" DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_ItemSelected"
                                CausesValidation="false" OnDataBound="ddlTenantName_DataBound">
                            </infs:WclDropDownList>   --%>
                            <label for="<%= ddlTenantName.ClientID %>_Input" class="cptn">Institution</label><span class="reqd">*</span>
                            <infs:WclComboBox ID="ddlTenantName" runat="server" AutoPostBack="true" DataTextField="TenantName" EnableAriaSupport="true"
                                ChangeTextOnKeyBoardNavigation="true"
                                DataValueField="TenantID" OnSelectedIndexChanged="ddlTenantName_ItemSelected" ValidationGroup="grpOrderSearch"
                                CausesValidation="true" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" TabIndex="0"
                                OnDataBound="ddlTenantName_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenantName" SetFocusOnError="true"
                                    InitialValue="--Select--" Display="Dynamic" CssClass="errmsg" ValidationGroup="grpOrderSearch"
                                    Text="Institution is required." />
                            </div>
                        </div>
                    </div>

                    <div id="divStatusColor" runat="server" class='form-group col-md-3' title="Select the status color whose data you want to view">
                        <%--<span class='cptn'>Status Color</span>--%>
                        <label for="<%= ddlIcon.ClientID %>_Input" class="cptn">Status Color</label>
                        <asp:Label ID="lblStatusColor" runat="server" Visible="false" Width="100%" CssClass="form-control" />
                        <telerik:RadComboBox ID="ddlIcon" runat="server" Width="100%" Skin="Silk" CssClass="form-control" EnableAriaSupport="true" 
                            AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlIcon_SelectedIndexChanged" />
                    </div>
                    <div id="divServiceGroup" runat="server" class='form-group col-md-3' title="Select the service group whose data you want to view"
                        visible="false">
                        <%--<span class='cptn'>Service Groups</span>--%>
                        <label for="<%= rcServiceGroup.ClientID %>_Input" class="cptn">Service Groups</label>
                        <telerik:RadComboBox ID="rcServiceGroup" runat="server" Width="100%" Skin="Silk" EnableAriaSupport="true"
                            DataTextField="BSG_Name" DataValueField="BSG_ID" OnSelectedIndexChanged="rcServiceGroup_SelectedIndexChanged"
                            AutoPostBack="true" Visible="false">
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div id="divInstitutionHierarchy" runat="server">
                        <%--<div class='form-group col-md-12' title="Click the link and select a node to restrict search results to the selected node">
                            <span class='cptn'>Institution Hierarchy</span>
                            <a href="#" id="A1" onclick="openHierarchyPopUp();">Select Institution Hierarchy</a>&nbsp;&nbsp
                            <asp:Label ID="lblinstituteHierarchy" runat="server"></asp:Label>
                        </div>--%>
                        <uc:CustomAttributeLoaderSearch ID="ucCustomAttributeLoaderSearch" runat="server" />
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Restrict search results to the entered first name">
                        <%--<span class='cptn'>First Name</span>--%>
                        <label for="<%= txtFirstName.ClientID %>" class="cptn">First Name</label>
                        <infs:WclTextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered last name">
                        <%--<span class='cptn'>Last Name</span>--%>
                        <label for="<%= txtLastName.ClientID %>" class="cptn">Last Name</label>
                        <infs:WclTextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true">
                        </infs:WclTextBox>
                    </div>
                    <div id="divDOB" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered dob">
                            <%--<span class='cptn'>DOB</span>--%>
                            <label for="<%= dpDob.ClientID %>_dateInput" class="cptn">DOB</label>
                            <infs:WclDatePicker ID="dpDob" runat="server" DateInput-EmptyMessage="Select a dob" EnableAriaSupport="true"
                                ClientEvents-OnPopupClosing="OnCalenderClosing" DateInput-SelectionOnFocus="CaretToBeginning"
                                 Calendar-EnableKeyboardNavigation="true" Calendar-EnableAriaSupport="true" DateInput-EnableAriaSupport="true"
                                Width="100%" CssClass="form-control" ClientEvents-OnDateSelected="CorrectFrmToCrtdDate">
                            </infs:WclDatePicker>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Restrict search results to the entered OrderId">
                        <%--<span class='cptn'>Order ID</span>--%>
                        <label for="<%= txtOrderNumber.ClientID %>" class="cptn">Order ID</label>
                        <infs:WclTextBox ID="txtOrderNumber" runat="server" Width="100%" CssClass="form-control" EnableAriaSupport="true" EmptyMessage="Enter an order number">
                        </infs:WclTextBox>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div id="divSSN" runat="server">
                        <div class='form-group col-md-3' title="Restrict search results to the entered ssn">
                            <%--<span class='cptn'>SSN</span>--%>
                            <%--<infs:WclMaskedTextBox ID="txtSSN" runat="server"  Mask="\#\#\#-\#\#-####"  >
                        </infs:WclMaskedTextBox>--%>
                            <label for="<%= txtSSN.ClientID %>" class="cptn">SSN</label>
                            <infs:WclMaskedTextBox ID="txtSSN" runat="server" Mask="aaa-aa-aaaa" Width="100%"
                                CssClass="form-control">
                            </infs:WclMaskedTextBox>
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Select payment Status to restrict search results to those statuses">
                        <%--<span class='cptn'>Payment Status(s)</span>--%>
                        <label for="<%= chkPaymentStatusType.ClientID %>_Input" class="cptn">Payment Status(s)</label>
                        <label style="display: none" for="<%= chkPaymentStatusToggle.ClientID %>">Payment Status(s)</label>
                        <infs:WclButton runat="server" ID="chkPaymentStatusToggle" ToggleType="CheckBox" TabIndex="0"
                            ButtonType="ToggleButton" AutoPostBack="false" Visible="false" CssClass="noUnderline">
                            <ToggleStates>
                                <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                <telerik:RadButtonToggleState Text="No" Value="False" />
                            </ToggleStates>
                        </infs:WclButton>
                        <infs:WclComboBox ID="chkPaymentStatusType" runat="server" DataTextField="Name" Width="100%" EnableAriaSupport="true"
                            CssClass="form-control" Skin="Silk" AutoSkinMode="false" DataValueField="OrderStatusID"
                            EmptyMessage="--Select--" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3'>
                        <%--<span class='cptn'>Order Date Type</span>--%>
                        <label id="lblOrderDateType" class="cptn">Order Date Type</label>
                        <asp:RadioButtonList ID="orderDateType" runat="server" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="rbl_OnSelectedIndexChanged" AutoPostBack="true" CssClass="form-control">
                            <asp:ListItem Text="Created" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Paid" Value="1"> </asp:ListItem>
                            <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvOrderDateType" ControlToValidate="orderDateType"
                                Enabled="false" SetFocusOnError="true"
                                class="errmsg" Display="Dynamic" ErrorMessage="Order date type is required."
                                ValidationGroup='grpOrderSearch' />
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Enter a range of dates to restrict search results to orders placed within that range">
                        <%--<span class='cptn'>Order From Date</span>--%>
                        <label for="<%= dpOdrCrtFrm.ClientID %>_dateInput" class="cptn">Order From Date</label>
                        <infs:WclDatePicker ID="dpOdrCrtFrm" runat="server" DateInput-EmptyMessage="Select a date (From)" DateInput-EnableAriaSupport="true" DateInput-SelectionOnFocus="CaretToBeginning"
                            Width="100%" CssClass="form-control" ClientEvents-OnDateSelected="SelectedFromDate" ClientEvents-OnPopupClosing="OnCalenderClosing">
                        </infs:WclDatePicker>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvOdrCrtFrm" ControlToValidate="dpOdrCrtFrm" SetFocusOnError="true"
                                class="errmsg" Display="Dynamic" ErrorMessage="Order from date is required."
                                Enabled="false" ValidationGroup='grpOrderSearch' />
                        </div>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Enter a range of dates to restrict search results to orders placed within that range">
                        <%--<span class='cptn'>Order To Date</span>--%>
                        <label for="<%= dpOdrCrtTo.ClientID %>_dateInput" class="cptn">Order To Date</label>
                        <infs:WclDatePicker ID="dpOdrCrtTo" runat="server" DateInput-EmptyMessage="Select a date (To)" DateInput-SelectionOnFocus="CaretToBeginning"
                            EnableAriaSupport="true" ClientEvents-OnPopupClosing="OnCalenderClosing"
                            Width="100%" CssClass="form-control" ClientEvents-OnPopupOpening="SetMinDateCrtd">
                            <Calendar EnableKeyboardNavigation="true" EnableAriaSupport="true"></Calendar>
                        </infs:WclDatePicker>
                        <div class='vldx'>
                            <asp:RequiredFieldValidator runat="server" ID="rfvOdrCrtTo" ControlToValidate="dpOdrCrtTo"
                                Enabled="false" SetFocusOnError="true"
                                class="errmsg" Display="Dynamic" ErrorMessage="Order to date is required."
                                ValidationGroup='grpOrderSearch' />
                        </div>
                    </div>
                    <div class='form-group col-md-3' title="Select order status types to restrict search results to those order status types">
                        <%--<span class='cptn'>Order Status(s)</span>--%>
                        <label for="<%= chkOrderStatusTypes.ClientID %>_Input" class="cptn">Order Status(s)</label>
                        <infs:WclComboBox ID="chkOrderStatusTypes" runat="server" DataTextField="StatusType" EnableAriaSupport="true"
                            DataValueField="OrderStatusTypeID" EmptyMessage="--Select--" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' id="div1" title="Select form status to restrict search results to those form status"
                        runat="server">
                        <%--<span class='cptn'>Form Status</span>--%>
                        <label for="<%= cmbFormStatus.ClientID %>_Input" class="cptn">Form Status</label>
                        <infs:WclComboBox ID="cmbFormStatus" runat="server" DataTextField="SFS_Name" EmptyMessage="--Select--" EnableAriaSupport="true"
                            DataValueField="SFS_ID" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <%--<div id="divAdminServices" runat="server" visible="true">--%>
                    <div class='form-group col-md-3' id="divServices" title="Select one or more Services to restrict search results to those services"
                        runat="server">
                        <%--<span class='cptn'>Services</span>--%>
                        <label for="<%= chkServices.ClientID %>_Input" class="cptn">Services</label>
                        <infs:WclComboBox ID="chkServices" runat="server" DataTextField="BSE_Name" EnableAriaSupport="true"
                            DataValueField="BSE_ID" EmptyMessage="--Select--" Width="100%" CssClass="form-control"
                            Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <%--</div>--%>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div id="divClientAdmin" runat="server" visible="false">
                        <div runat="server" id="divFlaged" visible="false">
                            <div class='form-group col-md-3'>
                                <%--<span class='cptn'>Is Flagged</span>--%>
                                <label id="lblIsFlagged" class="cptn">Is Flagged</label>
                                <%-- <infs:WclButton runat="server" ID="chkFlaggedToggle" ToggleType="CheckBox" CssClass="noUnderline"
                                    ButtonType="ToggleButton" AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>--%>
                                <asp:RadioButtonList ID="rblFlagged" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="false" CssClass="form-control" OnSelectedIndexChanged="rblFlagged_SelectedIndexChanged">
                                    <asp:ListItem Text="Flagged" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Not Flagged" Value="0"> </asp:ListItem>
                                    <asp:ListItem Text="All" Value="2" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class='form-group col-md-3' id="divCategory" title="Select one or more category to restrict search results to those category"
                            runat="server" visible="false">
                            <%--<span class='cptn'>Category</span>--%>
                            <label for="<%= rcbClientStatus.ClientID %>_Input" class="cptn">Category</label>
                            <infs:WclButton runat="server" ID="chkCategoryToggle" ToggleType="CheckBox" CssClass="noUnderline"
                                ButtonType="ToggleButton" AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>

                            <infs:WclComboBox ID="rcbClientStatus" runat="server" DataTextField="BOCS_OrderClientStatusTypeName"
                                Visible="false" Skin="Silk" Width="100%" CssClass="form-control" AutoSkinMode="false"
                                DataValueField="BOCS_ID" EmptyMessage="--Select--">
                            </infs:WclComboBox>
                        </div>
                        <div class='form-group col-md-3' id="divArchiveStatus" runat="server" visible="false">
                            <%--<span class='cptn'>Archive Status</span>--%>
                            <label for="<%= rcbArchiveStatus.ClientID %>_Input" class="cptn">Archive Status</label>
                            <infs:WclButton runat="server" ID="chkArchiveToggle" ToggleType="CheckBox" CssClass="noUnderline"
                                ButtonType="ToggleButton" AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>
                            <infs:WclComboBox ID="rcbArchiveStatus" runat="server" Visible="false" Width="100%"
                                CssClass="form-control" AutoSkinMode="false" Skin="Silk">
                                <Items>
                                    <telerik:RadComboBoxItem Value="0" Text="--Select--" />
                                    <telerik:RadComboBoxItem Value="true" Text="Archived Only" />
                                    <telerik:RadComboBoxItem Value="false" Text="Not Archived" />
                                </Items>
                            </infs:WclComboBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' title="Select a User Group">
                        <%--<span class="cptn">User Group</span>--%>
                        <label for="<%= ddlUserGroup.ClientID %>_Input" class="cptn">User Group</label>
                        <infs:WclComboBox ID="ddlUserGroup" runat="server" DataTextField="UG_Name" DataValueField="UG_ID" EnableAriaSupport="true"
                            AutoPostBack="false" OnDataBound="ddlUserGroup_DataBound" EmptyMessage="--Select--"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                    </div>
                    <div class='form-group col-md-3' title="Select &#34All&#34 to view all subscriptions per the other parameters or &#34Archived&#34 to view only archived subscriptions or &#34Active&#34 to view only non archived subscriptions">
                        <%--<span class="cptn">Subscription Archive State</span>--%>
                        <label id="lblSubscriptionArchiveState" class="cptn">Subscription Archive State</label>
                        <asp:RadioButtonList ID="rbSubscriptionState" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbSubscriptionState_SelectedIndexChanged"
                            DataTextField="As_Name" DataValueField="AS_Code" CssClass="w_cptn" AutoPostBack="true">
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group col-md-2">
                <div class="row">
                    <asp:CheckBox ID="chkSelectAllResults" Text="Select All Results" runat="server" OnCheckedChanged="chkSelectAllResults_CheckedChanged"
                        Width="100%" AutoPostBack="true" CssClass="form-control" />
                </div>
            </div>
        </div>
    </div>

    <div class="col-md-12">&nbsp;</div>
    <div class="col-md-12">
        <div class="row" id="trailingText">
            <div style="float: left; width: 67%">
                <infsu:CommandBar ID="fsucOrderCmdBar" runat="server" ButtonPosition="Right" DisplayButtons="Submit,Save,Cancel,Clear"
                    AutoPostbackButtons="Submit,Save,Cancel,Clear,Extra" SaveButtonIconClass="rbSearch"
                    SaveButtonText="Search" CancelButtonText="Cancel" SubmitButtonText="Reset" SubmitButtonIconClass="rbUndo"
                    OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click"
                    ValidationGroup="grpOrderSearch"
                    OnCancelClick="CmdBarCancel_Click" ClearButtonText="Send Message" ClearButtonIconClass="rbEnvelope"
                    OnClearClick="btnSendMessage_Click" UseAutoSkinMode="false" ButtonSkin="Silk">
                    <ExtraCommandButtons>
                        <infs:WclButton ID="btnExport" AutoPostBack="true" Text="Export D&A Document(s)" OnClick="btnExport_Click"
                            runat="server" AutoSkinMode="false" Skin="Silk">
                        </infs:WclButton>
                    </ExtraCommandButtons>
                </infsu:CommandBar>
            </div>
            <infs:WclMenu ID="cmd" runat="server" Skin="Default" AutoSkinMode="false" CssClass="top3">
                <Items>
                    <telerik:RadMenuItem Text="Archivemun">
                        <ItemTemplate>
                            <infs:WclButton ID="btnArchiveOrders" Text="Archive" runat="server" AutoSkinMode="false" Visible="true" CssClass="btn"
                                Skin="Silk" OnClick="fsucOrderCmdBar_ExtraClick">
                                <Icon PrimaryIconCssClass="rbArchive" />
                            </infs:WclButton>
                        </ItemTemplate>
                        <Items>
                            <telerik:RadMenuItem>
                                <ItemTemplate>
                                    <infs:WclButton ID="btnUnArchiveOrders" Text="Unarchive" runat="server" AutoSkinMode="false" Visible="true" CssClass="btn"
                                        Skin="Silk" OnClick="fsucOrderCmdBar_UnArchiveClick">
                                        <Icon PrimaryIconCssClass="rbArchive" />
                                    </infs:WclButton>
                                </ItemTemplate>
                            </telerik:RadMenuItem>
                        </Items>
                    </telerik:RadMenuItem>
                </Items>
            </infs:WclMenu>
            <div class="col-md-12">&nbsp;</div>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdBkgOrderDetails" AutoGenerateColumns="False"
            AllowSorting="True" EnableAriaSupport="true" ClientSettings-AllowKeyboardNavigation="true"
            AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0" EnableCustomExporting="true"
            NonExportingColumns="CanViewServiceGroups,CanViewStatus,OrderId,CanViewClearStarStatus,CanViewOrderStatus,CanViewOrderArchiveStatus,CanViewOrderClientStatus,CustomAttributes,SelectUsers,SSN,OrderNote"
            ShowClearFiltersButton="false" GridLines="Both" OnNeedDataSource="grdBkgOrderDetails_NeedDataSource"
            ShowAllExportButtons="False" OnItemDataBound="grdBkgOrderDetails_ItemDataBound"
            OnItemCommand="grdBkgOrderDetails_ItemCommand" AllowCustomPaging="true" OnSortCommand="grdBkgOrderDetails_SortCommand">
            <ClientSettings EnableRowHoverStyle="true" ClientEvents-OnGridCreated="onGridCreated">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>

            <GroupingSettings CaseSensitive="false" />
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="false" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrderId,OrderNumber,HierarchyNodeID" ClientDataKeyNames="OrderId"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="true"
                    ShowExportToPdfButton="true" ShowExportToCsvButton="true" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="SelectUsers" HeaderTooltip="Click this box to select all users on the active page"
                        AllowFiltering="false" ShowFilterIcon="false">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span class="sr-only" id="spnTest"></span>
                            <asp:CheckBox ID="chkSelectUser" runat="server"
                                onclick='<% # "UnCheckHeader(this,\"" + Eval("OrderId") + "\" );"%>' />
                            <%-- Checked='<%#Convert.ToBoolean(Eval("IsUserGroupMatching")) %>'
                                   <asp:Label ID="lblIsUserGroup" runat="server" Text='<%#Eval("IsUserGroupMatching") %>'
                                    Visible="false"></asp:Label>--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status" ItemStyle-Wrap="false" UniqueName="CanViewStatus">
                        <ItemTemplate>
                            <div>
                                <asp:Image ID="imgOrderStatus" runat="server" Visible="false" />
                                <asp:Image ID="imgInstitutionStatus" runat="server" Visible="false" />
                                <asp:ImageButton ID="imgPDF" AlternateText='<%# String.Concat("Click here to view Order result PDF for Order number ",Convert.ToString((Eval("OrderNumber")))) %>' runat="server" Visible="false" OnClientClick="return openReportWithOrderID(this);" />
                                <asp:HiddenField ID="hdnfOrderID" runat="server" Value='<%#Eval("OrderId") %>' />
                                <asp:HiddenField ID="hdnDPM_IsEmploymentType" runat="server" Value='<%#Eval("DPM_IsEmploymentType") %>'/>
                                <%--Text='<%# Eval("OrderId") %>' CommandArgument='<%# Eval("OrderId")%>' CommandName="ViewDoc"  CssClass="hlink"--%>
                                <asp:HiddenField ID="hdnInstColorId" runat="server" Value='<%#Eval("InstitutionStatusColorID") %>' />
                            </div>
                            <div style="font-size: 10px !important; text-align: center;">
                                <asp:LinkButton ID="lnkBtnColorFlag" runat="server" Visible="false"
                                    OnClientClick="return openColorFlagOrderStatusPopup(this);"></asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="OrderNumber" FilterControlAltText="Filter OrderId column"
                        AllowFiltering="false" HeaderText="Order ID" UniqueName="OrderIdTemp" Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Order ID" ItemStyle-Width="35px" DataField="OrderNumber"
                        UniqueName="OrderId"
                        FilterControlAltText="Filter OrderId column" SortExpression="OrderNumber" DataType="System.Int32"
                        HeaderStyle-Width="5%" HeaderTooltip="This column displays the order number for each record in the grid">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbGridOrderId" runat="server" Text='<%# Eval("OrderNumber") %>' ToolTip="Click here to go to detail." CommandName="ViewOrderDetail" />
                            <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%# Eval("OrderId") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="OrderPrice" FilterControlAltText="Filter OrderPrice column"
                        ItemStyle-Width="100px" HeaderStyle-Width="125px"
                        HeaderText="Total Order Price" SortExpression="OrderPrice" UniqueName="CanViewOrderPrice"
                        DataFormatString="{0:C}"
                        HeaderTooltip="This column displays the total order price for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantFirstName" FilterControlAltText="Filter ApplicantFirstName column"
                        ItemStyle-Width="100px" HeaderStyle-Width="125px"
                        HeaderText="First Name" SortExpression="ApplicantFirstName" UniqueName="ApplicantFirstName"
                        HeaderTooltip="This column displays the applicant's first name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ApplicantLastName" FilterControlAltText="Filter ApplicantLastName column"
                        ItemStyle-Width="100px" HeaderStyle-Width="125px"
                        HeaderText="Last Name" SortExpression="ApplicantLastName" UniqueName="ApplicantLastName"
                        HeaderTooltip="This column displays the applicant's last name for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="SSN" FilterControlAltText="Filter SSN column"
                        DataFormatString="{0:000-00-0000}"
                        HeaderText="SSN" SortExpression="SSN" UniqueName="SSN" ItemStyle-Width="150px"
                        HeaderTooltip="This column displays the applicant ssn for each record in the grid">
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn DataField="SSN" FilterControlAltText="Filter SSN column"
                        DataFormatString="{0:000-00-0000}"
                        HeaderText="SSN" SortExpression="SSN" UniqueName="_SSN" ItemStyle-Width="150px"
                        Display="false"
                        HeaderTooltip="This column displays the applicant ssn for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="HierarchyLabel" FilterControlAltText="Filter HierarchyLabel column"
                        AllowFiltering="false" HeaderText="Institution Hierarchy" SortExpression="HierarchyLabel"
                        ItemStyle-Width="200px"
                        UniqueName="HierarchyLabel" HeaderTooltip="This column displays the institution hierarchy for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="BkgPackageNames" FilterControlAltText="Filter BkgPackageNames column" AllowSorting="false"
                        ItemStyle-Width="200px" HeaderStyle-Width="125px" HeaderText="Screening Packages" SortExpression="BkgPackageNames" UniqueName="BkgPackageNames"
                        HeaderTooltip="Screening Packages This column displays the applicant's screening packages for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UserGroupNames" FilterControlAltText="Filter User Group Names column"
                        ItemStyle-Width="200px" HeaderStyle-Width="125px"
                        HeaderText="User Groups" SortExpression="UserGroupNames" UniqueName="UserGroupNames"
                        HeaderTooltip="This column displays the applicant's user groups for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CustomAttributes" FilterControlAltText="Filter CustomAttributes column"
                        AllowFiltering="false" HeaderText="Custom Attributes" AllowSorting="false" ItemStyle-Width="200px"
                        UniqueName="CustomAttributes" HeaderTooltip="Custom Attributes This column displays the Custom Attributes for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="CustomAttributes" AllowFiltering="false" HeaderText="Custom Attributes"
                        AllowSorting="false" ItemStyle-Width="300px"
                        UniqueName="CustomAttributesTemp" Display="false">
                    </telerik:GridBoundColumn>
                    <%--<telerik:GridTemplateColumn HeaderText="Order Date" ItemStyle-Width="100px" SortExpression="OrderCreatedDate" Display="false" UniqueName="CanViewOrderCreatedDate">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderCreateDate" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridDateTimeColumn HeaderText="Order Date" ItemStyle-Width="100px" SortExpression="OrderCreatedDate"
                        Display="true" DataFormatString="{0:MM/dd/yyyy}"
                        UniqueName="CanViewOrderCreatedDate" DataField="OrderCreatedDate">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridTemplateColumn HeaderText="Completed" ItemStyle-Width="100px" SortExpression="OrderCompletedDate"
                        Display="false" UniqueName="CanViewOrderCompletedDate">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderCompletedDate" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="OrderStatus" ItemStyle-Width="150px" FilterControlAltText="Filter PaymentStatus column"
                        HeaderText="Payment Status" UniqueName="CanViewPaymentStatus" AllowSorting="false"
                        HeaderTooltip="This column displays the Payment status for each order">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="BkgOrderStatus" Display="false"
                        HeaderText="Order Status" UniqueName="BkgOrderStatusTemp">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="BkgOrderStatus" FilterControlAltText="Filter OrderStatus column"
                        HeaderText="Order Status" SortExpression="BkgOrderStatus" UniqueName="CanViewOrderStatus"
                        ItemStyle-Width="150px" HeaderTooltip="This column displays the current order Status for each order">
                        <ItemTemplate>
                            <asp:Label ID="lblGridOrderStatus" runat="server" Text='<%# Convert.ToString(Eval("BkgOrderStatus"))  %>' />
                            <asp:HiddenField ID="hidGridOrderFlaggedIndicator" runat="server" Value='<%# Eval("OrderFlag") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="DOB" FilterControlAltText="Filter DOB column"
                        DataFormatString="{0:d}"
                        HeaderText="DOB" SortExpression="DOB" UniqueName="CanViewDOB" Display="false"
                        HeaderTooltip="This column displays the dob for each order" ItemStyle-Width="75px"
                        HeaderStyle-Width="50px">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrderFlag" FilterControlAltText="Filter OrderFlag column"
                        Display="false"
                        HeaderText="Order Flag" SortExpression="OrderFlag" UniqueName="CanViewOrderFlag"
                        HeaderTooltip="This column displays the order flag for each order">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="ClearStarStatus" Display="false"
                        HeaderText="ClearStar Status" UniqueName="ClearStarStatusTemp">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Clear Star Status" ItemStyle-Wrap="false"
                        UniqueName="CanViewClearStarStatus" Display="true" FilterControlAltText="Filter ClearStarStatus column"
                        SortExpression="ClearStarStatus">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbVendorStatus" Font-Underline="false" runat="server" Text='<%# Convert.ToString(Eval("ClearStarStatus"))  %>'
                                Enabled="false"
                                Visible="true" CommandName="Upload" CommandArgument='<%# Eval("OrderId") %>' />
                            <asp:Label ID="lblVendorStatus" Font-Underline="false" runat="server" Visible="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Service Groups" ItemStyle-Wrap="false" ItemStyle-Width="100px"
                        Display="false" HeaderTooltip="Service Groups This column displays the Service Groups for each order."
                        UniqueName="CanViewServiceGroups">
                        <ItemTemplate>
                            <asp:Repeater ID="rptServiceGroups" runat="server" OnItemDataBound="rptServiceGroups_ItemDataBound">
                                <ItemTemplate>
                                    <div style="text-align: Left;">
                                        <div id="divGroupStatus" runat="server" visible="false" aria-disabled="false" style="display: inline">
                                            <asp:Image ID="imgStatus" runat="server" ImageUrl='<%# ImagePath + "/blank.gif" %>'
                                                Visible="true" />
                                            <asp:HyperLink ID="hlPackageGroupDocument" runat="server" TabIndex="0"
                                                Visible="true" Target="_blank" onclick="openReportWithServiceGroupID(this)">
                                                <asp:Image ID="imgServiceGroupPDF" runat="server" ImageUrl='<%# ImagePath + "/pdf.gif" %>'
                                                    AlternateText='<%# String.Concat("Click here to view ", Eval("ServicreGroupName")," service group result PDF.") %>' Visible="true" CssClass="hlink" />
                                            </asp:HyperLink>
                                            <asp:HiddenField ID="hdnfServiceGroupID" runat="server" Value='<%#Eval("ServiceGroupId") %>' />
                                            <asp:HiddenField ID="hdnfOrderIDInsideRept" runat="server" Value='<%#Eval("OrderID") %>' />
                                            <asp:HiddenField ID="hdnfBkgPkgSvcGrpID" runat="server" Value ='<%#Eval("BkgPackageSvcGroupId") %>' />
                                        </div>
                                        <telerik:RadButton runat="server" ButtonType="LinkButton" ID="hlViewPackageGroup"
                                            Text='<%# Eval("ServicreGroupName") %>' CommandName="ViewServiceGroup" OnClick="hlViewPackageGroup_Click">
                                        </telerik:RadButton>
                                        <asp:HiddenField ID="hdnBkgServiceGroupMappingId" runat="server" Value='<%# Eval("BkgOrderPackageSvcGroupID") %>' />
                                        <asp:HiddenField ID="svcGrpName" runat="server" Value='<%# Eval("ServicreGroupName") %>' />
                                        <asp:Label ID="lblStatusText" runat="server" Visible="true" />
                                        <%--<asp:Repeater ID="rptStatus" runat="server" Visible="false">
                                            <HeaderTemplate>
                                                <br />
                                                <table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="white-space: nowrap; text-align: Left;">
                                                        <%# Eval("ServicreName")%>:
                                                            <%# Eval("ServiceFormStatus") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap; text-align: Left;">Last Updated:
                                                            <%#  Eval("LastStatusChangeDate", "{0:d}") %>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>--%>
                                    </div>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    <div style="clear: both;">
                                    </div>
                                </SeparatorTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn DataField="ManualServiceForms" FilterControlAltText="Filter CustomAttributes column"
                        AllowFiltering="false" HeaderText="Service Forms" AllowSorting="false" ItemStyle-Width="200px"
                        UniqueName="ManualServiceForms" HeaderTooltip="Service Forms This column displays the Service Forms and their status for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrderNote" FilterControlAltText="Filter OrderNote column"
                        AllowFiltering="false" HeaderText="Order Note" AllowSorting="false" ItemStyle-Width="200px"
                        UniqueName="OrderNote" HeaderTooltip="This column displays the Order Note for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrderNote" FilterControlAltText="Filter OrderNote column" Display="false"
                        AllowFiltering="false" HeaderText="Order Note" AllowSorting="false" ItemStyle-Width="200px"
                        UniqueName="_OrderNote" HeaderTooltip="This column displays the Order Note for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="" ItemStyle-Wrap="false" UniqueName="AddNotes" Display="true"
                        ItemStyle-Width="50px">
                        <ItemTemplate>
                            <telerik:RadButton ButtonType="LinkButton" runat="server" ID="btnAddNotes" Text="Add/View Note"
                                OnClick="btnAddNotes_Click">
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Category" ItemStyle-Wrap="false" UniqueName="CanViewOrderClientStatus"
                        Display="false" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderClientStatus" runat="server" Text='<%# Eval("OrderClientStatusTypeName") %>'
                                Visible='<%#  Convert.ToString(Eval("BkgOrderStatus")) != "Completed" %>' />
                            <asp:Panel ID="pnlOrderClientStatusEdit" runat="server" Visible='<%#  Convert.ToString(Eval("BkgOrderStatus")) == "Completed" %>'>
                                <asp:Label ID="lblClientStatus" runat="server" Enabled="false" Width="75px"></asp:Label>
                                <telerik:RadButton ButtonType="LinkButton" runat="server" ID="hlClientStatus" Text="Update"
                                    OnClick="hlClientStatus_Click">
                                </telerik:RadButton>
                            </asp:Panel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Archive Status" ItemStyle-Wrap="true" UniqueName="CanViewOrderArchiveStatus"
                        Display="false" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Panel ID="pnlOrderClientArchiveEdit" runat="server" Visible='<%#   ( Convert.ToString(Eval("BkgOrderStatus")) == "Completed" || Convert.ToString(Eval("BkgOrderStatus")) == "Cancelled" ) %>'>
                                <telerik:RadButton ToggleType="CheckBox" ButtonType="ToggleButton" ID="rbClientArchive"
                                    runat="server" Checked='<%# Convert.ToBoolean(Eval("ArchiveStatus")) %>' ToolTip="check to change archive status"
                                    AutoPostBack="true" OnCheckedChanged="rbClientArchive_OnCheckedChanged"
                                    BorderWidth="0" BackColor="Transparent" Enabled='true'>
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="" PrimaryIconCssClass="rbToggleCheckboxChecked" />
                                        <telerik:RadButtonToggleState Text="" PrimaryIconCssClass="rbToggleCheckbox" />
                                    </ToggleStates>
                                </telerik:RadButton>
                            </asp:Panel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
    <div class="col-md-12">
        <div class="row text-center">
            <infsu:CommandBar ID="fsucCmdExport" runat="server" ButtonPosition="Center" DisplayButtons="Extra"
                AutoPostbackButtons="Extra" ExtraButtonText="Export D&A Document(s)" OnExtraClick="fsucCmdExport_ExtraClick"
                UseAutoSkinMode="false" ButtonSkin="Silk">
            </infsu:CommandBar>
            <iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>
        </div>
    </div>
</div>
<asp:HiddenField ID="hfOrderID" runat="server" />
<asp:HiddenField ID="hfClientOrderStatus" runat="server" />
<asp:HiddenField ID="hfTenantId" runat="server" />
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />
<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnDepartmntPrgrmMppng" runat="server" Value="" />
<asp:HiddenField ID="hdnHierarchyLabel" runat="server" Value="" />
<asp:HiddenField ID="hdnInstitutionNodeId" runat="server" Value="" />
<asp:HiddenField ID="hdnOrderID" runat="server" />
<asp:HiddenField ID="hdnIsAllItemsChecked" runat="server" />
<asp:HiddenField ID="hdMessageSent" runat="server" Value="new" />
<asp:HiddenField ID="hdnIsEdsAccepted" runat="server" Value="" />
<asp:HiddenField ID="hdnEmployementDiscTypeCode" runat="server" />
<asp:HiddenField ID="hdnOrgUsrId" runat="server" />
<asp:HiddenField ID="hdnIsAdmin" runat="server" />
<asp:HiddenField ID="hdnColorFlagSavedStatus" Value="False" runat="server" />
<asp:HiddenField ID="hdnCurrentClicked" Value="" runat="server" />

<div style="display: none">
    <asp:Button ID="btnDoPostBackForColorFlag" runat="server" AutoPostBack="true"
        OnClick="btnDoPostBackForColorFlag_Click" />
</div>
<script type="text/javascript" language="javascript">
    var winopen = false;
    var minDate = new Date("01/01/1980");

    var editOrderIds = [];    

    function openPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var tenantId = $jQuery("[id$=hfTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var DepartmentProgramId = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            var url = $page.url.create("~/ComplianceOperations/Pages/InstitutionHierarchyList.aspx?TenantId=" + tenantId + "&DepartmentProgramId=" + DepartmentProgramId);
            var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
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

    function SelectedFromDate(picker) {
        var dateFrm = $jQuery("[id$=dpOdrCrtFrm]")[0].control.get_selectedDate();
        if (dateFrm != null) {
            $jQuery("[id$=rfvOrderDateType]")[0].enabled = true;
        }
    }


    function SetMinDateCrtd(picker) {

        var date = $jQuery("[id$=dpOdrCrtFrm]")[0].control.get_selectedDate();
        $jQuery("[id$=rfvOrderDateType]")[0].enabled = true;
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
        var _id = "lbGridOrderId";
        var b = e.get_gridDataItem().findElement(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }

    var winopen = false;
    function openPopUp(sender) {
        var OrderID = $jQuery("#<%= hfOrderID.ClientID %>").val();
        var ClientOrderStatus = $jQuery("#<%= hfClientOrderStatus.ClientID %>").val();
        var TenantId = $jQuery("#<%= hfTenantId.ClientID %>").val();
        var composeScreenWindowName = "Client Order Status";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/BkgOperations/Pages/BkgOrderClientStatus.aspx?OrderID=" + OrderID + "&ClientOrderStatusID=" + ClientOrderStatus + "&TenantId=" + TenantId);
        var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function openAddNotesPopUp(sender) {
        //debugger;
        $jQuery("[id$=hdnCurrentClicked]").val(sender.id);
        var OrderID = $jQuery("#<%= hfOrderID.ClientID %>").val();
        var TenantId = $jQuery("#<%= hfTenantId.ClientID %>").val();
        var composeScreenWindowName = "Background Order Note";
        var width = (window.screen.width) * (90 / 100);
        var height = (window.screen.height) * (80 / 100);
        var popupsize = width + ',' + height;
        var url = $page.url.create("~/BkgOperations/Pages/BkgOrderNote.aspx?OrderID=" + OrderID + "&TenantId=" + TenantId);
        var win = $window.createPopup(url, {
            size: popupsize, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize
                | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close
                | Telerik.Web.UI.WindowBehaviors.Modal, name: composeScreenWindowName, onclose: OnAddNotesClientClose
        }
               );
        winopen = true;
        return false;
    }

    function OnAddNotesClientClose(oWnd, args) {
        //debugger;
        oWnd.remove_close(OnAddNotesClientClose);
        var masterTable = $find("<%= grdBkgOrderDetails.ClientID %>").get_masterTableView();
        masterTable.rebind();
        if (winopen) {
            winopen = false;
        }
        var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
        if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
            setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 2000);
            $jQuery("[id$=hdnCurrentClicked]").val("");
        }
    }

    function openReportWithOrderID(sender) {
      
        var btnID = sender.id;
        //UAT-1923
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "-1");
        $jQuery("[id$=hdnCurrentClicked]").val(btnID);
        var containerID = btnID.substr(0, btnID.indexOf("imgPDF"));
        var TenantId = $jQuery("#<%= hfTenantId.ClientID %>").val()
        //var hfOrderID = $jQuery("#<%= hfOrderID.ClientID %>").val();
        var hdnfOrderID = $jQuery("[id$=" + containerID + "hdnfOrderID]").val();
        var documentType = $jQuery("[id$=hdnEmployementDiscTypeCode]").val();
        var orgUsrId = $jQuery("[id$=hdnOrgUsrId]").val();
        var isAdmin = $jQuery("[id$=hdnIsAdmin]").val();
       
        if ((isAdmin == "true") || ($jQuery("[id$=hdnIsEdsAccepted]").val() == "true") || ($jQuery("[id$=hdnDPM_IsEmploymentType]").val() == "False")) {
            var documentType = "ReportDocument";
            var reportType = "OrderCompletion";
            var composeScreenWindowName = "Report Detail";
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + hdnfOrderID + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + TenantId);
            var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnReportClientClose });
            winopen = true;
            return false;
        }
        else {

            var popupType = "report";
            OpenEmployerDisclosureDocument(TenantId, orgUsrId, documentType, hdnfOrderID, popupType)
        }
    }

    function openReportWithServiceGroupID(sender) {
        //debugger
        var btnID = sender.id;
        //UAT-1955
        $jQuery("[id$=" + sender.id + "]").focusout();
        //UAT-1923
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "-1");
        $jQuery("[id$=hdnCurrentClicked]").val(btnID);
        //var containerID = btnID.substr(0, btnID.indexOf("btnNotificationPdf"));
        var TenantId = $jQuery("#<%= hfTenantId.ClientID %>").val()
        var containerID = btnID.substr(0, btnID.indexOf("hlPackageGroupDocument"));
        var hdnfServiceGroupID = $jQuery("[id$=" + containerID + "hdnfServiceGroupID]").val();
        var hdnfOrderIDInsideRept = $jQuery("[id$=" + containerID + "hdnfOrderIDInsideRept]").val();
        var hdnfBkgPkgSvcGrpID = $jQuery("[id$=" + containerID + "hdnfBkgPkgSvcGrpID]").val();
        var isAdmin = $jQuery("[id$=hdnIsAdmin]").val();

        if ((isAdmin == "true") || ($jQuery("[id$=hdnIsEdsAccepted]").val() == "true") || ($jQuery("[id$=hdnDPM_IsEmploymentType]").val() == "False")) {
            var documentType = "ReportDocument";
            var reportType = "OrderCompletion";
            var composeScreenWindowName = "Filterd Report Detail";
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + hdnfOrderIDInsideRept + "&DocumentType=" + documentType + "&ServiceGroupID=" + hdnfServiceGroupID + "&ReportType=" + reportType + "&tenantId=" + TenantId + "&BkgPkgSvcGrpID=" + hdnfBkgPkgSvcGrpID);
            var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnReportClientClose });
            winopen = true;
            return false;
        }
        else {
            var documentType = $jQuery("[id$=hdnEmployementDiscTypeCode]").val();
            var orgUsrId = $jQuery("[id$=hdnOrgUsrId]").val();
            var popupType = "servicegroup";
            OpenEmployerDisclosureDocument(TenantId, orgUsrId, documentType, hdnfOrderIDInsideRept, popupType, hdnfServiceGroupID, hdnfBkgPkgSvcGrpID)
        }
    }

    function OpenEmployerDisclosureDocument(tenantId, orgUsrId, documentType, orderID, popupType, serviceGroupID, BkgPkgSvcGrpID) {
        var popupWindowName = "Employment Disclosure";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/ComplianceOperations/Pages/ReportEmploymentDisclosure.aspx?DocumentTypeCode=" + documentType + "&TenantID=" + tenantId + "&OrganizationUserID=" + orgUsrId + "&OrderId=" + orderID + "&PopupType=" + popupType + "&ServiceGroupID=" + serviceGroupID + "&BkgPkgSvcGrpID=" + BkgPkgSvcGrpID);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close, onclose: Close },
                                            function () {
                                                this.set_title(popupWindowName);
                                            });
        return false;
    }

    function Close(oWnd, args) {
      //  debugger;
        oWnd.remove_close(Close);
        var arg = args.get_argument();
        if (arg) {
            if (arg.Action == 'success') {
                if ($jQuery("[id$=hdnIsEdsAccepted]")) {
                    $jQuery("[id$=hdnIsEdsAccepted]").val("true");
                }
                //UAT-2364
                var popupHeight = $jQuery(window).height() * (100 / 100);

                if (arg.PopupType == "report") {
                    var documentType = "ReportDocument";
                    var reportType = "OrderCompletion";
                    var composeScreenWindowName = "Report Detail";

                    var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + arg.OrderId + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + arg.TenantId);
                    var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnReportClientClose });
                    winopen = true;
                }
                else if (arg.PopupType == "servicegroup") {
                    var documentType = "ReportDocument";
                    var reportType = "OrderCompletion";
                    var composeScreenWindowName = "Filterd Report Detail";
                    var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + arg.OrderId + "&DocumentType=" + documentType + "&ServiceGroupID=" + arg.ServiceGroupID + "&ReportType=" + reportType + "&tenantId=" + arg.TenantId + "&BkgPkgSvcGrpID=" + arg.BkgPkgSvcGrpID);
                    var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnReportClientClose });
                    winopen = true;
                    return false;
                }
            }
            else {
                //UAT-1923
                window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "0");
                var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
                if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
                    setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 1000);
                    $jQuery("[id$=hdnCurrentClicked]").val("");
                }
            }
            return false;
        }
        //UAT-1923
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "0");
        var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
        if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
            setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 1000);
            $jQuery("[id$=hdnCurrentClicked]").val("");
        }
    }

    function OnReportClientClose(oWnd, args) {
        //debugger;
        oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            winopen = false;
        }
        //UAT-1923
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "0");
        var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
        if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
            setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 1000);
            $jQuery("[id$=hdnCurrentClicked]").val("");
        }
    }
    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        var masterTable = $find("<%= grdBkgOrderDetails.ClientID %>").get_masterTableView();
        masterTable.rebind();
        if (winopen) {
            winopen = false;

        }
    }

    function openHierarchyPopUp() {
        var composeScreenWindowName = "Institution Hierarchy";
        var screenName = "BackgroundScreen";
        //var tenantId = 2;
        var tenantId = $jQuery("[id$=hfTenantId]").val();
        if (tenantId != "0" && tenantId != "") {
            var DelemittedDeptPrgMapIds = $jQuery("[id$=hdnDepartmntPrgrmMppng]").val();
            //UAT-2364
            var popupHeight = $jQuery(window).height() * (100 / 100);

            var url = $page.url.create("~/ComplianceOperations/Pages/NewInstitutionNodeHierarchyList.aspx?TenantId=" + tenantId + "&ScreenName=" + screenName + "&DelemittedDeptPrgMapIds=" + DelemittedDeptPrgMapIds);
            var win = $window.createPopup(url, { size: "600," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClose });
            winopen = true;
        }
        else {
            $alert("Please select Institution.");
        }
        return false;
    }

    function OnClose(oWnd, args) {
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
        }
    }

    //UAT-774

    //function to check all items on Header checked click.
    function CheckAll(id) {
        var masterTable = $find("<%= grdBkgOrderDetails.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectUser").checked = isChecked; // for checking the checkboxes
                var orderId = row[i].getDataKeyValue("OrderId");
                AddOrderIDsInList(isChecked, orderId);
            }
        }
    }

    //function to uncheck header check-box when any of the item is unchecked.
    function UnCheckHeader(id, orderId) {
        var checkHeader = true;
        var masterTable = $find("<%= grdBkgOrderDetails.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        AddOrderIDsInList(id.checked, orderId);
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectUser").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
        hdnIsAllItemsChecked = checkHeader;
    }

    //function to get OrderIds of all checked items and storing them in array.
    function AddOrderIDsInList(isChecked, orderId) {
        if (isChecked) {
            if (editOrderIds.indexOf(orderId) < 0) {
                editOrderIds.push(orderId);
            }
        }
        else {
            editOrderIds = $jQuery.grep(editOrderIds, function (value) {
                return value != orderId;
            });
        }
        $jQuery('[id$="hdnOrderID"]').val(editOrderIds);
    }

    function AddOrderIdsInArray() {
        var orderIds = $jQuery('[id$="hdnOrderID"]').val();
        if (orderIds != null && orderIds != "") {
            editOrderIds = orderIds.split(',');
        }
    }

    //function to Send Message
    function OpenPopup(sender, eventArgs) {
        var composeScreenWindowName = "composeScreen";
        var fromScreenName = "BkgOrderSearchQueue";
        var communicationTypeId = 'CT01';
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/Messaging/Pages/WriteMessage.aspx?cType=" + communicationTypeId + "&SName=" + fromScreenName);
        var win = $window.createPopup(url, { size: "900," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnMessagePopupClose });
        return false;
    }

    function pageLoad() {
        CheckRadGridAllCheckBox('chkSelectUser', 'chkSelectAll');
        SetDefaultButtonForSection("divSearchPanel", "fsucOrderCmdBar_btnSave", true);  //UAT-2860

        //UAT-1955 
        $jQuery('[id$="hlPackageGroupDocument"]').attr("tabindex", "0");
        $jQuery('[id$="hlPackageGroupDocument"]').keyup(function (e) { if (e.keyCode == 13) { openReportWithServiceGroupID(this); } });
        $jQuery("[id$=ddlTenantName]").attr('tabindex', '0');
        $jQuery("[id$=ddlIcon_Input]").attr('role', 'combobox');
        $jQuery("[id$=ddlIcon_Input]").attr('aria-disabled', 'false');
        $jQuery("[id$=rcServiceGroup_Input]").attr('role', 'combobox');
        $jQuery("[id$=rcServiceGroup_Input]").attr('aria-disabled', 'false');  

        $jQuery("div[id*=ucCustomAttributeLoaderSearch] .col-md-3 input:visible").each(function (i, e) {
            var input = $jQuery(this);
            var relatedSpan = $jQuery(input).parents('.col-md-3').children(0);
            var relatedSpanId = $jQuery(relatedSpan).attr('id');

            if ($jQuery(input).attr('type') != 'radio') {
                $jQuery(input).attr('aria-labelledBy', relatedSpanId);
            }
        });

        $jQuery("input[type='radio']").each(function (i, e) {
            var relatedLbl = $jQuery(this).parents('.col-md-3').children().get(0);
            var newLblId = "lblForRbn_" + i;
            var rbnOptionText = $jQuery(this).siblings().attr('id', newLblId);
            $jQuery(this).attr('aria-labelledBy', $jQuery(relatedLbl)[0].id + " " + newLblId);
        });

        $jQuery(".rgHeader").each(function () {
            if (this.innerText == 'Service Groups' || this.innerText == 'Service Forms' || this.innerText == 'Screening Packages' || this.innerText == 'Custom Attributes') {
                //debugger;
                this.tabIndex = 0;
            }
        });
    }
    //Function to check headerbox checkbox on page-load when all items are checked.
    function CheckRadGridAllCheckBox(chkBoxRadGridItemID, chkBoxSelectAllRadGridHeaderItemID) {
        var chkBoxRadGridItemList = $jQuery('input[type=checkbox][id$="' + chkBoxRadGridItemID + '"]');
        var chkBoxRadGridItemUncheckedList = $jQuery('input[type=checkbox][id*= "' + chkBoxRadGridItemID + '"]:not(:checked)');
        if (chkBoxRadGridItemUncheckedList.length == 0 && chkBoxRadGridItemList.length > 0) {
            $jQuery('input[type=checkbox][id$="' + chkBoxSelectAllRadGridHeaderItemID + '"]').attr('checked', 'checked');
        }
    }

    function ResetSelectedUsers() {
        editOrderIds = [];
    }

    //This event fired when Send Message popup closed.
    function OnMessagePopupClose(oWnd, args) {
        oWnd.remove_close(OnMessagePopupClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.MessageSentStatus == "sent") {
                $jQuery("[id$=hdMessageSent]").val("sent");
                var masterTable = $find("<%= grdBkgOrderDetails.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        }
    }


    //UAT-1996: 
    function openColorFlagOrderStatusPopup(sender) {
        //debugger;
        var btnID = sender.id;
        $jQuery("[id$=hdnCurrentClicked]").val(btnID);
        var containerID = btnID.substr(0, btnID.indexOf("lnkBtnColorFlag"));
        var OrderID = $jQuery("[id$=" + containerID + "hdnfOrderID]").val();
        var TenantId = $jQuery("#<%= hfTenantId.ClientID %>").val();
        var hdnInstColorId = $jQuery("[id$=" + containerID + "hdnInstColorId]").val();
        var composeScreenWindowName = "Background Order Color Flag";
        //UAT-2364
        var popupHeight280 = $jQuery(window).height() * (50 / 100);

        var url = $page.url.create("~/BkgOperations/Pages/ChangeBkgOrderColorStatus.aspx?OrderID=" + OrderID + "&TenantId=" + TenantId + "&InstitutionColorStatusID=" + hdnInstColorId);
        var win = $window.createPopup(url, { size: "500," + popupHeight280, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnColorFlagPopupClose });
        winopen = true;
        return false;
    }

    function OnColorFlagPopupClose(oWnd, args) {
        //debugger;
        oWnd.remove_close(OnColorFlagPopupClose);
        var arg = args.get_argument();
        if (arg) {
            $jQuery("[id$=hdnColorFlagSavedStatus]").val(arg.IsStatusSaved);
            if (arg.IsStatusSaved) {
                var masterTable = $find("<%= grdBkgOrderDetails.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        }
        //$jQuery("[id$=btnDoPostBackForColorFlag]").click();
        if (winopen) {
            winopen = false;
        }
        //UAT-1923
        var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
        if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
            setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 100);
            $jQuery("[id$=hdnCurrentClicked]").val("");
        }
    }
</script>
