---
layout: default
title: Home
nav_order: 1
---

#Unity Audio Manager (UAM)
Used to play/change/stop/mute/... sounds at certain circumstances or events in 2D and 3D simply via. code.

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

## Possible Errors

| **ID** | **CONSTANT**                  | **MEANING**                                                                                    |
| -------| ------------------------------| -----------------------------------------------------------------------------------------------|
| 0      | OK                            | Method succesfully executed                                                                    |
| 1      | DOES_NOT_EXIST                | Sound has not been registered with the AudioManager                                            |
| 2      | FOUND_MULTIPLE                | Multiple instances with the same name found. First will be played                              |
| 3      | ALREADY_EXISTS                | Can't add sound as there already exists a sound with that name                                 |
| 4      | INVALID_PATH                  | Can't add sound because the path does not lead to a valid audio clip                           |
| 5      | SAME_AS_CURRENT               | The given endValue is already the same as the current value                                    |
| 6      | TOO_SMALL                     | The given granularity is too small, has to be higher than or equal to 1                        |
| 7      | NOT_EXPOSED                   | The given parameter in the AudioMixer is not exposed or does not exist                         |
| 8      | MISSING_SOURCE                | Sound does not have an AudioSource component on the GameObject the AudioManager resides on     |
| 9      | MISSING_MIXER_GROUP           | Group methods may only be called with a sound that has a set AudioMixerGroup                   |

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
