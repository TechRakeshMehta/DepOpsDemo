<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchyRotationFieldOption.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchyRotationFieldOption" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rmpHierarchyControls">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~//Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Scripts/bootstrap.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid" tabindex="-1" id="dvRotationFieldOption" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Rotation Field Permissions</h2>
        </div>
    </div>
    <div id="dvAddRotationSettings" runat="server" class="row">
        <div class="col-md-12">
            <div class="form-inline">
                    <span class="cptn">Look up for Rotation Field setting(s) </span>
                    <asp:RadioButton ID="rbtnCheckParentSettingYes" OnCheckedChanged="rbtnCheckParentSettingYes_CheckedChanged" AutoPostBack="true" runat="server" GroupName="CheckParentSetting" Text="Yes" />
                    <asp:RadioButton ID="rbtnCheckParentSettingNo" OnCheckedChanged="rbtnCheckParentSettingNo_CheckedChanged" AutoPostBack="true" runat="server" GroupName="CheckParentSetting" Text="No" />
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div id="dvDisplayRotationSettings" runat="server" class="row">
        <div class="col-md-12">
            <div id="dvNodeRotationFieldOptionSetting" runat="server" class="table-responsive">
                <table class="table table-bordered">
                    <tbody>
                        <tr>
                            <td><span class="cptn">Rotation ID/Name </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnRotationNameYes" runat="server" GroupName="RotationName" Text="Required" />
                                <asp:RadioButton ID="rbtnRotationNameNo" runat="server" GroupName="RotationName" Text="Optional" />
                            </td>

                            <td>
                                <span class="cptn">Type/Specialty </span>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbtnTypeYes" runat="server" GroupName="RotationType" Text="Required" />
                                <asp:RadioButton ID="rbtnTypeNo" runat="server" GroupName="RotationType" Text="Optional" />
                            </td>
                            <td><span class="cptn">Department </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnDepartmentYes" runat="server" GroupName="RotationDept" Text="Required" />
                                <asp:RadioButton ID="rbtnDepartmentNo" runat="server" GroupName="RotationDept" Text="Optional" />
                            </td>
                        </tr>
                        <tr>
                            <td><span class="cptn">Program </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnProgramYes" runat="server" GroupName="RotationProgram" Text="Required" />
                                <asp:RadioButton ID="rbtnProgramNo" runat="server" GroupName="RotationProgram" Text="Optional" />
                            </td>
                            <td><span class="cptn">Course </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnCourseYes" runat="server" GroupName="RotationCourse" Text="Required" />
                                <asp:RadioButton ID="rbtnCourseNo" runat="server" GroupName="RotationCourse" Text="Optional" />
                            </td>
                            <td><span class="cptn">Term </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnTermYes" runat="server" GroupName="RotationTerm" Text="Required" />
                                <asp:RadioButton ID="rbtnTermNo" runat="server" GroupName="RotationTerm" Text="Optional" />
                            </td>
                        </tr>
                        <tr>
                            <td><span class="cptn">Unit/Floor or Location </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnUnitYes" runat="server" GroupName="RotationUnit" Text="Required" />
                                <asp:RadioButton ID="rbtnUnitNo" runat="server" GroupName="RotationUnit" Text="Optional" />
                            </td>
                            <td><span class="cptn"># of Students </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnStudentYes" runat="server" GroupName="RotationStudent" Text="Required" />
                                <asp:RadioButton ID="rbtnStudentNo" runat="server" GroupName="RotationStudent" Text="Optional" />
                            </td>
                            <td><span class="cptn"># of Recommended Hours </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnHourYes" runat="server" GroupName="RotationHours" Text="Required" />
                                <asp:RadioButton ID="rbtnHourNo" runat="server" GroupName="RotationHours" Text="Optional" />
                            </td>
                        </tr>
                        <tr>
                            <td><span class="cptn">Days </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnDaysYes" runat="server" GroupName="RotationDays" Text="Required" />
                                <asp:RadioButton ID="rbtnDaysNo" runat="server" GroupName="RotationDays" Text="Optional" />
                            </td>
                            <td><span class="cptn">Shift </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnShiftYes" runat="server" GroupName="RotationShift" Text="Required" />
                                <asp:RadioButton ID="rbtnShiftNo" runat="server" GroupName="RotationShift" Text="Optional" />
                            </td>
                            <td><span class="cptn">Instructor/Preceptor </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnIPYes" runat="server" GroupName="RotationIP" Text="Required" />
                                <asp:RadioButton ID="rbtnIPNo" runat="server" GroupName="RotationIP" Text="Optional" />
                            </td>
                        </tr>
                        <tr>
                            <td><span class="cptn">Start Time </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnStartTimeYes" runat="server" GroupName="RotationStartTime" Text="Required" />
                                <asp:RadioButton ID="rbtnStartTimeNo" runat="server" GroupName="RotationStartTime" Text="Optional" />
                            </td>
                            <td><span class="cptn">End Time </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnEndTimeYes" runat="server" GroupName="RotationEndTime" Text="Required" />
                                <asp:RadioButton ID="rbtnEndTimeNo" runat="server" GroupName="RotationEndTime" Text="Optional" />
                            </td>
                            <td><span class="cptn">Syllabus Document </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnSyllabusYes" runat="server" GroupName="RotationSyllabus" Text="Required" />
                                <asp:RadioButton ID="rbtnSyllabusNo" runat="server" GroupName="RotationSyllabus" Text="Optional" />
                            </td>
                        </tr>
                        <tr>
                            <td style="display:none"><span class="cptn">Days Before </span></td>
                            <td style="display:none">
                                <asp:RadioButton ID="rbtnDaysBeforeYes" runat="server" GroupName="RotationDaysBefore" Text="Required" />
                                <asp:RadioButton ID="rbtnDaysBeforeNo" runat="server" GroupName="RotationDaysBefore" Text="Optional" />
                            </td>
                            <td style="display:none"><span class="cptn">Frequency </span></td>
                            <td style="display:none">
                                <asp:RadioButton ID="rbtnFrequencyYes" runat="server" GroupName="RotationFrequency" Text="Required" />
                                <asp:RadioButton ID="rbtnFrequencyNo" runat="server" GroupName="RotationFrequency" Text="Optional" />
                            </td>
                            <td><span class="cptn">Deadline Date </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnDeadlineDateYes" runat="server" GroupName="RotationDeadline" Text="Required" />
                                <asp:RadioButton ID="rbtnDeadlineDateNo" runat="server" GroupName="RotationDeadline" Text="Optional" />
                            </td>
                             <td><span class="cptn">Additional Document(s) </span></td>
                            <td>
                                <asp:RadioButton ID="rbtnAdditionalDocumentsYes" runat="server" GroupName="RotationDocuments" Text="Required" />
                                <asp:RadioButton ID="rbtnAdditionalDocumentsNo" runat="server" GroupName="RotationDocuments" Text="Optional" />
                            </td>
                            
                        </tr>
                         
                    </tbody>
                </table>
            </div>

            <div class="clearfix"></div>
            <div class="form-group pull-right">
                <infsu:CommandBar ID="fsucCmdBar" runat="server" ButtonPosition="Right" DisplayButtons="Submit" UseAutoSkinMode="false" ButtonSkin="Silk"
                    AutoPostbackButtons="Submit" OnSubmitClick="fsucCmdBar_SubmitClick" SubmitButtonIconClass="rbSave" SubmitButtonText="Save" ValidationGroup="grpFormSubmit">
                </infsu:CommandBar>
            </div>
        </div>

    </div>
</div>
