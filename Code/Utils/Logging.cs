using System;
using System.Text;
using UnityEngine;

namespace DistrictStylesPlus.Code.Utils
{
    /// <summary>
    /// Logging utility class
    /// Inspiration taken from Algernon-A
    /// </summary>
    internal static class Logging
    {
        // TODO: false by default in the future
        internal static bool DebugLogging = true;
        
        private static readonly string LogModName = DistrictStylesPlusMod.modName;
        
        /// <summary>
        /// Log an Error message - basically message explaining why mod is not working.
        /// </summary>
        /// <param name="messages">messages to log</param>
        internal static void ErrorLog(params string[] messages) => WriteMessage("ERROR: ", messages);
        
        /// <summary>
        /// Log an Info message - just informative.
        /// </summary>
        /// <param name="messages">messages to log</param>
        internal static void InfoLog(params string[] messages) => WriteMessage("INFO: ", messages);

        /// <summary>
        /// Log a debug message - only for testing and if debug logging is enabled.
        /// </summary>
        /// <param name="messages">messages to log</param>
        internal static void DebugLog(params string[] messages)
        {
            if (DebugLogging) WriteMessage("DEBUG: ", messages);
        }
        
        /// <summary>
        /// Prints an exception message to the Unity output log.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="messages">Message to log (individual strings will be concatenated)</param>
        internal static void LogException(Exception exception, params string[] messages)
        {
            // Use StringBuilder for efficiency since we're doing a lot of manipulation here.
            // Start with mod name (to easily identify relevant messages), followed by colon to indicate start of actual message.
            var message = new StringBuilder(LogModName);
            message.Append(": ");

            // Add each message parameter.
            foreach (var logMessage in messages)
            {
                message.Append(logMessage);
            }

            // Finish with a new line and the exception information.
            message.AppendLine();
            message.AppendLine("Exception: ");
            message.AppendLine(exception.Message);
            message.AppendLine(exception.Source);
            message.AppendLine(exception.StackTrace);

            // Log inner exception as well, if there is one.
            if (exception.InnerException != null)
            {
                message.AppendLine("Inner exception:");
                message.AppendLine(exception.InnerException.Message);
                message.AppendLine(exception.InnerException.Source);
                message.AppendLine(exception.InnerException.StackTrace);
            }

            // Write to log.
            Debug.Log(message);
        }

        private static void WriteMessage(string prefix, params string[] messages)
        {
            // Use StringBuilder for efficiency since we are doing a lot of manipulation here.
            // Start with mod name (to easily identify relevant messages), followed by colon to indicate start of actual message.
            var message = new StringBuilder(LogModName);
            message.Append(": ");

            // Append prefix.
            message.Append(prefix);

            // Add each message parameter.
            foreach (var logMessage in messages)
            {
                message.Append(logMessage);
            }

            // Terminating period to confirm end of message.
            message.Append(".");

            Debug.Log(message);
        }
    }
}