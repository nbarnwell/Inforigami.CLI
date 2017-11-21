﻿namespace Inforigami.CLI.Host.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Inforigami.Regalo.Core;

    public class RegaloLogger : ILogger
    {
        private readonly object _syncLock = new object();

        public bool ShowDebugMessages { get; set; }

        public RegaloLogger()
        {
            ShowDebugMessages = false;
        }

        public void Debug(object sender, string format, params object[] args)
        {
            if (!ShowDebugMessages) return;

            Write(sender, Severity.Debug, string.Format(format, args));
        }

        public void Info(object sender, string format, params object[] args)
        {
            Write(sender, Severity.Information, string.Format(format, args));
        }

        public void Warn(object sender, string format, params object[] args)
        {
            Write(sender, Severity.Warning, string.Format(format, args));
        }

        public void Error(object sender, Exception exception, string format, params object[] args)
        {
            var message = string.Format(format, args);
            var exceptionText = GetExceptionReport(exception);
            message += "\r\n" + exceptionText;

            Write(sender, Severity.Exception, message);
        }

        public void Error(object sender, string format, params object[] args)
        {
            Write(sender, Severity.Error, string.Format(format, args));
        }

        private string GetExceptionReport(Exception exception)
        {
            var report = new StringBuilder();

            foreach (var ex in GetExceptions(exception))
            {
                report.AppendLine(FormatException(ex));
            }

            return report.ToString();
        }

        private IEnumerable<Exception> GetExceptions(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            do
            {
                yield return exception;
            } while ((exception = exception.InnerException) != null);
        }

        private string FormatException(Exception ex)
        {
            var result = new StringBuilder();

            result.AppendFormat("*** {0}: \"{1}\"", ex.GetType().Name, ex.Message).AppendLine();
            result.AppendFormat("{0}", ex.StackTrace).AppendLine();

            return result.ToString();
        }

        private void Write(object sender, Severity severity, string message)
        {
            lock (_syncLock)
            {
                WriteTimestamp();
                WriteDivider();
                WriteSeverity(severity);
                WriteDivider();
                WriteSender(sender);
                WriteDivider();
                WriteMessage(message);

                Console.WriteLine();
            }
        }

        private void WriteMessage(string message)
        {
            WriteText(message);
        }

        private void WriteSender(object sender)
        {
            var senderName = sender.GetType().Name;
            WriteText(senderName);
        }

        private void WriteTimestamp()
        {
            Console.Write($"{DateTimeOffset.Now:s}");
        }

        private void WriteSeverity(Severity severity)
        {
            ConsoleColor foreground = Console.ForegroundColor;
            ConsoleColor background = Console.BackgroundColor;
            string severityName = "";

            switch (severity)
            {
                case Severity.Debug:
                    foreground = ConsoleColor.Black;
                    background = ConsoleColor.Cyan;
                    severityName = "DEBUG";
                    break;
                case Severity.Information:
                    foreground = ConsoleColor.White;
                    background = ConsoleColor.DarkGreen;
                    severityName = "INFO";
                    break;
                case Severity.Warning:
                    foreground = ConsoleColor.White;
                    background = ConsoleColor.DarkYellow;
                    severityName = "WARN";
                    break;
                case Severity.Error:
                    foreground = ConsoleColor.White;
                    background = ConsoleColor.Red;
                    severityName = "ERROR";
                    break;
                case Severity.Exception:
                    foreground = ConsoleColor.White;
                    background = ConsoleColor.Red;
                    severityName = "EXCEPTION";
                    break;
            }

            Write(severityName, foreground, background);
        }

        private void WriteDivider()
        {
            Console.Write('\t');
        }

        private void Write(string text, ConsoleColor foreground, ConsoleColor background)
        {
            var prevForeground = Console.ForegroundColor;
            var prevBackground = Console.BackgroundColor;
            try
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                WriteText(text);
            }
            finally
            {
                Console.ForegroundColor = prevForeground;
                Console.BackgroundColor = prevBackground;
            }
        }

        private void WriteText(string text)
        {
            Console.Write(text);
        }

        private enum Severity
        {
            Debug,
            Information,
            Warning,
            Error,
            Exception
        }
    }
}