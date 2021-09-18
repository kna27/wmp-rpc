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
        static char filled = '◼';
        static char empty = '▭';
        static public void Main(String[] args)
        {
            // this is temporary dont bash me over it :p
            var appId = File.ReadAllLines(@"../../../../appId.txt")[0].Replace('\n', '\0');
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
                    double percentage = (durationTs.TotalSeconds / lengthTs.TotalSeconds) * 100;
                    int rounded = (int)(Math.Round(percentage / 10.0) * 10);

                    string progressBar = "";
                    for (int i = 0; i < rounded / 10; i++)
                    {
                        progressBar += filled;
                    }
                    for (int i = 0; i < 10 - rounded / 10; i++)
                    {
                        progressBar += empty;
                    }

                    client.SetPresence(new RichPresence()
                    {
                        Details = songName + " | " + artistName,
                        State = duration + " " + progressBar + " " + songLength,
                        Assets = new Assets()
                        {
                            LargeImageKey = "wmp_logo",
                            LargeImageText = $"Listening to \"{songName}\" on Windows Media Player"
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
