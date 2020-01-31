<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UniversalComplianceMappingView.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.UniversalComplianceMappingView" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxRotationDetails">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/KeyBoardSupport.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

</infs:WclResourceManagerProxy>
<link type="text/css" href="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" rel="stylesheet" />
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
        padding-top: 5px;
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

     .rbSkinnedButton .RadButton_Silk {
        height: 91% !important;
        width: 12% !important;
    }

    span.compressButton {
        max-height: 83% !important;
        width: 12% !important;
    }


    .compressButton .rbDecorated {
        line-height: 19px !important;
    }

    .RadGrid_Default .rgMasterTable .rgSelectedCell, .RadGrid_Default .rgSelectedRow {
        background: none !important;
        background-color: #828282 !important;
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
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Universal Compliance Mapping View
            </h2>
        </div>
    </div>

    <div class="row bgLightGreen">
        <asp:Panel ID="pnlSearch" runat="server">
            <div class='col-md-12'>
                <div class="row">
                    <div id="divTenant" runat="server">
                        <div class='form-group col-md-3' title="Select the Institution whose data you want to view">
                            <span class="cptn">Institution</span><span class='reqd'>*</span>

                            <infs:WclComboBox ID="ddlTenant" runat="server" DataTextField="TenantName" AutoPostBack="true"
                                DataValueField="TenantID" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                                OnDataBound="ddlTenant_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFormSearch" CssClass="errmsg"
                                    Text="Institution is required." />
                            </div>
                        </div>
                        <div class='form-group col-md-3' title="Select the Compliance Package whose data you want to view">
                            <span class="cptn">Package</span><span class='reqd'>*</span>

                            <infs:WclComboBox ID="ddlCompliancePackage" runat="server" DataTextField="PackageName" AutoPostBack="true"
                                DataValueField="CompliancePackageID" OnSelectedIndexChanged="ddlCompliancePackage_SelectedIndexChanged"
                                OnDataBound="ddlCompliancePackage_DataBound" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlCompliancePackage"
                                    InitialValue="--Select--" Display="Dynamic" ValidationGroup="grpFormSearch" CssClass="errmsg"
                                    Text="Compliance package is required." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <div class="row">
        <infs:WclGrid runat="server" ID="grdCategory" AllowCustomPaging="false"
            AutoGenerateColumns="False" AllowSorting="false" AllowFilteringByColumn="false" EnableLinqExpressions="false"
            CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" ShowClearFiltersButton="false" OnItemCreated="grdUniversalRotationMappingView_ItemCreated"
            NonExportingColumns="" ValidationGroup="" EnableDefaultFeatures="true" OnItemCommand="grdUniversalRotationMappingView_ItemCommand"
            OnNeedDataSource="grdUniversalRotationMappingView_NeedDataSource" OnItemDataBound="grdUniversalRotationMappingView_ItemDataBound" OnPreRender="grdUniversalRotationMappingView_PreRender">
            <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" CommandItemSettings-ShowAddNewRecordButton="false"
                DataKeyNames="UniversalCatMappingID,ComplianceCategoryID,UniversalCategoryID" HierarchyDefaultExpanded="false"
                AllowFilteringByColumn="false" Name="Catagory">
                <Columns>
                    <telerik:GridBoundColumn AllowSorting="false" DataField="ComplianceCategoryName" FilterControlAltText="Filter ComplianceCategoryName column"
                        HeaderText="Compliance Category" SortExpression="ComplianceCategoryName" UniqueName="ComplianceCategoryName">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Expand" HeaderStyle-Width="900px">
                        <ItemTemplate>
                            <telerik:RadButton ID="btnExpand" CommandName="Expand" runat="server" Icon-PrimaryIconCssClass="fa fa-expand" CssClass="compressButton"
                                ToolTip="Click here to expand category to attribute level." Text="Expand All" Skin="Silk">
                                <Icon PrimaryIconTop="17%" PrimaryIconLeft="13%" />
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--         <telerik:GridBoundColumn AllowSorting="false" DataField="UniversalCategoryName" FilterControlAltText="Filter UniversalCategoryName column"
                        HeaderText="Universal Category" SortExpression="UniversalCategoryName" UniqueName="UniversalCategoryName">
                    </telerik:GridBoundColumn>--%>
                    <%--                <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn"
                        EditImageUrl="../../Resources/Mod/Dashboard/images/editGrid.gif">
                        <HeaderStyle Width="30px" />
                    </telerik:GridEditCommandColumn>--%>
                </Columns>
                <%--    <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="bgLightGreen" runat="server" id="divEditBlock" visible="true">
                            <div class="col-md-12 bgLightGreen">
                                <h2 class="header-color paddTopBottom10 marginTopBottom0 heighAuto">
                                    <asp:Label ID="lblTitle" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Category" : "Update Compliance and Universal Category Mapping" %>'
                                        runat="server" /></h2>
                            </div>
                            <div class="msgbox">
                                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                            </div>
                            <asp:Panel runat="server" ID="pnlCategory">
                                <div class="col-md-12">
                                    <div class="row bgLightGreen">
                                        <div class='form-group col-md-3'>
                                            <span class="cptn">Compliance Category</span>
                                            <asp:Label ID="lblComplianceCategory" Text='<%# Eval("ComplianceCategoryName") %>' runat="server" CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false" />
                                        </div>

                                        <div class='form-group col-md-3'>
                                            <asp:Label ID="lblSelectClient" runat="server" Text="Universal Category" CssClass="cptn"></asp:Label>
                                            <infs:WclComboBox ID="ddlUniversalCategory" runat="server" AutoPostBack="true" OnDataBound="ddlUniversalCategory_DataBound"
                                                DataTextField="UC_Name" DataValueField="UC_ID" EmptyMessage="--Select--" Filter="None" OnClientKeyPressing="openCmbBoxOnTab"
                                                CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false">
                                            </infs:WclComboBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-md-12 text-right">
                            <infsu:CommandBar ID="fsucCmdBarCategory" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                GridInsertText="Save" GridUpdateText="Save" ValidationGroup="grpValdUniversal"
                                ExtraButtonIconClass="icnreset" UseAutoSkinMode="False" ButtonSkin="Silk" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>--%>
                <NestedViewTemplate>
                    <div class="swrap">
                        <infs:WclGrid runat="server" ID="grdItems" AllowPaging="false"
                            AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowClearFiltersButton="false"
                            ShowAllExportButtons="false" AllowFilteringByColumn="false"
                            PagerStyle-ShowPagerText="false" EnableDefaultFeatures="false" OnItemCommand="grdItems_ItemCommand" OnItemCreated="grdItems_ItemCreated"
                            OnItemDataBound="grdItems_ItemDataBound" OnNeedDataSource="grdItems_NeedDataSource">
                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="UniversalItemID,ComplianceItemID,UniversalItemMappingID,UniversalCatMappingID,ComplianceCategoryItemID,UniversalCatItemMappingID "
                                HierarchyDefaultExpanded="false"
                                AllowFilteringByColumn="false">
                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                    ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="ComplianceItemName" FilterControlAltText="Filter ComplianceItemName column"
                                        HeaderText="Compliance Item" SortExpression="ComplianceItemName" UniqueName="ComplianceItemName">
                                    </telerik:GridBoundColumn>
                                    <%--           <telerik:GridBoundColumn DataField="UniversalItemName" FilterControlAltText="Filter UniversalItemName column"
                                        HeaderText="Universal Item" SortExpression="UniversalItemName" UniqueName="UniversalItemName">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn"
                                        EditImageUrl="../../Resources/Mod/Dashboard/images/editGrid.gif">
                                        <HeaderStyle Width="30px" />
                                    </telerik:GridEditCommandColumn>--%>
                                </Columns>
                                <%-- <EditFormSettings EditFormType="Template">
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                    <FormTemplate>
                                        <div class="bgLightGreen" runat="server" id="divEditBlock" visible="true">
                                            <div class="col-md-12 bgLightGreen">
                                                <h2 class="header-color paddTopBottom10 marginTopBottom0 heighAuto">
                                                    <asp:Label ID="lblTitle" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Item" : "Update Compliance and Universal Item Mapping" %>'
                                                        runat="server" /></h2>
                                            </div>
                                            <div class="msgbox">
                                                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                            </div>
                                            <asp:Panel runat="server" ID="pnlItem">
                                                <div class="col-md-12">
                                                    <div class="row bgLightGreen">
                                                        <div class='form-group col-md-3'>
                                                            <span class="cptn">Compliance Item</span>
                                                            <asp:Label ID="lblComplianceItem" Text='<%# Eval("ComplianceItemName") %>' runat="server" CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false" />
                                                        </div>

                                                        <div class='form-group col-md-3'>
                                                            <asp:Label ID="lblUItem" runat="server" Text="Universal Item" CssClass="cptn"></asp:Label>
                                                            <infs:WclComboBox ID="ddlUniversalItem" runat="server" OnDataBound="ddlUniversalItem_DataBound" AutoPostBack="false"
                                                                DataTextField="UI_Name" DataValueField="UI_ID" EmptyMessage="--Select--" Filter="None" OnClientKeyPressing="openCmbBoxOnTab"
                                                                CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false">
                                                            </infs:WclComboBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="col-md-12 text-right">
                                            <infsu:CommandBar ID="fsucCmdBarItem" runat="server" GridMode="true" DefaultPanel="pnlItem"
                                                GridInsertText="Save" GridUpdateText="Save" ValidationGroup="grpValdUniversal"
                                                ExtraButtonIconClass="icnreset" UseAutoSkinMode="False" ButtonSkin="Silk" />
                                        </div>
                                    </FormTemplate>
                                </EditFormSettings>--%>
                                <NestedViewTemplate>
                                    <div class="swrap">
                                        <infs:WclGrid runat="server" ID="grdFields" AllowPaging="false"
                                            AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowClearFiltersButton="false"
                                            OnNeedDataSource="grdFields_NeedDataSource" OnItemCreated="grdFields_ItemCreated" OnItemCommand="grdFields_ItemCommand"
                                            ShowAllExportButtons="false" AllowFilteringByColumn="false" PagerStyle-ShowPagerText="false" OnItemDataBound="grdFields_ItemDataBound"
                                            EnableDefaultFeatures="false">
                                            <MasterTableView CommandItemDisplay="Top" DataKeyNames="ComplianceAttributeID,ComplianceItemAttributeID,ComplianceItemAttributeID,ComplianceCategoryItemID,UniversalFieldID,UniversalFieldMappingID,UniversalFieldMappingDate"
                                                HierarchyDefaultExpanded="false" AllowFilteringByColumn="false">
                                                <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                                    ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="ComplianceAttributeName" FilterControlAltText="Filter ComplianceAttributeName column"
                                                        HeaderText="Compliance Attribute" SortExpression="ComplianceAttributeName" UniqueName="ComplianceAttributeName">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ComplianceAttrDataTypeCode" FilterControlAltText="Filter ComplianceAttrDataTypeCode column"
                                                        Display="false" HeaderText="ComplianceAttrDataTypeCode" SortExpression="ComplianceAttrDataTypeCode" UniqueName="ComplianceAttrDataTypeCode">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="UniversalFieldName" FilterControlAltText="Filter UniversalAttributeName column"
                                                        HeaderText="Universal Attribute" SortExpression="UniversalAttributeName" UniqueName="UniversalAttributeName">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="UniversalFieldMappingDate" FilterControlAltText="Filter UniversalFieldMappingDate column"
                                                        HeaderText="Universal Mapping Date" DataFormatString="{0:d}" AllowSorting="false" UniqueName="UniversalFieldMappingDate">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn"
                                                        EditImageUrl="../../Resources/Mod/Dashboard/images/editGrid.gif">
                                                        <HeaderStyle Width="30px" />
                                                    </telerik:GridEditCommandColumn>
                                                </Columns>
                                                <EditFormSettings EditFormType="Template">
                                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                    </EditColumn>
                                                    <FormTemplate>
                                                        <div class="bgLightGreen" runat="server" id="divEditBlock" visible="true">
                                                            <div class="col-md-12 bgLightGreen">
                                                                <h2 class="header-color paddTopBottom10 marginTopBottom0 heighAuto">
                                                                    <asp:Label ID="lblTitle" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Attribute" : "Update Compliance Attribute and Universal Field Mapping" %>'
                                                                        runat="server" /></h2>
                                                            </div>
                                                            <div class="msgbox">
                                                                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                                            </div>
                                                            <asp:Panel runat="server" ID="pnlField">
                                                                <div class="col-md-12">
                                                                    <div class="row bgLightGreen">
                                                                        <div class='form-group col-md-3'>
                                                                            <span class="cptn">Compliance Attribute</span>
                                                                            <asp:Label ID="lblComplianceAttr" Text='<%# Eval("ComplianceAttributeName") %>' runat="server" CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false" />
                                                                        </div>

                                                                        <div class='form-group col-md-3'>
                                                                            <asp:Label ID="lblUField" runat="server" Text="Universal Field" CssClass="cptn"></asp:Label>
                                                                            <infs:WclComboBox ID="ddlUniversalField" runat="server" AutoPostBack="true" OnDataBound="ddlUniversalField_DataBound" DataTextField="UF_Name" DataValueField="UF_ID" EmptyMessage="--Select--" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                                                OnSelectedIndexChanged="ddlUniversalField_SelectedIndexChanged" CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false">
                                                                            </infs:WclComboBox>
                                                                        </div>
                                                                        <div class='form-group col-md-3'>
                                                                            <asp:Label ID="lblInputType" runat="server" Text="Input Type" CssClass="cptn"></asp:Label>
                                                                            <infs:WclComboBox ID="ddlInputAttribute" runat="server" AutoPostBack="false" OnClientDropDownClosed="OnClientDropDownClosedHandler"
                                                                                OnSelectedIndexChanged="ddlInputAttribute_SelectedIndexChanged" DataTextField="UF_Name" DataValueField="UF_ID" EmptyMessage="--Select--" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                                                                CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false" CheckBoxes="true">
                                                                            </infs:WclComboBox>
                                                                        </div>

                                                                    </div>

                                                                    <div class="row bgLightGreen">
                                                                        <div class='form-group col-md-3'>
                                                                            <asp:Label ID="lblSlctInputType" runat="server" Text="Selected Input Type" CssClass="cptn"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                    <asp:Repeater ID="rptrInputTypeAttribute" runat="server">
                                                                        <HeaderTemplate>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div class="row bgLightGreen">
                                                                                <div class='form-group col-md-3'>
                                                                                    <span>
                                                                                        <%#Eval("Name")%></span>
                                                                                    <asp:HiddenField ID="hdnUA_ID" Value='<%#Eval("ID")%>' runat="server" />
                                                                                </div>
                                                                                <div class='form-group col-md-3'>
                                                                                    <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="txtNumericInputPriority" Value='<%#Convert.ToInt32(Eval("InputPriority"))>0?Convert.ToInt32(Eval("InputPriority")):1%>'
                                                                                        MaxValue="2147483647" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter input Priority"
                                                                                        MinValue="1">
                                                                                        <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                                                    </infs:WclNumericTextBox>
                                                                                </div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                    <div id="dvAttributeOptions" runat="server">
                                                                        <div class="row bgLightGreen">
                                                                            <div class='form-group col-md-3'>
                                                                                <asp:Label ID="Label1" runat="server" Text="Option Mapping" CssClass="cptn"></asp:Label>
                                                                            </div>
                                                                        </div>
                                                                        <asp:Repeater ID="rptrCompFieldOptions" runat="server" OnItemDataBound="rptrCompFieldOptions_ItemDataBound">
                                                                            <HeaderTemplate>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div class="row bgLightGreen">
                                                                                    <div class='form-group col-md-3'>
                                                                                        <span>
                                                                                            <%#Eval("ComplianceAttributeOptionText")%></span>
                                                                                        <asp:HiddenField ID="hdnCompAttrOptn_ID" Value='<%#Eval("ComplianceAttributeOptionID")%>' runat="server" />
                                                                                        <asp:HiddenField ID="hdnMappedUniOptID" Value='<%#Eval("MappedUniversalAttrOptionID")%>' runat="server" />
                                                                                    </div>
                                                                                    <div class='form-group col-md-3'>
                                                                                        <infs:WclComboBox ID="ddlUniversalAttrOptions" runat="server" AutoPostBack="false"
                                                                                            OnDataBound="ddlUniversalAttrOptions_DataBound" DataTextField="Value" DataValueField="Key"
                                                                                            EmptyMessage="--Select--" Filter="None" OnClientKeyPressing="openCmbBoxOnTab"
                                                                                            CssClass="form-control" Skin="Silk" Width="100%" AutoSkinMode="false">
                                                                                        </infs:WclComboBox>
                                                                                    </div>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="col-md-12 text-right">
                                                            <infsu:CommandBar ID="fsucCmdBarField" runat="server" GridMode="true" DefaultPanel="pnlField"
                                                                GridInsertText="Save" GridUpdateText="Save" ValidationGroup="grpValdUniversal"
                                                                ExtraButtonIconClass="icnreset" UseAutoSkinMode="False" ButtonSkin="Silk" />
                                                        </div>
                                                    </FormTemplate>
                                                </EditFormSettings>
                                            </MasterTableView>
                                        </infs:WclGrid>
                                        <%--<div class="col-md-12"></div>--%>
                                    </div>
                                </NestedViewTemplate>
                            </MasterTableView>
                        </infs:WclGrid>
                    </div>
                </NestedViewTemplate>
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
</div>
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" />

<script type="text/javascript">


    function OnClientDropDownClosedHandler(sender, eventArgs) {
        __doPostBack("<%= btnDoPostBack.ClientID %>", "");
    }
</script>
