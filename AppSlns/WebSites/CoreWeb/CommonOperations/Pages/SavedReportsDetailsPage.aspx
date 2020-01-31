<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SavedReportsDetailsPage.aspx.cs"
    Inherits="CoreWeb.CommonOperations.Views.SavedReportsDetailsPage"
    MasterPageFile="~/Shared/ChildPage.master" Title="CommonOperations" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ReportViewer" Src="~/Reports/UserControl/ReportViewer.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*.RadComboBox RadComboBox_Outlook
        {
            width: 100% !important;
        }*/
        .RadComboBoxDropDown.rcbAutoWidth
        {
            min-width: 158px !important;
            max-width: 660px !important;
        }

        .section, .tabvw
        {
            margin-bottom: 0px !important;
            padding-top: 0px !important;
            padding-bottom: 0px !important;
        }
  
        /*html, body
        {
            overflow: hidden;
            margin: auto;
            height: 100%;
            width: 100%;
        }*/

        /*.frmRptVwr
        {
            height: 3vh !important;
            width: 100% !important;
            padding-bottom: 3px;
            padding-top: 3px;
        }*/

        /*.content-wrapper
        {
            position: relative !important;
            width: 75% !important;
            margin: 0 auto !important;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxSavedReportsDetailsPage">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
         <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
          </infs:WclResourceManagerProxy>
    <script type="text/javascript" language="javascript">
        $jQuery(document).ready(function () {
            parent.ResetTimer();
        });

        function KeyPress(sender, args) {
            if (args.get_keyCharacter() == sender.get_numberFormat().DecimalSeparator || args.get_keyCharacter() == '-') {
                args.set_cancel(true);
            }
        }

        function RefrshTree() {
            var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
            btn.click();
        }

        $jQuery(window).load(function () {
            setIframeHeight($jQuery('[id$=ifrDetails]', $jQuery(parent.theForm)));
        });

        function setIframeHeight(iframe) {
            if (iframe.length > 0) {
                var iframeReport = $jQuery('[id$=iframeReportViewer]');
                var iframeWin = iframe[0].contentWindow || iframe[0].contentDocument.parentWindow;
                if (iframeWin.document.body) {
                    var height = iframeWin.document.documentElement.scrollHeight || iframeWin.document.body.scrollHeight;
                    if (iframeReport.length > 0) {
                        iframeReport[0].height = height;
                    }

                }
            }
        };

        function DeleteConfirmations(sender) {

            var message = "Are you sure, you want to delete the report parameters?"
            $confirm(message, function (res) {
                if (res) {
                   
                    __doPostBack("<%= RemoveBtn.UniqueID %>", "");

                }

            }, 'Complio', true);

        }
      
        //function setIframeHeight(ifrm) {
        //    var doc = ifrm[0].contentDocument ? ifrm[0].contentDocument :
        //        ifrm[0].contentWindow.document;
        //    ifrm[0].style.visibility = 'hidden';
        //    ifrm[0].style.height = "10px"; // reset to minimal height ...
        //    // IE opt. for bing/msn needs a bit added or scrollbar appears
        //    ifrm[0].style.height = getDocHeight(doc) + 4 + "px";
        //    ifrm[0].style.visibility = 'visible';
        //};

        //function getDocHeight(doc) {
        //    doc = doc || document;
        //    // stackoverflow.com/questions/1145850/
        //    var body = doc.body, html = doc.documentElement;
        //    var height = Math.max(body.scrollHeight, body.offsetHeight,
        //        html.clientHeight, html.scrollHeight, html.offsetHeight);
        //    return height;
        //} 
    </script>

    <div class="container-fluid" id="divMain" runat="server">
        <div class="msgbox">
            <asp:Label ID="lblMessage" runat="server" CssClass="info"> </asp:Label>
        </div>
        <div  id="divFavParameter" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color">
                        <asp:Label ID="lblFavParamName" runat="server" Text=""></asp:Label>
                    </h2>
                </div>
            </div>
            <div id="divShowNode" class="row bgLightGreen" runat="server">
                <asp:Panel ID="pnlFavParameters" runat="server">
                    <div class='col-md-12'>
                        <div class="row">
                            <div class='form-group col-md-3'>
                                <asp:Label ID="lblName" runat="server" Text="Name" CssClass="cptn"></asp:Label><span
                                    class="reqd">*</span>
                                <infs:WclTextBox runat="server" Width="100%" CssClass="form-control" ID="txtName"
                                    MaxLength="100">
                                </infs:WclTextBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Name is required." ValidationGroup='grpSave' />
                                </div>
                            </div>
                            <div class='form-group col-md-3'>
                                <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="cptn"></asp:Label>
                                <infs:WclTextBox runat="server" Width="100%" CssClass="form-control" ID="txtDescription"
                                    MaxLength="500">
                                </infs:WclTextBox>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>

        <div  id="divParamValues" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color">
                        <asp:Label ID="lblParamValues" runat="server" Text="Parameter Values"></asp:Label>
                    </h2>
                </div>
            </div>
            <div id="div2" class="row bfLightGreen" runat="server">
                <asp:Panel ID="pnlParamValues" CssClass="col-md-12" runat="server">
                </asp:Panel>
            </div>
        </div>
        <asp:HiddenField ID="hdnSelectedInstitutionID" runat="server" />
        <asp:HiddenField ID="hdnSelectedNodeIDs" runat="server" />
        <asp:HiddenField ID="hdnSelectedCategoryIDs" runat="server" />
        <asp:LinkButton ID="RemoveBtn" runat="server" Style="display: none;" OnClick="RemoveBtn_Click"></asp:LinkButton>
        <infsu:CommandBar ID="fsucCmdBarParameter" runat="server" DisplayButtons="Clear,Save,Submit"
            AutoPostbackButtons="Save,Submit" SaveButtonText="Update" UseAutoSkinMode="false"
            ButtonSkin="Silk"
            SubmitButtonText="View Report" SubmitButtonIconClass="rbNext" ValidationGroup="grpSave"
            OnSaveClick="fsucCmdBarParameter_SaveClick" SaveButtonIconClass="rbSave"
            OnSubmitClick="fsucCmdBarParameter_SubmitClick"
            ClearButtonText="Delete" ClearButtonIconClass="rbArchive" 
            OnClearClientClick="DeleteConfirmations"   
            />

        <div id="divReport" runat="server" visible="false">
            <div  runat="server">
                <div class="row">
                    <div class="col-md-12">
                        <h2 class="header-color">
                            <asp:Label ID="lblReport" runat="server" Text="Report"></asp:Label>
                        </h2>
                    </div>
                </div>
            </div>
            <iframe id="iframeReportViewer" runat="server" width="100%" height="100%"></iframe>
        </div>
    </div>
</asp:Content>


