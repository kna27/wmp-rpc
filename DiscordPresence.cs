using MediaPlayerController;
using System;

namespace RemoteWindowsMediaPlayer
{
    class DiscordPresence
    {
        static string songName;
        static string songPath;
        static string artistName;
        static string albumName;
        static string duration;
        static string songLength;
        static public void Main(String[] args)
        {
            WindowsMediaPlayerController wmpc = new WindowsMediaPlayerController();
            SongDetails x = wmpc.GetCurrentSongDetails();

            songName = wmpc.GetCurrentSongName();
            songPath = x.SongPath;
            artistName = x.SongName.Substring(0,
                x.SongName.LastIndexOf(" - " + wmpc.GetCurrentSongName().ToString()));
            albumName = "[Album]";
            duration = x.Duration.ToString();
            songLength = "x:xx";

            Console.WriteLine("Windows Media Player\n{0}\n{1} | {2}\n{3}  x:xx",
            songName, albumName, artistName, duration, songLength);
            Console.ReadKey();
        }
    }
}
