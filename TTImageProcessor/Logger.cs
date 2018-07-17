using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace TTImageProcessor
{
    /// <summary>
    /// Log consumer which writes to the system event log and, failing that, the screen.
    /// </summary>
    internal static class Logger
    {
        /// <summary>
        /// Severity or impact of a message, or of messages below which to suppress
        /// </summary>
        internal enum Level
        {
            /// <summary>Logger should emit all log messages.</summary>
            All = 0,

            /// <summary>Informational messages</summary>
            Info,

            /// <summary>Potential operational issues or root cause information. Processing should continue.</summary>
            Warning,

            /// <summary>Fatal messages which require intervention.  Processing cannot continue.</summary>
            Error,

            /// <summary>Logger should emit no log messages.</summary>
            None
        }

        private const string ApplicationName = "TTBoardReader";
        private const int MaxEventLogs = 20;
        private static readonly EventLog eventLog = new EventLog("Application");

        /// <summary>
        /// Gets or sets the minimum log message severity required for a message to be processed.
        /// </summary>
        internal static Level LogLevel { get; set; } = Level.All;

        private static readonly TimeSpan messageBoxDelay = TimeSpan.FromSeconds(5);
        private static DateTime lastMessageBox = DateTime.MinValue;
        private static int suppressedMessageBoxes = 0;

        private static readonly TimeSpan eventLogDelay = TimeSpan.FromSeconds(2);
        private static DateTime[] lastEventLogRingBuffer = Enumerable.Range(1, MaxEventLogs).Select(i => DateTime.MinValue).ToArray();
        private static int lastEventLogIndex = MaxEventLogs - 1;
        private static int suppressedEventLogs = 0;

        static Logger()
        {
            eventLog.Source = ApplicationName;
        }

        /// <summary>
        /// Writes a message of caller-supplied severity to the system event log.
        /// </summary>
        /// <param name="level">Message Severity</param>
        /// <param name="eventId">Unique Id for event log</param>
        /// <param name="message">Message to log</param>
        /// <returns><see langword="true"/>if the message was skipped due to log level, or was consumed by the event log successfully.  <see langword="false"/>if the message should have been added to the event log but could not be added.</returns>
        internal static bool WriteEntry(Level level, int eventId, string message,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (level < LogLevel) return true;
            EventLogEntryType entryType;
            switch (level)
            {
                case Level.Info:
                    entryType = EventLogEntryType.Information; break;
                case Level.Warning:
                    entryType = EventLogEntryType.Warning; break;
                case Level.Error:
                    entryType = EventLogEntryType.Error; break;
                default: return false;
            }
            return WriteEntry(message,
                FormatCaller(callerName, callerFilePath, callerLineNumber),
                entryType, eventId);
        }

        /// <summary>
        /// Logs an informational message
        /// </summary>
        /// <param name="eventId">Unique Id for event log</param>
        /// <param name="message">Message to log</param>
        /// <returns><see langword="true"/>if the message was skipped due to log level, or was consumed by the event log successfully.  <see langword="false"/>if the message should have been added to the event log but could not be added.</returns>
        internal static bool Info(int eventId, string message,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (Level.Info < LogLevel) return true;
            return WriteEntry(message,
                FormatCaller(callerName, callerFilePath, callerLineNumber), EventLogEntryType.Information, eventId);
        }

        /// <summary>
        /// Logs a warning message, which indicates a non-fatal error or a potential contributing cause of a possible later error.
        /// </summary>
        /// <param name="eventId">Unique Id for event log</param>
        /// <param name="message">Message to log</param>
        /// <returns><see langword="true"/>if the message was skipped due to log level, or was consumed by the event log successfully.  <see langword="false"/>if the message should have been added to the event log but could not be added.</returns>
        internal static bool Warn(int eventId, string message,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (Level.Warning < LogLevel) return true;
            return WriteEntry(message,
                FormatCaller(callerName, callerFilePath, callerLineNumber), EventLogEntryType.Warning, eventId);
        }

        /// <summary>
        /// Logs an error message, which stops processing
        /// </summary>
        /// <param name="eventId">Unique Id for event log</param>
        /// <param name="message">Message to log</param>
        /// <returns><see langword="true"/>if the message was skipped due to log level, or was consumed by the event log successfully.  <see langword="false"/>if the message should have been added to the event log but could not be added.</returns>
        internal static bool Err(int eventId, string message,
            [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (Level.Error < LogLevel) return true;
            return WriteEntry(message,
               FormatCaller(callerName, callerFilePath, callerLineNumber), EventLogEntryType.Error, eventId);
        }

        private static string FormatCaller(string callerName, string callerFilePath, int callerLineNumber)
        {
            return $"{callerName}() at {callerFilePath}:{callerLineNumber}";
        }

        private static bool WriteEntry(string message, string caller, EventLogEntryType entryType, int eventId)
        {
            if (!CheckForEventOverage()) return false;
            try
            {
                eventLog.WriteEntry(FormatMessage(message, caller, entryType), 
                    entryType, eventId);
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    if (!CheckForMessageBoxTooFast()) return false;
                    System.Windows.Forms.MessageBox.Show(FormatMessage(message, caller, entryType, ex), $"{ApplicationName} - Event Log Write Failed", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.MessageBoxDefaultButton.Button1);


                }
                catch //ignored
                { }
                return false;
            }
        }

        private static string FormatMessage(string message, string caller, EventLogEntryType entryType, Exception eventLogWriteException = null)
        {
            var sb = new StringBuilder();

            if (eventLogWriteException != null)
            {
                sb.AppendLine("Event log write failed - event converted to message box.  Press Control+C to copy this text to the clipboard for posting.  Event log write exception details are at the end.");
            }

            if (suppressedEventLogs > 0)
            {
                sb.AppendLine($"Note: To prevent event log spam, {suppressedEventLogs} "
                    + $"event log message{(suppressedEventLogs == 1 ? "" : "s")}"
                    + " were suppressed.");
                suppressedEventLogs = 0;
            }

            if (suppressedMessageBoxes > 0)
            {
                sb.AppendLine($"Note: To prevent pop-up message box spam, {suppressedMessageBoxes} "
                    + $"event-log-failure message box{(suppressedMessageBoxes == 1 ? "" : "es")}"
                    + " were suppressed.");
                suppressedEventLogs = 0;
            }

            switch (entryType)
            {
                case EventLogEntryType.Error:
                    sb.Append($"{caller}: Error: ");
                    break;
                case EventLogEntryType.Warning:
                    sb.Append($"{caller}: Warning: ");
                    break;
            }

            sb.AppendLine(message);

            if (entryType == EventLogEntryType.Warning)
            {
                //errors should already contain exception information
                var stackLines = Environment.StackTrace.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in stackLines)
                {
                    var trimLine = line.TrimStart();
                    if (trimLine.StartsWith("at System.Environment.GetStackTrace(")
                        || trimLine.StartsWith("at System.Environment.get_StackTrace(")
                        || trimLine.StartsWith($"at {nameof(TTImageProcessor)}.{nameof(Logger)}."))
                        continue;
                    sb.AppendLine(line);
                    
                }
            }

            if (eventLogWriteException != null)
            {
                sb.AppendLine(new string('-', 76));
                sb.AppendLine("Exception occurred while writing the above message to the system event log:");
                sb.Append(eventLogWriteException.ToString());
            }

            return sb.ToString();
        }

        private static bool CheckForEventOverage()
        {
            var nextIdx = (lastEventLogIndex + 1) % MaxEventLogs;
            if (DateTime.Now.Subtract(lastEventLogRingBuffer[nextIdx]).CompareTo(eventLogDelay) > -1)
            {
                lastEventLogRingBuffer[nextIdx] = DateTime.Now;
                lastEventLogIndex = nextIdx;
                return true;
            }

            suppressedEventLogs++;
            return false;
        }

        private static bool CheckForMessageBoxTooFast()
        {
            if (DateTime.Now.Subtract(lastMessageBox).CompareTo(messageBoxDelay) > -1)
            {
                lastMessageBox = DateTime.Now;
                return true;
            }

            suppressedMessageBoxes++;
            return false;
        }
    }
}
