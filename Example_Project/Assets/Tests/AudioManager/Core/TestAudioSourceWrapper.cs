using AudioManager.Core;
using AudioManager.Helper;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

public sealed class TestAudioSourceWrapper {
    AudioSource m_source;
    AudioSource m_groupSource;
    AudioMixerGroup m_mixerGroup;
    AudioSourceWrapper m_wrapper;
    AudioSourceWrapper m_initalizedWrapper;

    [SetUp]
    public void TestSetUp() {
        var gameObject = new GameObject();
        m_source = gameObject.AddComponent<AudioSource>();
        m_groupSource = gameObject.AddComponent<AudioSource>();
        AudioMixer mixer = Resources.Load<AudioMixer>("Mixer");
        m_mixerGroup = mixer ? mixer.FindMatchingGroups("Master")[0] : null;
        m_groupSource.outputAudioMixerGroup = m_mixerGroup;
        m_wrapper = new AudioSourceWrapper(null);
        m_initalizedWrapper = new AudioSourceWrapper(m_source);
    }

    [Test]
    public void TestConstructor() {
        var wrapper = new AudioSourceWrapper(null);
        Assert.IsNull(wrapper.Source);
        Assert.IsNull(wrapper.MixerGroup);

        wrapper = new AudioSourceWrapper(m_source);
        Assert.IsNotNull(wrapper.Source);
        Assert.IsNull(wrapper.MixerGroup);

        wrapper = new AudioSourceWrapper(m_groupSource);
        Assert.IsNotNull(wrapper.Source);
        Assert.IsNotNull(wrapper.Mixer);
    }

    [Test]
    public void TestInvokeChild() {
        int calledCount = 0;
        int validcalledCallbackCount = 0;
        int invalidCalledCallbackCount = 0;
        ChildType child = ChildType.ALL;
        InvokeCallback validCallback = (s) => {
            validcalledCallbackCount++;
            Assert.IsNotNull(s);
        };
        InvokeCallback invalidCallback = (s) => {
            invalidCalledCallbackCount++;
            Assert.IsNull(s);
        };

        m_wrapper.InvokeChild(child, null);

        // Register child to test ChildType.ALL.
        m_wrapper.RegisterNewChild(ChildType.AT_3D_POS, null);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        m_wrapper.InvokeChild(child, invalidCallback);
        Assert.AreEqual(calledCount += 2, invalidCalledCallbackCount);

        Assert.AreEqual(calledCount++, invalidCalledCallbackCount);
        child = ChildType.PARENT;
        m_wrapper.InvokeChild(child, invalidCallback);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);

        Assert.AreEqual(calledCount++, invalidCalledCallbackCount);
        child = ChildType.AT_3D_POS;
        m_wrapper.InvokeChild(child, invalidCallback);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        child = ChildType.ATTCHD_TO_GO;
        m_wrapper.InvokeChild(child, invalidCallback);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        child = (ChildType)(-1);
        m_wrapper.InvokeChild(child, invalidCallback);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);

        calledCount = 0;
        // Register child to test ChildType.ALL.
        m_initalizedWrapper.RegisterNewChild(ChildType.AT_3D_POS, m_source);
        m_initalizedWrapper.RegisterNewChild(ChildType.ATTCHD_TO_GO, m_source);

        Assert.AreEqual(calledCount, validcalledCallbackCount);
        child = ChildType.ALL;
        m_initalizedWrapper.InvokeChild(child, validCallback);
        Assert.AreEqual(calledCount += 3, validcalledCallbackCount);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.PARENT;
        m_initalizedWrapper.InvokeChild(child, validCallback);
        Assert.AreEqual(calledCount, validcalledCallbackCount);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.AT_3D_POS;
        m_initalizedWrapper.InvokeChild(child, validCallback);
        Assert.AreEqual(calledCount, validcalledCallbackCount);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.ATTCHD_TO_GO;
        m_initalizedWrapper.InvokeChild(child, validCallback);
        Assert.AreEqual(calledCount, validcalledCallbackCount);

        Assert.AreEqual(calledCount, validcalledCallbackCount);
        child = (ChildType)(-1);
        m_initalizedWrapper.InvokeChild(child, validCallback);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
    }

    [Test]
    public void TestInvokeChildFloat() {
        float validValue = 1f;
        float invalidValue = float.NaN;
        int calledCount = 0;
        int validcalledCallbackCount = 0;
        int invalidCalledCallbackCount = 0;
        ChildType child = ChildType.ALL;
        InvokeCallback<float> validCallback = (s) => {
            validcalledCallbackCount++;
            Assert.IsNotNull(s);
            return validValue;
        };
        InvokeCallback<float> invalidCallback = (s) => {
            invalidCalledCallbackCount++;
            Assert.IsNull(s);
            return invalidValue;
        };

        m_wrapper.InvokeChild(child, null, out float value);

        // Register child to test ChildType.ALL.
        m_wrapper.RegisterNewChild(ChildType.AT_3D_POS, null);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount += 2, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount++, invalidCalledCallbackCount);
        child = ChildType.PARENT;
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount++, invalidCalledCallbackCount);
        child = ChildType.AT_3D_POS;
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        child = ChildType.ATTCHD_TO_GO;
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        child = (ChildType)(-1);
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.IsNaN(value);

        calledCount = 0;
        // Register child to test ChildType.ALL.
        m_initalizedWrapper.RegisterNewChild(ChildType.AT_3D_POS, m_source);
        m_initalizedWrapper.RegisterNewChild(ChildType.ATTCHD_TO_GO, m_source);

        Assert.AreEqual(calledCount, validcalledCallbackCount);
        child = ChildType.ALL;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount += 3, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.PARENT;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.AT_3D_POS;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.ATTCHD_TO_GO;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount, validcalledCallbackCount);
        child = (ChildType)(-1);
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.IsNaN(value);
    }

    [Test]
    public void TestInvokeChildDouble() {
        double validValue = 1d;
        double invalidValue = double.NaN;
        int calledCount = 0;
        int validcalledCallbackCount = 0;
        int invalidCalledCallbackCount = 0;
        ChildType child = ChildType.ALL;
        InvokeCallback<double> validCallback = (s) => {
            validcalledCallbackCount++;
            Assert.IsNotNull(s);
            return validValue;
        };
        InvokeCallback<double> invalidCallback = (s) => {
            invalidCalledCallbackCount++;
            Assert.IsNull(s);
            return invalidValue;
        };

        m_wrapper.InvokeChild(child, null, out double value);

        // Register child to test ChildType.ALL.
        m_wrapper.RegisterNewChild(ChildType.AT_3D_POS, null);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount += 2, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount++, invalidCalledCallbackCount);
        child = ChildType.PARENT;
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount++, invalidCalledCallbackCount);
        child = ChildType.AT_3D_POS;
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        child = ChildType.ATTCHD_TO_GO;
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        child = (ChildType)(-1);
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.IsNaN(value);

        calledCount = 0;
        // Register child to test ChildType.ALL.
        m_initalizedWrapper.RegisterNewChild(ChildType.AT_3D_POS, m_source);
        m_initalizedWrapper.RegisterNewChild(ChildType.ATTCHD_TO_GO, m_source);

        Assert.AreEqual(calledCount, validcalledCallbackCount);
        child = ChildType.ALL;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount += 3, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.PARENT;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.AT_3D_POS;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.ATTCHD_TO_GO;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount, validcalledCallbackCount);
        child = (ChildType)(-1);
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.IsNaN(value);
    }

    [Test]
    public void TestInvokeChildBool() {
        bool validValue = true;
        bool invalidValue = false;
        int calledCount = 0;
        int validcalledCallbackCount = 0;
        int invalidCalledCallbackCount = 0;
        ChildType child = ChildType.ALL;
        InvokeCallback<bool> validCallback = (s) => {
            validcalledCallbackCount++;
            Assert.IsNotNull(s);
            return validValue;
        };
        InvokeCallback<bool> invalidCallback = (s) => {
            invalidCalledCallbackCount++;
            Assert.IsNull(s);
            return invalidValue;
        };

        m_wrapper.InvokeChild(child, null, out bool value);

        // Register child to test ChildType.ALL.
        m_wrapper.RegisterNewChild(ChildType.AT_3D_POS, null);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount += 2, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount++, invalidCalledCallbackCount);
        child = ChildType.PARENT;
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount++, invalidCalledCallbackCount);
        child = ChildType.AT_3D_POS;
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        child = ChildType.ATTCHD_TO_GO;
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.AreEqual(invalidValue, value);

        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        child = (ChildType)(-1);
        m_wrapper.InvokeChild(child, invalidCallback, out value);
        Assert.AreEqual(calledCount, invalidCalledCallbackCount);
        Assert.IsFalse(value);

        calledCount = 0;
        // Register child to test ChildType.ALL.
        m_initalizedWrapper.RegisterNewChild(ChildType.AT_3D_POS, m_source);
        m_initalizedWrapper.RegisterNewChild(ChildType.ATTCHD_TO_GO, m_source);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.ALL;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.PARENT;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.AT_3D_POS;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount++, validcalledCallbackCount);
        child = ChildType.ATTCHD_TO_GO;
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.AreEqual(validValue, value);

        Assert.AreEqual(calledCount, validcalledCallbackCount);
        child = (ChildType)(-1);
        m_initalizedWrapper.InvokeChild(child, validCallback, out value);
        Assert.AreEqual(calledCount, validcalledCallbackCount);
        Assert.IsFalse(value);
    }

    [Test]
    public void TestGetChildSource() {
        ChildType child = ChildType.ALL;

        // Register child to test ChildType.ALL.
        m_wrapper.RegisterNewChild(ChildType.AT_3D_POS, null);

        m_wrapper.GetChildSource(child, out AudioSource source);
        Assert.IsNull(source);

        child = ChildType.PARENT;
        m_wrapper.GetChildSource(child, out source);
        Assert.IsNull(source);

        child = ChildType.AT_3D_POS;
        m_wrapper.GetChildSource(child, out source);
        Assert.IsNull(source);

        child = ChildType.ATTCHD_TO_GO;
        m_wrapper.GetChildSource(child, out source);
        Assert.IsNull(source);

        child = (ChildType)(-1);
        m_wrapper.GetChildSource(child, out source);
        Assert.IsNull(source);

        // Register child to test ChildType.ALL.
        m_initalizedWrapper.RegisterNewChild(ChildType.AT_3D_POS, m_source);
        m_initalizedWrapper.RegisterNewChild(ChildType.ATTCHD_TO_GO, m_source);

        child = ChildType.ALL;
        m_wrapper.GetChildSource(child, out source);
        Assert.IsNull(source);

        child = ChildType.PARENT;
        m_wrapper.GetChildSource(child, out source);
        Assert.IsNull(source);

        child = ChildType.AT_3D_POS;
        m_wrapper.GetChildSource(child, out source);
        Assert.IsNull(source);

        child = ChildType.ATTCHD_TO_GO;
        m_wrapper.GetChildSource(child, out source);
        Assert.IsNull(source);

        child = (ChildType)(-1);
        m_wrapper.GetChildSource(child, out source);
        Assert.IsNull(source);
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
