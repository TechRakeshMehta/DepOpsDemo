using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;

namespace INTSOF.Utils.FingerPrint
{
   public class NISTSpecification
    {
        public byte[] RemoveField(int RecordType, string FieldType, byte[] FileBytes)
        {
            try
            {
                List<byte> newbytes = new List<byte>();
                Encoding ascii = Encoding.ASCII;


                //Value of field field separator u\
                string valueOfSeparator = ((char)0x1D).ToString();

                //Regex for creating 
                Regex reForRemoveField = new Regex(RecordType + "." + FieldType + ":");

                //Regex For Field Count Update for NIST File 
                Regex reForCountUpdate = new Regex(RecordType + ".001:");


                //Get String From Orignal Bytes
                string dataString = ascii.GetString(FileBytes);

                //Match Fields
                Match match = reForRemoveField.Match(dataString);
                Match matchFieldsCountUpdate = reForCountUpdate.Match(dataString);

                if (!match.Success)
                {
                    return FileBytes;
                }

                while (match.Success)
                {

                    int startIndexOfRemoveField = dataString.IndexOf(valueOfSeparator, match.Index);
                    for (int i = 0; i < FileBytes.Length; i++)
                    {
                        if (i < match.Index || i > startIndexOfRemoveField)
                        {
                            newbytes.Add(FileBytes[i]);
                        }
                    }
                    int diff = FileBytes.Length - newbytes.Count;


                    // code for decrease count 
                    int valueOfCountUpdate = dataString.IndexOf(valueOfSeparator, matchFieldsCountUpdate.Index);
                    string strCountValue = dataString.Substring(matchFieldsCountUpdate.Index, (valueOfCountUpdate - matchFieldsCountUpdate.Index));
                    strCountValue = strCountValue.Split(':')[1];
                    int countValue = Convert.ToInt32(strCountValue);
                    int lenStartAddress = matchFieldsCountUpdate.Index + matchFieldsCountUpdate.Length;
                    int lenToRemove = strCountValue.Length;

                    for (int i = lenStartAddress + lenToRemove - 1; i >= lenStartAddress; i--)
                    {
                        newbytes.RemoveAt(i);
                    }
                    string actualCount = (countValue - diff).ToString();
                    for (int i = 0; i < actualCount.Length; i++)
                    {
                        newbytes.Insert(lenStartAddress + i, (byte)actualCount[i]);
                    }
                    Match nmatch = match.NextMatch();
                    match = nmatch;
                }
                return newbytes.ToArray();
            }
            catch(Exception ex)
            {
                return FileBytes;
            }
            
        }
    }
}
