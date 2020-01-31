<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchySetting.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchySetting" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rmpHierarchyControls">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~//Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Scripts/bootstrap.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid" tabindex="-1" id="dvAgencyHierarchySetting" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Agency Setting(s)</h2>
        </div>
    </div>
    <div id="dvHierarchySettings" runat="server" class="row">
        <div class="col-md-12">
            <div class="form-inline">
                <span class="cptn">Look up for Invitation Expiration Setting(s) </span>
                <asp:RadioButton ID="rbtnCheckParentSettingYes" OnCheckedChanged="rbtnCheckParentSettingYes_CheckedChanged" AutoPostBack="true" runat="server" GroupName="CheckParentSetting" Text="Yes" />
                <asp:RadioButton ID="rbtnCheckParentSettingNo" OnCheckedChanged="rbtnCheckParentSettingNo_CheckedChanged" AutoPostBack="true" runat="server" GroupName="CheckParentSetting" Text="No" />
            </div>
        </div>
    </div>

    <div class="clearfix"></div>
    <div id="dvDisplayAgencyHierarchySettings" runat="server" class="row">
        <div class="col-md-6">
            <div id="dvHierarchySetting" runat="server" class="table-responsive">
                <table class="table table-bordered">
                    <tbody>
                        <tr>
                            <td><span class="cptn">Invitation Expiration Criteria </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnExpirationCriteriaYes" runat="server" GroupName="ExpirationCriteria" Text="On" />
                                <asp:RadioButton ID="rbtnExpirationCriteriaNo" runat="server" GroupName="ExpirationCriteria" Text="Off" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="form-group col-md-12">
        <div class=" pull-right">
            <infsu:CommandBar ID="CommandBar1" runat="server" ButtonPosition="Right" DisplayButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk"
                AutoPostbackButtons="Submit" OnSubmitClick="fsucCmdBar_SubmitClick" SubmitButtonIconClass="rbSave" SubmitButtonText="Save" ValidationGroup="grpFormAgencyHierarchySettingSubmit">
            </infsu:CommandBar>
        </div>
    </div>
    <div class="clearfix"></div>
    <div id="dvAutoArchivedLookupSetting" runat="server" class="row">
        <div class="col-md-12">
            <div class="form-inline">
                <span class="cptn">Look up for Automatically Archived Rotation Setting(s) </span>
                <asp:RadioButton ID="rbtnAutoArchivedSettingCheckYes" OnCheckedChanged="rbtnAutoArchivedSettingCheckYes_CheckedChanged" AutoPostBack="true" runat="server" GroupName="AutoArchivedCheckParentSetting" Text="Yes" />
                <asp:RadioButton ID="rbtnAutoArchivedSettingCheckNo" OnCheckedChanged="rbtnAutoArchivedSettingCheckNo_CheckedChanged" AutoPostBack="true" runat="server" GroupName="AutoArchivedCheckParentSetting" Text="No" />
            </div>
        </div>
    </div>

    <div class="clearfix"></div>
    <div id="dvAutomaticallyArchivedSetting" runat="server" class="row">
        <div class="col-md-6">
            <div id="Div1" runat="server" class="table-responsive">
                <table class="table table-bordered">
                    <tbody>
                        <tr>
                            <td><span class="cptn">Automatically Archived Rotation</span></td>
                            <td>
                                <asp:RadioButton ID="rdbAutoArchivedYes" runat="server" GroupName="AutoArchivedRot" Text="On" />
                                <asp:RadioButton ID="rdbAutoArchivedNo" runat="server" GroupName="AutoArchivedRot" Text="Off" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <div class="clearfix"></div>
    <div class="form-group col-md-12">
        <div class=" pull-right">
            <infsu:CommandBar ID="fsucCmdBarArchivedRotation" runat="server" ButtonPosition="Right" DisplayButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk"
                AutoPostbackButtons="Submit" OnSubmitClick="fsucCmdBarArchivedRotation_SubmitClick" SubmitButtonIconClass="rbSave" SubmitButtonText="Save" ValidationGroup="grpFormAgencyHierarchySettingSubmit">
            </infsu:CommandBar>
        </div>
    </div>

    <%--Start UAT-4673--%>
    <div class="clearfix"></div>
    <div id="dvReviewStatusLookupSetting" runat="server" class="row">
        <div class="col-md-12">
            <div class="form-inline">
                <span class="cptn">Look up for Update Review status on Rotation End date change Setting(s) </span>
                <asp:RadioButton ID="rbReviewStatusParentSettingYes" OnCheckedChanged="rbReviewStatusParentSettingYes_CheckedChanged" AutoPostBack="true" runat="server" GroupName="UpdateReviewStatusParentSetting" Text="Yes" />
                <asp:RadioButton ID="rbReviewStatusParentSettingNo" OnCheckedChanged="rbReviewStatusParentSettingNo_CheckedChanged" AutoPostBack="true" runat="server" GroupName="UpdateReviewStatusParentSetting" Text="No" />
            </div>
        </div>
    </div>

    <div class="clearfix"></div>
    <div id="dvReviewStatusSetting" runat="server" class="row">
        <div class="col-md-6">
            <div id="Div4" runat="server" class="table-responsive">
                <table class="table table-bordered">
                    <tbody>
                        <tr>
                            <td><span class="cptn">Update Review status on Rotation End date change</span></td>
                            <td>
                                <asp:RadioButton ID="rdbUpdateReviewStatusYes" runat="server" GroupName="UpdateReviewStatus" Text="On" />
                                <asp:RadioButton ID="rdbUpdateReviewStatusNo" runat="server" GroupName="UpdateReviewStatus" Text="Off" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <div class="clearfix"></div>
    <div class="form-group col-md-12">
        <div class=" pull-right">
            <infsu:CommandBar ID="cmdBarReviewStatus" runat="server" ButtonPosition="Right" DisplayButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk"
                AutoPostbackButtons="Submit" OnSubmitClick="cmdBarReviewStatus_SubmitClick" SubmitButtonIconClass="rbSave" SubmitButtonText="Save" ValidationGroup="grpFormAgencyHierarchySettingSubmit">
            </infsu:CommandBar>
        </div>
    </div>
    <%--End UAT-4673--%>

    <div class="clearfix"></div>
    <div id="dvInstPrecpMandatoryIndvShare" runat="server" class="row">
        <div class="col-md-12">
            <div class="form-inline">
                <span class="cptn">Instructor/Preceptor Required for Applicant Share with Agency</span> 
                <asp:RadioButton ID="rdbtnRequired" runat="server" GroupName="InstPrecpIndividualShareSetting" Text="Yes" />
                <asp:RadioButton ID="rdbtnOptional" runat="server" GroupName="InstPrecpIndividualShareSetting" Text="No" />
                <asp:RadioButton ID="rdbtnDefault" runat="server" GroupName="InstPrecpIndividualShareSetting" Text="Default" /> 
            </div>
        </div>
    </div>
     <div class="clearfix"></div>
    <div class="form-group col-md-12">
        <div class=" pull-right">
            <infsu:CommandBar ID="cmdInstPrecpMandatoryIndvShare" runat="server" ButtonPosition="Right" DisplayButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk"
                AutoPostbackButtons="Submit" OnSubmitClick="cmdInstPrecpMandatoryIndvShare_SubmitClick" SubmitButtonIconClass="rbSave" SubmitButtonText="Save" ValidationGroup="grpFormInstPrecpMandatoryIndvShare">
            </infsu:CommandBar>
        </div>
    </div>

</div>
