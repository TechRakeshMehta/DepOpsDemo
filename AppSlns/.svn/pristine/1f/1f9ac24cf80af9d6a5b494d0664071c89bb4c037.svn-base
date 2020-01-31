<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Messaging.Views.TransferRulesMaintenanceForm" Codebehind="TransferRulesMaintenanceForm.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblGridFormHead" Text='<%# (Container is GridEditFormInsertItem) ? "Add New Record" : "Update Record" %>'
            runat="server" /></h1>
    <div class="content">
        <!-- Note: Please donot insert anything here. There should be nothing between content and form divs -->
        <div class="sxform auto">
            <div class="msgbox">
                <asp:Label ID="lblMessage" runat="server" CssClass="errmsg" ViewStateMode="Disabled" /></div>
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlName1">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Institution</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbInstitutions" AutoPostBack="true" DataTextField="TenantName" ClientKey="cmbInstitutions"
                            DataValueField="TenantID" runat="server" OnSelectedIndexChanged="cmbInstitutions_SelectedIndexChanged">
                        </infs:WclComboBox>
                    </div>
                    <div class='sxlb'>
                        <span class="cptn">Location</span>
                    </div>
                    <div class='sxlm'>
                        <infs:WclComboBox ID="cmbLocations" runat="server" AutoPostBack="true" DataTextField="LocationName"
                            DataValueField="OrganizationLocationID" OnSelectedIndexChanged="cmbLocation_SelectedIndexChanged">
                        </infs:WclComboBox>
                    </div>
                   <%-- <div class='sxlb'>
                        Program
                    </div>--%>
                   <%-- <div class='sxlm'>
                        <infs:WclComboBox ID="cmbPrograms" runat="server" DataTextField="ProgramStudy" 
                        DataValueField="AdminProgramStudyID" ClientKey="cmbPrograms" Visible="false">
                        </infs:WclComboBox>
                    </div>--%>
                     <div class='sxlb'>
                        <span class="cptn">Folder Name</span><span class="reqd" style="color:Red;">*</span>
                    </div>
                    <div class='sxlm '>
                        <infs:WclComboBox ID="cmbFolders" runat="server" AutoPostBack="false" DataTextField="MessageFolderName"
                            DataValueField="MessageFolderID" OnItemDataBound="cmbFolders_ItemDataBound"  ValidationGroup="FolderValidation" >
                        </infs:WclComboBox>
                        <asp:RequiredFieldValidator runat="server" ID="rfvFolder"  ValidationGroup="FolderValidation" ControlToValidate="cmbFolders"
                            class="errmsg" InitialValue="--SELECT--" Display="Dynamic"
                            ErrorMessage="Folder name is required." />
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <%--<div class='sxro sx3co'>
                   
                </div>--%>
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">User(s)</span>
                    </div>
                    <div class='sxlm m2spn'>
                        <infs:WclAutoCompleteBox runat="server" AllowCustomEntry="false" ID="autxSelectusers" ClientKey="autxSelectusers"
                            InputType="Token">
                            <TokensSettings AllowTokenEditing="true" />
                        </infs:WclAutoCompleteBox>
                    </div>
                    <div class='sxlm nobg'>
                        <infs:WclButton runat="server" ID="btnUsers" Text="..." ButtonType="LinkButton" AutoPostBack="false"
                            OnClientClicked="e_showaddresslist">
                        </infs:WclButton>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <infsu:CommandBar ID="fsucCmdBar1" ValidationGroup="FolderValidation" runat="server" GridMode="true" OnSaveClick="btnSave_Click" GridInsertText="Save" GridUpdateText="Save"
            DefaultPanel="pnlName1" />
    </div>
</div>
