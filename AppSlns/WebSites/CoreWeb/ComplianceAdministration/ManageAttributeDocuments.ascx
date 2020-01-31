<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.ManageAttributeDocuments" CodeBehind="ManageAttributeDocuments.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagName="UploadAttributeDocuments" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/UploadAttributeDocuments.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxUpload">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/upload.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<style>
    .classImageMail
    {
        background: url(../../App_Themes/Default/images/mail.png);
        background-position: 0 0;
        background-repeat: no-repeat;
        width: 20px;
        height: 20px;
    }

    .classImagePdf
    {
        background: url(../../images/medium/pdf.gif);
        background-position: 0 0;
        background-repeat: no-repeat;
        width: 20px;
        height: 20px;
    }

    .breakword
    {
        word-break: break-all;
    }

    .rgFilterRow
        {
            display: none !important;
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
        var documentType = "ClientSystemDocument";
        var tenantID = $jQuery("[id$=hdnfTenantId]").val();
        var composeScreenWindowName = "Service Form Details";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?systemDocumentId=" + hdnfSystemDocumentId + "&DocumentType=" + documentType + "&tenantId=" + tenantID);
        var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
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

<div class="section">
    <h1 class="mhdr">Manage Attribute Documents</h1>
    <div class="content">
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="info">
                </asp:Label>
            </div>
            <div class="sxpnl">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <asp:Label ID="lblTenant" runat="server" Text="Institution" CssClass="cptn"></asp:Label>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="ddlTenant" runat="server" AutoPostBack="true" DataTextField="TenantName" DataValueField="TenantID"
                            EmptyMessage="--Select--" OnDataBound="ddlTenant_DataBound" OnSelectedIndexChanged="ddlTenant_SelectedIndexChanged"
                            Filter="Contains" OnClientKeyPressing="openCmbBoxOnTab">
                        </infs:WclComboBox>
                        <div class="vldx">
                            <asp:RequiredFieldValidator runat="server" ID="rfvTenantName" ControlToValidate="ddlTenant"
                                InitialValue="--SELECT--" Display="Dynamic" ValidationGroup="grpFormSubmit" CssClass="errmsg"
                                Text="Institution is required." />
                        </div>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div runat="server" id="dvUploadDocument">
    <div class="upload-box-header">
        <h1>Upload Documents
        </h1>
        Click browse button to select files.
    </div>

    <div class="upload-box">
        <infsu:UploadAttributeDocuments ID="ucUploadDocuments" runat="server"></infsu:UploadAttributeDocuments>
    </div>

    <div class="section">

        <div class="content">
            <div class="swrap docGrid">
                <infs:WclGrid runat="server" ID="grdMapping" AllowPaging="True" PageSize="10"
                    AutoGenerateColumns="False" AllowSorting="True" GridLines="Both" ShowAllExportButtons="False"  AutoSkinMode="false"   ShowClearFiltersButton="false"
                    NonExportingColumns="EditCommandColumn, DeleteColumn, ManageDocument, FieldMapping" ValidationGroup="grpValdManageAgencies"
                    OnNeedDataSource="grdMapping_NeedDataSource" OnDeleteCommand="grdMapping_DeleteCommand" AllowFilteringByColumn="false"
                    OnUpdateCommand="grdMapping_UpdateCommand" OnItemCommand="grdMapping_ItemCommand">
                    <ExportSettings Pdf-PageWidth="300mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                        Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                        ExportOnlyData="true" IgnorePaging="true">
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true" >
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="CSD_ID">
                        <CommandItemSettings ShowAddNewRecordButton="false" 
                            ShowExportToExcelButton="true" ShowExportToPdfButton="true" ShowExportToCsvButton="true"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="DocumentPath" FilterControlAltText="Filter DocumentPath column"
                                HeaderText="ID" SortExpression="DocumentPath" UniqueName="DocumentPath"
                                Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CSD_FileName" FilterControlAltText="Filter FileName column" AllowSorting="true" ItemStyle-CssClass="breakword"  AllowFiltering="false" 
                                HeaderText="File Name" SortExpression="CSD_FileName" UniqueName="CSD_FileName" ReadOnly="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CSD_Size" FilterControlAltText="Filter Size column" ItemStyle-CssClass="breakword"  AllowFiltering="false" 
                                HeaderText="Size (KB)" SortExpression="CSD_Size" UniqueName="CSD_Size" ReadOnly="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CSD_Description" HeaderStyle-Width="40%" FilterControlAltText="Filter Description column" ItemStyle-CssClass="breakword"  AllowFiltering="false" 
                                HeaderText="Description" SortExpression="CSD_Description" UniqueName="CSD_Description" ReadOnly="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn AllowFiltering="false"  UniqueName="ManageDocument">
                                <HeaderStyle Width="230" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <telerik:RadButton ID="btnNotificationPdf" ToolTip="Click here to view the document" OnClientClicked="openPdfPopUp" AutoPostBack="false" CssClass="classImagePdf"
                                        runat="server" Font-Underline="true">
                                        <Image EnableImageButton="true" />
                                    </telerik:RadButton>
                                    <asp:HiddenField ID="hdnfSystemDocumentId" runat="server" Value='<%#Eval("CSD_ID")%>' />

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
                                <div class="section" runat="server" id="divEditBlock" visible="true">
                                    <h1 class="mhdr">
                                        <asp:Label ID="lblTitleAgency" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Attribute Document" : "Update Attribute Document" %>'
                                            runat="server" /></h1>
                                    <div class="content">
                                        <div class="sxform auto">
                                            <div class="msgbox">
                                                <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                            </div>
                                            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAttrDoc">
                                                <div class='sxro sx3co'>
                                                    <div class='sxlb'>
                                                        <span class="cptn">File Name</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox ID="txtFileName" Width="100%" runat="server" Text='<%# Eval("CSD_FileName") %>'
                                                            MaxLength="50" Enabled="false">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <span class="cptn">File Name</span>
                                                    </div>
                                                    <div class='sxlm'>
                                                        <infs:WclTextBox ID="txtSize" Width="100%" runat="server" Text='<%# Eval("CSD_Size") %>'
                                                            MaxLength="50" Enabled="false">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class='sxlb'>
                                                        <span class="cptn">Description</span>
                                                    </div>
                                                    <div class='sxlm '>
                                                        <infs:WclTextBox Width="100%" ID="txtDescription" runat="server"
                                                            Text='<%# Eval("CSD_Description") %>' MaxLength="500">
                                                        </infs:WclTextBox>
                                                    </div>
                                                    <div class='sxroend'>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <infsu:CommandBar ID="fsucCmdBarDocument" runat="server" GridMode="true" DefaultPanel="pnlAttrDoc" GridInsertText="Save" GridUpdateText="Save"
                                            ValidationGroup="grpValdAttrDoc" ExtraButtonIconClass="icnreset" />
                                    </div>
                                </div>
                            </FormTemplate>
                        </EditFormSettings>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
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
                        <asp:HiddenField ID="hdnfTenantId" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
