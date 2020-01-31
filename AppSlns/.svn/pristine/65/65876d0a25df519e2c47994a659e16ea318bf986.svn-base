<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgComplPkgDataMapping.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.BkgComplPkgDataMapping" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style type="text/css">
    .breakword {
        word-break: break-all;
    }

    /*.rgFilterRow {
        display: none !important;
    }*/
</style>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblBkgComplPkgDataMapping" runat="server" Text="Background Compliance Package Data Mapping"></asp:Label></h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info"> </asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlTenant">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <%--<infs:WclDropDownList ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" DefaultMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged">
                        </infs:WclDropDownList>--%>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName"
                            DataValueField="TenantID" EmptyMessage="--Select--" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenant" ControlToValidate="ddlTenant" InitialValue="--SELECT--"
                                Display="Dynamic" ValidationGroup="grpSearch" CssClass="errmsg" Text="Institution is required." Enabled="true" />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackages">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblBkgPackage" runat="server" Text="Background Package" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbBkgPackagesSearch" AutoPostBack="false" DataTextField="BPA_Name" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            DataValueField="BPA_ID" runat="server" OnDataBound="cmbBkgPackages_DataBound" ClientIDMode="Static">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <%-- <asp:RequiredFieldValidator runat="server" ID="rfvBkgPkg" ControlToValidate="cmbBkgPackages" InitialValue="--Select--"
                                Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Background Package is required." Enabled="true" />--%>
                        </div>
                    </div>
                    <div class='sxlb'>
                        <asp:Label ID="lblComplPackage" runat="server" Text="Compliance Package" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbComplPackagesSearch" AutoPostBack="false" DataTextField="PackageName" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                            DataValueField="CompliancePackageID" OnDataBound="cmbComplPackages_DataBound" runat="server" ClientIDMode="Static">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <%-- <asp:RequiredFieldValidator runat="server" ID="rfvComplPkg" ControlToValidate="cmbComplPackages" InitialValue="--Select--"
                                Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Compliance Package is required." Enabled="true" />--%>
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div style="padding-bottom: 5px">
            <infsu:CommandBar ID="CmdBarSearch" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel"
                AutoPostbackButtons="Submit,Save,Cancel" SubmitButtonIconClass="rbRefresh" SubmitButtonText="Reset" DefaultPanel="pnlSearch" DefaultPanelButton="Save"
                SaveButtonText="Search" SaveButtonIconClass="rbSearch" CancelButtonText="Cancel" ValidationGroup="grpSearch"
                OnSubmitClick="CmdBarReset_Click" OnSaveClick="CmdBarSearch_Click" OnCancelClick="CmdBarCancel_Click">
            </infsu:CommandBar>
        </div>
        <div class="swrap" runat="server" id="dvBkgComplPkgDataMapping">
            <infs:WclGrid runat="server" ID="grdBkgComplPkgDataMap" Visible="true" EnableLinqExpressions="false" OnInit="grdBkgComplPkgDataMap_Init"
                AutoSkinMode="true" CellSpacing="0" GridLines="Both" AutoGenerateColumns="False"  AllowCustomPaging="true"
                AllowSorting="true" OnNeedDataSource="grdBkgComplPkgDataMap_NeedDataSource" ShowClearFiltersButton="True" AllowPaging="true" MasterTableView-AllowFilteringByColumn="true"
                EnableDefaultFeatures="true" OnItemDataBound="grdBkgComplPkgDataMap_ItemDataBound" OnItemCommand="grdBkgComplPkgDataMap_ItemCommand" OnInsertCommand="grdBkgComplPkgDataMap_InsertCommand"
                OnSortCommand="grdBkgComplPkgDataMap_SortCommand" 
                OnUpdateCommand="grdBkgComplPkgDataMap_UpdateCommand" OnDeleteCommand="grdBkgComplPkgDataMap_DeleteCommand">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>

                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BCPM_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Mapping" ShowRefreshButton="false"></CommandItemSettings>
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn ItemStyle-CssClass="breakword" DataField="BPA_Name" HeaderText="Background Package"
                            SortExpression="BPA_Name" UniqueName="BPA_Name" FilterControlAltText="Filter BPA_Name column">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BSG_Name" HeaderText="Background Service Group"
                            SortExpression="BSG_Name" UniqueName="BSG_Name" FilterControlAltText="Filter BSG_Name column">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BSE_Name" HeaderText="Background Service"
                            SortExpression="BSE_Name" UniqueName="BSE_Name" FilterControlAltText="Filter BSE_Name column">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="BDPT_Name" HeaderText="Background Data Point"
                            SortExpression="BDPT_Name" UniqueName="BDPT_Name" FilterControlAltText="Filter BDPT_Name column">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageName" HeaderText="Compliance Package"
                            SortExpression="PackageName" UniqueName="PackageName" FilterControlAltText="Filter PackageName column">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CategoryName" HeaderText="Compliance Category"
                            SortExpression="CategoryName" UniqueName="CategoryName" FilterControlAltText="Filter CategoryName column">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ComplianceItemName" HeaderText="Compliance Item"
                            SortExpression="ComplianceItemName" UniqueName="ComplianceItemName" FilterControlAltText="Filter ComplianceItemName column">
                        </telerik:GridBoundColumn>     
                        <telerik:GridBoundColumn DataField="ComplianceAttributeName" HeaderText="Compliance Attribute"
                            SortExpression="ComplianceAttributeName" UniqueName="ComplianceAttributeName" FilterControlAltText="Filter ComplianceAttributeName column">
                        </telerik:GridBoundColumn>
                         <%--  UAT 3582--%>
                         <telerik:GridDateTimeColumn DataField="BCPM_CreatedOn" HeaderText="Date Added" DataFormatString="{0:MM/dd/yyyy}" FilterDateFormat="MM/dd/yyyy" 
                            SortExpression="BCPM_CreatedOn" UniqueName="BCPM_CreatedOn" EnableTimeIndependentFiltering="true" FilterControlAltText="Filter DateAdded column" PickerType="DatePicker" FilterControlWidth="100px" >
                        </telerik:GridDateTimeColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Edit" UniqueName="EditCommandColumn" Text="Edit">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridButtonColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Mapping?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section" visible="true" id="divEditFormBlock" runat="server">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblEHCustomForm" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Mapping" : "Update Mapping" %>'
                                        runat="server" /></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="info"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlCompliaceMapping">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Background Package</span><span class="reqd">*</span>
                                                    <%--<asp:Label ID="lblBkgPackage" runat="server" Text="Background Package" CssClass="cptn"></asp:Label>--%>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbBkgPkg" AutoPostBack="true" DataTextField="BPA_Name" OnDataBound="cmbBkgPkg_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                        OnSelectedIndexChanged="cmbBkgPkg_SelectedIndexChanged" DataValueField="BPA_ID" runat="server" ClientIDMode="Static">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvBkgPkg" ControlToValidate="cmbBkgPkg" InitialValue="--Select--"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Background Package is required." Enabled="true" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <span class="cptn">Compliance Package</span><span class="reqd">*</span>
                                                    <%--<asp:Label ID="lblComplPackage" runat="server" Text="Compliance Package" CssClass="cptn"></asp:Label>--%>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbComplPkg" AutoPostBack="true" DataTextField="PackageName" OnSelectedIndexChanged="cmbCompPkg_SelectedIndexChanged" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                        DataValueField="CompliancePackageID" runat="server" OnDataBound="cmbComplPkg_DataBound" ClientIDMode="Static">
                                                    </infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvComplPkg" ControlToValidate="cmbComplPkg" InitialValue="--Select--"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Compliance Package is required." Enabled="true" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div id="divDataPoints" runat="server" class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <span class="cptn">Background Data Point</span><span class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="cmbDataPoints" AutoPostBack="true" OnSelectedIndexChanged="cmbDataPoints_SelectedIndexChanged" OnDataBound="cmbDataPoints_DataBound" DataTextField="BDPT_Name" DataValueField="BDPT_Code" runat="server" ClientIDMode="Static"></infs:WclComboBox>
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDataPoints" ControlToValidate="cmbDataPoints" InitialValue="--Select--"
                                                            Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Background Data Point is required." Enabled="true" />
                                                    </div>
                                                </div>

                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div runat="server" id="dvSvcGrpPnl" visible="false">
                                                <div class='sxro sx3co' id="dvSvcPnl" runat="server">
                                                    <div runat="server" id="dvServiceGrp" visible="false">
                                                        <div class='sxlb'>
                                                            <span class="cptn">Background Service Group</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclComboBox ID="cmbServiceGroup" AutoPostBack="true" OnSelectedIndexChanged="cmbServiceGroup_SelectedIndexChanged" DataTextField="Name" OnDataBound="cmbServiceGroup_DataBound" DataValueField="ID" runat="server" ClientIDMode="Static"></infs:WclComboBox>
                                                            <div class="vldx">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvServiceGroup" ControlToValidate="cmbServiceGroup" InitialValue="--Select--"
                                                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Background Service Group is required." Enabled="true" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div runat="server" id="dvService" visible="false">
                                                        <div class='sxlb'>
                                                            <span class="cptn">Background Service</span><span class="reqd">*</span>
                                                        </div>
                                                        <div class='sxlm'>
                                                            <infs:WclComboBox ID="cmbServices" DataTextField="Name" AutoPostBack="true" OnDataBound="cmbServices_DataBound" OnSelectedIndexChanged="cmbServices_SelectedIndexChanged" DataValueField="ID" runat="server" ClientIDMode="Static"></infs:WclComboBox>
                                                            <div class="vldx">
                                                                <asp:RequiredFieldValidator runat="server" ID="rfvServices" ControlToValidate="cmbServices" InitialValue="--Select--"
                                                                    Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Background Service is required." Enabled="true" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                                <div class='sxro sx3co'>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Compliance Category</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="cmbCatagory" AutoPostBack="true" OnDataBound="cmbCatagory_DataBound" OnSelectedIndexChanged="cmbCatagory_SelectedIndexChanged" DataTextField="Name" DataValueField="ID" runat="server" ClientIDMode="Static"></infs:WclComboBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvCatagory" ControlToValidate="cmbCatagory" InitialValue="--Select--"
                                                                Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Compliance Category is required." Enabled="true" />
                                                        </div>
                                                    </div>

                                                    <div class='sxlb'>
                                                        <span class="cptn">Compliance Item</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="cmbItems" DataTextField="Name" AutoPostBack="true" OnDataBound="cmbItems_DataBound" OnSelectedIndexChanged="cmbItems_SelectedIndexChanged" DataValueField="ID" runat="server" ClientIDMode="Static"></infs:WclComboBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvItems" ControlToValidate="cmbItems" InitialValue="--Select--"
                                                                Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Compliance Item is required." Enabled="true" />
                                                        </div>
                                                    </div>

                                                    <div class='sxlb'>
                                                        <span class="cptn">Compliance Attribute</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="cmbAttributes" DataTextField="Name" AutoPostBack="true" OnDataBound="cmbAttributes_DataBound" OnSelectedIndexChanged="cmbAttributes_SelectedIndexChanged" DataValueField="ID" runat="server" ClientIDMode="Static"></infs:WclComboBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvAttributes" ControlToValidate="cmbAttributes" InitialValue="--Select--"
                                                                Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Compliance Attribute is required." Enabled="true" />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="dvOption" visible="false">
                                                <div class='sxro sx3co'>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Flagged</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <span><---------------------------------></span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="cmbFlagged" DataTextField="OptionText" AutoPostBack="true" OnDataBound="cmbFlagged_DataBound" OnSelectedIndexChanged="cmbFlagged_SelectedIndexChanged" DataValueField="OptionValue" runat="server" ClientIDMode="Static"></infs:WclComboBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbFlagged" InitialValue="--Select--"
                                                                Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Flagged is required." Enabled="true" />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                                <div class='sxro sx3co'>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Non-Flagged</span><span class="reqd">*</span>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <span><---------------------------------></span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclComboBox ID="cmbNonFlagged" DataTextField="OptionText" AutoPostBack="true" OnDataBound="cmbNonFlagged_DataBound" OnSelectedIndexChanged="cmbNonFlagged_SelectedIndexChanged" DataValueField="OptionValue" runat="server" ClientIDMode="Static"></infs:WclComboBox>
                                                        <div class="vldx">
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="cmbNonFlagged" InitialValue="--Select--"
                                                                Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg" Text="Non-Flagged is required." Enabled="true" />
                                                        </div>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlCompliaceMapping"
                                        ValidationGroup="grpFormSubmit" GridInsertText="Save" GridUpdateText="Save" ExtraButtonIconClass="icnreset" />
                                </div>
                            </div>
                        </FormTemplate>
                    </EditFormSettings>
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div id="divDataSyn" runat="server" style="padding-top: 5px; text-align: center">
            <infs:WclButton runat="server" ID="btnDataSync" Text="Data Sync From Background to Tracking" OnClick="btnDataSync_Click" ValidationGroup="grpSearch"></infs:WclButton>
        </div>

        <div class="gclr">
        </div>
    </div>
</div>
