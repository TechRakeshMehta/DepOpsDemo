using System;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface IEditServiceDetailView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        Int32 BkgPackageSrvcId
        {
            get;
            set;
        }

        BkgPackageSvc CurrentBkgPackageSvc
        {
            get;
            set;
        }

        Int32 BackgroundServiceId
        {
            get;
            set;
        }

        String ServiceName
        {
            get;
            set;
        }

        String DisplayName
        {
            get;
            set;
        }

        String Notes
        {
            get;
            set;
        }

        Int32? ResidenceDuration
        {
            get;
            set;
        }

        Boolean SendDocsToStudent
        {
            get;
            set;
        }

        Boolean IsSupplemental
        {
            get;
            set;
        }

        Boolean IgnoreRHOnSupplement
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
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

        String InfoMessage
        {
            get;
            set;
        }

        //UAT-3109
        String AMERNumber
        {
            get;
            set;
        }

    }
}
