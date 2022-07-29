---
layout: default
title: Toggle Mute
parent: AudioSource
grand_parent: Documentation
---

## Toggle Mute
**What it does:**
Sets the ```volume``` of the sound to 0 and resets it to its inital value if called again
and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how muting or unmuting the sound failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to toggle mute on / off
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
ChildType child = ChildType.PARENT;

AudioError err = am.ToggleMute(soundName, child);
if (err != AudioError.OK) {
    Debug.Log("Muting or unmuting sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Muting or unmuting sound called: " + soundName + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";

AudioError err = am.ToggleMute(soundName);
if (err != AudioError.OK) {
    Debug.Log("Muting or unmuting sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Muting or unmuting sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to completly silence a sound and still keep it playing in the background. For example if you have a radio channel with a mute or switch channel button.

**Remarks:**
See [```AudioSource.mute```](https://docs.unity3d.com/ScriptReference/AudioSource-mute.html) for more details on what toggle mute does.
