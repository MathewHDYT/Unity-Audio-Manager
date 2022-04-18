using AudioManager.Logger;
using UnityEngine;

public class DummyAudioLogger : IAudioLogger {
    /// <summary>
    /// Empty Constructor.
    /// </summary>
    public DummyAudioLogger() {
        // Nothing to do.
    }

    private bool m_logged;

    public bool Logged { 
        get { 
            return m_logged;
        }
        set {
            m_logged = value;
        }
    }

    private Object m_context;

    public Object Context {
        get {
            return m_context;
        }
        set {
            m_context = value;
        }
    }

    public void Log(object message, LoggingLevel level, LoggingType type, Object context) {
        m_logged = true;
        m_context = context;
    }

    public void LogFormat(string format, LoggingLevel level, LoggingType type, Object context, params object[] args) {
        m_logged = true;
        m_context = context;
    }

    public void LogException(System.Exception exception, LoggingLevel level, Object context) {
        m_logged = true;
        m_context = context;
    }

    public void LogAssert(bool condition, string message, LoggingLevel level, Object context) {
        m_logged = true;
        m_context = context;
    }

    public void LogAssertFormat(bool condition, string format, LoggingLevel level, Object context, params object[] args) {
        m_logged = true;
        m_context = context;
    }
}