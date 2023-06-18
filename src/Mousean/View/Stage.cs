using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mousean.Controller;

namespace Mousean.View;

public class Stage
{
  private readonly List<Sprite> _spriteList;
  private int _stageWidth = Constants.DefaultStageWidth;
  private int _stageHeight = Constants.DefaultStageHeight;
  private readonly int _stageSize = Constants.DefaultStageHeight;
  private float _stageScale;
  private float _stageScaleX;
  private float _stageScaleY;
  private TimeLine _timeLine;
  
  public Stage(int width, int height)
  {
      _spriteList = new List<Sprite>();
      SetStage(width, height);
  }
  
  public void SetStage(int width, int height)
  {
      var previousStageWidth = _stageWidth;
      var previousStageHeight = _stageHeight;
      _stageWidth = width;
      _stageHeight = height;
      if(_stageWidth>_stageHeight)
      {
          _stageScale=(float)_stageHeight/(float)_stageSize;
      }
      else
      {
          _stageScale=(float)_stageWidth/(float)_stageSize;
      }
      
      _stageScaleX = (float)_stageWidth/(float)previousStageWidth;
      _stageScaleY = (float)_stageHeight/(float)previousStageHeight;
      
      foreach (var sprite in _spriteList)
      {
          var newPosition = new Vector2(sprite.Position.X*_stageScaleX, sprite.Position.Y*_stageScaleY);
          sprite.Position = newPosition;
      }
      if (_timeLine!=null)
      {
          _timeLine.SetActions(_stageScaleX, _stageScaleY);
      }
  }
  
  public float GetScale()
  {
      return _stageScale;
  }
  
  public float GetScaleX()
  {
      return _stageScaleX;
  }
  
  public float GetScaleY()
  {
      return _stageScaleY;
  }
  
  public int AddSprite(Sprite sprite)
  {
      _spriteList.Add(sprite);
      return  _spriteList.Count-1;
  }
  
  public void MoveSprite(int spriteId, Vector2 startPosition, Vector2 endPosition, float proportion)
  {
      var vector = (endPosition - startPosition)*proportion+startPosition;
      _spriteList[spriteId].Position = vector;
  }
  
  public void CreateTimeline(bool isLooped)
  {
      _timeLine = new TimeLine(0, isLooped);
  }
  
  public void ResetTimeline()
  {
      _timeLine.Reset();
  }
  
  public void AddSpriteAction(Action action)
  {
      _timeLine.SetTimeRange(action.EndTime);
      _timeLine.AddAction(action);
  }
  
  public void UpdateSprites()
  {
      if (_timeLine != null)
      {
          var actions = _timeLine.GetActions();
          if (actions is { Count: > 0 })
          {
              foreach (var action in actions)
              {
                  switch (action.Type)
                  {
                      case ActionType.Move:
                          MoveSprite(action.SpriteId, action.StartPosition, action.EndPosition, (float)action.TimeCounter/(float)(action.EndTime-action.StartTime));
                          break;
                      default:
                          MoveSprite(action.SpriteId, action.StartPosition, action.EndPosition, (float)(action.EndTime-action.StartTime)/(float)action.TimeCounter);
                          break;
                  }   
              }
          }
      }
  }
    
  public void DrawSprites(SpriteBatch spriteBatch)
  {
      foreach (var sprite in _spriteList)
      {
          spriteBatch.Draw(sprite.Image, sprite.Position, null, Color.White, Constants.SpriteDefaultRotation, sprite.Origin, _stageScale, SpriteEffects.None, Constants.SpriteLayerDepth);
      }
  }
}