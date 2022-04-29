---
layout: default
title: Change Pitch
parent: AudioSource
grand_parent: Documentation
---

## Change Pitch
**What it does:**
Sets the ```pitch``` of the given sound to random value between the given min -and max values and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how chaging the ```pitch``` failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```MinPitch``` is the minimum amount of pitch the sound can be set to
- ```MaxPitch``` is the Maximum amount of pitch the sound can be set to

```csharp
string soundName = "SoundName";
float minPitch = 0.9f;
float maxPitch = 1.1f;

AudioError err = am.ChangePitch(soundName, minPitch, maxPitch);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Changing pitch for the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Changing pitch for the sound called: " + soundName + " with the given minimum pitch being: " + minPitch.ToString("0.00") + " and the given maximum pitch being: " + maxPitch.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to change the pitch to a random value so that a sound that is played often (footsteps, ui-hover, etc.) isn't as repetitive.
