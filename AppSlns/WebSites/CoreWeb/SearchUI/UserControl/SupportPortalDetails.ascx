<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.SearchUI.Views.SupportPortalDetails" CodeBehind="SupportPortalDetails.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantRequirementRotations.ascx" TagPrefix="infsu" TagName="ApplicantRequirementRotations" %>
<%@ Register Src="~/SearchUI/UserControl/ApplicantPortfolioProfile.ascx" TagPrefix="infsu"
    TagName="ApplicantPortfolioProfile" %>
<%@ Register Src="~/SearchUI/UserControl/SupportPortalNotes.ascx" TagPrefix="uc" TagName="SupportPortalNotes" %>

<style type="text/css">
    .autoRenewalLink {
        display: inline-block;
        color: Black;
        background-color: #D6D6D6;
        border-style: None;
        text-decoration: none;
        padding: 2px 15px;
    }

    .autoRenewalLinkOffButton {
        display: inline-block;
        color: Black;
        background-color: #D6D6D6;
        border-style: None;
        text-decoration: none;
        padding: 2px 15px;
    }

    a.autoRenewalLink:hover {
        background-color: #D5E5FF;
    }
</style>
<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />

    <infs:LinkedResource Path="../Resources/Mod/Accessibility/main-accessibility.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Main-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Accessibility/Grid-Accessibility.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    <%--   <infs:LinkedResource Path="~/Resources/Mod/SearchUI/ApplicantPortfolioDetails.js" ResourceType="JavaScript" />--%>
</infs:WclResourceManagerProxy>

<asp:UpdatePanel ID="pnlErrorPkgCompletion" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="msgbox" id="pageMsgBoxPkgCompletion" style="overflow-y: auto; max-height: 400px">
            <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblError"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="container-fluid">

    <%--  <div class="row">
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="row">
                    <%--            <h2 class="header-color" tabindex="0">
                        <asp:Label ID="lblApplicantPortFolioSearch" runat="server" Text="ADB Support Portal" ToolTip="Details pertaining to the student's user account are displayed in this section"></asp:Label>
                    </h2>--%>
    <%--  </div>
            </div>--%>
    <%--    <div class="col-md-6">
                <div class="row text-right">
                    <a runat="server" id="lnkGoBack" onclick="">Back to Search</a>
                </div>
            </div>--%>
    <%-- </div>
    </div>--%>
    <div id="dvSection">
        <div class="row">
            <div class="col-md-12">
                <div id="modcmd_bar">
                    <div id="vermod_cmds">
                        <asp:LinkButton ID="lnkGoBack" runat="server" OnClick="lnkGoBack_Click" Text="Back to Support Portal Search"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-md-12">&nbsp;</div>
    <div class="row">
        <div class='col-md-12'>

            <div class="form-group col-md-1" style="float: right">
                <infs:WclButton runat="server" ID="btnPortfolioDetail" Text="Portfolio" AutoSkinMode="false" Skin="Silk" OnClick="lnkPortfolioDetails_Click"></infs:WclButton>
            </div>
            <div class="form-group col-md-2" style="float: right; text-align: right">
                <infs:WclButton runat="server" ID="WclButton2" Text="Applicant Login" AutoSkinMode="false" Skin="Silk" OnClick="lnkApplicantLogin_Click"></infs:WclButton>
                
            </div>
      

            <%-- <span class="cptn">Institution</span>--%>
            <div id="dvTenants" runat="server" class="form-group col-md-3" style="float: right;">
                <div id="dvCmbTenants" runat="server" class="form-group col-md-9" style="float: right;">
                    <infs:WclComboBox runat="server" ID="cmblstTenants" AutoPostBack="true"
                        DataTextField="TenantName" DataValueField="TenantId" Visible="false"
                        CssClass="form-control" Skin="Silk" AutoSkinMode="false" OnSelectedIndexChanged="cmblstTenants_SelectedIndexChanged" Width="130%">
                    </infs:WclComboBox>

                </div>
                <div id="dvLblTenant" runat="server" style="float: right">
                    <asp:Label ID="lblTenant" runat="server" Text="Institution" Visible="false" CssClass="cptn" Width="100%"></asp:Label>
                </div>
            </div>
        </div>
    </div>


    <asp:Panel ID="Panel1" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Profile Details
                </h2>
            </div>
        </div>

        <infsu:ApplicantPortfolioProfile ID="ucApplicantPortfolioProfile" runat="server" />

    </asp:Panel>
    <%--   <asp:Panel ID="Panel1" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Profile Details
                </h2>
            </div>
        </div>
        <div class="row  allowscroll">

<%--        </div>
    </asp:Panel>--%>

    <asp:Panel ID="Panel2" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Order Details
                </h2>
            </div>
        </div>
        <div class="row  allowscroll">
            <infs:WclGrid ID="grdSupportPortalOrderDetail" AutoGenerateColumns="false" runat="server" AutoSkinMode="true"
                AllowSorting="True" PageSize="50" CellSpacing="0" ShowAllExportButtons="false"
                ShowExtraButtons="true" ShowClearFiltersButton="false" EnableLinqExpressions="false" GridLines="None" OnItemDataBound="grdSupportPortalOrderDetail_ItemDataBound"
                OnNeedDataSource="grdSupportPortalOrderDetail_NeedDataSource" OnItemCommand="grdSupportPortalOrderDetail_ItemCommand">
                <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="False" AllowSorting="false" DataKeyNames="ArchiveStatus,OrderId,BkgOrderID,OrderNumber,PackageSubscriptionID,CompliancePackageStatusCode,PackageTypeCode,OrderStatus,IsOrderItemsComplete,IsServiceGroupStatusComplete,OrderFlag,IsServiceGroupFlagged,ServicreGroupName,OrderStatusCode,IsOrderRenewed"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Order Number" HeaderTooltip="This column displays the Order Number for each record in the grid">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewOrderDetail" runat="server" Text='<%# Eval("OrderNumber") %>' CommandName="ViewOrderDetail" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="InstituteHierarchy" FilterControlAltText="Filter InstituteHierarchy column" HeaderStyle-HorizontalAlign="Center"
                            HeaderText="Order Hierarchy" SortExpression="InstituteHierarchy" UniqueName="InstituteHierarchy"
                            HeaderTooltip="This column displays the Order Hierarchy for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                            HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName"
                            HeaderTooltip="This column displays the Package Name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PackageLabel" FilterControlAltText="Filter PackageLabel column"
                            HeaderText="Package Label" SortExpression="PackageLabel" UniqueName="PackageLabel"
                            HeaderTooltip="This column displays the Package Label for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PaymentStatus" FilterControlAltText="Filter PaymentStatus column"
                            HeaderStyle-Width="125px" HeaderText="Payment Status" SortExpression="PaymentStatus" UniqueName="PackageLabel"
                            HeaderTooltip="This column displays the Payment Status for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ArchiveStatus" FilterControlAltText="Filter ArchiveStatus column"
                            HeaderText="Archive Status" SortExpression="ArchiveStatus" UniqueName="ArchiveStatus"
                            HeaderTooltip="This column displays the Archive Status for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ActiveStatus" FilterControlAltText="Filter ActiveStatus column"
                            HeaderText="Active/Expired Status" SortExpression="ActiveStatus" UniqueName="ActiveStatus"
                            HeaderTooltip=" This column displays the Subscription Expiration Status for each record in the grid.">
                        </telerik:GridBoundColumn>


                        <telerik:GridBoundColumn DataField="OrderDate" FilterControlAltText="Filter OrderDate column"
                            HeaderText="Order Date" SortExpression="OrderDate" UniqueName="OrderDate" DataFormatString="{0:MM/dd/yyyy}"
                            HeaderTooltip="This column displays the Order Date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="OrderPaidDate" FilterControlAltText="Filter OrderPaidDate column"
                            HeaderText="Order Paid Date" SortExpression="OrderPaidDate" UniqueName="OrderPaidDate" DataFormatString="{0:MM/dd/yyyy}"
                            HeaderTooltip="This column displays the Order Paid Date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="OrderCompleteDate" FilterControlAltText="Filter OrderCompleteDate column"
                            HeaderText="Order Completed Date" SortExpression="OrderCompleteDate" UniqueName="OrderCompleteDate" DataFormatString="{0:MM/dd/yyyy}"
                            HeaderTooltip="This column displays the Order Completed Date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="ExceptionPending" FilterControlAltText="Filter ExceptionPending column"
                            HeaderText="Exception Pending" SortExpression="ExceptionPending" UniqueName="ExceptionPending"
                            HeaderTooltip="This column displays the Exception Pending for each record in the grid">
                            <ItemTemplate>
                                <asp:Literal ID="litExceptionPending" runat="server" Text='<%# (Eval("ExceptionPending"))==null?String.Empty :(Convert.ToBoolean(Eval("ExceptionPending")) ? "Yes": "No")%>'></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblOverComplianceStatus" runat="server" Text="" CssClass="error"></asp:Label></span>&nbsp;
                                        <asp:Image ID="imgPackageComplianceStatus" runat="server" CssClass="img_status" />
                                <asp:Image ID="imgOrderStatus" runat="server" Visible="false" />
                                <asp:LinkButton ID="lnkViewDetail" runat="server" Text="Detail(s)" CommandName="ViewDetail" />
                                 <asp:Label  ID="lblPkgRenew" runat="server" Text="" CssClass="error"></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Archive/UnArchive" UniqueName="ArchiveUnArchive">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnArchiveUnArchive" runat="server" Text="Archive"
                                    OnClientClick="return ArhiveOrUnArchiveStatus(this);">
                                </asp:LinkButton>
                                <asp:HiddenField ID="hdnPackageSubscriptionID" runat="server" Value='<%#Eval("PackageSubscriptionID") %>' />
                                <asp:HiddenField ID="hdnBkgOrderID" runat="server" Value='<%#Eval("BkgOrderID") %>' />
                                <asp:HiddenField ID="hdnArchiveStatus" runat="server" Value='<%#Eval("ArchiveStatusCode") %>' />
                                <asp:HiddenField ID="hdnPackageType" runat="server" Value='<%#Eval("PackageType") %>' />
                                <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("OrderID") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="tplcohdr" />
                        </telerik:GridTemplateColumn>


                    </Columns>
                </MasterTableView>
            </infs:WclGrid>
        </div>
    </asp:Panel>

    <infsu:ApplicantRequirementRotations ID="ucApplicantRequirementRotations" runat="server" />

    <asp:Panel ID="pnlInvitationsList" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Manage Invitations
                </h2>
            </div>
        </div>
        <div class="row  allowscroll">
            <infs:WclGrid ID="grdInvitations" AutoGenerateColumns="false" runat="server" AutoSkinMode="true"
                AllowSorting="True" PageSize="10" CellSpacing="0" ShowAllExportButtons="false" MasterTableView-DataKeyNames="ID"
                ShowExtraButtons="true" ShowClearFiltersButton="false" EnableLinqExpressions="false" GridLines="None"
                OnItemDataBound="grdInvitations_ItemDataBound" OnNeedDataSource="grdInvitations_NeedDataSource" OnItemCommand="grdInvitations_ItemCommand">
                <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="False" AllowSorting="false"
                    AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter InviteeName column" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Invitee Name" SortExpression="Name" UniqueName="Name"
                            HeaderTooltip="This column displays the Invitee's name for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EmailAddress" FilterControlAltText="Filter InviteeEmail column" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Email" SortExpression="EmailAddress" UniqueName="EmailAddress"
                            HeaderTooltip="This column displays the Invitee's email for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Phone" FilterControlAltText="Filter Invitee Phone column" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Phone" SortExpression="Phone" UniqueName="Phone"
                            HeaderTooltip="This column displays the Invitee's phone for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Agency" FilterControlAltText="Filter Invitee Agency column" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Agency" SortExpression="Agency" UniqueName="Agency"
                            HeaderTooltip="This column displays the Invitee's Agency for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiration Date" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblExpirationDate" runat="server" Text='<%# Eval("ExpirationDate", "{0:d}") %>' Visible='<%# Eval("IsExpirationDateVisible") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Views Remaining" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblViewsLeft" runat="server" Text='<%# Eval("ViewsRemaining") %>' Visible='<%# Eval("IsExpirationCountVisible") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="InvitationDate" FilterControlAltText="Filter Invitation Date column" DataFormatString="{0:d}"
                            HeaderText="Invitation Date" SortExpression="InvitationDate" UniqueName="InvitationDate" HeaderStyle-HorizontalAlign="Center"
                            HeaderTooltip="This column displays the Invitee's invitation date for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LastViewedDate" FilterControlAltText="Filter Invitee Last Viewed column" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="125px" HeaderText="Last Viewed" SortExpression="LastViewedDate" UniqueName="LastViewedDate"
                            HeaderTooltip="This column displays the Invitee's last view for each record in the grid">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewAttestation" runat="server" Text="View Attestation(s)" CommandName="ViewAttestation" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </infs:WclGrid>
        </div>
    </asp:Panel>

    <div class="col-md-12">&nbsp;</div>
    <asp:Panel ID="pnlNotes" runat="server">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Notes</h2>
            </div>
            <uc:SupportPortalNotes ID="ucSupportPortalNotes" runat="server"></uc:SupportPortalNotes>
        </div>
    </asp:Panel>
</div>
<iframe id="ifrExportDocument" runat="server" height="0" width="0"></iframe>
<asp:HiddenField ID="hdnSelectedTenantID" runat="server" />
<asp:HiddenField ID="hdnCurrentloggedInUserId" runat="server" />
<script type="text/javascript">
  
    function OpenApplicantView(navUrl) {
        var win = window.open(navUrl, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        }
    }
    var dialog = null;
    //UAT-796 -Function to turn on/off Automatic renewal of an applicant.
    function ArhiveOrUnArchiveStatus(sender, eventArgs) {
        var btnID = sender.id;
        if (sender.attributes.Enabled != undefined && sender.attributes.Enabled.value == "false") {
            return false;
        }
        if (confirm('Are you sure you want to Archive/Un-archive this order?')) {
            var containerID = btnID.substr(0, btnID.indexOf("btnArchiveUnArchive"));
            var tenantId = $jQuery("#<%= hdnSelectedTenantID.ClientID %>").val();
            var bkgOrderID = $jQuery("[id$=" + containerID + "hdnBkgOrderID]").val();
            var currentUserID = $jQuery("#<%= hdnCurrentloggedInUserId.ClientID %>").val();
            var packageSubscriptionID = $jQuery("[id$=" + containerID + "hdnPackageSubscriptionID]").val();
            var packageType = $jQuery("[id$=" + containerID + "hdnPackageType]").val();
            var archiveStatus = $jQuery("[id$=" + containerID + "hdnArchiveStatus]").val();
            var urltoPost = "/SearchUI/Default.aspx/ArchiveOrUnArchive";
            var orderId = $jQuery("[id$=" + containerID + "hdnOrderId]").val();
            var dataString = "tenantID : '" + tenantId + "', bkgOrderID : '" + bkgOrderID + "', currentUserID : '" + currentUserID + "', buttonid : '" + btnID + "', packageSubscriptionID : '" + packageSubscriptionID + "', packageType : '" + packageType + "', archiveStatus : '" + archiveStatus + "', orderId : '" + orderId + "'";
            $jQuery.ajax
             (
              {
                  type: "POST",
                  url: urltoPost,
                  data: "{ " + dataString + " }",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function (result) {
                      var data = JSON.parse(result.d);
                      if (data.archiveOrUnArchiveStatus == "true") {

                          $jQuery("#pageMsgBoxPkgCompletion").children("span")[0].innerHTML = data.msg;
                          $jQuery("#pageMsgBoxPkgCompletion").children("span").attr("class", data.msgType);

                          c = "Archive/Un-archive Completed";

                          $jQuery("[id$=pnlErrorPkgCompletion]").hide();

                          $window.showDialog($jQuery("#pageMsgBoxPkgCompletion").clone().show(), {
                              closeBtn: {
                                  autoclose: true, text: "OK",
                                  click: function () {
                                      var grid = $find($jQuery("[id$=grdSupportPortalOrderDetail]")[0].id);
                                      var MasterTable = grid.get_masterTableView();
                                      MasterTable.rebind();
                                  }
                              }
                          }, 750, c);
                      }
                      else {

                          $jQuery("#pageMsgBoxPkgCompletion").children("span")[0].innerHTML = data.msg;
                          $jQuery("#pageMsgBoxPkgCompletion").children("span").attr("class", data.msgType);
                          c = "Unable to archive/un-archive";

                          $jQuery("[id$=pnlErrorPkgCompletion]").hide();

                          $window.showDialog($jQuery("#pageMsgBoxPkgCompletion").clone().show(), {
                              closeBtn: {
                                  autoclose: true, text: "OK",
                                  click: function () {
                                      var grid = $find($jQuery("[id$=grdSupportPortalOrderDetail]")[0].id);
                                      var MasterTable = grid.get_masterTableView();
                                      MasterTable.rebind();
                                  }

                              }
                          }, 750, c);
                      }
                  }
              });

            return false;
        }
        else {
            return false;
        }
    }
</script>
