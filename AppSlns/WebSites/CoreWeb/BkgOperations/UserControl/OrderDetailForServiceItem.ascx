<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderDetailForServiceItem.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.OrderDetailForServiceItem" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="ApplicantDetails" Src="~/BkgOperations/UserControl/ApplicantDetails.ascx" %>
<%@ Register TagPrefix="uc" TagName="ApplicantData" Src="~/BkgOperations/UserControl/ApplicantData.ascx" %>

<uc:ApplicantDetails ID="ucApplicantDetails" runat="server" />

<asp:Panel ID="pnlExistingData" runat="server">
    <uc:ApplicantData ID="ucApplicantData" runat="server" />
</asp:Panel>

<asp:Panel ID="pnlLoader" runat="server">
</asp:Panel>
<infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Save,Cancel"
    AutoPostbackButtons="Save,Cancel" 
    SaveButtonText="Cancel Order" SaveButtonIconClass="rbPrevious" CancelButtonIconClass="rbNext" CancelButtonText="Continue" 
    OnSaveClick="CmdBarRestart_Click" OnCancelClick="CmdBarSave_Click">
</infsu:CommandBar>

<asp:HiddenField ID="hdnHiddenReadOnlyPanels" runat="server" />