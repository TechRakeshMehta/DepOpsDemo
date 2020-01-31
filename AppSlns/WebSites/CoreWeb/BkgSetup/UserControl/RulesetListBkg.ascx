<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RulesetListBkg.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.RulesetListBkg" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/Shared/Controls/IsActiveToggle.ascx" TagName="IsActiveToggle"
    TagPrefix="uc1" %>
<infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<div class="page_cmd">
    <infs:WclButton runat="server" ID="btnAdd" Text="+ Add a Ruleset" OnClick="btnAdd_Click"
        Height="30px" ButtonType="LinkButton">
    </infs:WclButton>
</div>
<div class="section" id="divAddForm" runat="server" visible="false">
    <h1 class="mhdr">Add Ruleset</h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
            </div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlRulesets" DefaultButton="btnSave">
                <div class="sxgrp" runat="server" id="divCreate" visible="true">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Ruleset Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtName" MaxLength="100">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvRuleSetName" ControlToValidate="txtName"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Ruleset Name is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Is Active</span>
                        </div>
                        <div class='sxlm'>
                            <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false"/>
                            
                           <%-- <infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>--%>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Description</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclTextBox runat="server" ID="txtDescription" MaxLength="255">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="sxcbar">
            <div class="sxcmds" style="text-align: right">
                <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="grpFormSubmit">
                    <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                        PrimaryIconWidth="14" />
                </infs:WclButton>
                <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_click">
                    <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                        PrimaryIconWidth="14" />
                </infs:WclButton>
            </div>
        </div>
    </div>
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblTitle" Text="Rulesets" runat="server"></asp:Label>
    </h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdRuleSet" AllowPaging="false" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false"
                GridLines="None" OnNeedDataSource="grdRuleSet_NeedDataSource" OnDeleteCommand="grdRuleSet_DeleteCommand">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="BRLS_ID">
                    <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add New Ruleset"
                        ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false"
                        ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="BRLS_Name" 
                            HeaderText="Ruleset Name" SortExpression="BRLS_Name" UniqueName="RLS_Name">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="BRLS_Description" 
                            HeaderText="Description" SortExpression="BRLS_Description" UniqueName="BRLS_Description">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="BRLS_IsActive"
                            HeaderText="Is Active" SortExpression="BRLS_IsActive" UniqueName="BRLS_IsActive">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("BRLS_IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Ruleset?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                    </EditFormSettings>
                </MasterTableView>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
