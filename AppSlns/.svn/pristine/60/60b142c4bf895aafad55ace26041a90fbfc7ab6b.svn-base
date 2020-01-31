<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PopupMaster.master" AutoEventWireup="true" Inherits="Student_UserControls_Comments" Codebehind="Comments.aspx.cs" %>

<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxName1">
        <infs:LinkedResource Path="~/Resources/Mod/Student/compliances.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Student/comment.css" ResourceType="StyleSheet" />
    </infs:WclResourceManagerProxy>
    <div class="section">
        <h1 class="mhdr">
            Comments</h1>
        <div class="content">
            <!-- section contents -->
            <div class="comment_group">
                <div class="comment">
                    <div class="sender">
                        Greta A. (ADB Admin)</div>
                    <div class="time_stamp">
                        3/24/2013 12:40 PM</div>
                    <div class="msg">
                        Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Curabitur consequat metus
                        vitae est. Duis quis metus. Lorem ipsum dolor sit amet, consectetuer adipiscing
                        elit. Quisque scelerisque.
                    </div>
                </div>
                <div id="rgn_fcom">
                </div>
                <div class="comment">
                    <div class="sender">
                        Me</div>
                    <div class="time_stamp">
                        3/24/2013 12:40 PM</div>
                    <div class="msg">
                        Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Curabitur consequat metus
                        vitae est. Duis quis metus. Lorem ipsum dolor sit amet, consectetuer adipiscing
                        elit. Quisque scelerisque.
                    </div>
                </div>
            </div>
            <div class="comment_commands">
                <a id="btn_showall" href="#">Show</a>&nbsp;|&nbsp;<a id="btn_addcomment" href="#">Add
                    Comment</a>
            </div>
            <div class="post_comment">
                <h5 style="margin: 10px 0;">
                    Add Comment</h5>
                <textarea id="txtComment"></textarea>
                <div class="comment_commands">
                    <a id="btn_savecomment" href="#">Post Comment</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="Server">
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" DefaultPanel="pnlName1" CancelButtonText="Close"
        DisplayButtons="Cancel" ButtonPosition="Center" OnCancelClientClick="fnPopupClose" />
</asp:Content>
