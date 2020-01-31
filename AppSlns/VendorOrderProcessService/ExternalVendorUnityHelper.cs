#region Header Comment BaseUserControl

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ExternalVendorUnityHelper.cs
// Purpose:   
//

#endregion

#region Namespaces

using ExternalVendors;
using ExternalVendors.Interface;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

#endregion

namespace VendorOrderProcessService
{
    public class ExternalVendorUnityHelper
    {
        private static IUnityContainer _serviceContainer;
        private static Hashtable _serviceAdapters;

        public static IVendorServiceAdapter VendorServiceAdapter(String vendorName)
        {
            //check in hash table for vendorName, if vendorkey exists then return value of that key i.e. _iVendorServiceAdapter 
            //if not exists, then add vendorName key and _iVendorServiceAdapter value in hash table.
            if (!_serviceAdapters.ContainsKey(vendorName))
            {
                String adapterKey = ConfigurationManager.AppSettings[vendorName].ToString();
                _serviceAdapters.Add(vendorName, _serviceContainer.Resolve<IVendorServiceAdapter>(adapterKey));
            }

            return (IVendorServiceAdapter)_serviceAdapters[vendorName];
        }

        static ExternalVendorUnityHelper()
        {
            _serviceAdapters = new Hashtable();
            _serviceContainer = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(_serviceContainer);
        }

        /// <summary>
        /// This method provide the instance of class which implements the interface (T) on basis of its config repoName setting 
        /// </summary>
        /// <param name="repoName">config repoName setting</param>
        /// <returns>instance of class</returns>
        public static void SetVendorServiceAdapter(string vendorName)
        {
            ////check in hash table for vendorName, if vendorkey exists then return value of that key i.e. _iVendorServiceAdapter 
            ////if not exists, then add vendorName key and _iVendorServiceAdapter value in hash table.
            //if (!_serviceAdapters.ContainsKey(vendorName))
            //{
            //    String adapterKey = ConfigurationManager.AppSettings[vendorName].ToString();
            //    _serviceAdapters.Add(vendorName, _serviceContainer.Resolve<IVendorServiceAdapter>(adapterKey));
            //}

            //_vendorName = vendorName;
        }
    }
}
