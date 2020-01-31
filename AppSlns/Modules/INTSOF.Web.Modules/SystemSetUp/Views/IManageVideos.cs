using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.SystemSetUp.Views
{
    public interface IManageVideos
    {
        //IManageVideos CurrentViewContext
        //{
        //    get;
        //}

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        List<Entity.ApplicationVideo> lstApplicationVideos
        {
            get;
            set;
        }

        List<Entity.lkpVideoType> lstVideoType
        {
            get;
            set;
        }

        String VideoTitle
        {
            get;
            set;
        }

        Int32 VideoTypeID
        {
            get;
            set;
        }

        Int32 ApplicationVideoID
        {
            get;
            set;
        }

        String VideoDirectLink
        {
            get;
            set;
        }

        String VideoEmbedLink
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }

        String Description
        {
            get;
            set;
        }

        Boolean IsDisplayDirectLink
        {
            get;
            set;
        }

        Boolean IsDisplayDescription
        {
            get;
            set;
        }
    }
}
