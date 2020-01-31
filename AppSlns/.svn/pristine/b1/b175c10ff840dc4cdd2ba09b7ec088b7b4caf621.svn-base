using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServiceLibrary
{
    public class ValidateTokenFactory
    {
        #region Private Variables
        private static Object _lock = new Object();
        private static ValidateTokenFactory _this;
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Private Methods
        static ValidateTokenFactory()
        {
            lock (_lock)
            {
                if (_this == null)
                {
                    _this = new ValidateTokenFactory();
                }
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>ValidateTokenFactory</returns>
        /// <remarks></remarks>
        public static ValidateTokenFactory GetInstance()
        {
            return _this;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <returns>ILogger</returns>
        /// <remarks></remarks>
        public AceMapValidation GetAceMapValidation()
        {
            return new AceMapValidation();
        }

        public ValidateToken GetValidationTokenClass(String mappingCode)
        {
            ValidateToken validationTokenObject = null;

            if (String.Compare(mappingCode, ClientServiceConstant.AceMap_MAPPING_GROUP_GUID, true) == 0)
            {
                validationTokenObject = new AceMapValidation();
            }
            return validationTokenObject;
        }

        #endregion

        #endregion
    }
}
