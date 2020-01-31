<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Messaging.Views.SubscriptionSetting" Codebehind="SubscriptionSetting.ascx.cs" %>

 <%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        Email Settings
    </h1>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                <div class='sxro sx2co'>
                    <div class='sxlb'>
                        <span class="cptn">Notifications</span>
                    </div>
                    <div class='sxlm'>
                        <asp:CheckBoxList ID="cblNotification" runat="server" DataTextField="Name" DataValueField="CommunicationEventId"
                            RepeatLayout="UnorderedList">                            
                        </asp:CheckBoxList>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Reminders</span>
                    </div>
                    <div class='sxlm'>
                        <asp:CheckBoxList ID="cblReminder" runat="server" DataTextField="Name" DataValueField="CommunicationEventId"
                            RepeatLayout="UnorderedList">                            
                        </asp:CheckBoxList>
                    </div>
                  
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBar1" runat="server" DisplayButtons="Save" DefaultPanel="pnlName1"
            ButtonPosition="Center" AutoPostbackButtons="Save" OnSaveClick="btnSubmit_Click">                          
        </infsu:CommandBar>
    </div>
</div>

