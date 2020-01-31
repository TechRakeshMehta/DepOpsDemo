<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetUpAttributeGroupForServices.aspx.cs" Inherits="CoreWeb.BkgSetup.Views.SetUpAttributeGroupForServices"
    Title="Attribute group" MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="uc" TagName="ServiceFormsDispatchTypes" Src="~/BkgSetup/UserControl/OverridePackageServiceFormDispatchType.ascx" %>
<%@ Register TagPrefix="uc1" Src="~/Shared/Controls/IsActiveToggle.ascx" TagName="IsActiveToggle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
            parent.Page.hideProgress();
        });

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        //13/02/2014 Changes done for - "Package listing screen : Show splash screen on save"
        function SaveClick(sender, args) {
            if (Page_Validators != undefined && Page_Validators != null) {
                var i;
                for (i = 0; i < Page_Validators.length; i++) {
                    var val = Page_Validators[i];
                    if (!val.isvalid) {
                        return
                    }
                }
            }

            Page.showProgress("Processing...");
            args.set_cancel(false);
        }
    </script>
    <div style="display: none">
        <infs:WclButton runat="server" ID="btnEdit" Text="Page Is Under Progress.."
            Height="30px" AutoPostBack="false" ButtonType="SkinnedButton">
        </infs:WclButton>
    </div>
    <div class="page_cmd">
        <infs:WclButton runat="server" ID="btnEditService" Text="+ Edit Service" OnClick="btnEditService_Click"
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
    </div>

    <div class="section" id="divEditForm" runat="server" visible="false">
        <h1 class="mhdr">Edit Service</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>

                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
                    <div class="sxgrp" runat="server" id="divCreate" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Service</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox ID="txtServiceName" runat="server" Enabled="false" />
                            </div>
                            <%--UAT-3109--%>
                            <div class='sxlb'>
                                <span class="cptn">External Code</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtSvcAMERNumber" Enabled="false">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Display Name</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtSvcDisplayName" Text="" MaxLength="256">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Is Reportable</span>
                            </div>
                            <div class='sxlm'>
                                <uc1:IsActiveToggle runat="server" ID="rbIsReportable" Checked="true" IsActiveEnable="true" IsAutoPostBack="false" />
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Notes</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclTextBox runat="server" ID="txtSvcNotes" Text="" MaxLength="1024">
                                </infs:WclTextBox>
                            </div>
                            <div id="divPkgCount" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Included In Package Count</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtPkgCount" Text="" MaxLength="200">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <asp:RegularExpressionValidator ID="revPkgCount" runat="server" ControlToValidate="txtPkgCount"
                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter valid number."
                                            ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>

                        <div class='sxro sx3co' id="divSettings" runat="server" style="display: none;">
                            <div id="divYears" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Number Of Years Of Residence</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtResidenceDuration" Text="" MaxLength="200">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <asp:RegularExpressionValidator ID="revResidenceDuration" runat="server" ControlToValidate="txtResidenceDuration"
                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter valid number."
                                            ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                            <div id="divMinOcc" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Min Occurrences</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtMinOccurrences" Text="" MaxLength="200">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <asp:RegularExpressionValidator ID="revMinOccurrences" runat="server" ControlToValidate="txtMinOccurrences"
                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter valid number."
                                            ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                            <div id="divMaxOcc" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Max Occurrences</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtMaxOccurrences" Text="" MaxLength="200">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <asp:RegularExpressionValidator ID="revMaxOccurrences" runat="server" ControlToValidate="txtMaxOccurrences"
                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter valid number."
                                            ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="cvMaxOcc" runat="server" Operator="GreaterThan" ControlToCompare="txtMinOccurrences" ControlToValidate="txtMaxOccurrences"
                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Max Occurrence should be greater than Min Occurence." />

                                    </div>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co' id="divSettings2" runat="server" style="display: none;">
                            <div id="divDocToStud" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Send Documents To Student</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:CheckBox ID="chkSendDocsToStudent" runat="server" Checked="false" />
                                </div>
                            </div>
                            <div id="divIsSupplemental" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Is Supplemental</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:CheckBox ID="ChkIsSupplemental" runat="server" Checked="false" />
                                </div>
                            </div>
                            <div id="divIgnoreRHSuppl" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Ignore Residential History On Supplement</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:CheckBox ID="ChkIgnoreRHSuppl" runat="server" Checked="false" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>

                    </div>
                </asp:Panel>
                <div id="divServiceForms" runat="server">
                    <uc:ServiceFormsDispatchTypes ID="ucServiceForms" runat="server" />
                </div>
                <div class='sxroend'>
                </div>
            </div>

            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClicking="SaveClick"
                        ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>

        </div>
    </div>
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblAttributeGroupGridTitle" runat="server" Text="Attribute Groups"></asp:Label>
        </h1>
        <div class="content">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdMappedAttributeGroup" AllowPaging="false" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false"
                    GridLines="None" OnNeedDataSource="grdMappedAttributeGroup_NeedDataSource" OnDeleteCommand="grdMappedAttributeGroup_DeleteCommand"
                    EnableLinqExpressions="false">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="BkgPackageSvcId,AttributeGroupID">
                        <CommandItemSettings ShowAddNewRecordButton="false" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="AttributeGroupName" FilterControlAltText="Filter AttributeGroupName column"
                                HeaderText="Name" SortExpression="AttributeGroupName" UniqueName="Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="AttributeGroupDescription" FilterControlAltText="Filter AttributeGroupDescription column"
                                HeaderText="Description" SortExpression="AttributeGroupDescription"
                                UniqueName="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this record?"
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
            <div class="gclr">
            </div>
        </div>
    </div>
</asp:Content>
