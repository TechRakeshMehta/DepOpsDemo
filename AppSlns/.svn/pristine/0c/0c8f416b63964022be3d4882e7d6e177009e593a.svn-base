<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentEditor.ascx.cs" Inherits="CoreWeb.BkgSetup.Views.ContentEditor" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="section">
    <h1 id="hdrContent" runat="server" class="mhdr">Content Editor
    </h1>
    <div class="content" id="divRadContent" runat="server">
        <infs:WclEditor ID="radContent" ToolsFile="~/BkgSetup/Data/ToolsContentEditor.xml" runat="server"
            DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd" Width="99%">
        </infs:WclEditor>
    </div>
</div>
<infsu:CommandBar ID="fsucCmdBar1" runat="server" DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel"
    SubmitButtonText="Save HTML" DefaultPanel="pnlName1" ButtonPosition="Center" OnSaveClick="fsucCmdBar1_SaveClick" 
    OnCancelClick="fsucCmdBar1_CancelClick"/>
<div style="height: 20px">
</div>