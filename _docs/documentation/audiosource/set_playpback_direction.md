---
layout: default
title: Set Playback Direction
parent: AudioSource
grand_parent: Documentation
---

## Set Playback Direction
**What it does:**
Sets the given direction the song should be played in. A given pitch of 0 or more means it is a normal song and should just be played with the given pitch value from the start. Less than 0 means that the song will play in reverse from the end of the song and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how setting the playback direction failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_SOURCE
- MISSING_CLIP

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to set the playback direction of
- ```Pitch``` decides the direction and speed we play the song at, less than 0 means it will be played in reverse while 0 more will mean it will be played normally

```csharp
string soundName = "SoundName";
floatch pitch = -1f;

AudioError err = am.SetPlaypbackDirection(soundName, pitch);
if (err != AudioError.OK) {
    Debug.Log("Setting playback direction for sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Setting playback direction for sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to play a song in reverse or change its pitch.
