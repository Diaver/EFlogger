using System;

namespace EFlogger.Profiling
{
    public static class Nlogger
    {
        private static Action<string> _action;

        public static void AddLogMessage(string message)
        {
            if (_action != null)
                _action(message);
        }

        public static void SetLogDelegate(Action<string> action)
        {
            _action = action;
        }
    }
}
