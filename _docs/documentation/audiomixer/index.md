---
layout: default
title: AudioMixer
parent: Documentation
has_children: true
---

## AudioMixer API
Starting from v1.3 see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager/releases/)).

All public accessible methods below communicate and depend on the [```AudioMixer```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixer.html) and [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) API. This is done so the AudioManager can adjust multiple AudioSource at once without the need to change them all one by one per code, instead simply one of the sounds that is in that [```AudioMixerGroup```](https://docs.unity3d.com/ScriptReference/Audio.AudioMixerGroup.html) can change the volume, pitch or any other defined exposed parameter. If a documentation of the underlying API call exists it is added in the given method description.

Additionaly this adds the possiblity to add distortion or other effects to the [```AudioSource```](https://docs.unity3d.com/ScriptReference/AudioSource.html).

See [Audio Tutorial for Unity AudioMixer](https://www.raywenderlich.com/532-audio-tutorial-for-unity-the-audio-mixer#toc-anchor-010) on how to expose parameters so that they can be changed with the AudioManager.
