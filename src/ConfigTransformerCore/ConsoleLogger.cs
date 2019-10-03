using Microsoft.Web.XmlTransform;
using System;

namespace ConfigTransformerCore
{

    public class ConsoleLogger : IXmlTransformationLogger
    {
        public bool UseVerboseLogging { get; set; }

        public ConsoleLogger()
        {
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void LogMessage(string message, params object[] messageArgs)
        {
            Info(string.Format(message, messageArgs));
        }

        public void LogMessage(MessageType type, string message, params object[] messageArgs)
        {
            switch (type)
            {
                case MessageType.Verbose:
                    if (UseVerboseLogging) { LogMessage(message, messageArgs); }
                    break;
                default:
                    LogMessage(message, messageArgs);
                    break;
            }
        }

        public void LogWarning(string message, params object[] messageArgs)
        {
            Warn(string.Format(message, messageArgs));
        }

        public void LogWarning(string file, string message, params object[] messageArgs)
        {
            LogWarning(file, 0, 0, message, messageArgs);
        }

        public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            string format = "{0} ({1}, {2}) warning: {3}";
            Warn(string.Format(format, System.IO.Path.GetFileName(file), lineNumber, linePosition, string.Format(message, messageArgs)));
        }

        public void LogError(string message, params object[] messageArgs)
        {
            Error(string.Format(message, messageArgs));
        }

        public void LogError(string file, string message, params object[] messageArgs)
        {
            LogError(file, 0, 0, message, messageArgs);
        }

        public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            string format = "{0} ({1}, {2}) error: {3}";
            Error(string.Format(format, System.IO.Path.GetFileName(file), lineNumber, linePosition, string.Format(message, messageArgs)));
        }

        public void LogErrorFromException(Exception ex)
        {
            Error(ex);
        }

        public void LogErrorFromException(Exception ex, string file)
        {
            Error(ex);
        }

        public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
        {
            string message = ex.Message;
            LogError(file, lineNumber, linePosition, message);
        }

        public void StartSection(string message, params object[] messageArgs)
        {
            StartSection(MessageType.Normal, message, messageArgs);
        }

        public void StartSection(MessageType type, string message, params object[] messageArgs)
        {
            switch (type)
            {
                case MessageType.Verbose:
                    if (UseVerboseLogging) { LogMessage(message, messageArgs); }
                    break;
                default:
                    LogMessage(message, messageArgs);
                    break;
            }
        }

        public void EndSection(string message, params object[] messageArgs)
        {
            EndSection(MessageType.Normal, message, messageArgs);
        }

        public void EndSection(MessageType type, string message, params object[] messageArgs)
        {
            switch (type)
            {
                case MessageType.Verbose:
                    if (UseVerboseLogging) { LogMessage(message, messageArgs); }
                    break;
                default:
                    LogMessage(message, messageArgs);
                    break;
            }
        }

        private void Info(string message)
        {
            Console.WriteLine(message);
        }

        private void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void Error(string message)
        {
            if (Console.IsErrorRedirected) { Console.ForegroundColor = ConsoleColor.Red; }

            Console.Error.WriteLine(message);

            if (Console.IsErrorRedirected) { Console.ForegroundColor = ConsoleColor.White; }
        }

        private void Error(Exception ex)
        {
            while (true)
            {
                Error(ex.Message);
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    continue;
                }

                break;
            }
        }
    }
}
