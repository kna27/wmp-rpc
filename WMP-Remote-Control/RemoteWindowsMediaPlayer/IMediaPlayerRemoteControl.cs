namespace MediaPlayerController
{
    public interface IMediaPlayerRemoteControl
    {
        void OpenMediaPlayer();

        void CloseMediaPlayer();

        string GetCurrentSongName();

        SongDetails[] GetPlayList();

        int GetCurrentVolume();

        bool IsMediaPlayerRunning();

        SongDetails GetCurrentSongDetails();

        double GetCurrentSongTime();

        PlayingState GetPlayingState();

        bool GetShuffle();

        bool GetRepeat();

        bool GetMute();

        MediaPlayerTypes GetInterfaceType();
    }
}
