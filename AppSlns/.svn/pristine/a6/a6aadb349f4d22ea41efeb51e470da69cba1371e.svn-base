<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.VerificationDetail" Codebehind="VerificationDetail.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<%@ Register TagName="UploadDocuments" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/UploadDocuments.ascx" %>
<style type="text/css">
    html, body, #frmmod, #UpdatePanel1, #box_content
    {
        height: 100% !important;
    }
</style>
<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceDataEntry/Scripts/UploadDocuments.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<asp:HiddenField ID="PosX" runat="server" />
<asp:HiddenField ID="PosY" runat="server" />
<div style="display: none;">
    <infs:WclAsyncUpload runat="server" ID="hiddenuploader" HideFileInput="true" Skin="Hay"
        AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,gif,tif,tiff,docx,doc,rtf,pdf,ODT,TXT,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT"
        MultipleFileSelection="Automatic" OnClientValidationFailed="upl_OnClientValidationFailed">
        <Localization Select="Browse" />
    </infs:WclAsyncUpload>
</div>
<infs:WclSplitter ID="sptrMain" runat="server" LiveResize="true" Orientation="Horizontal"
    Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
    <infs:WclPane ID="pnToolbar" runat="server" MinHeight="80" Height="80" Width="100%"
        Scrolling="None" Collapsed="false">
        <div style="font-size: 11px">
            <div id="modwrapo">
                <div id="modwrapi">
                    <div id="breadcmb">
                        <infsu:breadcrumb ID="breadcrum" runat="server" />
                    </div>
                    <div id="modhdr">
                        <h1>
                            <asp:Label Text="" runat="server" ID="lblModHdr" />&nbsp;<asp:Label Text="" runat="server"
                                ID="lblPageHdr" CssClass="phdr" /></h1>
                    </div>
                </div>
                <div id="modcmds">
                </div>
            </div>
        </div>
    </infs:WclPane>
    <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%">
        <infs:WclSplitter ID="sptrVeriOuter" runat="server" LiveResize="true" Orientation="Vertical"
            Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true"
            ClientKey="sptrVerification">
            <infs:WclPane ID="pnLeft" runat="server" Height="100%" Width="50%" MinWidth="200"
                Scrolling="Y" Collapsed="false">
                <div class="section">
                    <div class="content">
                        <div class="sxform auto">
                            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlApplicantDetail">
                                <div class='sxro sx2co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Applicant Name</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtName" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">DOB</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtDOB" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx2co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Gender</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtGender" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">Phone</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtPhone" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlItemDetail">
                                <div class='sxro sx2co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Email</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtEmail" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx2co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Item Name</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtItemName" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxlb'>
                                        <span class="cptn">Client Name</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclTextBox runat="server" ID="txtTenantName" ReadOnly="true">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div id="errorMessageBox" class="msgbox" runat="server">
                    <asp:Label ID="lblError" runat="server" CssClass="error" Text="">
                    </asp:Label>
                </div>
                <div class="section" id="divExprBuilderBlock" runat="server">
                    <h1 class="mhdr">
                        Attributes</h1>
                    <div class="content">
                        <div class="swrap">
                            <infs:WclGrid runat="server" ID="grdAttributes" AutoGenerateColumns="False" AutoSkinMode="false"
                                CellSpacing="0" EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false"
                                ShowHeader="false" ShowFooter="false" OnItemDataBound="grdAttributes_ItemDataBound">
                                <ClientSettings EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="true"></Selecting>
                                </ClientSettings>
                                <MasterTableView CommandItemDisplay="None" DataKeyNames="ApplicantComplianceAttributeID"
                                    AllowPaging="false">
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ComplianceAttribute.Name" AllowFiltering="false"
                                            HeaderText="Attribute Name" SortExpression="ComplianceAttribute.Name" UniqueName="Name">
                                            <HeaderStyle Width="50%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderText="Value">
                                            <HeaderStyle Width="50%" />
                                            <ItemTemplate>
                                                <infs:WclTextBox ID="txtDataType" runat="server" Visible="false">
                                                </infs:WclTextBox>
                                                <infs:WclDatePicker ID="dtPicker" runat="server" Visible="false">
                                                </infs:WclDatePicker>
                                                <infs:WclTextBox ID="txtBox" runat="server" Visible="false">
                                                </infs:WclTextBox>
                                                <infs:WclNumericTextBox ID="numericTextBox" runat="server" Visible="false">
                                                </infs:WclNumericTextBox>
                                                <infs:WclComboBox ID="optionCombo" runat="server" Visible="false">
                                                </infs:WclComboBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        No Attributes found.
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                            </infs:WclGrid>
                        </div>
                        <div class="gclr">
                        </div>
                    </div>
                </div>
                <div id="documentSection" class="section" runat="server">
                    <h1 class="mhdr">
                        Documents
                    </h1>
                    <div class="content">
                        <div class="sxform auto">
                            <infsu:UploadDocuments ID="ucUploadDocuments" runat="server"></infsu:UploadDocuments>
                        </div>
                        <div class="swrap">
                            <infs:WclGrid runat="server" ID="grdDocuments" AutoGenerateColumns="False" AutoSkinMode="false"
                                CellSpacing="0" EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="false"
                                ShowHeader="false" ShowFooter="false" OnNeedDataSource="grdDocuments_NeedDataSource"
                                OnItemCommand="grdDocuments_ItemCommand">
                                <ClientSettings EnableRowHoverStyle="true">
                                    <%--<ClientEvents OnRowClick="OnGrdDocumentsRowClick" />--%>
                                    <%-- <Selecting AllowRowSelect="true"></Selecting>--%>
                                </ClientSettings>
                                <MasterTableView CommandItemDisplay="None" DataKeyNames="ApplicantDocumentID,DocumentPath"
                                    AllowPaging="false" ClientDataKeyNames="DocumentPath">
                                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Document Name" AllowFiltering="false">
                                            <HeaderStyle Width="50%" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDocument" runat="server" Text='<%# Eval("FileName")%>' CommandName="DocumentViewer"></asp:LinkButton>
                                                <%--<a href="#" onclick="ViewDocument('<%# Eval("DocumentPath") %>')">
                                                    <%# Eval("FileName")%></a>--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                                            HeaderText="Description" SortExpression="Description" UniqueName="Description">
                                            <HeaderStyle Width="30%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderText="" AllowFiltering="false">
                                            <HeaderStyle Width="20%" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkDocument" runat="server" Checked='<%#Eval("IsMapped") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        No Documents found.
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                            </infs:WclGrid>
                        </div>
                        <div class="gclr">
                        </div>
                    </div>
                </div>
                <div class="section">
                    <div class="content">
                        <div class="sxform auto">
                            <asp:Panel CssClass="sxpnl" runat="server" ID="pnlNotes">
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Applicant Notes</span>
                                    </div>
                                    <div class='sxlm m2spn'>
                                        <infs:WclTextBox runat="server" ID="txtNotes" TextMode="MultiLine" Height="50px"
                                            ReadOnly="true" CssClass="txt2ro">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Verification Comments</span>
                                    </div>
                                    <div class='sxlm m2spn'>
                                        <infs:WclTextBox runat="server" ID="txtComments" TextMode="MultiLine" Height="50px"
                                            ReadOnly="true" CssClass="txt2ro">
                                        </infs:WclTextBox>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                                <div class='sxro sx3co'>
                                    <div class='sxlb'>
                                        <span class="cptn">Add Comments</span>
                                    </div>
                                    <div class='sxlm m2spn'>
                                        <infs:WclTextBox runat="server" ID="txtCommentsNew" TextMode="MultiLine" Height="50px"
                                            CssClass="txt2ro">
                                        </infs:WclTextBox>
                                        <div class='vldx'>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvComments" Display="Dynamic" ControlToValidate="txtCommentsNew"
                                                class="errmsg" ErrorMessage="Comments are required." ValidationGroup="grpFormSubmit" />
                                        </div>
                                    </div>
                                    <div class='sxroend'>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <infsu:CommandBar ID="cbbuttons" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save,Cancel,Clear"
                    AutoPostbackButtons="Submit,Save,Cancel,Clear" SubmitButtonIconClass="rbRemove"
                    SaveButtonIconClass="rbOk" SubmitButtonText="Reject" SaveButtonText="Approve"
                    CancelButtonText="Cancel" ClearButtonText="Send for Third Party Review" ValidationGroup="grpFormSubmit"
                    OnSubmitClick="CmdBarReject_Click" OnSaveClick="CmdBarApprove_Click" OnClearClick="CmdBarThirdPartyReview_Click"
                    OnCancelClick="CmdBarCancel_Click">
                </infsu:CommandBar>
            </infs:WclPane>
            <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
            </infs:WclSplitBar>
            <infs:WclPane ID="pnRight" runat="server" Scrolling="Both" Width="100%" Height="100%">
                <iframe id="iframe" runat="server" width="99%" height="98%"></iframe>
                <%--<div style="margin: 0 auto; margin-top: 100px; width: 105px; color: #adadad">
                    Nothing to preview
                </div>--%>
            </infs:WclPane>
        </infs:WclSplitter>
    </infs:WclPane>
</infs:WclSplitter>
