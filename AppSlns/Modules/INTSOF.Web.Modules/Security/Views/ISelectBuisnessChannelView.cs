using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.IntsofSecurityModel;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface ISelectBuisnessChannelView
    {
        #region Variables

        #endregion

        #region Properties

       

        /// <summary>
        /// OrganizationUserID</summary>
        /// <value>
        /// Gets or sets the value for organization user's id.</value>
        Int32 OrganizationUserId
        {
            get;
            set;
        }

        /// <summary>
        /// DefaultLineOfBusiness</summary>
        /// <value>
        /// Gets or sets the value for Default Line of Business.</value>
        String DefaultLineOfBusiness
        {
            get;
            set;
        }


        /// <summary>
        /// Gets the selected block id.
        /// </summary>
        /// <remarks></remarks>
        Int32 SelectedBlockId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the selected block.
        /// </summary>
        /// <remarks></remarks>
        String SelectedBlockName
        {
            get;
            set;
        }
       

        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}




