#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ServiceClientWrapper.cs
// Purpose:   Wrapper Class
//

#endregion

#region Namespace

#region System Defined
using SPI;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
#endregion

#endregion

namespace CoreWeb.IntsofCachingModel.Wrapper
{
    /// <summary>
    /// ServiceClientWrapper 
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    internal class ServiceClientWrapper<T> : IDisposable
    {
        #region Private Variables
        private T _channel;
        #endregion

        #region Public Properties
        /// <summary>
        /// Channel
        /// </summary>
        public T Channel
        {
            get { return _channel; }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Contructor ServiceClientWrapper
        /// </summary>
        public ServiceClientWrapper()
        {
            _channel = SysXNetDataContractSerializerOperationBehavior.CreateClientChannel<T>("TcpSysXCaching");
        }

        #endregion

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (((IChannel)_channel).State == CommunicationState.Faulted)
                {
                    ((IChannel)_channel).Abort();
                }
                else
                {
                    ((IChannel)_channel).Close();
                }
            }
            catch
            {
            }
        }
    }
}
