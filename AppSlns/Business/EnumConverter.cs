using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Business
{
    public class EnumConverter
    {

        public static T ParseEnumbyCode<T>(String code) where T : struct
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidEnumArgumentException("The type specified is not an Enum.");
            }
            else
            {
                T obj;
                foreach (String item in Enum.GetNames(type))
                {
                    // Get fieldinfo for this type
                    FieldInfo fieldInfo = type.GetField(item.ToString());

                    // Get the stringvalue attributes
                    XmlEnumAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(XmlEnumAttribute), false) as XmlEnumAttribute[];

                    // Checking for Value and getting an enum out of it.
                    if (attribs.Any(c => c.Name.Split(',')[0].ToUpper().Equals(code.ToUpper())) && Enum.TryParse<T>(item.ToString(), out obj))
                    {
                        return obj;
                    }
                }
            }

            throw new KeyNotFoundException("Specified Code cannot be found in Enum.");
        }

        public static string GetLookUpCode<T>(T value) where T : struct
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidEnumArgumentException("The type specified is not an Enum.");
            }
            else
            {
                T obj;
                String item = Enum.GetName(type, value);
                // Get fieldinfo for this type
                FieldInfo fieldInfo = type.GetField(item.ToString());

                // Get the stringvalue attributes
                XmlEnumAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(XmlEnumAttribute), false) as XmlEnumAttribute[];

                if (attribs.Length > 0)
                {
                    return attribs[0].Name.Split(',')[AppConsts.NONE];
                }
            }
            return String.Empty;
        }

        public static string GetLookUpText<T>(Enum value) where T : struct
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidEnumArgumentException("The type specified is not an Enum.");
            }
            else
            {
                String item = Enum.GetName(type, value);
                // Get fieldinfo for this type
                FieldInfo fieldInfo = type.GetField(item.ToString());

                // Get the stringvalue attributes
                XmlEnumAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(XmlEnumAttribute), false) as XmlEnumAttribute[];

                if (attribs.Length > 0)
                {
                    return attribs[0].Name.Split(',')[AppConsts.ONE];
                }
            }
            return String.Empty;
        }
    }
}
