using System.Collections.Generic;
using Mousean.Controller;

namespace Mousean.View;

public class TimeLine {
    private double _timeRange;
    private double _timeCounter;
    private readonly bool _isLooped;
    private readonly List<Action> _timeLine;
    
    public TimeLine(double timeRange, bool isLooped)
    {
        _timeCounter=0;
        _timeRange = timeRange;
        _isLooped = isLooped;
        _timeLine = new List<Action>();
    }
    
    public void SetTimeRange(double timeRange)
    {
        if (_timeRange<timeRange)
            _timeRange = timeRange;
    }
    
    public void AddAction(Action action)
    {
        _timeLine.Add(action);
    }
    
    public void SetActions(float xScale, float yScale)
    {
        if (_timeLine!=null)
        {
            if (_timeLine.Count != 0)
            {
                foreach(var action in _timeLine)
                {
                    action.StartPosition.X *= xScale;
                    action.StartPosition.Y *= yScale;
                    action.EndPosition.X *= xScale;
                    action.EndPosition.Y *= yScale;
                }
            }
        }
    }
    
    public List<Action> GetActions()
    {
        _timeCounter += EntryPoint.Game.Elapsed;
        if (_timeCounter > _timeRange)
        {
            if (_isLooped)  _timeCounter=0;
                else _timeCounter = _timeRange;
        }
        if (_timeLine==null) return null;
        if (_timeLine.Count == 0) return null;
        var actionList = new List<Action>();
        foreach(var action in _timeLine)
        {
            if (action.StartTime<=_timeCounter && _timeCounter<action.EndTime)
            {
                action.TimeCounter = _timeCounter - action.StartTime;
                actionList.Add(action);
            }
        }
        return actionList;
    }
    
    public void Reset()
    {
        _timeCounter=0;
    }
}