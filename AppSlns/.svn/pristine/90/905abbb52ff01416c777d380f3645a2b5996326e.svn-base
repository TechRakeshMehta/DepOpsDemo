using CoreWeb.AgencyHierarchy.Views;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class PackageCategoryDetailsPopUp : BaseUserControl,IPackageCategoryDetailsPopUpView
    {

        public Int32 RequirementPackageID { get; set; }
        public List<RequirementCategoryContract> lstRequirementCategory
        {
            get;
            set;
        }
        private PackageCategoryDetailsPopUpPresenter _presenter = new PackageCategoryDetailsPopUpPresenter();


        public PackageCategoryDetailsPopUpPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public IPackageCategoryDetailsPopUpView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        //public Int32 PackageId { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            Presenter.GetRequirementPackageCategories(RequirementPackageID);

            BindPackageCategoryPopup(lstRequirementCategory);
        }

        private void BindPackageCategoryPopup(List<RequirementCategoryContract> lstRequirementCategory)
        {
            if (lstRequirementCategory!=null)
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append(@"<table border='0' cellpadding='0' cellspacing='0'>");
                strBuilder.Append("<tr><th> Category Name </th><th> Category Label </th></tr>");

                lstRequirementCategory.ForEach(gen =>
                {
                    strBuilder.AppendFormat("<tr><td width='50%'>{0}</td><td width='50%'>{1}</td></tr>", "<li>" + gen.RequirementCategoryName.HtmlEncode() + "</li>", "<li>" + gen.RequirementCategoryLabel.HtmlEncode() + "</li>");
                });
                strBuilder.AppendFormat("</table>");

                divNameOfPackageCategory.InnerHtml = strBuilder.ToString();
            }
            else
            {
                lblNameOfPackageCategory.Text = "Category not found.";
                divNameOfPackageCategory.Visible = false;
            }
        }
    }
}