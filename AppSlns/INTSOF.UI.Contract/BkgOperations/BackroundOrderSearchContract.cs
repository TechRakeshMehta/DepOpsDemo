using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class BackroundOrderSearchContract
    {
        private List<BackroundOrderContract> _backroundOrder;
        private List<BackroundServiceGroupContract> _backroundServiceGroup;
        private List<BackroundServicesContract> _backroundServices;

        public List<BackroundOrderContract> BackroundOrder
        {
            get
            {
                if (_backroundOrder == null) _backroundOrder = new List<BackroundOrderContract>();
                return _backroundOrder;
            }

            set
            {
                _backroundOrder = value;

            }

        }

        public List<BackroundServiceGroupContract> BackroundServiceGroup
        {
            get
            {
                if (_backroundServiceGroup == null) _backroundServiceGroup = new List<BackroundServiceGroupContract>();
                return _backroundServiceGroup;
            }

            set
            {
                _backroundServiceGroup = value;

            }

        }
        public List<BackroundServicesContract> BackroundServices
        {
            get
            {
                if (_backroundServices == null) _backroundServices = new List<BackroundServicesContract>();
                return _backroundServices;
            }

            set
            {
                _backroundServices = value;

            }
        }
    }
}
