<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ManageUploadDocument" CodeBehind="ManageUploadDocument.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="UploadDocuments" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/UploadDocuments.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxUpload">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/upload.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<style>
    #modcmd_bar {
        position: relative !important;
        z-index: 9999999999;
    }

    .Category label > input {
        display: none;
    }

    .Category label {
        color: black;
        background-color: white;
        padding-left: 10px !important;
    }

    #cmbItems li:not(.rcbSeparator) {
        padding: 5px 0px 0px 40px !important;
    }

    .box__dragndrop,
    .box__uploading,
    .box__success,
    .box__error {
        display: none;
    }

    .issue-drop-zone {
        border: 1px dashed #ccc;
        border-top-color: rgb(204, 204, 204);
        border-right-color: rgb(204, 204, 204);
        border-bottom-color: rgb(204, 204, 204);
        border-left-color: rgb(204, 204, 204);
        border-radius: 0;
        padding: 7px;
        padding-left: 7px;
    }

    .issue-drop-zone__drop-icon {
        position: relative;
    }

    .issue-drop-zone__drop-icon {
        position: relative;
    }

    .adg3 #attachmentmodule .issue-drop-zone .issue-drop-zone__text {
        width: 53%;
        font-family: -apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,Oxygen,Ubuntu,'Fira Sans','Droid Sans','Helvetica Neue',sans-serif;
        color: #172b4d;
        font-size: 14px;
        font-weight: 400;
        font-style: normal;
        line-height: 20px;
    }

    .issue-drop-zone__text {
        text-align: center;
    }

    .issue-drop-zone:not(.mod-content) {
        text-align: center;
    }

</style>
<div id="modcmd_bar">
    <div id="vermod_cmds" style="padding-top: 0px; position: relative; padding-right: 13px;">
        <asp:LinkButton Text="" ForeColor="Blue" runat="server" ID="lnkGoBack" OnClick="lnkGoBack_click"
            Visible="false" />
    </div>
</div>
<div style="float: right; margin-right: 10px; display: none;" runat="server" id="dvBackToCompTracking">
    <asp:LinkButton ID="lnkBacKToComplianceTracking" runat="server" OnClick="lnkBacKToComplianceTracking_Click">Back to Compliance Tracking</asp:LinkButton>
</div>

<div style="float: right; margin-right: 20px;" runat="server" id="divUnifiedDoc">
    <asp:LinkButton ID="lnkUnifiedDocument" runat="server" OnClick="lnkUnifiedDocument_Click">View Unified Document</asp:LinkButton>
    <iframe id="ifrUnifiedDocument" runat="server" height="0" width="0"></iframe>
</div>
<div runat="server" id="divUploadDoc">
    <div class="upload-box-header">
        <h1>Upload Documents
        </h1>
     
    </div>

    <div class="upload-box">
        <infsu:UploadDocuments ID="ucUploadDocuments" runat="server" isDropZoneEnabled="true" DropzoneID="ManageUploadDropZone"></infsu:UploadDocuments>
    </div>

</div>
<div class="section">
    <%--<h1 class="mhdr">Map Documents</h1>--%>
    <h1 class="mhdr"><%=Resources.Language.MAPDOCUMENTS %></h1>
    <div class="content">
        <div class="swrap docGrid">
            <infs:WclGrid runat="server" ID="grdMapping" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="False"
                GridLines="Both" OnNeedDataSource="grdMapping_NeedDataSource" OnDeleteCommand="grdMapping_DeleteCommand"
                OnUpdateCommand="grdMapping_UpdateCommand" OnItemCommand="grdMapping_ItemCommand"
                OnItemDataBound="grdMapping_ItemDataBound" >
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="ApplicantDocumentID,DocumentTypeCode,FileType">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" RefreshText="<%$Resources:Language,REFRESH %>" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="ExportCheckBox" AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="2%" ItemStyle-Width="2%">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectDocument" runat="server" onclick="UnCheckHeader(this)"
                                    OnCheckedChanged="chkSelectDocument_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="ApplicantUploadedDocumentID" FilterControlAltText="Filter ApplicantUploadedDocumentID column"
                            HeaderText="ID" SortExpression="ApplicantUploadedDocumentID" UniqueName="ApplicantUploadedDocumentID"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <%--HeaderText="File Name"--%>
                        <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter FileName column" HeaderStyle-Width="350"
                             HeaderText="<% $Resources:Language, FILENAME %>"
                            SortExpression="FileName" UniqueName="FileName" ReadOnly="true" HeaderTooltip="<% $Resources:Language, UPLOADFILENAMETOOLTIP%>">
                        </telerik:GridBoundColumn>
                        <%--HeaderText="File Type"--%>
                        <telerik:GridBoundColumn DataField="FileType" FilterControlAltText="Filter FileType column" HeaderStyle-Width="80"
                            HeaderText="<% $Resources:Language, FILETYPE %>"
                             SortExpression="FileType" UniqueName="FileType" ReadOnly="true" HeaderTooltip="<% $Resources:Language, UPLOADFILETYPETOOLTIP%>">
                        </telerik:GridBoundColumn>
                        <%--HeaderText="Size (KB)"--%>
                        <telerik:GridNumericColumn DataField="Size" FilterControlAltText="Filter Size column"
                             HeaderText="<% $Resources:Language, SIZE %>"
                             SortExpression="Size" UniqueName="Size" ReadOnly="true" HeaderTooltip="<% $Resources:Language, UPLOADFILESIZETOOLTIP%>" DataFormatString="{0:F}" DecimalDigits="0">
                            <HeaderStyle Width="80" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            </telerik:GridNumericColumn>

                        <%--<telerik:GridBoundColumn DataField="Size" FilterControlAltText="Filter Size column"
                             HeaderText="<% $Resources:Language, SIZE %>"
                             SortExpression="Size" UniqueName="Size" ReadOnly="true" HeaderTooltip="This column contains the file size of each uploaded document">
                            <HeaderStyle Width="80" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>--%>
                        <%--HeaderText="Uploaded By"--%>
                        <telerik:GridBoundColumn DataField="UploadedBy" FilterControlAltText="Filter UploadedBy column" HeaderStyle-Width="120"
                             HeaderText="<% $Resources:Language, UPLOADEDBY%>"
                             SortExpression="UploadedBy" UniqueName="UploadedBy" ReadOnly="true" HeaderTooltip="<% $Resources:Language, UPLOADFILEUPLOADEDBYTOOLTIP%>">
                        </telerik:GridBoundColumn>
                        <%--HeaderText="Uploaded On"--%>
                        <telerik:GridBoundColumn DataField="UploadedOn" FilterControlAltText="Filter UploadedOn column" HeaderStyle-Width="120" DataType="System.DateTime"
                            HeaderText="<% $Resources:Language, UPLOADEDON%>"  DataFormatString="{0:MM/dd/yyyy hh:mm tt}" ReadOnly="true"
                              SortExpression="UploadedOn" UniqueName="UploadedOn" HeaderTooltip="<% $Resources:Language, UPLOADFILEUPLOADEDONTOOLTIP%>">
                        </telerik:GridBoundColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="300" HeaderText="Mapped Items" UniqueName="ItemName" HeaderTooltip="This column contains the mapped items of uploaded document">
                            <ItemTemplate>
                                <a id="hrefItem" runat="server">
                                    <%# Convert.ToString(Eval("ItemName")).Length > 50 ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ItemName")).Substring(0,30))+"...." : INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ItemName"))) %></a>
                                <infs:WclToolTip runat="server" ID="tltpItem" TargetControlID="hrefItem" Width="300px"
                                    Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString( Eval("ItemName"))) %>' ManualClose="false" RelativeTo="Element"
                                    Position="TopRight" Visible='<%# Eval("ItemName").ToString().Length < 30? false : true %>'>
                                </infs:WclToolTip>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--HeaderText="Description"--%>
                        <telerik:GridTemplateColumn HeaderStyle-Width="300" HeaderText="<% $Resources:Language, DESCRIPTION%>" HeaderTooltip="<% $Resources:Language, UPLOADFILEDESCRIPTIONTOOLTIP%>">
                            <ItemTemplate>
                                <a id="hrefDesc" runat="server">
                                    <%# Convert.ToString(Eval("Description")).Length > 50 ? Convert.ToString(Eval("Description")).Substring(0,30)+"...." : Eval("Description") %></a>
                                <infs:WclToolTip runat="server" ID="tltpCatDesc" TargetControlID="hrefDesc" Width="300px"
                                    Text='<%# Eval("Description").ToString() %>' ManualClose="false" RelativeTo="Element"
                                    Position="TopRight" Visible='<%# Eval("Description").ToString().Length < 30? false : true %>'>
                                </infs:WclToolTip>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <infs:WclTextBox runat="server" ID="txtDescription" Width="100%" Text='<%#Eval("Description")%>' MaxLength="500">
                                </infs:WclTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageDocument">
                            <HeaderStyle Width="280" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <a runat="server" id="ancManageDocument" title="<% $Resources:Language, UPLOADFILEVIEWDOCSTOOLTIP%>"><%=Resources.Language.VIEWDOCS %><%--View Document--%></a>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <infs:WclComboBox ID="cmbItems" runat="server" Width="100%" CheckBoxes="true"
                                    OnItemDataBound="cmbItems_ItemDataBound" Filter="None" DataValueField="CategoryItemsID" DataTextField="ItemsName"
                                    OnClientItemChecked="OncmbItemChecked" OnClientKeyPressing="openCmbBoxOnTab">
                                </infs:WclComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Click here to edit the description of the document." UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" Width="25" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this document?"
                            Text="Click here to delete this document from your uploaded documents list." UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" Width="30" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" PageSizeLabelText="<% $Resources:Language,PAGESIZE %>" NextPagesToolTip="<%$Resources:Language,NXTPAGE %>" PrevPagesToolTip="<%$Resources:Language,PREVPAGE %>" FirstPageToolTip="<%$Resources:Language,FIRSTPAGE %>" LastPageToolTip="<%$Resources:Language,LSTPAGE %>" AlwaysVisible="true"   />
                    <%--+ "<%$Resources:Language,ITEMS %>"+"  <%$Resources:Language,IN %> {1} page(s)"--%>
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
        <%--ExtraButtonText="Print Document(s)"--%>
        <infsu:CommandBar ID="fsucCmdExport" runat="server" ButtonPosition="Center" DisplayButtons="Extra"
            AutoPostbackButtons="Extra" ExtraButtonText="<% $Resources:Language, PRNTDOCS %>" OnExtraClick="btnPrint_Click">
        </infsu:CommandBar>
    </div>
    <asp:HiddenField ID="hdnPrintDocumentURL" runat="server" />
    <asp:HiddenField ID="hdnDocumentAssociationSettingEnabled" runat="server" />
    <asp:HiddenField ID="hdnAlreadySelectedAppDocItemAssociationID" runat="server" />
</div>
<script type="text/javascript">
    //click on link button while double click on any row of grid.   

    function grd_rwDbClick(s, e) {
        var _id = "ancManageDocument";
        var b = e.get_gridDataItem().findElement(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
        //findElement findControl
    }

    function CheckAll(id) {
        var masterTable = $find("<%= grdMapping.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        var isChecked = false;
        if (id.checked == true) {
            var isChecked = true;
        }
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled == true)) {
                masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked = isChecked; // for checking the checkboxes
            }
        }
    }
    function UnCheckHeader(id) {
        var checkHeader = true;
        var masterTable = $find("<%= grdMapping.ClientID %>").get_masterTableView();
        var row = masterTable.get_dataItems();
        for (var i = 0; i < row.length; i++) {
            if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").disabled)) {
                if (!(masterTable.get_dataItems()[i].findElement("chkSelectDocument").checked)) {
                    checkHeader = false;
                    break;
                }
            }
        }
        $jQuery('[id$=chkSelectAll]')[0].checked = checkHeader;
    }

    function openApplicantDocumentToPrint() {
        var hdnPrintDocumentURL = $jQuery("[id$=hdnPrintDocumentURL]").val();
        var documentType = "Applicant Document";
        var url = $page.url.create(hdnPrintDocumentURL);
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url,
                                         {
                                             size: "800," + popupHeight,
                                             behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move
                                         },
                                         function () {
                                             this.set_title("Applicant Document");
                                             this.set_destroyOnClose(true);
                                             this.set_status("");
                                         });

        winopen = true;
        return false;
    }


    function OncmbItemChecked(sender, eventArgs) {
        var CheckedItem = eventArgs.get_item();
        if (CheckedItem.get_text() == 'Exception') {

            var exceptions = [];
            var FixedCategoryId = $jQuery("[id$=hdnAlreadySelectedAppDocItemAssociationID]").val();
            var lstFixedCategoryId = [];
            $jQuery.each(FixedCategoryId.split(","), function () { lstFixedCategoryId.push($jQuery.trim(this)); });

            if (CheckedItem.get_value().indexOf('_0') >= 0) {
                exceptions[CheckedItem.get_value()] = CheckedItem.get_checked();
            }
            sender.get_items().forEach(function (item) {

                if (item.get_value().indexOf('_-1') >= 0 || item.get_value().indexOf('_0') >= 0) return;

                var excvalue = item.get_value().substring(0, item.get_value().indexOf('_')).concat('_0');

                if (exceptions[excvalue] == undefined) {
                    var items = sender.findItemByValue(excvalue);
                    if (items != undefined) {
                        exceptions[excvalue] = sender.findItemByValue(excvalue).get_checked();
                    }
                    else {
                        exceptions[excvalue] = false;
                    }
                }

                if (exceptions[excvalue] == true) {

                    if (item.get_enabled()) {
                        item.set_checked(false);
                        item.disable();
                    }
                }
                else if (lstFixedCategoryId.indexOf(item.get_value()) < 0) {
                    item.enable();
                }

            });
        }
    }

</script>

