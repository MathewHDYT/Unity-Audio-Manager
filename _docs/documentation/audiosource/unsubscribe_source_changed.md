---
layout: default
title: Unsubscribe Progress Coroutine
parent: AudioSource
grand_parent: Documentation
---

## Unsubscribe Progress Coroutine
**What it does:**
Unsubscribes the previously via. [```SubscribeSourceChanged```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/subscribe_source_changed/) subscribed ```SourceChangedCallback```,
so that it will not be called anymore when the underlying ```AudioSourceWrapper``` of the subscribed sound has been changed
and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how unsubscribing the callback failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- MISSING_PARENT
- INVALID_PROGRESS
- NOT_SUBSCRIBED

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to unsubscribe
- ```Callback``` is the ```SourceChangedCallback(AudioSourceWrapper changedSource)``` that should not be called anymore

```csharp
string soundName = "SoundName";
SourceChangedCallback callback = SoundChanged;

AudioError error = am.UnsubscribeProgressCoroutine(soundName, callback);
if (err != AudioError.OK) {
    Debug.Log("Unsubscribing to the callback of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Unsubscribing to the callback of the sound called: " + soundName + " succesfull");
}
```

**Callback method:**
```csharp
private void SoundChanged(AudioSourceWrapper changedSource) {
    // Do something.
}
```

**When to use it:**
When you want to stop calling and detection of a previously subscribed ```SourceChangedCallback```.
