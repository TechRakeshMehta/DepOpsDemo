<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleInfoBkg.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.RuleInfoBkg" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="RuleExpressionObject" TagPrefix="infsu" Src="~/BkgSetup/UserControl/BkgRuleExpressionObject.ascx" %>

<%@ Register Src="~/Shared/Controls/IsActiveToggle.ascx" TagName="IsActiveToggle"
    TagPrefix="uc1" %>

<div class="section">
    <h1 class="mhdr">Rule Information</h1>
    <div class="content">
        <div class="sxform ">
            <asp:HiddenField ID="hdnObjectCount" runat="server" Value="0" />
            <asp:HiddenField ID="hdnIsCancelRequest" runat="server" Value="" />
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlRuleInfo">
                <div class="sxgrp auto" id="divSelect" visible="true">
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
                                    ID="rfvTemplates" class="errmsg" ValidationGroup="grpSubmit" Display="Dynamic"
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
                                    class="errmsg" ValidationGroup="grpSubmit" Display="Dynamic" ErrorMessage="Rule Name is required." />
                            </div>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Action Type</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclDropDownList ID="ddlActionType" runat="server" DataTextField="BACT_Description"
                                DataValueField="BACT_ID">
                            </infs:WclDropDownList>
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Rule Type</span>
                        </div>
                        <div class='sxlm'>
                            <div class='ronly'>
                                <infs:WclDropDownList ID="ddlRuleType" runat="server" DataTextField="BRLT_Description"
                                    DataValueField="BRLT_ID">
                                </infs:WclDropDownList>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <asp:Label Text="Select Rule" runat="server" ID="lblActionMapping" CssClass="cptn" />
                        </div>
                        <div class='sxlm'>
                            <infs:WclTextBox runat="server" ID="txtActionMapping">
                            </infs:WclTextBox>
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
                            <asp:Literal ID="litExpression" runat="server">
                            </asp:Literal>
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
            </asp:Panel>
        </div>

        <infsu:CommandBar ID="cmdBarRule" runat="server"
            DefaultPanel="pnlRuleInfo" AutoPostbackButtons="Save,Submit,Cancel" SubmitButtonText="Edit" SubmitButtonIconClass="rbEdit"
            OnSaveClick="cmdBarRule_SaveClick" OnCancelClick="cmdBarRule_CancelClick" OnSubmitClick="cmdBarRule_SubmitClick"
            CauseValidationOnCancel="false" OnCancelClientClick="OnCancelClientClick" />
    </div>
</div>
