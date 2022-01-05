---
layout: default
title: Installation
nav_order: 3
---

## Installation
**Required Software:**
- [Unity](https://unity3d.com/get-unity/download) Ver. 2020.3.17f1

The Audio Manager itself is version independent, as long as the AudioSource object already exists. Additionally the example project can be opened with Unity itself or the newest release can be downloaded and exectued to test the functionality.

If you prefer the first method, you can simply install the shown Unity version and after installing it you can download the project and open it in Unity (see [Opening a Project in Unity](https://docs.unity3d.com/2021.2/Documentation/Manual/GettingStartedOpeningProjects.html)). Then you can start the game with the play button to test the Audio Managers functionality.

To simply use the Audio Manager in your own project without downloading the Unity project get the two files in the **Example Project/Assets/Scritps/** called ```AudioManager.CS```, ```Sound.CS``` and ```ValueData.cs``` or alternatively get them from the newest release (may not include the newest changes) and save them in your own project. Then create a new empty ```gameObject``` and attach the ```AudioManager.CS``` script to it. Now you can easily add sounds like shown in [Adding a new sound](#adding-a-new-sound).
