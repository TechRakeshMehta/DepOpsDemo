﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.QueueManagement
{
    public class OrganizationUserContract
    {
        public Int32 OrganizationUserId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String FullName { get; set; }
        public String EmailAddress { get; set; }
    }
}
