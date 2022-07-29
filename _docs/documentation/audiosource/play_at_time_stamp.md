---
layout: default
title: Play At Time Stamp
parent: AudioSource
grand_parent: Documentation
---

## Play At Time Stamp
**What it does:**
Start playing the choosen sound at the given ```startTime``` and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound from the given ```startTime``` failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- MISSING_PARENT
- INVALID_TIME
- INVALID_PROGRESS
- ALREADY_SUBSCRIBED

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```StartTime``` is the moment we want to play the sound at so instead of starting at 0 seconds we start at 10 seconds in the audio clip
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
float startTime = 10f;
ChildType child = ChildType.PARENT;

AudioError err = am.PlayAtTimeStamp(soundName, startTime, child);
if (err != AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " at startTime: " + startTime.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " at startTime: " + startTime.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float startTime = 10f;

AudioError err = am.PlayAtTimeStamp(soundName, startTime);
if (err != AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " at startTime: " + startTime.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " at startTime: " + startTime.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to play a sound but skip a portion at the start. Could be used if only the second part of your sound is high intesity and normally you want to build up the intensity, but not when the game is in a special state.


**Remarks:**
Uses [```SubscribeProgressCoroutine```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/subscribe_progress_coroutine/) under the hood to reset the ```startTime``` after the song finished playing,
meaning if we already subscribed the same song to the progress value of ```Constants.MAX_PROGRESS```, this method will fail. Or in reverse if we call this method and then subscribe to the progress value of ```Constants.MAX_PROGRESS``` that method will fail.

To circumvent that [```SubscribeProgressCoroutine```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/subscribe_progress_coroutine/) has to be called by the user beforehand and then in that method,
the below shown method has to be called if the name of the sound that has triggered the callback is the one we called ```PlayAtTimeStamp``` on.

```csharp
private ProgressResponse ResetStartTime(string name, float progress, ChildType child) {
    TryGetSource(name, out var source);
    // Stop the sound if it isn't set to looping,
    // this is done to ensure the sound doesn't replay,
    // when it is not set to looping.
    AudioError error = source.InvokeChild(child, (s) => s.loop, out bool looping);
    if (error == AudioError.OK && !looping) {
        Stop(name, child); 
    }
    SetStartTime(name, 0f, child);
    return ProgressResponse.UNSUB;
}
```		
