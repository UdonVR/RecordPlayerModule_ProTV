# UdonVR Record Player Module: ProTV

Compatibility hook bridging the **UdonVR Record Player** (`RecordPlayer_VideoplayerClass`) with **ArchiTech ProTV 3.x**. ProTV is the gold-standard target for the Record Player and supports the complete feature set.

## Feature Compatibility

| Capability | Status | Implementation Details |
| :--- | :---: | :--- |
| **Load URL** | `Full` | Calls `proTv._ChangeMedia(_url, _url, title)` |
| **Play / Pause / Stop** | `Full` | Uses `proTv._Play()`, `proTv._Pause()`, and `proTv._Stop()` |
| **Seeking & Scrubbing** | `Full` | Claims ownership and calls `proTv._ChangeSeekTime(_time, false)` |
| **Tonearm Progress** | `Full` | Remaps `proTv.currentTime` / `proTv.endTime` (`0..1`) |
| **2-Way Volume Sync** | `Full` | Syncs on `_TvReady()` and `_TvVolumeChange()` |
| **Permission / Lock Check** | `Full` | Checks `proTv.CanPlayMedia` |
| **Track End Auto-Advance** | `Full` | Distinct `_TvMediaEnd()` vs. `_TvStop()` callbacks |
| **Single-Track Loop** | `Full` | 2-frame delayed `proTv._Play()` via `_loopCurrentUrl()` |
| **Custom Track Titles** | `Partial` | Injects `[RecordPlayer] Track - Album - Artist` on load; `_SetTitle()` currently disabled |

## Inspector Setup
1. Add the `RecordPlayer_VideoPlayer_ProTV` component to your module GameObject.
2. Assign your scene's `TVManager` instance to the **`proTv`** field.
3. Reference this module on your core `RecordPlayer` script.

<!-- MODULE_MANIFEST_START -->
```json
{
  "schemaVersion": "1.0.0",
  "id": "ProTV",
  "displayName": "ArchiTech ProTV",
  "repo": "[https://github.com/UdonVR/RecordPlayerModule_ProTV](https://github.com/UdonVR/RecordPlayerModule_ProTV)",
  "scriptPath": "RecordPlayer_VideoPlayer_ProTV.cs",
  "className": "UdonVR.RecordPlayer.RecordPlayer_VideoPlayer_ProTV",
  "baseClass": "UdonVR.RecordPlayer.RecordPlayer_VideoplayerClass",
  "requiredNamespaces": [
    "ArchiTech.ProTV"
  ],
  "inspectorReferences": [
    {
      "fieldName": "proTv",
      "fieldType": "ArchiTech.ProTV.TVManager",
      "required": true,
      "description": "Core ProTV TVManager component"
    }
  ],
  "capabilities": {
    "loadUrl": "full",
    "playPauseStop": "full",
    "seekTime": "full",
    "playbackProgress": "full",
    "volumeSync": "bidirectional",
    "permissionCheck": "full",
    "loopTrack": "full",
    "customTitleMetadata": "load_only",
    "naturalEndDetection": "full"
  },
  "notes": [
    "Gold standard reference implementation.",
    "Sets initial volume to proTv.DefaultVolume on Start().",
    "_SetTitle() currently has an early return; metadata is injected during _LoadUrl()."
  ]
}
```
<!-- MODULE_MANIFEST_END -->
