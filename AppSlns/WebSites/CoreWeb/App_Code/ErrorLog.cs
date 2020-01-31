using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using INTSOF.Logger.consts;
using System.Configuration;
/// <summary>
/// Summary description for ErrorLog
/// </summary>
public class ErrorLog
{
	private StreamWriter _streamWriter;
    private static Object _lockObject = new Object();

    public ErrorLog(String message, Exception ex = null)
	{
        try
        {
            Boolean isLogOnlinePayment = Convert.ToBoolean(ConfigurationManager.AppSettings[SysXLoggerConst.IS_LOG_ONLINEPAYMENT]);
            if (isLogOnlinePayment)
            {
                lock(_lockObject)
                {
                    String fileFullPath = CreateFile();
                    _streamWriter = new StreamWriter(fileFullPath, true);
                    WriteError(message, ex);
                    _streamWriter.AutoFlush = true;
                    _streamWriter.Close();
                }
            }
        }
        catch
        {
            // do not propagate exception if occured during text file logging
        }
	}

    private static String CreateFile()
    {
        String folderPath = ConfigurationManager.AppSettings[SysXLoggerConst.LOG_ONLINEPAYMENT_FOLDER_PATH];

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        String fileFullPath = folderPath + SysXLoggerConst.LOGGER_INITIALS + DateTime.Now.ToString(SysXLoggerConst.LOGGER_DATE_FORMAT) + SysXLoggerConst.LOGGER_EXTENSION;
        
        //Get the latest file formed today.
        String[] fileNamesArray = Directory.GetFiles(folderPath, "*" + DateTime.Now.ToString(SysXLoggerConst.LOGGER_DATE_FORMAT) + "_*" + SysXLoggerConst.LOGGER_EXTENSION);
        Int32 fileNumber = 0;

        foreach (var fileName in fileNamesArray)
        {
            if (fileName.Contains('_'))
            {
                Int32 indexOfUnderscore = fileName.LastIndexOf('_') + 1;
                Int32 indexOfDot = fileName.LastIndexOf('.');
                Int32 currentFileNumber = Convert.ToInt32(fileName.Substring(indexOfUnderscore, indexOfDot - indexOfUnderscore));

                if (fileNumber < currentFileNumber)
                {
                    fileNumber = currentFileNumber;
                    fileFullPath = fileName;
                }
            }
        }
        fileFullPath = CheckFileSize(folderPath, fileFullPath, fileNumber);        
        return fileFullPath;
    }

    //Check the file size.
    private static String CheckFileSize(String folderPath, String fileFullPath, Int32 fileNumber)
    {
        FileInfo fileInfo = new FileInfo(fileFullPath);

        if (fileInfo.Exists)
        {
            long filesizeInByte = fileInfo.Length;
            Double filesizeInMB = (filesizeInByte / 1024f) / 1024f;

            if (filesizeInMB > 5)
            {
                fileFullPath = CreateNewFile(folderPath, fileFullPath, fileNumber);
            }
        }
        return fileFullPath;
    }

    //Creates a new file if Sixe of old file is greater than 5 MB.
    private static String CreateNewFile(String folderPath, String fileFullPath, Int32 fileNumber)
    {
        fileFullPath = folderPath + SysXLoggerConst.LOGGER_INITIALS + DateTime.Now.ToString(SysXLoggerConst.LOGGER_DATE_FORMAT) + "_" +
            (fileNumber + 1) + SysXLoggerConst.LOGGER_EXTENSION;
        return fileFullPath;
    }

    //Writes the information and/or error.
    private void WriteError(String message, Exception ex = null)
    {
        if (ex != null)
        {
            _streamWriter.WriteLine(DateTime.Now.ToString(SysXLoggerConst.DATETIME_G) + SysXLoggerConst.PIPE_SYMBOL + message + Environment.NewLine + ex);
        }
        else
        {
            _streamWriter.WriteLine(DateTime.Now.ToString(SysXLoggerConst.DATETIME_G) + SysXLoggerConst.PIPE_SYMBOL + message);
        }
    }
}