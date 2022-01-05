![Unity Audio Manager](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/blob/main/logo.png/)

[![MIT license](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](https://lbesson.mit-license.org/)
[![Unity](https://img.shields.io/badge/Unity-5.2%2B-green.svg?style=flat-square)](https://docs.unity3d.com/520/)
[![GitHub release](https://img.shields.io/github/release/MathewHDYT/Unity-Audio-Manager-UAM/all.svg?style=flat-square)](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)
[![GitHub downloads](https://img.shields.io/github/downloads/MathewHDYT/Unity-Audio-Manager-UAM/all.svg?style=flat-square)](https://github.com/MathewHDYT/Unity-Audio-Manager-UAM/releases/)

# Unity Audio Manager (UAM)
Used to play/change/stop/mute/... sounds at certain circumstances or events in 2D and 3D simply via. code.

## Introduction
Nearly all games need music and soundeffects and this small and easily integrated Audio Manager can help you play sounds in Unity for your game quick and easily. For each method there is a description on how to call it and how to use it correctly for your game in the given section in the documentation

## Installation
**Required Software:**
- [Unity](https://unity3d.com/get-unity/download) Ver. 2020.3.17f1

The Audio Manager itself is version independent, as long as the AudioSource object already exists. Additionally the example project can be opened with Unity itself or the newest release can be downloaded and exectued to test the functionality.

If you prefer the first method, you can simply install the shown Unity version and after installing it you can download the project and open it in Unity (see [Opening a Project in Unity](https://docs.unity3d.com/2021.2/Documentation/Manual/GettingStartedOpeningProjects.html)). Then you can start the game with the play button to test the Audio Managers functionality.

To simply use the Audio Manager in your own project without downloading the Unity project get the two files in the **Example Project/Assets/Scritps/** called ```AudioManager.CS```, ```Sound.CS``` and ```ValueData.cs``` or alternatively get them from the newest release (may not include the newest changes) and save them in your own project. Then create a new empty ```gameObject``` and attach the ```AudioManager.CS``` script to it. Now you can easily add sounds like shown in [Adding a new sound](#adding-a-new-sound).

# Documentation
The documentation can be found on the [GitHub Page](https://mathewhdyt.github.io/Unity-Audio-Manager/)
