using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.PlacementMatching
{
    [Serializable]
    [DataContract]
    public class ShiftDetails
    {
        [DataMember]
        public Int32? ClinicalInventoryID { get; set; }
        [DataMember]
        public Int32 ClinicalInventoryShiftID { get; set; }
        [DataMember]
        public String Shift { get; set; }
        [DataMember]
        public TimeSpan? ShiftFrom { get; set; }
        [DataMember]
        public TimeSpan? ShiftTo { get; set; }
        [DataMember]
        public List<Int32> lstDaysId { get; set; }
        [DataMember]
        public String Days { get; set; }
        [DataMember]
        public Int32 NumberOfStudents { get; set; }
        [DataMember]
        public Boolean IsEditClick { get; set; }
        [DataMember]
        public String ShiftFromString
        {
            get
            {
                if (!ShiftFrom.IsNullOrEmpty())
                {
                    if ((ShiftFrom.Value.Hours * AppConsts.HUNDRED + ShiftFrom.Value.Minutes) >= (AppConsts.TWELVE * AppConsts.HUNDRED))
                    {
                        return ((!ShiftFrom.Value.Hours.IsNullOrEmpty() && ShiftFrom.Value.Hours > AppConsts.NONE ? (ShiftFrom.Value.Hours == AppConsts.TWELVE ? ShiftFrom.Value.Hours.ToString() : ((ShiftFrom.Value.Hours - AppConsts.TWELVE) > AppConsts.NINE ? (ShiftFrom.Value.Hours - AppConsts.TWELVE).ToString() : "0" + (ShiftFrom.Value.Hours - AppConsts.TWELVE).ToString())) : "00") //(ShiftFrom.Value.Hours == AppConsts.TWELVE ? ShiftFrom.Value.Hours.ToString() : ((ShiftFrom.Value.Hours - AppConsts.TWELVE).ToString())) : "00")
                                    + ":" + (!ShiftFrom.Value.Minutes.IsNullOrEmpty() && ShiftFrom.Value.Minutes > AppConsts.NONE ? ShiftFrom.Value.Minutes.ToString() : "00") + " " + "PM");
                    }
                    else
                    {
                        return ((!ShiftFrom.Value.Hours.IsNullOrEmpty() ? (ShiftFrom.Value.Hours == AppConsts.NONE ? AppConsts.TWELVE.ToString() : (ShiftFrom.Value.Hours > AppConsts.NINE ? ShiftFrom.Value.Hours.ToString() : ("0" + ShiftFrom.Value.Hours.ToString()))) : "00")   //(ShiftFrom.Value.Hours == AppConsts.NONE ? AppConsts.TWELVE.ToString() : ("0" + ShiftFrom.Value.Hours.ToString())) : "00")
                                    + ":" + (ShiftFrom.Value.Minutes > AppConsts.NONE ? ShiftFrom.Value.Minutes.ToString() : "00") + " " + "AM");
                    }
                }
                return String.Empty;
            }
        }
        [DataMember]
        public String ShiftToString
        {
            get
            {
                if (!ShiftTo.IsNullOrEmpty())
                {
                    if ((ShiftTo.Value.Hours * AppConsts.HUNDRED + ShiftTo.Value.Minutes) >= (AppConsts.TWELVE * AppConsts.HUNDRED))
                    {
                        return ((!ShiftTo.Value.Hours.IsNullOrEmpty() && ShiftTo.Value.Hours > AppConsts.NONE ? (ShiftTo.Value.Hours == AppConsts.TWELVE ? ShiftTo.Value.Hours.ToString() : ((ShiftTo.Value.Hours - AppConsts.TWELVE) > AppConsts.NINE ? (ShiftTo.Value.Hours - AppConsts.TWELVE).ToString() : "0" + (ShiftTo.Value.Hours - AppConsts.TWELVE).ToString())) : "00")
                                    + ":" + (!ShiftTo.Value.Minutes.IsNullOrEmpty() && ShiftTo.Value.Minutes > AppConsts.NONE ? ShiftTo.Value.Minutes.ToString() : "00") + " " + "PM");
                    }
                    else
                    {
                        return ((!ShiftTo.Value.Hours.IsNullOrEmpty() ? (ShiftTo.Value.Hours == AppConsts.NONE ? AppConsts.TWELVE.ToString() : (ShiftTo.Value.Hours > AppConsts.NINE ? ShiftTo.Value.Hours.ToString() : ("0" + ShiftTo.Value.Hours.ToString()))) : "00")
                                    + ":" + (ShiftTo.Value.Minutes > AppConsts.NONE ? ShiftTo.Value.Minutes.ToString() : "00") + " " + "AM");
                    }
                }
                return String.Empty;
            }
        }
    }
}
