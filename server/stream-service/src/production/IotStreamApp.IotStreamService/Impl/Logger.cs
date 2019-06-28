// ************************************************************************
// *****      COPYRIGHT 2014 - 2018 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

namespace Honeywell.IotStreamApp.IotStreamService.Impl
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;

    public static class Logger
    {
        public static void LogInfo(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.TraceInformation(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber));
        }

        public static void LogError(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.TraceError(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber));
        }

        public static void LogWarn(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.TraceWarning(FormatMessage(message, memberName, sourceFilePath, sourceLineNumber));
        }

        public static void LogException(Exception ex, string message = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "",[CallerLineNumber] int sourceLineNumber = 0)
        {
            if (message != null && ex != null)
            {
                Trace.TraceError(FormatMessage($"{message}{Environment.NewLine}{ex}", memberName, sourceFilePath, sourceLineNumber));
            }
            else
            {
                Trace.TraceError(FormatMessage($"{message ?? ex?.ToString()}", memberName, sourceFilePath, sourceLineNumber));
            }
        }

        private static string FormatMessage(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            return $"{Path.GetFileName(sourceFilePath)}::{memberName}({sourceLineNumber}) {message}";
        }
    }
}