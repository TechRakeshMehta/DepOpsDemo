using System;
using System.Collections.Generic;

namespace WidgetDataWebService.Model
{
    [Serializable]
    public class CustRecordset
    {
        public string Name { get; set; }
        public List<List<CustDictionary>> Rows { get; set; }

        public CustRecordset()
        {
            Rows = new List<List<CustDictionary>>();
        }
    }
}