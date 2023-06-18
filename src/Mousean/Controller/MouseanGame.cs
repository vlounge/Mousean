using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mousean.Controller;

public class MouseanGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    
    private int _screenWidth, _screenHeight; 
    
    public StateMachine StateMachine;     // Рендерер и аудио плеер внутри StateMachine

    public double Elapsed;                  // Время между апдейтами в секундах
    public double FramesPerSecond;          // Фреймы в секунду

    public MouseanGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = Constants.ContentRootDirectory;
        IsMouseVisible = true;
    }
    
    public int GetScreenWidth()
    {
        return _screenWidth;
    }
    
    public int GetScreenHeight()
    {
        return _screenHeight;
    }
    
    public void SetScreen()
    {
        Window.Title = Constants.Title;
        Window.IsBorderless = Constants.BorderlessWindow;
        _graphics.IsFullScreen = Constants.FullscreenMode;
        _screenWidth=_graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        _screenHeight=_graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
        if(_screenWidth<Constants.MinScreenWidth)
        {
            _graphics.IsFullScreen = false;
            _screenWidth = Constants.MinScreenWidth;
            Window.IsBorderless=false;
        }
        _graphics.PreferredBackBufferWidth=_screenWidth;
        if(_screenHeight<Constants.MinScreenHeight)
        {
            _graphics.IsFullScreen = false;
            _screenHeight = Constants.MinScreenHeight;
            Window.IsBorderless=false;
        }
        _graphics.PreferredBackBufferHeight=_screenHeight;
        _graphics.ApplyChanges();
    }
    
    public void SetScreen(int width, int height)
    {
        Window.Title = Constants.Title;
        Window.IsBorderless = false;
        _graphics.IsFullScreen = false;
        _screenWidth = width;
        _screenHeight = height;
        if(_screenWidth<Constants.MinScreenWidth)
        {
            _screenWidth = Constants.MinScreenWidth;
        }
        _graphics.PreferredBackBufferWidth=_screenWidth;
        if(_screenHeight<Constants.MinScreenHeight)
        {
            _screenHeight = Constants.MinScreenHeight;
        }
        _graphics.PreferredBackBufferHeight=_screenHeight;
        _graphics.ApplyChanges();
    }
        
    public override string ToString()
    {
        return $"{_screenWidth}x{_screenHeight} FPS:{FramesPerSecond:N0}";
    }

    protected override void Initialize()
    {
        SetScreen();
        StateMachine = new StateMachine();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        StateMachine.LoadStates();     // Загрузка всех состояний музыкальный контент и изображения
        // Сейчас StateMachine запустит DefaultState - StateLoadingScreen
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        // Логика апдейтов
        Elapsed = gameTime.ElapsedGameTime.TotalSeconds;
        FramesPerSecond = 1.0f / Elapsed;
        
        StateMachine.StateTrigger();   //  Выполнить или остановить текущее состояние или воспроизвести какое-либо действие со шкалой времени

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        StateMachine.StateDraw();      // Метод отрисовки внутри StateMachine

        base.Draw(gameTime);
    }
}
