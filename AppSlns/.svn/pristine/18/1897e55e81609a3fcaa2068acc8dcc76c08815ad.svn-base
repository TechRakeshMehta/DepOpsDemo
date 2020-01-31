<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.PackageList"
    Title="PackageList" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="PackageList.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
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
    <div class="page_cmd">
        <infs:WclButton runat="server" ID="btnAdd" Text="+ Add a Package" OnClick="btnAdd_Click"
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
    </div>
    <div class="section" id="divAddForm" runat="server" visible="false">
        <h1 class="mhdr">Add Package</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage" DefaultButton="btnSave">
                    <%-- <div class="sxgrp" id="divSelect" runat="server" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                Select Package
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbMaster" runat="server" ToolTip="Select from a master list OR create new">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Create New" Value="0" />
                                        <telerik:RadComboBoxItem Text="PACK 1" Value="1" />
                                        <telerik:RadComboBoxItem Text="PACK 2" Value="2" />
                                    </Items>
                                </infs:WclComboBox>
                            </div>
                            <div class='sxlm'>
                                <infs:WclButton runat="server" ID="btnCreate" Text="Create New">
                                    <Icon PrimaryIconCssClass="rbAdd" />
                                </infs:WclButton>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>--%>
                    <div class="sxgrp" runat="server" id="divCreate" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Package Name</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtPackageName" MaxLength="100">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPackageName" ControlToValidate="txtPackageName"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Package Name is required." />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Package Label</span><%--<span class="reqd">*</span>--%>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtPackageLabel" MaxLength="100">
                                </infs:WclTextBox>
                                <%-- <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rFvPackageLabel" ControlToValidate="txtPackageLabel"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Package Label is required." />
                                </div>--%>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Screen Label</span><%--<span class="reqd">*</span>--%>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtScreenLabel" MaxLength="100">
                                </infs:WclTextBox>
                                <%-- <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvScreenLabel" ControlToValidate="txtScreenLabel"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Screen Label is required." />
                                </div>--%>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Is Active</span>
                            </div>
                            <div class='sxlm'>
                                <%--<infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                    AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>--%>
                                <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Show Details in Order Flow</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclButton runat="server" ID="chkViewdetails" ToggleType="CheckBox" ButtonType="ToggleButton"
                                    AutoPostBack="false" Checked="true">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" Value="True" Selected="true" />
                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Description</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtPkgDescription" MaxLength="250">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Package Type</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbCompliancePackageType" runat="server" DataTextField="CPT_Name" AutoPostBack="false"
                                    DataValueField="CPT_ID">
                                </infs:WclComboBox>
                                <div class='vldx'>
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Checklist URL</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtChkDocumentURL" MaxLength="512">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RegularExpressionValidator ID="revCheckListDocument" runat="server" ControlToValidate="txtChkDocumentURL"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please enter a valid URL (i.e. - http://www.Example.com)."
                                        ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>

                        <div class='sxro sx3co monly'>
                            <div class='sxlb'>
                                <span class="cptn">Explanatory Notes</span>
                            </div>
                            <infs:WclTextBox runat="server" ID="txtPkgNotes" TextMode="MultiLine" Height="50px">
                            </infs:WclTextBox>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co monly'>
                            <div class='sxlb'>
                                <span class="cptn">Exception Description</span>
                            </div>
                            <infs:WclTextBox runat="server" ID="txtPkgExceptionDesc" TextMode="MultiLine" Height="50px">
                            </infs:WclTextBox>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co monly'>
                            <div class='sxlb'>
                                <span class="cptn">Package Detail</span>
                            </div>
                            <div class="sxro">
                                <infs:WclEditor ID="rdEditorPackageDetail" ClientIDMode="Static" runat="server" ToolsFile="~/BkgSetup/Data/Tools.xml" Width="100%" EnableResize="false"
                                    Height="150px">
                                </infs:WclEditor>
                                <div class='vldx'>
                                    <asp:CustomValidator runat="server" ID="cstValEditorPackageDetail" ControlToValidate="rdEditorPackageDetail" ClientValidationFunction="ValidatePackageDetailLength"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please don't enter more than 1000 characters." />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Package Details Display</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rbtnDisplayPosition" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                    <asp:ListItem Text="Above" Value="AAAA"></asp:ListItem>
                                    <asp:ListItem Text="Below" Value="AAAB" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="fsucCmdBarPackage_SaveClick" OnClientClicking="SaveClick"
                        ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="fsucCmdBarPackage_CancelClick">
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
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblTitle" runat="server" Text="Packages"></asp:Label>
        </h1>
        <div class="content">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdPackage" AllowPaging="True" AutoGenerateColumns="False"
                    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false"
                    OnNeedDataSource="grdPackage_NeedDataSource">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="true">
                    </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="CompliancePackageID">
                        <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add new Package" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                                HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="PackageLabel" FilterControlAltText="Filter PackageLabel column"
                                HeaderText="Package Label" SortExpression="PackageLabel" UniqueName="PackageLabel">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                                HeaderText="Description" SortExpression="Description" UniqueName="Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ScreenLabel" FilterControlAltText="Filter ScreenLabel column"
                                HeaderText="Screen Label" SortExpression="ScreenLabel" UniqueName="ScreenLabel">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="IsActive" FilterControlAltText="Filter IsActive column" DataType="System.Boolean"
                                HeaderText="Is Active" SortExpression="IsActive" UniqueName="IsActive">
                                <ItemTemplate>
                                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter TenantName column"
                                HeaderText="Tenant" SortExpression="TenantName" UniqueName="TenantName">
                            </telerik:GridBoundColumn>
                            <%-- <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this record?"
                                Text="Delete" UniqueName="DeleteColumn">
                                <HeaderStyle CssClass="tplcohdr" />
                                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>--%>
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
