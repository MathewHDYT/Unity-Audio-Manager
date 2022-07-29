---
layout: default
title: Play
parent: AudioSource
grand_parent: Documentation
---

## Play
**What it does:**
Starts playing the choosen sound and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
ChildType child = ChildType.PARENT;

AudioError err = am.Play(soundName, child);
if (err != AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";

AudioError err = am.Play(soundName);
if (err != AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to play a sound directly without changing its initally properties.

**Remarks:**
If you want to enable looping for a sound (see [Adding a new sound](https://mathewhdyt.github.io/Unity-Audio-Manager/installation#adding-a-new-sound)) for more information.
See [```AudioSource.Play```](https://docs.unity3d.com/ScriptReference/AudioSource.Play.html) for more details on what play does.
