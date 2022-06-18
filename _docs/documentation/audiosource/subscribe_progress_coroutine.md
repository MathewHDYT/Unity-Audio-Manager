---
layout: default
title: Subscribe Progress Coroutine
parent: AudioSource
grand_parent: Documentation
---

## Subscribe Progress Coroutine
**What it does:**
Subscribes the given AudioFinishedCallback, so that it will be called with the given name and progress as a parameter,
as soon as the sound has reached the given progress point in the clips runtime. Depeding on the return value of the callback,
it will be subscribed again for the next time that progress is hit.

**How to call it:**
- ```SoundName``` is the ```name``` the new sound should have
- ```Progress``` is the point in the clips runtime from 0 to 1, when the callback should be called
- ```Callback``` is the ```AudioFinishedCallback(string name, float progress)``` that should be called

```csharp
string soundName = "SoundName";
float progress = 0f;

AudioError error = am.SubscribeAudioFinished(soundName, progress, SoundFinishedCallback);
if (err != AudioError.OK) {
    Debug.Log("Subscribing to the callback of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Subscribing to the callback of the sound called: " + soundName + " succesfull");
}

private ProgressResponse SoundFinishedCallback(string name, float progress) {
    if (name == "SoundName") {
        // LerpVolume of old clip down to 0 in (progress * source.clip.length) time.
        // Play new sound registered with the AudioManager.
        // LerpVolume of new clip up to 1 in (progress * source.clip.length) time.
    }
    else {
        // Unexpected subscribed sound finished.
    }
	return ProgressResponse.UNSUB;
}
```

**When to use it:**
When you want to smoothly transition from one song into another you can use the given progress to start playing and fading in another sound and fading in the old sound with the [```LerpVolume```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/lerp_volume/) method.

**Remarks:**
The callback returns two params, which consist of the name that we subscribed the callback too, as well as the ```progress``` we wanted to call the callback at. Both of these parameters may be used to differentiate, between different callbacks if the callback method is subscribed to multiple sounds or at multiple times in the sound itself, or may be even used to decrease the volume, which we can caculate with the ```progress```. 

**ProgressResponse:**
| **ID** | **CONSTANT**                  | **MEANING**                                                                                                                                     |
| -------| ------------------------------| ------------------------------------------------------------------------------------------------------------------------------------------------|
| 0      | UNSUB                         | Does not call the given AudioCallback anymore                                                                                                   |
| 1      | RESUB_IN_LOOP                 | Calls the given AudioCallback for the next loop iteration of the song at the same progress point                                                |
| 2      | RESUB_IMMEDIATE               | Calls the given AudioCallback immediatly as soon as the subscribed time is reached again, only recommended if we skip back time in the callback |
