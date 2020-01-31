using System;

namespace WidgetDataWebService.Model
{
    [Serializable]
    public class CustDictionary
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public CustDictionary() { }
        public CustDictionary(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}