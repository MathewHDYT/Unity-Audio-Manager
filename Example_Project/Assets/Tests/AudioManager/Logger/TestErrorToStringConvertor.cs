using NUnit.Framework;
using AudioManager.Logger;
using AudioManager.Core;
using System;
using System.Linq;

public class TestErrorToStringConvertor {
    [Test]
    public void TestErrorToMessage() {
        var audioErrors = Enum.GetValues(typeof(AudioError)).Cast<AudioError>();
        foreach (var audioError in audioErrors) {
            Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(audioError));
        }
        Assert.IsEmpty(ErrorToStringConvertor.ErrorToMessage((AudioError)(-1)));
    }
}
