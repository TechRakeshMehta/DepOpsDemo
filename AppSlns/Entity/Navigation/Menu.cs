using INTSOF.Utils;
using System;
using System.Collections.Generic;

namespace Entity.Navigation
{
    public class Menu
    {
        #region Members
        private string _userGUID = String.Empty;
        private int _sysXBlockID = 0;
        private List<MenuViewItem> _menuItems;
        private MenuDAL _menuDAL;
        private readonly bool _isAdmin = false;


        #region AMS

        private Int16 _businessChannelTypeID = 1;

        #endregion


        #endregion

        /// <summary>
        /// Object instance will restrict navigational menu to only those items which the specific user has access to
        /// </summary>
        /// <param name="UserGUID">string UserID GUID value</param>
        /// <param name="SysXBlockID">int System X Block ID</param>
        public Menu(string UserGUID, int SysXBlockID, BusinessChannelTypeMappingData businessChannelType)
        {
            _userGUID = UserGUID ?? String.Empty;
            _sysXBlockID = SysXBlockID;
            _menuDAL = new MenuDAL();
            _businessChannelTypeID = businessChannelType == null ? (short)1 : businessChannelType.BusinessChannelTypeID;

        }

        /// <summary>
        /// Object instance will not restrict navigational menu based upon user (all items populated). 
        /// Caution: to be used only in the case of the Admin user(s)
        /// </summary>
        public Menu(BusinessChannelTypeMappingData businessChannelType)
        {
            _menuDAL = new MenuDAL();
            _isAdmin = true;
            _businessChannelTypeID = businessChannelType == null ? (short)1 : businessChannelType.BusinessChannelTypeID;
        }

        /// <summary>
        /// The RADMenu control datasource
        /// </summary>
        public List<MenuViewItem> MenuItems
        {
            get
            {
                if (_menuItems == null)
                    _menuItems = GetMenuItems();

                return _menuItems;
            }
        }

        private List<MenuViewItem> GetMenuItems()
        {
            if (!_isAdmin)
                return _menuDAL.GetMenuItems(_userGUID, _sysXBlockID, _businessChannelTypeID);
            else
                return _menuDAL.GetAllMenuItems(_businessChannelTypeID);
        }
    }
}
