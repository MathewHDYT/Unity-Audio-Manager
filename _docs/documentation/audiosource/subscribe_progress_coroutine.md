---
layout: default
title: Subscribe Progress Coroutine
parent: AudioSource
grand_parent: Documentation
---

## Subscribe Progress Coroutine
**What it does:**
Subscribes the given ```ProgressCoroutineCallback```, so that it will be called with the given name, progress and [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that triggered the callback,
as parameters as soon as the sound has reached the given progress point in the clips runtime. Depeding on the return value of the callback, it will be subscribed again for the next time that progress is hit
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
- ```Progress``` is the point in the clips runtime from 0 to 1, when the callback should be called
- ```Callback``` is the ```ProgressCoroutineCallback(string name, float progress, ChildType child)``` that should be called

```csharp
string soundName = "SoundName";
float progress = 0f;
ProgressCoroutineCallback callback = SoundStartedCallback;

AudioError error = am.SubscribeProgressCoroutine(soundName, progress, callback);
if (err != AudioError.OK) {
    Debug.Log("Subscribing to the callback of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Subscribing to the callback of the sound called: " + soundName + " succesfull");
}
```

**Callback method:**
```csharp
private ProgressResponse SoundStartedCallback(string name, float progress, ChildType child) {
    // Do something.
	return ProgressResponse.UNSUB;
}
```

**When to use it:**
When you want to smoothly transition from one song into another you can use the given progress to start playing and fading in another sound and fading in the old sound with the [```LerpVolume```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/lerp_volume/) method.

**Remarks:**
The callback has multiple parameters, which consist of the name that we subscribed the callback too, as well as the ```progress``` we wanted to call the callback at, as well as the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that actually called the callback.
All of these parameters may be used to differentiate, between different callbacks if the callback method is subscribed to multiple sounds or at multiple times in the sound itself.

**ProgressResponse:**

| **ID** | **CONSTANT**                  | **MEANING**                                                                                                                                                 |
| -------| ------------------------------| ------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 0      | UNSUB                         | Does not call the given ProgressCoroutineCallback anymore                                                                                                   |
| 1      | RESUB_IN_LOOP                 | Calls the given ProgressCoroutineCallback for the next loop iteration of the song at the same progress point                                                |
| 2      | RESUB_IMMEDIATE               | Calls the given ProgressCoroutineCallback immediatly as soon as the subscribed time is reached again, only recommended if we skip back time in the callback |
