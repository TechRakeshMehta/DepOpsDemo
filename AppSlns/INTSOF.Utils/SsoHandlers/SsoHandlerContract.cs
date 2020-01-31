using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.Utils.SsoHandlers
{
    public class SsoHandlerContract
    {

        /// <summary>
        /// This will be peopleSoftID from UCONN and wguUUID from WGU
        /// </summary>
        public String UniqueID { get; set; }

        /// <summary>
        /// This will be FirstName from and WGU.
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// This will be LastName from and WGU.
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// This will be email from WGU or eduPersonPrincipalName/Email from UCONN.
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// This will be role from both WGU as well as UCONN.
        /// </summary>
        public String Role { get; set; }

        /// <summary>
        /// This will be netid from UCONN and wguBannerID from WGU.
        /// </summary>
        public String AtributeID { get; set; }

        /// <summary>
        /// This will be username from WGU.
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// This will be Host from Sesion
        /// </summary>
        public String Host { get; set; }

        ///// <summary>
        ///// This will be targetURL from 
        ///// </summary>
        //public String TargetURL { get; set; }
        public Int32 IntegrationClientID { get; set; }

        /// <summary>
        /// contains info whether contract is made from UCONN or WGU.
        /// </summary>
        public String HandlerType { get; set; }

        /// <summary>
        /// UAT-3607
        /// contains the atributes value mapped with cutsom attributes id.
        /// </summary>
        //public Dictionary<Int32, String> dicAttributesWithID { get; set; }
        public String AttributesWithID { get; set; }

        /// <summary>
        /// contains info whether contract is made from NSC.
        /// </summary>
        public String DisplayName { get; set; }
        /// <summary>
        /// contains udc id of BSU
        /// </summary>
        public String UdcID { get; set; }
    }
}
