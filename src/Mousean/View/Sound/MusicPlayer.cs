using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace Mousean.View.Sound;

using Controller;

public class MusicPlayer
{
  private readonly List<Song> _songList;
  private int _currentSong;
  private readonly float _volume;
  private readonly bool _isRepeating;
  
  public MusicPlayer()
  {
      _songList = new List<Song> { EntryPoint.Game.Content.Load<Song>(View.Constants.MainThemeSong),  EntryPoint.Game.Content.Load<Song>(View.Constants.RoundThemeSong)};
      _volume=View.Constants.DefailtVolume;
      _currentSong=View.Constants.MainThemeIndex;
      _isRepeating=View.Constants.MainThemeRepeating;
  }
  
  public void Play()
  {
      MediaPlayer.Volume=_volume;
      MediaPlayer.IsRepeating=_isRepeating;
      MediaPlayer.Play(_songList[_currentSong]);
  }
  
  public void ChangeSong(int index)
  {
      if(index<0 && index>=_songList.Count) _currentSong = View.Constants.MainThemeIndex;
          else _currentSong = index;
  }
  
  public void Stop()
  {
      MediaPlayer.Stop();
  }
  
}