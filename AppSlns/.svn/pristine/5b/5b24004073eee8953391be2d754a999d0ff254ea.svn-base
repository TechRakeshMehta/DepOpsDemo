<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminEntryNodeSpecificTemplates.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.AdminEntryNodeSpecificTemplates" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<%--<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="Label2" Text="Agency Hierarchy Specific Templates" runat="server" /></h1>
    <div class="content">
          </div>
</div>--%>
<div class="sxform auto">
    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAdminEntryNodeSpecificTemplatesForm">


        <div class='sxro sx3co'>
            <div class='sxlb'>
                <span class="cptn">Template Name</span><span class="reqd">*</span>
            </div>
            <div class='sxlm m2spn'>
                <infs:WclTextBox ID="txtTemplateName" runat="server" MaxLength="100">
                </infs:WclTextBox>
                <div class='vldx'>
                    <asp:RequiredFieldValidator runat="server" ID="rfvTemplateName" ControlToValidate="txtTemplateName"
                        class="errmsg" Display="Dynamic" ErrorMessage="Template Name is required." ValidationGroup="grpFormSubmitAdminEntryNodeSpecificTemplate" />
                    <asp:RegularExpressionValidator runat="server" ID="rgvTemplateName" ControlToValidate="txtTemplateName"
                        class="errmsg" Display="Dynamic" ErrorMessage="Only alphanumeric is allowed."
                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="grpFormSubmitAdminEntryNodeSpecificTemplate" />
                </div>
            </div>
        </div>
        <div class='sxro sx3co'>
            <div class='sxlb'>
                <span class="cptn">Subject</span><span class="reqd">*</span>
            </div>
            <div class='sxlm m2spn'>
                <infs:WclTextBox ID="txtTemplateSubject" runat="server" MaxLength="255">
                </infs:WclTextBox>
                <div class='vldx'>
                    <asp:RequiredFieldValidator runat="server" ID="rfvSubject" ControlToValidate="txtTemplateSubject"
                        class="errmsg" Display="Dynamic" ErrorMessage="Subject is required." ValidationGroup="grpFormSubmitAdminEntryNodeSpecificTemplate" />
                </div>
            </div>

            <div class='sxroend'>
            </div>
        </div>

        <div class='sxro sx3co'>
            <div class='sxlb'>
                <asp:Label ID="lblTemplateEditor" Text="Template Content" runat="server" AssociatedControlID="editorContent" CssClass="cptn" /><span
                    class="reqd">*</span>
            </div>
            <div class='sxlm  m3spn'>
                <infs:WclEditor ID="editorContent" Height="200px" runat="server" Width="100%" EditModes="Design"
                    OnClientLoad="OnClientLoad" ToolsFile="~/Templates/Data/Tools.xml" OnClientCommandExecuting="OnClientCommandExecuting">
                    <Content>
                        <body style="background-color:White;"></body>
                    </Content>
                </infs:WclEditor>
                <div class='vldx'>
                    <asp:RequiredFieldValidator runat="server" ID="rfvContent" ControlToValidate="editorContent"
                        class="errmsg" Display="Dynamic" ErrorMessage="Template content is required."
                        ValidationGroup="grpFormSubmitAdminEntryNodeSpecificTemplate" />
                </div>
            </div>
            <div class='sxroend'>
            </div>
        </div>



        <%--<infsu:CommandBar ID="cmdBar" runat="server" DefaultPanel="pnlAdminEntryNodeSpecificTemplatesForm"
            GridMode="true" ValidationGroup="grpFormSubmitAdminEntryNodeSpecificTemplate"
            GridInsertText="Save" GridUpdateText="Save" />--%>

        <%--OnSaveClick="btnSave_Click"--%>

        <%--
        --%>
    </asp:Panel>

</div>
<infsu:CommandBar ID="fsucCmdBar" runat="server" DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel"
    SubmitButtonText="Save HTML" DefaultPanel="pnlAdminEntryNodeSpecificTemplatesForm" ValidationGroup="grpFormSubmitAdminEntryNodeSpecificTemplate"
    OnSaveClick="fsucCmdBar_SaveClick" OnCancelClick="fsucCmdBar_CancelClick" />

<script type="text/javascript">
    function OnClientCommandExecuting(editor, args) {
        var name = args.get_name();
        var val = args.get_value();

        if (name == "ddPlaceHolders") {
            editor.pasteHtml(val);
            //Cancel the further execution of the command as such a command does not exist in the editor command list
            args.set_cancel(true);
        }
    }

    // Function added to handle the issue related to distortion of the Template management in Add/Edit mode
    function OnClientLoad(editor, args) {
        $jQuery('ul.reToolbar').width('auto');
        //This function added to set focus on text box's in TemplatesMaintenanceFormEventSpecific.ascx to resolved the issue of User is not able to edit the Template Content 
        //Subject/Template Name until or unless make any changes in either Days and frequency fields
        setTimeout(function () {
           // $jQuery("input[id$='txtNoOfDays']").focus();
           // $jQuery("input[id$='txtTemplateName']").focus();
            $jQuery("input[id$='txtTemplateName']").val($jQuery("input[id$='txtTemplateName']").val());
            // $jQuery("input[id$='txtTemplateName']").blur();
            //$jQuery("input[id$='txtNoOfDays']").blur();
        });
    }
</script>
