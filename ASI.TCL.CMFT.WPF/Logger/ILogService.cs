using System;

namespace ASI.TCL.CMFT.WPF.Logger
{
    public interface ILogService
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message, Exception ex = null);
    }
}
