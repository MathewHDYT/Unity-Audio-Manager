using NUnit.Framework;
using AudioManager.Logger;
using AudioManager.Core;

public class TestErrorToStringConvertor {
    [Test]
    public void TestErrorToMessage() {
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.OK));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.DOES_NOT_EXIST));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.ALREADY_EXISTS));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.INVALID_PATH));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.INVALID_END_VALUE));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.INVALID_GRANULARITY));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.INVALID_TIME));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.INVALID_PROGRESS));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.MIXER_NOT_EXPOSED));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.MISSING_SOURCE));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.MISSING_MIXER_GROUP));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.CAN_NOT_BE_3D));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.NOT_INITIALIZED));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.MISSING_CLIP));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.MISSING_PARENT));
        Assert.IsNotEmpty(ErrorToStringConvertor.ErrorToMessage(AudioError.INVALID_PARENT));
        Assert.IsEmpty(ErrorToStringConvertor.ErrorToMessage((AudioError)(-1)));
    }
}
