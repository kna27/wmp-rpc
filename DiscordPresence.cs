using MediaPlayerController;
using System;

namespace RemoteWindowsMediaPlayer
{
    class DiscordPresence
    {
        static public void Main(String[] args)
        {
            var wmpc = new WindowsMediaPlayerController();
            var x = wmpc.GetCurrentSongDetails();
            Console.WriteLine("Windows Media Player\n{0}\n{1} | [Album Name]\n{3}  x:xx",
                wmpc.GetCurrentSongName().ToString(), x.SongName.Substring(0, (x.SongName.LastIndexOf(" - " + wmpc.GetCurrentSongName().ToString()))), x.SongName, x.Duration);
            Console.ReadKey();
        }
    }
}
