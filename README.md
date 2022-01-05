![Unity Audio Manager](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/blob/main/logo.png/)

[![MIT license](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](https://lbesson.mit-license.org/)
[![Unity](https://img.shields.io/badge/Unity-5.2%2B-green.svg?style=flat-square)](https://docs.unity3d.com/520/)
[![GitHub release](https://img.shields.io/github/release/MathewHDYT/Unity-Audio-Manager-UAM/all.svg?style=flat-square)](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)
[![GitHub downloads](https://img.shields.io/github/downloads/MathewHDYT/Unity-Audio-Manager-UAM/all.svg?style=flat-square)](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)

# Unity Audio Manager (UAM)
Used to play/change/stop/mute/... sounds at certain circumstances or events in 2D and 3D simply via. code.

## Contents
- [Unity Audio Manager (UAM)](#unity-audio-manager-uam)
  - [Contents](#contents)
  - [Introduction](#introduction)
  - [Installation](#installation)
- [Documentation](#documentation)
  - [Reference to Audio Manager Script](#reference-to-audio-manager-script)
  - [Possible Errors](#possible-errors)
  - [AudioMixer support](#audiomixer-support)
  - [Adding a new sound](#adding-a-new-sound)
  - [Public accesible methods](#public-accesible-methods)
  	- [Add Sound From Path method](#add-sound-from-path-method)
  	- [Play method](#play-method)
  	- [Play At Time Stamp method](#play-at-time-stamp-method)
  	- [Get Playback Position method](#get-playback-position-method)
  	- [Play At 3D Position method](#play-at-3d-position-method)
  	- [Play Attached To GameObject method](#play-attached-to-gameobject-method)
  	- [Play Delayed method](#play-delayed-method)
  	- [Play OneShot method](#play-oneshot-method)
  	- [Play Scheduled method](#play-scheduled-method)
  	- [Stop method](#stop-method)
  	- [Toggle Mute method](#toggle-mute-method)
  	- [Toggle Pause method](#toggle-pause-method)
  	- [Get Progress method](#get-progress-method)
  	- [Try Get Source method](#try-get-source-method)
  	- [Lerp Pitch method](#lerp-pitch-method)
  	- [Lerp Volume method](#lerp-volume-method)
  	- [Change Group Value method](#change-group-value-method)
  	- [Get Group Value method](#get-group-value-method)
  	- [Reset Group Value method](#reset-group-value-method)
  	- [Lerp Group Value method](#lerp-group-value-method)

## Introduction
Nearly all games need music and soundeffects and this small and easily integrated Audio Manager can help you play sounds in Unity for your game quick and easily.

**Unity Audio Manager implements the following methods consisting of a way to:**
- Add a new possible sound to play at runtime (see [Add Sound From Path method](#add-sound-from-path-method))
- Simply play a sound (see [Play method](#play-method))
- Start playing a sound at a given time in the sound (see [Play At Time Stamp method](#play-at-time-stamp-method))
- Get the amount of time a sound has been played (see [Get Playback Position method](#get-playback-position-method))
- Play a sound at a 3D position (see [Play At 3D Position method](#play-at-3d-position-method))
- Play a sound attached to a gameobject (see [Play Attached To GameObject method](#play-attached-to-gameobject-method))
- Play a sound after a certain delay time (see [Play Delayed method](#play-delayed-method))
- Play a sound once (see [Play OneShot method](#play-oneshot-method))
- Play a sound at a given time in the time line (see [Play Scheduled method](#play-scheduled-method))
- Stop a sound (see [Stop method](#stop-method))
- Mute or unmute a sound (see [Toggle Mute method](#toggle-mute-method))
- Pause or unpause a sound (see [Toggle Pause method](#toggle-pause-method)
- Get the progress of a sound (see [Get Progress method](#get-progress-method))
- Try to get the source of a sound (see [Try Get Source method](#try-get-source-method))
- Lerp the pitch of a sound over a given time (see [Lerp Pitch method](#lerp-pitch-method))
- Lerp the volume of a sound over a given time (see [Lerp Volume method](#lerp-volume-method))
- Change the value of the given exposed parameter of a given sound to the given newValue (see [Change Group Value method](#change-group-value-method))
- Get the value of the given exposed parameter of a given sound (see [Get Group Value method](#get-group-value-method))
- Reset the value of the given exposed parameter of a given sound (see [Reset Group Value method](#reset-group-value-method))
- Lerp the value of the given exposed parameter of a given sound over a given time (see [Lerp Group Value method](#lerp-group-value-method))

For each method there is a description on how to call it and how to use it correctly for your game in the given section.

## Installation
**Required Software:**
- [Unity](https://unity3d.com/get-unity/download) Ver. 2020.3.17f1

The Audio Manager itself is version independent, as long as the AudioSource object already exists. Additionally the example project can be opened with Unity itself or the newest release can be downloaded and exectued to test the functionality.

If you prefer the first method, you can simply install the shown Unity version and after installing it you can download the project and open it in Unity (see [Opening a Project in Unity](https://docs.unity3d.com/2021.2/Documentation/Manual/GettingStartedOpeningProjects.html)). Then you can start the game with the play button to test the Audio Managers functionality.

To simply use the Audio Manager in your own project without downloading the Unity project get the two files in the **Example Project/Assets/Scritps/** called ```AudioManager.CS``` and ```Sound.CS``` or alternatively get them from the newest release (may not include the newest changes) and save them in your own project. Then create a new empty ```gameObject``` and attach the ```AudioManager.CS``` script to it. Now you can easily add sounds like shown in [Adding a new sound](#adding-a-new-sound).

# Documentation
The documentation can be found on the [GitHub Page](https://mathewhdyt.github.io/Unity-Audio-Manager/)
