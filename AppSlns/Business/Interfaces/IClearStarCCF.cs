using System;
using System.Collections.Generic;

namespace Business.Interfaces
{
   public interface IClearStarCCF
    {       
       void SaveCCFDataAndPDF(Dictionary<String, Object> dicParam);
    }
}
