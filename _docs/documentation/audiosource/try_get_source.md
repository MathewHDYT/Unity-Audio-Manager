---
layout: default
title: Try Get Source
parent: AudioSource
grand_parent: Documentation
---

## Try Get Source
**What it does:**
Returns the ```source``` of the given sound, as well as an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how getting the source of the given sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the source from

```csharp
string soundName = "SoundName";
AudioSource source = default;

AudioError err = am.TryGetSource(soundName, out source);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Getting source of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Getting source of the sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to directly change the values of the given sound yourself and affect it while it's playing.
