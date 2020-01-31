<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ManageUniversalAttribute.aspx.cs" Title="ManageUniversalAttributes"
    MasterPageFile="~/Shared/ChildPage.master"
    Inherits="CoreWeb.ComplianceAdministration.Views.ManageUniversalAttribute" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxetupUniversalMapping">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript"> 
        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
    </script>
    <div class="container-fluid">
        <div class="col-md-12">
            <div class="row">&nbsp</div>
        </div>
        <div class="col-md-12">
            <div id="dvAddNewAttribute" runat="server" style="text-align: right">
                <infs:WclButton runat="server" ID="btnAddUniAtr" Text="Add Universal Attribute" OnClick="btnAddUniAtr_Click"
                    Icon-PrimaryIconCssClass="rbAddNew" ButtonType="StandardButton" Skin="Silk" AutoSkinMode="false">
                </infs:WclButton>
            </div>
        </div>
        <div id="dvAddAttribute" runat="server" style="display: none">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color">
                        <asp:Label ID="lblHeader" runat="server"></asp:Label></h2>
                </div>
            </div>
            <div class="row bgLightGreen">
                <div class="col-md-12">
                    <div class="msgbox">
                        <asp:Label ID="lblSuccess" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                    </div>
                    <asp:Panel runat="server" ID="pnlCategory">
                        <div class="col-md-12">
                            <div class="row">
                                <div class='form-group col-md-3'>
                                    <span class="cptn">Attribute Name</span><span class="reqd">*</span>
                                    <infs:WclTextBox runat="server" ID="txtAttributeName" MaxLength="100" Enabled="true" Width="100%" CssClass="form-control">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvAttributeName" ControlToValidate="txtAttributeName" ValidationGroup="grp1"
                                            Display="Dynamic" CssClass="errmsg" Text="Attribute Name is required." />
                                    </div>
                                </div>
                                <div class='form-group col-md-3'>
                                    <span class="cptn">Attribute Data Type</span><span class="reqd">*</span>
                                    <infs:WclComboBox ID="ddlDataType" runat="server" DataTextField="LUADT_Name" AutoPostBack="true" Width="100%"
                                        DataValueField="LUADT_Code" OnSelectedIndexChanged="ddlDataType_SelectedIndexChanged" AutoSkinMode="false" Skin="Silk">
                                    </infs:WclComboBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDataType" ControlToValidate="ddlDataType" ValidationGroup="grp1"
                                            InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" Text="Attribute Data Type is required." />
                                    </div>
                                </div>
                                <div class='form-group col-md-3' id="dvOptionType" runat="server" style="display: none">
                                    <span class="cptn">Option</span><span class="reqd">*</span>
                                    <infs:WclTextBox runat="server" ID="txtOption" EmptyMessage="E.g. Positive=1,Negative=2" MaxLength="100" Width="100%" CssClass="form-control" Enabled="true">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <asp:RequiredFieldValidator runat="server" ID="rfvOption" Enabled="false" ControlToValidate="txtOption" ValidationGroup="grp1"
                                            Display="Dynamic" CssClass="errmsg" Text="Option is required." />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="col-md-12">
                        <div class="row">
                            <div style="float: right" id="dvSaveCancelBtn" runat="server">
                                <infsu:CommandBar ID="fsucCmdBarSaveCategory" runat="server" UseAutoSkinMode="false" ButtonSkin="Silk" CancelButtonIconClass="rbCancel"
                                    DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel" ValidationGroup="grp1" SaveButtonIconClass="rbSave"
                                    ButtonPosition="Right" OnSaveClick="fsucCmdBarSaveCategory_SaveClick" OnCancelClick="fsucCmdBarSaveCategory_CancelClick">
                                </infsu:CommandBar>
                            </div>
                            <div style="float: right" id="dvEditBtn" runat="server">
                                <infs:WclButton ID="btnEdit" Text="Edit" runat="server" OnClick="btnEdit_Click" AutoSkinMode="false" Skin="Silk" Icon-PrimaryIconCssClass="rbEdit"></infs:WclButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="row">&nbsp</div>
        </div>
        <div class="section" id="dvUniAtrDetails" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color">Universal Attribute(s)</h2>
                </div>
            </div>
            <div class="row">
                <infs:WclGrid runat="server" ID="grdUniversalAttribute" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0" OnItemCommand="grdUniversalAttribute_ItemCommand"
                    EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false" OnNeedDataSource="grdUniversalAttribute_NeedDataSource">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="true">
                    </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="UA_ID">
                        <CommandItemSettings ShowAddNewRecordButton="false" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="UA_Name" FilterControlAltText="Filter UC_Name column"
                                HeaderText="Attribute Name" SortExpression="UA_Name" UniqueName="UA_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="lkpUniversalAttributeDataType.LUADT_Name" FilterControlAltText="Filter UC_Name column"
                                HeaderText="Attribute Data Type" SortExpression="lkpUniversalAttributeDataType.LUADT_Name" UniqueName="lkpUniversalAttributeDataType.LUADT_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this universal attribute because deleting this universal attribute will remove all the mapping from tracking and rotation package?"
                                Text="Delete" UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </infs:WclGrid>
            </div>
        </div>
    </div>
</asp:Content>
