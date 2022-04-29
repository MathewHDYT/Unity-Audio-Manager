---
layout: default
title: Subscribe Audio Finished
parent: AudioSource
grand_parent: Documentation
---

## Subscribe Audio Finished
**What it does:**
Subscribes the given callback, so that it will be called the moment when the given sound only has the given amount of ```remainingTime``` left to play and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how subscribing the callback failed.

**How to call it:**
- ```SoundName``` is the ```name``` the new sound should have
- ```RemainingTime``` is the amount of remaining playback time in seconds the sound has left to play when the callback should be called
- ```Callback``` is the ```AudioFinishedCallback(string name, float remainingTime)``` that should be called

```csharp
string soundName = "SoundName";
float remainingTime = 5f;

AudioError error = am.SubscribeAudioFinished(soundName, remainingTime, SoundFinishedCallback);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Subscribing to the callback of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Subscribing to the callback of the sound called: " + soundName + " succesfull");
}

private void SoundFinishedCallback(string name, float remainingTime) {
    if (name == "SoundName") {
        // LerpVolume of old clip down to 0 in the given remaingTime.
        // Play new sound registered with the AudioManager.
        // LerpVolume of new clip up to 1 in the given remaingTime.
    }
    else {
        // Unexpected subscribed sound finished.
    }
}
```

**When to use it:**
When you want to smoothly transition from one song into another you can use the given remainingTime to start playing and fading in another sound and fading in the old sound with the [```LerpVolume```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/lerp_volume/) method.

**Remarks:**
The callback returns two params, which consist of the name that we subscribed the callback too, as well as the ```remainingTime``` we wanted to call the callback at. Both of these parameters may be used to differentiate, between different callbacks if the callback method is subscribed to multiple sounds or at multiple times in the sound itself, or may be even used to decrease the volume over the given ```remainigTime```. 

