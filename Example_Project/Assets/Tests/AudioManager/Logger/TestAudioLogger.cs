using AudioManager.Logger;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public sealed class TestAudioLogger {
    string m_text;
    Object m_object;
    AudioLogger m_audioLogger;

    [SetUp]
    public void TestSetUp() {
        m_text = string.Empty;
        m_object = new GameObject();
        m_audioLogger = new AudioLogger(LoggingLevel.STOPWATCH);
    }

    [Test]
    public void TestLog() {
        /// ---------------------------------------------
        /// Valid case (LoggingType.NORMAL)
        /// ---------------------------------------------
        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.STOPWATCH, LoggingType.NORMAL, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.STOPWATCH, LoggingType.NORMAL, null);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.STOPWATCH, LoggingType.NORMAL, m_object);
        LogAssert.Expect(LogType.Log, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.HIGH, LoggingType.NORMAL, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.HIGH, LoggingType.NORMAL, null);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.HIGH, LoggingType.NORMAL, m_object);
        LogAssert.Expect(LogType.Log, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, null);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, m_object);
        LogAssert.Expect(LogType.Log, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.LOW, LoggingType.NORMAL, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.LOW, LoggingType.NORMAL, null);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.LOW, LoggingType.NORMAL, m_object);
        LogAssert.Expect(LogType.Log, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.NONE, LoggingType.NORMAL, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.NONE, LoggingType.NORMAL, null);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.NONE, LoggingType.NORMAL, m_object);
        LogAssert.Expect(LogType.Log, m_text);

        // Context is null.
        m_audioLogger.Log(m_text, (LoggingLevel)(-1), LoggingType.NORMAL, null);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, (LoggingLevel)(-1), LoggingType.NORMAL, m_object);
        LogAssert.Expect(LogType.Log, m_text);

        /// ---------------------------------------------
        /// Valid case (LoggingType.WARNING)
        /// ---------------------------------------------
        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.STOPWATCH, LoggingType.WARNING, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.STOPWATCH, LoggingType.WARNING, null);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.STOPWATCH, LoggingType.WARNING, m_object);
        LogAssert.Expect(LogType.Warning, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.HIGH, LoggingType.WARNING, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.HIGH, LoggingType.WARNING, null);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.HIGH, LoggingType.WARNING, m_object);
        LogAssert.Expect(LogType.Warning, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.INTERMEDIATE, LoggingType.WARNING, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.INTERMEDIATE, LoggingType.WARNING, null);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.INTERMEDIATE, LoggingType.WARNING, m_object);
        LogAssert.Expect(LogType.Warning, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.LOW, LoggingType.WARNING, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.LOW, LoggingType.WARNING, null);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.LOW, LoggingType.WARNING, m_object);
        LogAssert.Expect(LogType.Warning, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.NONE, LoggingType.WARNING, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.NONE, LoggingType.WARNING, null);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.NONE, LoggingType.WARNING, m_object);
        LogAssert.Expect(LogType.Warning, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, (LoggingLevel)(-1), LoggingType.WARNING, null);
        // Context is null.
        m_audioLogger.Log(m_text, (LoggingLevel)(-1), LoggingType.WARNING, null);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, (LoggingLevel)(-1), LoggingType.WARNING, m_object);
        LogAssert.Expect(LogType.Warning, m_text);

        /// ---------------------------------------------
        /// Valid case (LoggingType.ERROR)
        /// ---------------------------------------------
        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.STOPWATCH, LoggingType.ERROR, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.STOPWATCH, LoggingType.ERROR, null);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.STOPWATCH, LoggingType.ERROR, m_object);
        LogAssert.Expect(LogType.Error, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.HIGH, LoggingType.ERROR, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.HIGH, LoggingType.ERROR, null);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.HIGH, LoggingType.ERROR, m_object);
        LogAssert.Expect(LogType.Error, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.INTERMEDIATE, LoggingType.ERROR, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.INTERMEDIATE, LoggingType.ERROR, null);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.INTERMEDIATE, LoggingType.ERROR, m_object);
        LogAssert.Expect(LogType.Error, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.LOW, LoggingType.ERROR, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.LOW, LoggingType.ERROR, null);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.LOW, LoggingType.ERROR, m_object);
        LogAssert.Expect(LogType.Error, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.NONE, LoggingType.ERROR, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.NONE, LoggingType.ERROR, null);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.NONE, LoggingType.ERROR, m_object);
        LogAssert.Expect(LogType.Error, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, (LoggingLevel)(-1), LoggingType.ERROR, null);
        // Context is null.
        m_audioLogger.Log(m_text, (LoggingLevel)(-1), LoggingType.ERROR, null);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, (LoggingLevel)(-1), LoggingType.ERROR, m_object);
        LogAssert.Expect(LogType.Error, m_text);

        /// ---------------------------------------------
        /// Valid case (LoggingType.ASSERTION)
        /// ---------------------------------------------
        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.STOPWATCH, LoggingType.ASSERTION, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.STOPWATCH, LoggingType.ASSERTION, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.STOPWATCH, LoggingType.ASSERTION, m_object);
        LogAssert.Expect(LogType.Assert, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.HIGH, LoggingType.ASSERTION, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.HIGH, LoggingType.ASSERTION, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.HIGH, LoggingType.ASSERTION, m_object);
        LogAssert.Expect(LogType.Assert, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.INTERMEDIATE, LoggingType.ASSERTION, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.INTERMEDIATE, LoggingType.ASSERTION, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.INTERMEDIATE, LoggingType.ASSERTION, m_object);
        LogAssert.Expect(LogType.Assert, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.LOW, LoggingType.ASSERTION, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.LOW, LoggingType.ASSERTION, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.LOW, LoggingType.ASSERTION, m_object);
        LogAssert.Expect(LogType.Assert, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.NONE, LoggingType.ASSERTION, null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.NONE, LoggingType.ASSERTION, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.NONE, LoggingType.ASSERTION, m_object);
        LogAssert.Expect(LogType.Assert, m_text);

        // Context is null, message is null.
        m_audioLogger.Log(null, (LoggingLevel)(-1), LoggingType.ASSERTION, null);
        // Context is null.
        m_audioLogger.Log(m_text, (LoggingLevel)(-1), LoggingType.ASSERTION, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.Log(m_text, (LoggingLevel)(-1), LoggingType.ASSERTION, m_object);
        LogAssert.Expect(LogType.Assert, m_text);

        /// ---------------------------------------------
        /// Invalid case (LoggingType)(-1)
        /// ---------------------------------------------
        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.STOPWATCH, (LoggingType)(-1), null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.STOPWATCH, (LoggingType)(-1), null);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.STOPWATCH, (LoggingType)(-1), m_object);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.HIGH, (LoggingType)(-1), null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.HIGH, (LoggingType)(-1), null);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.HIGH, (LoggingType)(-1), m_object);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.INTERMEDIATE, (LoggingType)(-1), null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.INTERMEDIATE, (LoggingType)(-1), null);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.INTERMEDIATE, (LoggingType)(-1), m_object);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.LOW, (LoggingType)(-1), null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.LOW, (LoggingType)(-1), null);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.LOW, (LoggingType)(-1), m_object);

        // Context is null, message is null.
        m_audioLogger.Log(null, LoggingLevel.NONE, (LoggingType)(-1), null);
        // Context is null.
        m_audioLogger.Log(m_text, LoggingLevel.NONE, (LoggingType)(-1), null);
        // Context is not null.
        m_audioLogger.Log(m_text, LoggingLevel.NONE, (LoggingType)(-1), m_object);

        // Context is null, message is null.
        m_audioLogger.Log(null, (LoggingLevel)(-1), (LoggingType)(-1), null);
        // Context is null.
        m_audioLogger.Log(m_text, (LoggingLevel)(-1), (LoggingType)(-1), null);
        // Context is not null.
        m_audioLogger.Log(m_text, (LoggingLevel)(-1), (LoggingType)(-1), m_object);
    }

    [Test]
    public void TestLogFormat() {
        /// ---------------------------------------------
        /// Valid case (LoggingType.NORMAL)
        /// ---------------------------------------------
        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.NORMAL, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.NORMAL, null, m_text);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.NORMAL, m_object, m_text);
        LogAssert.Expect(LogType.Log, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.NORMAL, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.NORMAL, null, m_text);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.NORMAL, m_object, m_text);
        LogAssert.Expect(LogType.Log, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, null, m_text);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.NORMAL, m_object, m_text);
        LogAssert.Expect(LogType.Log, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.NORMAL, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.NORMAL, null, m_text);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.NORMAL, m_object, m_text);
        LogAssert.Expect(LogType.Log, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.NORMAL, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.NORMAL, null, m_text);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.NORMAL, m_object, m_text);
        LogAssert.Expect(LogType.Log, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.NORMAL, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.NORMAL, null, m_text);
        LogAssert.Expect(LogType.Log, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.NORMAL, m_object, m_text);
        LogAssert.Expect(LogType.Log, m_text);

        /// ---------------------------------------------
        /// Valid case (LoggingType.WARNING)
        /// ---------------------------------------------
        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.WARNING, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.WARNING, null, m_text);
        LogAssert.Expect(LogType.Warning, m_text);
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.WARNING, m_object, m_text);
        LogAssert.Expect(LogType.Warning, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.WARNING, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.WARNING, null, m_text);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.WARNING, m_object, m_text);
        LogAssert.Expect(LogType.Warning, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.WARNING, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.WARNING, null, m_text);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.WARNING, m_object, m_text);
        LogAssert.Expect(LogType.Warning, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.WARNING, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.WARNING, null, m_text);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.WARNING, m_object, m_text);
        LogAssert.Expect(LogType.Warning, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.WARNING, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.WARNING, null, m_text);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.WARNING, m_object, m_text);
        LogAssert.Expect(LogType.Warning, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.WARNING, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.WARNING, null, m_text);
        LogAssert.Expect(LogType.Warning, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.WARNING, m_object, m_text);
        LogAssert.Expect(LogType.Warning, m_text);

        /// ---------------------------------------------
        /// Valid case (LoggingType.ERROR)
        /// ---------------------------------------------
        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.ERROR, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.ERROR, null, m_text);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.ERROR, m_object, m_text);
        LogAssert.Expect(LogType.Error, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.ERROR, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.ERROR, null, m_text);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.ERROR, m_object, m_text);
        LogAssert.Expect(LogType.Error, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.ERROR, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.ERROR, null, m_text);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.ERROR, m_object, m_text);
        LogAssert.Expect(LogType.Error, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.ERROR, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.ERROR, null, m_text);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.ERROR, m_object, m_text);
        LogAssert.Expect(LogType.Error, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.ERROR, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.ERROR, null, m_text);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.ERROR, m_object, m_text);
        LogAssert.Expect(LogType.Error, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.ERROR, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.ERROR, null, m_text);
        LogAssert.Expect(LogType.Error, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.ERROR, m_object, m_text);
        LogAssert.Expect(LogType.Error, m_text);

        /// ---------------------------------------------
        /// Valid case (LoggingType.ASSERTION)
        /// ---------------------------------------------
        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.ASSERTION, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.ASSERTION, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, LoggingType.ASSERTION, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.ASSERTION, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.ASSERTION, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, LoggingType.ASSERTION, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.ASSERTION, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.ASSERTION, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, LoggingType.ASSERTION, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.ASSERTION, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.ASSERTION, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, LoggingType.ASSERTION, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.ASSERTION, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.ASSERTION, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, LoggingType.ASSERTION, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.ASSERTION, null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.ASSERTION, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), LoggingType.ASSERTION, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);

        /// ---------------------------------------------
        /// Invalid case (LoggingType)(-1)
        /// ---------------------------------------------
        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, (LoggingType)(-1), null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, (LoggingType)(-1), null, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.STOPWATCH, (LoggingType)(-1), m_object, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, (LoggingType)(-1), null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, (LoggingType)(-1), null, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.HIGH, (LoggingType)(-1), m_object, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, (LoggingType)(-1), null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, (LoggingType)(-1), null, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.INTERMEDIATE, (LoggingType)(-1), m_object, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, (LoggingType)(-1), null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, (LoggingType)(-1), null, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.LOW, (LoggingType)(-1), m_object, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, (LoggingType)(-1), null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, (LoggingType)(-1), null, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, LoggingLevel.NONE, (LoggingType)(-1), m_object, m_text);

        // Context is null, arguments are null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), (LoggingType)(-1), null, null);
        // Context is null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), (LoggingType)(-1), null, m_text);
        // Context is not null.
        m_audioLogger.LogFormat(m_text, (LoggingLevel)(-1), (LoggingType)(-1), m_object, m_text);
    }

    [Test]
    public void TestLogException() {
        var exception = new System.NullReferenceException();
        const string message = "NullReferenceException: Object reference not set to an instance of an object.";

        // Context is null, exception is null.
        m_audioLogger.LogException(null, LoggingLevel.STOPWATCH, null);
        // Context is null.
        m_audioLogger.LogException(exception, LoggingLevel.STOPWATCH, null);
        LogAssert.Expect(LogType.Exception, message);
        // Context is not null.
        m_audioLogger.LogException(exception, LoggingLevel.STOPWATCH, m_object);
        LogAssert.Expect(LogType.Exception, message);

        // Context is null, exception is null.
        m_audioLogger.LogException(null, LoggingLevel.HIGH, null);
        // Context is null.
        m_audioLogger.LogException(exception, LoggingLevel.HIGH, null);
        LogAssert.Expect(LogType.Exception, message);
        // Context is not null.
        m_audioLogger.LogException(exception, LoggingLevel.HIGH, m_object);
        LogAssert.Expect(LogType.Exception, message);

        // Context is null, exception is null.
        m_audioLogger.LogException(null, LoggingLevel.INTERMEDIATE, null);
        // Context is null.
        m_audioLogger.LogException(exception, LoggingLevel.INTERMEDIATE, null);
        LogAssert.Expect(LogType.Exception, message);
        // Context is not null.
        m_audioLogger.LogException(exception, LoggingLevel.INTERMEDIATE, m_object);
        LogAssert.Expect(LogType.Exception, message);

        // Context is null, exception is null.
        m_audioLogger.LogException(null, LoggingLevel.LOW, null);
        // Context is null.
        m_audioLogger.LogException(exception, LoggingLevel.LOW, null);
        LogAssert.Expect(LogType.Exception, message);
        // Context is not null.
        m_audioLogger.LogException(exception, LoggingLevel.LOW, m_object);
        LogAssert.Expect(LogType.Exception, message);

        // Context is null, exception is null.
        m_audioLogger.LogException(null, LoggingLevel.NONE, null);
        // Context is null.
        m_audioLogger.LogException(exception, LoggingLevel.NONE, null);
        LogAssert.Expect(LogType.Exception, message);
        // Context is not null.
        m_audioLogger.LogException(exception, LoggingLevel.NONE, m_object);
        LogAssert.Expect(LogType.Exception, message);

        // Context is null, exception is null.
        m_audioLogger.LogException(null, (LoggingLevel)(-1), null);
        // Context is null.
        m_audioLogger.LogException(exception, (LoggingLevel)(-1), null);
        LogAssert.Expect(LogType.Exception, message);
        // Context is not null.
        m_audioLogger.LogException(exception, (LoggingLevel)(-1), m_object);
        LogAssert.Expect(LogType.Exception, message);
    }

    [Test]
    public void TestLogAssert() {
        // Context is null, message is null, assert is true.
        m_audioLogger.LogAssert(true, null, LoggingLevel.STOPWATCH, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssert(true, m_text, LoggingLevel.STOPWATCH, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssert(true, m_text, LoggingLevel.STOPWATCH, m_object);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, message is null, assert is false.
        m_audioLogger.LogAssert(false, null, LoggingLevel.STOPWATCH, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssert(false, m_text, LoggingLevel.STOPWATCH, null);
        // Assert is false.
        m_audioLogger.LogAssert(false, m_text, LoggingLevel.STOPWATCH, m_object);

        // Context is null, message is null, assert is true.
        m_audioLogger.LogAssert(true, null, LoggingLevel.HIGH, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssert(true, m_text, LoggingLevel.HIGH, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssert(true, m_text, LoggingLevel.HIGH, m_object);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, message is null, assert is false.
        m_audioLogger.LogAssert(false, null, LoggingLevel.STOPWATCH, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssert(false, m_text, LoggingLevel.HIGH, null);
        // Assert is false.
        m_audioLogger.LogAssert(false, m_text, LoggingLevel.HIGH, m_object);

        // Context is null, message is null, assert is true.
        m_audioLogger.LogAssert(true, null, LoggingLevel.INTERMEDIATE, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssert(true, m_text, LoggingLevel.INTERMEDIATE, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssert(true, m_text, LoggingLevel.INTERMEDIATE, m_object);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, message is null, assert is false.
        m_audioLogger.LogAssert(false, null, LoggingLevel.INTERMEDIATE, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssert(false, m_text, LoggingLevel.INTERMEDIATE, null);
        // Assert is false.
        m_audioLogger.LogAssert(false, m_text, LoggingLevel.INTERMEDIATE, m_object);

        // Context is null, message is null, assert is true.
        m_audioLogger.LogAssert(true, null, LoggingLevel.LOW, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssert(true, m_text, LoggingLevel.LOW, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssert(true, m_text, LoggingLevel.LOW, m_object);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, message is null, assert is true.
        m_audioLogger.LogAssert(false, null, LoggingLevel.LOW, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssert(false, m_text, LoggingLevel.LOW, null);
        // Assert is false.
        m_audioLogger.LogAssert(false, m_text, LoggingLevel.LOW, m_object);

        // Context is null, message is null, assert is true.
        m_audioLogger.LogAssert(true, null, LoggingLevel.NONE, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssert(true, m_text, LoggingLevel.NONE, null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssert(true, m_text, LoggingLevel.NONE, m_object);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, message is null, assert is false.
        m_audioLogger.LogAssert(false, null, LoggingLevel.NONE, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssert(false, m_text, LoggingLevel.NONE, null);
        // Assert is false.
        m_audioLogger.LogAssert(false, m_text, LoggingLevel.NONE, m_object);

        // Context is null, message is null, assert is true.
        m_audioLogger.LogAssert(true, null, (LoggingLevel)(-1), null);
        // Context is null, assert is true.
        m_audioLogger.LogAssert(true, m_text, (LoggingLevel)(-1), null);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssert(true, m_text, (LoggingLevel)(-1), m_object);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, message is null, assert is true.
        m_audioLogger.LogAssert(false, null, (LoggingLevel)(-1), null);
        // Context is null, assert is false.
        m_audioLogger.LogAssert(false, m_text, (LoggingLevel)(-1), null);
        // Assert is false.
        m_audioLogger.LogAssert(false, m_text, (LoggingLevel)(-1), m_object);
    }

    [Test]
    public void TestLogAssertFormat() {
        // Context is null, arguments are null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.STOPWATCH, null, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.STOPWATCH, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.STOPWATCH, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, arguments are null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.STOPWATCH, null, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.STOPWATCH, null, m_text);
        // Assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.STOPWATCH, m_object, m_text);

        // Context is null, arguments are null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.HIGH, null, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.HIGH, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.HIGH, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, arguments are null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.HIGH, null, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.HIGH, null, m_text);
        // Assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.HIGH, m_object, m_text);

        // Context is null, arguments are null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.INTERMEDIATE, null, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.INTERMEDIATE, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.INTERMEDIATE, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, arguments are null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.INTERMEDIATE, null, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.INTERMEDIATE, null, m_text);
        // Assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.INTERMEDIATE, m_object, m_text);

        // Context is null, arguments are null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.LOW, null, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.LOW, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.LOW, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, arguments are null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.LOW, null, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.LOW, null, m_text);
        // Assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.LOW, m_object, m_text);

        // Context is null, arguments are null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.NONE, null, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.NONE, null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, LoggingLevel.NONE, m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, arguments are null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.NONE, null, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.NONE, null, m_text);
        // Assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, LoggingLevel.NONE, m_object, m_text);

        // Context is null, arguments are null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, (LoggingLevel)(-1), null, null);
        // Context is null, assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, (LoggingLevel)(-1), null, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Assert is true.
        m_audioLogger.LogAssertFormat(true, m_text, (LoggingLevel)(-1), m_object, m_text);
        LogAssert.Expect(LogType.Assert, m_text);
        // Context is null, arguments are null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, (LoggingLevel)(-1), null, null);
        // Context is null, assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, (LoggingLevel)(-1), null, m_text);
        // Assert is false.
        m_audioLogger.LogAssertFormat(false, m_text, (LoggingLevel)(-1), m_object, m_text);
    }
}
