---
layout: default
title: Toggle Mute
parent: Documentation
---

## Toggle Mute
**What it does:**
Sets the ```volume``` of the sound to 0 and resets it to it's initally value if called again and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how muting or unmuting the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to toggle mute on / off

```csharp
string soundName = "SoundName";

AudioManager.AudioError err = am.ToggleMute(soundName);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Muting or unmuting sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Muting or unmuting sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to completly silence a sound and still keep it playing in the background. For example if you have a radio channel with a mute or switch channel button.
