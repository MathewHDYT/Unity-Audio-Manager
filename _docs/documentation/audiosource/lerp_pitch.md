---
layout: default
title: Lerp Pitch
parent: AudioSource
grand_parent: Documentation
---

## Lerp Pitch
**What it does:**
Lerps the ```pitch``` of a sound over a given amount of time and returns an AudioError (see [Possible Errors](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)), showing wheter and how lerping the pitch of the given sound failed.

[**Possible Errors:**](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-errors)
- DOES_NOT_EXIST
- MISSING_WRAPPER
- MISSING_SOURCE
- MISSING_CLIP
- INVALID_END_VALUE
- MISSING_PARENT

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to change the pitch from
- ```EndValue``` (0.1 - 3) is the value the ```pitch``` should have at the end
- ```Duration``` defines the total amount of time needed to achieve the given ```endValue```
- ```Child``` is the [```ChildType```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/index/#possible-children) that we want to call this method on

```csharp
string soundName = "SoundName";
float endValue = 0.8f;
float duration = 1f;
ChildType child = ChildType.PARENT;

AudioError err = am.LerpPitch(soundName, endValue, duration, child);
if (err != AudioError.OK) {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " in the time: " + duration.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float endValue = 0.8f;

AudioError err = am.LerpPitch(soundName, endValue);
if (err != AudioError.OK) {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " to the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to decrease -or increase the ```pitch``` over a given amount of time.

**Remarks:**
Inspired from John Leonard French's blog with an article about different methods to fade audio in and out.
See https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/.
As explained in the blog post as well the resolution of the fade is only as smooth as the fps count of the client that calls the method.
If a smoother lerp is wanted [```LerpGroupValue```](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiomixer/lerp_group_value/) can be used,
because of additonaly interpolation between frames applied by the [```AudioMixer```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.html).
