---
layout: default
title: Lerp Volume
parent: AudioSource
grand_parent: Documentation
---

## Lerp Volume
**What it does:**
Lerps the ```volume``` of a given sound over a given amount of time and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how lerping the volume of the given sound failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- INVALID_END_VALUE
- MISSING_PARENT

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to change the volume from
- ```EndValue``` (0 - 1) is the value the ```volume``` should have at the end
- ```Duration``` defines the total amount of time needed to achieve the given ```endValue```
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
float endValue = 0.8f;
float duration = 1f;
ChildType child = ChildType.PARENT;

AudioError err = am.LerpVolume(soundName, endValue, duration, child);
if (err != AudioError.OK) {
    Debug.Log("Lerping volume of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping volume of the sound called: " + soundName + " in the time: " + duration.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float endValue = 0.8f;

AudioError err = am.LerpVolume(soundName, endValue);
if (err != AudioError.OK) {
    Debug.Log("Lerping volume of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping volume of the sound called: " + soundName + " to the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to decrease -or increase the ```volume``` over a given amount of time.

**Remarks:**
Inspired from John Leonard French's blog with an article about different methods to fade audio in and out.
See https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/.
As explained in the blog post as well the resolution of the fade is only as smooth as the fps count of the client that calls the method.
If a smoother lerp is wanted [```LerpGroupValue```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiomixer/lerp_group_value/) can be used,
because of additonaly interpolation between frames applied by the [```AudioMixer```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.html).
