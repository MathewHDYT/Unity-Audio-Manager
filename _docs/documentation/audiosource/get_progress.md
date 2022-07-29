---
layout: default
title: Get Progress
parent: AudioSource
grand_parent: Documentation
---

## Get Progress
**What it does:**
Returns the ```progress``` of the given sound, which is a float from 0 to 1 and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how getting the current ```progress``` of the sound failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the progress from
- ```Progress``` is the ```variable``` the progress will be copied into ```float.NaN``` on failure
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
float progress = float.NaN;
ChildType child = ChildType.PARENT;

AudioError error = am.GetProgress(soundName, out progress, child);
if (error != AudioError.OK) {
    Debug.Log("Getting progress of the sound called: " + soundName + " failed with error id: " + error);
}
else {
    Debug.Log("Getting progress of the sound called: " + soundName + " with the progress being: " + (progress * 100).ToString("0.00") + "% succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float progress = float.NaN;

AudioError error = am.GetProgress(soundName, out progress);
if (error != AudioError.OK) {
    Debug.Log("Getting progress of the sound called: " + soundName + " failed with error id: " + error);
}
else {
    Debug.Log("Getting progress of the sound called: " + soundName + " with the progress being: " + (progress * 100).ToString("0.00") + "% succesfull");
}
```

**When to use it:**
When you want to get the progress of a sound for an animation or to track once it's finished to start a new sound.
