using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class InstitutionProfileContract
    {
        public InstitutionProfileContract(string name, double share, bool isExploded, string pieColor)
        {
            _name = name;
            _share = share;
            _isExploded = isExploded;
            _pieColor = pieColor;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private double _share;
        public double Share
        {
            get { return _share; }
            set { _share = value; }
        }
        private bool _isExploded;
        public bool IsExploded
        {
            get { return _isExploded; }
            set { _isExploded = value; }
        }
        //UAT 2727
        private string _pieColor;
        public string pieColor
        {
            get { return _pieColor; }
            set { _pieColor = value; }
        }

        
    }
}
