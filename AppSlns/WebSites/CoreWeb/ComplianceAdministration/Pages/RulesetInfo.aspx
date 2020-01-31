<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.RulesetInfo"
    Title="RulesetInfo" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="RulesetInfo.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemsListng" Src="~/ComplianceAdministration/UserControl/ItemsListing.ascx" %>
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
    <div class="page_cmd">&nbsp;</div>
    <div class="section">
        <h1 class="mhdr">Ruleset Information</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="msgbox">
                    <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                </div>
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlRulesets">
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class="cptn">Ruleset Name</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtName" MaxLength="100">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvRuleSetName" ControlToValidate="txtName"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Ruleset Name is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Is Active</span>
                        </div>
                        <div class='sxlm'>
                            <%--  <infs:WclButton runat="server" ID="chkActive" ToggleType="CheckBox" ButtonType="ToggleButton"
                                AutoPostBack="false">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                    <telerik:RadButtonToggleState Text="No" Value="False" />
                                </ToggleStates>
                            </infs:WclButton>--%>
                            <uc1:IsActiveToggle runat="server" ID="chkActive" IsActiveEnable="true" IsAutoPostBack="false" />
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
                </asp:Panel>
            </div>
            <infsu:CommandBar ID="fsucCmdBarRuleInfo" runat="server" DefaultPanel="pnlRulesets"
                SubmitButtonIconClass="rbEdit" OnSaveClick="fsucCmdBarRuleInfo_SaveClick" SubmitButtonText="Edit"
                SaveButtonText="Save" OnSubmitClick="fsucCmdBarRuleInfo_SubmitClick" OnCancelClick="fsucCmdBarRuleInfo_CancelClick"
                AutoPostbackButtons="Save,Submit,Cancel">
            </infsu:CommandBar>
        </div>
    </div>
    <%-- <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblTitle" runat="server" Text="Items"></asp:Label>
        </h1>
        <infsu:ItemsListng ID="ItemsListing" runat="server"></infsu:ItemsListng>
    </div>--%>
</asp:Content>
