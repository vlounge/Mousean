using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mousean.Controller;
using Mousean.View.UI;
using ButtonState = Mousean.View.UI.ButtonState;

namespace Mousean.View.Renderers;

public class MonoGameRenderer {
    private readonly List<Stage> _stageList;
    private readonly ContentManager _content = EntryPoint.Game.Content;
    private readonly SpriteBatch _spriteBatch;
    private readonly SpriteFont _mono12SpriteFont;
    //private Texture2D _cursorSprite;
    //private MouseCursor _cursor;
    
    
    public MonoGameRenderer()
    {
      _spriteBatch = new SpriteBatch(EntryPoint.Game.GraphicsDevice);
      _stageList = new List<Stage>();
      //Настройка курсора
      var cursorSprite = _content.Load<Texture2D>(Constants.DefaultCursor);
      var cursor = MouseCursor.FromTexture2D(cursorSprite, 5, 5);
      Mouse.SetCursor(cursor);
      
      AddStage(Controller.Constants.LoadingScreenStageType);
      _mono12SpriteFont = _content.Load<SpriteFont>(Constants.DefaultInfoFont);
    }
    
    //Возвращает id сцены
    public int AddStage(int stageType)
    {
        var stage = new Stage(EntryPoint.Game.GetScreenWidth(),EntryPoint.Game.GetScreenHeight());
        switch (stageType)
        {
          case Controller.Constants.LoadingScreenStageType:
              StageMaker.LoadingScreenStageMaker(stage);
              break;
          case Controller.Constants.LoadingScreenStageSkippedType:
              StageMaker.LoadingScreenSkippedStageMaker(stage);
              break;
          case Controller.Constants.LoadingLevel1StageType:
              StageMaker.LoadingLevel1StageMaker(stage);
              break;
          case Controller.Constants.Level1StageType:
              StageMaker.Level1StageMaker(stage);
              break;
          default:
              StageMaker.LoadingScreenStageMaker(stage);
              break;
        }
        _stageList.Add(stage);
        return _stageList.Count-1;
    }
    
    public void LoadButton(Button button)
    {
        Texture2D image;
        switch (button.Type)
        {
            case(ButtonType.Skip):
                image = _content.Load<Texture2D>(Constants.DefaultSkipButton);
                button.Sprite = new Sprite(image, new Vector2((float)image.Width/2f, (float)image.Width/2f),
                    new Vector2(EntryPoint.Game.GetScreenWidth() - button.Size, EntryPoint.Game.GetScreenHeight() - button.Size));
                button.ActiveRect = new Rectangle(0 ,0 ,image.Width, image.Width);
                button.HoverRect = new Rectangle(0 ,image.Width ,image.Width, image.Width);
                button.PressedRect = new Rectangle(0 ,image.Width*2 ,image.Width, image.Width);
                break;
            case(ButtonType.Continue):
                image = _content.Load<Texture2D>(Constants.DefaultContinueButton);
                button.Sprite = new Sprite(image, new Vector2((float)image.Width/2f, (float)image.Width/2f),
                    new Vector2(EntryPoint.Game.GetScreenWidth() - button.Size, EntryPoint.Game.GetScreenHeight() - button.Size));
                button.ActiveRect = new Rectangle(0 ,0 ,image.Width, image.Width);
                button.HoverRect = new Rectangle(0 ,image.Width ,image.Width, image.Width);
                button.PressedRect = new Rectangle(0 ,image.Width*2 ,image.Width, image.Width);
                break;
            default:
                image = _content.Load<Texture2D>(Constants.DefaultSkipButton);
                button.Sprite = new Sprite(image, new Vector2((float)image.Width/2f, (float)image.Width/2f),
                    new Vector2(EntryPoint.Game.GetScreenWidth() - button.Size, EntryPoint.Game.GetScreenHeight() - button.Size));
                button.ActiveRect = new Rectangle(0 ,0 ,image.Width, image.Width);
                button.HoverRect = new Rectangle(0 ,image.Width ,image.Width, image.Width*2);
                button.PressedRect = new Rectangle(0 ,image.Width*2 ,image.Width, image.Width*3);
                break;
        }
    }
    
    public void ScreeenResized()
    {
        var screenWidth = EntryPoint.Game.GetScreenWidth();
        var screenHeight = EntryPoint.Game.GetScreenHeight();
        foreach (var stage in _stageList)
        {
            stage.SetStage(screenWidth, screenHeight);
        }
        // Изменение позиции кноки относительно скейла на  LoadingGameStage  
        EntryPoint.Game.StateMachine.SkipButton.Sprite.Position.X *= _stageList[0].GetScaleX();
        EntryPoint.Game.StateMachine.SkipButton.Sprite.Position.Y *= _stageList[0].GetScaleY();
        // Изменение позиции кноки относительно скейла на  LoadingGameSkippedStage  
        EntryPoint.Game.StateMachine.ContinueButton.Sprite.Position.X *= _stageList[0].GetScaleX();
        EntryPoint.Game.StateMachine.ContinueButton.Sprite.Position.Y *= _stageList[0].GetScaleY();
        //Установить смещение для DarKonMaw, отличное от 1080p
        EntryPoint.Game.StateMachine.DarKonMaw.SetOffset(screenWidth, screenHeight);
        EntryPoint.Game.StateMachine.DontHitCat.SetOffset(screenWidth, screenHeight);
        foreach(var wall in EntryPoint.Game.StateMachine.Walls)
        {
            wall.SetOffset(screenWidth, screenHeight);
        }
    }
    
    public float GetStageScale(int stageId)
    {
        return _stageList[stageId].GetScale();
    }
    
    public void UpdateStage(int id)
    {
        _stageList[id].UpdateSprites();
        EntryPoint.Game.StateMachine.DarKonMaw.Update();
        EntryPoint.Game.StateMachine.DontHitCat.Update();
        EntryPoint.Game.StateMachine.Referee.Update();
    }
    
    public void ResetTimeline(int id)
    {
        _stageList[id].ResetTimeline();
    }
    
    
    public void DrawStage(int id)
    {
        _spriteBatch.Begin();
        _stageList[id].DrawSprites(_spriteBatch);
        //Рисование объектов
        if (id > 4)
        {
            foreach (var wall in EntryPoint.Game.StateMachine.Walls)
            {
                wall.Draw(_spriteBatch, _stageList[id].GetScale());
            }
        }
        if (!EntryPoint.Game.StateMachine.DarKonMaw.Hide)
        {
            EntryPoint.Game.StateMachine.DarKonMaw.Draw(_spriteBatch, _stageList[id].GetScale());
        }
        if(!EntryPoint.Game.StateMachine.DontHitCat.Hide)
        {
            EntryPoint.Game.StateMachine.DontHitCat.Draw(_spriteBatch, _stageList[id].GetScale());
        }

        //Рисунок пользовательского интерфейса

        var uiSstringColor = new Color(0.5f,0.5f,0.5f,1.0f);
        _spriteBatch.DrawString(_mono12SpriteFont, EntryPoint.Game.ToString(), new Vector2(20,10), uiSstringColor);
        if (id>1) _spriteBatch.DrawString(_mono12SpriteFont,
            EntryPoint.Game.StateMachine.Referee.ToString(),
            new Vector2(20,30), Color.White);
        // UI DrawButtons
        var button = EntryPoint.Game.StateMachine.SkipButton;
        if(!button.Hide)
        {
            switch (button.State)
            {
                case(ButtonState.Active):
                    _spriteBatch.Draw(button.Sprite.Image, button.Sprite.Position, button.ActiveRect, Color.White,
                        Constants.SpriteDefaultRotation, button.Sprite.Origin, _stageList[id].GetScale(), SpriteEffects.None, Constants.SpriteLayerDepth);
                    break;
                case(ButtonState.Hover):
                    _spriteBatch.Draw(button.Sprite.Image, button.Sprite.Position, button.HoverRect, Color.White,
                        Constants.SpriteDefaultRotation, button.Sprite.Origin, _stageList[id].GetScale(), SpriteEffects.None, Constants.SpriteLayerDepth);
                    break;
                case(ButtonState.Pressed):
                    _spriteBatch.Draw(button.Sprite.Image, button.Sprite.Position, button.PressedRect, Color.White,
                        Constants.SpriteDefaultRotation, button.Sprite.Origin, _stageList[id].GetScale(), SpriteEffects.None, Constants.SpriteLayerDepth);
                    break;
                default:
                    _spriteBatch.Draw(button.Sprite.Image, button.Sprite.Position, button.ActiveRect, Color.White,
                        Constants.SpriteDefaultRotation, button.Sprite.Origin, _stageList[id].GetScale(), SpriteEffects.None, Constants.SpriteLayerDepth);
                    break;
            }
        }
        button = EntryPoint.Game.StateMachine.ContinueButton;
        if(!button.Hide)
        {
            switch (button.State)
            {
                case(ButtonState.Active):
                    _spriteBatch.Draw(button.Sprite.Image, button.Sprite.Position, button.ActiveRect, Color.White,
                        Constants.SpriteDefaultRotation, button.Sprite.Origin, _stageList[id].GetScale(), SpriteEffects.None, Constants.SpriteLayerDepth);
                    break;
                case(ButtonState.Hover):
                    _spriteBatch.Draw(button.Sprite.Image, button.Sprite.Position, button.HoverRect, Color.White,
                        Constants.SpriteDefaultRotation, button.Sprite.Origin, _stageList[id].GetScale(), SpriteEffects.None, Constants.SpriteLayerDepth);
                    break;
                case(ButtonState.Pressed):
                    _spriteBatch.Draw(button.Sprite.Image, button.Sprite.Position, button.PressedRect, Color.White,
                        Constants.SpriteDefaultRotation, button.Sprite.Origin, _stageList[id].GetScale(), SpriteEffects.None, Constants.SpriteLayerDepth);
                    break;
                default:
                    _spriteBatch.Draw(button.Sprite.Image, button.Sprite.Position, button.ActiveRect, Color.White,
                        Constants.SpriteDefaultRotation, button.Sprite.Origin, _stageList[id].GetScale(), SpriteEffects.None, Constants.SpriteLayerDepth);
                    break;
            }
        }
        _spriteBatch.End();
    }
}