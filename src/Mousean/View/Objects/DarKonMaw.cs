using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mousean.Controller;
using Mousean.Model;

namespace Mousean.View.Objects;

public class DarKonMaw {
    public Vector2 CurrentPosition;
    public Vector2 TargetPosition;
    public float TargetPositionMargin = Model.Constants.TargetPositionMargin;
    public float Size = Constants.DefaultObjectSize;
    public float OffsetX;
    public float OffsetY;
    public bool AspectOffset=false;
    public Sprite Sprite;
    public Sprite BlowSprite;
    public bool Hide = true;
    public bool Blowed = false;
    public Rectangle[] Rectangle;
    public Rectangle[] BlowRectangle;
    public int CurrentRect = 0;
    public int CurrentBlowRect = 0;
    public double AnimationTimeline = 0;
    public double BlowAnimationTimeline = 0;
    public float Velocity;
    public double TargetTimer;
    public double SpeedBoostTimer;
    public float SpeedBoost=0;
    public bool LevelStarted = false;
    
    public DarKonMaw()
    {
        TargetTimer = 0;
        Velocity = Model.Constants.DarKonMawDefaultVelocity;
        Rectangle = new Rectangle[Constants.DarKonMawFrames];
        for (var i=0; i<Constants.DarKonMawFrames; i++)
        {
            Rectangle[i] = new Rectangle(0, (int)(i*Size), (int)Size, (int)Size);
        }
        BlowRectangle = new Rectangle[Constants.BlowFrames];
        for (var i=0; i<Constants.BlowFrames; i++)
        {
            BlowRectangle[i] = new Rectangle(0, (int)(i*Size), (int)Size, (int)Size);
        }
        CurrentPosition = new Vector2(Model.Constants.DefaultArenaWidth - Constants.DarKonMawStartPositionOffsetFromRight, Model.Constants.DefaultArenaHeight/2f);
        TargetPosition = CurrentPosition;
        LoadContent();
        SetOffset(EntryPoint.Game.GetScreenWidth(), EntryPoint.Game.GetScreenHeight());
    }
    
    public void LoadContent()
    {
        var image = EntryPoint.Game.Content.Load<Texture2D>(Constants.DarKonMawSprite);
        Sprite = new Sprite(image, new Vector2(Size/2f, Size/2f), CurrentPosition);
        image = EntryPoint.Game.Content.Load<Texture2D>(Constants.BlowSprite);
        BlowSprite = new Sprite(image, new Vector2(Size/2f, Size/2f), CurrentPosition);
    }
    
    public void SetVelocity(float velocity)
    {
        var previousVelocity = Velocity;
        Velocity = velocity;
        TargetPositionMargin *= Velocity/previousVelocity;
        //if (Velocity == Model.Constants.DarKonMawDefaultVelocity) TargetPositionMargin = Model.Constants.TargetPositionMargin;
    }
    
    public void Start()
    {
        LevelStarted = true;
    }
    
    public void SetTargetPosition(Vector2 targetPosition)
    {
        TargetPosition = targetPosition;
    }
    
    public void Stop()
    {
        TargetPosition = CurrentPosition;
        LevelStarted = false;
    }
    
    public void Blow()
    {
        Blowed = true;
    }
    
    public void Reset()
    {
        Blowed = false;
        BlowAnimationTimeline = 0d;
        AnimationTimeline = 0d;
        CurrentPosition = new Vector2(Model.Constants.DefaultArenaWidth - Constants.DarKonMawStartPositionOffsetFromRight, Model.Constants.DefaultArenaHeight/2f);
        TargetPosition = CurrentPosition;
    }
    
    public void SetOffset(int width, int height)
    {
        float screenWidth = (float)EntryPoint.Game.GetScreenWidth();
        float screenHeight = (float)EntryPoint.Game.GetScreenHeight();
        OffsetX = screenWidth / Model.Constants.DefaultArenaWidth;
        OffsetY = screenHeight / Model.Constants.DefaultArenaHeight;
        float arenaAspect = (float)Model.Constants.DefaultArenaWidth / (float)Model.Constants.DefaultArenaHeight;
        float screenAspect = screenWidth / screenHeight;
        if (screenAspect>arenaAspect)
        {
            AspectOffset = true;
            OffsetX *= arenaAspect/screenAspect;
        }
        else
        {
            AspectOffset = false;
        }
    }
    
    public void Update()
    {
        if(Blowed)
        {
            Stop();
            BlowAnimationTimeline += Constants.BlowAnimationSpeed * (float)EntryPoint.Game.Elapsed;
            if (BlowAnimationTimeline >= (double)Constants.BlowFrames)
            {
                BlowAnimationTimeline = (double)Constants.BlowFrames-1d;
            }
            CurrentBlowRect = (int)BlowAnimationTimeline;
        }
        else
        {
            Vector2 pathVector = TargetPosition - CurrentPosition;
            if (pathVector.Length()<TargetPositionMargin)
            {
                TargetPosition=CurrentPosition;

                if (CurrentRect > 0 && CurrentRect != Constants.AdditionGroundedFrame)
                {
                    AnimationTimeline += Constants.DarKonMawAnimationSpeed * (float)EntryPoint.Game.Elapsed * Velocity/Model.Constants.DarKonMawDefaultVelocity;
                    if (AnimationTimeline > (double)Constants.DarKonMawFrames)
                    {
                        AnimationTimeline =0f;
                        CurrentRect = 0;
                    }
                    CurrentRect = (int)AnimationTimeline;
                }
            }
            else
            {
                var seconds = EntryPoint.Game.Elapsed;
                float path = Velocity * (float)seconds;
                float factor = path / pathVector.Length();
                pathVector *= factor;
                var pathCorrector = new PathCorrector(CurrentPosition, TargetPosition, pathVector);
                TargetPosition = pathCorrector.GetTargetPosition();
                pathVector = pathCorrector.GetRelativePathStep();
                CurrentPosition += pathVector;
                AnimationTimeline += Constants.DarKonMawAnimationSpeed * (float)seconds * Velocity/Model.Constants.DarKonMawDefaultVelocity;
                if (AnimationTimeline >= (double)Constants.DarKonMawFrames) AnimationTimeline -= (double)Constants.DarKonMawFrames;
                CurrentRect = (int)AnimationTimeline;
            }
            if(LevelStarted)
            {
                var radar = new AIRadar(TargetPosition);
                TargetPosition = radar.GetTargetPosition();
            }
        }  
    }
    
    public void Draw(SpriteBatch spriteBatch, float scale)
    {
        var offsetPosition = new Vector2();
        if (AspectOffset)
        {
            offsetPosition.X = CurrentPosition.X*OffsetX + (EntryPoint.Game.GetScreenWidth() - Model.Constants.DefaultArenaWidth*scale)/2f;
        }
        else
        {
            offsetPosition.X = CurrentPosition.X*OffsetX;
        }
        offsetPosition.Y = CurrentPosition.Y*OffsetY;
        if (!Blowed)
        {
            spriteBatch.Draw(Sprite.Image, offsetPosition, Rectangle[CurrentRect], Color.White,
                Constants.SpriteDefaultRotation, Sprite.Origin, scale, SpriteEffects.None, Constants.SpriteLayerDepth);
        }
        else
        {
            spriteBatch.Draw(BlowSprite.Image, offsetPosition, BlowRectangle[CurrentBlowRect], Color.White,
                Constants.SpriteDefaultRotation, BlowSprite.Origin, scale, SpriteEffects.None, Constants.SpriteLayerDepth);
        }
    }
}