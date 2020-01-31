<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageApplicationVideos.ascx.cs" Inherits="CoreWeb.SystemSetUp.Views.ManageApplicationVideos" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<div class="msgbox" id="divSuccessMsg">
    <asp:Label Text="" ID="lblSuccess" runat="server" Visible="false" />
</div>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblManageApplicationVideos" runat="server" Text=""></asp:Label></h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdManageApplicationVideo" DataSourceID="" AllowPaging="True"
                PageSize="10" AutoGenerateColumns="False" AllowSorting="True" GridLines="Both"
                NonExportingColumns="EditCommandColumn, DeleteColumn" OnNeedDataSource="grdManageApplicationVideo_NeedDataSource"
                OnItemCreated="grdManageApplicationVideo_ItemCreated" OnInsertCommand="grdManageApplicationVideo_InsertCommand"
                OnUpdateCommand="grdManageApplicationVideo_UpdateCommand" OnDeleteCommand="grdManageApplicationVideo_DeleteCommand">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                    Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="APV_ID">
                    <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Videos" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="APV_Title" HeaderText="Video Title">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="APV_DirectLink" HeaderText="Video Direct Link">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="lkpVideoType.VT_Name" HeaderText="Video Type">
                        </telerik:GridBoundColumn>
                        <telerik:GridEditCommandColumn ButtonType="ImageButton" EditText="Edit" UniqueName="EditCommandColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Video?"
                            Text="Delete" UniqueName="DeleteColumn">
                            <HeaderStyle Width="30px" />
                            <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                        </telerik:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                        <FormTemplate>
                            <div class="section">
                                <h1 class="mhdr">
                                    <asp:Label ID="lblHeading" Text='<%#(Container is GridEditFormInsertItem) ? "Add New Video" : "Edit Video"%>'
                                        runat="server"></asp:Label></h1>
                                <div class="content">
                                    <div class="sxform auto">
                                        <div class="msgbox">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="required"></asp:Label>
                                        </div>
                                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlManageApplicationVideos">
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblName" runat="server" AssociatedControlID="txtVideoTitle" Text="Title" CssClass="cptn"></asp:Label><span
                                                        class="reqd">*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclTextBox Width="100%" Text='<%# Bind("APV_Title")%>' MaxLength="512" TabIndex="1"
                                                        ID="txtVideoTitle" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtVideoTitle"
                                                            Display="Dynamic" CssClass="errmsg"
                                                            ErrorMessage="Please enter Video Title."
                                                            ValidationGroup="grpValdManageApplicationVideos" />
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblUserName" runat="server" AssociatedControlID="ddlVideoType" Text="Video Type" CssClass="cptn">                                                        
                                                    </asp:Label><span class='reqd'>*</span>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclComboBox ID="ddlVideoType" runat="server" AutoPostBack="false" DataTextField="VT_Name"
                                                        MaxHeight="200px" DataValueField="VT_ID" TabIndex="2">
                                                    </infs:WclComboBox>
                                                    <div class='vldx'>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvVideoType" ControlToValidate="ddlVideoType"
                                                            class="errmsg" ValidationGroup="grpValdManageApplicationVideos" Display="Dynamic"
                                                            ErrorMessage="Video Type is required."
                                                            InitialValue="--SELECT--" />
                                                    </div>
                                                    <%--<infs:WclTextBox Enabled="false" runat="server" Visible="false" ID="txtHierUser">
                                                    </infs:WclTextBox>--%>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblVideoDirectLink" runat="server" AssociatedControlID="txtVideoDirectLink"
                                                        Text="Direct Link" CssClass="cptn"></asp:Label><span
                                                            class="reqd">*</span>
                                                </div>
                                                <div class='sxlm m3spn'>
                                                    <infs:WclTextBox Width="100%" Text='<%# Bind("APV_DirectLink")%>' TabIndex="3" EmptyMessage="http|https://www.example.com"
                                                        ID="txtVideoDirectLink" runat="server" />
                                                    <div class="vldx">
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvVideoDirectLink" ControlToValidate="txtVideoDirectLink"
                                                            Display="Dynamic" CssClass="errmsg"
                                                            ErrorMessage="Please enter Video Direct Link."
                                                            ValidationGroup="grpValdManageApplicationVideos" />
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblVideoEmbedLink" runat="server" AssociatedControlID="txtVideoEmbedLink"
                                                        Text="Embed Link" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm m3spn'>
                                                    <infs:WclTextBox Width="100%" Text='<%# Bind("APV_EmbedLink")%>' MaxLength="255" TabIndex="4"
                                                        ID="txtVideoEmbedLink" runat="server" />
                                                    <div class="vldx">
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDescription"
                                                        Text="Description" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm m3spn'>
                                                    <infs:WclTextBox ID="txtDescription" runat="server" Text='<%# Eval("APV_Description") %>'
                                                        TextMode="MultiLine" MaxLength="2048" Height="50px" TabIndex="5">
                                                    </infs:WclTextBox>
                                                    <div class="vldx">
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                            <div class='sxro sx3co'>
                                                <div class='sxlb'>
                                                    <asp:Label ID="lblDisplayDirectLink" runat="server"
                                                        Text="Display Direct Link on Widget" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" Text="Yes" ToggleType="Radio" ButtonType="ToggleButton" 
                                                        GroupName="rbtnDisplayDirectLink" AutoPostBack="false" ID="rbtnDisplayDLinkYes">
                                                    </infs:WclButton>
                                                    <infs:WclButton runat="server" Text="No" ToggleType="Radio" ButtonType="ToggleButton" Checked="true"
                                                        GroupName="rbtnDisplayDirectLink" AutoPostBack="false" ID="rbtnDisplayDLinkNo">
                                                    </infs:WclButton>
                                                    <div class="vldx">
                                                    </div>
                                                </div>
                                                <div class='sxlb'>
                                                    <asp:Label ID="Label1" runat="server"
                                                        Text="Display Description on Widget" CssClass="cptn"></asp:Label>
                                                </div>
                                                <div class='sxlm'>
                                                    <infs:WclButton runat="server" Text="Yes" ToggleType="Radio" ButtonType="ToggleButton"
                                                        GroupName="rbtnDisplayDesc" AutoPostBack="false" ID="rbtnDisplayDescYes">
                                                    </infs:WclButton>
                                                    <infs:WclButton runat="server" Text="No" ToggleType="Radio" ButtonType="ToggleButton" Checked="true"
                                                        GroupName="rbtnDisplayDesc" AutoPostBack="false" ID="rbtnDisplayDescNo">
                                                    </infs:WclButton>
                                                    <div class="vldx">
                                                    </div>
                                                </div>
                                                <div class='sxroend'>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                    <infsu:CommandBar ID="fsucCmdBarManageApplicationVideos" ValidationGroup="grpValdManageApplicationVideos" GridInsertText="Save" GridUpdateText="Save"
                                        runat="server" GridMode="true" TabIndexAt="6" DefaultPanel="pnlManageApplicationVideos" />
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
    </div>
</div>
