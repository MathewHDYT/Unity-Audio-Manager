---
layout: default
title: Add Group
parent: AudioMixer
grand_parent: Documentation
---

## Remove Group
**What it does:**
Adds the given [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) to the given sound and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how adding the group failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to add the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) on
- ```MixerGroup``` is the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) we want to add

```csharp
string soundName = "SoundName";
AudioMixerGroup mixerGroup = null;

AudioError error = am.AddGroup(soundName, mixerGroup);
if (error != AudioManager.AudioError.OK) {
    Debug.Log("Adding AudioMixerGroup on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Adding AudioMixerGroup on the sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to add the influence and settings of a [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) from a sound. A sound can only ever be influenced by one [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) at a time.
