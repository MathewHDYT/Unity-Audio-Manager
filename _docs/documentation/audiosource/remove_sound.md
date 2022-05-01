---
layout: default
title: Remove Sound
parent: AudioSource
grand_parent: Documentation
---

## Remove Sound
**What it does:**
Remove and deregisters a sound with the given name from the AudioManager and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how removing the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to remove

```csharp
string soundName = "SoundName";

AudioError err = am.RemoveSound(soundName);
if (err != AudioError.OK) {
    Debug.Log("Removing sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Removing sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to remove a sound, because it will not be called anymore.
