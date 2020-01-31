using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class ManagePlacementConfiguration : BaseUserControl
    {

        protected override void OnInit(EventArgs e)
        {
            try
            {
                String title = "Placement Matching Setup";
                base.Title = title;
                base.SetPageTitle(title);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                hdnSelected.Value = "Department";
                liDepartment.Attributes.Add("class", "li active");

            }
            switch (hdnSelected.Value)
            {
                case "Department":
                    {
                        cnPlacementDepartment.Visible = true;
                        cnPlacementSpecialty.Visible = false;
                        cnPlacementStudentType.Visible = false;
                        liDepartment.Attributes.Add("class", "li active");
                        liStudentType.Attributes.Remove("class");
                        liSpecialty.Attributes.Remove("class");
                        break;
                    }
                case "StudentType":
                    {
                        cnPlacementDepartment.Visible = false;
                        cnPlacementSpecialty.Visible = false;
                        cnPlacementStudentType.Visible = true;
                        liStudentType.Attributes.Add("class", "li active");
                        liDepartment.Attributes.Remove("class");
                        liSpecialty.Attributes.Remove("class");
                        break;
                    }
                case "Specialty":
                    {
                        cnPlacementDepartment.Visible = false;
                        cnPlacementSpecialty.Visible = true;
                        cnPlacementStudentType.Visible = false;
                        liSpecialty.Attributes.Add("class", "li active");
                        liDepartment.Attributes.Remove("class");
                        liStudentType.Attributes.Remove("class");
                        break;
                    }
                default:
                    {
                        cnPlacementDepartment.Visible = true;
                        cnPlacementSpecialty.Visible = false;
                        cnPlacementStudentType.Visible = false;
                        liDepartment.Attributes.Add("class", "li active");
                        liStudentType.Attributes.Remove("class");
                        liSpecialty.Attributes.Remove("class");
                        break;
                    }
            }
        

        }

        protected void tabDepartment_ServerClick(object sender, EventArgs e)
        {

        }

        protected void tabStudentType_ServerClick(object sender, EventArgs e)
        {

        }

        protected void tabSpecialty_ServerClick(object sender, EventArgs e)
        {

        }
    }
}