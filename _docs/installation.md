---
layout: default
title: Installation
nav_order: 3
---

## Reference to Audio Manager Script
To use the Audio Manager to start playing sounds outside of itself you need to reference it. As the Audio Manager is a [Singelton](https://stackoverflow.com/questions/2155688/what-is-a-singleton-in-c) this can be done easily when we get the instance and save it as a private variable in the script that uses the Audio Manager.

```csharp
private AudioManager am;

void Start() {
    am = AudioManager.instance;
    // Calling method in AudioManager
    am.Play("SoundName");
}
```

Alternatively you can directly call the methods this is not advised tough, if it is executed multiple times or you're going to need the instance multiple times in the same file.

```csharp
void Start() {
    AudioManager.Play("SoundName");
}
```

## AudioMixer support
Starting from ```v1.3``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)), the AudioManager now supports the [```AudioMixer```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixer.html), meaning sounds can be assigned an [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) which will make it possible to decrease the volume of multiple sounds with one call as long as they are in the same [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html).

Additonaly the [```AudioMixer```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixer.html), even makes it possible to add distortion or other effects to the sounds.

See [Audio Tutorial for Unity AudioMixer](https://www.raywenderlich.com/532-audio-tutorial-for-unity-the-audio-mixer#toc-anchor-010) on how to expose parameters so that they can be changed with the AudioManager.

## Adding a new sound
**To add a new sound you simply have to create a new element in the Sounds array with the properties:**
- ```Name``` (This is used to reference the sound in the Audio Manager so ensure it's unique)
- ```Mixer Group``` ([```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to)
- ```Clip``` (Audio that should be played when starting to play the sound, simply add a audio file that is saved in your Unity Project)
- ```Volume``` (How loud the sound is)
- ```Pitch``` (Distortion of the sound effect, set it to 1 if you wan't to ensure that it sounds like intended)
- ```Loop``` (Determines if the sound should be repeated automatically after finishing --> Usefull for a theme sound)

![Image of AudioManager Script](https://image.prntscr.com/image/X9_38TspTDGMwruRz1LCHA.png)

## Required Software
- [Unity](https://unity3d.com/get-unity/download) Ver. 2020.3.17f1

The Audio Manager itself is version independent, as long as the AudioSource object already exists. Additionally the example project can be opened with Unity itself or the newest release can be downloaded and exectued to test the functionality.

If you prefer the first method, you can simply install the shown Unity version and after installing it you can download the project and open it in Unity (see [Opening a Project in Unity](https://docs.unity3d.com/2021.2/Documentation/Manual/GettingStartedOpeningProjects.html)). Then you can start the game with the play button to test the Audio Managers functionality.

To simply use the Audio Manager in your own project without downloading the Unity project get the two files in the **Example Project/Assets/Scritps/** called ```AudioManager.CS```, ```Sound.CS``` and ```ValueData.cs``` or alternatively get them from the newest release (may not include the newest changes) and save them in your own project. Then create a new empty ```gameObject``` and attach the ```AudioManager.CS``` script to it. Now you can easily add sounds like shown in [Adding a new sound](#adding-a-new-sound).
