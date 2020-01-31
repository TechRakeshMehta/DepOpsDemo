using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.SystemSetUp.Views
{
    public class ManageVideosPresenter : Presenter<IManageVideos>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public override void OnViewLoaded()
        {

        }

        public void GetApplicationVideos()
        {
            View.lstApplicationVideos = SecurityManager.GetApplicationVideos().ToList();
        }

        public void GetVideoType()
        {
            View.lstVideoType = LookupManager.GetLookUpData<lkpVideoType>();
        }

        public void SaveApplicationVideos()
        {
            ApplicationVideo appVideo = new ApplicationVideo();
            appVideo.APV_Title = View.VideoTitle;
            appVideo.APV_EmbedLink = View.VideoEmbedLink;
            appVideo.APV_VideoTypeID = View.VideoTypeID;
            appVideo.APV_DirectLink = View.VideoDirectLink;
            appVideo.APV_Description = View.Description;
            appVideo.APV_IsDeleted = false;
            appVideo.APV_CreatedOn = DateTime.Now;
            appVideo.APV_IsDisplayDirectLink = View.IsDisplayDirectLink;
            appVideo.APV_IsDisplayDescription = View.IsDisplayDescription;

            appVideo.APV_CreatedByID = 1;
            if (SecurityManager.SaveApplicationVideos(appVideo))
            {
                View.SuccessMessage = "Video added successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error occurred while adding Video. Please try again or contact system administrator.";
            }
        }

        public void UpdateApplicationVideo()
        {
            ApplicationVideo appVideo = SecurityManager.GetApplicationVideo(View.ApplicationVideoID);
            appVideo.APV_Title = View.VideoTitle;
            appVideo.APV_EmbedLink = View.VideoEmbedLink;
            appVideo.APV_VideoTypeID = View.VideoTypeID;
            appVideo.APV_DirectLink = View.VideoDirectLink;
            appVideo.APV_ModifiedOn = DateTime.Now;
            appVideo.APV_ModifiedByID = 1;
            appVideo.APV_Description = View.Description;
            appVideo.APV_IsDisplayDirectLink = View.IsDisplayDirectLink;
            appVideo.APV_IsDisplayDescription = View.IsDisplayDescription;

            if (SecurityManager.UpdateApplicationVideo(appVideo))
            {
                View.SuccessMessage = "Video updated successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error occurred while updating Video. Please try again or contact system administrator.";
            }
        }

        public void DeleteApplicationVideo()
        {
            ApplicationVideo appVideo = SecurityManager.GetApplicationVideo(View.ApplicationVideoID);
            appVideo.APV_IsDeleted = true;
            appVideo.APV_ModifiedOn = DateTime.Now;
            appVideo.APV_ModifiedByID = 1;

            if (SecurityManager.UpdateApplicationVideo(appVideo))
            {
                View.SuccessMessage = "Video deleted successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error occurred while deleting Video. Please try again or contact system administrator.";
            }
        }
    }
}
