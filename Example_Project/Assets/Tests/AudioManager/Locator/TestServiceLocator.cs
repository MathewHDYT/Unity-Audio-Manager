using AudioManager.Core;
using AudioManager.Locator;
using AudioManager.Logger;
using NUnit.Framework;

public sealed class TestServiceLocator {
    [Test]
    public void TestGetService() {
        // Checking if default is the NullAudioManager.
        IAudioManager audioManager = ServiceLocator.GetService();
        Assert.IsTrue(audioManager is NullAudioManager);
    }

    [Test]
    public void TestRegisterLogger() {
        // Setting null and checking if it wasn't set.
        ServiceLocator.RegisterLogger(null, null);
        IAudioManager audioManager = ServiceLocator.GetService();
        Assert.IsTrue(audioManager is NullAudioManager);
        Assert.IsFalse(audioManager is LoggedAudioManager);

        // Setting custom IAudioLogger implementation and checking if it was set.
        IAudioLogger logger = new AudioLogger(LoggingLevel.NONE);
        ServiceLocator.RegisterLogger(logger, null);
        audioManager = ServiceLocator.GetService();
        Assert.IsTrue(audioManager is LoggedAudioManager);
    }

    [Test]
    public void TestRegisterService() {
        // Setting null and checking if the default was set.
        ServiceLocator.RegisterService(null);
        IAudioManager audioManager = ServiceLocator.GetService();
        Assert.IsTrue(audioManager is NullAudioManager);
        Assert.IsFalse(audioManager is DummyAudioManager);

        // Setting custom IAudioManager implementation and checking if it was set.
        ServiceLocator.RegisterService(new DummyAudioManager());
        audioManager = ServiceLocator.GetService();
        Assert.IsTrue(audioManager is DummyAudioManager);
    }
}
