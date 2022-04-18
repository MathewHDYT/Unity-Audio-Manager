using AudioManager.Core;
using AudioManager.Locator;
using AudioManager.Logger;
using NUnit.Framework;

public class TestServiceLocator {
    [Test]
    public void TestGetService() {
        // Checking if default is the NullAudioManager.
        IAudioManager audioManager = ServiceLocator.GetService();
        Assert.IsNotNull(audioManager as NullAudioManager);
    }

    [Test]
    public void TestRegisterLogger() {
        // Setting custom IAudioLogger implementation and checking if it was set.
        IAudioLogger logger = new AudioLogger(LoggingLevel.NONE);
        ServiceLocator.RegisterLogger(logger, null);
        IAudioManager audioManager = ServiceLocator.GetService();
        Assert.IsNotNull(audioManager as LoggedAudioManager);
    }

    [Test]
    public void TestRegisterService() {
        // Setting custom IAudioManager implementation and checking if it was set.
        ServiceLocator.RegisterService(new DummyAudioManager());
        IAudioManager audioManager = ServiceLocator.GetService();
        Assert.IsNotNull(audioManager as DummyAudioManager);
    }
}
