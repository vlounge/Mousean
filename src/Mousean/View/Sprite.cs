using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mousean.View;

public class Sprite
{
  public Texture2D Image;
  public Vector2 Origin;
  public Vector2 Position;
  
  public Sprite(Texture2D image, Vector2 origin, Vector2 position)
  {
      Image = image;
      Origin = origin;
      Position = position;
  }
}