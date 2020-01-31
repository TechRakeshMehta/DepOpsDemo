<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedCustomAttributes.ascx.cs" Inherits="CoreWeb.PlacementMatching.Views.SharedCustomAttributes" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%--<%@ Register TagPrefix="uc" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>--%>

<infs:WclResourceManagerProxy runat="server" ID="SysXResourceManagerProxy1">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .marginBtn {
        margin-left: 5px;
    }

    .marginCntrl {
        margin-top: 5px;
    }
</style>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Shared Custom Attributes
            </h2>
        </div>
    </div>
    <div class="row">&nbsp;&nbsp;</div>

    <div class="row">
        <infs:WclGrid Width="100%" CssClass="gridhover" runat="server" ID="grdSharedCustomAttribbutes"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="true"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            OnNeedDataSource="grdSharedCustomAttribbutes_NeedDataSource" OnItemCommand="grdSharedCustomAttribbutes_ItemCommand" OnItemDataBound="grdSharedCustomAttribbutes_ItemDataBound"
            EnableLinqExpressions="false" ShowClearFiltersButton="true">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="SharedCustomAttributeID,SharedCustomAttributeMappingID"
                AllowFilteringByColumn="true">
                <CommandItemSettings ShowAddNewRecordButton="true" ShowExportToCsvButton="true" AddNewRecordText="Add New Attribute"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <AlternatingItemStyle BackColor="#f2f2f2" />
                <ItemStyle BackColor="#ffffff" />
                <Columns>
                    <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency" AllowSorting="true" SortExpression="AgencyName"
                        HeaderTooltip="This column displays the agency root node name for each record in the grid"
                        UniqueName="AgencyName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AttributeName" HeaderText="Attribute Name" AllowSorting="true" SortExpression="AttributeName"
                        HeaderTooltip="This column displays the attribute name for each record in the grid"
                        UniqueName="AttributeName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AttributeLabel" HeaderText="Attribute Label" AllowSorting="true" SortExpression="AttributeLabel"
                        HeaderTooltip="This column displays the attribute label for each record in the grid"
                        UniqueName="AttributeLabel">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AttributeDataType" HeaderText="Data Type" AllowSorting="true" SortExpression="AttributeDataType"
                        HeaderTooltip="This column displays the attribute data type for each record in the grid"
                        UniqueName="AttributeDataType">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AttributeUseType" HeaderText="Use Type" AllowSorting="true"
                        SortExpression="AttributeUseType" HeaderTooltip="This column displays the attribute use type for each record in the grid" UniqueName="AttributeUseType">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="IsActive" HeaderText="Is Active" SortExpression="IsActive" AllowFiltering="false"
                        HeaderTooltip="This column displays whether the attribute is active or not for each record in the grid"
                        UniqueName="IsActive">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="IsRequired" HeaderText="Is Required" SortExpression="IsRequired" AllowFiltering="false"
                        HeaderTooltip="This column displays whether the attribute is required or not for each record in the grid"
                        UniqueName="IsRequired">
                    </telerik:GridBoundColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" />
                    </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Attribute?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>

                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h2 class="header-color">
                                        <asp:Label ID="lblRotation" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Attribute" : "Update Attribute" %>'
                                            runat="server" />
                                    </h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="msgbox">
                                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel ID="pnlEditForm" CssClass="editForm" runat="server">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <span class='cptn'>Agency</span><span class="reqd">*</span>
                                                <infs:WclComboBox ID="cmbAgency" runat="server" AutoPostBack="false" DataTextField="AgencyName"
                                                    CssClass="form-control marginCntrl" Skin="Silk" AutoSkinMode="false" Width="100%" Filter="Contains"
                                                    DataValueField="AgencyID">
                                                </infs:WclComboBox>
                                                <div class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAgency" ControlToValidate="cmbAgency" InitialValue="--Select--"
                                                        class="errmsg" Display="Dynamic" ErrorMessage="Agency is required." ValidationGroup='grpAttribute' />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3' title="Enter the shared attribute name.">
                                                <span class="cptn">Attribute Name</span><span class='reqd'>*</span>
                                                <infs:WclTextBox runat="server" ID="txtAttributeName" Text='<%# Eval("AttributeName") %>' Width="100%" CssClass="form-control marginCntrl" Skin="Silk" AutoSkinMode="false"
                                                    MaxLength="200">
                                                </infs:WclTextBox>
                                                <div class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvAttributeName" ControlToValidate="txtAttributeName"
                                                        class="errmsg" Display="Dynamic" ErrorMessage="Attribute Name is required." ValidationGroup='grpAttribute' />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3' title="Enter the shared attribute label.">
                                                <span class="cptn">Attribute Label</span>
                                                <infs:WclTextBox runat="server" ID="txtAttributeLabel" Text='<%# Eval("AttributeLabel") %>' Width="100%" CssClass="form-control marginCntrl"
                                                    Skin="Silk" AutoSkinMode="false"
                                                    MaxLength="200">
                                                </infs:WclTextBox>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class='cptn'>Is Active</span>
                                                <%--  <uc:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" Checked='<%# (Container is GridEditFormInsertItem)? true : Eval("IsActive") %>' />--%>

                                                <asp:RadioButtonList ID="rblIsActive" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                                    Width="100%" CssClass="form-control marginCntrl" Skin="Silk" AutoSkinMode="false">
                                                    <asp:ListItem Text="Yes " Value="True" Selected="True" />
                                                    <asp:ListItem Text="No" Value="False" />
                                                </asp:RadioButtonList>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-md-12">&nbsp;</div>
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class='form-group col-md-3'>
                                                <span class='cptn'>Data Type</span><span class="reqd">*</span>
                                                <infs:WclComboBox ID="cmbDataType" runat="server" AutoPostBack="true" DataTextField="Name" CssClass="form-control marginCntrl" Skin="Silk" AutoSkinMode="false" Width="100%"
                                                    DataValueField="CustomAttributeDataTypeID" OnSelectedIndexChanged="cmbDataType_SelectedIndexChanged" Filter="Contains">
                                                </infs:WclComboBox>
                                                <div class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvDataType" ControlToValidate="cmbDataType" InitialValue="--Select--"
                                                        class="errmsg" Display="Dynamic" ErrorMessage="Data type is required." ValidationGroup='grpAttribute' />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class='cptn'>Use Type</span><span class="reqd">*</span>
                                                <infs:WclComboBox ID="cmbUseType" runat="server" AutoPostBack="true" DataTextField="SCAUT_Name" CssClass="form-control marginCntrl" Skin="Silk" AutoSkinMode="false" Width="100%"
                                                    DataValueField="SCAUT_ID" Filter="Contains">
                                                </infs:WclComboBox>
                                                <div class='vldx'>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvUseType" ControlToValidate="cmbUseType" InitialValue="--Select--"
                                                        class="errmsg" Display="Dynamic" ErrorMessage="Attribute use type is required." ValidationGroup='grpAttribute' />
                                                </div>
                                            </div>
                                            <div class='form-group col-md-3'>
                                                <span class="cptn">Is Required</span>
                                                <asp:RadioButtonList ID="rblIsRequired" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" Width="100%" CssClass="form-control marginCntrl" Skin="Silk" AutoSkinMode="false">
                                                    <asp:ListItem Text="Yes " Value="True" Selected="True" />
                                                    <asp:ListItem Text="No" Value="False" />
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12">&nbsp;</div>

                                    <div id="dvTextTypeInputs" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3'>
                                                    <span class='cptn'>Maximum Characters</span><span class="reqd">*</span>
                                                    <infs:WclNumericTextBox Type="Number" ID="ntxtTextMaxChars" MaxLength="9" NumberFormat-DecimalDigits="0"
                                                        Width="100%" CssClass="form-control marginCntrl" Skin="Silk" AutoSkinMode="false"
                                                        runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a number" Text='<%# Eval("StringLength") %>'>
                                                        <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                                    </infs:WclNumericTextBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvMaximumCharacters" ControlToValidate="ntxtTextMaxChars"
                                                            class="errmsg" Display="Dynamic" ErrorMessage="Maximum Character is required."
                                                            ValidationGroup='grpAttribute' />
                                                    </div>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class='cptn'>Regular Expression</span>
                                                    <infs:WclTextBox runat="server" ID="txtRegularExp" Text='<%# Eval("RegularExpression") %>' Width="100%"
                                                        CssClass="form-control marginCntrl" Skin="Silk" AutoSkinMode="false">
                                                    </infs:WclTextBox>
                                                </div>
                                                <div class='form-group col-md-3'>
                                                    <span class='cptn'>Error Message</span>
                                                    <infs:WclTextBox runat="server" ID="txtRegExpErrorMsg" Text='<%# Eval("RegExpErrorMsg") %>' Width="100%"
                                                        CssClass="form-control marginCntrl" Skin="Silk" AutoSkinMode="false"
                                                        MaxLength="1024">
                                                    </infs:WclTextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="dvValidate" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class='form-group col-md-3' style="display: none">
                                                    <span class='cptn'>Related Profile Attribute</span>
                                                    <infs:WclComboBox ID="cmbRelatedAttribute" runat="server" DataTextField="SCA_AttributeName"
                                                        DataValueField="SCA_ID">
                                                    </infs:WclComboBox>
                                                </div>
                                                <div class='form-group col-md-4'>
                                                    <span class='cptn'>Input Text to Validate</span>
                                                    <div style="float: left; width: 75%;">
                                                        <infs:WclTextBox runat="server" ID="txtValString" MaxLength="1024" Width="100%" CssClass="form-control marginCntrl" Skin="Silk" AutoSkinMode="false">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div style="float: right; width: 25%;">
                                                        <infs:WclButton runat="server" ID="btnValidateRegExp" Text="Validate" OnClick="btnValidateRegExp_Click" AutoPostBack="true" Width="100%" Skin="Silk" AutoSkinMode="false" CssClass="marginBtn"></infs:WclButton>
                                                    </div>
                                                    <div style="float: left;">
                                                        <asp:Label runat="server" ID="lblValidStatus"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <infsu:CommandBar ID="fsucCmdBarSharedAttributes" runat="server" GridMode="true" DefaultPanel="pnlEditForm"
                                GridInsertText="Save" GridUpdateText="Save"
                                ValidationGroup="grpAttribute" UseAutoSkinMode="false" ButtonSkin="Silk" />
                    </FormTemplate>
                </EditFormSettings>


                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)"
                    Position="TopAndBottom" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="false">
            </FilterMenu>
        </infs:WclGrid>

    </div>
</div>
