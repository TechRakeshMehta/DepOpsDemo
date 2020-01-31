<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisclosureAndReleaseForm.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.DisclosureAndReleaseForm" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxManageInvitationExpiration">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style>
    .classImagePdf {
        background: url(../../images/medium/pdf.gif);
        background-position: 0 0;
        background-repeat: no-repeat;
        width: 20px;
        height: 20px;
    }
</style>
<script type="text/javascript">

    //Added isAdditionalDoc in UAT-3745
    function openPdfPopUp(sender, isAdditionalDoc) {
        var btnID = $jQuery(sender)[0].id;
        //UAT-1923        
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "-1");
        $jQuery("[id$=hdnCurrentClicked]").val(btnID);
        var tenantId = $jQuery("[id$=hdnTenantId]").val();

        var containerID = btnID.substr(0, btnID.indexOf("btnDnrPdf"));
        var hdnApplicantDocumentId = $jQuery("[id$=" + containerID + "hdnfApplicantDocumentId]").val();
        var documentType = "DisclosureReleaseDocument";
        var composeScreenWindowName = "D & R Details";

        if (isAdditionalDoc != undefined && isAdditionalDoc != null && isAdditionalDoc) {
            containerID = btnID.substr(0, btnID.indexOf("btnAdditionalDoc"));
            hdnApplicantDocumentId = $jQuery("[id$=" + containerID + "hdnAdditionalApplicantDocumentId]").val();
            documentType = "AdditionalDocument";
            composeScreenWindowName = "Additional Document";
        }

        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/BkgOperations/Pages/DisclosureReleaseDocViewer.aspx?ApplicantDocumentId=" + hdnApplicantDocumentId + "&DocumentType=" + documentType + "&TenantID=" + tenantId);
        var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            winopen = false;
        }
        //UAT-1923
        var currentLinkFocus = $jQuery("[id$=hdnCurrentClicked]").val();
        window.parent.parent.$jQuery("[id$=dvSkipContent]").attr("tabindex", "0");
        if (currentLinkFocus != undefined && currentLinkFocus != null && currentLinkFocus != "") {
            setTimeout(function () { $jQuery("[id$=" + currentLinkFocus + "]").focus(); }, 500);
            $jQuery("[id$=hdnCurrentClicked]").val("");
        }
    }

</script>
<asp:HiddenField ID="hdnTenantId" runat="server" Value='' />
<asp:HiddenField ID="hdnCurrentClicked" Value="" runat="server" />
<div class="container-fluid">
    <div id="dvDandADocs">
        <div class="row">
            <div class="col-md-12">
                <h1 class="header-color" tabindex="0">Disclosure and Authorization</h1>
            </div>
        </div>
        <div class="row">


            <infs:WclGrid CssClass="removeExtraSpace" runat="server" ID="grdDNR" AutoGenerateColumns="false"
                AllowSorting="True" AutoSkinMode="True" CellSpacing="0" EnableAriaSupport="true"
                GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" OnNeedDataSource="grdDNR_NeedDataSource" AllowPaging="false" EnableDefaultFeatures="false">
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ApplicantDocumentID" AllowPaging="false" PagerStyle-Visible="false" AllowSorting="false" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Form" UniqueName="imgServiceForm">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnDnrPdf" ToolTip="Click here to view Disclosure & Authorization form attached to this order" AutoPostBack="false" CssClass="classImagePdf"
                                    runat="server" Font-Underline="true">
                                    <Image EnableImageButton="true" />
                                </telerik:RadButton>
                                <asp:HiddenField ID="hdnfApplicantDocumentId" runat="server" Value='<%#Eval("ApplicantDocumentID")%>' />

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter FileName column" AllowFiltering="false"
                            HeaderText="Document Name" SortExpression="FileName" UniqueName="DocumentName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column" AllowFiltering="false"
                            HeaderText="Description" SortExpression="Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                    </Columns>

                </MasterTableView>
            </infs:WclGrid>

            <div class="gclr">
            </div>

        </div>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row">&nbsp;</div>

    <div id="dvAdditionalDocs" runat="server" style="display: none">
        <div class="row">
            <div class="col-md-12">
                <h1 class="header-color" tabindex="0">Additional Document(s)</h1>
            </div>
        </div>

        <div class="row">
            <infs:WclGrid CssClass="removeExtraSpace" runat="server" ID="grdAdditionalDocuments" AutoGenerateColumns="false"
                AllowSorting="True" AutoSkinMode="True" CellSpacing="0" EnableAriaSupport="false"
                GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" OnNeedDataSource="grdAdditionalDocuments_NeedDataSource" OnPreRender="grdAdditionalDocuments_PreRender"
                AllowPaging="false" EnableDefaultFeatures="false">
                <MasterTableView CommandItemDisplay="None" DataKeyNames="ApplicantDocumentID" AllowPaging="false" PagerStyle-Visible="false" AllowSorting="false" AllowFilteringByColumn="false">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="BkgServiceName" FilterControlAltText="Filter BkgServiceName column" AllowFiltering="false"
                            HeaderText="Service Name" SortExpression="BkgServiceName" UniqueName="BkgServiceName">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Form" UniqueName="imgServiceForm">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnAdditionalDoc" ToolTip="Click here to view additional document attached to this service." AutoPostBack="false" CssClass="classImagePdf"
                                    runat="server" Font-Underline="true">
                                    <Image EnableImageButton="true" />
                                </telerik:RadButton>
                                <asp:HiddenField ID="hdnAdditionalApplicantDocumentId" runat="server" Value='<%#Eval("ApplicantDocumentID")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter FileName column" AllowFiltering="false"
                            HeaderText="Document Name" SortExpression="FileName" UniqueName="DocumentName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column" AllowFiltering="false"
                            HeaderText="Description" SortExpression="Description" UniqueName="Description">
                        </telerik:GridBoundColumn>
                    </Columns>

                </MasterTableView>
            </infs:WclGrid>
            <div class="gclr">
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function pageLoad() {
        $jQuery("[id$=btnDnrPdf]").click(function () {
            openPdfPopUp(this, false);
        });

        //UAT-3745
        $jQuery("[id$=btnAdditionalDoc]").click(function () {
            openPdfPopUp(this, true);
        });
    }
</script>
