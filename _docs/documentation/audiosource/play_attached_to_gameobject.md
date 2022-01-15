---
layout: default
title: Play Attached To GameObject
parent: AudioSource
grand_parent: Documentation
---

### Play Attached To GameObject
**What it does:**
Starts playing the choosen sound attached to a ```gameObject``` and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how playing the sound attached to the given gameobject failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```GameObject``` is the object the sound is emitting from

```csharp
string soundName = "SoundName";
GameObject gameObject = this.gameObject;

AudioError err = am.PlayAttachedToGameObject(soundName, gameObject);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " attached to: " + gameObject.name + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " attached to: " + gameObject.name + " succesfull");
}
```

**When to use it:**
When you want to play a sound directly from a object and make the ```volume``` be influenced by the distance the player has from that object.

**Remarks:**
To use either [```PlayAt3DPosition```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/play_at_3d_position/) or [```PlayAttachedToGameObject```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/play_attached_to_gameobject/) the sound we want to play with it has to have 3D enabled. Either use [```Set3DAudioOptions```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/set_3d_audio_options/) to set the needed settings via. code or make the sound 3D capable with the ```Spatial Blend``` parameter on the ```AudioSourceSetting```.
