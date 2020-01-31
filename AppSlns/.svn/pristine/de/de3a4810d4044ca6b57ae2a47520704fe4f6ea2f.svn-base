<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.VerificationApplicantPanel" CodeBehind="VerificationApplicantPanel.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemDetails" Src="~/ComplianceOperations/UserControl/ItemDetails.ascx" %>

<%--<style type="text/css">
    .RadButton_Outlook.RadButton.rbLinkButton
    {
        background-color: #fbfaf9;
        border: none !important;
        color: black;
        padding: 0px;
    }

    #divCatLinks .rbCatlinks
    {
        background-color: transparent !important;
        border: none !important;
    }

        #divCatLinks .rbCatlinks span
        {
            text-align: left !important;
            padding-left: 25px !important;
        }

    #divCatLinks .rbCatlinks
    {
        height: auto !important;
    }

    #divCatLinks .rbCatlinks
    {
        text-decoration: none;
        width: 100%;
        height: 100%;
        display: inline-block;
        color: #000;
    }

        #divCatLinks .rbCatlinks:visited
        {
            color: #000;
        }

        #divCatLinks .rbCatlinks:hover
        {
            color: blue;
        }
</style>--%>

<div class="framebar fixed_box">
    <div class="title user" id="divUser" runat="server">
        <%-- style="vertical-align:middle; height:90%; padding-top:4px;"--%>
        <asp:Label ID="lblApplicantName" runat="server" ToolTip="Applicant First Name" />
        <asp:Label ID="lblApplicantMiddleName" runat="server" ToolTip="Applicant Middle Name" ForeColor="Red" />
        <asp:Label ID="lblApplicantLastName" runat="server" ToolTip="Applicant Last Name" />&nbsp;<asp:Label
            ID="lblApplicantDOB" runat="server" class="dob" Style="padding-bottom: 4px;" />        
    </div>
    <div class="commands">
        <!--Code changed by [BS] Image instaed of ImageButton. Performance improvement task-->
        <asp:Image ID="imgRushOrder" runat="server" AlternateText="" Height="16" Width="16"
            ImageUrl="~/Resources/Mod/Shared/images/rush.png" />
        <a onclick="openInPageFrame(this)" id="lnkBtnPrevApp" runat="server">
            <asp:Image ID="btnPrevApp" ImageUrl="~/Resources/Mod/Compliance/images/left-arrow-blue.png"
                runat="server" ToolTip="Go to Previous Subscription" />
        </a><a onclick="openInPageFrame(this)" id="lnkBtnNextApp" runat="server">
            <asp:Image ID="btnNextApp" ImageUrl="~/Resources/Mod/Compliance/images/right-arrow-Blue.png"
                runat="server" ToolTip="Go to Next Subscription" />
        </a>
        <%-- <infs:WclButton runat="server" OnClick="btnPrevApplicant_Click" ID="btnPrevApplicant" Width="20px" Height="19px"
            ToolTip="Goto Previous Subscription">
            <Image EnableImageButton="true" DisabledImageUrl="~/Resources/Mod/Compliance/images/left-arrowd.png"
                ImageUrl="~/Resources/Mod/Compliance/images/left-arrow.png" />
        </infs:WclButton>
        <infs:WclButton runat="server" OnClick="btnNextApplicant_Click" ID="btnNextApplicant" Width="20px" Height="19px"
            ToolTip="Goto Next Subscription">
            <Image EnableImageButton="true" DisabledImageUrl="~/Resources/Mod/Compliance/images/right-arrowd.png"
                ImageUrl="~/Resources/Mod/Compliance/images/right-arrow.png" />
        </infs:WclButton>--%>
    </div>
</div>
<div id="applicant_data" class="border_box scroll-box">
    <div id="cmd_profile_info" class="colsd">
        Show more details
    </div>
    <div id="profile_info">
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
            <div class="row icon email" id="dvSecondaryEmail" runat="server" title="Applicant's Secondary Email">
                <asp:Label ID="lblSecondaryEmail" runat="server" />
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
    <infs:WclListBox runat="server" ID="lstCategories" Width="100%" AutoPostBack="false"
        CssClass="list_cate" OnSelectedIndexChanged="lstCategories_SelectedIndexChanged"
        OnItemDataBound="lstCategories_DataBound">
        <ItemTemplate>
            <div>
                <a onclick="openInPageFrame(this)" id="lnkCategoriesNavigation" runat="server" class="cat_lslnk">
                    <asp:Image ID="imgStatus" ImageUrl='<%# GetImageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "CategoryStatusCode")),
                    Convert.ToString( DataBinder.Eval(Container.DataItem, "CategoryExceptionStatusCode")), Convert.ToBoolean( DataBinder.Eval(Container.DataItem, "IsComplianceRequired"))
                    ,Convert.ToString(DataBinder.Eval(Container.DataItem,"RulesStatusID"))) %>'
                        runat="server" Style="vertical-align: top" ToolTip='<%# GetStatus(Convert.ToString(DataBinder.Eval(Container.DataItem, "CategoryStatusCode")),
                    Convert.ToString( DataBinder.Eval(Container.DataItem, "CategoryExceptionStatusCode")),
                    Convert.ToString(DataBinder.Eval(Container.DataItem, "CategoryStatusName"))) + GetComplianceRequiredDateSet(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsActualComplianceRequired"))
                    ,Convert.ToString(DataBinder.Eval(Container.DataItem, "ComplianceStartDate"))
                    ,Convert.ToString(DataBinder.Eval(Container.DataItem, "ComplianceEndDate"))
                    
                    ) %>' />
                    <%# DataBinder.Eval(Container.DataItem, "CategoryName")%>
                </a>
            </div>
            <%--<div id="divCatLinks">
                <infs:WclButton ButtonType="LinkButton" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName")%>' OnClick="btnCategorylnk_Click"
                    CssClass="rbCatlinks" AutoPostBack="true" runat="server" ID="btnCategorylnk" Width="100%" Height="20px">
                </infs:WclButton>
            </div>--%>
        </ItemTemplate>
        <HeaderTemplate>
            <div style="padding: 5px; font-size: 14px">
                Categories
            </div>
        </HeaderTemplate>
    </infs:WclListBox>
    <asp:HiddenField ID="hdnApplicantId" runat="server" />
    <asp:HiddenField ID="hdnApplicantName" runat="server" />
    <asp:HiddenField ID="hdnNextSubscriptionURLApplicant" runat="server" />
    <asp:HiddenField ID="hdnPreviousSubscriptionURLApplicant" runat="server" />
</div>
