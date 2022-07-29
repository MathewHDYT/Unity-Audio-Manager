---
layout: default
title: Get Playback Position
parent: AudioSource
grand_parent: Documentation
---

## Get Playback Position
**What it does:**
Returns the current playback position of the given sound in seconds and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how getting the current playback position of the sound failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the playback position of
- ```Time``` is the ```variable``` the playback position in seconds will be copied into ```float.NaN``` on failure
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
float time = float.NaN;
ChildType child = ChildType.PARENT;

AudioError error = am.GetPlaybackPosition(soundName, out time, child);
if (error != AudioError.OK) {
    Debug.Log("Getting playBackPosition of the sound called: " + soundName + " failed with error id: " + error);
}
else {
    Debug.Log("Getting playBackPosition of the sound called: " + soundName + " with the position being: " + time.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float time = float.NaN;

AudioError error = am.GetPlaybackPosition(soundName, out time);
if (error != AudioError.OK) {
    Debug.Log("Getting playBackPosition of the sound called: " + soundName + " failed with error id: " + error);
}
else {
    Debug.Log("Getting playBackPosition of the sound called: " + soundName + " with the position being: " + time.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to get the time the current amount of time the sound has been playing already.
