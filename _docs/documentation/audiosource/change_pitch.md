---
layout: default
title: Change Pitch
parent: AudioSource
grand_parent: Documentation
---

## Change Pitch
**What it does:**
Sets the ```pitch``` of the given sound to random value between the given min -and max values and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how chaging the ```pitch``` failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```MinPitch``` is the minimum amount of pitch the sound can be set to
- ```MaxPitch``` is the maximum amount of pitch the sound can be set to
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
float minPitch = 0.9f;
float maxPitch = 1.1f;
ChildType child = ChildType.PARENT;

AudioError err = am.ChangePitch(soundName, minPitch, maxPitch, child);
if (err != AudioError.OK) {
    Debug.Log("Changing pitch for the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Changing pitch for the sound called: " + soundName + " with the given minimum pitch being: " + minPitch.ToString("0.00") + " and the given maximum pitch being: " + maxPitch.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float minPitch = 0.9f;
float maxPitch = 1.1f;

AudioError err = am.ChangePitch(soundName, minPitch, maxPitch);
if (err != AudioError.OK) {
    Debug.Log("Changing pitch for the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Changing pitch for the sound called: " + soundName + " with the given minimum pitch being: " + minPitch.ToString("0.00") + " and the given maximum pitch being: " + maxPitch.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to change the pitch to a random value so that a sound that is played often (footsteps, ui-hover, etc.) isn't as repetitive.
