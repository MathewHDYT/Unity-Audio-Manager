---
layout: default
title: Audio Import Settings
nav_order: 10
permalink: /audio_import_settings
---


# Audio Import Settings
This page contains an explaination of when, which audio clip import settings should be used
and their respective advantages and disadvantages. It is ordered after the order of settings on the Audio Clip (Import Settings) page.
 
## [Force to Mono](https://youtu.be/eBJzP6ecUFU)
This section describes the difference between mono and stereo.

Generally speaking most often Stereo sounds are background music or ambience tracks where as sound effects most of the time are already mono or can be forced to it without any drawback.

### Mono
- Audio clip only has one channel that audio is played on, meaning that left and right share the same channel

#### Advantages
- Slightly more efficient than Stereo, because only one channel with audio data is needed

#### Disadvantages
- Forcing to mono can cause the audio to mute or degrade itself (*Phase Cancellation*), this happens because both channels contained frequencies that are complete or partial opposites

#### Remarks
> **Info**
> Sounds that have been forced to mono have the tendency to sound quieter, check and enable the **Normalize** option if needed

> **Warning**
> 3D sounds in Unity are automatically forced to mono, therefore they **Forced to mono** option should always be enabled for 3D sounds

### Stereo
- Audio clip only has two channels that audio is played on, meaning that left and right both have a seperate channel. Which makes it possible to make sound appear to come from either the left or right side or somewhere in between
- This is done via. the nearly same content in both channels, but differences in:
    - Level / Loudness
    - Time of Arrival
    - Frequency Content

#### Advantages
- Audio that has been made with Stereo in mind can sound much fuller

#### Disadvantages
- Slight overhead for both performance (CPU usage) and memory usage

#### Remarks
> **Warning**
> Stereo is the standard Audio format meaning that a lot of sounds are stereo even tough they might not contain any stereo information. Therefore always try to **Force to mono** and check if the sound changes in the first place

## [Load in Background](https://youtu.be/OGmt7pgFXXI)
If enabled, the audio clip will be loading  in the background without causing stalls on the main thread. Note that play requests on ```AudioClips``` that are still loading in the background will be deferred until the clip is done loading.

### Advantages
- Loading begins asynchronously once the ```AudioClip``` has been called

### Disadvantages
- Can cause out-of-sync sound depeding on the size of the ```AudioClip``` because of loading time, meaning if the audio was played directly on startup it will only play a few seconds later after the clip has been loaded

### Remarks
> **Info**
> Any ```AudioClips``` that are not needed right away in the scene and do not need to be played immediately after the play request has been sent. Should be marked as [**Load in Background**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#load-in-background).

> **Info**
> Any ```AudioClips``` that are not needed right away in the scene but do need to be played immediately after the play request has been sent. Should be marked as [**Load in Background**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#load-in-background) and as [**Preload Audio Data**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#preload-audio-data).

> **Warning**
> Enabling neither [**Load in Background**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#load-in-background) nor [**Preload Audio Data**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#preload-audio-data), causes frame freezes because the clip blocks the main thread until it is done loading

## [Load Type](https://youtu.be/T6Uag8VlMck)
This section describes the different load types that can be used to decrease the overall CPU or  memory usage of the sounds in your game.

As with nearly everything in software development this is a tradeoff between performance (CPU usage) and needed memory usage (Size in memory).

### Decompressed on Load
- Loads the Audio decoded / decompressed to [**PCM**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#pulse_code_modulation_(pcm)) into RAM

#### Advantages
- Audio is in a ready-to-play state
- **Very good for sounds that are played often and are small in size**

#### Disadvantages
- Audio requires much more space in memory

#### Remarks
> **Warning**
> Using this setting with compressed audio is counterproductive, because it will cause the game to still need the same amount of allocated memory and therefore waste it

### Compressed in Memory
- Loads the Audio with the given [**Compression Format**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#compression-format) into RAM

#### Advantages
- Audio is in a ready-to-play state
- Compromise between RAM CPU usage
- **Very good for sounds that are played somewhat often, but not enough to be always needed in a decoded / decompressed state**

#### Disadvantages
- Tiny overhead for storing compressed audio data (1 MB)
- Needs more CPU usage to be played

### Streaming
- Purposefully does not prime your audio for playback, instead it will be read, decoded and decompressed on the fly from the storage medium

#### Advantages
- Uses the least amount of memory (around 10% of the already compressed audio size)
- **Very good for sounds like background music that are very big and only one or two will play at the same time**

#### Disadvantages
- Highest CPU usage of all other options
- Disables [**Preload Audio Data**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#preload-audio-data) option

#### Remarks
> **Warning**
> It is generally not recommended to use more than one or two Audio Clips playing backs at the same time

## [Preload Audio Data](https://youtu.be/PaP5soZ3WWI)
If enabled, the audio clip will be pre-loaded when the scene is loaded. This is on by default to reflect standard Unity behaviour where all ```AudioClips``` have finished loading when the scene starts playing.

### Advantages
- Ensures that the audio clip is ready to be used as soon as the scene has started playing

### Disadvantages
- The scene needs to load the clips before it starts so loading the scene requires longer load times especially if a lot of big audio clips have to be loaded

### Remarks
> **Info**
> Any ```AudioClips``` that are needed right away in the scene should be marked with [**Preload Audio Data**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#preload-audio-data) **only** to ensure they are **playable from the first moment**

> **Info**
> Any ```AudioClips``` that are not needed right away in the scene but do need to be played immediately after the play request has been sent. Should be marked as [**Load in Background**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#load-in-background) and as [**Preload Audio Data**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#preload-audio-data).

> **Warning**
> Enabling neither [**Load in Background**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#load-in-background) nor [**Preload Audio Data**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#preload-audio-data), causes frame freezes, because the clip is not already loaded and ready to be played

## [Compression Format](https://youtu.be/4dr_1Yie4Os)
This section describes the different compression formats that can be used to decrease the overall size of the game.

### Pulse Code Modulation (PCM)

#### Advantages
- Lossless codec (Original quality)
- Nearly no CPU usage to playback (No decode / decompression)
- Unity adds loseless compression on some seemingly random files automatically (Undocumented feature)

#### Disadvantages
- Uncompressed file size (Problematic on mobile devices)

### Adaptive Differential Pulse Code Modulation (ADPCM)

#### Advantages
- 3.5x times smaller than PCM
- Light CPU usage to playback (decode / decompression)

#### Disadvantages
- Lossy codec
- Quantization distortion (Distortion / extra "noises") --> Happens often with high frequency sounds

#### Remarks
> **Warning**
> Always check ADPCM files after compressing

### Vorbis

#### Advantages
- Up to 20x times smaller than PCM
- Quality slider
- 70% quality compression is nearly impossible to differentiate

#### Disadvantages
- Lossy codec
- High CPU usage to playback (decode / decompression)

## [Sample Rate Settings](https://youtu.be/dRSNyw7DRUs)
This section describes the different saple rate settings that can be used to , meaning the volume information of a particular time in an audio clip.    

### Preserve Sample Rate

#### Remarks
> **Info**
> If the audio clip has already been optimized for sample rate, which most often is the case, then this option can simply be used and even if it hasn't been in nearly all cases it is better to to use proper audio software to downsample, because Unity's sample rate convertor is not particulary good.

### Optimize Sample Rate

#### Advantages
- Saves space because Unity attempts to detect the highest used frequency in the audio clip and downsample the audio to that frequency times two with some leeway

#### Disadvantages
- Might cause **Audio Aliasing**, because the conversion was not good enough and caused artifacts in the audio clip

#### Remarks
> **Warning**
> Always check audio clip files after setting the [**Optimize Sample Rate**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#optimize-sample-rate) option and when there is **Audio Aliasing** use the [**Override Sample Rate**](https://mathewhdyt.github.io/Unity-Audio-Manager/audio_import_settings#optimize-sample-rate) option with a similair but slightly higher sample rate  instead until the **Audio Aliasing** can not be heard anymore

### Override Sample Rate

#### Advantages
- Saves space because we can set the sample rate ourselves to the actual sample rate needed by the audio clip

#### Disadvantages
- Might need trial and error to adjust the sample rate to the correct amount
- Oversampling will not increase the quality and only increase the file size
