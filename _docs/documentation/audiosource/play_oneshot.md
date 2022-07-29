---
layout: default
title: Play OneShot
parent: AudioSource
grand_parent: Documentation
---

## Play OneShot
**What it does:**
Starts playing the choosen sound once and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound once failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play once
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
ChildType child = ChildType.PARENT;

AudioError err = am.PlayOneShot(soundName, child);
if (err != AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " once failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " once succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";

AudioError err = am.PlayOneShot(soundName);
if (err != AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " once failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " once succesfull");
}
```

**When to use it:**
When you want to only play a sound once. Having multiple instances of the same sound running at the same time is only possible with this method.

**Remarks:**
See [```AudioSource.PlayOneShot```](https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html) for more details on what play oneshot does.
