---
layout: default
title: Play At Time Stamp
parent: AudioSource
grand_parent: Documentation
---

## Play At Time Stamp
**What it does:**
Start playing the choosen sound at the given ```startTime``` and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound from the given ```startTime``` failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```StartTime``` is the moment we want to play the sound at so instead of starting at 0 seconds we start at 10 seconds in the audio clip

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
