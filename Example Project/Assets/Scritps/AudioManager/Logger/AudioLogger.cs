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
                    if (!ContextIsValid(context)) {
                        Debug.Log(message);
                        break;
                    }
                    Debug.Log(message, context);
                    break;
                case LoggingType.WARNING:
                    if (!ContextIsValid(context)) {
                        Debug.LogWarning(message);
                        break;
                    }
                    Debug.LogWarning(message, context);
                    break;
                case LoggingType.ERROR:
                    if (!ContextIsValid(context)) {
                        Debug.LogError(message);
                        break;
                    }
                    Debug.LogError(message, context);
                    break;
                case LoggingType.ASSERTION:
                    if (!ContextIsValid(context)) {
                        Debug.LogAssertion(message);
                        break;
                    }
                    Debug.LogAssertion(message, context);
                    break;
                default:
                    // Unexpected LoggingType argument.
                    break;
            }
        }

        public void LogFormat(string format, LoggingLevel level, LoggingType type, Object context, params object[] args) {
            if (!CanLog(format, level)) {
                return;
            }

            switch (type) {
                case LoggingType.NORMAL:
                    if (!ContextIsValid(context)) {
                        Debug.LogFormat(format, args);
                        break;
                    }
                    Debug.LogFormat(LogType.Log, LogOption.None, context, format, args);
                    break;
                case LoggingType.WARNING:
                    if (!ContextIsValid(context)) {
                        Debug.LogWarningFormat(format, args);
                        break;
                    }
                    Debug.LogFormat(LogType.Warning, LogOption.None, context, format, args);
                    break;
                case LoggingType.ERROR:
                    if (!ContextIsValid(context)) {
                        Debug.LogErrorFormat(format, args);
                        break;
                    }
                    Debug.LogErrorFormat(format, args, context);
                    Debug.LogFormat(LogType.Error, LogOption.None, context, format, args);
                    break;
                case LoggingType.ASSERTION:
                    if (!ContextIsValid(context)) {
                        Debug.LogAssertionFormat(format, args);
                        break;
                    }
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

            if (!ContextIsValid(context)) {
                Debug.LogException(exception);
                return;
            }

            Debug.LogException(exception, context);
        }

        public void LogAssert(bool condition, string message, LoggingLevel level, Object context) {
            if (!CanLog(message, level)) {
                return;
            }

            if (!ContextIsValid(context)) {
                Debug.Assert(condition, message);
                return;
            }

            Debug.Assert(condition, message, context);
        }

        public void LogAssertFormat(bool condition, string format, LoggingLevel level, Object context, params object[] args) {
            if (!CanLog(format, level)) {
                return;
            }

            if (!ContextIsValid(context)) {
                Debug.AssertFormat(condition, format, args);
                return;
            }

            Debug.AssertFormat(condition, format, context, args);
        }

        private bool CanLog(string message, LoggingLevel level) {
            return !string.IsNullOrEmpty(message) && level <= m_logLevel;
        }

        private bool CanLog(object message, LoggingLevel level) {
            return message is object && level <= m_logLevel;
        }

        private bool CanLog(System.Exception exception, LoggingLevel level) {
            return exception is object && !string.IsNullOrEmpty(exception.Message) && level <= m_logLevel;
        }

        private bool ContextIsValid(Object context) {
            return context is object;
        }
    }
}
