---
layout: default
title: Play OneShot
parent: AudioSource
grand_parent: Documentation
---

## Play OneShot
**What it does:**
Starts playing the choosen sound once and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound once failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play once

```csharp
string soundName = "SoundName";

AudioError err = am.PlayOneShot(soundName);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " once failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " once succesfull");
}
```

**When to use it:**
When you want to only play a sound once. Having multiple instances of the same sound running at the same time is only possible with this method.

**Remarks:**
See [```AudioSource.PlayOneShot```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.PlayOneShot.html) for more details on what play oneshot does.
