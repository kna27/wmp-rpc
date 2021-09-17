using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WMPLib;


namespace MediaPlayerController
{
    public class WindowsMediaPlayerController : IMediaPlayerRemoteControl
    {

        private readonly WMPCore mediaPlayerCore;

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);



        public MediaPlayerTypes GetInterfaceType()
        {
            return MediaPlayerTypes.WindowsMediaPlayer;
        }

        public WindowsMediaPlayerController()
        {
            WMPRemote.RemotedWindowsMediaPlayer rm = new WMPRemote.RemotedWindowsMediaPlayer();
            rm.CreateComObject();
            rm.Connect();

            this.mediaPlayerCore = rm.GetCore();
            this.HaveCoreClass();
        }


        private bool HaveCoreClass()
        {
            bool flag = false;
            if (this.IsWMPRunning())
            {
                flag = this.mediaPlayerCore != null;

            }
            return flag;
        }



        public bool IsWMPRunning()
        {
            return Process.GetProcessesByName("wmplayer").Length > 0;
        }

        public void OpenMediaPlayer()
        {
            Process.Start("wmplayer.exe");
        }

        public void CloseMediaPlayer()
        {
            foreach (var process in Process.GetProcessesByName("wmplayer.exe"))
            {
                process.Kill();
            }

        }

        public WMPCore GetWMPCore()
        {
            return (WMPCore)this.mediaPlayerCore;
        }

        public string GetCurrentSongName()
        {
            if (this.HaveCoreClass())
                return ((IWMPCore3)this.mediaPlayerCore).currentMedia.name;
            return string.Empty;
        }

        public SongDetails[] GetPlayList()
        {
            SongDetails[] songDetailsArray = new SongDetails[((IWMPCore3)this.mediaPlayerCore).currentPlaylist.count];
            if (!this.HaveCoreClass())
                return songDetailsArray;
            for (int lIndex = 0; lIndex < ((IWMPCore3)this.mediaPlayerCore).currentPlaylist.count; ++lIndex)
                songDetailsArray[lIndex] = new SongDetails()
                {
                    SongName = ((IWMPCore3)this.mediaPlayerCore).currentPlaylist.get_Item(lIndex).name,
                    Duration = ((IWMPCore3)this.mediaPlayerCore).currentPlaylist.get_Item(lIndex).duration,
                    SongPath = ((IWMPCore3)this.mediaPlayerCore).currentPlaylist.get_Item(lIndex).sourceURL,
                    SongNumber = lIndex
                };
            return songDetailsArray;
        }

        public int GetCurrentVolume()
        {
            if (this.HaveCoreClass())
                return ((IWMPCore3)this.mediaPlayerCore).settings.volume;
            return -1;
        }

        public bool IsMediaPlayerRunning()
        {
            return this.IsWMPRunning();
        }

        public SongDetails GetCurrentSongDetails()
        {
            SongDetails songDetails = new SongDetails();
            if (this.HaveCoreClass() && ((IWMPCore3)this.mediaPlayerCore).currentMedia != null)
            {
                string str1 = string.Empty;
                string itemInfo = ((IWMPCore3)this.mediaPlayerCore).currentMedia.getItemInfo("Artist");
                if (!string.IsNullOrEmpty(itemInfo))
                    str1 = itemInfo + " - ";
                string str2 = str1 + ((IWMPCore3)this.mediaPlayerCore).currentMedia.name;
                songDetails.SongName = str2;
                songDetails.Duration = ((IWMPCore3)this.mediaPlayerCore).currentMedia.duration;
            }
            return songDetails;
        }

        public double GetCurrentSongTime()
        {
            if (this.HaveCoreClass())
                return ((IWMPCore3)this.mediaPlayerCore).controls.currentPosition;
            return 0.0;
        }

        public PlayingState GetPlayingState()
        {
            PlayingState playingState;
            if (this.HaveCoreClass())
            {
                switch (((IWMPCore3)this.mediaPlayerCore).playState)
                {
                    case WMPPlayState.wmppsUndefined:
                        playingState = PlayingState.Stoped;
                        break;
                    case WMPPlayState.wmppsStopped:
                        playingState = PlayingState.Stoped;
                        break;
                    case WMPPlayState.wmppsPaused:
                        playingState = PlayingState.Paused;
                        break;
                    case WMPPlayState.wmppsPlaying:
                        playingState = PlayingState.Playing;
                        break;
                    case WMPPlayState.wmppsScanForward:
                        playingState = PlayingState.Loading;
                        break;
                    case WMPPlayState.wmppsScanReverse:
                        playingState = PlayingState.Loading;
                        break;
                    case WMPPlayState.wmppsBuffering:
                        playingState = PlayingState.Loading;
                        break;
                    case WMPPlayState.wmppsWaiting:
                        playingState = PlayingState.Loading;
                        break;
                    case WMPPlayState.wmppsMediaEnded:
                        playingState = PlayingState.Loading;
                        break;
                    case WMPPlayState.wmppsTransitioning:
                        playingState = PlayingState.Loading;
                        break;
                    case WMPPlayState.wmppsReady:
                        playingState = PlayingState.Stoped;
                        break;
                    case WMPPlayState.wmppsReconnecting:
                        playingState = PlayingState.Loading;
                        break;
                    case WMPPlayState.wmppsLast:
                        playingState = PlayingState.Playing;
                        break;
                    default:
                        playingState = PlayingState.Stoped;
                        break;
                }
            }
            else
                playingState = PlayingState.Stoped;
            return playingState;
        }

        public bool GetShuffle()
        {
            bool flag = false;
            if (this.HaveCoreClass())
                flag = ((IWMPCore3)this.mediaPlayerCore).settings.getMode("shuffle");
            return flag;
        }

        public bool GetRepeat()
        {
            bool flag = false;
            if (this.HaveCoreClass())
                flag = ((IWMPCore3)this.mediaPlayerCore).settings.getMode("loop");
            return flag;
        }

        public bool GetMute()
        {
            if (this.HaveCoreClass())
                return ((IWMPCore3)this.mediaPlayerCore).settings.mute;
            return true;
        }

        public string GetSupportedFiles()
        {
            return null;//ConfigurationSettings.AppSettings["WMPFilesSupport"].ToString();
        }
    }
}
