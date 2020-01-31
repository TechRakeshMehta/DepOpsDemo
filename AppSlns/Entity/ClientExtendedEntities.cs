#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.ComponentModel.DataAnnotations.Schema;


#endregion

#region Application Specific

#endregion

#endregion

namespace Entity.ClientEntities
{
    public partial class ClientComplianceItemAttribute
    {
        public String FormatOptions
        {
            get
            {
                StringBuilder formatOptions = new StringBuilder();
                if (this.ClientComplianceAttributeOptions != null && this.ClientComplianceAttributeOptions.Count > 0)
                {
                    foreach (ClientComplianceAttributeOption attributeOption in this.ClientComplianceAttributeOptions.Where(x => !x.IsDeleted))
                        formatOptions.AppendFormat("{0}={1},", attributeOption.OptionText, attributeOption.OptionValue);

                    Int32 index = formatOptions.ToString().LastIndexOf(',');
                    if (index >= 0)
                        formatOptions.Remove(index, 1);
                }
                return formatOptions.ToString();
            }
        }
    }

    public partial class ApplicantUploadedDocument
    {
        public String MappedDocumentDetails
        {
            get
            {
                if (this.ApplicantComplianceDocumentMaps == null || this.ApplicantComplianceDocumentMaps.Count == 0)
                    return String.Empty;

                var documentMaps = this.ApplicantComplianceDocumentMaps.Where(x => !x.IsDeleted);
                var attributeData = documentMaps.Select(x => x.ApplicantComplianceAttributeData).Where(x => !x.IsDeleted);
                var itemData = attributeData.Select(x => x.ApplicantComplianceItemData).Where(x => !x.IsDeleted);

                IEnumerable<ClientComplianceItem> mappedItems = itemData
                    .Select(x => x.ClientComplianceItem)
                    .Where(x => !x.IsDeleted);


                List<MappedDocumentPackageDetail> mappedDocumentPackageDetails = new List<MappedDocumentPackageDetail>();

                foreach (ClientComplianceItem mappedItem in mappedItems)
                {
                    MappedDocumentPackageDetail packageDetail = mappedDocumentPackageDetails.FirstOrDefault(x => x.ClientCompliancePackageID == mappedItem.ClientComplianceCategory.ClientCompliancePackage.ClientCompliancePackageID);

                    if (packageDetail == null)
                    {
                        packageDetail = new MappedDocumentPackageDetail();
                        packageDetail.ClientCompliancePackageID = mappedItem.ClientComplianceCategory.ClientCompliancePackage.ClientCompliancePackageID;
                        packageDetail.PackageName = mappedItem.ClientComplianceCategory.ClientCompliancePackage.PackageName;

                        packageDetail.MappedDocumentCategoryDetails = new List<MappedDocumentPackageDetail.MappedDocumentCategoryDetail>();
                        MappedDocumentPackageDetail.MappedDocumentCategoryDetail categoryDetail = new MappedDocumentPackageDetail.MappedDocumentCategoryDetail();
                        categoryDetail.ClientComplianceCategoryID = mappedItem.ClientComplianceCategory.ClientComplianceCategoryID;
                        categoryDetail.CategoryName = mappedItem.ClientComplianceCategory.CategoryName;
                        categoryDetail.ItemsName = mappedItem.Name;
                        packageDetail.MappedDocumentCategoryDetails.Add(categoryDetail);

                        mappedDocumentPackageDetails.Add(packageDetail);
                    }
                    else
                    {
                        MappedDocumentPackageDetail.MappedDocumentCategoryDetail categoryDetail = packageDetail.MappedDocumentCategoryDetails
                            .FirstOrDefault(x => x.ClientComplianceCategoryID == mappedItem.ClientComplianceCategory.ClientComplianceCategoryID);

                        if (categoryDetail == null)
                        {
                            categoryDetail = new MappedDocumentPackageDetail.MappedDocumentCategoryDetail();
                            categoryDetail.ClientComplianceCategoryID = mappedItem.ClientComplianceCategory.ClientComplianceCategoryID;
                            categoryDetail.CategoryName = mappedItem.ClientComplianceCategory.CategoryName;
                            categoryDetail.ItemsName = mappedItem.Name;
                            packageDetail.MappedDocumentCategoryDetails.Add(categoryDetail);
                        }
                        else
                            categoryDetail.ItemsName += String.Format(", {0}", mappedItem.Name);
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (MappedDocumentPackageDetail packageDetail in mappedDocumentPackageDetails)
                {
                    sb.Append("<div style='padding-left: 10px'>");
                    sb.Append(packageDetail.PackageName);
                    foreach (MappedDocumentPackageDetail.MappedDocumentCategoryDetail categoryDetail in packageDetail.MappedDocumentCategoryDetails)
                    {
                        sb.AppendFormat("<div style='padding-left: 10px'>{0}<div style='padding-left: 10px'>{1}</div></div>",
                            categoryDetail.CategoryName,
                            categoryDetail.ItemsName);
                    }
                    sb.Append("</div>");
                }

                return sb.ToString();

            }
        }

        private class MappedDocumentPackageDetail
        {
            public Int32 ClientCompliancePackageID { get; set; }
            public String PackageName { get; set; }
            public List<MappedDocumentCategoryDetail> MappedDocumentCategoryDetails { get; set; }

            public class MappedDocumentCategoryDetail
            {
                public Int32 ClientComplianceCategoryID { get; set; }
                public String CategoryName { get; set; }
                public String ItemsName { get; set; }
            }

        }
    }

}
