---
layout: default
title: Stop
parent: AudioSource
grand_parent: Documentation
---

## Stop
**What it does:**
Stops the sound if it is currently playing and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how stopping the sound failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to stop
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
ChildType child = ChildType.PARENT;

AudioError err = am.Stop(soundName, child);
if (err != AudioError.OK) {
    Debug.Log("Stopping sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Stopping sound called: " + soundName + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";

AudioError err = am.Stop(soundName);
if (err != AudioError.OK) {
    Debug.Log("Stopping sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Stopping sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to stop the given sound, if you restart it later the sound will start anew. So to really only pause it use the [Toggle Pause method](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/toggle_pause/).

**Remarks:**
See [```AudioSource.Stop```](https://docs.unity3d.com/ScriptReference/AudioSource.Stop.html) for more details on what stop does.
