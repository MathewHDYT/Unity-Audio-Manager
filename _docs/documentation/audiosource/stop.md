---
layout: default
title: Stop
parent: AudioSource
grand_parent: Documentation
---

## Stop
**What it does:**
Stops the sound if it is currently playing and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how stopping the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to stop

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
