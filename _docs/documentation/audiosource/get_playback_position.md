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
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the playback position of

```csharp
string soundName = "SoundName";

AudioError error = am.GetPlaybackPosition(soundName, out float time);
if (error != AudioError.OK) {
    Debug.Log("Getting playBackPosition of the sound called: " + soundName + " failed with error id: " + error);
}
else {
    Debug.Log("Getting playBackPosition of the sound called: " + soundName + " with the position being: " + time.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When yu want to get the time the current amount of time the sound has been playing already.
