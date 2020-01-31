<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageUniversalAttributes.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.ManageUniversalAttributes" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>



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

<div class="section" id="dvUniAtrDetails" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Universal Attribute(s)</h2>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdUniversalAttribute" AllowPaging="True" AutoGenerateColumns="False"
            AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0" OnItemCommand="grdUniversalAttribute_ItemCommand" OnItemDataBound="grdUniversalAttribute_ItemDataBound"
            EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false" OnNeedDataSource="grdUniversalAttribute_NeedDataSource">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                HideStructureColumns="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="UF_ID,UF_AttributeDataTypeID,UF_Name,lkpUniversalAttributeDataType.LUADT_Code">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Universal Attribute" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="UF_Name" FilterControlAltText="Filter UF_Name column"
                        HeaderText="Attribute Name" SortExpression="UF_Name" UniqueName="UF_Name">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="lkpUniversalAttributeDataType.LUADT_Name" FilterControlAltText="Filter UC_Name column"
                        HeaderText="Attribute Data Type" SortExpression="lkpUniversalAttributeDataType.LUADT_Name" UniqueName="lkpUniversalAttributeDataType.LUADT_Name">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this universal attribute because deleting this universal attribute will remove all the mapping from tracking and rotation package?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" EditImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif"
                        UniqueName="EditCommandColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridEditCommandColumn>

                </Columns>

                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h2 class="header-color">
                                        <asp:Label ID="lblRotation" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Universal Attribute" : "Update Universal Attribute" %>'
                                            runat="server" />

                                    </h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="msgbox">
                                        <asp:Label ID="lblInfoMessage" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <asp:Panel ID="pnlEditForm" runat="server">
                                <div class="row bgLightGreen">
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
                                                    DataValueField="LUADT_ID" OnSelectedIndexChanged="ddlDataType_SelectedIndexChanged" AutoSkinMode="false" Skin="Silk">
                                                </infs:WclComboBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvDataType" ControlToValidate="ddlDataType" ValidationGroup="grp1"
                                                        InitialValue="--SELECT--" Display="Dynamic" CssClass="errmsg" Text="Attribute Data Type is required." />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3' id="dvOptionType" runat="server" style="display: none">
                                                <span class="cptn">Option</span><span class="reqd">*</span>
                                                <infs:WclTextBox runat="server" ID="txtOption" EmptyMessage="E.g. Positive=1|Negative=2" MaxLength="100" Width="100%" CssClass="form-control" Enabled="true">
                                                </infs:WclTextBox>
                                                <div class="vldx">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvOption" Enabled="false" ControlToValidate="txtOption" ValidationGroup="grp1"
                                                        Display="Dynamic" CssClass="errmsg" Text="Option is required." />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <infsu:CommandBar ID="fsucCmdBarRotation" runat="server" GridMode="true" DefaultPanel="pnlCategory"
                                GridInsertText="Save" GridUpdateText="Save"
                                ValidationGroup="grp1" UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>


