<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderSummary.ascx.cs"
    Inherits="CoreWeb.BkgOperations.Views.BkgOrderSummary" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style>
    .hlink {
        cursor: pointer;
    }
</style>
<script type="text/javascript">
    var winopen = false;
    function openEDSDocumentByOrderID(sender) {
        //UAT-1923
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "-1");
        $jQuery("[id$=hdnCurrentClicked]").val(sender.id);
        var tenantId = $jQuery("#<%= hdfTenantId.ClientID %>").val()
        var orderId = $jQuery("#<%= hdfOrderID.ClientID %>").val();
        var documentType = $jQuery("#<%= hdfDocumentType.ClientID %>").val();
        var composeScreenWindowName = "Electronic Drug Screening Form";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + orderId + "&DocumentType=" + documentType + "&tenantId=" + tenantId);
        var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            winopen = false;
        }
        //UAT-1923
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "0");
        var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
        if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
            setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 500);
            $jQuery("[id$=hdnCurrentClicked]").val("");
        }
    }

    function pageLoad() {
      

        $jQuery("[id$=lnkEDSDocument]").attr("tabindex", "0");
        $jQuery("[id$=lnkEDSDocument]").attr("onkeypress", "openEDSDocumentByOrderID(this)");
        $jQuery("[id$=cmbPaymentType]").attr('tabindex', '0');
        $jQuery("[id$=cmbSvcGroupReviewStatus]").attr('tabindex', '0');
    }
</script>
<infs:WclResourceManagerProxy runat="server" ID="rprxBkgOrderSummary">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <%--<asp:Label ID="Label1" runat="server" Text="Package Order Summary"></asp:Label>--%>
                <span id="Label1" tabindex="0">Package Order Summary</span>
            </h2>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <%--<asp:Label ID="lblOrderSummary" runat="server" Text="Order Information" TabIndex="0"></asp:Label>--%>
                <span id="lblOrderSummary" tabindex="0">Order Information</span>
            </h2>
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlOrderSummary">
        <div class="row bgLightGreen">
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Institution Hierarchy</span>--%>
                <label for="<%= txtHierarchy.ClientID %>" class="cptn">Institution Hierarchy</label>
                <infs:WclTextBox runat="server" ID="txtHierarchy" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Order Number</span>--%>
                <label for="<%= txtOrderId.ClientID %>" class="cptn">Order Number</label>

                <infs:WclTextBox runat="server" ID="txtOrderId" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Order Status</span>--%>
                <label for="<%= txtOrderStatus.ClientID %>" class="cptn">Order Status</label>


                <infs:WclTextBox runat="server" ID="txtOrderStatus" ReadOnly="true" Width="100%"
                    CssClass="form-control">
                </infs:WclTextBox>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Date Created</span>--%>
                <label for="<%= txtDateCreated.ClientID %>" class="cptn">Date Created</label>


                <infs:WclTextBox runat="server" ID="txtDateCreated" ReadOnly="true" Width="100%"
                    CssClass="form-control">
                </infs:WclTextBox>
            </div>
        </div>
        <div class="row bgLightGreen">
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Date Paid</span>--%>
                <label for="<%= txtpaidDate.ClientID %>" class="cptn">Date Paid</label>


                <infs:WclTextBox runat="server" ID="txtpaidDate" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Date Completed</span>--%>
                <label for="<%= txtDateCompleted.ClientID %>" class="cptn">Date Completed</label>


                <infs:WclTextBox runat="server" ID="txtDateCompleted" ReadOnly="true" Width="100%"
                    CssClass="form-control">
                </infs:WclTextBox>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Payment Method</span>--%>
                <label for="<%= cmbPaymentType.ClientID %>_Input" class="cptn">Payment Method</label>


                <%--  <infs:WclTextBox runat="server" ID="txtPaymentMethod" Enabled="false">
                        </infs:WclTextBox>--%>
                <infs:WclComboBox ID="cmbPaymentType" DataValueField="PaymentOptionID" DataTextField="PaymentOption"
                    runat="server" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                </infs:WclComboBox>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Category</span>--%>
                <label for="<%= txtCategory.ClientID %>" class="cptn">Category</label>

                <infs:WclTextBox runat="server" ID="txtCategory" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
        </div>


        <%-- :>Commented below code on 17/07/2014 by Sachin Singh  as discussed with Rajesh Kumar
                         Because this link was not worked for Client Admin--%>
        <%-- <div class='form-group col-md-3'>
                        <span class='cptn'>Disclosure and Authorization</span>
                    </div>
                    <div class='sxlm'>
                        <asp:HyperLink ID="lnkDisAndRels" runat="server" Enabled="true" Text="Disclosure and Authorization Report"
                            Visible="true" Font-Underline="true" BackColor="Transparent" BorderStyle="None" ForeColor="Black" Target="_blank" onclick="openEDSDocumentByOrderID(this)" CssClass="hlink">
                        </asp:HyperLink>
                    </div>--%>
        <div class="row bgLightGreen">
            <div id="divEDSDocumentLink" runat="server" visible="false">
                <div class='form-group col-md-12'>
                    <%--<span class='cptn'>Electronic Drug Screen</span>--%>
                    <label for="<%= lnkEDSDocument.ClientID %>" class="cptn">Electronic Drug Screen</label>

                    <asp:HyperLink ID="lnkEDSDocument" runat="server" Enabled="true" Text="Electronic Drug Screen Report"
                        Visible="true" Target="_blank" onclick="openEDSDocumentByOrderID(this)" CssClass="form-control blueText"
                        Width="100%">
                    </asp:HyperLink>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--UAT-860
            <div class="swrap">
            <infs:WclGrid runat="server" ID="grdOrderNotes" AllowPaging="false" AutoGenerateColumns="False"
                AllowSorting="false" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false"
                GridLines="None">
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ONTS_ID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ONTS_NoteText" FilterControlAltText="Filter Test column"
                            HeaderText="Notes" SortExpression="ONTS_NoteText" UniqueName="ONTS_NoteText">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>--%>

    <div id="dvServiceGroup" runat="server" visible="false">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">Service Group Details</h2>
            </div>
        </div>
        <div class="row bgLightGreen">
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Service Group Name</span>--%>
                <label for="<%= lblSvcGroupName.ClientID %>" class="cptn">Service Group Name</label>

                <asp:Label ID="lblSvcGroupName" runat="server" Text=""></asp:Label>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Review Status</span>--%>
                <label for="<%= cmbSvcGroupReviewStatus.ClientID %>_Input" class="cptn">Review Status</label>

                <infs:WclComboBox ID="cmbSvcGroupReviewStatus" runat="server" DataTextField="BSGRS_ReviewStatusType"
                    DataValueField="BSGRS_ReviewCode" EmptyMessage="--Select--" ValidationGroup="grpOrderStatus">
                </infs:WclComboBox>
                <div class="vldx">
                    <asp:RequiredFieldValidator runat="server" ID="rfvSvcGroupReviewStatus" ControlToValidate="cmbSvcGroupReviewStatus"
                        Display="Dynamic" ValidationGroup="grpOrderStatus" CssClass="errmsg"
                        Text="Service Group Review Status is required." />
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <%--<asp:Label ID="lblPersonInfo" runat="server" Text="Person Information"></asp:Label>--%>
                <span id="lblPersonInfo" tabindex="0">Person Information</span>
            </h2>
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlPersonInfo">
        <div class="row bgLightGreen">
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Applicant Name</span>--%>
                <label for="<%= txtApplicantName.ClientID %>" class="cptn">Applicant Name</label>

                <infs:WclTextBox runat="server" ID="txtApplicantName" ReadOnly="true" Width="100%"
                    CssClass="form-control">
                </infs:WclTextBox>
            </div>
            <div id="divSSN" runat="server">
                <div class='form-group col-md-3'>
                    <%--<span class='cptn'>SSN</span>--%>
                    <label for="<%= txtSSN.ClientID %>" class="cptn">SSN</label>

                    <infs:WclMaskedTextBox ID="txtSSN" runat="server" ReadOnly="true" Width="100%" CssClass="form-control"
                        Mask="###-##-####" />
                </div>
            </div>
            <div id="divSSNMasked" visible="false" runat="server">
                <div class='form-group col-md-3'>
                    <%--<span class='cptn'>SSN</span>--%>
                    <label for="<%= txtSSNMAsked.ClientID %>" class="cptn">SSN</label>

                    <infs:WclTextBox ID="txtSSNMAsked" runat="server" ReadOnly="true" Width="100%" CssClass="form-control">
                    </infs:WclTextBox>
                </div>
            </div>
            <div id="divDOB" runat="server">
                <div class='form-group col-md-3'>
                    <%--<span class='cptn'>Date of Birth</span>--%>
                    <label for="<%= txtDOB.ClientID %>" class="cptn">Date of Birth</label>

                    <infs:WclTextBox runat="server" ID="txtDOB" ReadOnly="true" Width="100%" CssClass="form-control">
                    </infs:WclTextBox>
                </div>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Gender</span>--%>
                <label for="<%= txtGender.ClientID %>" class="cptn">Gender</label>

                <infs:WclTextBox runat="server" ID="txtGender" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
        </div>
    </asp:Panel>

    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <%--<asp:Label ID="lblAddressInfo" runat="server" Text="Address Information"></asp:Label>--%>
                <span id="lblAddressInfo" tabindex="0">Address Information</span>
            </h2>
        </div>
    </div>
    <asp:Panel runat="server" ID="pnllAddressInfo">
        <div class="row bgLightGreen">
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Address1</span>--%>
                <label for="<%= txtAddress1.ClientID %>" class="cptn">Address1</label>

                <infs:WclTextBox runat="server" ID="txtAddress1" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Address2</span>--%>
                <label for="<%= txtAddress2.ClientID %>" class="cptn">Address2</label>

                <infs:WclTextBox runat="server" ID="txtAddress2" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>City</span>--%>
                <label for="<%= txtCity.ClientID %>" class="cptn">City</label>

                <infs:WclTextBox runat="server" ID="txtCity" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>State</span>--%>
                <label for="<%= txtState.ClientID %>" class="cptn">State</label>

                <infs:WclTextBox runat="server" ID="txtState" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
        </div>
        <div class="row bgLightGreen">
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Zip Code</span>--%>
                <label for="<%= txtZipCode.ClientID %>" class="cptn">Zip Code</label>

                <infs:WclTextBox runat="server" ID="txtZipCode" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Phone</span>--%>
                <label for="<%= txtPhone.ClientID %>" class="cptn">Phone</label>

                <infs:WclMaskedTextBox ID="txtPhone" runat="server" ReadOnly="true" Width="100%"
                    CssClass="form-control" Mask="(###)-###-####" />
            </div>
            <div class='form-group col-md-3'>
                <%--<span class='cptn'>Email</span>--%>
                <label for="<%= txtEmail.ClientID %>" class="cptn">Email</label>

                <infs:WclTextBox runat="server" ID="txtEmail" ReadOnly="true" Width="100%" CssClass="form-control">
                </infs:WclTextBox>
            </div>
        </div>

    </asp:Panel>

</div>
<asp:HiddenField ID="hdfTenantId" runat="server" />
<asp:HiddenField ID="hdfOrderID" runat="server" />
<asp:HiddenField ID="hdfDocumentType" runat="server" />
<asp:HiddenField ID="hdnCurrentClicked" Value="" runat="server" />
