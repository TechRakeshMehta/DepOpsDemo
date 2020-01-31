<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetUpServiceGroupsForPackage.aspx.cs"
    Title="SetUpServiceGroupsForPackage" MasterPageFile="~/Shared/ChildPage.master" Inherits="CoreWeb.BkgSetup.Views.SetUpServiceGroupsForPackage" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register Src="~/Shared/Controls/IsActiveToggle.ascx" TagName="IsActiveToggle"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="WclResourceManagerProxy1">
        <infs:LinkedResource Path="~/Resources/Mod/BkgSetup/BkgContentEditor.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <style type="text/css">
        th.thState {
            font-size: 13px;
            font-weight: bold;
            padding: 2px 41px 4px 52px;
        }

        th.thStateAbb {
            font-size: 13px;
            font-weight: bold;
            padding: 2px 41px 4px 52px;
        }

        td.tdState {
            border-color: #dedede;
            background-color: #eeeeee;
            color: black;
            border-right-width: 0px;
            padding: 3px 5px 4px;
            border-style: solid;
            width: 35%;
            line-height: 10px !important;
        }

        td.tdStateAbb {
            border-color: #dedede;
            background-color: #eeeeee;
            color: black;
            text-align: center;
            border-style: solid;
            line-height: 10px !important;
        }

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
        <infs:WclButton runat="server" ID="btnAdd" Text="+ Add a Service Group" OnClick="btnAdd_Click"
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
        <infs:WclButton runat="server" ID="btnEdit" Text="+ Edit Package" OnClick="btnEdit_Click"
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
        <infs:WclButton runat="server" ID="btnEditInstructionText" Text="+ Edit Instruction Text" OnClick="btnEditInstructionText_Click"
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>
        <%--<infs:WclButton runat="server" ID="btnEditPkgStateSearchCriteria" Text="+ Edit Package State Search Criteria" OnClick="btnEditPkgStateSearchCriteria_Click"
            ButtonType="LinkButton" Height="30px">
        </infs:WclButton>--%>
    </div>

    <div class="section" id="divAddForm" runat="server" visible="false">
        <h1 class="mhdr">Add Service Group</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
                    <div class="sxgrp" runat="server" id="divCreate" visible="true">
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Select Service Group</span><%--<span class="reqd">*</span>--%>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbServiceGroup" AutoPostBack="true"
                                    runat="server" DataTextField="BSG_Name" DataValueField="BSG_ID" OnSelectedIndexChanged="cmbServiceGroup_OnSelectedIndexChanged">
                                </infs:WclComboBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvServiceType" ControlToValidate="cmbServiceGroup"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Service Group."
                                        InitialValue="--SELECT--" />
                                </div>
                            </div>
                        </div>
                        <div class="sxgrp" id="divAddNewServiceGroup" runat="server">
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
                                <%--  <div class='sxlb'>
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
                                </div>--%>
                            </div>
                        </div>
                        <div class='sxro sx3co' id="dvReviewTriggers" runat="server">
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

                    </div>
                </asp:Panel>
            </div>
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <infs:WclButton ID="btnSave" runat="server" Text="Save" OnClick="fsucCmdBarSvcGroup_SaveClick" OnClientClicking="SaveClick"
                        ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="fsucCmdBarSvcGroup_CancelClick">
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

    <div class="section" id="divEditPackage" runat="server" visible="false">
        <h1 class="mhdr">Edit Package</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="Label1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="Panel1">
                    <div class="sxgrp" runat="server" id="div2" visible="true">
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
                                <uc1:IsActiveToggle runat="server" ID="chkCheckBocPkg" IsActiveEnable="true" IsAutoPostBack="false" />
                                <%-- <infs:WclButton runat="server" ID="chkCheckBocPkg" ToggleType="CheckBox" ButtonType="ToggleButton"
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
                                    AutoPostBack="false">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class="sxro sx3co">
                            <div class="sxlb">
                                <span class="cptn">Package Label</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtPkgLabel" MaxLength="250">
                                </infs:WclTextBox>
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

                            <div class='sxlb'>
                                <span class="cptn">Invite Only Package</span>
                            </div>
                            <div class='sxlm'>
                                <asp:RadioButtonList ID="rdbInviteOnlyPackage" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <%-- <div class="sxlb">
                                <span class="cptn">Description</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <infs:WclTextBox runat="server" ID="txtPkgDescription" MaxLength="250">
                                </infs:WclTextBox>
                            </div>--%>
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
                                <infs:WclComboBox ID="cmbBkgPackageType" runat="server" DataTextField="BPT_Name" AutoPostBack="false"
                                    DataValueField="BPT_Id">
                                </infs:WclComboBox>
                                 <div class='vldx'>
                                </div>
                            </div>
                            <div class='sxroend'>
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
                        <div id="Div3" runat="server">
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
                        <div id="dvAutpmaticPackageInvitation" runat="server">
                            <h2 style="color: #bd6a38" class="mhdr">Automatic Package Invitation</h2>
                            <div class="sxro sx3co">
                                <div class='sxlb'>
                                    <span class="cptn">Automatic Package Invitation Setting</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:RadioButton ID="rbtnTriggerAutomaticPackageInvitationYes" OnCheckedChanged="rbtnTriggerAutomaticPackageInvitationYes_CheckedChanged" AutoPostBack="true" runat="server" GroupName="TriggerAutomaticPackageInvitation" Text="Yes" />
                                    <asp:RadioButton ID="rbtnTriggerAutomaticPackageInvitationNo" OnCheckedChanged="rbtnTriggerAutomaticPackageInvitationNo_CheckedChanged" AutoPostBack="true" runat="server" GroupName="TriggerAutomaticPackageInvitation" Text="No" />
                                </div>

                                <div class='sxroend'>
                                </div>

                            </div>
                            <div runat="server" id="dvShowSettings">
                                <div class="sxro sx3co">
                                    <div class="sxlb">
                                        <span class="cptn">Month(s)</span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ShowSpinButtons="True" Type="Number" ID="txtAutomaticInvitationMonth"
                                            MaxValue="99" runat="server" InvalidStyleDuration="100" EmptyMessage="Enter a month"
                                            MinValue="1">
                                            <NumberFormat AllowRounding="true" DecimalDigits="0" DecimalSeparator="," GroupSizes="3" />
                                        </infs:WclNumericTextBox>
                                        <div style="color: red" class='vldx'>
                                            <asp:Label ID="lblErrorMsg" Visible="false" Style="color: red" runat="server" Text="Month is required."></asp:Label>
                                        </div>
                                    </div>
                                    <div class="sxlb">
                                        <span class="cptn">Package(s)</span><span class="reqd">*</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclComboBox ID="chkBkgInvitationPackages" runat="server" Width="277px" CheckBoxes="true" EmptyMessage="--SELECT--"
                                            AutoPostBack="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                                            <Localization CheckAllString="Select All" />
                                        </infs:WclComboBox>
                                        <div style="color: red" class='vldx'>
                                            <asp:Label ID="lblSeletedPackageError" Visible="false" Style="color: red" runat="server" Text="Package is required."></asp:Label>
                                        </div>
                                    </div>
                                    <%-- <div class="vldx">
                                    <asp:CustomValidator ID="rfvchkBkgPackages" CssClass="errmsg" Display="Dynamic" runat="server"
                                        ErrorMessage="Package is required." EnableClientScript="true" ValidationGroup="grpFormSubmit" Enabled="false"
                                        ClientValidationFunction="ValidateBackgroundPackage">
                                    </asp:CustomValidator>
                                </div>--%>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </div>
                </asp:Panel>
            </div>
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <infs:WclButton ID="WclButton3" runat="server" Text="Save" OnClick="fsucCmdBarPackage_SaveClick" OnClientClicking="SaveClick"
                        ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="WclButton4" runat="server" Text="Cancel" OnClick="fsucCmdBarPackage_CancelClick">
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
    <div class="section" id="divEditInstructionText" runat="server" visible="false">
        <h1 class="mhdr">Data Form Instructions</h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="Panel2">
                    <div class="sxgrp" runat="server" id="div1" visible="true">
                        <div class='sxro sx2co'>
                            <div class='sxlb'>
                                <span class="cptn">Choose Attribute Group</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbAttributeGroups" DataTextField="AttributeGroupName" DataValueField="AttributeGroupId" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbAttributeGroups_SelectedIndexChanged"></infs:WclComboBox>

                            </div>
                            <div class='sxlm'>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvcmbAttributeGroups" ControlToValidate="cmbAttributeGroups" InitialValue="--SELECT--"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Attribute Group is required." />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx2co'>
                            <div class='sxlb '>
                                <span class="cptn">Instruction Text</span>
                            </div>
                            <div class='sxlm m2spn'>
                                <asp:Panel CssClass="content" ID="pnlRadContent" runat="server" Height="250">
                                    <infs:WclEditor ID="radHTMLEditor" ToolsFile="~/WebSite/Data/Tools.xml" runat="server" EditModes="Preview"
                                        Width="98%" Height="242px" OnClientLoad="OnClientLoad" EnableResize="false">
                                        <%--  <ImageManager ViewPaths="~/InstitutionImages" UploadPaths="~/InstitutionImages" DeletePaths="~/InstitutionImages"
                                             MaxUploadFileSize="7100000" />--%>
                                    </infs:WclEditor>
                                </asp:Panel>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: right">
                    <infs:WclButton ID="btnSaveInst" runat="server" Text="Save" OnClick="btnSaveInst_Click" OnClientClicking="SaveClick"
                        ValidationGroup="grpFormSubmit">
                        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancelInst" runat="server" Text="Cancel" OnClick="btnCancelInst_Click">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
        </div>

    </div>
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblTitle" runat="server" Text="Service Group"></asp:Label>
        </h1>
        <div class="content">
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdSvcGroup" AllowPaging="false" AutoGenerateColumns="False" AllowFilteringByColumn="false"
                    AllowSorting="True" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false"
                    OnNeedDataSource="grdSvcGroup_NeedDataSource" OnItemCommand="grdSvcGroup_ItemCommand"
                    OnItemDataBound="grdSvcGroup_ItemDataBound">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="true">
                    </ExportSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="BSG_ID">
                        <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add new Package" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="BSG_Name" FilterControlAltText="Filter PackageName column"
                                HeaderText="Service Group Name" SortExpression="BSG_Name" UniqueName="BSG_Name">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BSG_Description" FilterControlAltText="Filter Description column"
                                HeaderText="Description" SortExpression="BSG_Description" UniqueName="BSG_Description">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BkgPackageSvcGroup.BPSG_IsFirstReviewTrigger" FilterControlAltText="Filter IsFirstReviewTrigger column"
                                HeaderText="Is First Review Trigger" SortExpression="BkgPackageSvcGroup.BPSG_IsFirstReviewTrigger" UniqueName="BPSG_IsFirstReviewTrigger"
                                DataType="System.Boolean">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="BkgPackageSvcGroup.BPSG_IsSecondReviewTrigger" FilterControlAltText="Filter BPSG_IsSecondReviewTrigger column"
                                HeaderText="Is Second Review Trigger" SortExpression="BkgPackageSvcGroup.BPSG_IsSecondReviewTrigger" UniqueName="BPSG_IsSecondReviewTrigger"
                                DataType="System.Boolean">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="BSG_Active" FilterControlAltText="Filter IsActive column"
                                HeaderText="Is Active" SortExpression="BSG_Active" UniqueName="BSG_Active">
                                <ItemTemplate>
                                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("BSG_Active"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
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
    <hr style="border-bottom: solid 1px #c0c0c0;">
    <div style="right: 20px;">
        <infsu:CommandBar ButtonPosition="Right" ID="cmdBarStateSearch" runat="server" DisplayButtons="Extra,Save" SaveButtonText="Edit"
            OnSaveClick="cmdBarStateSearch_SaveClick" ExtraButtonText="Update from Master" OnExtraClick="cmdBarStateSearch_ExtraClick"
            SaveButtonIconClass="rbEdit" AutoPostbackButtons="Extra,Save">
        </infsu:CommandBar>
    </div>
    <div class="section">
        <div id="divPkgDetailMhdr" class="mhdr" style="position: relative; bottom: 2px;">
            <h1 style="font-size: 14px; padding-bottom: 2px;">State Search Criteria</h1>
        </div>

        <%-- <div class="content">
            <div class="swrap">
                <!--TO-DO-->
                <div class="sxform auto">
                    <asp:Repeater ID="rptStateCounty" runat="server" OnItemDataBound="rptStateCounty_ItemDataBound" OnItemCommand="rptStateCounty_ItemCommand">
                        <HeaderTemplate>
                            <h4 style="padding-left: 8px">State Name</h4>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="content" style="margin-bottom: 0px !important;" id="#dvRptContent">
                                <div class="sxform auto">
                                    <asp:Panel ID="pnlInternal" CssClass="sxpnl" runat="server">
                                        <div class='sxro sx1co' style="margin: 0px !important">
                                            <div class="sxlb">
                                                <asp:Label ID="lblStateName" Text='<%#Eval("StateName")%>' runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnStateID" Value='<%# Eval("StateID") %>' runat="server" />
                                                <asp:Label ID="Label1" Text='<%#"(" + Eval("StateAbbreviation") + ")"%>' runat="server"></asp:Label>
                                            </div>
                                            <div class="sxlm" style="margin: 0px !important; padding: 0px !important; height: 25px;">
                                                <asp:Label ID="lblStateSearch" Text="State Search" runat="server"></asp:Label>
                                                <asp:CheckBox ID="chkStateSearch" runat="server" Enabled="false" />
                                                &nbsp;&nbsp;
                                            <asp:Label ID="lblCountySearch" Text="County Search" runat="server"></asp:Label>
                                                <asp:CheckBox ID="chkCountySearch" runat="server" Enabled="false" Checked="false" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="sxcbar">
                    <div class="sxcmds" style="text-align: right">
                        <infs:WclButton ID="btnSaveStateSearchCriteria" runat="server" Visible="false" Text="Save" OnClick="btnSaveStateSearchCriteria_Click">
                            <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                PrimaryIconWidth="14" />
                        </infs:WclButton>
                        <infs:WclButton ID="btnCancelStateSearchCriteria" runat="server" Visible="false" Text="Cancel" OnClick="btnCancelStateSearchCriteria_Click">
                            <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                PrimaryIconWidth="14" />
                        </infs:WclButton>
                    </div>
                </div>
            </div>
        </div>--%>
        <div class="content">
            <div class="swrap">
                <div id="divStateCounty" class="sxform auto" style="padding-top: 5px;">
                    <asp:Repeater ID="rptStateCounty" runat="server" OnItemDataBound="rptStateCounty_ItemDataBound" OnItemCommand="rptStateCounty_ItemCommand">
                        <HeaderTemplate>
                            <table id="tblStateCounty">
                                <tr>
                                    <th class="thState" style="padding-bottom: 10px !important; background-color: #808080">State Name</th>
                                    <th class="thStateAbb" style="padding-bottom: 10px !important; background-color: #808080">State Abbreviation</th>
                                    <th class="allStateCheckbox" style="padding-right: 40px; padding-bottom: 10px; font-weight: bold; font-size: 13px; background-color: #808080">
                                        <asp:CheckBox ID="chkAllState" Text="Select All" runat="server" Enabled="false" onclick="checkAll('stateSearch')" />
                                    </th>
                                    <th class="allCountyCheckbox" style="padding-bottom: 10px; font-weight: bold; font-size: 13px; background-color: #808080">
                                        <asp:CheckBox ID="chkAllCounty" Text="Select All" runat="server" Enabled="false" onclick="checkAll('countySearch')" />
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="tdState">
                                    <asp:Label ID="lblStateName" Text='<%#Eval("StateName")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnStateID" Value='<%# Eval("StateID") %>' runat="server" />
                                </td>
                                <td class="tdStateAbb">
                                    <asp:Label ID="lblStateAbbreviation" Text='<%#"(" + Eval("StateAbbreviation") + ")"%>' runat="server"></asp:Label>
                                </td>
                                <td class="stateCheckbox">
                                    <asp:CheckBox ID="chkStateSearch" runat="server" Enabled="false" Text="State Search" onclick="UnCheckHeader(this)" />
                                </td>
                                <td class="countyCheckbox">
                                    <asp:CheckBox ID="chkCountySearch" runat="server" Enabled="false" Text="County Search" onclick="UnCheckHeader(this)" />
                                </td>
                            </tr>
                            <tr style="height: 3px !important;">
                                <td colspan="4"></td>
                            </tr>
                            <%--<div class="content" style="margin-bottom: 0px !important;" id="#dvRptContent">
                            <div class="sxform auto">
                                <asp:Panel ID="pnlInternal" CssClass="sxpnl" runat="server">
                                    <div class='sxro sx1co' style="margin: 0px !important">
                                        <div class="sxlb">
                                            <asp:Label ID="lblStateName" Text='<%#Eval("StateName")%>' runat="server"></asp:Label>
                                            <asp:HiddenField ID="hdnStateID" Value='<%# Eval("StateID") %>' runat="server" />
                                            <asp:Label ID="Label1" Text='<%#"(" + Eval("StateAbbreviation") + ")"%>' runat="server"></asp:Label>
                                        </div>
                                        <div class="sxlm" style="margin: 0px !important; padding: 0px !important; height: 25px;">
                                            <asp:Label ID="lblStateSearch" Text="State Search" runat="server"></asp:Label>
                                            <ins:checkbox id="chkStateSearch" runat="server" enabled="false" />
                                            &nbsp;&nbsp;
                                            <asp:Label ID="lblCountySearch" Text="County Search" runat="server"></asp:Label>
                                            <asp:CheckBox ID="chkCountySearch" runat="server" Enabled="false" Checked="false" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>--%>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div class="sxcbar">
                    <div class="sxcmds" style="text-align: center">
                        <infs:WclButton ID="btnSaveStateSearchCriteria" runat="server" Visible="false" Text="Save" OnClick="btnSaveStateSearchCriteria_Click">
                            <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                PrimaryIconWidth="14" />
                        </infs:WclButton>
                        <infs:WclButton ID="btnCancelStateSearchCriteria" runat="server" Visible="false" Text="Cancel" OnClick="btnCancelStateSearchCriteria_Click">
                            <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                                PrimaryIconWidth="14" />
                        </infs:WclButton>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <script type="text/javascript">
        function OnClientLoad(editor, args) {
            $jQuery('ul.reToolbar').width('auto');
        }

        //Function to check alll checkboxes if header is checked
        function checkAll(searchType) {
            //debugger;
            var allCheckbox;
            var checkboxes;
            if (searchType == "stateSearch") {
                allCheckbox = $jQuery('.allStateCheckbox :checkbox');
                checkboxes = $jQuery('.stateCheckbox :checkbox');
            }
            else {
                allCheckbox = $jQuery('.allCountyCheckbox :checkbox');
                checkboxes = $jQuery('.countyCheckbox :checkbox');
            }
            if (allCheckbox[0].checked) {
                for (var i = 0; i < checkboxes.length; i++) {
                    if (!checkboxes[i].checked) {
                        checkboxes[i].checked = true;
                    }
                }
            }
            else {
                for (var i = 0; i < checkboxes.length ; i++) {
                    if (checkboxes[i].checked) {
                        checkboxes[i].checked = false;
                    }
                }
            }
        };

        //Function to uncheck header if any checkbox is unchecked and also rechecked header if again all checkboxes are checked
        function UnCheckHeader(sender) {
            //debugger;
            var btnID = sender.id;
            var allCheckbox;
            var searchType;
            if (btnID.indexOf("StateSearch") > 0) {
                allCheckbox = $jQuery('.allStateCheckbox :checkbox');
                searchType = "StateSearch";
            }
            else {
                allCheckbox = $jQuery('.allCountyCheckbox :checkbox');
            }
            if (!$jQuery('#' + sender.id)[0].checked) {
                allCheckbox[0].checked = false;
                searchType = "CountySearch";
            }
            else {
                CheckHeader(searchType);
            }
        }

        //Page-load function 
        function pageLoad() {
            //debugger;
            CheckHeader('StateSearch');
            CheckHeader('CountySearch');
        }

        //Function to check header checkbox
        function CheckHeader(searchType) {
            var allCheckbox;
            var checkboxes;
            var isUncheckFound = false;
            if (searchType == "StateSearch") {
                allCheckbox = $jQuery('.allStateCheckbox :checkbox');
                checkboxes = $jQuery('.stateCheckbox :checkbox');
            }
            else {
                allCheckbox = $jQuery('.allCountyCheckbox :checkbox');
                checkboxes = $jQuery('.countyCheckbox :checkbox');
            }
            for (var i = 0; i < checkboxes.length; i++) {
                if (!checkboxes[i].checked) {
                    isUncheckFound = true;
                    break;
                }
            }
            if (!isUncheckFound) {
                allCheckbox[0].checked = true;
            }
        }
    </script>

</asp:Content>
