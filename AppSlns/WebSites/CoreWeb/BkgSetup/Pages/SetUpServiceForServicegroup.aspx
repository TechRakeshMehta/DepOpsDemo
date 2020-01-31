<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetUpServiceForServicegroup.aspx.cs"
    Title="SetUpServiceForServicegroup" MasterPageFile="~/Shared/ChildPage.master" Inherits="CoreWeb.BkgSetup.Views.SetUpServiceForServicegroup" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="ServiceFormsDispatchTypes" Src="~/BkgSetup/UserControl/OverridePackageServiceFormDispatchType.ascx" %>
<%@ Register Src="~/Shared/Controls/IsActiveToggle.ascx" TagName="IsActiveToggle"
    TagPrefix="uc1" %>
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
    <div class="dummyBtn" style="display: none;">
        <infs:WclButton runat="server" ID="WclButton1" Text=""
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
    </div>
    <div class="page_cmd">
        <infs:WclButton runat="server" ID="btnAdd" Text="+ Add Services" OnClick="btnAdd_Click"
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
        <infs:WclButton runat="server" ID="btnEdit" Text="+ Edit Service Group" OnClick="btnEdit_Click"
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
    </div>

    <div class="section" id="divAddForm" runat="server" visible="false">
        <h1 class="mhdr">Add Services</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
                    <div class="sxgrp" runat="server" id="divCreate" visible="true">
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Services</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbService" AutoPostBack="true"
                                    runat="server" DataTextField="BSE_Name" DataValueField="BSE_ID" OnDataBound="cmbService_DataBound" OnSelectedIndexChanged="cmbService_SelectedIndexChanged">
                                </infs:WclComboBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvServiceType" ControlToValidate="cmbService"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Service."
                                        InitialValue="--SELECT--" />
                                </div>
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
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Notes</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtSvcNotes" Text="" MaxLength="1024">
                                </infs:WclTextBox>
                            </div>

                            <div id="divPkgCount" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Included In Package Count</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtPkgCount" Text="" MaxLength="9">
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
                        <div class='sxro sx2co' id="divSettings" runat="server" style="display: none;">
                            <div id="divYears" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Number Of Years Of Residence</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtResidenceDuration" Text="" MaxLength="9">
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
                                    <infs:WclTextBox runat="server" ID="txtMinOccurrences" Text="" MaxLength="9">
                                    </infs:WclTextBox>
                                    <div class="vldx">
                                        <asp:RegularExpressionValidator ID="revMinOccurrences" runat="server" ControlToValidate="txtMinOccurrences"
                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter valid number."
                                            ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>

                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx2co' id="divSettings2" runat="server" style="display: none;">
                            <div id="divMaxOcc" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Max Occurrences</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclTextBox runat="server" ID="txtMaxOccurrences" Text="" MaxLength="9">
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
                            <div id="divDocToStud" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Send Documents To Student</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:CheckBox ID="chkSendDocsToStudent" runat="server" Checked="false" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx2co' id="divSettings3" runat="server" style="display: none;">
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
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Is Reportable</span>
                            </div>
                            <div class='sxlm'>
                                 <uc1:IsActiveToggle runat="server" ID="rbIsReportable" Checked="true" IsActiveEnable="true" IsAutoPostBack="false" />
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div id="divServiceForms" runat="server" style="display: none">
                    <uc:ServiceFormsDispatchTypes ID="ucServiceForms" runat="server" />
                    <div class="gclr">
                    </div>
                </div>
            </div>

            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="fsucCmdBarService_SaveClick" OnClientClicking="SaveClick"
                        ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="fsucCmdBarService_CancelClick">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
            <%--<infsu:CommandBar ID="fsucCmdBarPackage" runat="server" DefaultPanel="pnlPackage"
                DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel" OnSaveClick="fsucCmdBarPackage_SaveClick"
                OnCancelClick="fsucCmdBarPackage_CancelClick" ValidationGroup="grpFormSubmit">
            </infsu:CommandBar>--%>
        </div>
    </div>

    <div class="section" id="dvEditServiceGroup" runat="server" visible="false">
        <h1 class="mhdr">Edit Service Group</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="Label1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlServiceGroup">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Service Group Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtSvcGroupName" MaxLength="250">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvSvcGroupName" ControlToValidate="txtSvcGroupName"
                                    class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Service Group Name is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Description</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <infs:WclTextBox runat="server" ID="txtSvcGroupDescription" MaxLength="1024">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Is Active</span>
                        </div>
                        <div class='sxlm'>
                            <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Is First Review Trigger</span>
                        </div>
                        <div class='sxlm'>
                            <uc1:IsActiveToggle runat="server" ID="chkIsFRT" IsActiveEnable="true" IsAutoPostBack="false" />
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Is Second Review Trigger</span>
                        </div>
                        <div class='sxlm'>
                            <uc1:IsActiveToggle runat="server" ID="chkIsSRT" IsActiveEnable="true" IsAutoPostBack="false" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <infs:WclButton ID="btnSaveServiceGroup" runat="server" Text="Save" OnClick="btnSaveServiceGroup_Click" OnClientClicking="SaveClick"
                        ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancelServiceGroup" runat="server" Text="Cancel" OnClick="btnCancelServiceGroup_Click">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
        </div>
    </div>

    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblTitle" runat="server" Text="Services"></asp:Label>
        </h1>
        <div class="content">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdService" AllowPaging="false" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                    AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false" OnNeedDataSource="grdService_NeedDataSource"
                    OnItemDataBound="grdService_ItemDataBound" OnItemCommand="grdService_ItemCommand">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="true">
                    </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="BSE_ID">
                        <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add new Package" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="BSE_Name" FilterControlAltText="Filter Service column"
                                HeaderText="Service Name" SortExpression="BSE_Name" UniqueName="BSE_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Display Name" UniqueName="DisplayName"
                                FilterControlAltText="Filter DisplayName column">
                                <ItemTemplate>
                                    <asp:Label ID="lblDisplayName" runat="server"></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="BSE_Description" FilterControlAltText="Filter Description column"
                                HeaderText="Description" SortExpression="BSE_Description" UniqueName="BSE_Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="BSE_IsDeleted" FilterControlAltText="Filter IsActive column"
                                HeaderText="Is Active" SortExpression="BSE_IsDeleted" UniqueName="BSE_IsDeleted">
                                <ItemTemplate>
                                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("BSE_IsDeleted"))== false ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--  <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter TenantName column"
                                HeaderText="Tenant" SortExpression="TenantName" UniqueName="TenantName">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this record?"
                                Text="Delete" UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <EditFormSettings EditFormType="Template">
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
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
