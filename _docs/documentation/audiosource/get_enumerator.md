---
layout: default
title: Get Enumerator
parent: AudioSource
grand_parent: Documentation
---

## Get Enumerator
**What it does:**
Returns an enumerable to the underlying list of all registered sound names. Can be used to call methods like (see [LerpVolume](https://mathewhdyt.github.io/Unity-Audio-Manager/docs/documentation/audiosource/lerp_volume/)) for each registered sound.

**How to call it:**
```csharp
IEnumerable<string> sounds = am.GetEnumerator();

foreach (var name in sounds) {
	// Do something with the registered sound name.
}
```

**When to use it:**
When you want to repeat an action for every sound name that has been registered with the AudioManager.
