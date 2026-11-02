# UdonVR Record Player Module: ProTV

Compatibility hook bridging the **UdonVR Record Player** (`RecordPlayer_VideoplayerClass`) with **ArchiTech ProTV 3.x**[cite: 1]. ProTV is the gold-standard target for the Record Player and supports the complete feature set, including Editor Auto-Setup.

## Feature Compatibility

| Capability | Status | Implementation Details |
| :--- | :---: | :--- |
| **Editor Auto-Setup** | `Full` | Supported via UdonVR Module Manager / Editor scripts |
| **Load URL** | `Full` | Calls `proTv._ChangeMedia(_url, _url, title)` |
| **External Load Detection** | `Full` | `_TvLoading()` triggers `_OnVideoLoad()` to stop vinyl if TV is used externally[cite: 1] |
| **Play / Pause / Stop** | `Full` | Uses `proTv._Play()`, `proTv._Pause()`, and `proTv._Stop()` |
| **Seeking & Scrubbing** | `Full` | Claims ownership and calls `proTv._ChangeSeekTime(_time, false)` |
| **Tonearm Progress** | `Full` | Remaps `proTv.currentTime` / `proTv.endTime` (`0..1`) |
| **2-Way Volume Sync** | `Full` | Syncs on `_TvReady()` and `_TvVolumeChange()` |
| **Permission / Lock Check** | `Full` | Checks `proTv.CanPlayMedia` |
| **Album Auto-Advance & Shuffle** | `Full` | `_TvMediaEnd()` triggers `_PlaybackEnded()` (Next Track, Shuffle, Album Shuffle)[cite: 1] |
| **Single-Track Loop** | `Full` | 2-frame delayed `proTv._Play()` via `_loopCurrentUrl()` |
| **Custom Track Titles** | `Partial` | Injects `[RecordPlayer] Track - Album - Artist` on load; `_SetTitle()` currently disabled |

## Inspector Setup
* **Automated:** Use the Record Player Auto-Setup tool to automatically link ProTV.
* **Manual:**
  1. Add `RecordPlayer_VideoPlayer_ProTV` to your module GameObject.
  2. Assign your scene's `TVManager` to **`proTv`** and `RecordPlayer` to **`recordPlayer`**.

<!-- MODULE_MANIFEST_START -->
```json
{
  "schemaVersion": "1.1.0",
  "id": "ProTV",
  "displayName": "ArchiTech ProTV",
  "repo": "[https://github.com/UdonVR/RecordPlayerModule_ProTV](https://github.com/UdonVR/RecordPlayerModule_ProTV)",
  "scriptPath": "RecordPlayer_VideoPlayer_ProTV.cs",
  "className": "UdonVR.RecordPlayer.RecordPlayer_VideoPlayer_ProTV",
  "baseClass": "UdonVR.RecordPlayer.RecordPlayer_VideoplayerClass",
  "autoSetup": true,
  "requiredNamespaces": [
    "ArchiTech.ProTV"
  ],
  "inspectorReferences": [
    {
      "fieldName": "recordPlayer",
      "fieldType": "UdonVR.RecordPlayer.RecordPlayer",
      "required": true,
      "description": "Core RecordPlayer controller"
    },
    {
      "fieldName": "proTv",
      "fieldType": "ArchiTech.ProTV.TVManager",
      "required": true,
      "description": "Core ProTV TVManager component"
    }
  ],
  "capabilities": {
    "autoSetup": "full",
    "loadUrl": "full",
    "externalLoadDetection": "full",
    "playPauseStop": "full",
    "seekTime": "full",
    "playbackProgress": "full",
    "volumeSync": "bidirectional",
    "permissionCheck": "full",
    "autoAdvanceAndShuffle": "full",
    "loopTrack": "full",
    "customTitleMetadata": "load_only"
  },
  "notes": [
    "Gold standard reference implementation.",
    "Sets initial volume to proTv.DefaultVolume on Start().",
    "_SetTitle() currently has an early return; metadata is injected during _LoadUrl()."
  ]
}
```
<!-- MODULE_MANIFEST_END -->
