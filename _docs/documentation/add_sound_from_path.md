---
layout: default
title: Add Sound From Path
parent: Documentation
nav_order: 2
---

**What it does:**
Adds the given sound to the list of possible playable sounds and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how adding the sound from the given path failed.

**How to call it:**
- ```SoundName``` is the ```name``` the new sound should have
- ```Path``` is the path to the ```AudioClip``` we want to add to the new sound in the Resource folder
- ```Volume``` is the volume we want the new sound to have
- ```Pitch``` is the pitch we want the new sound to have
- ```Loop``` defines wheter we want to repeat the new sound after completing it or not
- ```Source``` is the ```AudioSource``` object we want to add to the new sound
- ```MixerGroup``` is the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to

```csharp
string soundName = "SoundName";
string path = "Audio/audioClip01";
float volume = 1f;
float pitch = 1f;
bool loop = false;
AudioSource source = null;
AudioMixerGroup mixerGroup = null;

AudioManager.AudioError err = am.AddSoundFromPath(soundName, path, volume, pitch, loop, source, mixerGroup);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Adding the sound called: " + soundName + " from the given path failed with error id: " + err);
}
else {
    Debug.Log("Adding the sound called: " + soundName + " from the given path succesfull");
}
```
Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
string path = "Audio/audioClip01";

AudioManager.AudioError err = am.AddSoundFromPath(soundName, path);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Adding the sound called: " + soundName + " from the given path failed with error id: " + err);
}
else {
    Debug.Log("Adding the sound called: " + soundName + " from the given path succesfull");
}
```

**When to use it:**
When you want to add a new sound at runtime, could be useful if you need to add a lot of songs and don't want to add them manually through the GameObject the Audio Manager script resides on.

### Play method
**What it does:**
Starts playing the choosen sound and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how playing the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play

```csharp
string soundName = "SoundName";

AudioManager.AudioError err = am.Play(soundName);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to play a sound directly without changing its initally properties.

If you want to enable looping for a sound (see [Adding a new sound](#adding-a-new-sound)) for more information.

See [```AudioSource.Play```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.Play.html) for more details on what play does.

### Play At Time Stamp method
**What it does:**
Start playing the choosen sound at the given ```startTime``` and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how playing the sound from the given ```startTime``` failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```StartTime``` is the moment we want to play the sound at so instead of starting at 0 seconds we start at 10 seconds

```csharp
string soundName = "SoundName";
float startTime = 10f;

AudioManager.AudioError err = am.PlayAtTimeStamp(soundName, startTime);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " at startTime: " + startTime.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " at startTime: " + startTime.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to play a sound but skip a portion at the start. Could be used if only the second part of your sound is high intesity and normally you want to build up the intensity, but not when the game is in a special state.

### Get Playback Position method
**What it does:**
Returns an instance of the ValueDataError class, where the value (gettable with ```Value```), is the current playback position of the given sound in seconds and where the error (gettable with ```Error```) is an integer representing the AudioError Enum (see [Possible Errors](#possible-errors)), showing wheter and how getting the current playback position of the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the playback position of

```csharp
string soundName = "SoundName";

ValueDataError<float> valueDataError = am.GetPlaybackPosition(soundName);
if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
    Debug.Log("Getting playBackPosition of the sound called: " + soundName + " failed with error id: " + valueDataError.Error);
}
else {
    Debug.Log("Getting playBackPosition of the sound called: " + soundName + " with the position being: " + valueDataError.Value.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When yu want to get the time the current amount of time the sound has been playing already.

### Play At 3D Position method
**What it does:**
Starts playing the choosen sound at a given 3D position and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how playing the sound at the given position failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```WorldPosition``` is the 3D position in world space we want the sound to be emitting from
- ```MinDistance``` is the distance the sound will not get louder at
- ```MaxDistance``` is the distance the sound will still be hearable at
- ```Spread``` is the angle the sound will be emitted at in degrees
- ```SpatialBlend``` defines how much the sound is affected by 3D (0f = 2D, 1f = 3D)
- ```DopplerLevel``` defines the doppler scale for our sound
- ```RolloffMode``` defines how the sound should decline in ```volume``` between the min and max distance

```csharp
string soundName = "SoundName";
Vector3 worldPosition = new Vector3(10f, 10f, 0f);
float minDistance = 5f;
float maxDistance = 15f;
float spread = 0f;
float spatialBlend = 1f;
float dopplerLevel = 1f;
AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;

AudioManager.AudioError err = am.PlayAt3DPosition(soundName, worldPosition, minDistance, maxDistance, spread, spatialBlend, dopplerLevel, rolloffMode);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
Vector3 worldPosition = new Vector3(10f, 10f, 0f);
float minDistance = 5f;
float maxDistance = 15f;

AudioManager.AudioError err = am.PlayAt3DPosition(soundName, worldPosition, minDistance, maxDistance);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " at the position x " + worldPosition.x.ToString("0.00") + " and y " + worldPosition.y.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to play a sound directly from a 3D position and make the ```volume``` be influenced by the distance the player has from that position.

### Play Attached To GameObject method
**What it does:**
Starts playing the choosen sound attached to a ```gameObject``` and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how playing the sound attached to the given gameobject failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play
- ```GameObject``` is the object the sound is emitting from
- ```MinDistance``` is the distance the sound will not get louder at
- ```MaxDistance``` is the distance the sound will still be hearable at
- ```Spread``` is the angle the sound will be emitted at in degrees
- ```SpatialBlend``` defines how much the sound is affected by 3D (0f = 2D, 1f = 3D)
- ```DopplerLevel``` defines the doppler scale for our sound
- ```RolloffMode``` defines how the sound should decline in ```volume``` between the min and max distance

```csharp
string soundName = "SoundName";
GameObject gameObject = this.gameObject;
float minDistance = 5f;
float maxDistance = 15f;
float spread = 0f;
float spatialBlend = 1f;
float dopplerLevel = 1f;
AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;

AudioManager.AudioError err = am.PlayAttachedToGameObject(soundName, gameObject, minDistance, maxDistance, spread, spatialBlend, dopplerLevel, rolloffMode);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " attached to: " + gameObject.name + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " attached to: " + gameObject.name + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
GameObject gameObject = this.gameObject;
float minDistance = 5f;
float maxDistance = 15f;

AudioManager.AudioError err = am.PlayAttachedToGameObject(soundName, gameObject, minDistance, maxDistance);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " attached to: " + gameObject.name + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " attached to: " + gameObject.name + " succesfull");
}
```

**When to use it:**
When you want to play a sound directly from a object and make the ```volume``` be influenced by the distance the player has from that object.

### Play Delayed method
**What it does:**
Starts playing the choosen sound after the given amount of time and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how playing the sound after the given amount of time failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play after the given amount of time which would be the 5 seconds we've defined
- ```Delay``` is the time after which we want to start playing the sound.

```csharp
string soundName = "SoundName";
float delay = 5f;

AudioManager.AudioError err = am.PlayDelayed(soundName, delay);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " seconds failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " seconds succesfull");
}
```

**When to use it:**
When you want to play a sound after a given delay time instead of directly when the method is called.

See [```AudioSource.PlayDelayed```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.PlayDelayed.html) for more details on what play delayed does.

### Play OneShot method
**What it does:**
Starts playing the choosen sound once and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how playing the sound once failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play once

```csharp
string soundName = "SoundName";

AudioManager.AudioError err = am.PlayOneShot(soundName);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " once failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " once succesfull");
}
```

**When to use it:**
When you want to only play a sound once. Having multiple instances of the same sound running at the same time is only possible with this method.

See [```AudioSource.PlayOneShot```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.PlayOneShot.html) for more details on what play oneshot does.

### Play Scheduled method
**What it does:**
Starts playing the sound after the given amount of time with additional buffer time to fetch the data from media and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how playing the sound after the given amount of time failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to play after the given amount of time which would be the 10 seconds we've defined
- ```Delay``` is the time after which we want to start playing the sound

```csharp
string soundName = "SoundName";
double delay = 10d;

AudioManager.AudioError err = am.PlayScheduled(soundName, delay);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " failed with error id: " + err);
}
else {
    Debug.Log("Playing sound called: " + soundName + " after " + delay.ToString("0.00") + " seconds succesfull");
}
```

**When to use it:**
When you want to switch smoothly between sounds, because the method it is independent of the frame rate and gives the audio system enough time to prepare the playback of the sound to fetch it from media, where the opening and buffering takes a lot of time (streams) without causing sudden CPU spikes.

See [```AudioSource.PlayScheduled```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.PlayScheduled.html) for more details on what play scheduled does.

### Stop method
**What it does:**
Stops the sound if it is currently playing and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how stopping the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to stop

```csharp
string soundName = "SoundName";

AudioManager.AudioError err = am.Stop(soundName);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Stopping sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Stopping sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to stop the given sound, if you restart it later the sound will start anew. So to really only pause it use the [Toggle Pause method](#toggle-pause-method).

See [```AudioSource.Stop```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.Stop.html) for more details on what stop does.

### Toggle Mute method
**What it does:**
Sets the ```volume``` of the sound to 0 and resets it to it's initally value if called again and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how muting or unmuting the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to toggle mute on / off

```csharp
string soundName = "SoundName";

AudioManager.AudioError err = am.ToggleMute(soundName);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Muting or unmuting sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Muting or unmuting sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to completly silence a sound and still keep it playing in the background. For example if you have a radio channel with a mute or switch channel button.

### Toggle Pause method
**What it does:**
Completly pauses or unpauses playback of the given sound until it is toggled again and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how pausing or unpausing the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to toggle pause on / off

```csharp
string soundName = "SoundName";

AudioManager.AudioError err = am.TogglePause(soundName);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Pausing or unpausing sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Pausing or unpausing sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to pause or unpause a sound without stopping it completly so it can be later restarted at the stopped time.

See [```AudioSource.UnPause```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.UnPause.html) for more details on what unpause does and see [```AudioSource.Pause```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/AudioSource.Pause.html) for more details on what pause does.

### Get Progress method
**What it does:**
Returns an instance of the ValueDataError class, where the value (gettable with ```Value```), is the ```progress``` of the given sound, which is a float from 0 to 1 and where the error (gettable with ```Error```) is an integer representing the AudioError Enum (see [Possible Errors](#possible-errors)), showing wheter and how getting the current  of ```progress``` of the sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the progress from

```csharp
string soundName = "SoundName";

ValueDataError<float> valueDataError = am.GetProgress(soundName);
if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
    Debug.Log("Getting progress of the sound called: " + soundName + " failed with error id: " + valueDataError.Error);
}
else {
    Debug.Log("Getting progress of the sound called: " + soundName + " with the progress being: " + (valueDataError.Value * 100).ToString("0.00") + "% succesfull");
}
```

**When to use it:**
When you want to get the progress of a sound for an animation or to track once it's finished to start a new sound.

### Try Get Source method
**What it does:**
Returns the ```source``` of the given sound, as well as an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how getting the source of the given sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the source from

```csharp
string soundName = "SoundName";
AudioSource source = default;

AudioManager.AudioError err = am.TryGetSource(soundName, out source);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Getting source of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Getting source of the sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to directly change the values of the given sound yourself and affect it while it's playing.

### Lerp Pitch method
**What it does:**
Lerps the ```pitch``` of a sound over a given amount of time and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how lerping the pitch of the given sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to change the pitch from
- ```EndValue``` (0.1 - 3) is the value the ```pitch``` should have at the end
- ```WaitTime``` defines the total amount of time needed to achieve the given ```endValue```
- ```Granularity``` is the amount of steps in which we decrease the volume to the ```endValue```

```csharp
string soundName = "SoundName";
float endValue = 0.8f;
float waitTime = 1f;
float granularity = 2f;

AudioManager.AudioError err = am.LerpPitch(soundName, endValue, waitTime, granularity);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " in the time: " + waitTime.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float endValue = 0.8f;

AudioManager.AudioError err = am.LerpPitch(soundName, endValue);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping pitch of the sound called: " + soundName + " to the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to decrease -or increase the ```pitch``` over a given amount of time.

### Lerp Volume method
**What it does:**
Lerps the ```volume``` of a given sound over a given amount of time and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how lerping the volume of the given sound failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to change the volume from
- ```EndValue``` (0 - 1) is the value the ```volume``` should have at the end
- ```WaitTime``` defines the total amount of time needed to achieve the given ```endValue```
- ```Granularity``` is the amount of steps in which we decrease the volume to the ```endValue```

```csharp
string soundName = "SoundName";
float endValue = 0.8f;
float waitTime = 1f;
float granularity = 2f;

AudioManager.AudioError err = am.LerpVolume(soundName, endValue, waitTime, granularity);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Lerping volume of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping volume of the sound called: " + soundName + " in the time: " + waitTime.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
float endValue = 0.8f;

AudioManager.AudioError err = am.LerpVolume(soundName, endValue);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Lerping volume of the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping volume of the sound called: " + soundName + " to the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to decrease -or increase the ```volume``` over a given amount of time.

### Change Group Value method
**What it does:**
Changes the value of the given exposed parameter for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) of a given sound to the given newValue and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how changing the value of the given exposed parameter failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to change the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixer.html)
- ```NewValue``` is the value we want to set the exposed parameter to

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";
float newValue = -80f;

AudioManager.AudioError error = am.ChangeGroupValue(soundName, exposedParameterName, newValue);
if (error != AudioManager.AudioError.OK) {
    Debug.Log("Changing AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Changing AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " with the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to change an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.

### Get Group Value method
**What it does:**
Returns an instance of the ValueDataError class, where the value (gettable with ```Value```), is the current value of the given exposed parameter and where the error (gettable with ```Error```) is an integer representing the AudioError Enum (see [Possible Errors](#possible-errors)), showing wheter and how getting the value of the given exposed parameter failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to get the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixer.html)

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";

ValueDataError<float> valueDataError = am.GetGroupValue(soundName, exposedParameterName);
if (valueDataError.Error != (int)AudioManager.AudioError.OK) {
    Debug.Log("Getting AudioMixerGroup volume of the sound called: " + soundName + " failed with error id: " + valueDataError.Error);
}
else {
    Debug.Log("Getting AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " with the current value being: " + valueDataError.Value.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to get an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.

### Reset Group Value method
**What it does:**
Reset the value of the given exposed parameter for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) of a given sound to the default value and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how resetting the value of the given exposed parameter failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to reset the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixer.html)

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";

AudioManager.AudioError error = am.ResetGroupValue(soundName, exposedParameterName);
if (error != AudioManager.AudioError.OK) {
    Debug.Log("Resetting AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Resetting AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " succesfull");
}
```

**When to use it:**
When you want to reset an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.

### Lerp Group Value method
**What it does:**
Lerps the value of the given exposed parameter for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) of a given sound over a given amount of time and returns an AudioError (see [Possible Errors](#possible-errors)), showing wheter and how lerping the value of the given exposed parameter failed.

**How to call it:**
- ```SoundName``` is the ```name``` we have given the sound we want to reset the [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) parameter on
- ```ExposedParameterName``` is the name we have given the exposed parameter on the [```AudioMixer```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixer.html)
- ```EndValue``` is the value the exposed parameter should have at the end
- ```WaitTime``` defines the total amount of time needed to achieve the given ```endValue```
- ```Granularity``` is the amount of steps in which we decrease the volume to the ```endValue```

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";
float endValue = -80f;
float waitTime = 1f;
float granularity = 2f;

AudioManager.AudioError err = am.LerpGroupValue(soundName, exposedParameterName, endValue, waitTime, granularity);
if (error != AudioManager.AudioError.OK) {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " in the time: " + waitTime.ToString("0.00") + " seconds with the endValue: " + endValue.ToString("0.00") + " and the granularity: " + granularity.ToString("0.00") + " succesfull");
}
```

Alternatively you can call the methods with less paramters as some of them have default arguments.

```csharp
string soundName = "SoundName";
string exposedParameterName = "Volume";
float endValue = -80f;

AudioManager.AudioError err = am.LerpGroupValue(soundName, exposedParameterName, endValue);
if (err != AudioManager.AudioError.OK) {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " failed with error id: " + err);
}
else {
    Debug.Log("Lerping AudioMixerGroup exposed parameter with the name " + exposedParameterName + " on the sound called: " + soundName + " to the endValue: " + endValue.ToString("0.00") + " succesfull");
}
```

**When to use it:**
When you want to lerp an exposed parameter (for example the volume or pitch) for the complete [```AudioMixerGroup```](https://docs.unity3d.com/2021.2/Documentation/ScriptReference/Audio.AudioMixerGroup.html) the sound is connected to.
