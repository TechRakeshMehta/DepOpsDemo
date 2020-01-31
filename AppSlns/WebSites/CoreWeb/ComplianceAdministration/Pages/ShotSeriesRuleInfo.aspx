<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.ShotSeriesRuleInfo"
    Title="RuleInfo" MasterPageFile="~/Shared/ChildPage.master" CodeBehind="ShotSeriesRuleInfo.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="ExpressionObject" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/ShotSeriesExpressionObject.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
            // ChkUserGrpIsRequired();
        });

        function RefrshTree(data) {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }
        function OnCancelClientClick(sender, eventArgs) {
            $jQuery("[id$=hdnIsCancelRequest]")[0].value = true;
        }

        function ChkUserGrpIsRequired() {
            var isVisible;
            if ($jQuery("[id$=cmbUserGroup]") != undefined && $jQuery("[id$=cmbUserGroup]").length != 0) {
                isVisible = true;
            }
            else {
                isVisible = false;
            }

            if (isVisible) {
                $jQuery("[id$=rfvUserGroup]")[0].enabled = true;
                $jQuery("[id$=dvUserGroupVldx]").show();
            }
        }
    </script>
    <div class="page_cmd">
        &nbsp;
    </div>
    <div class="section">
        <h1 class="mhdr">Rule Information</h1>
        <div class="content">
            <div class="sxform ">
                <div class="msgbox" id="divSuccessMsg">
                    <asp:Label ID="lblSuccess" runat="server" Visible="false"></asp:Label>
                </div>
                <asp:HiddenField ID="hdnObjectCount" runat="server" Value="0" />
                <asp:HiddenField ID="hdnIsCancelRequest" runat="server" Value="" />
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlRuleInfo">
                    <div class="sxgrp auto" id="divSelect" visible="true">

                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Apply Rule On</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="ddlObjectType" runat="server" AutoPostBack="true" ToolTip="Select from given list"
                                    OnSelectedIndexChanged="ddlObjectType_SelectedIndexChanged">
                                    <Items>
                                    </Items>
                                </infs:WclComboBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlObjectType"
                                        ID="rfvDdlObjectType" class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic"
                                        ErrorMessage="Please select object type." InitialValue="--SELECT--" />
                                </div>
                            </div>
                            <div id="dvSeriesItem" runat="server" visible="false">
                                <div class='sxlb'>
                                    <span class="cptn">Series Items</span><span class="reqd">*</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclComboBox ID="cmbSeriesItems" runat="server" EmptyMessage="--SELECT--" CheckBoxes="true" Height="100px">
                                    </infs:WclComboBox>

                                    <div class="vldx">
                                        <asp:RequiredFieldValidator ID="rvSeriesItems" runat="server" ControlToValidate="cmbSeriesItems"
                                            class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Please select Series Item(s)">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Select a Template</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbMasterTemplates" runat="server" AutoPostBack="true" ToolTip="Select from a master list"
                                    OnSelectedIndexChanged="cmbMasterTemplates_SelectedIndexChanged">
                                </infs:WclComboBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="cmbMasterTemplates"
                                        ID="rfvTemplates" class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic"
                                        ErrorMessage="Please select template." InitialValue="--SELECT--" />
                                </div>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Rule Name</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtRuleName" MaxLength="100">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtRuleName"
                                        class="errmsg" ValidationGroup="grpFormSubmit" Display="Dynamic" ErrorMessage="Rule Name is required." />
                                </div>
                            </div>
                            <div class='sxlb'>
                                <span class="cptn">Rule Action</span>
                            </div>
                            <div class='sxlm'>
                                <div class='ronly'>
                                    <asp:Label ID="lblAction" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class='sxlb'>
                                <asp:Label Text="Select Rule" runat="server" ID="lblActionMapping" CssClass="cptn" />
                            </div>
                            <div class='sxlm'>
                                <infs:WclTextBox runat="server" ID="txtActionMapping">
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
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Specify Error Message</span>
                            </div>
                            <div class='sxlm m3spn'>
                                <infs:WclTextBox runat="server" ID="txtErrorMessage">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxro sx3co'>
                            <div class='sxlb'>
                                <span class="cptn">Specify Sucess Message</span>
                            </div>
                            <div class='sxlm m3spn'>
                                <infs:WclTextBox runat="server" ID="txtSucessMessage">
                                </infs:WclTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="sxgrp" id="divCreate" visible="true">
                        <div id="divCateogry">
                            <%--Conditional Category Rule Demonstration --%>
                            <div class="sxro">
                                <h1 class="shdr">Rule Expressions</h1>
                                <pre>
                                            <asp:Literal ID="litExpression" runat="server"></asp:Literal>
                                    </pre>
                            </div>
                            <h1 class="shdr">Map Expression Objects</h1>
                            <asp:Panel ID="pnlExpressionObjects" runat="server">
                            </asp:Panel>
                        </div>
                    </div>
                    <div style="padding: 5px; text-align: right">
                        <infs:WclButton runat="server" ID="btnValidate" Text="Validate Rule" OnClick="btnValidate_Click">
                        </infs:WclButton>
                    </div>
                    <div class='sxro sx3co monly auto'>
                        <div class='sxlb'>
                            <span class="cptn">Validation Result</span>
                        </div>
                        <infs:WclTextBox runat="server" ID="txtValidationResult" Height="50px" ReadOnly="true"
                            TextMode="MultiLine">
                        </infs:WclTextBox>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div style="padding: 5px; text-align: right">
                        <infs:WclDropDownList ID="ddlTestUsersList" runat="server" Enabled="false" DefaultMessage="Select Test Users">
                            <Items>
                                <telerik:DropDownListItem Text="Test User1" Value="1" />
                                <telerik:DropDownListItem Text="Test User2" Value="2" />
                            </Items>
                        </infs:WclDropDownList>
                        <infs:WclButton runat="server" ID="btn" Text="Test Rule" Enabled="false">
                        </infs:WclButton>
                    </div>
                    <div class='sxro sx3co monly auto'>
                        <div class='sxlb'>
                            <span class="cptn">Test Result</span>
                        </div>
                        <infs:WclTextBox runat="server" ID="txtTestResult" Height="50px" ReadOnly="true"
                            TextMode="MultiLine">
                        </infs:WclTextBox>
                        <div class='sxroend'>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <infsu:CommandBar ID="fsucCmdBarRule" runat="server"
                DefaultPanel="pnlRuleInfo" AutoPostbackButtons="Submit, Save, Cancel" SubmitButtonIconClass="rbEdit"
                CauseValidationOnCancel="false" SubmitButtonText="Edit" SaveButtonText="Save"
                OnSubmitClick="fsucCmdBarRule_SubmitClick" OnSaveClick="fsucCmdBarRule_SaveClick"
                OnCancelClick="fsucCmdBarRule_CancelClick" OnCancelClientClick="OnCancelClientClick" />
        </div>
    </div>
</asp:Content>


