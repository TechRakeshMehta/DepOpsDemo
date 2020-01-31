<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OverridePackageServiceFormDispatchType.ascx.cs" Inherits="CoreWeb.BkgSetup.UserControl.OverridePackageServiceFormType" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<style>
    span.frmRpt
    {
        width: 180px;
        padding-right: 80px;
        padding-bottom: 10px;
        font-weight: bold;
    }

    div.frmRpt
    {
        width: 180px;
        display: inline-block;
        padding-right: 10px;
    }

    .header1
    {
        padding-left: 30px;
        padding-right: 50px !important;
    }

    .header2
    {
        width: 100px;
    }

    .contentCol1
    {
        vertical-align: top;
        padding-top: 5px;
    }

    .contentCol2
    {
        vertical-align: top;
        width: 75px !important;
        padding-left: 20px;
    }

    .contentCol3
    {
        width: 330px !important;
    }

    .contentCol4
    {
        vertical-align: top;
        padding-top: 5px;
    }

    .rbtnTest
    {
        padding-top: 10px;
    }

    div.frmRpt input[type="checkbox"] 
    { 
        padding-top: 0px !important; 
    }
</style>
<infs:WclResourceManagerProxy runat="server" ID="rprxDispatch">
    <infs:LinkedResource ResourceType="JavaScript" Path="~/Resources/Mod/BkgSetup/OverridePackageServiceFormDispatchType.js" />
</infs:WclResourceManagerProxy>
<div style="padding-top:20px;;padding-left:5px;">
    <h4> Service Form(s)</h4>

</div>
<asp:Repeater ID="rptServiceForms" runat="server" OnItemDataBound="rptServiceForms_ItemDataBound">
    <HeaderTemplate>
        <div style="display:block;padding-left:30px; padding-top:10px; padding-bottom:5px;   background-color:#adaaaa">
           <span class="frmRpt header1">Service Form Name</span>   <span class="frmRpt">Hide Service Form </span> 
        <span class="frmRpt">Package level Dispatch Mode Setting</span><span class="frmRpt">Dispatch Mode</span><br /></div>
    </HeaderTemplate>
    <ItemTemplate>
        <div style="display:block; padding-left:30px; background-color:#cbc7c7" class="rootDiv" >
            <asp:Panel ID="Panel3" CssClass="sxpnl" runat="server">
                    <div class='frmRpt contentCol1'>
                         <asp:Literal ID="litServiceFormName" runat="server" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ServiceFormName"))) %>'></asp:Literal>
                      
                    </div>
                    <div class='frmRpt contentCol2'>
                        <asp:CheckBox ID="chkVisibility" runat="server" onchange="ManageSts(this)" Checked='<%#  Eval("HideServiceForm") %>' />
                    </div>
                   
                    <div class='frmRpt contentCol3'>
                        <asp:RadioButtonList ID="rbtnList" CssClass="rbtnTest" runat="server" onclick="ManageSts(this)" RepeatDirection="Horizontal" width="300">
                            <asp:ListItem Value="AAAA" Text="Inherited"></asp:ListItem>
                            <asp:ListItem Value="AAAB" Text="Automatic"></asp:ListItem>
                            <asp:ListItem Value="AAAC" Text="Manual"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:HiddenField ID="hdfSAFMId" runat="server" Value='<%# Eval("ServiceAttachedFormMappingId") %>' />
                        <asp:HiddenField ID="hdfBPSOId" runat="server" Value='<%# Eval("BPSOId") %>' />
                        <asp:HiddenField ID="hdnInheritedStatus" runat="server"></asp:HiddenField>
                    </div>
                 <div class='frmRpt contentCol4'>
                       <span id="spnDispatchMode"></span>
                    </div>
            </asp:Panel>
        </div>
        <br />
    </ItemTemplate>
</asp:Repeater> 