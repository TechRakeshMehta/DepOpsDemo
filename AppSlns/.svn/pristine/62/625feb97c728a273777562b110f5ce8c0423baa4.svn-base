using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClientServiceLibrary.Utility
{
    public static class XMLToJSONConverter
    {
        /// <summary>
        /// XML To JSON converter
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static String XMLToJSONConversion(string xml)
        {
            //Load XML
            System.Xml.Linq.XDocument input = System.Xml.Linq.XDocument.Parse(xml);

            //Return JSON String
            return JsonConvert.SerializeXNode(input, Newtonsoft.Json.Formatting.None, true);

        }
    }
}
