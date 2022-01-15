---
layout: default
title: Play Delayed
parent: AudioSource
grand_parent: Documentation
---

## Play Delayed
**What it does:**
Starts playing the choosen sound after the given amount of time and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound after the given amount of time failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play after the given amount of time which would be the 5 seconds we've defined
- ```Delay``` is the time after which we want to start playing the sound.

```csharp
string soundName = "SoundName";
float delay = 5f;

AudioError err = am.PlayDelayed(soundName, delay);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " seconds failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " seconds succesfull");
}
```

**When to use it:**
When you want to play a sound after a given delay time instead of directly when the method is called.

**Remarks:**
See [```AudioSource.PlayDelayed```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.PlayDelayed.html) for more details on what play delayed does.
