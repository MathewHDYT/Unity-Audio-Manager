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
- ```MinDistance``` is the distance the sound will not get louder at
- ```MaxDistance``` is the distance the sound will still be hearable at
- ```Spread``` is the angle the sound will be emitted at in degrees
- ```SpatialBlend``` defines how much the sound is affected by 3D (0f = 2D, 1f = 3D)
- ```DopplerLevel``` defines the doppler scale for our sound
- ```RolloffMode``` defines how the sound should decline in ```volume``` between the min and max distance

```csharp
string soundName = "SoundName";
GameObject gameObject = this.gameObject;
float minDistance = 5f;
float maxDistance = 15f;
float spread = 0f;
float spatialBlend = 1f;
float dopplerLevel = 1f;
AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;

AudioManager.AudioError err = am.PlayAttachedToGameObject(soundName, gameObject, minDistance, maxDistance, spread, spatialBlend, dopplerLevel, rolloffMode);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " attached to: " + gameObject.name + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " attached to: " + gameObject.name + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
GameObject gameObject = this.gameObject;
float minDistance = 5f;
float maxDistance = 15f;

AudioManager.AudioError err = am.PlayAttachedToGameObject(soundName, gameObject, minDistance, maxDistance);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " attached to: " + gameObject.name + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " attached to: " + gameObject.name + " succesfull");
}
```

**When to use it:**
When you want to play a sound directly from a object and make the ```volume``` be influenced by the distance the player has from that object.
