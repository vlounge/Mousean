using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mousean.Model;
using Mousean.View.Objects;
using Mousean.View.Renderers;
using Mousean.View.Sound;
using Mousean.View.UI;
using System;
using System.Collections.Generic;
using ButtonState = Mousean.View.UI.ButtonState;

namespace Mousean.Controller;

public class StateMachine {
    
    public Random Random;
    private int _currentState;
    private MusicPlayer _musicPlayer;
    private MonoGameRenderer _renderer;
    private Vector2 _mousePosition;
  
    public Button SkipButton;
    public Button ContinueButton;
  
    public Refferee Referee;
  
    public DarKonMaw DarKonMaw;
    public DontHitCat DontHitCat;
    public List<Wall> Walls;

    public StateMachine()
    {
        _currentState=Constants.StateLoadingScreenStage;
    }
  
    public void SetState(int state)
    {
        _currentState = state;
    }
  
    public void LoadStates()
    {
        _renderer= new MonoGameRenderer();                    // Здесь создается LoadingScreenStage в кострукторе
        // Set LoadingScreenStage
        _musicPlayer = new MusicPlayer();                     // Конструктор загружает основную музыкальную тему
        SkipButton = new Button(ButtonType.Skip);             // Сделайте объект skipButton для StateMachine, он будет использоваться во всех сценах
        _renderer.LoadButton(SkipButton);                     // Загрузите содержимое для объекта skipButton, оно будет использоваться во всех сценах.
        // Set LoadingScreenStageSkipped
        _renderer.AddStage(Constants.LoadingScreenStageSkippedType);
        ContinueButton = new Button(ButtonType.Continue);
        _renderer.LoadButton(ContinueButton);
        // Set LoadingLevel1Stage
        _renderer.AddStage(Constants.LoadingLevel1StageType);
        // Create Referee
        Referee = new Refferee();
        // Constructor + LoadContent for DarKonMaw
        Random = new Random();
        DarKonMaw = new DarKonMaw();
        DontHitCat = new DontHitCat();
        DarKonMaw.SetVelocity(Model.Constants.DarKonMawDefaultVelocity);
        DontHitCat.SetVelocity(Model.Constants.DontHitCatDefaultVelocity);
        //Walls = new List<Wall>() {new Wall(new Vector2(500,500))};
        Walls = new List<Wall> { new Wall(new Vector2(500, 500)) };
        _renderer.AddStage(Constants.Level1StageType);
      
        if(_currentState==Constants.StateLoadingScreenStage)
        {
            Start();
        }
    }
  
    public void Start()
    {
        switch (_currentState)
        {
            case Constants.StateLoadingScreenStage:
                SkipButton.Hide=false;
                _musicPlayer.ChangeSong(View.Constants.MainThemeIndex);
                _musicPlayer.Play();
                _renderer.ResetTimeline(_currentState);
                break;
            case Constants.StateLoadingScreenStageSkipped:
                ContinueButton.Hide=false;
                //_musicPlayer.Play();
                break;
            case Constants.StateLoadingLevel1Stage:
                ContinueButton.Hide=false;
                //_musicPlayer.Play();
                DarKonMaw.Hide = false;
                DontHitCat.Hide = false;
                break;
            case Constants.StateLevel1Stage:
                //ContinueButton.Hide=false;
                _musicPlayer.ChangeSong(View.Constants.RoundThemeIndex);
                _musicPlayer.Play();
                DarKonMaw.Hide = false;
                DarKonMaw.Start();
                DontHitCat.Hide = false;
                Referee.StartTimer();
                //DontHitCat.SetTargetPosition(new Vector2(150,540));
                break;
            default:
                SkipButton.Hide=false;
                _musicPlayer.Play();
                break;
        }
    }
  
    public void Stop()
    {
        switch (_currentState)
        {
            case Constants.StateLoadingScreenStage:
                SkipButton.Hide=true;
                //_musicPlayer.Stop();
                // Здесь создается переменная для остановки апдейтов 
                break;
            case Constants.StateLoadingScreenStageSkipped:
                ContinueButton.Hide=true;
                //_musicPlayer.Stop();
                // Здесь создается переменная для остановки апдейтов 
                break;
            case Constants.StateLoadingLevel1Stage:
                //SkipButton.Hide=false;
                //_musicPlayer.Play();
                _musicPlayer.Stop();
                ContinueButton.Hide=true;
                //DarKonMaw.Hide = true;
                break;
            case Constants.StateLevel1Stage:
                //SkipButton.Hide=false;
                _musicPlayer.Stop();
                DarKonMaw.Stop();
                DontHitCat.Stop();
                //ContinueButton.Hide=true;
                //DarKonMaw.Hide = true;
                break;
            default:
                SkipButton.Hide=true;
                _musicPlayer.Stop();
                break;
        }
    }
    
    public void StateTrigger()
    {
        _mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
      
        switch (_currentState)
        {
            case Constants.StateLoadingScreenStage:
                /*if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    EntryPoint.Game.SetScreen(EntryPoint.Game.GetScreenWidth()-100, EntryPoint.Game.GetScreenHeight()-100);
                    _renderer.ScreeenResized();
                }*/
                if(!SkipButton.Hide)
                {
                    var position = SkipButton.Sprite.Position;
                    position -= _mousePosition;
                    SkipButton.State = position.Length() < SkipButton.Size*_renderer.GetStageScale(_currentState)/2f ? ButtonState.Hover : ButtonState.Active;
                    if(SkipButton.State==ButtonState.Hover && Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        SkipButton.State = ButtonState.Pressed;
                        SkipButton.Pressed = true;
                    }
                    // Если Change переход на следующую сцену
                    if (SkipButton.Pressed && Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        SkipButton.Pressed = false;
                        Stop();
                        _currentState++;
                        Start();
                    }
                }
                break;
            case Constants.StateLoadingScreenStageSkipped:
                /*if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    EntryPoint.Game.SetScreen(EntryPoint.Game.GetScreenWidth()-100, EntryPoint.Game.GetScreenHeight()-100);
                    _renderer.ScreeenResized();
                }*/
                if(!ContinueButton.Hide)
                {
                    var position = ContinueButton.Sprite.Position;
                    position -= _mousePosition;
                    ContinueButton.State = position.Length() < ContinueButton.Size*_renderer.GetStageScale(_currentState)/2f ? ButtonState.Hover : ButtonState.Active;
                    if(ContinueButton.State==ButtonState.Hover && Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        ContinueButton.State = ButtonState.Pressed;
                        ContinueButton.Pressed = true;
                    }
                    // Если Change переход на следующую сцену
                    if (ContinueButton.Pressed && Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        ContinueButton.Pressed = false;
                        Stop();
                        _currentState++;
                        Start();
                    }
                }
                break;
            case Constants.StateLoadingLevel1Stage:
                /*if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    EntryPoint.Game.SetScreen(EntryPoint.Game.GetScreenWidth()-100, EntryPoint.Game.GetScreenHeight()-100);
                    _renderer.ScreeenResized();
                }*/
                if(!ContinueButton.Hide)
                {
                    var position = ContinueButton.Sprite.Position;
                    position -= _mousePosition;
                    ContinueButton.State = position.Length() < ContinueButton.Size*_renderer.GetStageScale(_currentState)/2f ? ButtonState.Hover : ButtonState.Active;
                    if(ContinueButton.State==ButtonState.Hover && Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        ContinueButton.State = ButtonState.Pressed;
                        ContinueButton.Pressed = true;
                    }
                    // Если Change переход на следующую сцену
                    if (ContinueButton.Pressed && Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        ContinueButton.Pressed = false;
                        Stop();
                        _currentState++;
                        Start();
                    }
                }
                break;
            case Constants.StateLevel1Stage:
                if (!DontHitCat.Hide && Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    DontHitCat.SetTargetPosition(_mousePosition);
                }
                if(Referee.GameEnded)
                {
                    Referee.GameEnded=false;
                    Stop();
                    DarKonMaw.Reset();
                    DontHitCat.Reset();
                    DarKonMaw.Hide=true;
                    DontHitCat.Hide=true;
                    DarKonMaw.SetVelocity(Model.Constants.DarKonMawDefaultVelocity);
                    _currentState=0;
                    Start();
                }
                else if(Referee.RoundEnded)
                {
                    Stop();
                    _currentState--;
                    DarKonMaw.Reset();
                    DontHitCat.Reset();
                    Start();
                }
                /*if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    EntryPoint.Game.SetScreen(EntryPoint.Game.GetScreenWidth()-100, EntryPoint.Game.GetScreenHeight()-100);
                    _renderer.ScreeenResized();
                }*/
                /*if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    DarKonMaw.SetVelocity(DarKonMaw.Velocity+50f);
                }*/
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    DontHitCat.Stop();
                }
                /*if (Keyboard.GetState().IsKeyDown(Keys.B))
                {
                    DarKonMaw.Blow();
                }*/
                /*if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    DarKonMaw.Reset();
                }*/
                break;
            default:
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    EntryPoint.Game.SetScreen(EntryPoint.Game.GetScreenWidth()-100, EntryPoint.Game.GetScreenHeight()-100);
                    _renderer.ScreeenResized();
                }
              
                break;
        }
        // Если не на паузе
        _renderer.UpdateStage(_currentState);
    }
  
    public void StateDraw()
    {
        _renderer.DrawStage(_currentState);
    }
}