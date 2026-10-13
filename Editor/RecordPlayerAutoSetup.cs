using System.Collections;
using System.Collections.Generic;
using ArchiTech.ProTV;
using UdonVR.RecordPlayer;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using VRC.SDKBase.Editor.BuildPipeline;

namespace com.udonvr.recordplayer.protvEditor
{

    public class RecordPlayerAutoSetup : IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => -9999;

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }

        [PostProcessScene(-9999)]
        private static void ProcessScene()
        {
            RecordPlayer recordPlayer = GameObject.FindObjectOfType<RecordPlayer>();
            TVManager _proTV = GameObject.FindObjectOfType<TVManager>();
            if (recordPlayer == null)
            {
                Debug.LogWarning("[UdonVR][ProTV AutoSetup] Record Player was not found");
                return;
            }
            
            if (_proTV == null)
            {
                Debug.LogError("[UdonVR][ProTV AutoSetup] ProTV was not found");
                return;
            }
            
            if (recordPlayer.videoplayerModule != null && recordPlayer.videoplayerModule.GetType() != typeof(RecordPlayer_VideoPlayer_ProTV))
            {
                if (EditorUtility.DisplayDialog($"Existing Module found", $"There was a Module found attached to the record player, but it wasn't ProTV. Do you want to continue ProTV Auto Setup?", $"Yes", "No"))
                {

                }
                else
                {
                    Debug.LogError($"[UdonVR][ProTV AutoSetup] Module found [{recordPlayer.videoplayerModule.name}] but is not for ProTV, Canceling AutoSetup");
                    return;
                }
            }

            RecordPlayer_VideoplayerClass module = GameObject.FindObjectOfType<RecordPlayer_VideoPlayer_ProTV>();
            if (module == null)
            {
                var loadedObject = AssetDatabase.LoadAssetAtPath("Packages/com.udonvr.recordplayer.protv/RecordPlayerVideoModule - ProTV.prefab", typeof(UnityEngine.Object));
                if (loadedObject == null)
                {
                    Debug.LogError("[UdonVR][ProTV AutoSetup] No Videoplayer module found");
                    return;
                }
                if (!((GameObject)PrefabUtility.InstantiatePrefab(loadedObject)).transform.TryGetComponent(out module))
                {
                    Debug.LogError("[UdonVR][ProTV AutoSetup] No Videoplayer module found, failed to instantiate");
                    return;
                }
                else
                {
                    Debug.Log($"[UdonVR][ProTV AutoSetup] Instantiate ProTV videoplayer module: [{module.name}]");
                    recordPlayer.videoplayerModule = module;
                    if (module.GetType() != typeof(RecordPlayer_VideoPlayer_ProTV))
                    {
                        Debug.LogError("[UdonVR][ProTV AutoSetup] Created module found but is not for ProTV, Canceling AutoSetup");
                        return;
                    }
                }
            }

            if (module.recordPlayer == null)
            {
                module.recordPlayer = recordPlayer;
                Debug.Log($"[UdonVR][ProTV AutoSetup] Module's recordPlayer Reference was null, binding to [{recordPlayer.name}]");
            }
            else
            {
                Debug.Log($"[UdonVR][ProTV AutoSetup] Module's recordPlayer Reference already filled in, skipping");
            }

            if (((RecordPlayer_VideoPlayer_ProTV)module).proTv == null)
            {
                ((RecordPlayer_VideoPlayer_ProTV)module).proTv = _proTV;
                Debug.Log($"[UdonVR][ProTV AutoSetup] Module's ProTV Reference was null, binding to [{_proTV.name}]");
            }
            else
            {
                Debug.Log($"[UdonVR][ProTV AutoSetup] Module's ProTV Reference already filled in, skipping");
            }
        }
    }
}