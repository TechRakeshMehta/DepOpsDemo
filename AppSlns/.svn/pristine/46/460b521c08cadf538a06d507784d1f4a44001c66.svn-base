<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Messaging.Views.WriteMessage"
    Title="" MasterPageFile="~/Shared/PopupMaster.master" CodeBehind="WriteMessage.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
    <style type="text/css">
        #PoupContent_rdlCommunicationMode td {
            padding-right: 10px;
        }

        .clsVisibility
        {
            padding-left : 22px !important;
        }

    </style>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="~/Resources/Mod/Accessibility/Main-Accessibility.js"></script>

    <script>

        $(document).ready(function () {
            $jQuery('.ruFileInput').attr('tabindex', '0');
            $jQuery('.reEditorModes a.reMode_preview').keydown(function (e) {
                if (e.keyCode == 9 && !e.shiftKey) {
                    e.preventDefault();
                    $jQuery("[id$=<%=WclToolBar1.ClientID %>]").attr('tabindex', '0').focus();
                }
            })
            $jQuery('.rwTitlebarControls a.rwMaximizeButton').keydown(function (e) {

                if (e.shiftKey && e.keyCode == tabKey) {
                    e.preventDefault();
                    $jQuery('.reEditorModes a.reMode_preview').focus();
                }
            }
            )
         

            $jQuery("input[type='radio']").each(function (i, e) {
                var newLblId = "lblForRbn_" + i;
                var rbnOptionText = $jQuery(this).siblings().attr('id', newLblId);
                $jQuery(this).attr('aria-labelledBy', "lblCommunicationMode " + newLblId);
            });
            //UAT-2364
            var pheight = $jQuery("[id$=WclPane3]").height() - 20;
            $jQuery("[id$=WclPane3]").find(".RadEditor").first().height(pheight);
            $jQuery("[id$=WclPane3]").find(".RadEditor table").first().height(pheight);
        });
      
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="prxMP">
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/writemessage.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/writemessage.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <infs:WclSplitter ID="WclSplitter1" runat="server" LiveResize="true" Orientation="Horizontal"
        Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
        <infs:WclPane ID="WclPane1" runat="server" MinHeight="32" Height="32">
            <infs:WclToolBar ID="WclToolBar1" runat="server" Width="100%" OnClientButtonClicking="btntoolbar_clicked" TabIndex="0">
                <Items>
                    <telerik:RadToolBarButton Text="Send" ImageUrl="~/Resources/Mod/Messaging/Icons/sendmessage.png"
                        CommandName="Send">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton Text="Save" ImageUrl="~/Resources/Mod/Messaging/Icons/save.png"
                        CommandName="SaveMessage">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton IsSeparator="true">
                    </telerik:RadToolBarButton>
                    <%--<telerik:RadToolBarButton Text="Check Names" ImageUrl="~/Resources/Mod/Messaging/Icons/checknames.png">
                    </telerik:RadToolBarButton>--%>
                    <telerik:RadToolBarButton Text="High Importance" ImageUrl="~/Resources/Mod/Messaging/Icons/important.png"
                        CheckOnClick="true" CommandName="HighImportance">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton Text="Attach File" ImageUrl="~/Resources/Mod/Messaging/Icons/attachment.png"
                        Visible="false">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton Text="Cancel" ImageUrl="~/Resources/Mod/Compliance/icons/no16.png"
                        CommandName="cancel">
                    </telerik:RadToolBarButton>
                </Items>
            </infs:WclToolBar>
        </infs:WclPane>
        <infs:WclPane ID="WclPane2" runat="server" MaxHeight="225">
            <asp:Panel ID="Panel1" runat="server" CssClass="m_wmform no-adjust">
                <table class="m_wmform">
                    <tr id="trTemplates" runat="server">
                        <td class="m_wmlb">
                            <%--<asp:Label ID="lblTemplate" runat="server" Text="Template" AssociatedControlID="cmbTemplates" CssClass="cptn"></asp:Label>--%>
                            <label for="<%= cmbTemplates.ClientID %>_Input" class="cptn" id="lblTemplate">Template</label>
                        </td>
                        <td class="m_wmlm">
                            <infs:WclComboBox ID="cmbTemplates" AutoPostBack="true" DataTextField="TemplateName" EnableAriaSupport="true"
                                DataValueField="ADBMessageID" runat="server" OnSelectedIndexChanged="cmbTemplates_SelectedIndexChanged">
                            </infs:WclComboBox>
                        </td>
                    </tr>
                    <tr id="trCommunicationSelection" runat="server">
                        <td class="m_wmlb">
                            <%--<asp:Label runat="server" Text="Mode" AssociatedControlID="rdlCommunicationMode" CssClass="cptn"></asp:Label>--%>
                            <label id="lblCommunicationMode" class="cptn">Mode</label>
                        </td>
                        <td class="m_wmlm">
                            <asp:RadioButtonList ID="rdlCommunicationMode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" TabIndex="0"
                                OnSelectedIndexChanged="rdlCommunicationMode_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="1" Text="Communication Center"></asp:ListItem>
                                <asp:ListItem Value="2" Text="E-Mail"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="m_wmlb">
                            <infs:WclButton runat="server" ID="btnTo" Text="To.." ButtonType="LinkButton" AutoPostBack="false"
                                OnClientClicked="e_showaddresslist">
                            </infs:WclButton>
                        </td>
                        <td class="m_wmlm">
                            <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" Width="100%" ID="acbToList"
                                ClientIDMode="Static" InputType="Token">
                                <TokensSettings AllowTokenEditing="true" />
                            </infs:WclAutoCompleteBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="m_wmlb">
                            <infs:WclButton runat="server" ID="btnCc" Text="CC.." ButtonType="LinkButton" AutoPostBack="false"
                                OnClientClicked="e_showaddresslist">
                            </infs:WclButton>
                        </td>
                        <td class="m_wmlm">
                            <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" Width="100%" ID="acbCcList"
                                ClientIDMode="Static" InputType="Token">
                                <TokensSettings AllowTokenEditing="true" />
                            </infs:WclAutoCompleteBox>
                        </td>
                    </tr>
                    <tr id="dvBccusers" runat="server">
                        <td class="m_wmlb">
                            <infs:WclButton runat="server" ID="btnBcc" Text="BCC.." ButtonType="LinkButton" AutoPostBack="false"
                                OnClientClicked="e_showaddresslist">
                            </infs:WclButton>
                        </td>
                        <td class="m_wmlm">
                            <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" Width="100%" ID="acbBccList"
                                ClientIDMode="Static" InputType="Token">
                                <TokensSettings AllowTokenEditing="true" />
                            </infs:WclAutoCompleteBox>
                        </td>
                    </tr>
                    <tr id="dvIsCopyOfMailToSender" runat="server" style="width: 100%">
                        <td class="m_wmlb"></td>
                        <td class="m_wmlm">
                            <asp:CheckBox ID="chkIsCopyOfMailToSender" runat="server" Text="Send me a copy of this email" />
                        </td>
                    </tr>
                    <tr>
                        <td class="m_wmlb">
                            <%--<span class="cptn">Subject</span><span class="reqd">*</span>--%>
                            <%--                            <asp:Label ID="lblSubject" runat="server" Text="Subject" AssociatedControlID="txtSubject" CssClass="cptn"></asp:Label><span class="reqd">*</span>--%>
                            <label for="<%=txtSubject.ClientID%>" class="cptn">Subject</label><span class="reqd">*</span>
                        </td>
                        <td class="m_wmlm">
                            <infs:WclTextBox ID="txtSubject" runat="server" onkeydown="textBoxKeyDown();">
                            </infs:WclTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="m_wmlb">
                            <%--<span class="cptn">Attach file</span>--%>
                            <%--<asp:Label ID="lblfupDoc" runat="server" Text="Attach file" AssociatedControlID="fupDocument" CssClass="cptn"></asp:Label>--%>
                            <label for="<%=fupDocument.ClientID %>" class="cptn">Attach File</label>
                        </td>
                        <td class="m_wmlm">
                            <infs:WclAsyncUpload ID="fupDocument" runat="server" ControlObjectsVisibility="ClearButtons" DisablePlugins="true" TabIndex="0"
                                OnClientFileUploadRemoved="onFileRemoved" OnClientFileUploaded="onFileUploaded"
                                Width="70%" MultipleFileSelection="Automatic" AllowedFileExtensions=".txt,.pdf,.doc,.docx,.xls,.xlsx,.zip,.jpg,.png,.bmp,.gif,.TXT,.PDF,.DOC,.DOCX,.XLS,.XLSX,.ZIP,.JPG,.PNG,.BMP,.GIF"
                                OnClientValidationFailed="Validationfailed"
                                ClientKey="fupDocument">
                            </infs:WclAsyncUpload>
                        </td>
                    </tr>
                    <tr id="trAttachFiles" runat="server">
                        <td class="m_wmlb"></td>
                        <td class="m_wmlm" style="float: left;">
                            <infsu:CommandBar ID="comandSaveAtachments" runat="server" DisplayButtons="Submit"
                                AutoPostbackButtons="Submit" SubmitButtonText="Attach Files" OnSubmitClick="comandSaveAtachments_SubmitClick" />
                        </td>
                    </tr>
                    <tr id="trAttachemnets" runat="server">
                        <td class="m_wmlb">
                            <%-- <span class="cptn">Attachment</span>--%>
                            <label for="<%=acbAttachedFiles.ClientID %>" class="cptn">Attach File</label>
                        </td>
                        <td class="m_wmlm">
                            <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" Width="100%" ID="acbAttachedFiles"
                                ClientIDMode="Static" InputType="Token">
                                <TokensSettings AllowTokenEditing="true" />
                            </infs:WclAutoCompleteBox>
                        </td>
                    </tr>
                    <%--comented to solve bug of description.-- %>
                    <caption>
                        &gt;
                        <%--<tr>
                        <td class="m_wmlb">
                            Description<span class="reqd">*</span>
                        </td>
                    </tr>
                    </caption>--%>
                </table>
            </asp:Panel>
        </infs:WclPane>
        <infs:WclPane ID="WclPane3" runat="server" Scrolling="None" OnClientResized="e_editorpaneresize">
            <div class="m_wmedtwrap ">
                <infs:WclEditor ID="editorContent" runat="server" NewLineMode="Div" Width="100%" OnClientLoad="OnClientLoad"
                    ToolsFile="~/Messaging/Data/tools.xml" EnableAriaSupport="true">
                    <Content>
                    <body style="background-color:White;"></body>
                <div style="margin:0px;"></div>
                    </Content>
                </infs:WclEditor>
            </div>
        </infs:WclPane>
    </infs:WclSplitter>
    <asp:HiddenField ID="hdnMessageImportance" runat="server" Value="false" />
    <asp:HiddenField ID="hdnMessageId" runat="server" />
    <asp:HiddenField ID="hdnIsSavedInDraft" runat="server" />
    <asp:HiddenField ID="hdnAllowedFileSize" runat="server" />
    <asp:HiddenField ID="hdnIsReplyMode" runat="server" />
    <asp:HiddenField ID="hdnCount" runat="server" />
    <asp:HiddenField ID="hdnIntervalId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnIsSharedUser" runat="server" />
    <asp:HiddenField ID="hdnIsNeedToShowUsersInBCCInsteadOfTo" runat="server" />
    <%--<asp:HiddenField ID="hdnIsNededToShowCopyMeInMailCheckBox" runat="server" />--%>
</asp:Content>

