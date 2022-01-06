---
layout: default
title: Toggle Pause
parent: AudioSource
---

## Toggle Pause
**What it does:**
Completly pauses or unpauses playback of the given sound until it is toggled again and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how pausing or unpausing the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to toggle pause on / off

```csharp
string soundName = "SoundName";

AudioManager.AudioError err = am.TogglePause(soundName);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Pausing or unpausing sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Pausing or unpausing sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to pause or unpause a sound without stopping it completly so it can be later restarted at the stopped time.

See [```AudioSource.UnPause```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.UnPause.html) for more details on what unpause does and see [```AudioSource.Pause```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.Pause.html) for more details on what pause does.
