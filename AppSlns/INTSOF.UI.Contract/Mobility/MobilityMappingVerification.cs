using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.Mobility
{
    /// <summary>
    /// Used to return the results for the validation process of the mapping, when admin change program is submitted
    /// </summary>
    public class MobilityMappingVerification
    {
        public MobilityMappingVerification(String vaildationMessage, Boolean isReviewRequiredEveryTransaction, Boolean ifReviewMappingCanBeSkipped)
        {
            this.ValidationMessage = vaildationMessage;
            this.IsReviewRequiredEveryTransaction = isReviewRequiredEveryTransaction;
            this.IfReviewMappingCanBeSkipped = ifReviewMappingCanBeSkipped;
        }

        public String ValidationMessage { get; set; }
        public Boolean IsReviewRequiredEveryTransaction { get; set; }
        public Boolean IfReviewMappingCanBeSkipped { get; set; }
    }
}
