using DiscordRPC;
using MediaPlayerController;
using System;
using System.IO;
using System.Threading;

namespace RemoteWindowsMediaPlayer
{
    class DiscordPresence
    {
        public static DiscordRpcClient client;

        static TimeSpan durationTs;
        static TimeSpan lengthTs;
        static string songName;
        static string artistName;
        static string duration;
        static string songLength;

        static public void Main(String[] args)
        {
            // this is temporary dont bash me over it :p
            var appId = File.ReadAllLines(@"../../../../appId.txt")[0].Replace('\n', '\0');
            Console.WriteLine(appId);
            client = new DiscordRpcClient(appId);
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.Initialize();
            while (true)
            {
                WindowsMediaPlayerController wmpc = new WindowsMediaPlayerController();
                SongDetails x = wmpc.GetCurrentSongDetails();
                if (wmpc.GetPlayingState() == PlayingState.Playing)
                {
                    songName = wmpc.GetCurrentSongName();
                    artistName = x.SongName.Substring(0,
                        x.SongName.LastIndexOf(" - " + wmpc.GetCurrentSongName().ToString()));
                    durationTs = TimeSpan.FromSeconds(wmpc.GetCurrentSongTime());
                    duration = $"{durationTs.Minutes:D2}:{durationTs.Seconds:D2}";
                    lengthTs = TimeSpan.FromSeconds(x.Duration);
                    songLength = $"{lengthTs.Minutes:D2}:{lengthTs.Seconds:D2}";


                    client.SetPresence(new RichPresence()
                    {
                        Details = songName + " | " + artistName,
                        State = duration + "----" + songLength,
                        Assets = new Assets()
                        {
                            LargeImageKey = "wmp_logo",
                            LargeImageText = $"Listening to {songName} on Windows Media Player"
                        }
                    });
                }
                else
                {
                    client.SetPresence(new RichPresence()
                    {
                        Details = "Idle",
                        Assets = new Assets()
                        {
                            LargeImageKey = "wmp_logo",
                            LargeImageText = "Windows Media Player"
                        }
                    });
                }
                Thread.Sleep(1000);
            }
        }
    }
}
