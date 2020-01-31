
using Business.RepoManagers;
using INTSOF.UI.Contract.DataFeed_Framework;
using INTSOF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Business.DataFeed_Framework.DataFeed_Formatter
{
    class DataFeedFormatterCSV : IDataFeedFormatter
    {
        /// <summary>
        /// Method to format data feed into dictionary of dictionary
        /// </summary>
        /// <param name="Type">Type</param>
        /// <param name="XmlData">XmlData</param>
        /// <param name="TenantID">TenantID</param>
        /// <param name="FormatID">FormatID</param>
        /// <returns></returns>
        public Dictionary<String, Dictionary<String, String>> FormatDataFeed(FileFormat Type, String XmlData, Int32 TenantID, Int32 FormatID, DataFeedSettingContract dataFeedSettingContract)
        {
            Dictionary<String, Dictionary<String, String>> res = new Dictionary<String, Dictionary<String, String>>();
            try
            {
                res = ConvertToDictionary(XmlData, FormatID, TenantID, dataFeedSettingContract);
                return res;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return res;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return res;
            }
        }


        /// <summary>
        /// Method to get include only new bit
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="SettingID">SettingID</param>
        /// <returns></returns>
        public Boolean GetIncludeOnlynew(Int32 tenantId, Int32 SettingID)
        {
            try
            {
                return DataFeedManager.GetIncludeOnlyNew(tenantId, SettingID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return false;
            }
        }

        /// <summary>
        ///Convert XMl file to CSV and returns dictionary of dictionary
        /// </summary>
        /// <param name="XmlData">XmlData</param>
        /// <param name="FormatID">FormatID</param>
        /// <param name="TenantID">TenantID</param>
        /// <returns></returns>
        private Dictionary<String, Dictionary<String, String>> ConvertToDictionary(String XmlData, Int32 FormatID, Int32 TenantID, DataFeedSettingContract dataFeedSettingContract)
        {
            try
            {
                Dictionary<String, Dictionary<String, String>> finaldic = new Dictionary<String, Dictionary<String, String>>();
                List<OutputColumn> outputColumns = DataFeedManager.GetFormat(TenantID, FormatID); //gets csv format with header and node xml path from db
                XDocument xdoc = XDocument.Parse(XmlData);
                var parentnode = outputColumns.Where(x => x.DisplayHeader == "##Parent").Select(x => x.DataXPath).FirstOrDefault();//name of the key node
                var attribute = outputColumns.Where(x => x.DisplayHeader == "##Attribute").Select(x => x.DataXPath).FirstOrDefault();//name of the key attribute in the key node
                var modified = outputColumns.Where(x => x.DisplayHeader == "##LastUpdatedOn").Select(x => x.DataXPath).FirstOrDefault();
                outputColumns = outputColumns.Where(x => x.DisplayHeader != "##Parent" && x.DisplayHeader != "##Attribute").ToList();
                if (dataFeedSettingContract.IncludeServiceGroup)
                {
                    TranslateServiceGroupHeaderText(outputColumns);
                }
                else
                {
                    outputColumns.RemoveAll(cond => cond.IsServiceGroup);
                }
                if (dataFeedSettingContract.IncludeCustomFields)
                {
                    TranslateCustomAttributeHeaderText(outputColumns);
                }
                else
                {
                    outputColumns.RemoveAll(cond => cond.IsCustomAttribute);
                }
                foreach (var column in outputColumns)
                {
                    IEnumerable<XElement> lstelements = ((IEnumerable)xdoc.XPathEvaluate(column.DataXPath)).Cast<XElement>();
                    foreach (XElement xelement in lstelements)
                    {
                        //find ancestor(parent) node and the attribute value
                        var parent = xelement.Ancestors(parentnode);
                        if (parent.IsNotNull())
                        {
                            var orderIDAttrbiute = parent.Attributes(attribute).Select(e => e.Value).FirstOrDefault();
                            //create dictionary with parent key if not exists 
                            if (!finaldic.ContainsKey(orderIDAttrbiute))
                            {
                                finaldic.Add(orderIDAttrbiute, new Dictionary<String, String>());
                            }

                            // add item to dictionary with key
                            if (!finaldic[orderIDAttrbiute].ContainsKey(column.DisplayHeader))
                            {
                                finaldic[orderIDAttrbiute].Add(column.DisplayHeader, xelement.Value);
                            }
                        }
                    }
                }

                return finaldic;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static void TranslateCustomAttributeHeaderText(List<OutputColumn> outputColumns)
        {
            var allCustomAttribute = outputColumns.Where(cond => cond.IsCustomAttribute);

            List<String> customAttrName = allCustomAttribute.GroupBy(i => i.HeaderTextName,
                                           (key, group) => new { key, group }).Where(cond => cond.group.Count() == AppConsts.TWO)
                                           .Select(col => col.key)
                                           .Distinct().ToList();
            customAttrName.ForEach(cName =>
            {
                List<OutputColumn> newOutputCols = outputColumns.Where(cond => cond.HeaderTextName == cName && cond.IsCustomAttribute).ToList();
                newOutputCols.ForEach(nca =>
                {
                    OutputColumn newOutputCol = outputColumns.FirstOrDefault(cond => cond.DisplayHeader == nca.DisplayHeader && cond.IsCustomAttribute).DeepClone();

                    String[] ndisplayHeader = newOutputCol.DisplayHeader.Split('[');
                    String[] nDisplayHeader1 = ndisplayHeader[1].Split(']');
                    newOutputCol.DisplayHeader = ndisplayHeader[0] + nDisplayHeader1[1];

                    outputColumns.RemoveAll(cond => cond.DisplayHeader == nca.DisplayHeader);
                    outputColumns.Add(newOutputCol);
                });
            });
        }

        private static void TranslateServiceGroupHeaderText(List<OutputColumn> outputColumns)
        {
            var allServiceGroup = outputColumns.Where(cond => cond.IsServiceGroup);
            List<String> svcGrpsName = allServiceGroup.GroupBy(i => i.HeaderTextName,
                                           (key, group) => new { key, group }).Where(cond => cond.group.Count() == AppConsts.FOUR)
                                           .Select(col => col.key)
                                           .Distinct().ToList();
            svcGrpsName.ForEach(sName =>
            {
                List<OutputColumn> newOutputCols = outputColumns.Where(cond => cond.HeaderTextName == sName).ToList();
                newOutputCols.ForEach(nc =>
                {
                    OutputColumn newOutputCol = outputColumns.FirstOrDefault(cond => cond.DisplayHeader == nc.DisplayHeader).DeepClone();
                    String[] ndisplayHeader = newOutputCol.DisplayHeader.Split('[');
                    String[] nDisplayHeader1 = ndisplayHeader[1].Split(']');
                    newOutputCol.DisplayHeader = ndisplayHeader[0] + nDisplayHeader1[1];
                    outputColumns.RemoveAll(cond => cond.DisplayHeader == nc.DisplayHeader);
                    outputColumns.Add(newOutputCol);
                });
            });
        }



        /// <summary>
        /// Converts dictionary of dictionary to CSV 
        /// </summary> 
        /// <param name="formatID">formatID</param>
        /// <returns></returns>
        public String ConvertDictionarytoCSV(Dictionary<String, Dictionary<String, String>> filteredData, DataFeedSettingContract dataFeedSettingContract)
        {
            try
            {
                List<OutputColumn> lstxmlformat = DataFeedManager.GetFormat(dataFeedSettingContract.TenantID, dataFeedSettingContract.FormatID);//gets csv format from db
                lstxmlformat = lstxmlformat.Where(x => x.DisplayOrder != 0).ToList();//removes parent value and attribute value items from list
                String titleslist = "";
                String finalData = "";
                var keys = filteredData.Select(d => d.Key).ToList();//select distinct keys from main dictionary

                if (dataFeedSettingContract.IncludeCustomFields)
                {
                    TranslateCustomAttributeHeaderText(lstxmlformat);
                }
                else
                {
                    lstxmlformat.RemoveAll(cond => cond.IsCustomAttribute);
                }
                if (dataFeedSettingContract.IncludeServiceGroup)
                {
                    TranslateServiceGroupHeaderText(lstxmlformat);
                }
                else
                {
                    lstxmlformat.RemoveAll(cond => cond.IsServiceGroup);
                }

                List<OutputColumn> orderFormatLst = lstxmlformat.OrderBy(s => s.IsServiceGroup).ThenBy(x => x.IsCustomAttribute).ToList();
                String fieldSeparator = String.Empty;
                String rowSeparator = String.Empty;
                if (dataFeedSettingContract.OutputCode.ToLower() == DataFeedUtilityConstants.LKP_OUTPUT_CSV.ToLower())
                {
                    fieldSeparator = ",";
                    rowSeparator = Environment.NewLine;
                }
                else if (dataFeedSettingContract.OutputCode.ToLower() == DataFeedUtilityConstants.LKP_OUTPUT_TEXT.ToLower())
                {
                    fieldSeparator = dataFeedSettingContract.FieldSeparator;
                    rowSeparator = dataFeedSettingContract.RowSeparator + Environment.NewLine;
                }

                foreach (var key in keys)
                {
                    Dictionary<String, String> innerdict = filteredData[key]; //select inner dictionary from main dictionary based on key
                    String str = "";
                    titleslist = "";
                    foreach (var format in orderFormatLst)
                    {
                        //header of the file.
                        titleslist += format.DisplayHeader + fieldSeparator;
                        if (innerdict.ContainsKey(format.DisplayHeader))
                        {
                            //if header data is present  
                            //str += innerdict.Where(d => d.Key == format.DisplayHeader).Select(d => d.Value).FirstOrDefault() + ",";
                            str += innerdict.Where(d => d.Key == format.DisplayHeader).Select(d => d.Value).FirstOrDefault() + fieldSeparator;
                        }
                        else if (format.DisplayHeader.Contains("IsUsed"))
                        {
                            //str += "0" + ",";
                            str += "0" + fieldSeparator;
                        }
                        else
                        {
                            //else null value
                            //str += ",";
                            str += fieldSeparator;
                        }
                    }

                    if (str.Length > 0)
                    {
                        str = str.Substring(0, str.Length - 1);
                    }
                    //str += Environment.NewLine;
                    str += rowSeparator;
                    // list is added to final list
                    finalData += str;
                }
                titleslist = titleslist.Substring(0, titleslist.Length - 1);
                //titleslist += Environment.NewLine;
                titleslist += rowSeparator;
                finalData = finalData.Insert(0, titleslist); //titles list is added at starting.
                return finalData;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }


    }
}
