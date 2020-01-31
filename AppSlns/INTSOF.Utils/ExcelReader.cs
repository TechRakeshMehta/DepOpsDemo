using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;


namespace INTSOF.Utils
{
    /// <summary>
    /// Used to read excel file
    /// </summary>
    public static class ExcelReader
    {
        /// <summary>
        /// Generate applicant list from  Excel sheet
        /// </summary>
        /// <param name="path">Path of Excel sheet that used to generate applicant list</param>
        /// <returns>List of applicants</returns>
        public static List<ApplicantDetailContract> GetApplicantListFromFile(String path, Boolean isEmailExist = false)
        {
            List<ApplicantDetailContract> lstApplicantDetails = new List<ApplicantDetailContract>();
            if (File.Exists(path))
            {

                XSSFWorkbook xssfwb;
                HSSFWorkbook hssfwb;
                ISheet sheet;
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
                var fileExt = Path.GetExtension(path);
                if (fileExt == ".xls")
                {
                    hssfwb = new HSSFWorkbook(file);
                    sheet = hssfwb.GetSheetAt(0);
                }
                else
                {
                    xssfwb = new XSSFWorkbook(file);
                    sheet = xssfwb.GetSheetAt(0);

                }

                ApplicantDetailContract applicantDetail = null;

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    applicantDetail = new ApplicantDetailContract();

                    //null is when the row only contains empty cells 
                    if (sheet.GetRow(row) != null && sheet.GetRow(row).Cells != null && sheet.GetRow(row).Cells.Count >= AppConsts.THREE
                        && (sheet.GetRow(row).GetCell(0).CellType != CellType.Blank
                        || sheet.GetRow(row).GetCell(1).CellType != CellType.Blank
                        || sheet.GetRow(row).GetCell(2).CellType != CellType.Blank))
                    {
                        applicantDetail.FirstName = sheet.GetRow(row).GetCell(0).StringCellValue;
                        applicantDetail.LastName = sheet.GetRow(row).GetCell(1).StringCellValue;
                        //UAT-1729: Bulk upload of the Admin Invitation to Complio and download of upload template.
                        if (isEmailExist)
                            applicantDetail.Email = sheet.GetRow(row).GetCell(2).StringCellValue;
                        else
                            applicantDetail.DOB = sheet.GetRow(row).GetCell(2).CellType == NPOI.SS.UserModel.CellType.String
                                                                                        ? Convert.ToDateTime(sheet.GetRow(row).GetCell(2).StringCellValue)
                                                                                        : (sheet.GetRow(row).GetCell(2).DateCellValue);
                        lstApplicantDetails.Add(applicantDetail);
                    }
                }

            }
            return lstApplicantDetails;
        }

        /// <summary>
        /// Convert Applicant details in XMl Format
        /// </summary>
        /// <param name="lstApplicantDetails">List of applicants</param>
        /// <returns>XMl Data</returns>
        public static String ConvertApplicantDetailInXMLFormat(List<ApplicantDetailContract> lstApplicantDetails)
        {
            StringBuilder xmlData = new StringBuilder();
            xmlData.Append("<ApplicantDetails>");
            lstApplicantDetails.ForEach(app =>
            {
                xmlData.Append("<ApplicantDetail>");
                xmlData.Append("<FirstName>" + app.FirstName + "</FirstName>");
                xmlData.Append("<LastName>" + app.LastName + "</LastName>");
                xmlData.Append("<DOB>" + app.DOB.Value.ToString("yyyy-MM-dd") + "</DOB>");
                xmlData.Append("</ApplicantDetail>");

            });
            xmlData.Append("</ApplicantDetails>");

            return xmlData.ToString();
        }

        #region Clinical Rotation :UAT-1344:Automated NPI Number association and agency creation
        /// <summary>
        /// Generate Agency detail list from  Excel sheet
        /// </summary>
        /// <param name="path">Path of Excel sheet that used to generate Agency detail list</param>
        /// <returns>List of Agencies</returns>
        public static List<AgencyExcelDataContract> GetAgencyDetailFromFile(String path)
        {
            List<AgencyExcelDataContract> lstAgencyDetail = new List<AgencyExcelDataContract>();
            if (File.Exists(path))
            {

                XSSFWorkbook xssfwb;
                HSSFWorkbook hssfwb;
                ISheet sheet;
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
                var fileExt = Path.GetExtension(path);
                if (fileExt == ".xls")
                {
                    hssfwb = new HSSFWorkbook(file);
                    sheet = hssfwb.GetSheetAt(0);
                }
                else
                {
                    xssfwb = new XSSFWorkbook(file);
                    sheet = xssfwb.GetSheetAt(0);

                }

                AgencyExcelDataContract agencyDetail = null;

                var headerRow = sheet.GetRow(0);

                List<String> headerColumnList = GetHeaderColumnList();

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    agencyDetail = new AgencyExcelDataContract();
                    try
                    {
                        if (sheet.GetRow(row) != null && sheet.GetRow(row).Cells.Any(x => x.CellType != CellType.Blank))
                        {
                            SetAgencyDetailFromRow(headerRow, sheet.GetRow(row), agencyDetail, headerColumnList);
                            if (!agencyDetail.IsNullOrEmpty())
                            {
                                lstAgencyDetail.Add(agencyDetail);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            return lstAgencyDetail;
        }

        /// <summary>
        /// Convert Agency details in XMl Format
        /// </summary>
        /// <param name="lstAgencyDetails">List of Agencies</param>
        /// <returns>XMl Data</returns>
        public static String ConvertAgencyDetailInXML(List<AgencyExcelDataContract> lstAgencyDetails)
        {
            XElement xmlElements = new XElement("AgencyDetails", lstAgencyDetails
                                   .Select(app => new XElement("AgencyDetail",
                                        new XAttribute("AgencyName", GetAgencyName(app)),
                                        new XAttribute("AgencyAddress1", app.AgencyAddress1.IsNullOrEmpty() ? String.Empty : app.AgencyAddress1),
                                        new XAttribute("AgencyAddress2", app.AgencyAddress2.IsNullOrEmpty() ? String.Empty : app.AgencyAddress2),
                                        new XAttribute("NPINumber", (app.NPINumber.IsNullOrEmpty() ? app.ReplacementNPI.IsNullOrEmpty() ? String.Empty : app.ReplacementNPI
                                                                                                                                                       : app.NPINumber)),
                                        new XAttribute("ReplacementNPI", app.ReplacementNPI.IsNullOrEmpty() ? String.Empty : app.ReplacementNPI),
                                        new XAttribute("ZipCode", app.ZipCode.IsNullOrEmpty() ? String.Empty : app.ZipCode),
                                        new XAttribute("City", app.City.IsNullOrEmpty() ? String.Empty : app.City),
                                        new XAttribute("StateAbbreviation", app.State.IsNullOrEmpty() ? String.Empty : app.State)
                                        ))
                                        );
            return xmlElements.ToString();
        }

        /// <summary>
        /// Method to set agency detail from excel rows.
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="currentRow"></param>
        /// <param name="agencyDataContract"></param>
        /// <param name="headerColumnList"></param>
        private static void SetAgencyDetailFromRow(IRow headerRow, IRow currentRow, AgencyExcelDataContract agencyDataContract, List<String> headerColumnList)
        {
            if (headerRow != null && currentRow != null && currentRow.Cells != null)
            {
                headerColumnList.ForEach(col =>
                {
                    Int32 columnIndex = headerRow.Cells.Where(x => x.StringCellValue.Trim() == col).FirstOrDefault().ColumnIndex;
                    SetColumnValue(currentRow, agencyDataContract, columnIndex, col);
                });
            }
        }

        /// <summary>
        /// Method to return fixed column list.
        /// </summary>
        /// <returns>List<String></returns>
        private static List<String> GetHeaderColumnList()
        {
            List<String> lstColumns = new List<String>();
            lstColumns.Add(ExcelReaderConstants.ProviderOrganizationName);
            lstColumns.Add(ExcelReaderConstants.ProviderNamePrefixText);
            lstColumns.Add(ExcelReaderConstants.ReplacementNPI);
            lstColumns.Add(ExcelReaderConstants.ZipCode);
            lstColumns.Add(ExcelReaderConstants.State);
            lstColumns.Add(ExcelReaderConstants.ProviderLastName);
            lstColumns.Add(ExcelReaderConstants.ProviderFirstName);
            lstColumns.Add(ExcelReaderConstants.ProviderCredentialText);
            lstColumns.Add(ExcelReaderConstants.NPINumber);
            lstColumns.Add(ExcelReaderConstants.City);
            lstColumns.Add(ExcelReaderConstants.AgencyAddress2);
            lstColumns.Add(ExcelReaderConstants.AgencyAddress1);
            return lstColumns;

        }

        /// <summary>
        /// Method to set column data from excel file row and column to contract.
        /// </summary>
        /// <param name="currentRow">currentRow</param>
        /// <param name="agencyDataContract">agencyDataContract</param>
        /// <param name="columnIndex">columnIndex</param>
        /// <param name="columnName">columnName</param>
        private static void SetColumnValue(IRow currentRow, AgencyExcelDataContract agencyDataContract, Int32 columnIndex, String columnName)
        {
            String cellValue = String.Empty;
            switch (columnName)
            {
                case ExcelReaderConstants.ProviderOrganizationName:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = GetExcelCellValue(currentRow, columnIndex);
                        }
                        agencyDataContract.ProviderOrganizationName = cellValue.Trim();
                        break;
                    }
                case ExcelReaderConstants.ProviderNamePrefixText:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = GetExcelCellValue(currentRow, columnIndex);
                        }
                        agencyDataContract.ProviderNamePrefixText = cellValue.Trim();
                        break;
                    }
                case ExcelReaderConstants.ReplacementNPI:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = GetExcelCellValue(currentRow, columnIndex);
                        }
                        agencyDataContract.ReplacementNPI = cellValue.Trim() != AppConsts.ZERO ? cellValue.Trim() : String.Empty;
                        break;
                    }
                case ExcelReaderConstants.ZipCode:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            String zipCode = GetExcelCellValue(currentRow, columnIndex);
                            if (!zipCode.IsNullOrEmpty() && zipCode.Length > AppConsts.FIVE)
                            {
                                cellValue = zipCode.Substring(0, 5);
                            }
                            else
                            {
                                cellValue = zipCode;
                            }
                        }
                        agencyDataContract.ZipCode = cellValue.Trim();
                        break;
                    }
                case ExcelReaderConstants.State:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = currentRow.GetCell(columnIndex).StringCellValue;
                        }
                        agencyDataContract.State = cellValue.Trim();
                        break;
                    }
                case ExcelReaderConstants.ProviderLastName:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = GetExcelCellValue(currentRow, columnIndex);
                        }
                        agencyDataContract.ProviderLastName = cellValue.Trim();
                        break;
                    }
                case ExcelReaderConstants.ProviderFirstName:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = GetExcelCellValue(currentRow, columnIndex);
                        }
                        agencyDataContract.ProviderFirstName = cellValue.Trim();
                        break;
                    }
                case ExcelReaderConstants.ProviderCredentialText:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = GetExcelCellValue(currentRow, columnIndex);
                        }
                        agencyDataContract.ProviderCredentialText = cellValue.Trim();
                        break;
                    }
                case ExcelReaderConstants.NPINumber:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = GetExcelCellValue(currentRow, columnIndex);
                        }
                        agencyDataContract.NPINumber = cellValue.Trim() != AppConsts.ZERO ? cellValue.Trim() : String.Empty;
                        break;
                    }
                case ExcelReaderConstants.City:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = currentRow.GetCell(columnIndex).StringCellValue;
                        }
                        agencyDataContract.City = cellValue.Trim();
                        break;
                    }
                case ExcelReaderConstants.AgencyAddress2:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = GetExcelCellValue(currentRow, columnIndex);
                        }
                        agencyDataContract.AgencyAddress2 = cellValue.Trim();
                        break;
                    }
                case ExcelReaderConstants.AgencyAddress1:
                    {
                        if (!currentRow.GetCell(columnIndex).IsNullOrEmpty())
                        {
                            cellValue = GetExcelCellValue(currentRow, columnIndex);
                        }
                        agencyDataContract.AgencyAddress1 = cellValue.Trim();
                        break;
                    }
            }
        }

        /// <summary>
        /// Method to return agency name on the basis of bussiness logic
        /// </summary>
        /// <param name="agencyExlDataContract">agencyExlDataContract</param>
        /// <returns></returns>
        private static String GetAgencyName(AgencyExcelDataContract agencyExlDataContract)
        {
            string agencyName = String.Empty;
            //Scenario 1: “If data in Column C AND Columns D, E or G then agency name: Column C (Column G Column D Column E Column I)”
            if ((!agencyExlDataContract.ProviderFirstName.IsNullOrEmpty()
                   || !agencyExlDataContract.ProviderLastName.IsNullOrEmpty()
                   || !agencyExlDataContract.ProviderNamePrefixText.IsNullOrEmpty()
                 ) && !agencyExlDataContract.ProviderOrganizationName.IsNullOrEmpty()
                )
            {
                agencyName = agencyExlDataContract.ProviderOrganizationName + "("
                             + (agencyExlDataContract.ProviderNamePrefixText.IsNullOrEmpty() ? String.Empty : agencyExlDataContract.ProviderNamePrefixText + " ")
                             + (agencyExlDataContract.ProviderFirstName.IsNullOrEmpty() ? String.Empty : agencyExlDataContract.ProviderFirstName + " ")
                             + (agencyExlDataContract.ProviderLastName.IsNullOrEmpty() ? String.Empty : agencyExlDataContract.ProviderLastName + " ")
                             + (agencyExlDataContract.ProviderCredentialText.IsNullOrEmpty() ? String.Empty : agencyExlDataContract.ProviderCredentialText) + ")";
            }
            //Scenario 2: “If no Data in column C then agency name: Column G Column D Column E Column I (Ex: Prefix First Name Last Name Credential Text)”
            else if (!agencyExlDataContract.ProviderFirstName.IsNullOrEmpty()
                     || !agencyExlDataContract.ProviderLastName.IsNullOrEmpty()
                     || !agencyExlDataContract.ProviderNamePrefixText.IsNullOrEmpty()
                   )
            {
                agencyName = (agencyExlDataContract.ProviderNamePrefixText.IsNullOrEmpty() ? String.Empty : agencyExlDataContract.ProviderNamePrefixText + " ")
                             + (agencyExlDataContract.ProviderFirstName.IsNullOrEmpty() ? String.Empty : agencyExlDataContract.ProviderFirstName + " ")
                             + (agencyExlDataContract.ProviderLastName.IsNullOrEmpty() ? String.Empty : agencyExlDataContract.ProviderLastName + " ")
                             + (agencyExlDataContract.ProviderCredentialText.IsNullOrEmpty() ? String.Empty : agencyExlDataContract.ProviderCredentialText);
            }
            //Scenario 3:“If no Data in Columns D, E, or G then Agency Name: Column C”
            else
            {
                agencyName = agencyExlDataContract.ProviderOrganizationName.IsNullOrEmpty() ? String.Empty : agencyExlDataContract.ProviderOrganizationName;
            }

            return agencyName;
        }

        /// <summary>
        /// Method to return Current Row cell value
        /// </summary>
        /// <param name="currentRow">current Row</param>
        /// <param name="columnIndex">column Index</param>
        /// <returns>Cell value</returns>
        private static String GetExcelCellValue(IRow currentRow, Int32 columnIndex)
        {
            String cellValue = currentRow.GetCell(columnIndex).CellType == NPOI.SS.UserModel.CellType.String ? currentRow.GetCell(columnIndex).StringCellValue
                                                                                : Convert.ToString(currentRow.GetCell(columnIndex).NumericCellValue);
            if (cellValue.IsNullOrEmpty() || cellValue == AppConsts.ZERO)
            {
                cellValue = String.Empty;
            }
            return cellValue;
        }
        #endregion

        /// <summary>
        /// UAT - 1503 : Convert the Rtoation data into bytes, for Download in Excel format
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Byte[] GetRotationDetailBytesforDownload(DataTable dt, String fileName)
        {
            HSSFWorkbook wb;
            HSSFSheet sheet;
            Byte[] fileBytes = null;
            if (!File.Exists(fileName + ".xls"))
            {
                wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
                sheet = (HSSFSheet)wb.CreateSheet("Sheet1");

                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Status");
                headerRow.CreateCell(1).SetCellValue("School");
                headerRow.CreateCell(2).SetCellValue("Agency");
                headerRow.CreateCell(3).SetCellValue("Rotation Id");
                headerRow.CreateCell(4).SetCellValue("Department");
                headerRow.CreateCell(5).SetCellValue("Program");
                headerRow.CreateCell(6).SetCellValue("Course");
                headerRow.CreateCell(7).SetCellValue("Term");
                headerRow.CreateCell(8).SetCellValue("Unit/Floor ");
                headerRow.CreateCell(9).SetCellValue("Recommended Hours");
                headerRow.CreateCell(10).SetCellValue("Students");
                headerRow.CreateCell(11).SetCellValue("Days");
                headerRow.CreateCell(12).SetCellValue("Shift");
                headerRow.CreateCell(13).SetCellValue("Start Time ");
                headerRow.CreateCell(14).SetCellValue("End Time");
                headerRow.CreateCell(15).SetCellValue("Start Date");
                headerRow.CreateCell(16).SetCellValue("End Date");
                headerRow.CreateCell(17).SetCellValue("Complio Id");
                sheet.CreateFreezePane(0, 1, 0, 1);

                int rowsNumber = 1;
                for (int rowNum = 0; dt.Rows.Count > rowNum; rowNum++)
                {
                    var row = sheet.CreateRow(rowsNumber++);
                    row.CreateCell(0).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationReviewStatus"]));
                    row.CreateCell(1).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Tenant"]));
                    row.CreateCell(2).SetCellValue(Convert.ToString(dt.Rows[rowNum]["AgencyName"]));
                    row.CreateCell(3).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationName"]));
                    row.CreateCell(4).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationDepartment"]));
                    row.CreateCell(5).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationProgram"]));
                    row.CreateCell(6).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationCourse"]));
                    row.CreateCell(7).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationTerm"]));
                    row.CreateCell(8).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationUnitFloorLoc"]));
                    row.CreateCell(9).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationNoOfHrs"]));
                    row.CreateCell(10).SetCellValue(Convert.ToString(dt.Rows[rowNum]["ApplicantCount"]));
                    row.CreateCell(11).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationDays"]));
                    row.CreateCell(12).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationShift"]));
                    row.CreateCell(13).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationStartTime"]));
                    row.CreateCell(14).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationEndTime"]));
                    row.CreateCell(15).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationStartDate"]));
                    row.CreateCell(16).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationEndDate"]));
                    row.CreateCell(17).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RotationComplioID"]));
                }

                MemoryStream output = new MemoryStream();
                wb.Write(output);
                fileBytes = output.GetBuffer();
            }
            return fileBytes;
        }

        /// <summary>
        /// Get Bytes to Create Account Invitation
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Byte[] GetCreateAccountInvitationBytes(DataTable dt, String fileName)
        {
            HSSFWorkbook wb;
            HSSFSheet sheet;
            Byte[] fileBytes = null;
            wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
            sheet = (HSSFSheet)wb.CreateSheet("Sheet1");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("First Name");
            headerRow.CreateCell(1).SetCellValue("Last Name");
            headerRow.CreateCell(2).SetCellValue("Email");

            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowsNumber = 1;
            for (int rowNum = 0; dt.Rows.Count > rowNum; rowNum++)
            {
                var row = sheet.CreateRow(rowsNumber++);
                row.CreateCell(0).SetCellValue(Convert.ToString(dt.Rows[rowNum]["FirstName"]));
                row.CreateCell(1).SetCellValue(Convert.ToString(dt.Rows[rowNum]["LastName"]));
                row.CreateCell(2).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Email"]));

            }

            MemoryStream output = new MemoryStream();
            wb.Write(output);
            fileBytes = output.GetBuffer();

            return fileBytes;
        }


        /// <summary>
        /// Generate applicant list from  Excel sheet
        /// </summary>
        /// <param name="path">Path of Excel sheet that used to generate applicant list</param>
        /// <returns>List of applicants</returns>
        public static List<ApplicantDetailContract> GetApplicantDetailsFromFile(String path)
        {
            List<ApplicantDetailContract> lstApplicantDetails = new List<ApplicantDetailContract>();
            if (File.Exists(path))
            {
                XSSFWorkbook xssfwb;
                HSSFWorkbook hssfwb;
                ISheet sheet;
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
                var fileExt = Path.GetExtension(path);
                if (fileExt == ".xls")
                {
                    hssfwb = new HSSFWorkbook(file);
                    sheet = hssfwb.GetSheetAt(0);
                }
                else
                {
                    xssfwb = new XSSFWorkbook(file);
                    sheet = xssfwb.GetSheetAt(0);
                }

                ApplicantDetailContract applicantDetail = null;

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    applicantDetail = new ApplicantDetailContract();

                    //null is when the row only contains empty cells 
                    if (sheet.GetRow(row) != null && sheet.GetRow(row).Cells != null && sheet.GetRow(row).Cells.Count >= AppConsts.THREE
                        && (sheet.GetRow(row).GetCell(0).CellType != CellType.Blank
                        || sheet.GetRow(row).GetCell(1).CellType != CellType.Blank
                        || sheet.GetRow(row).GetCell(2).CellType != CellType.Blank
                        || sheet.GetRow(row).GetCell(3).CellType != CellType.Blank
                        || sheet.GetRow(row).GetCell(4).CellType != CellType.Blank
                        || sheet.GetRow(row).GetCell(5).CellType != CellType.Blank
                        || sheet.GetRow(row).GetCell(6).CellType != CellType.Blank
                        || sheet.GetRow(row).GetCell(7).CellType != CellType.Blank))
                    {
                        String[] format = { "MM-dd-yyyy", "MM/dd/yyyy", "M-dd-yyyy", "M/dd/yyyy", "MM-dd-yy", "MM/dd/yy", "M-dd-yy", "M/dd/yy",
                                            "MM-dd-yyyy hh:mm:ss tt", "MM/dd/yyyy hh:mm:ss tt", "M-dd-yyyy hh:mm:ss tt", "M/dd/yyyy hh:mm:ss tt", "MM-dd-yy hh:mm:ss tt", 
                                            "MM/dd/yy hh:mm:ss tt", "M-dd-yy hh:mm:ss tt", "M/dd/yy hh:mm:ss tt"};
                        String validFormat = "MM/dd/yyyy";

                        //Commented below code related to UAT-2697
                        //applicantDetail.FirstName = sheet.GetRow(row).GetCell(0).StringCellValue;
                        //applicantDetail.LastName = sheet.GetRow(row).GetCell(1).StringCellValue;
                        //applicantDetail.Email = sheet.GetRow(row).GetCell(2).StringCellValue;

                        //UAT-2697

                        if (!sheet.GetRow(row).GetCell(0).IsNullOrEmpty())
                        {
                            applicantDetail.FirstName = sheet.GetRow(row).GetCell(0).StringCellValue;
                        }

                        if (!sheet.GetRow(row).GetCell(1).IsNullOrEmpty())
                        {
                            applicantDetail.LastName = sheet.GetRow(row).GetCell(1).StringCellValue;
                        }
                        if (!sheet.GetRow(row).GetCell(2).IsNullOrEmpty())
                        {
                            applicantDetail.Email = sheet.GetRow(row).GetCell(2).StringCellValue;
                        }

                        if (!sheet.GetRow(row).GetCell(3).IsNullOrEmpty())
                        {
                            applicantDetail.PackageID = sheet.GetRow(row).GetCell(3).CellType == NPOI.SS.UserModel.CellType.String
                                                                                        ? (int?)Convert.ToInt32(sheet.GetRow(row).GetCell(3).StringCellValue)
                                                                                        : (int?)sheet.GetRow(row).GetCell(3).NumericCellValue;
                        }
                        if (!sheet.GetRow(row).GetCell(4).IsNullOrEmpty())
                            applicantDetail.OrderNodeID = sheet.GetRow(row).GetCell(4).CellType == NPOI.SS.UserModel.CellType.String
                                                                                        ? (int?)Convert.ToInt32(sheet.GetRow(row).GetCell(4).StringCellValue)
                                                                                        : (int?)sheet.GetRow(row).GetCell(4).NumericCellValue;
                        if (!sheet.GetRow(row).GetCell(5).IsNullOrEmpty())
                        {
                            //applicantDetail.StartDate = sheet.GetRow(row).GetCell(5).CellType == NPOI.SS.UserModel.CellType.String
                            //                                                            ? Convert.ToDateTime(sheet.GetRow(row).GetCell(5).StringCellValue)
                            //                                                            : (sheet.GetRow(row).GetCell(5).DateCellValue);

                            DateTime startDateTime;
                            if (sheet.GetRow(row).GetCell(5).CellType == NPOI.SS.UserModel.CellType.String)
                            {
                                if (DateTime.TryParseExact(sheet.GetRow(row).GetCell(5).StringCellValue, format, System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out startDateTime))
                                {
                                    applicantDetail.StartDate = Convert.ToDateTime(startDateTime.ToString(validFormat, System.Globalization.CultureInfo.InvariantCulture));
                                }
                            }
                            else
                            {
                                if (DateTime.TryParseExact(sheet.GetRow(row).GetCell(5).DateCellValue.ToString(validFormat), format, System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out startDateTime))
                                {
                                    applicantDetail.StartDate = Convert.ToDateTime(startDateTime.ToString(validFormat, System.Globalization.CultureInfo.InvariantCulture));
                                }
                            }
                        }
                        if (!sheet.GetRow(row).GetCell(6).IsNullOrEmpty())
                        {
                            //applicantDetail.EndDate = sheet.GetRow(row).GetCell(6).CellType == NPOI.SS.UserModel.CellType.String
                            //                                                            ? Convert.ToDateTime(sheet.GetRow(row).GetCell(6).StringCellValue)
                            //                                                            : (sheet.GetRow(row).GetCell(6).DateCellValue);
                            DateTime endDateTime;
                            if (sheet.GetRow(row).GetCell(6).CellType == NPOI.SS.UserModel.CellType.String)
                            {

                                if (DateTime.TryParseExact(sheet.GetRow(row).GetCell(6).StringCellValue, format, System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out endDateTime))
                                {
                                    applicantDetail.EndDate = Convert.ToDateTime(endDateTime.ToString(validFormat, System.Globalization.CultureInfo.InvariantCulture));
                                }
                            }
                            else
                            {
                                if (DateTime.TryParseExact(sheet.GetRow(row).GetCell(6).DateCellValue.ToString(validFormat), format, System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out endDateTime))
                                {
                                    applicantDetail.EndDate = Convert.ToDateTime(endDateTime.ToString(validFormat, System.Globalization.CultureInfo.InvariantCulture));
                                }
                            }
                        }
                        if (!sheet.GetRow(row).GetCell(7).IsNullOrEmpty())
                            applicantDetail.Interval = sheet.GetRow(row).GetCell(7).CellType == NPOI.SS.UserModel.CellType.String
                                                                                        ? (int?)Convert.ToInt32(sheet.GetRow(row).GetCell(7).StringCellValue)
                                                                                        : (int?)sheet.GetRow(row).GetCell(7).NumericCellValue;

                        lstApplicantDetails.Add(applicantDetail);
                    }
                }
            }
            return lstApplicantDetails;
        }

        /// <summary>
        /// Get Bytes for Bulk Order Upload
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Byte[] GetBulkOrderUploadBytes(DataTable dt, String fileName, Boolean isRepeatedSearchScreen)
        {
            HSSFWorkbook wb;
            HSSFSheet sheet;
            Byte[] fileBytes = null;
            wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
            sheet = (HSSFSheet)wb.CreateSheet("Sheet1");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("First Name");
            headerRow.CreateCell(1).SetCellValue("Last Name");
            headerRow.CreateCell(2).SetCellValue("Email Address");
            headerRow.CreateCell(3).SetCellValue("Package ID");
            headerRow.CreateCell(4).SetCellValue("Order Node ID");
            //UAT-2697
            if (!isRepeatedSearchScreen)
            {
                headerRow.CreateCell(5).SetCellValue("Start Date");
                headerRow.CreateCell(6).SetCellValue("End Date");
                headerRow.CreateCell(7).SetCellValue("Interval");
            }

            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowsNumber = 1;
            for (int rowNum = 0; dt.Rows.Count > rowNum; rowNum++)
            {
                var row = sheet.CreateRow(rowsNumber++);
                row.CreateCell(0).SetCellValue(Convert.ToString(dt.Rows[rowNum]["FirstName"]));
                row.CreateCell(1).SetCellValue(Convert.ToString(dt.Rows[rowNum]["LastName"]));
                row.CreateCell(2).SetCellValue(Convert.ToString(dt.Rows[rowNum]["EmailAddress"]));
                row.CreateCell(3).SetCellValue(Convert.ToString(dt.Rows[rowNum]["PackageID"]));
                row.CreateCell(4).SetCellValue(Convert.ToString(dt.Rows[rowNum]["OrderNodeID"]));
                //UAT-2697
                if (!isRepeatedSearchScreen)
                {
                    row.CreateCell(5).SetCellValue(Convert.ToString(dt.Rows[rowNum]["StartDate"]));
                    row.CreateCell(6).SetCellValue(Convert.ToString(dt.Rows[rowNum]["EndDate"]));
                    row.CreateCell(7).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Interval"]));
                }

            }

            MemoryStream output = new MemoryStream();
            wb.Write(output);
            fileBytes = output.GetBuffer();

            return fileBytes;
        }

        /// <summary>
        /// Get Bytes for Bulk Rotation Upload
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Byte[] GetBulkRotationUploadBytes(DataTable dt, String fileName, List<String> AgencyHierarchyIdAgencys)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet realSheet = (HSSFSheet)workbook.CreateSheet("Datasheet");
            HSSFSheet hidden = (HSSFSheet)workbook.CreateSheet("hiddenAgencyHierarchyAgencyList");

            #region Bind AgencyHierarchyAgencyList
            for (int i = 0, length = AgencyHierarchyIdAgencys.Count; i < length; i++)
            {
                String name = AgencyHierarchyIdAgencys[i];
                HSSFRow row = (HSSFRow)hidden.CreateRow(i);
                HSSFCell cell = (HSSFCell)row.CreateCell(0);
                cell.SetCellValue(name);
            }
            #endregion

            #region Defining columns

            HSSFRow datarow = null;
            HSSFCell datacell = null;

            datarow = (HSSFRow)realSheet.CreateRow(0);
            datacell = (HSSFCell)datarow.CreateCell(0);
            datacell.SetCellValue("Institute Node ID");

            realSheet.SetColumnWidth(0, 5000);
            datarow.CreateCell(1).SetCellValue("Agency");
            realSheet.SetColumnWidth(1, 5000);
            datarow.CreateCell(2).SetCellValue("Rotation ID/Name");
            realSheet.SetColumnWidth(2, 5000);
            datarow.CreateCell(3).SetCellValue("Rotation Review Status");
            realSheet.SetColumnWidth(3, 5000);
            datarow.CreateCell(4).SetCellValue("Type/Specialty");
            realSheet.SetColumnWidth(4, 4000);
            datarow.CreateCell(5).SetCellValue("Department");
            realSheet.SetColumnWidth(5, 3000);
            datarow.CreateCell(6).SetCellValue("Program");
            datarow.CreateCell(7).SetCellValue("Course");
            datarow.CreateCell(8).SetCellValue("Term");
            datarow.CreateCell(9).SetCellValue("Unit/Floor");
            realSheet.SetColumnWidth(9, 3000);
            datarow.CreateCell(10).SetCellValue("# of Students");
            realSheet.SetColumnWidth(10, 5000);
            datarow.CreateCell(11).SetCellValue("# of Recommended Hours");
            realSheet.SetColumnWidth(11, 7000);
            datarow.CreateCell(12).SetCellValue("Days");
            realSheet.SetColumnWidth(12, 5000);
            datarow.CreateCell(13).SetCellValue("Shift");
            datarow.CreateCell(14).SetCellValue("Time");
            realSheet.SetColumnWidth(14, 5000);
            datarow.CreateCell(15).SetCellValue("Start Date");
            datarow.CreateCell(16).SetCellValue("End Date");
            datarow.CreateCell(17).SetCellValue("Instructor/Preceptor");
            realSheet.SetColumnWidth(17, 4000);
            #endregion

            #region Bind agency hierarchy agency list to datasheet
            var namedCell = workbook.CreateName();
            namedCell.NameName = "hiddenAgencyHierarchyAgencyList";
            namedCell.RefersToFormula = "hiddenAgencyHierarchyAgencyList!$A:$A";// "hiddenAgencyHierarchyAgencyList!$A$1:$A$";
            DVConstraint constraint = DVConstraint.CreateFormulaListConstraint("hiddenAgencyHierarchyAgencyList");
            for (int i = 1, length = 1000; i < length; i++)
            {
                CellRangeAddressList addressList = new CellRangeAddressList(1, i, 1, 1);
                HSSFDataValidation validation = new HSSFDataValidation(addressList, constraint);
                validation.EmptyCellAllowed = false;
                validation.ShowPromptBox = true;
                validation.SuppressDropDownArrow = false;
                realSheet.AddValidationData(validation);
            }
            #endregion

            //Hiding the hiddenAgencyHierarchyAgencyList sheet
            workbook.SetSheetHidden(1, true);

            int rowsNumber = 1;
            for (int rowNum = 0; dt.Rows.Count > rowNum; rowNum++)
            {
                var dummyrow = realSheet.CreateRow(rowsNumber++);
                dummyrow.CreateCell(0).SetCellValue(Convert.ToString(dt.Rows[rowNum]["InstitutionNodeID"]));
                dummyrow.CreateCell(1).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Agency"]));
                dummyrow.CreateCell(2).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Rotation_Name"]));
                dummyrow.CreateCell(3).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Rotation_Review_Status"]));
                dummyrow.CreateCell(4).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Type_Specialty"]));
                dummyrow.CreateCell(5).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Department"]));
                dummyrow.CreateCell(6).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Program"]));
                dummyrow.CreateCell(7).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Course"]));
                dummyrow.CreateCell(8).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Term"]));
                dummyrow.CreateCell(9).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Unit_Floor"]));
                dummyrow.CreateCell(10).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Students"]));
                dummyrow.CreateCell(11).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Recommended_Hours"]));
                dummyrow.CreateCell(12).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Days"]));
                dummyrow.CreateCell(13).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Shift"]));
                dummyrow.CreateCell(14).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Time"]));
                dummyrow.CreateCell(15).SetCellValue(Convert.ToString(dt.Rows[rowNum]["StartDate"]));
                dummyrow.CreateCell(16).SetCellValue(Convert.ToString(dt.Rows[rowNum]["EndDate"]));
                dummyrow.CreateCell(17).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Instructor_Preceptor"]));
            }


            Byte[] fileBytes = null;
            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            fileBytes = output.GetBuffer();

            return fileBytes;
        }



        /// <summary>
        /// Generate rotation list from  Excel sheet
        /// </summary>
        /// <param name="path">Path of Excel sheet that used to generate rotation list</param>
        /// <returns>List of rotations</returns>
        public static DataTable GetRotationDetailsFromFile(String path)
        {
            DataTable dtRotationDetails = new DataTable();

            #region Add Columns To dataTable

            DataColumn dc = new DataColumn("InstitutionNodeID", typeof(Int32));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Agency", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            //dc = new DataColumn("Hierarchy", typeof(String));
            //dtRotationDetails.Columns.Add(dc);

            //dc = new DataColumn("ComplioID", typeof(String));
            //dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Rotation_Name", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Rotation_Review_Status", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Type_Specialty", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Department", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Program", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Course", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Term", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Unit_Floor", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Students", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Recommended_Hours", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Days", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Shift", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Time", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("StartDate", typeof(DateTime));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("EndDate", typeof(DateTime));
            dtRotationDetails.Columns.Add(dc);

            dc = new DataColumn("Instructor_Preceptor", typeof(String));
            dtRotationDetails.Columns.Add(dc);

            #endregion


            if (File.Exists(path))
            {
                XSSFWorkbook xssfwb;
                HSSFWorkbook hssfwb;
                ISheet sheet;
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
                var fileExt = Path.GetExtension(path);
                if (fileExt == ".xls")
                {
                    hssfwb = new HSSFWorkbook(file);
                    sheet = hssfwb.GetSheetAt(0);
                }
                else
                {
                    xssfwb = new XSSFWorkbook(file);
                    sheet = xssfwb.GetSheetAt(0);
                }

                String[] format = { "MM-dd-yyyy", "MM/dd/yyyy", "M-dd-yyyy", "M/dd/yyyy", "MM-dd-yy", "MM/dd/yy", "M-dd-yy", "M/dd/yy",
                                            "MM-dd-yyyy hh:mm:ss tt", "MM/dd/yyyy hh:mm:ss tt", "M-dd-yyyy hh:mm:ss tt", "M/dd/yyyy hh:mm:ss tt", "MM-dd-yy hh:mm:ss tt", 
                                            "MM/dd/yy hh:mm:ss tt", "M-dd-yy hh:mm:ss tt", "M/dd/yy hh:mm:ss tt"};
                String validFormat = "MM/dd/yyyy";

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    #region Add rows to DataTable
                    //null is when the row only contains empty cells 
                    if (sheet.GetRow(row) != null && sheet.GetRow(row).Cells != null //&& sheet.GetRow(row).Cells.Count >= AppConsts.THREE
                        && ((!sheet.GetRow(row).GetCell(0).IsNullOrEmpty() && sheet.GetRow(row).GetCell(0).CellType != CellType.Blank)
                        || (!sheet.GetRow(row).GetCell(1).IsNullOrEmpty() && sheet.GetRow(row).GetCell(1).CellType != CellType.Blank)
                        // || sheet.GetRow(row).GetCell(2).CellType != CellType.Blank
                        || (!sheet.GetRow(row).GetCell(15).IsNullOrEmpty() && sheet.GetRow(row).GetCell(15).CellType != CellType.Blank)
                        || (!sheet.GetRow(row).GetCell(16).IsNullOrEmpty() && sheet.GetRow(row).GetCell(16).CellType != CellType.Blank)))
                    {
                        DataRow dr = dtRotationDetails.NewRow();
                        if (!sheet.GetRow(row).GetCell(0).IsNullOrEmpty())
                        {
                            try
                            {
                                dr[0] = sheet.GetRow(row).GetCell(0).CellType == NPOI.SS.UserModel.CellType.String
                                                                                                                       ? (int?)Convert.ToInt32(sheet.GetRow(row).GetCell(0).StringCellValue.Trim())
                                                                                                                       : (int?)sheet.GetRow(row).GetCell(0).NumericCellValue;
                            }
                            catch (Exception)
                            {

                                throw new Exception("Invalid Institute Node ID");
                            }

                        }
                        try
                        {
                            dr[1] = !sheet.GetRow(row).GetCell(1).IsNullOrEmpty() && sheet.GetRow(row).GetCell(1).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(1).StringCellValue.Trim() : String.Empty;   //"Agency"]));
                        }
                        catch (Exception)
                        {

                            throw new Exception("Invalid Agency");
                        }

                        try
                        {
                            dr[2] = !sheet.GetRow(row).GetCell(2).IsNullOrEmpty() && sheet.GetRow(row).GetCell(2).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(2).StringCellValue.Trim() : String.Empty;   //"Rotation_Name"]));
                        }
                        catch (Exception)
                        {
                            throw new Exception("Invalid Rotation Name");
                        }


                        try
                        {
                            dr[3] = !sheet.GetRow(row).GetCell(3).IsNullOrEmpty() && sheet.GetRow(row).GetCell(3).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(3).StringCellValue.Trim() : String.Empty;   //"Rotation_Review_Status"]))
                        }
                        catch (Exception)
                        {
                            throw new Exception("Invalid Rotation Status");
                        }

                        try
                        {
                            dr[4] = !sheet.GetRow(row).GetCell(4).IsNullOrEmpty() && sheet.GetRow(row).GetCell(4).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(4).StringCellValue.Trim() : String.Empty;   //"Type_Specialty"]));
                        }
                        catch (Exception)
                        {
                            throw new Exception("Invalid Type/Speciality");
                        }

                        try
                        {
                            dr[5] = !sheet.GetRow(row).GetCell(5).IsNullOrEmpty() && sheet.GetRow(row).GetCell(5).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(5).StringCellValue.Trim() : String.Empty;   //"Department"]));
                        }
                        catch (Exception)
                        {
                            throw new Exception("Invalid Rotation Department");
                        }

                        try
                        {
                            dr[6] = !sheet.GetRow(row).GetCell(6).IsNullOrEmpty() && sheet.GetRow(row).GetCell(6).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(6).StringCellValue.Trim() : String.Empty;   //"Program"]));
                        }
                        catch (Exception)
                        {
                            throw new Exception("Invalid Rotation Program");
                        }

                        try
                        {
                            dr[7] = !sheet.GetRow(row).GetCell(7).IsNullOrEmpty() && sheet.GetRow(row).GetCell(7).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(7).StringCellValue.Trim() : String.Empty;   //"Course"]));
                        }
                        catch (Exception)
                        {
                            throw new Exception("Invalid Rotation Course");
                        }

                        try
                        {
                            dr[8] = !sheet.GetRow(row).GetCell(8).IsNullOrEmpty() && sheet.GetRow(row).GetCell(8).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(8).StringCellValue.Trim() : String.Empty;  //"Term"]));
                        }
                        catch (Exception)
                        {
                            throw new Exception("Invalid Rotation Term");
                        }
                        if (!sheet.GetRow(row).GetCell(9).IsNullOrEmpty() && sheet.GetRow(row).GetCell(9).CellType != CellType.Blank)
                        {
                            try
                            {

                                dr[9] = sheet.GetRow(row).GetCell(9).CellType == CellType.String ? Convert.ToString(sheet.GetRow(row).GetCell(9).StringCellValue) : Convert.ToString(sheet.GetRow(row).GetCell(9).NumericCellValue);  //"Unit_Floor"]));
                            }
                            catch (Exception)
                            {
                                throw new Exception("Invalid Unit/Floor");
                            }
                        }

                        else
                        {
                            dr[9] = String.Empty;
                        }

                        if (!sheet.GetRow(row).GetCell(10).IsNullOrEmpty() && sheet.GetRow(row).GetCell(10).CellType != CellType.Blank)
                        {
                            try
                            {
                                dr[10] = sheet.GetRow(row).GetCell(10).CellType == CellType.String ? Convert.ToString(sheet.GetRow(row).GetCell(10).StringCellValue) : Convert.ToString(sheet.GetRow(row).GetCell(10).NumericCellValue);  //"Students"]));
                            }
                            catch (Exception)
                            {

                                throw new Exception("Invalid # of students");
                            }

                        }
                        else
                        {
                            dr[10] = String.Empty;
                        }

                        if (!sheet.GetRow(row).GetCell(11).IsNullOrEmpty() && sheet.GetRow(row).GetCell(11).CellType != CellType.Blank)
                        {
                            try
                            {
                                dr[11] = sheet.GetRow(row).GetCell(11).CellType == CellType.String ? Convert.ToString(sheet.GetRow(row).GetCell(11).StringCellValue) : Convert.ToString(sheet.GetRow(row).GetCell(11).NumericCellValue);  //"Hours"]));
                            }
                            catch (Exception)
                            {

                                throw new Exception("Invalid No. of Hours");
                            }
                        }
                        else
                        {
                            dr[11] = String.Empty;
                        }
                        try
                        {
                            dr[12] = !sheet.GetRow(row).GetCell(12).IsNullOrEmpty() && sheet.GetRow(row).GetCell(12).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(12).StringCellValue.Trim() : String.Empty;  //"Days"]));
                        }
                        catch (Exception)
                        {

                            throw new Exception("Invalid Days");
                        }

                        try
                        {
                            dr[13] = !sheet.GetRow(row).GetCell(13).IsNullOrEmpty() && sheet.GetRow(row).GetCell(13).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(13).StringCellValue.Trim() : String.Empty;  //"Shift"]));
                        }
                        catch (Exception)
                        {

                            throw new Exception("Invalid Rotation shift");
                        }

                        try
                        {
                            dr[14] = !sheet.GetRow(row).GetCell(14).IsNullOrEmpty() && sheet.GetRow(row).GetCell(14).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(14).StringCellValue.Trim() : String.Empty;  //"Time"]));
                        }
                        catch (Exception)
                        {

                            throw new Exception("Invalid Time");
                        }

                        if (!sheet.GetRow(row).GetCell(15).IsNullOrEmpty())
                        {
                            DateTime DateTime;
                            if (sheet.GetRow(row).GetCell(15).CellType == NPOI.SS.UserModel.CellType.String)
                            {
                                if (DateTime.TryParseExact(sheet.GetRow(row).GetCell(15).StringCellValue.Trim(), format, System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out DateTime))
                                {
                                    try
                                    {
                                        dr[15] = Convert.ToDateTime(DateTime.ToString(validFormat, System.Globalization.CultureInfo.InvariantCulture));
                                    }
                                    catch (Exception)
                                    {
                                        throw new Exception("Invalid Start Date");
                                    }
                                }
                            }
                            else
                            {
                                if (DateTime.TryParseExact(sheet.GetRow(row).GetCell(15).DateCellValue.ToString(validFormat), format, System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out DateTime))
                                {
                                    try
                                    {
                                        dr[15] = Convert.ToDateTime(DateTime.ToString(validFormat, System.Globalization.CultureInfo.InvariantCulture));
                                    }
                                    catch (Exception)
                                    {
                                        throw new Exception("Invalid Start Date");
                                    }
                                }
                            }
                        }
                        if (!sheet.GetRow(row).GetCell(16).IsNullOrEmpty())
                        {
                            DateTime DateTime;
                            if (sheet.GetRow(row).GetCell(16).CellType == NPOI.SS.UserModel.CellType.String)
                            {
                                if (DateTime.TryParseExact(sheet.GetRow(row).GetCell(16).StringCellValue.Trim(), format, System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out DateTime))
                                {
                                    try
                                    {
                                        dr[16] = Convert.ToDateTime(DateTime.ToString(validFormat, System.Globalization.CultureInfo.InvariantCulture));
                                    }
                                    catch (Exception)
                                    {

                                        throw new Exception("Invalid End Date");
                                    }

                                }
                            }
                            else
                            {
                                if (DateTime.TryParseExact(sheet.GetRow(row).GetCell(16).DateCellValue.ToString(validFormat), format, System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out DateTime))
                                {
                                    try
                                    {
                                        dr[16] = Convert.ToDateTime(DateTime.ToString(validFormat, System.Globalization.CultureInfo.InvariantCulture));
                                    }
                                    catch (Exception)
                                    {

                                        throw new Exception("Invalid End Date");
                                    }

                                }
                            }
                        }
                        try
                        {
                            dr[17] = !sheet.GetRow(row).GetCell(17).IsNullOrEmpty() && sheet.GetRow(row).GetCell(17).CellType != CellType.Blank ? sheet.GetRow(row).GetCell(17).StringCellValue.Trim() : String.Empty;  //"Instructor_Preceptor"]));
                        }
                        catch (Exception)
                        {

                            throw new Exception("Invalid Instructor Preceptor");
                        }


                        if (!String.IsNullOrEmpty(dr[0].ToString())
                            && dr[1].ToString() != "Delete this row"
                            && !String.IsNullOrEmpty(dr[1].ToString())
                            && dr[2].ToString() != "Delete this row"
                            //&& !dr[5].IsNullOrEmpty()
                            //&& !dr[6].IsNullOrEmpty()
                            //&& !dr[7].IsNullOrEmpty()
                            && !String.IsNullOrEmpty(dr[15].ToString())
                            && !String.IsNullOrEmpty(dr[16].ToString()))
                        {
                            dtRotationDetails.Rows.Add(dr);
                        }
                    }

                    #endregion
                }
            }
            return dtRotationDetails;
        }

        /// <summary>
        /// Get Bytes to Create Account Invitation
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Byte[] GetTrackingUpdateBytes(DataTable dt)
        {
            HSSFWorkbook wb;
            HSSFSheet sheet;
            Byte[] fileBytes = null;
            wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
            sheet = (HSSFSheet)wb.CreateSheet("Sheet1");

            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("User Name");
            headerRow.CreateCell(1).SetCellValue("University Unique Identifier");
            headerRow.CreateCell(2).SetCellValue("Category Name");
            headerRow.CreateCell(3).SetCellValue("Category Status");
            headerRow.CreateCell(4).SetCellValue("Category Expiration Date");
            headerRow.CreateCell(5).SetCellValue("Category Compliance Date");

            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowsNumber = 1;
            for (int rowNum = 0; dt.Rows.Count > rowNum; rowNum++)
            {
                var row = sheet.CreateRow(rowsNumber++);
                row.CreateCell(0).SetCellValue(Convert.ToString(dt.Rows[rowNum]["UserName"]));
                row.CreateCell(1).SetCellValue(Convert.ToString(dt.Rows[rowNum]["UniversityUniqueIdentifier"]));
                row.CreateCell(2).SetCellValue(Convert.ToString(dt.Rows[rowNum]["CategoryName"]));
                row.CreateCell(3).SetCellValue(Convert.ToString(dt.Rows[rowNum]["CategoryStatus"]));
                row.CreateCell(4).SetCellValue(Convert.ToString(dt.Rows[rowNum]["CategoryExpirationDate"]));
                row.CreateCell(5).SetCellValue(Convert.ToString(dt.Rows[rowNum]["CategoryComplianceDate"]));
            }

            MemoryStream output = new MemoryStream();
            wb.Write(output);
            fileBytes = output.GetBuffer();

            return fileBytes;
        }

        public static Byte[] GetUpdatedApplicantRequirementsByAgency(DataTable dt, String fileName, String agencyName)
        {
            HSSFWorkbook wb;
            HSSFSheet sheet;
            Byte[] fileBytes = null;
            if (!File.Exists(fileName + ".xls"))
            {
                wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
                sheet = (HSSFSheet)wb.CreateSheet("Sheet1");

                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Agency Name");
                headerRow.CreateCell(1).SetCellValue("Complio ID");
                headerRow.CreateCell(2).SetCellValue("Student First Name");
                headerRow.CreateCell(3).SetCellValue("Student Middle Name");
                headerRow.CreateCell(4).SetCellValue("Student Last Name");
                headerRow.CreateCell(5).SetCellValue("Student Institution");
                headerRow.CreateCell(6).SetCellValue("Item Name");
                sheet.CreateFreezePane(0, 1, 0, 1);

                int rowsNumber = 1;
                for (int rowNum = 0; dt.Rows.Count > rowNum; rowNum++)
                {
                    var row = sheet.CreateRow(rowsNumber++);
                    row.CreateCell(0).SetCellValue(agencyName);
                    row.CreateCell(1).SetCellValue(Convert.ToString(dt.Rows[rowNum]["ComplioID"]));
                    row.CreateCell(2).SetCellValue(Convert.ToString(dt.Rows[rowNum]["ApplicantFirstName"]));
                    row.CreateCell(3).SetCellValue(Convert.ToString(dt.Rows[rowNum]["ApplicantMiddleName"]));
                    row.CreateCell(4).SetCellValue(Convert.ToString(dt.Rows[rowNum]["ApplicantLastName"]));
                    row.CreateCell(5).SetCellValue(Convert.ToString(dt.Rows[rowNum]["InstitutionName"]));
                    row.CreateCell(6).SetCellValue(Convert.ToString(dt.Rows[rowNum]["ItemNames"]));
                }

                MemoryStream output = new MemoryStream();
                wb.Write(output);
                fileBytes = output.GetBuffer();
            }
            return fileBytes;
        }
        #region UAT-3820
        public static Byte[] GetPendingReceivedfromStudentServiceFormStatus(DataTable dt, String fileName)
        {
            HSSFWorkbook wb;
            HSSFSheet sheet;
            Byte[] fileBytes = null;
            if (!File.Exists(fileName + ".xls"))
            {
                wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
                sheet = (HSSFSheet)wb.CreateSheet("Sheet1");

                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Applicant FirstName");
                headerRow.CreateCell(1).SetCellValue("Applicant LastName");
                headerRow.CreateCell(2).SetCellValue("Institution");
                headerRow.CreateCell(3).SetCellValue("Order Number");
                headerRow.CreateCell(4).SetCellValue("Recieved Date");
                sheet.CreateFreezePane(0, 1, 0, 1);

                int rowsNumber = 1;
                for (int rowNum = 0; dt.Rows.Count > rowNum; rowNum++)
                {
                    var row = sheet.CreateRow(rowsNumber++);
                    row.CreateCell(0).SetCellValue(Convert.ToString(dt.Rows[rowNum]["ApplicantFirstName"]));
                    row.CreateCell(1).SetCellValue(Convert.ToString(dt.Rows[rowNum]["ApplicantLastName"]));
                    row.CreateCell(2).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Institution"]));
                    row.CreateCell(3).SetCellValue(Convert.ToString(dt.Rows[rowNum]["OrderNumber"]));
                    row.CreateCell(4).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RecievedDate"]));
                }

                MemoryStream output = new MemoryStream();
                wb.Write(output);
                fileBytes = output.GetBuffer();
            }
            return fileBytes;
        }
        #endregion


        #region UAT-3795
        public static Byte[] ConvertingDataInExcelForNonCompliantStudentsReport(DataTable dt, String fileName)
        {
            HSSFWorkbook wb;
            HSSFSheet sheet;
            Byte[] fileBytes = null;
            if (!File.Exists(fileName + ".xls"))
            {
                wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
                sheet = (HSSFSheet)wb.CreateSheet("Sheet1");

                var headerFont = wb.CreateFont();
                headerFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //titleFont.FontHeightInPoints = 11;
                //titleFont.Underline = NPOI.SS.UserModel.FontUnderlineType.Single;

                var headerStyle = wb.CreateCellStyle();
                headerStyle.SetFont(headerFont);

                var headerRow = sheet.CreateRow(0);
                var cell0 = headerRow.CreateCell(0);
                cell0.CellStyle = headerStyle;
                cell0.SetCellValue("First Name");

                var cell1 = headerRow.CreateCell(1);
                cell1.CellStyle = headerStyle;
                cell1.SetCellValue("Last Name");

                var cell2 = headerRow.CreateCell(2);
                cell2.CellStyle = headerStyle;
                cell2.SetCellValue("Phone Number");

                var cell3 = headerRow.CreateCell(3);
                cell3.CellStyle = headerStyle;
                cell3.SetCellValue("Order Hierarchy");

                var cell4 = headerRow.CreateCell(4);
                cell4.CellStyle = headerStyle;
                cell4.SetCellValue("Order Number");

                var cell5 = headerRow.CreateCell(5);
                cell5.CellStyle = headerStyle;
                cell5.SetCellValue("Categories not compliant");

                //headerRow.CreateCell(0).SetCellValue("First Name");
                //headerRow.CreateCell(1).SetCellValue("Last Name");
                //headerRow.CreateCell(2).SetCellValue("Phone Number");
                //headerRow.CreateCell(3).SetCellValue("Order Hierarchy");
                //headerRow.CreateCell(4).SetCellValue("Order Number");
                //headerRow.CreateCell(5).SetCellValue("Categories not compliant");
                sheet.CreateFreezePane(0, 1, 0, 1);

                int rowsNumber = 1;
                for (int rowNum = 0; dt.Rows.Count > rowNum; rowNum++)
                {
                    var row = sheet.CreateRow(rowsNumber++);
                    row.CreateCell(0).SetCellValue(Convert.ToString(dt.Rows[rowNum]["FirstName"]));
                    row.CreateCell(1).SetCellValue(Convert.ToString(dt.Rows[rowNum]["LastName"]));
                    row.CreateCell(2).SetCellValue(Convert.ToString(dt.Rows[rowNum]["PhoneNumber"]));
                    row.CreateCell(3).SetCellValue(Convert.ToString(dt.Rows[rowNum]["OrderHierarchy"]));
                    row.CreateCell(4).SetCellValue(Convert.ToString(dt.Rows[rowNum]["OrderNumber"]));
                    row.CreateCell(5).SetCellValue(Convert.ToString(dt.Rows[rowNum]["CategoryNotCompliant"]));
                }

                MemoryStream output = new MemoryStream();
                wb.Write(output);
                fileBytes = output.GetBuffer();
            }
            return fileBytes;
        }



        #endregion

        #region UAT-4613

        public static Byte[] GetInProcessAgencyfromApplicantServiceFormStatus(DataTable dt, String fileName)
        {
            HSSFWorkbook wb;
            HSSFSheet sheet;
            Byte[] fileBytes = null;
            if (!File.Exists(fileName + ".xls"))
            {
                wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
                sheet = (HSSFSheet)wb.CreateSheet("Sheet1");

                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Applicant FirstName");
                headerRow.CreateCell(1).SetCellValue("Applicant LastName");
                headerRow.CreateCell(2).SetCellValue("Institution");
                headerRow.CreateCell(3).SetCellValue("Order Number");
                headerRow.CreateCell(4).SetCellValue("Recieved Date");
                sheet.CreateFreezePane(0, 1, 0, 1);

                int rowsNumber = 1;
                for (int rowNum = 0; dt.Rows.Count > rowNum; rowNum++)
                {
                    var row = sheet.CreateRow(rowsNumber++);
                    row.CreateCell(0).SetCellValue(Convert.ToString(dt.Rows[rowNum]["ApplicantFirstName"]));
                    row.CreateCell(1).SetCellValue(Convert.ToString(dt.Rows[rowNum]["ApplicantLastName"]));
                    row.CreateCell(2).SetCellValue(Convert.ToString(dt.Rows[rowNum]["Institution"]));
                    row.CreateCell(3).SetCellValue(Convert.ToString(dt.Rows[rowNum]["OrderNumber"]));
                    row.CreateCell(4).SetCellValue(Convert.ToString(dt.Rows[rowNum]["RecievedDate"]));
                }

                MemoryStream output = new MemoryStream();
                wb.Write(output);
                fileBytes = output.GetBuffer();
            }
            return fileBytes;
        }

        #endregion
    }
}
