using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.Templates.Views
{
    public class TemplatesMarkupPresenter : Presenter<ITemplatesMarkupView>
    {
        /// <summary>
        /// Method used to get Templates details in which we are only passing template content and name on the main page.
        /// </summary>
        public void GetTemplateDetails()
        {
            View.SystemEventTemplate = TemplatesManager.GetTemplateDetails(View.TemplateId);
            View.TemplateName = View.SystemEventTemplate.TemplateName;
            View.TemplateContent = View.SystemEventTemplate.TemplateContent;
        }
    }
}
