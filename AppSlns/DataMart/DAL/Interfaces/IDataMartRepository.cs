using Entity;
using Entity.SharedDataEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataMart.DAL.Interfaces
{
    public interface IDataMartRepository
    {
        #region Manage Agency

        IQueryable<AgencyUser> GetAgencyUsers();

        IEnumerable<DataRow> GetAgenciesOfAgencyUser();

        IQueryable<ProfileSharingInvitationGroup> GetProfileSharingInvitationGroups();

        IEnumerable<DataRow> GetSharedItemsOfInvitationGroup(String invitationGroupIDs);

        IEnumerable<DataRow> GetModifiedAgencyUsers(DateTime lastSyncDate);

        IEnumerable<DataRow> GetModifiedInvitationGroups(DateTime lastSyncDate);

        IEnumerable<DataRow> GetModifiedRotationDetails(DateTime lastSyncDate);

        List<ClientDBConfiguration> GetClientDBConfigurations();

        IEnumerable<DataRow> GetRotationDetailsOfInvitationGroup(String invitationGroupIDs);

        #endregion
    }
}
