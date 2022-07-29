---
layout: default
title: Unsubscribe Progress Coroutine
parent: AudioSource
grand_parent: Documentation
---

## Unsubscribe Progress Coroutine
**What it does:**
Unsubscribes the previously via. [```SubscribeProgressCoroutine```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/subscribe_progress_coroutine/) subscribed ```ProgressCoroutineCallback```,
so that it will not be called anymore when the sound reaches the given progress point in the clips runtime
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
- ```Progress``` is the point in the clips runtime from 0 to 1 the callback we want to unsubscribe was subscribed at

```csharp
string soundName = "SoundName";
float progress = 0f;

AudioError error = am.UnsubscribeProgressCoroutine(soundName, progress);
if (err != AudioError.OK) {
    Debug.Log("Unsubscribing to the callback of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Unsubscribing to the callback of the sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to stop calling and detection of a previously subscribed ```ProgressCoroutineCallback```. Is also done automatically when returning ```ProgressResponse.UNSUB``` in the callback itself.
