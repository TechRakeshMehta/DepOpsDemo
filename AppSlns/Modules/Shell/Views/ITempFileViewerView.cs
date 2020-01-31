using Entity;
using System;

namespace CoreWeb.Shell.Views
{
    public interface ITempFileViewerView
    {
        Guid Id
        {
            get;
            set;
        }

        String FilePath
        {
            get;
            set;
        }

        Double TotalMinutes
        {
            get;
            set;
        }
        TempFile TempFileRecord
        {
            get;
            set;
        }
    }
}




