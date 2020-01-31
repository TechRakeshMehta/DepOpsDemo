<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FeatureActionList.aspx.cs" Inherits="CoreWeb.IntsofSecurityModel.Views.FeatureActionList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
 
<script type="text/javascript">
    // To close the popup.
    function ClosePopup() {
        top.$window.get_radManager().getActiveWindow().close();
    }

    function HideCollapseAll()
    {
        $jQuery('.rtlCollapse').hide();
    }
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HeadUserControlFolderList" runat="server">
    <title>List For Feature Actions</title>
</head>
<body>
    <form id="formFeatureActionList" runat="server">
        <asp:ScriptManager ID="scmMain" runat="server" ScriptMode="Release">
        </asp:ScriptManager>
        <infs:WclResourceManager ID="ActionListManager" runat="server">
           <%-- <infs:LinkedResource Path="~/Resources/Generic/popup.css" ResourceType="StyleSheet" />--%>
            <infs:LinkedResource Path="~/Resources/Generic/popup.js" ResourceType="JavaScript" />
        </infs:WclResourceManager>
        <div class="popupContent">
            <div class="swrap">
                <infs:WclTreeList runat="server" ID="treeListFeatureAction" OnItemDataBound="treeListFeatureAction_ItemDataBound" OnNeedDataSource="treeListFeatureAction_NeedDataSource" AllowPaging="false" AllowSorting="false" DataKeyNames="NodeId" ParentDataKeyNames="ParentId"
                    AutoGenerateColumns="false" AllowMultiItemSelection="true" ClientSettings-Scrolling-AllowScroll="true" Height="510px">
                    <Columns>
                        <telerik:TreeListBoundColumn DataField="Name" UniqueName="Name"
                            HeaderText="Feature Name" />
                        <telerik:TreeListBoundColumn DataField="Code" UniqueName="Code" Visible="false"
                            HeaderText="Feature Code" />
                        <telerik:TreeListBoundColumn DataField="NodeId" UniqueName="NodeId"
                            HeaderText="Feature NodeId" Visible="false" />
                        <telerik:TreeListTemplateColumn HeaderStyle-Width="100px" HeaderText="Full Access" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:RadioButton ID="rbFeatureFullAccess" GroupName="permission" runat="server" Visible='<%# (Eval("Code").Equals("AAAD")) ? true : false %>' />
                            </ItemTemplate>
                        </telerik:TreeListTemplateColumn>

                        <telerik:TreeListTemplateColumn HeaderStyle-Width="100px" HeaderText="Read Only" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:RadioButton ID="rbFeatureReadOnly" runat="server" GroupName="permission" Visible='<%# (Eval("Code").Equals("AAAD")) ? true : false %>' />
                            </ItemTemplate>
                        </telerik:TreeListTemplateColumn>

                        <telerik:TreeListTemplateColumn HeaderStyle-Width="100px" HeaderText="No Access" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:RadioButton ID="rbFeatureNoAccess" runat="server" GroupName="permission" Visible='<%# (Eval("Code").Equals("AAAD")) ? true : false %>' />
                            </ItemTemplate>
                        </telerik:TreeListTemplateColumn>
                    </Columns>
                    <ClientSettings AllowPostBackOnItemClick="false">
                    </ClientSettings>
                </infs:WclTreeList>

            </div>
            <infsu:CommandBar ID="fsucFeatureActionList" runat="server" AutoPostbackButtons="Save" OnSaveClick="fsucFeatureActionList_SaveClick" DisplayButtons="Save,Cancel" CancelButtonText="Cancel"
                ButtonPosition="Right" CauseValidationOnCancel="false" OnCancelClientClick="ClosePopup" />
        </div>
    </form>
</body>
</html>

