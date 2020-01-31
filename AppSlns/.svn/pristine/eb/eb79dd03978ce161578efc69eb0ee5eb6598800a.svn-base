<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetupPackagesForInsitiute.aspx.cs"
    Title="SetupPackagesForInsitiute" MasterPageFile="~/Shared/ChildPage.master" Inherits="CoreWeb.BkgSetup.Views.SetupPackagesForInsitiute" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/Shared/Controls/IsActiveToggle.ascx" TagName="IsActiveToggle"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/BkgSetup/BkgContentEditor.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/mapping_pages.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <style type="text/css">
        .reEditorModes a {
            display: none;
        }

        .reToolZone {
            display: none;
        }

        .bullet ul {
            margin-left: 10px;
            padding-left: 10px !important;
        }

        .bullet li {
            list-style-position: inside;
            list-style: disc;
        }

        .bullet ol {
            list-style-type: decimal;
            margin-left: 10px;
            padding-left: 10px;
        }

            .bullet ol li {
                list-style: decimal;
            }
    </style>
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
    <div class="dummyButton" style="display: none;">
        <infs:WclButton runat="server" ID="WclButton1" Text=""
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
    </div>
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
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
                    <div class="sxgrp" runat="server" id="divCreate" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Package Name</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtPackageName" MaxLength="100" ClientEvents-OnLoad="SetFocus">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPackageName" ControlToValidate="txtPackageName"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Package Name is required." />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Is Active</span>
                            </div>
                            <div class='sxlm'>
                                <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />

                                <%--<infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                    AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>--%>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Show Details in Order Flow</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclButton runat="server" ID="chkViewdetails" ToggleType="CheckBox" ButtonType="ToggleButton"
                                    AutoPostBack="false" CssClass="radio_list">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class="sxlb">
                                <span class="cptn">Package Label</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtPkgLabel" MaxLength="250">
                                </infs:WclTextBox>
                            </div>
                            <%--<div class="sxlb">
                                <span class="cptn">Description</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclTextBox runat="server" ID="txtPkgDescription" MaxLength="250">
                                </infs:WclTextBox>
                            </div>--%>
                            <div class='sxlb'>
                                <span class="cptn">Package Details Display</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rbtnDisplayPosition" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                    <asp:ListItem Text="Above" Value="AAAA"></asp:ListItem>
                                    <asp:ListItem Text="Below" Value="AAAB" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Invite Only Package</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rdbInviteOnlyPackage" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>

                            <div class='sxroend'>
                            </div>
                        </div>

                        <div class='sxro sx3co'>
                            <div class="sxlb">
                                <span class="cptn">Is Available for Applicant Order</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rblAvalblForApplicant" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                    <asp:ListItem Text="Yes" Value="True" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Is Available for Admin Order</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rblAvalblForClientAdmin" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Is Required to assign in rotation</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rblIsReqToQualifyInRotation" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">PackageType</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbBkgPackageType" OnDataBound="cmbBkgPackageType_DataBound" runat="server" DataTextField="BPT_Name" AutoPostBack="false"
                                    DataValueField="BPT_Id">
                                </infs:WclComboBox>
                               <div class='vldx'>                                   
                                </div>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class="sxlb">
                                <span class="cptn">Description</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclTextBox runat="server" ID="txtPkgDescription" MaxLength="250">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                        <div id="Div1" runat="server">
                            <div class='sxlb'>
                                <span class="cptn">Passcode</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox ID="txtPasscode" runat="server" MaxLength="6">
                                </infs:WclTextBox>
                                <div class='vldx'>                                    
                                    <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3" ControlToValidate="txtPasscode"
                                        Display="Dynamic" CssClass="errmsg" ValidationExpression="^[\w\d]{1,50}$" ValidationGroup="grpFormSubmit" 
                                        ErrorMessage="Invalid Characters." />
                                </div>
                            </div>
                        </div>

                        <div class='sxroend'>
                        </div>
                    </div> 
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Package Detail</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclEditor ID="rdEditorPackageDetail" ClientIDMode="Static" runat="server" ToolsFile="~/BkgSetup/Data/Tools.xml" Width="99.3%" EnableResize="false"
                                    Height="150px">
                                </infs:WclEditor>
                                <div class='vldx'>
                                    <asp:CustomValidator runat="server" ID="cstValEditorPackageDetail" ControlToValidate="rdEditorPackageDetail" ClientValidationFunction="ValidateBKGPackageDetailLength"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please don't enter more than 1000 characters." />
                                </div>
                            </div>

                            <%--<div class='sxlb'>
                                <span class="cptn">Package Details Display</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rbtnDisplayPosition" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                    <asp:ListItem Text="Above" Value="AAAA"></asp:ListItem>
                                    <asp:ListItem Text="Below" Value="AAAB" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>--%>
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
                <infs:WclGrid runat="server" ID="grdPackage" AllowPaging="false" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                    AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false" OnNeedDataSource="grdPackage_NeedDataSource" OnItemCommand="grdPackage_ItemCommand">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="true">
                    </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="BPA_ID">
                        <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add new Package" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="BPA_Name" FilterControlAltText="Filter PackageName column"
                                HeaderText="Package Name" SortExpression="BPA_Name" UniqueName="BPA_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BPA_Description" FilterControlAltText="Filter Description column"
                                HeaderText="Description" SortExpression="BPA_Description" UniqueName="BPA_Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="BPA_IsActive" FilterControlAltText="Filter IsActive column"
                                HeaderText="Is Active" SortExpression="BPA_IsActive" UniqueName="BPA_IsActive">
                                <ItemTemplate>
                                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("BPA_IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
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
