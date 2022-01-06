---
layout: default
title: Play
parent: AudioSource
grand_parent: Documentation
---

## Play
**What it does:**
Starts playing the choosen sound and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play

```csharp
string soundName = "SoundName";

AudioManager.AudioError err = am.Play(soundName);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to play a sound directly without changing its initally properties.

If you want to enable looping for a sound (see [Adding a new sound](https://mathewhdyt.github.io/Unity-Audio-Manager/installation#adding-a-new-sound)) for more information.

See [```AudioSource.Play```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.Play.html) for more details on what play does.
