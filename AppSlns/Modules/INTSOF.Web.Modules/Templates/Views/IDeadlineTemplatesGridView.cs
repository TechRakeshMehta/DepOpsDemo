using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Templates.Views
{
    public interface IDeadlineTemplatesGridView
    {
        Int32 CurrentLoggedInUserId { get; }

        String SubEventCode
        {
            get;
            set;
        }

        String NodeNotificationTypeCode
        {
            get;
            set;
        }

        /// <summary>
        /// Could be NNM_Id of the Root Node or Current Node,
        /// depending on the 'TemplateNodeLevel' property
        /// </summary>
        Int32 NodeNNMId
        {
            get;
            set;
        }

        Int32 SelectedTenantId { get; set; }

        /// <summary>
        /// Used to identify whether the screen is for Nage Emails or Deadline Emails
        /// </summary>
        String TemplateNodeLevel
        {
            get;
            set;
        }
    }
}
