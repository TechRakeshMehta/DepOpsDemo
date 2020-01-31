using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.Messaging
{
   public class MessageSearchContract
    {
       public Int32 UserGroupID
       {
           get;
           set;
       }
       public String SenderId
        {
            get;
            set;
        }


       public String ToUserList
        {
            get;
            set;
        }

        public String MessageBody
        {
            get;
            set;
        }

        public String Subject
        {
            get;
            set;
        }
    }
}
