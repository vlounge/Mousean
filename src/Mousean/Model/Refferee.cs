using Microsoft.Xna.Framework;
using Mousean.Controller;

namespace Mousean.Model;

public class Refferee {
    public double RoundTime;
    public int CurrentRound;
    public int DarKonMawCount;
    public int DontHitCatCount;
    public bool Started;
    public bool RoundEnded;
    public bool GameEnded;
    
    public Refferee()
    {
        RoundTime = Constants.DefaultRoundTime;
        CurrentRound = 1;
        DarKonMawCount = 0;
        DontHitCatCount = 0;
        RoundEnded = false;
        GameEnded = false;
        Started = false;
    }
    
    public void StartTimer()
    {
        if(CurrentRound<Constants.MaxRound)
        {
            Started=true;
            RoundEnded = false;
        }
    }
    
    public void AddTime()
    {
        RoundTime += Constants.RoundTimeBonus;
    }
    
    public void Update()
    {
        if(Started)
        {
            RoundTime -= EntryPoint.Game.Elapsed;
            if (RoundTime<0)
            {
                RoundTime=  0;
                Started = false;
                RoundEnded = true;
                DarKonMawCount++;
                CurrentRound++;
                RoundTime = Constants.DefaultRoundTime;
            }
            
            Vector2 testColision = EntryPoint.Game.StateMachine.DontHitCat.CurrentPosition - EntryPoint.Game.StateMachine.DarKonMaw.CurrentPosition;
            if(testColision.Length() < Constants.CollisionDetectionLength)
            {
                Started = false;
                RoundEnded = true;
                DontHitCatCount++;
                CurrentRound++;
                RoundTime = Constants.DefaultRoundTime;
            }
        }
        if(DontHitCatCount == Constants.MaxCount || DarKonMawCount == Constants.MaxCount)
        {
            Started = false;
            GameEnded = true;
            RoundTime = Constants.DefaultRoundTime;
            CurrentRound = 1;
            DarKonMawCount = 0;
            DontHitCatCount = 0;
            RoundEnded = false;
        }
    }

    public override string ToString()
    {
        return $"Round {CurrentRound} DHC {DontHitCatCount}:{DarKonMawCount} DKM Timer: {RoundTime:N0} sec.";
        //return $"Round 1 DHC {DontHitCatCount}:{DarKonMawCount} DKM Timer: {Constants.DefaultRoundTime:N0} sec.";
    }
}