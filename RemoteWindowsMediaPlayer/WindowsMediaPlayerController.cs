// Decompiled with JetBrains decompiler
// Type: MediaPlayerController.WindowsMediaPlayerController
// Assembly: MediaPlayerController, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF010E8D-48F2-4017-8165-88EFC4873936
// Assembly location: C:\Program Files (x86)\WmpRemote Server\MediaPlayerController.dll

using Microsoft.Win32;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WMPLib;


namespace MediaPlayerController
{
  public class WindowsMediaPlayerController : IMediaPlayerRemoteControl
  {

    private WMPCore mediaPlayerCore;

    [DllImport("user32.dll")]
    private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);



    public MediaPlayerTypes GetInterfaceType()
    {
      return MediaPlayerTypes.WindowsMediaPlayer;
    }

    public WindowsMediaPlayerController()
    {
            WMPRemote.RemotedWindowsMediaPlayer rm = new WMPRemote.RemotedWindowsMediaPlayer();
            rm.createComObject();
            rm.connect();
          
            this.mediaPlayerCore = rm.getCore();
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
      return   Process.GetProcessesByName("wmplayer.exe").Length > 0;
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


        public WMPCore  getWMPCore()
        {
            return (WMPCore)this.mediaPlayerCore;
        }


    public void Play()
    {
     
      ((IWMPCore3) this.mediaPlayerCore).controls.play();
    }

    public void Stop()
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).controls.stop();
    }

    public void Pause()
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).controls.pause();
    }

    public void NextTrack()
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).controls.next();
    }

    public void PrevTrack()
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).controls.previous();
    }

    public string GetCurrentSongName()
    {
      if (this.HaveCoreClass())
        return ((IWMPCore3) this.mediaPlayerCore).currentMedia.name;
      return string.Empty;
    }

    public void JumpToTime(double secondTime)
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).controls.currentPosition = secondTime;
    }

    public SongDetails[] GetPlayList()
    {
      SongDetails[] songDetailsArray = new SongDetails[((IWMPCore3) this.mediaPlayerCore).currentPlaylist.count];
      if (!this.HaveCoreClass())
        return songDetailsArray;
      for (int lIndex = 0; lIndex < ((IWMPCore3) this.mediaPlayerCore).currentPlaylist.count; ++lIndex)
        songDetailsArray[lIndex] = new SongDetails()
        {
          SongName = ((IWMPCore3) this.mediaPlayerCore).currentPlaylist.get_Item(lIndex).name,
          Duration = ((IWMPCore3) this.mediaPlayerCore).currentPlaylist.get_Item(lIndex).duration,
          SongPath = ((IWMPCore3) this.mediaPlayerCore).currentPlaylist.get_Item(lIndex).sourceURL,
          SongNumber = lIndex
        };
      return songDetailsArray;
    }

    public void JumpToSongInPlayList(int nSongNumber)
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).controls.currentItem = ((IWMPCore3) this.mediaPlayerCore).currentPlaylist.get_Item(nSongNumber);
    }

    public void AddSongToPlayList(SongDetails song)
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).currentPlaylist.appendItem(this.mediaPlayerCore.newMedia(song.SongPath));
    }

    public void RemoveSongFromPlayList(SongDetails song)
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).currentPlaylist.removeItem(((IWMPCore3) this.mediaPlayerCore).currentPlaylist.get_Item(song.SongNumber));
    }

    public void VolumeUp(int nAmount)
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).settings.volume += nAmount;
    }

    public void VolumeDown(int nAmount)
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).settings.volume -= nAmount;
    }

    public void SetFixedVolume(int volume)
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).settings.volume = volume;
    }

    public int GetCurrentVolume()
    {
      if (this.HaveCoreClass())
        return ((IWMPCore3) this.mediaPlayerCore).settings.volume;
      return -1;
    }

    public bool IsMediaPlayerRunning()
    {
      return this.IsWMPRunning();
    }

    public SongDetails GetCurrentSongDetails()
    {
      SongDetails songDetails = new SongDetails();
      if (this.HaveCoreClass() && ((IWMPCore3) this.mediaPlayerCore).currentMedia != null)
      {
        string str1 = string.Empty;
        string itemInfo = ((IWMPCore3) this.mediaPlayerCore).currentMedia.getItemInfo("Artist");
        if (!string.IsNullOrEmpty(itemInfo))
          str1 = itemInfo + " - ";
        string str2 = str1 + ((IWMPCore3) this.mediaPlayerCore).currentMedia.name;
        songDetails.SongName = str2;
        songDetails.Duration = ((IWMPCore3) this.mediaPlayerCore).currentMedia.duration;
      }
      return songDetails;
    }

    public double GetCurrentSongTime()
    {
      if (this.HaveCoreClass())
        return ((IWMPCore3) this.mediaPlayerCore).controls.currentPosition;
      return 0.0;
    }

    public PlayingState GetPlayingState()
    {
      PlayingState playingState;
      if (this.HaveCoreClass())
      {
        switch (((IWMPCore3) this.mediaPlayerCore).playState)
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

    public void ClearPlaylist()
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).currentPlaylist.clear();
    }

    public bool GetShuffle()
    {
      bool flag = false;
      if (this.HaveCoreClass())
        flag = ((IWMPCore3) this.mediaPlayerCore).settings.getMode("shuffle");
      return flag;
    }

    public void SetShuffle(bool bIsShuffle)
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).settings.setMode("shuffle", bIsShuffle);
    }

    public bool GetRepeat()
    {
      bool flag = false;
      if (this.HaveCoreClass())
        flag = ((IWMPCore3) this.mediaPlayerCore).settings.getMode("loop");
      return flag;
    }

    public void SetRepeat(bool bIsRepeat)
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).settings.setMode("loop", bIsRepeat);
    }

    public void SetMute(bool bIsMute)
    {
      if (!this.HaveCoreClass())
        return;
      ((IWMPCore3) this.mediaPlayerCore).settings.mute = bIsMute;
    }

    public bool GetMute()
    {
      if (this.HaveCoreClass())
        return ((IWMPCore3) this.mediaPlayerCore).settings.mute;
      return true;
    }

    public void SetFullScreen(bool bIsFullScreen)
    {
      if (!this.HaveCoreClass())
        return;
      this.SetFocusOnMediaPlayer();
      //FullScreen();

    }

    private void SetFocusOnMediaPlayer()
    {
      foreach (Process process in Process.GetProcessesByName("wmplayer"))
        WindowsMediaPlayerController.ShowWindow(process.MainWindowHandle, 0);
    }

    public bool GetFullScreen()
    {
      return false;
    }

    public string GetSupportedFiles()
    {
            return null;//ConfigurationSettings.AppSettings["WMPFilesSupport"].ToString();
    }
  }
}
