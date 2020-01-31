<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ApplicantRotationRequirement.Views.SharedUserRequirementAttributeControl" CodeBehind="SharedUserRequirementAttributeControl.ascx.cs" %>
<script type="text/javascript">
    
</script>
<div id="dvMain" runat="server">
    <div class="form-group col-md-6">
        <asp:Literal ID="litLabel" runat="server"></asp:Literal>
        <asp:Panel ID="pnlControls" runat="server">
        </asp:Panel>
        <asp:HiddenField ID="hdfApplicantFieldDataId" runat="server" />
        <div id="divValidator" runat="server" class="vldx">
        </div>
    </div>
</div>
