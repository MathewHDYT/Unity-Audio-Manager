using AudioManager.Core;
using NUnit.Framework;

public class TestValueDataError {
    [Test]
    public void TestConstructor() {
        const float expectedValue = float.NaN;
        const AudioError expectedError = AudioError.OK;

        // Passing value in constructor.
        ValueDataError<float> actual = new ValueDataError<float>(expectedValue, expectedError);
        Assert.AreEqual(expectedValue, actual.Value);
        Assert.AreEqual(expectedError, actual.Error);

        // Passing value by properties.
        actual = new ValueDataError<float>();
        actual.Value = expectedValue;
        actual.Error = expectedError;
        Assert.AreEqual(expectedValue, actual.Value);
        Assert.AreEqual(expectedError, actual.Error);
    }
}
