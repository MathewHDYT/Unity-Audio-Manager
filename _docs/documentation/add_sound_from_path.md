---
layout: default
title: Add Sound From Path
parent: Documentation
---

## Play At Time Stamp
**What it does:**
Adds the given sound to the list of possible playable sounds and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/#possible-errors)), showing wheter and how adding the sound from the given path failed.

**How to call it:**
- ```SoundName``` is the ```name``` the new sound should have
- ```Path``` is the path to the ```AudioClip``` we want to add to the new sound in the Resource folder
- ```Volume``` is the volume we want the new sound to have
- ```Pitch``` is the pitch we want the new sound to have
- ```Loop``` defines wheter we want to repeat the new sound after completing it or not
- ```Source``` is the ```AudioSource``` object we want to add to the new sound
- ```MixerGroup``` is the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to

```csharp
string soundName = "SoundName";
string path = "Audio/audioClip01";
float volume = 1f;
float pitch = 1f;
bool loop = false;
AudioSource source = null;
AudioMixerGroup mixerGroup = null;

AudioManager.AudioError err = am.AddSoundFromPath(soundName, path, volume, pitch, loop, source, mixerGroup);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Adding the sound called: " + soundName + " from the given path failed with error id: " + err);
}
else {
    Debug.Log("Adding the sound called: " + soundName + " from the given path succesfull");
}
```
Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
string path = "Audio/audioClip01";

AudioManager.AudioError err = am.AddSoundFromPath(soundName, path);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Adding the sound called: " + soundName + " from the given path failed with error id: " + err);
}
else {
    Debug.Log("Adding the sound called: " + soundName + " from the given path succesfull");
}
```

**When to use it:**
When you want to add a new sound at runtime, could be useful if you need to add a lot of songs and don't want to add them manually through the GameObject the Audio Manager script resides on.
