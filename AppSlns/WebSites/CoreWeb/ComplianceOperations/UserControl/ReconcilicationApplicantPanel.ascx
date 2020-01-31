<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReconcilicationApplicantPanel.ascx.cs"
    Inherits="CoreWeb.ComplianceOperations.Views.ReconcilicationApplicantPanel" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemDetails" Src="~/ComplianceOperations/UserControl/ItemDetails.ascx" %>

<div class="framebar fixed_box">
    <div class="title user" id="divUser" runat="server">
        <%-- style="vertical-align:middle; height:90%; padding-top:4px;"--%>
         <asp:Label ID="lblApplicantName" runat="server" ToolTip="Applicant First Name" />
        <asp:Label ID="lblApplicantMiddleName" runat="server" ToolTip="Applicant Middle Name" ForeColor="Red" />
        <asp:Label ID="lblApplicantLastName" runat="server" ToolTip="Applicant Last Name" />&nbsp;
        <asp:Label
            ID="lblApplicantDOB" runat="server" class="dob" Style="padding-bottom: 4px;" />
    </div>
    <div class="commands">
        <!--Code changed by [BS] Image instaed of ImageButton. Performance improvement task-->
        <asp:Image ID="imgRushOrder" runat="server" AlternateText="" Height="16" Width="16"
            ImageUrl="~/Resources/Mod/Shared/images/rush.png" />
        <%--<asp:ImageButton ID="btnPrevApp" ImageUrl="~/Resources/Mod/Compliance/images/left-arrow.png"
            runat="server" ToolTip="Goto Previous Subscription" OnClick="btnPrevApp_Click" />
        <asp:ImageButton ID="btnNextApp" ImageUrl="~/Resources/Mod/Compliance/images/right-arrow.png"
            runat="server" ToolTip="Goto Next Subscription" OnClick="btnNextApp_Click" />--%>
        <a onclick="openInPageFrame(this)" id="lnkBtnPrevApp" runat="server">
            <asp:Image ID="btnPrevApp" ImageUrl="~/Resources/Mod/Compliance/images/left-arrow-blue.png"
            runat="server" ToolTip="Go to Previous Subscription" />
        </a><a onclick="openInPageFrame(this)" id="lnkBtnNextApp" runat="server">
             <asp:Image ID="btnNextApp" ImageUrl="~/Resources/Mod/Compliance/images/right-arrow-Blue.png"
            runat="server" ToolTip="Go to Next Subscription"/>
        </a>
    </div>
</div>
<div id="applicant_data" class="border_box scroll-box">
    <div id="profile_info" style="display: block!important">
        <div class="top">
            <div class="left">
                <div class="order_info">
                    <span class="o_title">Order#</span>&nbsp;<span class="o_val"><asp:Label ID="lblOrder"
                        runat="server" /></span>
                </div>
                <div class="order_approval">
                    <span class="o_title">Order Approval Date:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblOrderApprovalDate" runat="server" />
                    </span>
                </div>
                <div class="order_approval">
                    <span class="o_title">Subscription Expiration Date</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblExpirationDate" runat="server" />
                    </span>
                </div>
                <div class="compliance_info">
                    <span class="c_title">Overall Compliance Status&nbsp;</span><span class="c_val"><asp:Label
                        ID="lblOverComplianceStatus" runat="server" Text="" CssClass="error"></asp:Label></span>&nbsp;
                    <asp:Image ID="imgPackageComplianceStatus" runat="server" CssClass="img_status" />
                </div>
            </div>
            <div class="right">
                <div class="img_wrapper">
                    <div class="thumb">
                        <asp:Image ID="imgApplicantPhoto" runat="server" AlternateText="" Height="77" Width="72" />
                    </div>
                    <div class="name-in">
                        <asp:Label Text="AD" runat="server" ID="lblNameInitials" />
                    </div>
                </div>
            </div>
            <div class="reset">
            </div>
        </div>
        <div class="bottom">
            <div class="row icon hier" title="Institute Hierarchy">
                <asp:Label ID="lblBredCrum" runat="server" Text="" CssClass="hier_data"></asp:Label>
            </div>
            <div class="row icon email" title="Applicant's Email">
                <asp:Label ID="lblEmail" runat="server" />
            </div>
            <div class="row icon ssn" title="Applicant's SSN" id="divSSN" runat="server">
                <asp:Label ID="lblSSN" runat="server" />
            </div>
            <div class="row icon home" title="Applicant's Address">
                <asp:Label ID="lblAddress" runat="server" />
            </div>
            <div class="row icon ph" title="Applicant's Phone Numbers">
                <asp:Label ID="lblPhones" runat="server" />
            </div>
            <div class="row icon als" id="dvAlias" title="Applicant's Alias/Maiden" runat="server" visible="false">
                <asp:Label ID="lblAlias" runat="server" />
            </div>
            <div class="row icon usrGrp" id="dvUserGroup" title="Applicant's User Group(s)" runat="server" visible="false">
                <asp:Label ID="lblUserGroups" runat="server" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnApplicantId" runat="server" />
    <asp:HiddenField ID="hdnApplicantName" runat="server" />
</div>


