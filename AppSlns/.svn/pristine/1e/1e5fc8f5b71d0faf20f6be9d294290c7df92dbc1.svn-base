<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotificationMailDetails.aspx.cs" Inherits="CoreWeb.BkgOperations.Views.NotificationMailDetails" MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        // To close the popup.
        function ClosePopup() {
            top.$window.get_radManager().getActiveWindow().close();
        }
    </script>
    <div class="section">
        <h1 class="mhdr">
            <asp:Label ID="lblNotificationMail" runat="server" Text="Notification Mail Details"></asp:Label></h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel CssClass="sxpnl" runat="server" ID="pnlMailDetails">
                    <div class='sxro sx1co'>
                        <div class='sxlb'>
                            <span class='cptn'>Sender Name</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <div runat="server" id="divSenderName"></div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <div class='sxlb'>
                            <span class='cptn'>Sender Email ID</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <div runat="server" id="divSenderEmailID"></div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <div class='sxro sx1co'>
                        <div class='sxlb'>
                            <span class='cptn'>Subject</span>
                        </div>
                        <div class='sxlm m2spn'>
                            <div runat="server" id="divSubject"></div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>
                    <br />
                    <hr style="border-bottom: solid 1px #c0c0c0;" />
                    <br />
                    <div runat="server" id="divBody"></div>

                </asp:Panel>
            </div>
        </div>
        <infsu:CommandBar ID="fsucNotificationMailDetail" runat="server" DisplayButtons="Cancel" CancelButtonText="Close"
            ButtonPosition="Right" CauseValidationOnCancel="false" OnCancelClientClick="ClosePopup" />
    </div>
</asp:Content>
