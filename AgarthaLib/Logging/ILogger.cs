using System;

namespace AgarthaLib.Logging
{
    public interface ILogger
    {
        public void LogInfo(string message);

        public void LogWarn(string message);

        public void LogError(string message);

        public void LogError(Exception exception);
    }
}
