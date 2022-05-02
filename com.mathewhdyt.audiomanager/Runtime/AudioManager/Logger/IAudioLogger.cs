using UnityEngine;

namespace AudioManager.Logger {
    public interface IAudioLogger {
        /// <summary>
        /// Simply logs the given message with the given type if it surpasses the previously entered minmum log level.
        /// </summary>
        /// <param name="message">Message we want to print in the console.</param>
        /// <param name="level">Level the current message should be printed in the console at.</param>
        /// <param name="type">Type of the message. (Error, Warning, etc.)</param>
        /// <param name="context">Optional object to which the message applies.</param>
        public void Log(object message, LoggingLevel level, LoggingType type, Object context = null);

        /// <summary>
        /// Logs the given arguments with the given format and with the given type if it surpasses the previously entered minmum log level.
        /// </summary>
        /// <param name="format">Format we want to print the messaged in.</param>
        /// <param name="level">Level the current message should be printed in the console at.</param>
        /// <param name="type">Type of the message. (Error, Warning, etc.)</param>
        /// <param name="context">Optional object to which the message applies.</param>   
        /// <param name="args">Formatted arguements that should be printed.</param>
        public void LogFormat(string format, LoggingLevel level, LoggingType type, Object context = null, params object[] args);

        /// <summary>
        /// Logs the given expection if it surpasses the previously entered minmum log level.
        /// </summary>
        /// <param name="exception">Exception we want to print into the console.</param>
        /// <param name="level">Level the current message should be printed in the console at.</param>
        /// <param name="context">Optional object to which the message applies.</param>
        public void LogException(System.Exception exception, LoggingLevel level, Object context = null);

        /// <summary>
        /// Logs the given assertion with the given message if it surpasses the previously entered minmum log level.
        /// </summary>
        /// <param name="condition">Condition we want to print in the console.</param>
        /// <param name="message">Message we want to print in the console.</param>
        /// <param name="level">Level the current message should be printed in the console at.</param>
        /// <param name="context">Optional object to which the message applies.</param>
        public void LogAssert(bool condition, string message, LoggingLevel level, Object context = null);

        /// <summary>
        /// Logs the given assertion arguments with the given format and with the given type if it surpasses the previously entered minmum log level.
        /// </summary>
        /// <param name="condition">Condition we want to print in the console.</param>
        /// <param name="format">Format we want to print the messaged in.</param>
        /// <param name="level">Level the current message should be printed in the console at.</param>
        /// <param name="context">Optional object to which the message applies.</param>
        /// <param name="args">Formatted arguements that should be printed.</param>
        public void LogAssertFormat(bool condition, string format, LoggingLevel level, Object context = null, params object[] args);
    }
}
