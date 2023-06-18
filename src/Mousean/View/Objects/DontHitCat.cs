using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mousean.Controller;
using Mousean.Model;

namespace Mousean.View.Objects;

public class DontHitCat {
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
    
    public DontHitCat()
    {
        Velocity = Model.Constants.DontHitCatDefaultVelocity;
        Rectangle = new Rectangle[Constants.DontHitCatFrames];
        for (var i=0; i<Constants.DontHitCatFrames; i++)
        {
            Rectangle[i] = new Rectangle(0, (int)(i*Size), (int)Size, (int)Size);
        }
        BlowRectangle = new Rectangle[Constants.BlowFrames];
        for (var i=0; i<Constants.BlowFrames; i++)
        {
            BlowRectangle[i] = new Rectangle(0, (int)(i*Size), (int)Size, (int)Size);
        }
        CurrentPosition = new Vector2(0 + Constants.DontHitCatStartPositionOffsetFromLeft, Model.Constants.DefaultArenaHeight/2f);
        TargetPosition = CurrentPosition;
        LoadContent();
        SetOffset(EntryPoint.Game.GetScreenWidth(), EntryPoint.Game.GetScreenHeight());
    }
    
    public void LoadContent()
    {
        var image = EntryPoint.Game.Content.Load<Texture2D>(Constants.DontHitCatSprite);
        Sprite = new Sprite(image, new Vector2(Size/2f, Size/2f), CurrentPosition);
        image = EntryPoint.Game.Content.Load<Texture2D>(Constants.BlowSprite);
        BlowSprite = new Sprite(image, new Vector2(Size/2f, Size/2f), CurrentPosition);
    }
    
    public void SetVelocity(float velocity)
    {
        var previousVelocity = Velocity;
        Velocity = velocity;
        TargetPositionMargin *= Velocity/previousVelocity;
        //if (Velocity == Model.Constants.DontHitCatDefaultVelocity) TargetPositionMargin = Model.Constants.TargetPositionMargin;
    }
    
    public void SetTargetPosition(Vector2 targetPosition)
    {
        if(AspectOffset)
        {
            targetPosition.X -= (EntryPoint.Game.GetScreenWidth() - Model.Constants.DefaultArenaWidth * OffsetX) / 2;
            targetPosition.X /= OffsetX;
            
        }
        else
        {
            targetPosition.X /= OffsetX;
        }
        targetPosition.Y /= OffsetY;
        TargetPosition = targetPosition;
    }

    public void Stop()
    {
        TargetPosition = CurrentPosition;
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
        CurrentPosition = new Vector2(Constants.DontHitCatStartPositionOffsetFromLeft, Model.Constants.DefaultArenaHeight/2f);
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
                    AnimationTimeline += Constants.DontHitCatAnimationSpeed * (float)EntryPoint.Game.Elapsed * Velocity/Model.Constants.DontHitCatDefaultVelocity;
                    if (AnimationTimeline > (double)Constants.DontHitCatFrames)
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
                AnimationTimeline += Constants.DontHitCatAnimationSpeed * (float)seconds * Velocity/Model.Constants.DontHitCatDefaultVelocity;
                if (AnimationTimeline >= (double)Constants.DontHitCatFrames) AnimationTimeline -= (double)Constants.DontHitCatFrames;
                CurrentRect = (int)AnimationTimeline;
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