
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.DataFeed_Framework
{
    [Serializable]
    public class OutputColumn
    {
        /// <summary>
        /// Display order of the column in output
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Header of the column in output
        /// </summary>
        public String DisplayHeader { get; set; }
        /// <summary>
        /// Xpath in source xml to identify data of the column
        /// </summary>
        public String DataXPath { get; set; }

        public Boolean IsServiceGroup { get; set; }

        public Boolean IsCustomAttribute { get; set; }

        public String HeaderTextName { get; set; }
    }

    public class BkgOrderSettings
    {
        public bool OnlyNew { get; set; }
        public bool IncludeServiceGroups { get; set; }
        public bool IncludeAllServiceGroups { get; set; }
        public bool IncludeCustomFields { get; set; }
    }
    public class EntityFilterType
    {
        public String Key { get; set; }
        public String Value { get; set; }
    }
}
