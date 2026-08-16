using System;
using UnityEngine;

namespace AgarthaLib.Logging
{
    public class DebugLogger : ILogger
    {
        private string _prefix = string.Empty;

        public DebugLogger(string prefix)
            => _prefix = prefix;

        public string FormatMessage(string message)
            => $"[{_prefix}] {message}";

        public void LogInfo(string message)
            => Debug.Log(FormatMessage(message));

        public void LogWarn(string message)
            => Debug.LogWarning(FormatMessage(message));

        public void LogError(string message)
            => Debug.LogError(FormatMessage(message));

        public void LogError(Exception exception)
            => Debug.LogError(FormatMessage($"{exception.Message}\n{exception.Source}"));
    }
}
