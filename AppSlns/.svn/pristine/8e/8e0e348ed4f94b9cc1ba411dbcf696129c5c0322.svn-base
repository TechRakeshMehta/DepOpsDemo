<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.BkgSetup.Views.DisclosureDocuments" CodeBehind="DisclosureDocuments.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagName="UploadDisclosureDocuments" TagPrefix="infsu" Src="~/BkgSetup/UserControl/UploadDisclosureDocuments.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxUpload">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/upload.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<style>
    .classImageMail {
        background: url(../../App_Themes/Default/images/mail.png);
        background-position: 0 0;
        background-repeat: no-repeat;
        width: 20px;
        height: 20px;
    }

    .classImagePdf {
        background: url(../../images/medium/pdf.gif);
        background-position: 0 0;
        background-repeat: no-repeat;
        width: 20px;
        height: 20px;
    }
      .rbLinkButton {
    height: 40px !important;
    }
</style>
<script type="text/javascript">

    var winopen = false;

    function openPdfPopUp(sender) {
        var btnID = sender.get_id();
        var containerID = btnID.substr(0, btnID.indexOf("btnNotificationPdf"));
        var hdnfSystemDocumentId = $jQuery("[id$=" + containerID + "hdnfSystemDocumentId]").val();
        var documentType = "ServiceFormDocument";
        var composeScreenWindowName = "Service Form Details";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?systemDocumentId=" + hdnfSystemDocumentId + "&DocumentType=" + documentType);
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
    }

</script>
<h1 class="mhdr">Manage D&A/Additional Documents</h1>

<div class="section">
    <div class="content">
        <div class="sxform auto">
            <div class="sxpnl">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Document Type</span><span class="reqd">*</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox runat="server" ID="cmbDocumentType" AutoPostBack="true" OnDataBound="cmbDocumentType_DataBound"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnSelectedIndexChanged="cmbDocumentType_SelectedIndexChanged">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvDocType" ControlToValidate="cmbDocumentType"
                                class="errmsg" ValidationGroup="grpUploadDoc" Display="Dynamic" ErrorMessage="Document Type is required."
                                InitialValue="--Select--" />
                        </div>
                    </div>

                    <div runat="server" id="dvIsOperational" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Is Operational</span>
                        </div>
                        <div class='sxlm'>
                            <asp:CheckBox ID="chkIsOperational" Text="Is Operational" runat="server" Width="100%" AutoPostBack="false" />
                        </div>
                        <div class='sxlb'>
                            <span class="cptn">Send To Student</span>
                        </div>
                        <div class='sxlm'>
                            <asp:CheckBox ID="chkSendToStudent" Text="Send To Student" runat="server" Width="100%" AutoPostBack="false" />
                        </div>
                    </div>
                    <div runat="server" id="dvAgeGroupType" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">Age Group</span><span class="reqd">*</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox runat="server" ID="cmbAgeGroups" AutoPostBack="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                            </infs:WclComboBox>
                            <div class="vldx">
                                <asp:RequiredFieldValidator runat="server" ID="rfv_AgeGroups" ControlToValidate="cmbAgeGroups" Enabled="false"
                                    class="errmsg" ValidationGroup="grpUploadDoc" Display="Dynamic" ErrorMessage="Age Group is required."
                                    InitialValue="--Select--" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class='sxro sx3co'>
                    <div runat="server" id="dvExternalBkgSvc" visible="false">
                        <div class='sxlb'>
                            <span class="cptn">External Service</span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclComboBox runat="server" ID="cmbExtBkService" AutoPostBack="false" Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab" OnDataBound="cmbExtBkService_DataBound">
                            </infs:WclComboBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="upload-box-header">
    <h1>Upload Documents
    </h1>
    Click browse button to select files.
</div>

<div class="upload-box">
    <infsu:UploadDisclosureDocuments ID="ucUploadDocuments" runat="server"></infsu:UploadDisclosureDocuments>
</div>

<div class="section">
    <div class="content">
        <div class="swrap docGrid">
            <infs:WclGrid runat="server" ID="grdMapping" AllowPaging="True" AutoGenerateColumns="False" EnableLinqExpressions="false"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="False" OnItemDataBound="grdMapping_ItemDataBound"
                OnNeedDataSource="grdMapping_NeedDataSource" OnDeleteCommand="grdMapping_DeleteCommand"
                OnUpdateCommand="grdMapping_UpdateCommand" OnItemCommand="grdMapping_ItemCommand">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="SystemDocumentID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false" ShowExportToExcelButton="false"
                        ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="DocumentPath" FilterControlAltText="Filter DocumentPath column"
                            HeaderText="ID" SortExpression="DocumentPath" UniqueName="DocumentPath"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter FileName column"
                            HeaderText="File Name" SortExpression="FileName" UniqueName="FileName" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Size" FilterControlAltText="Filter Size column"
                            HeaderText="Size(KB)" SortExpression="Size" UniqueName="Size" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpDocumentType.DT_Name" FilterControlAltText="Filter Document Type column"
                            HeaderText="Document Type" SortExpression="lkpDocumentType.DT_Name" UniqueName="DocType" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <ItemTemplate>
                                <%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Description")) )%>                               
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="padding: 10px;">
                                    <infs:WclTextBox runat="server" ID="txtDescription" Width="100%" Text=' <%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Description")) )%>'>
                                    </infs:WclTextBox>
                                </div>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="IsOperational" FilterControlAltText="Filter IsOperational column"
                            HeaderText="Is Operational" SortExpression="IsOperational" UniqueName="IsOperational" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SendToStudent" FilterControlAltText="Filter SendToStudent column"
                            HeaderText="Send To Student" SortExpression="SendToStudent" UniqueName="SendToStudent" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpDisclosureDocumentAgeGroup.LDDAG_Name" FilterControlAltText="Filter Age Group column"
                            HeaderText="Age Group" SortExpression="lkpDisclosureDocumentAgeGroup.LDDAG_Name" UniqueName="AgeGroup" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn FilterControlAltText="Filter ExternalServiceID column" Visible="false"
                            HeaderText="External Service ID" SortExpression="ExternalServiceID" UniqueName="ExternalServiceID" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn FilterControlAltText="Filter ExternalServiceID column" Visible="false"
                            HeaderText="External Service ID" SortExpression="ExternalServiceID" UniqueName="ExternalServiceID" ReadOnly="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Service" UniqueName="Service">

                            <EditItemTemplate>
                                <infs:WclComboBox runat="server" ID="cmbExtBkServiceEdit" AutoPostBack="false" Filter="Contains"
                                    OnClientKeyPressing="openCmbBoxOnTab">
                                </infs:WclComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageDocument">
                            <HeaderStyle Width="130" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadButton ID="btnNotificationPdf" ToolTip="Click here to view the document" OnClientClicked="openPdfPopUp" AutoPostBack="false" CssClass="classImagePdf"
                                    runat="server" Font-Underline="true">
                                    <Image EnableImageButton="true" />
                                </telerik:RadButton>
                                <asp:HiddenField ID="hdnfSystemDocumentId" runat="server" Value='<%#Eval("SystemDocumentId")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="FieldMapping">
                            <ItemTemplate>
                                <telerik:RadButton ID="btnFieldMapping" ButtonType="LinkButton" CommandName="FieldMapping"
                                    runat="server" Text="Manage Field Mapping" ToolTip="Manage Field Mapping" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                                </telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this document?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                        </FormTemplate>
                    </EditFormSettings>

                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
        <div id="divCmdButton" runat="server">
            <div class="sxcbar">
                <div class="sxcmds" style="text-align: center">
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false">
                        <Icon PrimaryIconCssClass="rbCancel" PrimaryIconLeft="4" PrimaryIconTop="4" PrimaryIconHeight="14"
                            PrimaryIconWidth="14" />
                    </infs:WclButton>
                </div>
            </div>
        </div>
    </div>
</div>
