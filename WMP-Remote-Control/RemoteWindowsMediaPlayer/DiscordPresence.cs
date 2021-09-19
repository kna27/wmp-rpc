using DiscordRPC;
using MediaPlayerController;
using System;
using System.Diagnostics;
using System.Threading;

namespace WindowsMediaPlayerDiscordPresence
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
        /// <summary>
        /// The character for a filled-in section of the progress bar
        /// </summary>
        readonly static char filled = '◼';
        /// <summary>
        /// The character for an empty section of the progress bar
        /// </summary>
        readonly static char empty = '▭';

        static public void Main(String[] args)
        {
            if (Process.GetProcessesByName("WMP Discord Presence").Length > 1)
            {
                Environment.Exit(1);
            }

            // Get Discord app ID
            string appId = Properties.Resources.appId.Replace('\n', '\0');

            // Start a new Discord RPC client
            client = new DiscordRpcClient(appId);
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.Initialize();

            // Run forever
            while (true)
            {
                // Get new data from Windows Media Player
                WindowsMediaPlayerController wmpc = new WindowsMediaPlayerController();
                SongDetails x = wmpc.GetCurrentSongDetails();

                // If currently playing
                if (wmpc.GetPlayingState() == PlayingState.Playing)
                {
                    // Format song and artist information
                    songName = wmpc.GetCurrentSongName();
                    if (x.SongName.LastIndexOf(" - " + wmpc.GetCurrentSongName().ToString()) == -1)
                    {
                        artistName = "";
                    }
                    else
                    {
                        artistName = x.SongName.Substring(0,
                        x.SongName.LastIndexOf(" - " + wmpc.GetCurrentSongName().ToString()));
                    }

                    durationTs = TimeSpan.FromSeconds(wmpc.GetCurrentSongTime());
                    duration = $"{durationTs.Minutes:D2}:{durationTs.Seconds:D2}";
                    lengthTs = TimeSpan.FromSeconds(x.Duration);
                    songLength = $"{lengthTs.Minutes:D2}:{lengthTs.Seconds:D2}";

                    // Get % of how much of the song has been played and round to nearest 10%
                    double percentage = (durationTs.TotalSeconds / lengthTs.TotalSeconds) * 100;
                    int rounded = (int)(Math.Round(percentage / 10.0) * 10);

                    // Add a filled char for every 10% and an empty char for the remaining percent
                    string progressBar = "";
                    for (int i = 0; i < rounded / 10; i++)
                    {
                        progressBar += filled;
                    }
                    for (int i = 0; i < 10 - rounded / 10; i++)
                    {
                        progressBar += empty;
                    }

                    // Update RPC with new information
                    client.SetPresence(new RichPresence()
                    {
                        Details = songName + (artistName != "" ? " | " + artistName : ""),
                        State = duration + " " + progressBar + " " + songLength,
                        Assets = new Assets()
                        {
                            LargeImageKey = "wmp_logo",
                            LargeImageText = $"Listening to \"{songName}\" on Windows Media Player"
                        }
                    });
                }
                // If not currently listening to a song
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
                // Wait one second
                Thread.Sleep(1000);
            }
        }
    }
}
