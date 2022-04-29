---
layout: default
title: Remove Group
parent: AudioMixer
grand_parent: Documentation
---

## Remove Group
**What it does:**
Remove the [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) of a given sound and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how removing the group failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to remove the [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) on

```csharp
string soundName = "SoundName";

AudioError error = am.RemoveGroup(soundName);
if (error != AudioManager.AudioError.OK) {
    Debug.Log("Removing AudioMixerGroup on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Removing AudioMixerGroup on the sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to remove the influence and settings of a [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) from a sound.
