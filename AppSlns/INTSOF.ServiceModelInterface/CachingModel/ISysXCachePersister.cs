using System;

namespace CoreWeb.IntsofCachingModel.Interface
{
    public interface ISysXCachePersister
    {
        void persistCache(Object objToPersist);

        Object getObjectFromStore();

    }
}
