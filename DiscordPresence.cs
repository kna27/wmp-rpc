using MediaPlayerController;
using System;
using System.Threading;

namespace RemoteWindowsMediaPlayer
{
    class DiscordPresence
    {
        static TimeSpan durationTs;
        static TimeSpan lengthTs;
        static string songName;
        static string songPath;
        static string artistName;
        static string albumName;
        static string duration;
        static string songLength;

        static public void Main(String[] args)
        {
            while (true)
            {
                WindowsMediaPlayerController wmpc = new WindowsMediaPlayerController();
                SongDetails x = wmpc.GetCurrentSongDetails();
                if (wmpc.GetPlayingState() == PlayingState.Playing || wmpc.GetPlayingState() == PlayingState.Paused)
                {
                    songName = wmpc.GetCurrentSongName();
                    songPath = x.SongPath;
                    artistName = x.SongName.Substring(0,
                        x.SongName.LastIndexOf(" - " + wmpc.GetCurrentSongName().ToString()));
                    albumName = "[Album name]";

                    if (wmpc.GetPlayingState() == PlayingState.Paused)
                    {
                        duration = "Paused";
                        songLength = "";
                    }
                    else
                    {
                        durationTs = TimeSpan.FromSeconds(wmpc.GetCurrentSongTime());
                        duration = $"{durationTs.Minutes:D2}:{durationTs.Seconds:D2}";
                        lengthTs = TimeSpan.FromSeconds(x.Duration);
                        songLength = $"{lengthTs.Minutes:D2}:{lengthTs.Seconds:D2}";
                    }

                    Console.WriteLine("Windows Media Player\n{0}\n{1} | {2}\n{3}  {4}\n\n",
                        songName, albumName, artistName, duration, songLength);
                }
                else
                {
                    Console.WriteLine("Windows Media Player\nIdle\n\n");
                }
                Thread.Sleep(1000);
            }
        }
    }
}
