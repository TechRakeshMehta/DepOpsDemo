using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Entity.Navigation
{
    internal class MenuDAL
    {
        private static readonly String _connectionString = ConfigurationManager.ConnectionStrings["ConnectionName"].ConnectionString;

        internal static String ConnectionString
        {
            get { return _connectionString; }
        }

        #region Constants
        private const string _USP_GETMENUITEMS = "GetMenuFeatures";
        private const string _USP_GETMENUITEMSALL = "GetAdminMenuFeatures";
        private const string _PARAM_USERID = "UserID";
        private const string _PARAM_BLOCKID = "BlockID";
        private const string _PARAM_PRODFEATUREID = "ProductFeatureID";
        private const string _PARAM_PARENTPRODFEATUREID = "ParentProductFeatureID";
        private const string _PARAM_NAME = "Name";
        private const string _PARAM_DESCRIPTION = "Description";
        private const string _PARAM_NAVURL = "NavigationURL";
        private const string _PARAM_PERMISSIONTYPEID = "PermissionTypeId";
        private const string _PARAM_UICONTROLID = "UIControlID";
        private const string _PARAM_TENANTID = "TenantID";
        private const string _PARAM_ISREACTAPPURL = "IsReactAppUrl";//Admin Entry Portal

        #region AMS

        private const string _PARAM_UI_BUSINESSCHANNEL_TYPEID = "BusinessChannelTypeID";

        #endregion

        #endregion

        internal List<MenuViewItem> GetMenuItems(string UserID, int BlockID, Int16 BusinessChannelTypeID)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(_USP_GETMENUITEMS, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(_PARAM_USERID, UserID);
                    cmd.Parameters.AddWithValue(_PARAM_BLOCKID, BlockID);

                    //AMS
                    cmd.Parameters.AddWithValue(_PARAM_UI_BUSINESSCHANNEL_TYPEID, BusinessChannelTypeID);
                    //End

                    cmd.Connection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        return ExecuteReader(dr);
                    }
                }
            }
        }

        internal List<MenuViewItem> GetAllMenuItems(Int16 BusinessChannelTypeID)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(_USP_GETMENUITEMSALL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(_PARAM_UI_BUSINESSCHANNEL_TYPEID, BusinessChannelTypeID);
                    cmd.Connection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        return ExecuteReader(dr);
                    }
                }
            }
        }

        private List<MenuViewItem> ExecuteReader(SqlDataReader dr)
        {
            List<MenuViewItem> retValue = new List<MenuViewItem>();

            while (dr.Read())
            {
                retValue.Add(new MenuViewItem()
                {
                    ID = (int)dr[_PARAM_PRODFEATUREID],
                    ParentID = dr[_PARAM_PARENTPRODFEATUREID] is DBNull ? (int?)null : (int)dr[_PARAM_PARENTPRODFEATUREID],
                    Text = dr[_PARAM_NAME] is DBNull ? null : (string)dr[_PARAM_NAME],
                    URL = dr[_PARAM_NAVURL] is DBNull ? null : (string)dr[_PARAM_NAVURL],
                    Tooltip = dr[_PARAM_DESCRIPTION] is DBNull ? null : (string)dr[_PARAM_DESCRIPTION],
                    PermissionTypeId = (PermissionTypeEnum)(int)dr[_PARAM_PERMISSIONTYPEID],
                    UIControlID = dr[_PARAM_UICONTROLID] is DBNull ? null : (string)dr[_PARAM_UICONTROLID],
                    TenantID = dr[_PARAM_TENANTID] is DBNull ? -1 : Convert.ToInt32(dr[_PARAM_TENANTID]),
                    IsReactAppUrl = dr[_PARAM_ISREACTAPPURL] is DBNull ? false : Convert.ToBoolean(dr[_PARAM_ISREACTAPPURL]),// Admin Entry Portal
                });

            }

            return retValue;
        }
    }

    [Serializable]
    public class MenuViewItem
    {
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string Text { get; set; }
        public string URL { get; set; }
        public string Tooltip { get; set; }
        public PermissionTypeEnum? PermissionTypeId { get; set; }
        public string UIControlID { get; set; }
        public Int32 TenantID { get; set; }
        public Boolean IsReactAppUrl { get; set; } //Admin Entry Portal
    }

    public enum PermissionTypeEnum
    {
        FullAccess = 1,
        ReadOnly = 2,
        NoAccess = 3
    }
}
