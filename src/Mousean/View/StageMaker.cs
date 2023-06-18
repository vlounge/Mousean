using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mousean.Controller;

namespace Mousean.View;

public static class StageMaker {
    public static void LoadingScreenStageMaker(Stage stage)
    {
        //Sprite 0
        var image = EntryPoint.Game.Content.Load<Texture2D>("Sprites/Text");
        var origin = new Vector2((float)image.Width/2f, (float)image.Height/2f);
        var position = new Vector2((float)EntryPoint.Game.GetScreenWidth()/2f, 1900f);
        var sprite = new Sprite(image, origin, position);
        var spriteId = stage.AddSprite(sprite);
        var actionType = ActionType.Move;
        stage.CreateTimeline(false);
        var action = new Action(spriteId, actionType, 0, 90, sprite.Position, new Vector2((float)EntryPoint.Game.GetScreenWidth()/2f, 250f));
        stage.AddSpriteAction(action);
        
        //Sprite 1
        image = EntryPoint.Game.Content.Load<Texture2D>("Sprites/MouseanLogo");
        origin = new Vector2((float)image.Width/2f, (float)image.Height/2f);
        position = new Vector2((float)EntryPoint.Game.GetScreenWidth()/2f, (float)EntryPoint.Game.GetScreenHeight()/2f);
        sprite = new Sprite(image, origin, position);
        spriteId = stage.AddSprite(sprite);
        actionType = ActionType.Move;
        action = new Action(spriteId, actionType, 23, 46, sprite.Position, new Vector2((float)EntryPoint.Game.GetScreenWidth()/2f, 130f));
        stage.AddSpriteAction(action);
    }
    
    public static void LoadingScreenSkippedStageMaker(Stage stage)
    {
        //Sprite 0
        var image = EntryPoint.Game.Content.Load<Texture2D>("Sprites/Text");
        var origin = new Vector2((float)image.Width/2f, (float)image.Height/2f);
        var position = new Vector2((float)EntryPoint.Game.GetScreenWidth()/2f, 250f);
        var sprite = new Sprite(image, origin, position);
        stage.AddSprite(sprite);

        //Sprite 1
        image = EntryPoint.Game.Content.Load<Texture2D>("Sprites/MouseanLogo");
        origin = new Vector2((float)image.Width/2f, (float)image.Height/2f);
        position = new Vector2((float)EntryPoint.Game.GetScreenWidth()/2f, 130f);
        sprite = new Sprite(image, origin, position);
        stage.AddSprite(sprite);
    }
    
    public static void LoadingLevel1StageMaker(Stage stage)
    {
        //Sprite 0
        var image = EntryPoint.Game.Content.Load<Texture2D>("Sprites/Arena");
        var origin = new Vector2((float)image.Width/2f, (float)image.Height/2f);
        var position = new Vector2((float)EntryPoint.Game.GetScreenWidth()/2f, (float)EntryPoint.Game.GetScreenHeight()/2f);
        var sprite = new Sprite(image, origin, position);
        stage.AddSprite(sprite);
        
        //Sprite 1
        image = EntryPoint.Game.Content.Load<Texture2D>("Hints/Level1");
        origin = new Vector2((float)image.Width/2f, (float)image.Height/2f);
        position = new Vector2((float)EntryPoint.Game.GetScreenWidth()/2f, (float)EntryPoint.Game.GetScreenHeight()/2f);
        sprite = new Sprite(image, origin, position);
        stage.AddSprite(sprite);
    }
    
    public static void Level1StageMaker(Stage stage)
    {
        //Sprite 0
        var image = EntryPoint.Game.Content.Load<Texture2D>("Sprites/Arena");
        var origin = new Vector2((float)image.Width/2f, (float)image.Height/2f);
        var position = new Vector2((float)EntryPoint.Game.GetScreenWidth()/2f, (float)EntryPoint.Game.GetScreenHeight()/2f);
        var sprite = new Sprite(image, origin, position);
        stage.AddSprite(sprite);
    }
}