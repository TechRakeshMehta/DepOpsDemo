<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Messaging.Views.MoveToFolders"
    Title="Move To Folder" MasterPageFile="~/Shared/PopupMaster.master" Codebehind="MoveToFolders.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxName1">
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/MoveToFolder.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <div style="padding: 10px; background-color: #efefef;">
        <div style="margin-top: 10px; margin-bottom: 5px;">
            Move the selected items to the folder:</div>
        <div style="height: 200px; background-color: White; padding: 5px; border: 1px solid #adadad;
            overflow: auto">
            <input type="hidden" id="hdnParentID" />
            <input type="hidden" id="hdnMoveTofolderID" />
            <infs:WclTreeView ID="treePersonalFolders" runat="server" OnClientNodeClicked="e_nodeclicked"
                OnNodeDataBound="treeFolders_NodeDataBound">
                <%-- <Nodes>
                    <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/personaldir.png" Text="Personal Folders"
                        Expanded="true" Value="RootNode" Selected="true">
                        <Nodes>
                            <telerik:RadTreeNode Expanded="true" ImageUrl="~/Resources/Mod/Messaging/Icons/inbox.png"
                                Text="Inbox" Value="Inbox">
                                <Nodes>
                                    <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/internalmsg.png" Text="Internal Messages">
                                    </telerik:RadTreeNode>
                                    <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/email.png" Text="Emails">
                                    </telerik:RadTreeNode>
                                </Nodes>
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/sms.png" Text="SMS" Enabled="false">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/alerts.png" Text="Notifications">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/outbox.png" Text="Outbox" Enabled="false">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/sent.png" Text="Sent Items">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/deleted.png" Text="Deleted Items">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/notes.png" Text="Notes">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/searchdir.png" Text="Search Folders"
                                Enabled="false">
                                <Nodes>
                                    <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/searchdir.png" Text="Liberty University">
                                    </telerik:RadTreeNode>
                                    <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/searchdir.png" Text="Applicants Messages">
                                    </telerik:RadTreeNode>
                                </Nodes>
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/folder.png" Text="Liberty Admins">
                            </telerik:RadTreeNode>
                            <telerik:RadTreeNode ImageUrl="~/Resources/Mod/Messaging/Icons/folder.png" Text="FSU Mails">
                            </telerik:RadTreeNode>
                        </Nodes>
                    </telerik:RadTreeNode>
                </Nodes>--%>
            </infs:WclTreeView>
        </div>
        <div style="margin-top: 10px; margin-bottom: 5px;">
            Create a new folder in <span id="foldername" style="font-weight: bold">Personal Folders</span>:</div>
        <div>
            <infs:WclTextBox runat="server" ID="txtFolderName" Width="100%">
            </infs:WclTextBox>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" DisplayButtons="Save,Cancel" DefaultPanel="pnlName1"
        SaveButtonText="Move" OnSaveClientClick="saveClick" OnCancelClientClick="cancelClick" />
</asp:Content>
