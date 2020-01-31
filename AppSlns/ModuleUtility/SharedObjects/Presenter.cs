using Business.RepoManagers;
using System;

namespace INTSOF.SharedObjects
{
    public abstract class Presenter<TView> : IDisposable
    {

        protected Presenter()
        { }

        public TView View { get; set; }

        public virtual void OnViewInitialized()
        { }
        public virtual void OnViewLoaded()
        { }
        public virtual void Dispose()
        { }

    }
}
