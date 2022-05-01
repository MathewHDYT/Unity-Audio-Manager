---
layout: default
title: Installation
nav_order: 3
permalink: /installation
---

## Integration

### Example project

To test the example project simply open the continuously integrated [Itch.io build](https://mathewhd.itch.io/unity-audio-manager) and either run the WebGL build in your Browser directly or download the Windows build.

You can also install the complete Example project from the ```main``` branch for the given Unity version see ([Required Software](https://mathewhdyt.github.io/Unity-Audio-Manager/installation#required-software)) and after installing it you can download the project and open it in Unity (see [Opening a Project in Unity](https://docs.unity3d.com/2021.2/Documentation/Manual/GettingStartedOpeningProjects.html)). Then you can start the game with the play button to test the AudioManagers functionalities.

### Using AudioManager

To simply use the AudioManager in your own project download the latest release.
    
Starting from ```v1.5.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)) simply open Unity and install the package from the [local folder](https://docs.unity3d.com/Manual/upm-ui-local.html) or alternatively add the package via [git url](https://docs.unity3d.com/Manual/upm-git.html#syntax).

In older version copy all files included in the release into your ```Assets``` folder.

### [Git package url](https://github.com/MathewHDYT/Unity-Audio-Manager.git?path=/com.mathewhdyt.audiomanager#package)

Once you've completed this step you can now you can easily add sounds like shown in [Adding a new sound](https://mathewhdyt.github.io/Unity-Audio-Manager/installation#adding-a-new-sound).

## Initalizing Audio Manager

Starting from ```v1.7.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)) the project structure has been completly reworked. Therefore intializing the Audio Manager now entails, adding the ```AudioManagerSettings.CS``` script onto a ```GameObject``` in your scene.

### Old Way of initalizing Audio Manager before v1.7.0

Simply attach the ```AudioManger.CS``` script onto a local ```GameObject``` in your scene.

## Reference to Audio Manager script

Starting from ```v1.7.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)) the project structure has been completly reworked. It now uses the ```Service Locator``` pattern, see ([Design](https://mathewhdyt.github.io/Unity-Audio-Manager/design#service-locator-pattern)) for more info on what that exactly means, therefore using the Audio Manager in your project has been slightly overhauled.

To include and use it in a script first include the needed namespaces. For a simple project that would be ```AudioManager.Core``` and ```AudioManager.Locator```. Then simply add a local private variable to hold the ```IAudioManager``` instance and then get it with the ```ServiceLocator.GetService()``` method.

```csharp
using AudioManager.Locator;
using AudioManager.Core;

private IAudioManager am;

private void Start() {
    am = ServiceLocator.GetService();
    // Calling method in AudioManager
	am.Play("SoundName");
}
```

Alternatively you can directly call the methods this is not advised tough, if it is executed multiple times or you're going to need the instance multiple times in the same file.

```csharp
using AudioManager.Locator;

void Start() {
    // Calling method in AudioManager
    ServiceLocator.GetService().Play("SoundName");
}
```

For a more detailed description on all the namespaces and for what specifically they are needed see ([Design](https://mathewhdyt.github.io/Unity-Audio-Manager/design#namespaces)).

### Old Way to add new sounds before v1.7.0

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
    // Calling method in AudioManager
    AudioManager.Play("SoundName");
}
```

## AudioMixer support

Starting from ```v1.3.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)), the AudioManager now supports the [```AudioMixer```](https://docs.unity3d.com/Documentation/ScriptReference/Audio.AudioMixer.html), meaning sounds can be assigned an [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) which will make it possible to decrease the volume of multiple sounds with one call as long as they are in the same [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html).

Additonally the [```AudioMixer```](https://docs.unity3d.com/Documentation/ScriptReference/Audio.AudioMixer.html), even makes it possible to add distortion or other effects to the sounds.

See [Audio Tutorial for Unity AudioMixer](https://www.raywenderlich.com/532-audio-tutorial-for-unity-the-audio-mixer#toc-anchor-010) on how to expose parameters so that they can be changed with the AudioManager.

## Adding a new sound

Starting from ```v1.4.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)), the AudioManager now relies on ```ScriptableObjects``` called ```AudioSourceSettings``` with custom UnityEditor code to initaly create a sound. For that you simply create as many ```AudioSourceSetting``` as you want too.

This can be done in any folder in the Assets, simply **Right-Click --> Create --> AudioManager --> AudioSourceSettings**. You can now give your ```ScriptableObject``` a name and set its values.

To remark is that the inital state of the ```AudioSourceSettings``` is simply 2D functionality.

To enable 3D functionality you have to increase the ```Spatial Blend``` setting to more than 0, because this signals how much your sound should be treated as 3D. Where 0 is effectively 2D. Therefore when you increase this to more than 0 additional settings for 3D functionality appear.

Image of 2D ```AudioSourceSettings```                                                                                                          |   Image of 3D ```AudioSourceSettings```
:-------------------------:                                                                                                                    |   :-------------------------:
![Image of 2D ```AudioSourceSettings```](https://raw.githubusercontent.com/MathewHDYT/Unity-Audio-Manager/gh-pages/_images/2d_audiosourcesetting.png)    |   ![Image of 3D ```AudioSourceSettings```](https://raw.githubusercontent.com/MathewHDYT/Unity-Audio-Manager/gh-pages/_images/3d_audiosourcesetting.png)

Lastly the ```ScriptableObjects``` then have to be dragged into the list on the ```AudioManagerSettings.CS``` script if using ```v1.7.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)) or into ```AudioManager.CS``` if using an older version.
![Image of 2D ```AudioSourceSettings```](https://raw.githubusercontent.com/MathewHDYT/Unity-Audio-Manager/gh-pages/_images/AudioManager.png)

### Old Way to add new sounds before v1.4.0

**To add a new sound you simply have to create a new element in the Sounds array with the properties:**
- ```Name``` (This is used to reference the sound in the Audio Manager so ensure it's unique)
- ```Mixer Group``` ([```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to)
- ```Clip``` (Audio that should be played when starting to play the sound, simply add a audio file that is saved in your Unity Project)
- ```Volume``` (How loud the sound is)
- ```Pitch``` (Distortion of the sound effect, set it to 1 if you wan't to ensure that it sounds like intended)
- ```Loop``` (Determines if the sound should be repeated automatically after finishing --> Usefull for a theme sound)

![Image of AudioManager Script](https://raw.githubusercontent.com/MathewHDYT/Unity-Audio-Manager/gh-pages/_images/old_audiomanager.png)

## Required Software
- [Unity](https://unity3d.com/get-unity/download/archive) minimum ```Ver. 2021.3.1f1``` for downloading and opening the example project in Unity
- [Unity](https://unity3d.com/get-unity/download/archive) minimum ```Ver. 5.2``` for the AudioManager
- [Unity](https://unity3d.com/get-unity/download/archive) minimum ```Ver. 2018.1```, when the AudioManager should be integrated via. the package. Possible starting from ```v1.5.0``` see ([GitHub release](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/))

### Certain parts of this library need different versions:

- ```Ver. 2021.3.1f1```--> Because the Example project was made in that version and downgrading to a later version is not possible.
- ```Ver. 2018.1``` --> Because the integration of the package ui was in ```Ver. 2018.1```, so before that you can't install custom packages.
- ```Ver. 5.2``` --> Because the AudioManager code itself relies only on min. ```Ver. 5.2```, because this was when support for both [AudioMixer](https://docs.unity3d.com/520/Documentation/Manual/AudioMixer.html) and [AudioSource](https://docs.unity3d.com/520/Documentation/Manual/class-AudioSource.html) were first added.
