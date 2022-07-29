---
layout: default
title: Deregister Child
parent: AudioSource
grand_parent: Documentation
---

## Deregister Child
**What it does:**
Deregisters and deletes the underlying AudioSource component of a previously registered child sound.
and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how deregistering the child failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- INVALID_CHILD

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to remove
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to deregister

```csharp
string soundName = "SoundName";
ChildType child = ChildType.AT_3D_POS;

AudioError err = am.DeregisterChild(soundName, child);
if (err != AudioError.OK) {
    Debug.Log("Deregistering child of sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Deregistering child of sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to remove a registered child of a given sound, because it is not needed anymore. Also deletes the underlying ```AudioSource``` component of that child.
