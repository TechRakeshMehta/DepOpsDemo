using System;

namespace DAL.Interfaces
{
    public interface IApplicationDataRepository
    {
        Object GetObjectDataByKey(String key);

        void AddWebApplicationData(String key, Object data, Int32 validtimespan);

        void RemoveWebApplicationData(String key);

        void UpdateWebApplicationData(String key, Object data);
    }
}


