<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderDetailPage.ascx.cs"
    Inherits="CoreWeb.BkgOperations.Views.OrderDetailPage" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/BkgOperations/UserControl/OrderNotificationHistory.ascx" TagPrefix="uc1"
    TagName="OrderNotificationHistory" %>



<infs:WclResourceManagerProxy runat="server" ID="rprxOrderDetailPage">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<script type="text/javascript">
    function UpdateHeaderInfo(orderStatus, imageUrl) {

        var summaryBar = parent.$jQuery("[id$=summaryBar]");
        if (summaryBar.length > 0) {

            lblStatus = summaryBar.find("[id$=lblStatus]");
            if (lblStatus.length > 0) {
                lblStatus[0].innerHTML = orderStatus;
            }

            if (imageUrl != '') {
                var imgOrderFlag = summaryBar.find("[id$=imgOrderFlag]");

                if (imgOrderFlag.length > 0) {
                    var reImage = imageUrl.substring(imageUrl.lastIndexOf('/'));

                    var imagePathPrefix = imgOrderFlag.attr('src').substring(0, imgOrderFlag.attr('src').lastIndexOf('/'));

                    imgOrderFlag.attr('src', imagePathPrefix + reImage);
                    imgOrderFlag[0].style.display = 'block'
                }
            }
        }


        return true;
    }

    function ClickMe() {
        var lnkGoBack = $jQuery("[id$=lnkGoBack]");
        //window.parent.location = lnkGoBack[0].href;
        var url = lnkGoBack[0].href;
        parent.openURLInPageFrame(url);
        return true;
    }

    //Redirect to Bkg Order Review Queue
    function RedirectToBkgOrderReviewQueue(url) {
        parent.openURLInPageFrame(url);
        return true;
    }

</script>
<%--<style type="text/css">
    #txtAddress
    {
        word-wrap: break-word;
        word-break: break-all;
    }
</style>--%>

<div class="container-fluid">
    <div class="msgbox" id="divSuccessMsg">
        <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
    </div>
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Applicant Details</h2>
        </div>
    </div>
    <div class="row bgLightGreen">
        <div class="col-md-12">
            <div class="row">
                <div class='form-group col-md-3'>
                    <span class='cptn'>Applicant Name</span>
                    <infs:WclTextBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtApplicantName"
                        runat="server" Enabled="false" />
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Alias/Maiden Names</span>
                    <infs:WclComboBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="cmbAliasName"
                        DataValueField="ID" DataTextField="ApplicantAliasName" runat="server">
                    </infs:WclComboBox>
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Gender</span>
                    <infs:WclTextBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtGender" runat="server"
                        Enabled="false" />
                </div>
            </div>
        </div>

        <div class="col-md-12">
            <div class="row bgLightGreen">
                <div class='form-group col-md-3'>
                    <div id="divDOB" runat="server">

                        <span class='cptn'>Date of Birth</span>

                        <infs:WclTextBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtDOB" runat="server"
                            Enabled="false" />
                    </div>
                </div>
                <div class='form-group col-md-3'>

                    <span class='cptn'>Phone</span>

                    <infs:WclMaskedTextBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtPhone"
                        runat="server" Enabled="false"
                        Mask="(###)-###-####" />
                    <infs:WclTextBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtPhoneUnMasking"
                        runat="server" Enabled="false"/>
                </div>
                <div class='form-group col-md-3'>

                    <span class='cptn'>Service Groups</span>

                    <infs:WclComboBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="cmbServiceGroups"
                        DataValueField="BSG_ID" DataTextField="BSG_Name" runat="server">
                    </infs:WclComboBox>
                </div>
            </div>

        </div>
        <div class="col-md-12">
            <div class="row bgLightGreen">
                <div id="divSSN" runat="server">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>SSN</span>

                        <infs:WclMaskedTextBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtSSN"
                            runat="server" Enabled="false"
                            Mask="###-##-####" />
                    </div>
                </div>
                <div id="divSSNMasked" visible="false" runat="server">

                    <div class='form-group col-md-3'>
                        <span class='cptn'>SSN</span>

                        <infs:WclTextBox ID="txtSSNMasked" Skin="Silk" AutoSkinMode="false" Width="100%"
                            runat="server" Enabled="false" />
                    </div>
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Email</span>

                    <infs:WclTextBox ID="txtEmail" Skin="Silk" AutoSkinMode="false" Width="100%" runat="server"
                        Enabled="false" />
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Address</span>

                    <infs:WclTextBox ID="txtAddress" Skin="Silk" AutoSkinMode="false" Width="100%" runat="server"
                        Enabled="false" Wrap="true" ClientIDMode="Static" />
                </div>

            </div>
        </div>
        <div class="col-md-12">
            <div class="row bgLightGreen">
                <div class='form-group col-md-6'>
                    <span class='cptn'>Institute Hierarchy</span>

                    <infs:WclTextBox ID="txtInstituteHierarchy" Skin="Silk" AutoSkinMode="false" Width="100%"
                        runat="server" Enabled="false" />
                </div>

            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color">Order Details</h2>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="row bgLightGreen">
                <div class='form-group col-md-3'>
                    <span class='cptn'>Vendor Account</span>
                    <infs:WclComboBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="cmbClearStarAccount"
                        runat="server" DataTextField="EVOD_AccountNumber">
                    </infs:WclComboBox>
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Created On</span>
                    <infs:WclTextBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtCreatedOn"
                        runat="server" ReadOnly="true" />
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Order Status</span>
                    <infs:WclComboBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="cmbOrderStatus"
                        runat="server" DataValueField="OrderStatusTypeID" DataTextField="StatusType"
                        EmptyMessage="--Select--" AllowCustomText="true" ValidationGroup="grpOrderStatus">
                    </infs:WclComboBox>
                    <div class="vldx">
                        <asp:RequiredFieldValidator runat="server" ID="rfvOrderStatus" ControlToValidate="cmbOrderStatus"
                            Display="Dynamic" ValidationGroup="grpOrderStatus" CssClass="errmsg"
                            Text="Order Status is required." />
                    </div>
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Completed On</span>
                    <infs:WclTextBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtCompletedOn"
                        runat="server" ReadOnly="true" />
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="row bgLightGreen">
                <div style="display: none;">
                    <div class='form-group col-md-3'>
                        <span class='cptn'>Flag Color</span>
                        <asp:Image ID="imgOrderFlag" ImageUrl="" runat="server" Height="20px" Width="20px" />
                        <infs:WclTextBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="lblOrderFlag"
                            Visible="false" runat="server" ReadOnly="true" />
                    </div>
                    <%--   <div class='sxlb'>
                        <span class='cptn'>Completed On</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclTextBox ID="txtCompletedOn" runat="server" Enabled="false" />
                    </div>--%>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="row bgLightGreen">
                <div class='form-group col-md-3'>
                    <span class='cptn'>Flag Color</span>
                    <infs:WclComboBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="rcbInstitutionStatusColorIcons"
                        runat="server" DataValueField="IOF_ID" DataTextField="OFL_Tooltip" EmptyMessage="--Select--"
                        AllowCustomText="true">
                        <ItemTemplate>
                            <asp:Image ID="imbIcon" runat="server" ImageUrl='<%# String.Format("~/{0}/{1}", Eval("OFL_FilePath"), Eval("OFL_FileName")) %>' />
                            <asp:Label ID="lblTooltip" runat="server" Text='<%# Eval("OFL_Tooltip") %>'></asp:Label>
                        </ItemTemplate>
                    </infs:WclComboBox>
                </div>
                <%--<div class='sxlb'>
                        <span class='cptn'>Order Status</span>
                    </div>--%>
            </div>
        </div>
    </div>


    <div class="section" id="dvServiceGroup" runat="server" visible="false">
        <h1 class="mhdr">Service Group Details</h1>
        <div class="content">
            <div class="sxform auto">
                <div class="sxpnl" >
                    <div class='sxro sx3co'>
                        <div class='sxlb'>
                            <span class='cptn'>Service Group Name</span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblSvcGroupName" runat="server" Text=""></asp:Label>
                        </div>
                        <div class='sxlb'>
                            <span class='cptn'>Current Review Status</span>
                        </div>
                        <div class='sxlm'>
                            <asp:Label ID="lblCurrentReviewStatus" runat="server" Text=""></asp:Label>
                        </div>
                        <div id="dvUpdateReviewStatus" runat="server">
                            <div class='sxlb'>
                                <span class='cptn'>Update Review Status</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclComboBox ID="cmbSvcGroupReviewStatus" runat="server" DataTextField="BSGRS_ReviewStatusType" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab"
                                    DataValueField="BSGRS_ReviewCode" EmptyMessage="--Select--" ValidationGroup="grpOrderStatus">
                                </infs:WclComboBox>
                                <%--<div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvSvcGroupReviewStatus" ControlToValidate="cmbSvcGroupReviewStatus"
                                Display="Dynamic" ValidationGroup="grpOrderStatus" CssClass="errmsg"
                                Text="Service Group Review Status is required." />
                        </div>--%>
                            </div>
                        </div>
                        <%--<div class='sxlb'>
                        <span class='cptn'>Flag Color</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbSvcGroupFlagColor" runat="server" DataValueField="IOF_ID" DataTextField="OFL_Tooltip" EmptyMessage="--Select--" AllowCustomText="true">
                            <ItemTemplate>
                                <asp:Image ID="imbIcon" runat="server" ImageUrl='<%# String.Format("~/{0}/{1}", Eval("OFL_FilePath"), Eval("OFL_FileName")) %>' />
                                <asp:Label ID="lblTooltip" runat="server" Text='<%# Eval("OFL_Tooltip") %>'></asp:Label>
                            </ItemTemplate>
                        </infs:WclComboBox>
                    </div>--%>
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <h1 class="header-color">Payment Details</h1>
                </div>
            </div>
        </div>
        <div class="col-md-12">
            <div class="row bgLightGreen">

                <div class='form-group col-md-3'>
                    <span class='cptn'>Total Order Price</span>

                    <infs:WclNumericTextBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="txtTotalOrderPrice"
                        runat="server" Enabled="false" NumberFormat-DecimalDigits="2" Type="Currency">
                    </infs:WclNumericTextBox>
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Payment Type</span>

                    <%--<infs:WclTextBox ID="txtPaymentType" runat="server" Enabled="false"/>--%>
                    <infs:WclComboBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="cmbPaymentType"
                        DataValueField="PaymentOptionID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" DataTextField="PaymentOption" runat="server">
                    </infs:WclComboBox>
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Payment Status</span>

                    <%--<infs:WclTextBox ID="txtPaymentStatus" runat="server" Enabled="false" />--%>
                    <infs:WclComboBox Skin="Silk" AutoSkinMode="false" Width="100%" ID="cmbPaymentStatus"
                        DataValueField="PaymentStatusID" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" DataTextField="PaymentStatus" runat="server">
                    </infs:WclComboBox>
                </div>
            </div>
        </div>


    </div>




    <div class="row">
        <div class="col-md-12">
            <h1 class="header-color">MVR Details</h1>
        </div>
    </div>
    <div class="row bgLightGreen">
        <div class="col-md-12">
            <div class='row' runat="server" id="divNoRecords">
                <div class='col-md-3'>
                    No records to display.
                </div>
            </div>
            <div id="divDriverInfo" runat="server" visible="false">
                <div class='form-group col-md-3'>
                    <span class='cptn'>Driver License Number</span>
                    <infs:WclTextBox CssClass="form-control" Width="100%" ID="txtDriverLicenseNo"
                        runat="server" Enabled="false" />
                </div>
                <div class='form-group col-md-3'>
                    <span class='cptn'>Driver License State</span>
                    <infs:WclTextBox CssClass="form-control" Width="100%" ID="txtDriverLicenseName"
                        runat="server" Enabled="false" />
                </div>
            </div>
        </div>
    </div>
</div>

<uc1:OrderNotificationHistory runat="server" ID="OrderNotificationHistory" />
<div class="col-md-12">&nbsp;</div>
<div class="col-md-12 text-center">
    <infs:WclButton ID="btnUpdate" runat="server" Skin="Silk" AutoSkinMode="false" Text="Save"
        OnClick="btnUpdate_Click" AutoPostBack="true"
        ValidationGroup="grpOrderStatus">
        <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
            PrimaryIconWidth="14" />
    </infs:WclButton>
    &nbsp;&nbsp;&nbsp;
     <infs:WclButton ID="btnReview" runat="server" Skin="Silk" AutoSkinMode="false" Text="Review"
         OnClick="btnReview_Click" AutoPostBack="true" ValidationGroup="" Visible="false">
         <Icon PrimaryIconCssClass="rbReview" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
             PrimaryIconWidth="14" />
     </infs:WclButton>
    &nbsp;&nbsp;&nbsp;
    <infs:WclButton ID="btnRollback" runat="server" Skin="Silk" AutoSkinMode="false" Text="Rollback Auto Completion"
        OnClick="btnRollback_Click" AutoPostBack="true" ValidationGroup="" Visible="false">
        <Icon PrimaryIconCssClass="rbRollback" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
            PrimaryIconWidth="14" />
    </infs:WclButton>
    &nbsp;&nbsp;&nbsp;
     <infs:WclButton ID="btnIsSupplement" runat="server" Text="Supplement Order" AutoPostBack="false"
         OnClientClicked="ClickMe" Visible="false">
         <%-- <Icon PrimaryIconCssClass="rbSave" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
             PrimaryIconWidth="14" />--%>
     </infs:WclButton>



    <a runat="server" id="lnkGoBack" target="_top"></a>
</div>
<div class="col-md-12">&nbsp;</div>

