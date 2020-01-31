<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementApplicantDetailPanel.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.RequirementApplicantDetailPanel" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxAdminView1">
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/RequirementVerificationDetail.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="framebar fixed_box">
    <div class="title user" id="divUser" runat="server">
        <%--UAT-4308--%>
        <asp:Label ID="lblApplicantName" runat="server" ToolTip="Applicant Name" />
         <asp:Label ID="lblApplicantMiddleName" runat="server" ToolTip="Applicant Middle Name" ForeColor="Red" />
        <asp:Label ID="lblApplicantLastName" runat="server" ToolTip="Applicant Last Name" />&nbsp;
        
        <asp:Label ID="lblApplicantDOB" runat="server" class="dob" Style="padding-bottom: 4px;" />
    </div>
    <div class="commands">
        <!--Code changed by [BS] Image instaed of ImageButton. Performance improvement task-->
        <%-- <asp:Image ID="imgRushOrder" runat="server" AlternateText="" Height="16" Width="16"
            ImageUrl="~/Resources/Mod/Shared/images/rush.png" />--%>
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
                <div class="Rotation_School">
                    <span class="o_title">School:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblSchool" runat="server" />
                    </span>
                </div>
                <div class="Agency_Name">
                    <span class="o_title">Agency:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblAgencyName" runat="server" />
                    </span>
                </div>
                <div class="Rotation_Name">
                    <span class="o_title">Rotation ID/Name:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblRotationName" runat="server" />
                    </span>
                </div>
                <div class="Rotation_Department">
                    <span class="o_title">Department:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblDepatment" runat="server" />
                    </span>
                </div>
                <div class="Rotation_Program">
                    <span class="o_title">Program:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblProgram" runat="server" />
                    </span>
                </div>
                <div class="Rotation_Course">
                    <span class="o_title">Course:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblCourse" runat="server" />
                    </span>
                </div>
                <div class="Rotation_Term">
                    <span class="o_title">Term:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblTerm" runat="server" />
                    </span>
                </div>
                <div class="Rotation_Unit/Floor">
                    <span class="o_title">Unit/Floor:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblUnitFloor" runat="server" />
                    </span>
                </div>
                <div class="Rotation_Recommended_Hours">
                    <span class="o_title">Recommended Hours:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblRecommendedHours" runat="server" />
                    </span>
                </div>
                <div class="Rotation_Students">
                    <span class="o_title">Students:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblStudents" runat="server" />
                    </span>
                </div>
                <div class="Rotation_Days">
                    <span class="o_title">Days:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblDays" runat="server" />
                    </span>
                </div>
                <div class="Rotation_Shift">
                    <span class="o_title">Shift:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblShift" runat="server" />
                    </span>
                </div>
                <div class="Rotation_StartTime">
                    <span class="o_title">Start Time:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblStartTime" runat="server" />
                    </span>
                </div>
                <div class="Rotation_EndTime">
                    <span class="o_title">End Time:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblEndTime" runat="server" />
                    </span>
                </div>
                <div class="Rotation_StartDate">
                    <span class="o_title">Start Date:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblStartDate" runat="server" />
                    </span>
                </div>
                <div class="Rotation_EndDate">
                    <span class="o_title">End Date:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblEndDate" runat="server" />
                    </span>
                </div>
                <div class="Complio_ID">
                    <span class="o_title">Complio ID:</span>&nbsp;<span class="o_val" style="font-weight: 700; font-size: 110%">
                        <asp:Label ID="lblComplioID" runat="server" />
                    </span>
                </div>
                <div class="compliance_info">
                    <span class="c_title">Rotation Compliance Status:&nbsp;</span><span class="c_val"><asp:Label
                        ID="lblRotationComplianceStatus" runat="server" Text="" CssClass="error"></asp:Label></span>&nbsp;
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
            <div class="row icon hier" title="Package Name">
                <asp:Label ID="lblPackageName" runat="server" Text="" CssClass="hier_data"></asp:Label>
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
            <div class="row icon home" title="Applicant's Address" id="divAddress" runat="server">
                <asp:Label ID="lblAddress" runat="server" />
            </div>
            <div class="row icon ph" title="Applicant's Phone Numbers" id="divPhoneNumber" runat="server">
                <asp:Label ID="lblPhones" runat="server" />
            </div>
            <div class="row icon als" id="dvAlias" title="Applicant's Alias/Maiden" runat="server" visible="false">
                <asp:Label ID="lblAlias" runat="server" />
            </div>
        </div>
    </div>
    <div style="padding: 0px 5px 5px;">
        <infs:WclButton ID="btnApproveAllPendingReviewItems" AutoPostBack="false" runat="server" Text="Approve All Pending Items" ToolTip="Mark All Pending Review as Meets Requirement" OnClientClicked="getConfirmation" OnClick="btnApproveAllPendingReviewItems_Click"></infs:WclButton>
        <div style="width: 90px; float: right; padding-top: 5px;">
            <asp:Label runat="server" ID="lblCurrentRotation" Visible="false" Style="color: #8C1921 !important;">Current Rotation</asp:Label>
        </div>
    </div>
    <div id="dvCategories">
        <infs:WclListBox runat="server" ID="lstCategories" Width="100%" AutoPostBack="false" OnSelectedIndexChanged="lstCategories_SelectedIndexChanged"
            CssClass="list_cate" OnItemDataBound="lstCategories_DataBound">
            <ItemTemplate>
                <div>
                    <a onclick="openInPageFrame(this)" id="lnkCategoriesNavigation" runat="server" class="cat_lslnk">
                        <asp:Image ID="imgStatus" ImageUrl='<%# GetImageUrl(Convert.ToString(DataBinder.Eval(Container.DataItem, "CatStatusCode")),
                                                                            Convert.ToBoolean( DataBinder.Eval(Container.DataItem, "IsComplianceRequired")),
                                                                            Convert.ToString(DataBinder.Eval(Container.DataItem,"CategoryRuleStatusID"))
                                                                            ) %>'
                            runat="server" Style="vertical-align: top" ToolTip='<%#(DataBinder.Eval(Container.DataItem, "CatStatusName"))+GetComplianceRequiredDateSet(Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isActualComplianceRequired"))
                    ,Convert.ToString(DataBinder.Eval(Container.DataItem, "ComplianceRqdStartDate"))
                    ,Convert.ToString(DataBinder.Eval(Container.DataItem, "ComplianceRqdEndDate"))
                    
                    ) %>' />
                        <%# DataBinder.Eval(Container.DataItem, "CatName")%>
                    </a>
                </div>
            </ItemTemplate>
            <HeaderTemplate>
                <div style="padding: 5px; font-size: 14px">
                    Categories
                </div>
            </HeaderTemplate>
        </infs:WclListBox>
    </div>
    <asp:HiddenField ID="hdnApplicantId" runat="server" />
    <asp:HiddenField ID="hdnApplicantName" runat="server" />
    <asp:HiddenField ID="hdnSuccMsg" runat="server" />
    <asp:HiddenField ID="hdnPendingItems" runat="server" />
    <asp:HiddenField ID="hdnNextSubscriptionURLApplicant" runat="server" />
    <asp:HiddenField ID="hdnPreviousSubscriptionURLApplicant" runat="server" />

</div>
<script type="text/javascript">

    $jQuery(document).ready(function () {

        if ($jQuery("[id$=hdnSuccMsg]")[0].value != '') {
            $jQuery("[id$=pageMsgBox]")[0].style.display = "block";
            $jQuery("[id$=lblError]")[0].innerHTML = $jQuery("[id$=hdnSuccMsg]")[0].value;
            $jQuery("[id$=lblError]").attr("class", "sucs");
        }
        if ($jQuery("[id$=hdnPendingItems]")[0].value != '') {
            $jQuery("[id$=pageMsgBox2]")[0].style.display = "block";
            $jQuery("[id$=lblPendingItems]")[0].innerHTML = $jQuery("[id$=hdnPendingItems]")[0].value;
            $jQuery("[id$=lblPendingItems]").attr("class", "info");
        }
    });

    function getConfirmation() {
        $confirm('Are you sure you want to mark all pending items as meets requirements?', function (res) {
            if (res) {
                __doPostBack('<%= btnApproveAllPendingReviewItems.UniqueID %>', '');
            }
        }, 'Complio', true);
    }
</script>

