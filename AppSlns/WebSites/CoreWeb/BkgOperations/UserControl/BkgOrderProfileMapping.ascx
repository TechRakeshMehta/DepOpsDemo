<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderProfileMapping.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.BkgOrderProfileMapping" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxBkgOrderProfileMapping">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="col-md-12">
    <div class="row">
        <div class="msgbox" id="divSuccessMsg">
            <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
        </div>
    </div>
</div>

<div class="container-fluid">

    <asp:Panel ID="pnlLinkProfile" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Link Profile
                </h2>
            </div>
        </div>
        <div class="row bgLightGreen">
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3' style="display: none">
                        <span class="cptn">External Vendor</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="cmbExternalVendor" runat="server" DataTextField="EVE_Name" AutoPostBack="false" Visible="false"
                            DataValueField="EVE_ID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnDataBound="cmb_DataBound">
                        </infs:WclComboBox>

                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvExternalVendor" ControlToValidate="cmbExternalVendor" Enabled="false"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="External Vendor is required." InitialValue="--Select--"
                                ValidationGroup="grpFormSubmitLinkProfile" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Vendor Profile ID</span><span class="reqd">*</span>
                        <infs:WclTextBox ID="txtMappedVendorProfileID" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>

                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvVendorProfileID" ControlToValidate="txtMappedVendorProfileID"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Vendor Profile ID is required."
                                ValidationGroup="grpFormSubmitLinkProfile" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Vendor Order ID</span><span class="reqd">*</span>
                        <infs:WclTextBox ID="txtMappedVendorOrderID" runat="server" Width="100%" CssClass="form-control">
                        </infs:WclTextBox>

                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvVendorOrderID" ControlToValidate="txtMappedVendorOrderID"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Vendor Order ID is required."
                                ValidationGroup="grpFormSubmitLinkProfile" />
                        </div>
                    </div>
                </div>
            </div>
            <div class='col-md-12'>
                <div class="row">
                    <div class='form-group col-md-3'>
                        <span class="cptn">Package</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="cmbPackage" runat="server" DataTextField="Value" AutoPostBack="true"
                            DataValueField="Key" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            OnSelectedIndexChanged="cmbPackage_SelectedIndexChanged"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnDataBound="cmb_DataBound">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvPackage" ControlToValidate="cmbPackage"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Package is required." InitialValue="--Select--"
                                ValidationGroup="grpFormSubmitLinkProfile" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Service Group</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="cmbServiceGroup" runat="server" DataTextField="Value" AutoPostBack="true"
                            DataValueField="Key" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            OnSelectedIndexChanged="cmbServiceGroup_SelectedIndexChanged"
                            Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnDataBound="cmb_DataBound">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvServiceGroup" ControlToValidate="cmbServiceGroup"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Service Group is required." InitialValue="--Select--"
                                ValidationGroup="grpFormSubmitLinkProfile" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Service</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="cmbServices" DataTextField="Value"
                            DataValueField="Key" runat="server" Width="100%" CssClass="form-control" OnClientKeyPressing="openCmbBoxOnTab"
                            Skin="Silk" AutoSkinMode="false" OnDataBound="cmb_DataBound">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvServices" ControlToValidate="cmbServices"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Service is required." InitialValue="--Select--"
                                ValidationGroup="grpFormSubmitLinkProfile" />
                        </div>
                    </div>
                    <div class='form-group col-md-3'>
                        <span class="cptn">Vendor Status</span><span class="reqd">*</span>
                        <infs:WclComboBox ID="cmbLineItemStatus" DataTextField="LIRS_Name" Enabled="true"
                            DataValueField="LIRS_ID" runat="server" Width="100%" CssClass="form-control" OnClientKeyPressing="openCmbBoxOnTab"
                            Skin="Silk" AutoSkinMode="false" OnDataBound="cmb_DataBound">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvLineItemStatus" ControlToValidate="cmbLineItemStatus"
                                Display="Dynamic" CssClass="errmsg" ErrorMessage="Vendor status is required." InitialValue="--Select--"
                                ValidationGroup="grpFormSubmitLinkProfile" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12">&nbsp;</div>
            <div class="col-md-12">
                <div class="row ">
                    <div style="float: left; width: 50%;">
                        <infsu:CommandBar ID="fsucCmdBarButton" runat="server" ButtonPosition="Right" DisplayButtons="Save,Cancel"
                            AutoPostbackButtons="Save,Cancel" SaveButtonText="Save" CancelButtonText="Cancel"
                            OnSaveClick="fsucCmdBarButton_SaveClick"  OnCancelClick="fsucCmdBarButton_CancelClick"
                            UseAutoSkinMode="false" ButtonSkin="Silk" ValidationGroup="grpFormSubmitLinkProfile">
                            <%--OnCancelClientClick="ClosePopup"--%>
                        </infsu:CommandBar>
                    </div>
                </div>
            </div>
        </div>

    </asp:Panel>

    <asp:Panel ID="pnlAddNewLineItemMapping" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">External Vendor Profile Mapping
                </h2>
            </div>
        </div>
        <div id="Div1" class="row" runat="server">
            <infs:WclGrid runat="server" ID="grdExternalVendorProfileMapping" AllowPaging="false" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="false" CellSpacing="0" BorderWidth="0px" BorderStyle="NotSet"
                OnNeedDataSource="grdExternalVendorProfileMapping_NeedDataSource" GridLines="None"
                OnItemDataBound="grdExternalVendorProfileMapping_ItemDataBound" OnItemCommand="grdExternalVendorProfileMapping_ItemCommand">
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BkgOrderID,PackageSvcLineItemID" CommandItemSettings-ShowAddNewRecordButton="true" 
                    CommandItemSettings-AddNewRecordText ="Add New Line Item"> 
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                       <%-- <telerik:GridBoundColumn DataField="ExtVendorID" FilterControlAltText="Filter External Vendor column"
                            HeaderText="External Vendor" SortExpression="ExtVendorID" UniqueName="ExtVendorID">
                        </telerik:GridBoundColumn>--%>

                        <telerik:GridBoundColumn DataField="VendorProfileID" FilterControlAltText="Filter Vendor Profile ID column"
                            HeaderText="Vendor Profile ID" SortExpression="VendorProfileID" UniqueName="VendorProfileID">
                            <ItemStyle Wrap="true" Width="300px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="VendorLineItemOrderID" FilterControlAltText="Filter Vendor Order ID column"
                            HeaderText="Vendor Order ID" SortExpression="VendorLineItemOrderID" UniqueName="VendorLineItemOrderID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BackgroundPackageName" FilterControlAltText="Filter Package column"
                            HeaderText="Package" SortExpression="BackgroundPackageName" UniqueName="BackgroundPackageName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ServiceGroupName" FilterControlAltText="Filter Service Group column"
                            HeaderText="Service Group" SortExpression="ServiceGroupName" UniqueName="ServiceGroupName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ServiceName" FilterControlAltText="Filter Service column"
                            HeaderText="Service" SortExpression="ServiceName" UniqueName="ServiceName">
                        </telerik:GridBoundColumn>

                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-12">
                                        <h2 class="header-color">
                                            <asp:Label ID="lblEvProfileMapping" Text='<%# (Container is GridEditFormInsertItem) ? "Add New External Vendor Profile Mapping" : "Edit External Vendor Service" %>'
                                                runat="server" /></h2>
                                    </div>
                                </div>
                                <div class='col-md-12'>
                                    <div class="row">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <asp:Panel runat="server" ID="pnlExternalVendorProfileMapping">
                                    <div class='col-md-12'>
                                        <div class="row">
                                            <div class='form-group col-md-3' style="display: none">
                                                <span class="cptn">External Vendor</span><span class="reqd">*</span>
                                                <infs:WclComboBox ID="cmbExternalVendor" runat="server" DataTextField="EVE_Name" AutoPostBack="false" Visible="false" OnClientKeyPressing="openCmbBoxOnTab"
                                                    DataValueField="EVE_ID" Filter="Contains"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnDataBound="cmb_DataBound">
                                                </infs:WclComboBox>

                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvExternalVendor" ControlToValidate="cmbExternalVendor" Enabled="false"
                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="External Vendor is required." InitialValue="--Select--"
                                                        ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Vendor Profile ID</span><span class="reqd">*</span>
                                                <infs:WclTextBox ID="txtMappedVendorProfileIDGrd" runat="server" Width="100%" CssClass="form-control">
                                                </infs:WclTextBox>

                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvVendorProfileID" ControlToValidate="txtMappedVendorProfileIDGrd"
                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Vendor Profile ID is required."
                                                        ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Vendor Order ID</span><span class="reqd">*</span>
                                                <infs:WclTextBox ID="txtVendorOrderIDGrd" runat="server" Width="100%" CssClass="form-control">
                                                </infs:WclTextBox>

                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvVendorOrderID" ControlToValidate="txtVendorOrderIDGrd"
                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Vendor Order ID is required."
                                                        ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class='col-md-12'>
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Package</span><span class="reqd">*</span>
                                                <infs:WclComboBox ID="cmbPackage" runat="server" DataTextField="Value" AutoPostBack="true" OnClientKeyPressing="openCmbBoxOnTab"
                                                    DataValueField="Key" Filter="Contains"
                                                    OnSelectedIndexChanged="cmbPackage_SelectedIndexChanged"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnDataBound="cmb_DataBound">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvPackage" ControlToValidate="cmbPackage"
                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Package is required." InitialValue="--Select--"
                                                        ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Service Group</span><span class="reqd">*</span>
                                                <infs:WclComboBox ID="cmbServiceGroup" runat="server" DataTextField="Value" AutoPostBack="true" OnClientKeyPressing="openCmbBoxOnTab"
                                                    DataValueField="Key" Filter="Contains"
                                                    OnSelectedIndexChanged="cmbServiceGroup_SelectedIndexChanged"
                                                    Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnDataBound="cmb_DataBound">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvServiceGroup" ControlToValidate="cmbServiceGroup"
                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Service Group is required." InitialValue="--Select--"
                                                        ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Service</span><span class="reqd">*</span>
                                                <infs:WclComboBox ID="cmbServices" DataTextField="Value" OnClientKeyPressing="openCmbBoxOnTab"
                                                    DataValueField="Key" runat="server" Width="100%" CssClass="form-control"
                                                    Skin="Silk" AutoSkinMode="false" OnDataBound="cmb_DataBound">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvServices" ControlToValidate="cmbServices"
                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Service is required." InitialValue="--Select--"
                                                        ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Vendor Status</span><span class="reqd">*</span>
                                                <infs:WclComboBox ID="cmbLineItemStatus" DataTextField="LIRS_Name" OnClientKeyPressing="openCmbBoxOnTab"
                                                    DataValueField="LIRS_ID" runat="server" Width="100%" CssClass="form-control"
                                                    Skin="Silk" AutoSkinMode="false" OnDataBound="cmb_DataBound">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvLineItemStatus" ControlToValidate="cmbLineItemStatus"
                                                        Display="Dynamic" CssClass="errmsg" ErrorMessage="Vendor status is required." InitialValue="--Select--"
                                                        ValidationGroup="grpFormSubmit" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="col-md-12 text-right">
                                <infsu:CommandBar ID="fsucCmdBar" runat="server" GridMode="true" DefaultPanel="pnlExternalVendorProfileMapping"
                                    ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset"
                                    UseAutoSkinMode="false" ButtonSkin="Silk" />
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                </MasterTableView>
            </infs:WclGrid>
        </div>
    </asp:Panel>
</div>

<asp:HiddenField ID="hdnIsSavedSuccessfully" runat="server" /> 
<asp:HiddenField ID="hdnIsUpdatesSuccessfully" runat="server" /> 

<%--<script type="text/javascript">
    // To close the popup.
    function ClosePopup() {
        top.$window.get_radManager().getActiveWindow().close();
    }
</script>--%>
