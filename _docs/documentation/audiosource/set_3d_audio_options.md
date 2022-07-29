---
layout: default
title: Set 3D Audio Options
parent: AudioSource
grand_parent: Documentation
---

## Set 3D Audio Options
**What it does:**
Enables and sets the possible 3D audio options needed and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how setting the 3D audio options failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- CAN_NOT_BE_3D

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```MinDistance``` is the distance the sound will not get louder at
- ```MaxDistance``` is the distance the sound will still be hearable at
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on
- ```SpatialBlend``` defines how much the sound is affected by 3D (0f = 2D, 1f = 3D)
- ```Spread``` is the angle the sound will be emitted at in degrees (0f - 360f)
- ```DopplerLevel``` defines the doppler scale for our sound (0f - 5f)
- ```RolloffMode``` defines how the sound should decline in ```volume``` between the min and max distance

```csharp
string soundName = "SoundName";
float minDistance = 1f;
float maxDistance = 500f;
ChildType child = ChildType.PARENT;
float spatialBlend = 1f;
float spreadAngle = 0f;
float dopplerLevel = 1f;
AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

AudioError err = am.Set3DAudioOptions(soundName, minDistance, maxDistance, child, spatialBlend, spreadAngle, dopplerLevel, rolloffMode);
if (err != AudioError.OK) {
    Debug.Log("Setting 3D audio options for the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Setting 3D audio options for the sound called: " + soundName + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";

AudioError err = am.PlayAt3DPosition(soundName);
if (err != AudioError.OK) {
    Debug.Log("Setting 3D audio options for the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Setting 3D audio options for the sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to make a 2D sound gain 3D capabilities via. code instead of the ```ScriptableObject```.
