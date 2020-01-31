<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ManagePersonalDocument" CodeBehind="ManagePersonalDocument.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagName="UploadDocuments" TagPrefix="infsu" Src="~/ComplianceOperations/UserControl/UploadDocuments.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxUpload">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/upload.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<style>
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

<div runat="server" id="divUploadDoc">
    <div class="upload-box-header">
        <h1>Upload Documents
        </h1>
    </div>

    <div class="upload-box">
        <infsu:UploadDocuments ID="ucUploadDocuments" runat="server" isDropZoneEnabled="false" DropzoneID="ManagePersonalDropZone"></infsu:UploadDocuments>
    </div>
</div>
<div class="section">
    <h1 class="mhdr">Manage Professional Documents</h1>
    <div class="content">
        <div class="swrap docGrid">
            <infs:WclGrid runat="server" ID="grdPersonalDocs" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="false" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="false" ShowAllExportButtons="false" ShowExtraButtons="False"
                GridLines="Both" OnNeedDataSource="grdPersonalDocs_NeedDataSource" OnDeleteCommand="grdPersonalDocs_DeleteCommand"
                OnUpdateCommand="grdPersonalDocs_UpdateCommand" OnItemCommand="grdPersonalDocs_ItemCommand"
                OnItemDataBound="grdPersonalDocs_ItemDataBound">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="450mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <ClientEvents OnRowDblClick="grd_rwDbClick" />
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" EditMode="InPlace" DataKeyNames="ApplicantDocumentID">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToCsvButton="false"
                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="true" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="ApplicantUploadedDocumentID" FilterControlAltText="Filter ApplicantUploadedDocumentID column"
                            HeaderText="ID" SortExpression="ApplicantUploadedDocumentID" UniqueName="ApplicantUploadedDocumentID"
                            Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter FileName column" HeaderStyle-Width="350"
                            HeaderText="File Name" SortExpression="FileName" UniqueName="FileName" ReadOnly="true" HeaderTooltip="This column contains the name of each uploaded document">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="FileType" FilterControlAltText="Filter FileType column" HeaderStyle-Width="80"
                            HeaderText="File Type" SortExpression="FileType" UniqueName="FileType" ReadOnly="true" HeaderTooltip="This column contains the type of each uploaded document">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Size" FilterControlAltText="Filter Size column"
                            HeaderText="Size (KB)" SortExpression="Size" UniqueName="Size" ReadOnly="true" HeaderTooltip="This column contains the file size of each uploaded document">
                            <HeaderStyle Width="80" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="UploadedOn" FilterControlAltText="Filter UploadedOn column" HeaderStyle-Width="120" DataType="System.DateTime" DataFormatString="{0:d}" HeaderText="Uploaded On" SortExpression="UploadedOn" UniqueName="UploadedOn" ReadOnly="true" HeaderTooltip="This column contains the upload date of uploaded document">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="300" HeaderText="Description" HeaderTooltip="This column displays the description of each uploaded document">
                            <ItemTemplate>
                                <a id="hrefDesc" runat="server">
                                    <%# Convert.ToString(Eval("Description")).Length > 50 ? INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Description")).Substring(0,30))+"...." : INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Description"))) %></a>
                                <infs:WclToolTip runat="server" ID="tltpCatDesc" TargetControlID="hrefDesc" Width="300px"
                                    Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("Description"))) %>' ManualClose="false" RelativeTo="Element"
                                    Position="TopRight" Visible='<%# Eval("Description").ToString().Length < 30? false : true %>'>
                                </infs:WclToolTip>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <infs:WclTextBox runat="server" ID="txtDescription" Width="100%" Text='<%#Eval("Description")%>' MaxLength="500">
                                </infs:WclTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManageDocument">
                            <HeaderStyle Width="110" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <a runat="server" id="ancManageDocument" title="Click here to view the document">View Document</a>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Click here to edit the description of the document." UniqueName="EditCommandColumn">
                            <HeaderStyle CssClass="tplcohdr" Width="30" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this document?"
                            Text="Click here to delete this document from your uploaded documents list." UniqueName="DeleteColumn">
                            <HeaderStyle CssClass="tplcohdr" Width="30" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
                </MasterTableView>
                <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
            </infs:WclGrid>
        </div>
        <div class="gclr">
        </div>
    </div>
</div>

<script type="text/javascript">
    //click on link button while double click on any row of grid.   

    function grd_rwDbClick(s, e) {
        var _id = "ancManageDocument";
        var b = e.get_gridDataItem().findElement(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
        //findElement findControl
    }
</script>
