using Microsoft.Xna.Framework;

namespace Mousean.View;

public class Action
{
    public double StartTime;
    public ActionType Type;
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public double EndTime;
    public int SpriteId;
    public double TimeCounter;
    
    
    public Action(int spriteId, ActionType type, double startTime, double endTime, Vector2 startPosition, Vector2 endPosition)
    {
        SpriteId = spriteId;
        Type = type;
        StartTime = startTime;
        EndTime = endTime;
        StartPosition = startPosition;
        EndPosition = endPosition;
        TimeCounter =0;
    }
    
    public Action(int spriteId, ActionType type, double startTime, double endTime)
    {
        SpriteId = spriteId;
        StartTime = startTime;
        EndTime = endTime;
        Type = type;
        TimeCounter = 0;
    }
}