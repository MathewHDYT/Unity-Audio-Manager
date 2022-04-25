using AudioManager.Core;
using NUnit.Framework;

public class TestValueDataError {
    float m_value;
    AudioError m_audioError;

    [SetUp]
    public void TestSetUp() {
        m_value = float.NaN;
        m_audioError = AudioError.OK;
    }

    [Test]
    public void TestPassByConstructor() {
        ValueDataError<float> valueDataError = new ValueDataError<float>(m_value, m_audioError);
        Assert.AreEqual(m_value, valueDataError.Value);
        Assert.AreEqual(m_audioError, valueDataError.Error);
    }

    [Test]
    public void TestByProperties() {
        ValueDataError<float> valueDataError = new ValueDataError<float>();
        valueDataError.Value = m_value;
        valueDataError.Error = m_audioError;
        Assert.AreEqual(m_value, valueDataError.Value);
        Assert.AreEqual(m_audioError, valueDataError.Error);
    }
}
