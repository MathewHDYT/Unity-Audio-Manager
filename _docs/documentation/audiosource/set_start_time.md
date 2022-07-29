---
layout: default
title: Set Start Time
parent: AudioSource
grand_parent: Documentation
---

## Set Start Time
**What it does:**
Sets the ```startTime``` in seconds of the given sound and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how setting the given ```startTime``` failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- INVALID_TIME

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```StartTime``` is the moment we want to play the sound at so instead of starting at 0 seconds we start at 10 seconds
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
float startTime = 10f;
ChildType child = ChildType.PARENT;

AudioError err = am.SetStartTime(soundName, startTime, child);
if (err != AudioError.OK) {
    Debug.Log("Setting startTime for the sound called: " + soundName + " to the value: " + startTime.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Setting startTime for the sound called: " + soundName + " to the value: " + startTime.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float startTime = 10f;

AudioError err = am.SetStartTime(soundName, startTime);
if (err != AudioError.OK) {
    Debug.Log("Setting startTime for the sound called: " + soundName + " to the value: " + startTime.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Setting startTime for the sound called: " + soundName + " to the value: " + startTime.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to play a sound but skip a portion at the start. Could be used if only the second part of your sound is high intesity and normally you want to build up the intensity, but not when the game is in a special state.
Additionaly this method makes it possible to use another ```Play``` method of the AudioManager and still start a the given ```startTime```. 

**Remarks:**
Does not reset the ```startTime``` when playing the sound again use [```PlayAtTimeStamp```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/play_at_time_stamp/) for that.
