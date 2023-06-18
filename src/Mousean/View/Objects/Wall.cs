using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mousean.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mousean.Model;

namespace Mousean.View.Objects
{
    public class Wall
    {
        public Vector2 Position;
        public float Size = Model.Constants.WallSize;
        public float OffsetX;
        public float OffsetY;
        public bool AspectOffset = false;
        public Sprite Sprite;

        public Wall(Vector2 position) 
        {
            Position = position;
            Sprite = new Sprite(EntryPoint.Game.Content.Load<Texture2D>(Constants.WallSprite), new Vector2(Size / 2f, Size / 2f), Position);
            SetOffset(EntryPoint.Game.GetScreenWidth(), EntryPoint.Game.GetScreenHeight());
        }

        public void SetOffset(int width, int height)
        {
            float screenWidth = (float)EntryPoint.Game.GetScreenWidth();
            float screenHeight = (float)EntryPoint.Game.GetScreenHeight();
            OffsetX = screenWidth / Model.Constants.DefaultArenaWidth;
            OffsetY = screenHeight / Model.Constants.DefaultArenaHeight;
            float arenaAspect = (float)Model.Constants.DefaultArenaWidth / (float)Model.Constants.DefaultArenaHeight;
            float screenAspect = screenWidth / screenHeight;
            if (screenAspect > arenaAspect)
            {
                AspectOffset = true;
                OffsetX *= arenaAspect / screenAspect;
            }
            else
            {
                AspectOffset = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float scale)
        {
            var offsetPosition = new Vector2();
            if (AspectOffset)
            {
                offsetPosition.X = Position.X * OffsetX + (EntryPoint.Game.GetScreenWidth() - Model.Constants.DefaultArenaWidth * scale) / 2f;
            }
            else
            {
                offsetPosition.X = Position.X * OffsetX;
            }
            offsetPosition.Y = Position.Y * OffsetY;
            spriteBatch.Draw(Sprite.Image, offsetPosition, null, Color.White,
                Constants.SpriteDefaultRotation, Sprite.Origin, scale, SpriteEffects.None, Constants.SpriteLayerDepth);
            
        }
    }
}
