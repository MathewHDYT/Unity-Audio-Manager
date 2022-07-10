---
layout: default
title: Custom Audio Manager
nav_order: 8
permalink: /custom_audio_manager
---

## Custom Audio Manager

Starting from ```v1.7.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)) the project structure has been completly reworked. Therefore a custom ```IAudioManager``` implementation can now be implemented.

To create a custom ```IAudioManager``` implementation, we first have to create a new C# script. An example template can be found below:

```csharp
using AudioManager.Core;
using UnityEngine;
using UnityEngine.Audio;

public class ExampleAudioManager : IAudioManager {
    public ExampleAudioManager() {
        // Nothing to do.
    }

    public AudioError AddSoundFromPath(string name, string path, float volume, float pitch, bool loop, AudioSource source, AudioMixerGroup mixerGroup) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError Play(string name) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError PlayAtTimeStamp(string name, float startTime) {
        return AudioError.NOT_INITIALIZED;
    }

    public ValueDataError<float> GetPlaybackPosition(string name) {
        return new ValueDataError<float>(float.NaN, AudioError.NOT_INITIALIZED);
    }

    public AudioError PlayAt3DPosition(string name, Vector3 position) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError PlayOneShotAt3DPosition(string name, Vector3 position) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError PlayAttachedToGameObject(string name, GameObject gameObject) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError PlayOneShotAttachedToGameObject(string name, GameObject gameObject) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError PlayDelayed(string name, float delay) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError PlayOneShot(string name) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError ChangePitch(string name, float minPitch, float maxPitch) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError PlayScheduled(string name, double time) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError Stop(string name) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError ToggleMute(string name) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError TogglePause(string name) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError SubscribeAudioFinished(string name, float remainingTime, AudioFinishedCallback callback) {
        return AudioError.NOT_INITIALIZED;
    }

    public ValueDataError<float> GetProgress(string name) {
        return new ValueDataError<float>(float.NaN, AudioError.NOT_INITIALIZED);
    }

    public AudioError TryGetSource(string name, out AudioSource source) {
        source = null;
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError LerpPitch(string name, float endValue, float waitTime, int granularity) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError LerpVolume(string name, float endValue, float waitTime, int granularity) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError ChangeGroupValue(string name, string exposedParameterName, float newValue) {
        return AudioError.NOT_INITIALIZED;
    }

    public ValueDataError<float> GetGroupValue(string name, string exposedParameterName) {
        return new ValueDataError<float>(float.NaN, AudioError.NOT_INITIALIZED);
    }

    public AudioError ResetGroupValue(string name, string exposedParameterName) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError LerpGroupValue(string name, string exposedParameterName, float endValue, float waitTime, int granularity) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError RemoveGroup(string name) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError AddGroup(string name, AudioMixerGroup mixerGroup) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError RemoveSound(string name) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError Set3DAudioOptions(string name, float minDistance, float maxDistance, float spatialBlend, float spread, float dopplerLevel, AudioRolloffMode rolloffMode) {
        return AudioError.NOT_INITIALIZED;
    }

    public AudioError SetStartTime(string name, float startTime) {
        return AudioError.NOT_INITIALIZED;
    }
}
```

Be aware calling ```ServiceLocator.RegisterService()``` will register the ```NullAudioManager``` if null is passed into the function, meaning no Audio will be played anymore until ```ServiceLocator.RegisterService()```, has been called again.
Additionaly registering your own custom ```IAudioManager``` with ```ServiceLocator.RegisterService()```, will result in logging being disabled if this is wanted as well either use the default ```IAudioLogger``` found in the ```AudioManager.Logger``` namespace or see ([Logging](https://mathewhdyt.github.io/Unity-Audio-Manager/logging)) if you also want to create a custom ```IAudioLogger``` implementation.

## Disabling audio at runtime

To disable all use of the AudioManager globally at runtime you can use the ```ServiceLocator.RegisterService()``` and simply pass null. Be aware tough this will result in both the ```IAudioLogger``` and ```IAudioManager```  being reset.
Therefore if the same ```IAudioManager``` wants to be used afterwards, first use ```ServiceLocator.GetService()``` and stash the result locally in the script. Then if Audio needs be enabled again simply call ```ServiceLocator.RegisterService()``` again, with the stashed ```IAudioManager```.
A short example can be found below:

```csharp
using AudioManager.Core;
using AudioManager.Locator;

public class AudioHandler {
    // Set the cachedInstance to NullAudioManager per default,
    // to ensure EnableAudio doesn't register null and disable audio completly.
    private IAudioManager cachedInstance = NullAudioManager;

    public static void DisableAudio() {
        cachedInstance = ServiceLocator.GetService();
        ServiceLocator.RegisterService(null);
    }

    public static void EnableAudio() {
        ServiceLocator.RegisterService(cachedInstance);
    }
}
```
