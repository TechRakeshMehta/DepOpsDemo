using Business.RepoManagers;
using MobileWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace MobileWebApi.Service
{
    public static class ApplicantPackageService
    {
        public static List<PackageContract> GetApplicantPackages(Int32 tenantID, Int32 organizationUserID)
        {
            List<PackageContract> pkgList = new List<PackageContract>();
            //Get Data from ADB Manager layer
            List<PackageSubscription> lstPkgSubscription = new List<PackageSubscription>();
            lstPkgSubscription = ComplianceDataManager.GetSubscribedPackagesForUser(tenantID, organizationUserID);
            lstPkgSubscription.Where(cond => !cond.IsDeleted).ToList().ForEach(x =>
            {
                PackageContract packageDetail = new PackageContract();
                packageDetail.PackageSubscriptionId = x.PackageSubscriptionID;
                packageDetail.PackageId = x.CompliancePackageID;
                packageDetail.PackageName = x.CompliancePackage.PackageName;
                packageDetail.PackageStatus = x.lkpPackageComplianceStatu.Name;
                packageDetail.PackageName = x.CompliancePackage.PackageName;
                pkgList.Add(packageDetail);
            });

            return pkgList;
        }

        public static PackageContract GetApplicantPackageCategoryDetails(Int32 tenantID, Int32 organizationUserID, Int32 pkgId)
        {
            PackageContract packageContract = new PackageContract();
            //Get Data from ADB Manager layer
            PackageSubscription pkgSubscription = new PackageSubscription();
            pkgSubscription = ComplianceDataManager.GetPackageSubscriptionByPackageID(pkgId, organizationUserID, tenantID);
            packageContract.PackageSubscriptionId = pkgSubscription.PackageSubscriptionID;
            packageContract.PackageId = pkgSubscription.CompliancePackageID;
            packageContract.PackageName = String.IsNullOrEmpty(pkgSubscription.CompliancePackage.PackageLabel) ? pkgSubscription.CompliancePackage.PackageName : pkgSubscription.CompliancePackage.PackageLabel;
            packageContract.PackageStatus = pkgSubscription.lkpPackageComplianceStatu.Name;

            List<ItemPaymentContract> itemPaymentData = ComplianceDataManager.GetItemPaymentDetail(pkgSubscription.PackageSubscriptionID, tenantID, false);

            if (packageContract.lstCategory == null)
            {
                packageContract.lstCategory = new List<CategoryContract>();
            }
            pkgSubscription.ApplicantComplianceCategoryDatas.Where(cond => !cond.IsDeleted).ToList().ForEach(cd =>
            {
                CategoryContract categoryData = new CategoryContract();
                categoryData.CategoryId = cd.ComplianceCategoryID;
                categoryData.CategoryName = String.IsNullOrEmpty(cd.ComplianceCategory.CategoryLabel) ? cd.ComplianceCategory.CategoryName : cd.ComplianceCategory.CategoryLabel;
                categoryData.CategoryStatus = cd.lkpCategoryComplianceStatu.Name;
                categoryData.CategoryNonCompliancedate = cd.CategoryComplianceExpiryDate.HasValue ? cd.CategoryComplianceExpiryDate.Value.ToShortDateString() : String.Empty;
                if (categoryData.lstItem == null)
                {
                    categoryData.lstItem = new List<ItemContract>();
                }
                cd.ApplicantComplianceItemDatas.Where(cond => !cond.IsDeleted).ToList().ForEach(itemData =>
                {
                    ItemContract itemcontract = new ItemContract();
                    itemcontract.ItemId = itemData.ComplianceItemID;
                    itemcontract.ItemName = String.IsNullOrEmpty(itemData.ComplianceItem.ItemLabel) ? itemData.ComplianceItem.Name : itemData.ComplianceItem.ItemLabel;
                    itemcontract.ItemStatus = itemData.lkpItemComplianceStatu.Name;

                    if (itemcontract.lstAttribute == null)
                    {
                        itemcontract.lstAttribute = new List<AttributeContract>();
                    }

                    if (itemPaymentData.Count > 0 && itemPaymentData.Where(cond => cond.ItemID == itemData.ComplianceItemID).ToList().Count > 0)
                    {
                        ItemPaymentContract itempayment = new ItemPaymentContract();
                        itempayment = itemPaymentData.Where(cond => cond.ItemID == itemData.ComplianceItemID).FirstOrDefault();
                        AttributeContract attrDetails = new AttributeContract();
                        attrDetails.AttributeName = "Amount";
                        attrDetails.AttributeValue = "$" + itempayment.PaidAmount.ToString("0.00");
                        itemcontract.lstAttribute.Add(attrDetails);
                        if (itempayment.OrderNumber != null || itempayment.OrderNumber != "")
                        {
                            attrDetails = new AttributeContract();
                            attrDetails.AttributeName = "Order Number";
                            attrDetails.AttributeValue = itempayment.OrderNumber.ToString();
                            itemcontract.lstAttribute.Add(attrDetails);
                            attrDetails = new AttributeContract();
                            attrDetails.AttributeName = "Payment Status";
                            attrDetails.AttributeValue = itempayment.OrderStatus.ToString();
                            itemcontract.lstAttribute.Add(attrDetails);
                        }
                    }
                    itemData.ApplicantComplianceAttributeDatas.Where(cond => !cond.IsDeleted).ToList().ForEach(attrData =>
                    {
                        AttributeContract attrDetails = new AttributeContract();
                        attrDetails.AttributeId = attrData.ComplianceAttributeID;
                        attrDetails.AttributeName = String.IsNullOrEmpty(attrData.ComplianceAttribute.AttributeLabel) ? attrData.ComplianceAttribute.Name : attrData.ComplianceAttribute.AttributeLabel;
                        String attributeTypeCode = attrData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code;
                        if (attributeTypeCode == "ADTDAT" && !String.IsNullOrEmpty(attrData.AttributeValue))
                        {
                            if (attrData.AttributeValue.Contains(":") && attrData.AttributeValue.Contains(" "))
                            {
                                attrDetails.AttributeValue = attrData.AttributeValue.Substring(0,attrData.AttributeValue.IndexOf(" "));
                            }
                            else
                            {
                                attrDetails.AttributeValue = attrData.AttributeValue;
                            }
                        }
                        else if (attributeTypeCode == "ADTOPT" && !String.IsNullOrEmpty(attrData.AttributeValue))
                        {
                            var obj = attrData.ComplianceAttribute.ComplianceAttributeOptions.Where(cond => !cond.IsDeleted && cond.ComplianceItemAttributeID == attrData.ComplianceAttributeID
                                                                                             && cond.OptionValue == attrData.AttributeValue).FirstOrDefault();
                            if (obj != null)
                            { 
                                attrDetails.AttributeValue = obj.OptionText;
                            }
                            else
                            {
                                attrDetails.AttributeValue = String.Empty;
                            }
                        }
                        else if (attributeTypeCode == "ADTVWD" || attributeTypeCode == "ADTSDOC" || attributeTypeCode == "ADTSIGN")
                        {
                            attrDetails.AttributeValue = attrData.AttributeValue == "True" ? "Yes" : "No";
                        }
                        else
                        {
                            attrDetails.AttributeValue = attrData.AttributeValue;
                        }
                        itemcontract.lstAttribute.Add(attrDetails);
                    });
                    categoryData.lstItem.Add(itemcontract);
                });
                packageContract.lstCategory.Add(categoryData);
            });

            return packageContract;
        }
    }
}