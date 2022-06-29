using AudioManager.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

public class TestAudioSourceWrapper {
    AudioSource m_source;
    AudioMixerGroup m_mixerGroup;
    AudioSourceWrapper m_wrapper;
    AudioSourceWrapper m_initalizedWrapper;

    [SetUp]
    public void TestSetUp() {
        GameObject gameObject = new GameObject();
        m_source = gameObject.AddComponent<AudioSource>();
        AudioMixer mixer = Resources.Load<AudioMixer>("Mixer");
        m_mixerGroup = mixer ? mixer.FindMatchingGroups("Master")[0] : null;
        m_wrapper = new AudioSourceWrapper(null);
        m_initalizedWrapper = new AudioSourceWrapper(m_source);
    }

    [Test]
    public void TestConstructor() {
        var wrapper = new AudioSourceWrapper(null);
        Assert.IsNull(wrapper.Source);

        wrapper = new AudioSourceWrapper(m_source);
        Assert.IsNotNull(wrapper.Source);
    }

    [Test]
    public void TestSet() {
        Assert.AreEqual(default, m_wrapper.MixerGroup);
        m_wrapper.MixerGroup = m_mixerGroup;
        Assert.AreEqual(default, m_wrapper.MixerGroup);

        Assert.AreEqual(default(float), m_wrapper.Volume);
        m_wrapper.Volume = 0.5f;
        Assert.AreEqual(default(float), m_wrapper.Volume);

        Assert.AreEqual(default(float), m_wrapper.Pitch);
        m_wrapper.Pitch = 0.5f;
        Assert.AreEqual(default(float), m_wrapper.Pitch);

        Assert.AreEqual(default(float), m_wrapper.Time);
        m_wrapper.Time = 0.5f;
        Assert.AreEqual(default(float), m_wrapper.Time);

        Assert.AreEqual(default(float), m_wrapper.SpatialBlend);
        m_wrapper.SpatialBlend = 0.5f;
        Assert.AreEqual(default(float), m_wrapper.SpatialBlend);

        Assert.AreEqual(default(float), m_wrapper.DopplerLevel);
        m_wrapper.DopplerLevel = 0.5f;
        Assert.AreEqual(default(float), m_wrapper.DopplerLevel);

        Assert.AreEqual(default(float), m_wrapper.Spread);
        m_wrapper.Spread = 0.5f;
        Assert.AreEqual(default(float), m_wrapper.Spread);

        Assert.AreEqual((AudioRolloffMode)default(int), m_wrapper.RolloffMode);
        m_wrapper.RolloffMode = AudioRolloffMode.Custom;
        Assert.AreEqual((AudioRolloffMode)default(int), m_wrapper.RolloffMode);

        Assert.AreEqual(default(float), m_wrapper.MinDistance);
        m_wrapper.MinDistance = 0.5f;
        Assert.AreEqual(default(float), m_wrapper.MinDistance);

        Assert.AreEqual(default(float), m_wrapper.MaxDistance);
        m_wrapper.MaxDistance = 0.5f;
        Assert.AreEqual(default(float), m_wrapper.MaxDistance);

        Assert.AreEqual(default(bool), m_wrapper.Loop);
        m_wrapper.Loop = true;
        Assert.AreEqual(default(bool), m_wrapper.Loop);

        Assert.AreEqual(default(bool), m_wrapper.Spatialize);
        m_wrapper.Spatialize = true;
        Assert.AreEqual(default(bool), m_wrapper.Spatialize);

        Assert.AreEqual(default(bool), m_wrapper.Mute);
        m_wrapper.Mute = true;
        Assert.AreEqual(default(bool), m_wrapper.Mute);

        Assert.AreEqual(m_source.outputAudioMixerGroup, m_initalizedWrapper.MixerGroup);
        m_initalizedWrapper.MixerGroup = m_mixerGroup;
        Assert.AreEqual(m_source.outputAudioMixerGroup, m_initalizedWrapper.MixerGroup);

        Assert.AreEqual(m_source.volume, m_initalizedWrapper.Volume);
        m_initalizedWrapper.Volume = 0.5f;
        Assert.AreEqual(m_source.volume, m_initalizedWrapper.Volume);

        Assert.AreEqual(m_source.pitch, m_initalizedWrapper.Pitch);
        m_initalizedWrapper.Pitch = 0.5f;
        Assert.AreEqual(m_source.pitch, m_initalizedWrapper.Pitch);

        Assert.AreEqual(m_source.time, m_initalizedWrapper.Time);
        m_initalizedWrapper.Time = 0.5f;
        Assert.AreEqual(m_source.time, m_initalizedWrapper.Time);

        Assert.AreEqual(m_source.spatialBlend, m_initalizedWrapper.SpatialBlend);
        m_initalizedWrapper.SpatialBlend = 0.5f;
        Assert.AreEqual(m_source.spatialBlend, m_initalizedWrapper.SpatialBlend);

        Assert.AreEqual(m_source.dopplerLevel, m_initalizedWrapper.DopplerLevel);
        m_initalizedWrapper.SpatialBlend = 0.5f;
        Assert.AreEqual(m_source.dopplerLevel, m_initalizedWrapper.DopplerLevel);

        Assert.AreEqual(m_source.spread, m_initalizedWrapper.Spread);
        m_initalizedWrapper.SpatialBlend = 0.5f;
        Assert.AreEqual(m_source.spread, m_initalizedWrapper.Spread);

        Assert.AreEqual(m_source.rolloffMode, m_initalizedWrapper.RolloffMode);
        m_initalizedWrapper.RolloffMode = AudioRolloffMode.Custom;
        Assert.AreEqual(m_source.rolloffMode, m_initalizedWrapper.RolloffMode);

        Assert.AreEqual(m_source.minDistance, m_initalizedWrapper.MinDistance);
        m_initalizedWrapper.SpatialBlend = 0.5f;
        Assert.AreEqual(m_source.minDistance, m_initalizedWrapper.MinDistance);

        Assert.AreEqual(m_source.maxDistance, m_initalizedWrapper.MaxDistance);
        m_initalizedWrapper.SpatialBlend = 0.5f;
        Assert.AreEqual(m_source.maxDistance, m_initalizedWrapper.MaxDistance);

        Assert.AreEqual(m_source.loop, m_initalizedWrapper.Loop);
        m_initalizedWrapper.Loop = true;
        Assert.AreEqual(m_source.loop, m_initalizedWrapper.Loop);

        Assert.AreEqual(m_source.spatialize, m_initalizedWrapper.Spatialize);
        m_initalizedWrapper.Loop = true;
        Assert.AreEqual(m_source.spatialize, m_initalizedWrapper.Spatialize);

        Assert.AreEqual(m_source.mute, m_initalizedWrapper.Mute);
        m_initalizedWrapper.Loop = true;
        Assert.AreEqual(m_source.mute, m_initalizedWrapper.Mute);
    }

    [Test]
    public void TestGet() {
        Assert.AreEqual(default, m_wrapper.Source);
        Assert.AreEqual(default, m_wrapper.MixerGroup);
        Assert.AreEqual(default(float), m_wrapper.Volume);
        Assert.AreEqual(default(float), m_wrapper.Pitch);
        Assert.AreEqual(default(float), m_wrapper.Time);
        Assert.AreEqual(default(float), m_wrapper.SpatialBlend);
        Assert.AreEqual(default(float), m_wrapper.DopplerLevel);
        Assert.AreEqual(default(float), m_wrapper.Spread);
        Assert.AreEqual((AudioRolloffMode)default(int), m_wrapper.RolloffMode);
        Assert.AreEqual(default(float), m_wrapper.MinDistance);
        Assert.AreEqual(default(float), m_wrapper.MaxDistance);
        Assert.AreEqual(default(bool), m_wrapper.Loop);
        Assert.AreEqual(default(bool), m_wrapper.Spatialize);
        Assert.AreEqual(default(bool), m_wrapper.Mute);

        Assert.AreEqual(m_source, m_initalizedWrapper.Source);
        Assert.AreEqual(m_source.outputAudioMixerGroup, m_initalizedWrapper.MixerGroup);
        Assert.AreEqual(m_source.volume, m_initalizedWrapper.Volume);
        Assert.AreEqual(m_source.pitch, m_initalizedWrapper.Pitch);
        Assert.AreEqual(m_source.time, m_initalizedWrapper.Time);
        Assert.AreEqual(m_source.spatialBlend, m_initalizedWrapper.SpatialBlend);
        Assert.AreEqual(m_source.dopplerLevel, m_initalizedWrapper.DopplerLevel);
        Assert.AreEqual(m_source.spread, m_initalizedWrapper.Spread);
        Assert.AreEqual(m_source.rolloffMode, m_initalizedWrapper.RolloffMode);
        Assert.AreEqual(m_source.minDistance, m_initalizedWrapper.MinDistance);
        Assert.AreEqual(m_source.maxDistance, m_initalizedWrapper.MaxDistance);
        Assert.AreEqual(m_source.loop, m_initalizedWrapper.Loop);
        Assert.AreEqual(m_source.spatialize, m_initalizedWrapper.Spatialize);
        Assert.AreEqual(m_source.mute, m_initalizedWrapper.Mute);
    }
}
