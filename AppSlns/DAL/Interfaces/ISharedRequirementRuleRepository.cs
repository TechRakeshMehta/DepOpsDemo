

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System.Data;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace DAL.Interfaces
{
    public interface ISharedRequirementRuleRepository
    {

        /// <summary>
        /// GetRuleTemplates
        /// </summary>
        /// <returns></returns>
        List<RequirementRuleTemplate> GetlstRuleTemplate();
        
        /// <summary>
        /// GetRuleTemplate
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        RequirementRuleTemplate GetRuleTemplate(Int32 ruleId);

        /// <summary>
        /// UpdateRuleTemplate
        /// </summary>
        /// <param name="rule"></param>
        void UpdateRuleTemplate(Entity.ComplianceRuleTemplate ruleTemplate, List<Int32> expressionIds);

        /// <summary>
        /// AddRuleTemplate
        /// </summary>
        /// <param name="ruleTemplate"></param>
        void AddRuleTemplate(Entity.ComplianceRuleTemplate ruleTemplate);

        /// <summary>
        /// Delete Rule Template
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="currentUserId"></param>
        void DeleteRuleTemplate(Int32 ruleId, Int32 currentUserId);

        /// <summary>
        /// ValidateRuleTemplate
        /// </summary>
        /// <param name="ruleTemplateXML"></param>
        /// <returns></returns>
        String ValidateRuleTemplate(string ruleTemplateXML);

        //String ValidateExpression(String ruleTemplateXml, String ruleExpression);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleTemplateId"></param>
        /// <returns></returns>
        //RequirementRuleTemplate GetRuleTemplateDetails(Int32 ruleTemplateId);
        
    }
}
