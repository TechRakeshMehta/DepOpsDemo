using System;
using System.Configuration;
using System.ServiceModel;
using INTSOF.Utils.Enums;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace INTSOF.ServiceProxy.Core
{
    public class BaseServiceProxy<TServiceInterface>
    {
        /// <summary>
        /// The _factory
        /// </summary>
        private ChannelFactory<TServiceInterface> _factory = null;

        /// <summary>
        /// The _client
        /// </summary> 
        protected TServiceInterface ServiceChannel { get; set; }

        /// <summary>
        /// The _active user
        /// </summary>
        protected UserContext _activeUser;

        /// <summary>
        /// Initializing channel factory switch between ref as dll or ref as service.
        /// </summary>
        protected BaseServiceProxy(String serviceKey)
        {
            CheckInitialized();

            String setUpWSURL = ConfigurationManager.AppSettings[serviceKey];

            if (String.IsNullOrEmpty(setUpWSURL))
            {
                throw new Exception(serviceKey + " was not found in the system configuration.");
            }
            ServiceChannel = GetClient(setUpWSURL);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        protected TServiceInterface GetClient(string url)
        {
            if(IsAssemblyBindingEnabled)
            {
                return ExtractFromAssembly();
            }

            if (_factory == null)
            {
                var _timeoutTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings[ServiceAppConstants.WCFSERVICE_TIMEOUT_KEY]));
                var _maxReceivedMsgSize = 2147483647;

                EndpointAddress _endPointAddress = new EndpointAddress(url);
                if (url.ToLower().StartsWith("https"))
                {
                    WSHttpBinding _wsHttpBinding = new WSHttpBinding(SecurityMode.Transport);
                    _wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    _wsHttpBinding.MaxReceivedMessageSize = _maxReceivedMsgSize;
                    _wsHttpBinding.SendTimeout = _timeoutTimeSpan;
                    _factory = new ChannelFactory<TServiceInterface>(_wsHttpBinding, _endPointAddress);
                }
                else if (url.ToLower().StartsWith("net.tcp"))
                {
                    NetTcpBinding _netTcpBinding = new NetTcpBinding(SecurityMode.None);
                   // _netTcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
                    _netTcpBinding.MaxReceivedMessageSize = _maxReceivedMsgSize;
                    _netTcpBinding.SendTimeout = _timeoutTimeSpan;
                    _factory = new ChannelFactory<TServiceInterface>(_netTcpBinding, _endPointAddress);

                }
                else
                {
                    BasicHttpBinding _basicHttpBinding = new BasicHttpBinding();
                    _basicHttpBinding.MaxReceivedMessageSize = _maxReceivedMsgSize;
                    _basicHttpBinding.SendTimeout = _timeoutTimeSpan;
                    _factory = new ChannelFactory<TServiceInterface>(_basicHttpBinding, _endPointAddress);
                }

                _factory.Endpoint.EndpointBehaviors.Add(new ServiceBehavior());
            }
            return _factory.CreateChannel();
        }

        private TServiceInterface ExtractFromAssembly()
        {
            UnityContainer serviceContainer = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity"); 
            section.Configure(serviceContainer);

            TServiceInterface serviceInterface = serviceContainer.Resolve<TServiceInterface>(typeof(TServiceInterface).Name);
            return serviceInterface;
        }

        protected UserContext ActiveUser
        {
            get
            {
                if (_activeUser == null)
                {
                    ISysXSessionService sessionService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSessionService;

                    if (sessionService.IsNotNull())
                    {
                        _activeUser = new UserContext();

                        short businessChannelTypeID = AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE;
                        if (sessionService.BusinessChannelType != null)
                        {
                            businessChannelTypeID = sessionService.BusinessChannelType.BusinessChannelTypeID;
                        }

                        _activeUser.IsSysXAdmin = sessionService.IsSysXAdmin;
                        _activeUser.OrganizationUserId = sessionService.OrganizationUserId;
                        _activeUser.SysXBlockId = sessionService.SysXBlockId;
                        _activeUser.SysXBlockName = sessionService.SysXBlockName;
                        _activeUser.UserID = sessionService.UserId;
                        _activeUser.BusinessChannelTypeID = businessChannelTypeID;
                    }
                }
                return _activeUser;
            }
            set
            {
                _activeUser = value;
            }
        }

        protected void CheckInitialized()
        {
            if (ActiveUser == null)
            {
                throw new InvalidOperationException("Proxy not initialized.");
            }
            else
            {
                RequestData.ActiveUser = _activeUser;
            }
        }

        protected Boolean IsAssemblyBindingEnabled
        {
            get
            {
                return ConfigurationManager.AppSettings["IsAssemblyBindingEnabled"].IsNotNull()
                                        ? Convert.ToBoolean(ConfigurationManager.AppSettings["IsAssemblyBindingEnabled"])
                                        : false;
            }
        }

    }
}
