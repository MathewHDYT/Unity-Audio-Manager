---
layout: default
title: Get Playback Position
parent: Documentation
---

## Get Playback Position
**What it does:**
Returns an instance of the ValueDataError class, where the value (gettable with ```Value```), is the current playback position of the given sound in seconds and where the error (gettable with ```Error```) is an integer representing the AudioError Enum (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/#possible-errors)), showing wheter and how getting the current playback position of the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the playback position of

```csharp
string soundName = "SoundName";

ValueDataError<float> valueDataError = am.GetPlaybackPosition(soundName);
if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
    Debug.Log("Getting playBackPosition of the sound called: " + soundName + " failed with error id: " + valueDataError.Error);
}
else {
    Debug.Log("Getting playBackPosition of the sound called: " + soundName + " with the position being: " + valueDataError.Value.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When yu want to get the time the current amount of time the sound has been playing already.
