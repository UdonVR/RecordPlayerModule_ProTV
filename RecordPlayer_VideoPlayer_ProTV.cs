
using ArchiTech.ProTV;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVR.RecordPlayer
{
    public class RecordPlayer_VideoPlayer_ProTV : RecordPlayer_VideoplayerClass
    {
        public TVManager proTv;

        void Start()
        {
            proTv._RegisterListener(this, 0);
            _SetVolume(proTv.DefaultVolume);
        }

        public override void _LoadUrl(VRCUrl _url)
        {
            RecordPlayer_Disk disk = recordPlayer.records[recordPlayer.currentRecord];
            string wow = string.IsNullOrWhiteSpace(disk.trackNames[recordPlayer.currentTrack])
                ? " "
                : $" {disk.trackNames[recordPlayer.currentTrack]} - " ;
            proTv._ChangeMedia(_url, _url, $"[RecordPlayer]{wow}{disk.albumName} - {disk.artistName}");
        }

        public override bool _CanPlayVideo()
        {
            return proTv.CanPlayMedia;
        }
        
        public override bool _IsVideoplayerOwner()
        {
            return Networking.IsOwner(proTv.gameObject);
        }

        public override float _GetPlaybackTime()
        {
            float curtime = proTv.currentTime;
            float totalTime = proTv.endTime;
            return Remap(curtime, 0, totalTime, 0, 1);
        }

        public override float _GetPlaybackDuration_Current() { return proTv.currentTime; }
        
        public override float _GetPlaybackDuration_End() { return proTv.endTime; }

        public override bool _IsPaused()
        {
            return proTv.IsPaused;
        }

        public override void _Pause()
        {
            Debug.LogError("[UdonVR][Record - ProTV] _Pause");
            if (proTv.IsPaused)
            {
                proTv._Play();
            }
            else
            {
                proTv._Pause();
            }
        }

        public override void _Stop()
        {
            Debug.LogError("[UdonVR][Record - ProTV] _Stop");
            proTv._Stop();
        }

        public override void _loopCurrentUrl()
        {
            Debug.LogError("[UdonVR][Record - ProTV] _loopCurrentUrl");
            SendCustomEventDelayedFrames(nameof(_loopCurrentUrl2),2);
        }

        public void _loopCurrentUrl2()
        {
            Debug.LogError("[UdonVR][Record - ProTV] _loopCurrentUrl2");
            proTv._Play();
        }

        public override void _SetVolume(float _volume)
        {
            proTv._ChangeVolume(_volume);
        }

        public override float _GetVolume()
        {
            return proTv.volume;
        }

        public override void _SetTitle(RecordPlayer_Disk _disk)
        {
            return;
            string wow = string.IsNullOrWhiteSpace(_disk.chapterData[recordPlayer.currentTrack].chapterNames[recordPlayer.currentChapter])
                ? " "
                : $" {_disk.trackNames[recordPlayer.currentTrack]} - " ;
            Debug.Log($"Changing title to: [RecordPlayer]{wow}{_disk.albumName} - {_disk.artistName}");
            proTv._ChangeTitle($"[RecordPlayer]{wow}{_disk.albumName} - {_disk.artistName}");
        }

        public override void _ChangePlaybackTime(float _time)
        {
            proTv.Owner = Networking.LocalPlayer;
            proTv._ChangeSeekTime(_time,false);
        }

        //##############################################################################################################
        #region ProTv Callbacks

        public void _TvReady()
        {
            recordPlayer.volumeSlider._Internal_UpdateValue(_GetVolume(), false);
        }

        public void _TvStop()
        {
            Debug.LogError("[UdonVR][Record - ProTV] _TvStop");
            _End();
        }

        public void _TvMediaEnd()
        {
            Debug.LogError("[UdonVR][Record - ProTV] _TvMediaEnd");
            _PlaybackEnded();
        }

        public void _TvVideoPlayerError()
        {
            Debug.LogError("[UdonVR][Record - ProTV] _TvVideoPlayerError");
            _PlaybackEnded();
        }
        
        public void _TvLoading()
        {
            Debug.LogError("[UdonVR][Record - ProTV] _TvLoading");
            _OnVideoLoad();
        }

        public void _TvVolumeChange()
        {
            recordPlayer.volumeSlider._Internal_UpdateValue(_GetVolume(), false);
        }

        #endregion
        //##############################################################################################################
    }
}