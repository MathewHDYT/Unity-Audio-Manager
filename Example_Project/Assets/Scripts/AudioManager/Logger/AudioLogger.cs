using UnityEngine;

namespace AudioManager.Logger {
    public class AudioLogger : IAudioLogger {
        // Holds the minmum log level that must be passed to the Log method,
        // so that the message actually gets printed to the console.
        private readonly LoggingLevel m_logLevel = LoggingLevel.NONE;

        /// <summary>
        /// Constructor for the Logger, pass the minimal log level needed to be printed into the console.
        /// </summary>
        /// <param name="minLogLevel">Minmum log level that must be passed to the Log methods so that the message actually get's printed to the console.</param>
        public AudioLogger(LoggingLevel minLogLevel) {
            m_logLevel = minLogLevel;
        }

        public void Log(object message, LoggingLevel level, LoggingType type, Object context) {
            if (!CanLog(message, level)) {
                return;
            }

            switch (type) {
                case LoggingType.NORMAL:
                    Debug.Log(message, context);
                    break;
                case LoggingType.WARNING:
                    Debug.LogWarning(message, context);
                    break;
                case LoggingType.ERROR:
                    Debug.LogError(message, context);
                    break;
                case LoggingType.ASSERTION:
                    Debug.LogAssertion(message, context);
                    break;
                default:
                    // Unexpected LoggingType argument.
                    break;
            }
        }

        public void LogFormat(string format, LoggingLevel level, LoggingType type, Object context, params object[] args) {
            if (!CanLog(level, args)) {
                return;
            }

            switch (type) {
                case LoggingType.NORMAL:
                    Debug.LogFormat(LogType.Log, LogOption.None, context, format, args);
                    break;
                case LoggingType.WARNING:
                    Debug.LogFormat(LogType.Warning, LogOption.None, context, format, args);
                    break;
                case LoggingType.ERROR:
                    Debug.LogFormat(LogType.Error, LogOption.None, context, format, args);
                    break;
                case LoggingType.ASSERTION:
                    Debug.LogFormat(LogType.Assert, LogOption.None, context, format, args);
                    break;
                default:
                    // Unexpected LoggingType argument.
                    break;
            }
        }

        public void LogException(System.Exception exception, LoggingLevel level, Object context) {
            if (!CanLog(exception, level)) {
                return;
            }

            Debug.LogException(exception, context);
        }

        public void LogAssert(bool condition, string message, LoggingLevel level, Object context) {
            if (!CanLog(message, level)) {
                return;
            }

            Debug.Assert(condition, message, context);
        }

        public void LogAssertFormat(bool condition, string format, LoggingLevel level, Object context, params object[] args) {
            if (!CanLog(level, args)) {
                return;
            }

            Debug.AssertFormat(condition, format, context, args);
        }

        private bool CanLog(LoggingLevel level, params object[] args) {
            return args is not null && level <= m_logLevel;
        }

        private bool CanLog(object message, LoggingLevel level) {
            return message is not null && level <= m_logLevel;
        }

        private bool CanLog(System.Exception exception, LoggingLevel level) {
            return exception is not null && level <= m_logLevel;
        }
    }
}
