using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Serialization;


namespace INTSOF.Utils
{
    [Serializable]
    public class CustomPagingArgsContract
    {
        #region Sort options

        /// <summary>
        /// Get or set grid sort expression (if any)
        /// </summary>
        public String SortExpression
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set grid sort direction, true if descending 
        /// </summary>
        public Boolean SortDirectionDescending
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set grid default sort expression (if SortExpression is not found)
        /// </summary>
        public String DefaultSortExpression
        {
            get;
            set;
        }

        public String SecondarySortExpression
        {
            get;
            set;
        }

        #endregion

        #region Paging Options

        /// <summary>
        /// Get or set current page index of grid
        /// </summary>
        public Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set page size of grid
        /// </summary>
        public Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set virtual page count of grid
        /// </summary>
        public Int32 VirtualPageCount
        {
            get;
            set;
        }

        #endregion

        #region Filter option

        /// <summary>
        /// Get or set filter fields name
        /// </summary>
        public List<String> FilterColumns
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set filter operators name
        /// </summary>
        public List<String> FilterOperators
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set filter values
        /// </summary>
        public ArrayList FilterValues
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set filter types
        /// </summary>
        public List<String> FilterTypes
        {
            get;
            set;
        }

        /// <summary>
        /// If the current filteration is done on Extended Property
        /// </summary>
        public Boolean UseExtendedProperty
        {
            get;
            set;
        }

        #endregion

        public String XML
        {
            get
            {
                return CreateXml();
            }
        }

        public String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(CustomPagingXmlContract));
            var sb = new StringBuilder();

            CustomPagingXmlContract xmlData = new CustomPagingXmlContract();
            if (this.FilterColumns.IsNotNull())
            {
                xmlData.FilterData = this.FilterColumns.Select((col, index) => new FilteringData { FilterColumn = col, FilterOperator = this.FilterOperators[index], FilterValue = FormatSQLValue(this.FilterValues[index],this.FilterTypes[index]) }).ToList();
            }

            xmlData.SortExpression = this.SortExpression;
            xmlData.SecondarySortExpression = this.SecondarySortExpression;
            xmlData.SortDirectionDescending = this.SortDirectionDescending;
            xmlData.PageSize = this.PageSize;
            xmlData.PageIndex = this.CurrentPageIndex;
            xmlData.DefaultSortExpression = this.DefaultSortExpression;

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }


            return sb.ToString();
        }

        private string FormatSQLValue(dynamic someValue,String filterType)
        {
            string FormattedValue = "";

            if (someValue == null)
            {
                FormattedValue = "NULL";
            }
            else
            {
                String type = someValue.GetType().Name;
                switch (type)
                {
                    case "System.String": FormattedValue = "''" + (someValue).Replace("'", "''") + "''"; break;
                    case "System.DateTime": FormattedValue = "''" + (someValue).ToString("yyyy/MM/dd hh:mm:ss") + "''"; break;
                    case "System.Boolean": FormattedValue = someValue ? "1" : "0"; break;
                    default: FormattedValue = someValue.ToString(); break;
                }
            }
            return FormattedValue;
        }


        [Serializable]
        public class CustomPagingXmlContract
        {
            [DataMember]
            public List<FilteringData> FilterData
            {
                get;
                set;
            }

            [DataMember]
            public Int32 PageSize
            {
                get;
                set;
            }

            [DataMember]
            public Int32 PageIndex
            {
                get;
                set;
            }

            [DataMember]
            public String SortExpression
            {
                get;
                set;
            }

            [DataMember]
            public String SecondarySortExpression
            {
                get;
                set;
            }

            [DataMember]
            public Boolean SortDirectionDescending
            {
                get;
                set;
            }
            [DataMember]
            public String DefaultSortExpression
            {
                get;
                set;
            }
        }

        [Serializable]
        public class FilteringData
        {
            [DataMember]
            public String FilterColumn { get; set; }

            [DataMember]
            public String FilterOperator { get; set; }

            [DataMember]
            public dynamic FilterValue { get; set; }
        }
    }

}
