<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.CommonOperations.Views.SavedReportsPage" CodeBehind="SavedReportsPage.aspx.cs"
    MasterPageFile="~/Shared/DefaultMaster.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/PageBreadCrumb.ascx" TagName="PageBreadCrumb" TagPrefix="infsu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadContent" runat="server">
    <style type="text/css">
        html, body, #frmmod, #UpdatePanel1, #box_content {
            height: 100% !important;
        }

        .child_pageframe {
            width: 99%;
            height: 99%;
        }

        .pane-client {
            background-color: transparent;
            height: 40px;
            position: absolute;
            top: 0;
            right: 10px;
        }

        .main_drop {
            width: 457px;
            margin-top: 12px;
            color: #000;
        }

        .tree-pane {
            padding-top: 10px;
        }

            .tree-pane a, .tree-pane a:visited {
                color: Black;
                font-family: Arial;
            }

        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxSetUp">
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/CommonOperations/SavedReports.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/Breadcrumb.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

    </infs:WclResourceManagerProxy>
                
    <infs:WclSplitter ID="sptrContentPage" runat="server" LiveResize="true" Orientation="Horizontal"
        Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
        <infs:WclPane ID="pnMainToolbar" runat="server" Height="50" Width="100%" Scrolling="None"
            Collapsed="false">
            <div class="container-fluid" id="dvSharedUserBreadcrumb">
                <div class="col-md-12">
                    <div class="row">
                        <h1 class="text-right">
                            <asp:Label Text="Saved Reports" runat="server" ID="lblModHdr" />
                            &nbsp;<asp:Label Text="Saved Reports Parameters" runat="server" ID="lblPageHdr" CssClass="phdr" />
                        </h1>
                    </div>
                    <div class="breadcrumb padbott10 text-right">
                        <infsu:PageBreadCrumb ID="breadcrum" runat="server" />
                    </div>
                
                </div>
           
            </div>
            
             
        </infs:WclPane>

        <infs:WclPane ID="pnMain" runat="server" Scrolling="None" Width="100%">
           
            <infs:WclSplitter ID="sptrPage" runat="server" LiveResize="true" Orientation="Vertical"
                Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
                <infs:WclPane ID="pnUpper" runat="server" Height="100%" Width="300" MinWidth="300"
                    Scrolling="Y" Collapsed="false">
                    <asp:UpdatePanel runat="server" ID="updpnlTree" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnDefaultScreen" runat="server" />
               
                            <infs:WclButton runat="server" ID="chkAllParameters" Skin="Silk" Text="Check All" 
                                ToolTip="Check All Reports" OnClick="chkAllParameters_Click"  Icon-PrimaryIconCssClass="rbReview" AutoSkinMode="false"></infs:WclButton>
                            <asp:LinkButton ID="hdnRemoveBtn" runat="server" Style="display: none;" OnClick="btnRemove_Click"></asp:LinkButton>
                            <infs:WclButton ID="hdnRemoveDisable" Style="display: none;"   runat="server" OnClick="hdnRemoveDisable_Click"></infs:WclButton>
                            <infs:WclButton ID="btnRemove" Text="Delete" Icon-PrimaryIconCssClass="rbArchive" ToolTip="Delete Saved Report Parameters" 
                                 Skin="Silk" AutoSkinMode="false"  runat="server" OnClientClicked="DeleteConfirmations" AutoPostBack="false"></infs:WclButton>
                            <asp:HiddenField runat="server" ID="hdnIschkParameters" Value="false" />
                            <asp:HiddenField ID="hdnSelectedDeletedNode" runat="server" Value="" />
                            <!--OnNodeCheck="treeFavParameters_NodeCheck"-->
                            <infs:WclTreeView ID="treeFavParameters" runat="server" ClientKey="treeFavParameters"
                                OnNodeDataBound="treeFavParameters_NodeDataBound" AutoSkinMode="false" Skin="Silk"
                                OnClientLoad="SetTreeNode" OnClientNodeClicked="GetSelectedNode" OnClientNodeChecked="ClientNodeChecked" CheckBoxes="true">
                            </infs:WclTreeView>
                            <asp:Button ID="btnUpdateTree" runat="server" UseSubmitBehavior="false" CssClass="btnUpdateTree" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnUpdateTree" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </infs:WclPane>
                <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
                </infs:WclSplitBar>
           
                <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%" Height="100%">
               <div class="msgbox" id="pageMsgBoxSchuduleInv"  style="overflow-y: auto; max-height: 400px">
                <asp:Label CssClass="info" EnableViewState="false"  runat="server" ID="lblError"></asp:Label>
            </div>
                    <iframe name="childpageframe" id="ifrDetails" runat="server" class="child_pageframe"
                        src=""></iframe>
                    <asp:HiddenField ID="hdnSelectedNode" runat="server" Value="" />


                </infs:WclPane>
            </infs:WclSplitter>
        </infs:WclPane>
    </infs:WclSplitter>
    <script type="text/jscript">

        function ClientNodeChecked(sender, eventArgs) {
            var deletenodeIds = new Array();
            debugger;
            if ($jQuery("#<%=hdnSelectedDeletedNode.ClientID %>").val() != "") {
                deletenodeIds = $jQuery("#<%=hdnSelectedDeletedNode.ClientID %>").val().split(',');
            }
            var node = eventArgs.get_node();
            val = node.get_value();
            if (node.get_checked()) {
                deletenodeIds.push(val);
            }
            else {
                $jQuery("#<%=hdnIschkParameters.ClientID %>").val("False");
                deletenodeIds = $jQuery.grep(deletenodeIds, function (value) {
                    return value != val;
                });
            }
            $jQuery("#<%=hdnSelectedDeletedNode.ClientID %>").val(deletenodeIds);

            __doPostBack("<%= hdnRemoveDisable.UniqueID %>", "");


        }

        function DeleteConfirmations(sender) {
          
            var message = "Are you sure, you want to delete the report parameters?"
            $confirm(message, function (res) {
                if (res) {
                    $jQuery("#<%=ifrDetails.ClientID %>").attr('src', 'about:blank');
                    __doPostBack("<%= hdnRemoveBtn.UniqueID %>", "");

                }

            }, 'Complio', true);

        }

    </script>
</asp:Content>

