# WMP-Remote-Control

C# Windows Media Player Remote Control Library - using WMPLib , the C# way for development of remote control to media player with full features.


### Features

You can use this interface to completely control your windows media player

```csharp
    void OpenMediaPlayer();

    void CloseMediaPlayer();

    void Play();

    void Stop();

    void Pause();

    void NextTrack();

    void PrevTrack();

    string GetCurrentSongName();

    void JumpToTime(double secondTime);

    SongDetails[] GetPlayList();

    void JumpToSongInPlayList(int nSongNumber);

    void AddSongToPlayList(SongDetails song);

    void RemoveSongFromPlayList(SongDetails song);

    void VolumeUp(int nAmount);

    void VolumeDown(int nAmount);

    void SetFixedVolume(int volume);

    int GetCurrentVolume();

    bool IsMediaPlayerRunning();

    SongDetails GetCurrentSongDetails();

    double GetCurrentSongTime();

    PlayingState GetPlayingState();

    void ClearPlaylist();

    bool GetShuffle();

    void SetShuffle(bool bIsShuffle);

    bool GetRepeat();

    void SetRepeat(bool bIsRepeat);

    void SetMute(bool bIsMute);

    bool GetMute();

    void SetFullScreen(bool bIsFullScreen);

    bool GetFullScreen();

    MediaPlayerTypes GetInterfaceType();
```




