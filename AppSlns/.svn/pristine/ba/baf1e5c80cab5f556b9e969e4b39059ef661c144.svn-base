<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageSharedUserDocument.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.UserControl.ManageSharedUserDocument" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<style>
    .Icon {
        top: 9px !important;
        left: 12px !important;
    }
</style>

<div class="container-fluid">
    <div class="row bgLightGreen" id="dvInstitution" runat="server">
        <div class="col-md-12">&nbsp;</div>
        <div class="col-md-12">
            <div class="row">
                <div class="form-group col-md-3">
                    <span class="cptn">Institution</span><span class="reqd">*</span>
                    <infs:WclComboBox ID="cmbTenant" runat="server" Width="100%" AutoSkinMode="false"
                        Skin="Silk" CssClass="form-control" AutoPostBack="true" DataTextField="TenantName"
                        OnSelectedIndexChanged="cmbTenant_SelectedIndexChanged" OnDataBound="cmbTenant_DataBound"
                        DataValueField="TenantID" Filter="None" OnClientKeyPressing="openCmbBoxOnTab">
                    </infs:WclComboBox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Instructor/Preceptor Document(s)</h2>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdUploadDocuments" AllowPaging="true" PagerStyle-Position="TopAndBottom"
            PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
            ShowClearFiltersButton="false"
            gridviewmode="AutoAddOnly" NonExportingColumns="EditCommandColumn, DeleteColumn"
            OnItemDataBound="grdUploadDocuments_ItemDataBound"
            OnNeedDataSource="grdUploadDocuments_NeedDataSource" OnItemCommand="grdUploadDocuments_ItemCommand"
            ShowAllExportButtons="false">
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="DocumentID,TempDocumentID"
                AllowFilteringByColumn="false" PagerStyle-Position="TopAndBottom">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Upload New Document"
                    ShowRefreshButton="false"
                    ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                <Columns>
                    <telerik:GridBoundColumn DataField="DocumentTypeName" FilterControlAltText="Filter DocumentTypeName column"
                        HeaderText="Document Type" SortExpression="DocumentTypeName" UniqueName="DocumentTypeName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FileName" FilterControlAltText="Filter DocumentName column"
                        HeaderText="Document Name" SortExpression="FileName" UniqueName="DocumentName">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                        HeaderText="Description" SortExpression="Description" UniqueName="Description">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn UniqueName="ViewDocument">
                        <ItemTemplate>
                            <asp:LinkButton Visible="true" Text="View Document" runat="server" ID="lnkClientContactDoc"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="tplcohdr" />
                    </telerik:GridTemplateColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                        ConfirmText="Are you sure you want to delete this document?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div runat="server" id="divEditBlock" visible="true">
                            <div class="col-md-12">
                                <h2 class="header-color">
                                    <asp:Label ID="lblNewHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Upload New Document" : "Edit Document"%>'
                                        runat="server"></asp:Label>
                                </h2>
                            </div>
                            <asp:Panel runat="server" ID="pnlMNew">
                                <div class="col-md-12">
                                    <div class="row bgLightGreen">
                                        <div class="form-group col-md-3">
                                            <asp:Label ID="lblUserName" runat="server" Text="Document Type" CssClass="cptn"> </asp:Label><span
                                                class="reqd">*</span>
                                            <infs:WclComboBox ID="cmbDocumentType" Skin="Silk" AutoSkinMode="false" Width="100%"
                                                CssClass="form-control" OnDataBound="cmbDocumentType_DataBound" runat="server"
                                                AutoPostBack="false"
                                                DataTextField="Name" MaxHeight="200px" DataValueField="SharedSystemDocTypeID">
                                            </infs:WclComboBox>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvDocType" ControlToValidate="cmbDocumentType"
                                                class="errmsg" ValidationGroup="grpUploadDoc" Display="Dynamic" ErrorMessage="Document Type is required."
                                                InitialValue="--Select--" />
                                        </div>
                                        <div class="form-group col-md-3">
                                            <asp:Label ID="lblDocUpload" runat="server" Text="Select Document" CssClass="cptn"></asp:Label><span
                                                class="reqd">*</span>
                                            <infs:WclAsyncUpload runat="server" ID="uploadControl" HideFileInput="true" Skin="Hay"
                                                MaxFileInputsCount="1"
                                                MultipleFileSelection="Disabled" OnClientFileSelected="clientFileSelected" OnClientValidationFailed="upl_OnClientValidationFailed"
                                                OnClientFileUploading="OnClientFileUploading" OnClientFileUploaded="OnClientFileUploaded"
                                                AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,JPG,gif,tif,tiff,docx,doc,rtf,pdf,odt,txt,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,JPG,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT">
                                                <Localization Select="Browse" />
                                            </infs:WclAsyncUpload>
                                        </div>
                                        <div class="form-group col-md-3">
                                            <asp:Label ID="ldlDescription" runat="server" Text="Description" CssClass="cptn"> </asp:Label>
                                            <infs:WclTextBox runat="server" Width="100%" CssClass="form-control" ID="txtDocDescription"
                                                MaxLength="500">
                                            </infs:WclTextBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="col-md-12">
                                <div class="row text-right">
                                    <infsu:CommandBar ID="fsucCmdBarNew" runat="server" UseAutoSkinMode="false" ButtonSkin="Silk"
                                        GridMode="true" ValidationGroup="grpUploadDoc" GridInsertText="Upload" GridUpdateText="Save"
                                        DefaultPanel="pnlMNew" SaveButtonIconClass="fa fa-cloud-upload Icon">
                                    </infsu:CommandBar>
                                </div>
                            </div>
                        </div>
                    </FormTemplate>
                </EditFormSettings>
            </MasterTableView>
        </infs:WclGrid>
    </div>
    <div class="row">&nbsp;</div>
    <asp:HiddenField ID="hfTenantId" runat="server" />
</div>
