// Decompiled with JetBrains decompiler
// Type: MediaPlayerController.SongDetails
// Assembly: MediaPlayerController, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF010E8D-48F2-4017-8165-88EFC4873936
// Assembly location: C:\Program Files (x86)\WmpRemote Server\MediaPlayerController.dll

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
      return string.Empty + "<SongNumber>" + (object) this.SongNumber + "</SongNumber>" + "<SongName>" + this.SongName + "</SongName>" + "<Duration>" + (object) this.Duration + "</Duration>";
    }

    public string GetTimeString()
    {
      return new TimeSpan(0, 0, (int) this.Duration).ToString();
    }
  }
}
