<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminEntryDrugScreenDataControl.ascx.cs" Inherits="CoreWeb.AdminEntryPortal.Views.AdminEntryDrugScreenDataControl" %>
<%@ Register TagPrefix="uc" TagName="WebCCF" Src="~/AdminEntryPortal/UserControl/AdminEntryWebCCF.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <div class="content">
        <div id="divCustomHtml" runat="server" class="customli" style="padding-bottom: 30px;">
        </div>
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <div class="msgbox" id="dvErrorMessageWithoutRegId" runat="server" visible="false">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="error" Text="We apologize for the inconvenience. Unfortunately your drug screening order cannot be completed. Please try again later.">
                </asp:Label>
            </div>

            <div id="divUCWebCCF" runat="server">
                <asp:UpdatePanel ID="updWebCCF" runat="server">
                   
                    <ContentTemplate>
                        <uc:WebCCF ID="ucWebCCF" runat="server" OnWebCCFError="ucWebCCF_Error" OnWebCCFSuccess="ucWebCCF_Success" />
                        <infsu:CommandBar ID="cmdRefresh" runat="server" Visible="true" ButtonPosition="Center" DisplayButtons="Submit"
                            AutoPostbackButtons="Submit" SubmitButtonText="Refresh" SubmitButtonIconClass="rbRefresh"
                            OnSubmitClick="cmdRefresh_SubmitClick">
                        </infsu:CommandBar>
                        <p style="font-size: .9em; text-align: center">If the location selection control above doesn't load within 30 seconds, please click the Refresh button to try again.</p>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divHiddenFields">
                <asp:HiddenField ID="hdnAttributeGroupId" runat="server" />
                <asp:HiddenField ID="hdnCustomFormId" runat="server" />
            </div>
            <asp:Panel ID="pnlReadOnly" CssClass="sxpnl" runat="server" Visible="false">
                <h3 class="mhdr">Drug Screen Information</h3>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Status</span>
                    </div>
                    <div class='sxlm m2sspn'>
                        <asp:Label ID="lblSuccess" runat="server" Text="Registered" />
                        <asp:Label ID="lblFailure" runat="server" Visible="false" CssClass="error" Text="NOT SCHEDULED - Please contact American DataBank to schedule your drug screen at the end of the order." />
                    </div>

                    <div class='sxlb'>
                       <%-- <asp:Label CssClass="cptn" ID="spnDrugScreen" runat="server" Visible="false" Text="Drug Screen Expiration"></asp:Label>--%>
                        <asp:Label ID="lblDrugScreenExpiration" runat="server" Visible="false" Text="Registration Expiration Date" CssClass="cptn" />
                    </div>
                    <div class='sxlm m2sspn'>
                        <asp:Label ID="lblExpiration" Visible="false" runat="server"  />
                    </div>
                    <div class='sxlb' style="display: none">
                        <span class="cptn" style="display: none">Registration ID</span>
                    </div>
                    <div class='sxlm' style="display: none">
                        <asp:Label ID="lblRegistrationId" Visible="false" runat="server" />
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>



