using Microsoft.Xna.Framework;

namespace Mousean.Model;

public class PathCorrector {
    private readonly Vector2 _relativePathStep;
    private readonly Vector2 _targetPosition;
    
    public PathCorrector(Vector2 currentPosition, Vector2 targetPosition, Vector2 relativePathStep)
    {
        float minX = Constants.ArenaMargin;
        float minY = Constants.ArenaMargin;
        float maxX = Constants.DefaultArenaWidth - Constants.ArenaMargin;
        float maxY = Constants.DefaultArenaHeight - Constants.ArenaMargin;
        Vector2 pathStep = currentPosition + relativePathStep;
        if (pathStep.X > maxX) pathStep.X = maxX;
        if (pathStep.X < minX) pathStep.X = minX;
        if (pathStep.Y > maxY) pathStep.Y = maxY;
        if (pathStep.Y < minY) pathStep.Y = minY;
        if (targetPosition.X > maxX) targetPosition.X = maxX;
        if (targetPosition.X < minX) targetPosition.X = minX;
        if (targetPosition.Y > maxY) targetPosition.Y = maxY;
        if (targetPosition.Y < minY) targetPosition.Y = minY;
        _relativePathStep = pathStep - currentPosition;
        _targetPosition = targetPosition;
    }
    
    public Vector2 GetTargetPosition()
    {
        return _targetPosition;
    }
    
    public Vector2 GetRelativePathStep()
    {
        return _relativePathStep;
    }
}