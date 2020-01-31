<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.PackageInfo"
    Title="PackageInfo" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="PackageInfo.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/CategoryListing.ascx" TagPrefix="uc"
    TagName="CategoryList" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });
        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
    </script>

    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <div class="page_cmd">&nbsp;</div>
    <div class="section">
        <h1 class="mhdr">Package Information</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPackage">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Package Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtPackageName" MaxLength="100">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvPackageName" ControlToValidate="txtPackageName"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Package Name is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Package Label</span><%--<span class="reqd">*</span>--%>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtPackageLabel" MaxLength="100">
                            </infs:WclTextBox>
                            <%--<div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rFvPackageLabel" ControlToValidate="txtPackageLabel"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Package Label is required." />
                            </div>--%>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Screen Label</span><%--<span class="reqd">*</span>--%>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtScreenLabel" MaxLength="100">
                            </infs:WclTextBox>
                            <%--  <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvScreenLabel" ControlToValidate="txtScreenLabel"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Screen Label is required." />
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
                            <span class="cptn">Description</span>
                        </div>
                        <div class="sxlm m2spn">
                            <infs:WclTextBox runat="server" ID="txtPkgDescription" MaxLength="250">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
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
                    <div id="dvMappingHierarchy" runat="server" style="display:none" class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Mapped Hierarchy</span>
                        </div>
                        <div class="sxlm m3spn">
                            <asp:Label ID="lblMappedHierarchy" runat="server"></asp:Label>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div id="dvBundleMappingHierarchy" runat="server" style="display:none" class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Bundle Mapped Hierarchy</span>
                        </div>
                        <div class="sxlm m3spn">
                            <asp:Label ID="lblBundleMappedHierarchy" runat="server"></asp:Label>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <telerik:RadButton ID="btnCopyPkgData" ToolTip="If there are any active rotation subscription then data will be copied from tracking package to rotation package."
                            runat="server" Text="Copy Package Data to Rotation" Visible="true" UseSubmitBehavior="false" OnClick="btnCopyPkgData_Click">
                        </telerik:RadButton>
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
                                    class="errmsg" Display="Dynamic" ErrorMessage="Please don't enter more than 1000 characters." />
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
                </asp:Panel>
            </div>
            <infsu:CommandBar ID="fsucCmdBarPackage" runat="server" DefaultPanel="pnlPackage"
                SubmitButtonIconClass="rbEdit" OnSaveClick="fsucCmdBarPackage_SaveClick" SubmitButtonText="Edit"
                SaveButtonText="Save" OnSubmitClick="fsucCmdBarPackage_SubmitClick" OnCancelClick="fsucCmdBarPackage_CancelClick"
                AutoPostbackButtons="Save,Submit,cancel">
            </infsu:CommandBar>
        </div>
    </div>
    <div>
        <uc:CategoryList runat="server" ID="grdCategorylist" />
    </div>
</asp:Content>
