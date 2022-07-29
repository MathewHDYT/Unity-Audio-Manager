---
layout: default
title: Toggle Pause
parent: AudioSource
grand_parent: Documentation
---

## Toggle Pause
**What it does:**
Completly pauses or unpauses playback of the given sound until it's toggled again
and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how pausing or unpausing the sound failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to toggle pause on / off
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
ChildType child = ChildType.PARENT;

AudioError err = am.TogglePause(soundName, child);
if (err != AudioError.OK) {
    Debug.Log("Pausing or unpausing sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Pausing or unpausing sound called: " + soundName + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";

AudioError err = am.TogglePause(soundName);
if (err != AudioError.OK) {
    Debug.Log("Pausing or unpausing sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Pausing or unpausing sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to pause or unpause a sound without stopping it completly so it can be later restarted at the stopped time.

**Remarks:**
See [```AudioSource.UnPause```](https://docs.unity3d.com/ScriptReference/AudioSource.UnPause.html) for more details on what unpause does and see [```AudioSource.Pause```](https://docs.unity3d.com/ScriptReference/AudioSource.Pause.html) for more details on what pause does.
