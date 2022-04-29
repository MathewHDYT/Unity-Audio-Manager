---
layout: default
title: Get Progress
parent: AudioSource
grand_parent: Documentation
---

## Get Progress
**What it does:**
Returns an instance of the ValueDataError class, where the value (gettable with ```Value```), is the ```progress``` of the given sound, which is a float from 0 to 1 and where the error (gettable with ```Error```) is an integer representing the AudioError Enum (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how getting the current  of ```progress``` of the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the progress from

```csharp
string soundName = "SoundName";

ValueDataError<float> valueDataError = am.GetProgress(soundName);
if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
    Debug.Log("Getting progress of the sound called: " + soundName + " failed with error id: " + valueDataError.Error);
}
else {
    Debug.Log("Getting progress of the sound called: " + soundName + " with the progress being: " + (valueDataError.Value * 100).ToString("0.00") + "% succesfull");
}
```

**When to use it:**
When you want to get the progress of a sound for an animation or to track once it's finished to start a new sound.
