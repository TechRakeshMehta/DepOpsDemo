<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyNodeMapping.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.AgencyNodeMapping" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxManageAgencyHierarchyPackages">
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceDataEntry/Scripts/UploadDocuments.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div id="dvAgencyHierarchyAgency" runat="server" class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Map Agency</h2>
        </div>
    </div>
    <div class="row">
        <%--<div class="col-md-12">--%>
        <infs:WclGrid runat="server" ID="grdAgencyHirarchyAgency" AllowCustomPaging="false" EnableViewState="True"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false" AutoSkinMode="true" CellSpacing="0" GridLines="Both"
            ShowAllExportButtons="false" NonExportingColumns="DeleteColumn" OnDeleteCommand="grdAgencyHirarchyAgency_DeleteCommand"
            OnNeedDataSource="grdAgencyHirarchyAgency_NeedDataSource" OnItemCommand="grdAgencyHirarchyAgency_ItemCommand" OnItemDataBound="grdAgencyHirarchyAgency_ItemDataBound"
            OnSortCommand="grdAgencyHirarchyAgency_SortCommand" EnableLinqExpressions="false" ShowClearFiltersButton="false" EnableAriaSupport="true">
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="AgencyHierarchyAgencyID, AgencyHierarchyID,AgencyID"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Agency" ShowExportToCsvButton="true"
                    ShowExportToExcelButton="true" ShowExportToPdfButton="true" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="AgencyName" HeaderText="Agency Name" ItemStyle-Width="30%"
                        SortExpression="AgencyName" UniqueName="AgencyName" HeaderTooltip="This column displays the Requirement Agency Name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif" ItemStyle-Width="1%"
                        CommandName="Delete" ConfirmText="Are you sure you want to delete this Agency?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                        <ItemStyle Width="1%" />
                        <HeaderStyle CssClass="tplcohdr" />
                        <ItemStyle CssClass="MyImageButton" />
                    </telerik:GridEditCommandColumn>
                </Columns>
                <EditFormSettings EditFormType="Template">
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                    <FormTemplate>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h2 class="header-color">
                                        <asp:Label ID="lblEHServiceGroup" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Agency" : "Update Agency" %>'
                                            runat="server" />
                                    </h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="msgbox">
                                        <asp:Label ID="lblName1" runat="server" CssClass="info"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <asp:Panel ID="pnlEditForm" runat="server">
                                <div class="row bgLightGreen">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class='form-group col-md-3' runat="server" visible='<%# (Container is GridEditFormInsertItem) ? true : false %>' title="Select a package.">
                                                <span class="cptn">Select Agency</span><span class="reqd">*</span>

                                                <infs:WclComboBox ID="cmbAgency" runat="server" DataTextField="AgencyName" Filter="Contains" DataValueField="AgencyID" CheckBoxes="true" EnableCheckAllItemsCheckBox="false" OnClientKeyPressing="openCmbBoxOnTab"
                                                    AutoPostBack="false" Skin="Silk" AutoSkinMode="false" Width="200%" CssClass="form-control">
                                                </infs:WclComboBox>
                                                <div class='vldx'>
                                                    <asp:Label ID="lblError" runat="server" Style="display: none" CssClass="errmsg" Text="Agency is required."></asp:Label>
                                                    <asp:CustomValidator ID="cstValidator" runat="server" ErrorMessage="Agency is required." CssClass="errmsg" Display="Dynamic"
                                                        ClientValidationFunction="CheckForSelectedAgency" ClientIDMode="Static" ValidationGroup="grpManageAgencyHierarchyAgencyFormSubmit"></asp:CustomValidator>
                                                </div>
                                            </div>
                                            <div class="form-group col-md-12" runat="server" visible='<%# (Container is GridEditFormInsertItem) ? false : true %>'>
                                                <span class="cptn">Agency Name</span>
                                                <asp:Label ID="lblAgenctName" Style="height: auto !important;" runat="server" CssClass="form-control" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("AgencyName"))) %>'></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12" runat="server" visible='<%# (Container is GridEditFormInsertItem) ? false : true %>'>
                                        <div class="row">
                                            <div class="form-group col-md-3" title="">
                                                <span class="cptn">Agency Attestation Form Setting </span>
                                                <asp:RadioButtonList ID="rbtnAttestationFormSettings" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                                    <asp:ListItem Text="On" Value="Y"></asp:ListItem>
                                                    <asp:ListItem Text="Off" Value="N"></asp:ListItem>
                                                    <asp:ListItem Text="Default" Value="D" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <div class="form-group col-md-3" title=""></div>
                                            <div class="form-group col-md-3" runat="server" id="dvAttestationFormUpload">
                                                <div class="upload-box-header">
                                                    <h1>Upload Document
                                                    </h1>
                                                    Click browse button to select files.
                                                </div>
                                                <div class="upload-box">
                                                    <infs:WclAsyncUpload runat="server" ID="uploadAttestationControl" HideFileInput="true" Skin="Hay" MultipleFileSelection="Disabled"
                                                        Width="100%" MaxFileInputsCount="1"
                                                        AllowedFileExtensions="pdf"
                                                        OnClientValidationFailed="upl_OnClientValidationFailed"
                                                        OnClientFileSelected="clientFileSelected" OnClientFileUploadRemoved="onFileRemoved"
                                                        OnClientFileUploading="OnClientFileUploading"
                                                        OnClientFileUploaded="OnAttestationFormUploaded"
                                                        ToolTip="Click here to select files to upload from your computer">
                                                        <Localization Select="Browse" />
                                                    </infs:WclAsyncUpload>
                                                    <div class='vldx'>
                                                        <asp:CustomValidator ID="cvAttestationUploadControl" runat="server" ErrorMessage="Attestation Form required." CssClass="errmsg" Display="Dynamic"
                                                            ClientValidationFunction="CheckAttestationFormFileCount" ClientIDMode="Static" ValidationGroup="grpManageAgencyHierarchyAgencyFormSubmit"></asp:CustomValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="dvUploadedDocuments" runat="server" style="margin-bottom: 30px;">
                                                <div class="col-md-12">
                                                    <div class="row">
                                                        <div class='form-group col-md-3'>
                                                            <span class="cptn">Document Name</span>

                                                        </div>
                                                        <div class='form-group col-md-3'>
                                                            <span class="cptn">Actions</span>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class='form-group col-md-3'>
                                                            <asp:Label runat="server" ID="lblDocumentName"></asp:Label>
                                                            <asp:HiddenField ID="hdnDocPath" runat="server" Value='<%# Bind("AttestationDocumentPath") %>' />
                                                        </div>
                                                        <div class='form-group col-md-3'>
                                                            <asp:LinkButton runat="server" ID="lnkViewDocuments" Text="View" OnClick="lnkViewDocument_Click"></asp:LinkButton>
                                                            <span style="padding-left: 3px; padding-right: 3px;">|</span>
                                                            <asp:LinkButton runat="server" ID="lnkDeletes" Text="Delete" OnClientClick="return DeleteConfirmations();"></asp:LinkButton>
                                                            <asp:LinkButton runat="server" ID="lnkDeleteDocuments" Style="display: none;" Text="Delete" OnClick="lnkDeleteDocument_Click"></asp:LinkButton>
                                                            <iframe runat="server" id="iFrameViewDocs" style="display: none;"></iframe>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <infsu:CommandBar ID="fsucCmdBarAgencyHierarchy" runat="server" GridMode="true"
                                GridInsertText="Save" GridUpdateText="Save" SaveButtonIconClass="rbSave"
                                ValidationGroup="grpManageAgencyHierarchyAgencyFormSubmit" UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                    </FormTemplate>
                </EditFormSettings>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
        <%--</div>--%>
    </div>
</div>


<script type="text/javascript">

    function CheckForSelectedAgency(sender, args) {
        args.IsValid = false;
        var idlist = '';
        var cbmitems = $find($jQuery("[id$=cmbAgency]")[0].id);

        cbmitems.get_checkedItems().forEach(function (item) {

            idlist = idlist.concat(item.get_value(), ',');
        });
        if (idlist != undefined && idlist != '') {
            args.IsValid = true;
        }
    }

    function CheckAttestationFormFileCount(sender, args) {
        args.IsValid = false;
        var selectedvalue = $jQuery("[id$=rbtnAttestationFormSettings] [type='radio']:checked").val()
        if (selectedvalue == "Y") {
            var uploadcontrol = $find($jQuery("[id$=uploadAttestationControl]")[0].id);
            var uploadedFilesCount = $jQuery(uploadcontrol._uploadedFiles).toArray().length;
            if (uploadedFilesCount > 0) {
                args.IsValid = true;
            }
        }
        else {
            args.IsValid = true;
        }
    }

    function DeleteConfirmations() {
        var selectedvalue = $jQuery("[id$=rbtnAttestationFormSettings] [type='radio']:checked").val();
        var message = "";
        if (selectedvalue == "Y") {
            message = "Are you sure, you want to delete this document? Upon deleting this form, Attestation setting of this agency will change to Off."
        }
        else {
            message = "Are you sure, you want to delete this document?"
        }
        //  var lnkDeleteDocument = $find($jQuery("[id$=lnkDeleteDocument]")[0].id);
        $confirm(message, function (res) {
            if (res) {
                $jQuery("[id$=lnkDeleteDocuments]")[0].click();
                // __doPostBack(lnkDeleteDocument.UniqueID, "");
                return true;
            }
            else {
                return false;
            }
        }, 'Complio', true);

        return false;
    }
</script>
<script src="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>

