using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceUtil
{
    public static class ClientDataUploadHelper
    {
        public static IDataUploadServiceAdapter ClientDataUploadAdapter()
        {
            return new DataUploadServiceAdapter();
        }

    }
}
