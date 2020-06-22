using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using EFlogger.Network.Commands;
using EFlogger.Network.Network;
using EFlogger.Profiling.Helpers;

namespace EFlogger.Profiling
{
    public static class Logger
    {
        public static bool IsSendToNetwork { get; set; }
        public static bool IsEnableDecompiling { get; set; }

        static Logger()
        {
            IsSendToNetwork = true;
        }

        public static void Log(string commandText, long elapsedMilliseconds, StackFrame stackFrame, int resultRowsCount, string stackTrace)
        {
            if (!IsSendToNetwork) return;

            ThreadPool.QueueUserWorkItem(LogInThread, new ThreadInfoLogCommand
            {
                CommandText = commandText,
                ResultRowsCount = resultRowsCount,
                ElapsedMilliseconds = elapsedMilliseconds,
                StackFrame = stackFrame,
                StackTraceSnippet = stackTrace
            });
        }

        private static void LogInThread(object threadInfoObject)
        {
            var infoLogCommand = (ThreadInfoLogCommand)threadInfoObject;

            MethodBase methodBase = infoLogCommand.StackFrame.GetMethod();
            string methodBody = GetMethodBody(methodBase);

            if (IsSendToNetwork)
            {
                var queryCommand = new QueryCommand
                {
                    CommandText = infoLogCommand.CommandText,
                    Created = DateTime.Now.ToString(),
                    MethodName = methodBase.Name,
                    MethodBody = methodBody,
                    ClassName = methodBase.DeclaringType.FullName,
                    QueryMiliseconds = infoLogCommand.ElapsedMilliseconds,
                    ResultRowsCount = infoLogCommand.ResultRowsCount,
                    StackTrace = infoLogCommand.StackTraceSnippet
                };
                CommandSender.SendQueryCommand(queryCommand);
            }
            Nlogger.AddLogMessage(string.Format("\r\n Command text:{0}; \r\n  Method Name:{1}; \r\n Class Name:{2}; \r\n Elapsed Miliseconds:{3}", infoLogCommand.CommandText, methodBase.Name, methodBase.DeclaringType.FullName, infoLogCommand.ElapsedMilliseconds));
        }

        public static void LogException(Exception exception, long elapsedMilliseconds, StackFrame stackFrame, int resultRowsCount, string stackTrace)
        {
            if (!IsSendToNetwork) return;

            ThreadPool.QueueUserWorkItem(LogExceptionInThread, new ThreadInfoLogCommand
            {
                Exception = exception,
                ResultRowsCount = resultRowsCount,
                ElapsedMilliseconds = elapsedMilliseconds,
                StackFrame = stackFrame,
                StackTraceSnippet = stackTrace
            });
        }

        private static void LogExceptionInThread(object threadInfoObject)
        {
            var infoLogCommand = (ThreadInfoLogCommand)threadInfoObject;
            MethodBase methodBase = infoLogCommand.StackFrame.GetMethod();
            string methodBody = GetMethodBody(methodBase);

            string exceptionText = infoLogCommand.Exception == null ? string.Empty : infoLogCommand.Exception.Message + infoLogCommand.Exception.InnerException;

            if (IsSendToNetwork)
            {

                var queryCommand = new QueryCommand
                {
                    CommandText = exceptionText,
                    Created = DateTime.Now.ToString(),
                    MethodName = methodBase.Name,
                    MethodBody = methodBody,
                    ClassName = methodBase.DeclaringType.FullName,
                    QueryMiliseconds = infoLogCommand.ElapsedMilliseconds,
                    ResultRowsCount = infoLogCommand.ResultRowsCount,
                    StackTrace = infoLogCommand.StackTraceSnippet
                };
                CommandSender.SendQueryCommand(queryCommand);
            }

            Nlogger.AddLogMessage(string.Format("\r\n Exception {0}; \r\n Method Name {1}; \r\n Class Name {2}; \r\n Elapsed Miliseconds {3}", exceptionText, methodBase.Name, methodBase.DeclaringType.FullName, infoLogCommand.ElapsedMilliseconds));
        }

        public static void WriteMessage(string message)
        {
            if (!IsSendToNetwork) return;

            StackFrame stackFrame;
            string stackTrace = StackTraceSnippet.Get(out stackFrame);
            ThreadPool.QueueUserWorkItem(WriteMessageInThread, new ThreadInfoLogCommand
            {
                StackFrame = stackFrame,
                Message = message,
                StackTraceSnippet = stackTrace
            });
        }

        private static void WriteMessageInThread(object threadInfoObject)
        {
            var infoLogCommand = (ThreadInfoLogCommand)threadInfoObject;

            MethodBase methodBase = infoLogCommand.StackFrame.GetMethod();
            string methodBody = GetMethodBody(methodBase);

            if (IsSendToNetwork)
            {
                var queryCommand = new QueryCommand
                {
                    CommandText = infoLogCommand.Message,
                    Created = DateTime.Now.ToString(),
                    MethodName = methodBase.Name,
                    MethodBody = methodBody,
                    ClassName = methodBase.DeclaringType.FullName,
                    QueryMiliseconds = 0,
                    ResultRowsCount = 0,
                    StackTrace = infoLogCommand.StackTraceSnippet
                };
                CommandSender.SendQueryCommand(queryCommand);
            }
           
                Nlogger.AddLogMessage(string.Format("\r\n Message: {0};\r\n Method Name: {1};\r\n Class Name: {2}", infoLogCommand.Message, methodBase.Name, methodBase.DeclaringType.FullName));
        }

        private static string GetMethodBody(MethodBase methodBase)
        {
            return IsEnableDecompiling
                ? Decompiler.GetSourceCode(methodBase.Module.FullyQualifiedName, methodBase.DeclaringType.Name, methodBase.Name)
                : "To enable decompiling, call method EFloggerFor6.EnableDecompiling() or EFloggerFor4.EnableDecompiling()";
        }

        public static void ClearLog()
        {
            if (!IsSendToNetwork) return;

            ThreadPool.QueueUserWorkItem(ClearLogInThread, null);
        }

        private static void ClearLogInThread(object threadInfoObject)
        {
            if (IsSendToNetwork)
            {
                CommandSender.SendClearLogDataGrid();
            }
        }

        public static void SetLogDelegate(Action<string> action)
        {
            Nlogger.SetLogDelegate(action);
        }
    }

    class ThreadInfoLogCommand
    {
        public string CommandText { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public StackFrame StackFrame { get; set; }
        public int ResultRowsCount { get; set; }
        public string StackTraceSnippet { get; set; }

        public Exception Exception { get; set; }

        public string Message { get; set; }
    }
}