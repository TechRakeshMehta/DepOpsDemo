<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Messaging.Views.ManageTemplate"
    Title="Manage Template" MasterPageFile="~/Shared/PopupMaster.master" Codebehind="ManageTemplate.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="content" ContentPlaceHolderID="PoupContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="prxMP">
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/writemessage.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <div style="padding: 20px; background-color: #C5E2F8;">
        <span class="cptn">Template</span>
        <infs:WclComboBox MarkFirstMatch="true" DataTextField="TemplateName" DataValueField="ADBMessageId"
            ID="cmbTemplates" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbTemplates_SelectedIndexChanged">
        </infs:WclComboBox>
        &nbsp;&nbsp;&nbsp;
        <infs:WclButton runat="server" ID="btnDelete" Text="Remove" OnClick="btnDelete_Click">
            <Icon PrimaryIconCssClass="rbCancel" />
        </infs:WclButton>
    </div>
    <infs:WclResourceManagerProxy runat="server" ID="rprxName1">
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/ManageTemplate.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/Reply.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/reply.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Generic/popup.min.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <div class="section">
        <div class="content" style="overflow: visible">
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                    <asp:HiddenField runat="server" ID="hdnIsUserGroupforCompany" />
                    <asp:HiddenField runat="server" ID="hdnToListIds" />
                    <asp:HiddenField runat="server" ID="hdnCcIsUserGroupforCompany" />
                    <asp:HiddenField runat="server" ID="hdnCcListIds" />
                    <div class='sxro sx2co'>
                        <div class="sxlb">
                            <asp:Label ID="lblTemplateName" runat="server" Text="Template Name" CssClass="cptn" /><span class="reqd">*</span></div>
                        <div class="sxlm m2spn">
                            <infs:WclTextBox runat="server" ID="txtTemplateName" ClientKey="TemplateName">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx2co'>
                        <div class="sxlb">
                            <infs:WclButton runat="server" ID="btnTo" Text="To.." ButtonType="LinkButton" AutoPostBack="false"
                                OnClientClicked="e_showaddresslist">
                            </infs:WclButton>
                        </div>
                        <div class="sxlm m2spn">
                            <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" Width="410px" ID="acbToList"
                                InputType="Token">
                                <TokensSettings AllowTokenEditing="true" />
                            </infs:WclAutoCompleteBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx2co'>
                        <div class="sxlb">
                            <infs:WclButton runat="server" ID="btnCc" Text="CC.." ButtonType="LinkButton" AutoPostBack="false"
                                OnClientClicked="e_showaddresslist">
                            </infs:WclButton>
                        </div>
                        <div class="sxlm m2spn">
                            <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" Width="410px" ID="acbCcList"
                                InputType="Token">
                                <TokensSettings AllowTokenEditing="true" />
                            </infs:WclAutoCompleteBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx2co'>
                        <div class="sxlb">
                            <infs:WclButton runat="server" ID="btnBcc" Text="BCC.." ButtonType="LinkButton" AutoPostBack="false"
                                OnClientClicked="e_showaddresslist" >
                            </infs:WclButton>
                       </div>
                        <div class="sxlm m2spn">
                            <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" Width="410px" ID="acbBccList"
                                ClientIDMode="Static" InputType="Token">
                                <TokensSettings AllowTokenEditing="true" />
                            </infs:WclAutoCompleteBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx2co'>
                        <div class="sxlb">
                            <asp:Label ID="lblSubject" runat="server" Text="Subject" CssClass="cptn"/><span class="reqd">*</span></div>
                        <div class="sxlm m2spn">
                            <infs:WclTextBox Width="100%" ClientKey="TemplateSubject" ID="txtSubject" runat="server"
                                CssClass="HideArrow">
                            </infs:WclTextBox>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <div class="sxlb">
                            <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="cptn" /><span class="reqd">*</span>
                        </div>
                    </div>
                    <div>
                        <infs:WclEditor ID="editorContent" runat="server" Width="100%" EditModes="Design"
                            ToolsFile="~/Messaging/Data/tools.xml" ClientKey="templateBody" Height="330">
                            <Content>
                               <body style="background-color:White;"></body>
                            </Content>
                        </infs:WclEditor>
                    </div>
                </asp:Panel>
            </div>
            <asp:HiddenField ID="txtHighImportance" runat="server" Value="false" />
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="Server">
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel"
        SaveButtonText="Save" CancelButtonText="Cancel" OnCancelClientClick="returnToParent"
        DefaultPanel="pnlName1" OnSaveClick="btnSaveMessage_Click" OnSaveClientClick="SaveTemplateClientClick"
        ButtonPosition="Center" ClientIDMode="Static" />
</asp:Content>
