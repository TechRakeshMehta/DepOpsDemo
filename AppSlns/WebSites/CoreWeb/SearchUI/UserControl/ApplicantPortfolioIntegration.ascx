<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Search.Views.ApplicantPortfolioIntegration"
    CodeBehind="ApplicantPortfolioIntegration.ascx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="row">
    <div class='col-md-12'>
        <h2 class="header-color" title="Details pertaining to the student's subscription are displayed in this section" tabindex="0">Integration(s)
        </h2>
    </div>
</div>

<div class="row">

    <div runat="server" id="divIntegrationSection" class="section">
        <div class="col-md-12 bgLightGreen" id="divContentIntegration" runat="server">
            <asp:Repeater ID="rptrIntegration" runat="server" OnItemCommand="rptrIntegration_ItemCommand"  OnItemDataBound="rptrIntegration_ItemDataBound" >
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class='form-group col-md-12'>
                        <div class="row">
                            <div class='form-group col-md-3'>
                                <label id="lblIntegrationName" style="color: #666; font-size: 15px; font-weight: 600; font-family: 'Titillium Web', sans-serif"><%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("IntegrationName"))) %></label>
                                <asp:Label ID="lblOrganizationUserIntegration" Visible="false" CssClass="form-control col-md-12" runat="server" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("IntegrationName"))) %>'></asp:Label>
                            </div>
                            <div class="pull-right">
                             <infsu:CommandBar ID="fsucCmdBarRemoveLinking" runat="server" ButtonPosition="Right" DisplayButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk"   SubmitButtonIconClass=""
                                    AutoPostbackButtons="Submit"  SubmitButtonText="Remove Linking" Visible='<%# Convert.ToBoolean(Eval("IsAdminCanRemoveLinking")) %>'>
                                </infsu:CommandBar>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<asp:HiddenField ID="hdnorganizationUserId" runat="server" Value="" />
<asp:HiddenField ID="hdncurrentLoggedInUserId" runat="server" Value="" />
<br />


