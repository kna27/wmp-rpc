using System;

namespace MediaPlayerController
{
    public class SongDetails
    {
        public string SongName;
        public int SongNumber;
        public double Duration;
        public string SongPath;

        public override string ToString()
        {
            return string.Empty + "<SongNumber>" + (object)this.SongNumber + "</SongNumber>" + "<SongName>" + this.SongName + "</SongName>" + "<Duration>" + (object)this.Duration + "</Duration>";
        }

        public string GetTimeString()
        {
            return new TimeSpan(0, 0, (int)this.Duration).ToString();
        }
    }
}
