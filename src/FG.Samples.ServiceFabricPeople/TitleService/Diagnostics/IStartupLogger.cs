using System;

namespace TitleService.Diagnostics
{
    public interface IStartupLogger
    {
        void ServiceTypeRegistered(int processId, string name);
        void ServiceHostInitializationFailed(Exception exception);
        void UnhandledException(Exception exception);
    }
}