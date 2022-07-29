---
layout: default
title: Subscribe Source Changed
parent: AudioSource
grand_parent: Documentation
---

## Subscribe Source Changed
**What it does:**
Subscribes the given ```SourceChangedCallback```, so that it will be called when the underlying ```AudioSourceWrapper``` of the subscribed sound has been changed.
and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how subscribing the callback failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- MISSING_PARENT
- INVALID_PROGRESS
- ALREADY_SUBSCRIBED

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to subscribe
- ```Callback``` is the ```SourceChangedCallback(AudioSourceWrapper changedSource)``` that should be called

```csharp
string soundName = "SoundName";
SourceChangedCallback callback = SoundChanged;

AudioError error = am.SubscribeProgressCoroutine(soundName, callback);
if (err != AudioError.OK) {
    Debug.Log("Subscribing to the callback of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Subscribing to the callback of the sound called: " + soundName + " succesfull");
}
```

**Callback method:**
```csharp
private void SoundChanged(AudioSourceWrapper changedSource) {
    // Do something.
}
```

**When to use it:**
When you want to smoothly transition from one song into another you can use the given progress to start playing and fading in another sound and fading in the old sound with the [```LerpVolume```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/lerp_volume/) method.

**Remarks:**
The callback has one parameter, which is the ```AudioSourceWrapper``` that has changed some of its values.
