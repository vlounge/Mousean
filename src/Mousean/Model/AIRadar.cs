using Microsoft.Xna.Framework;
using Mousean.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mousean.Model
{
    public class AIRadar
    {
        public float[] Vision;
        public Vector2 CurrentPosition;
        public Dictionary<int, Danger> AngledDangers;
        public Vector2 TargetPosition;
        private Converter Converter;
        public Vector2 GetTargetPosition()
        {
            return TargetPosition;
        }
        public AIRadar(Vector2 targetPosition)
        {
            var timer = EntryPoint.Game.StateMachine.DarKonMaw.TargetTimer;
            var angles = new List<int>();
            var result = new Vector2A(new Vector2());
            AngledDangers = new Dictionary<int, Danger>();
            Vision = new float[Constants.SAMPLLING_RATE];
            CurrentPosition = EntryPoint.Game.StateMachine.DarKonMaw.CurrentPosition;
            Converter = new Converter();
            for (int i = 0; i < Vision.Length; i++)
            {
                Vision[i] = Constants.RAT_VISION_RANGE;
            }
            var danger = new Danger(new Vector2(CurrentPosition.X, 0) - CurrentPosition, Constants.AREA_ZONE);
            //Кот
            danger = new Danger(EntryPoint.Game.StateMachine.DontHitCat.CurrentPosition - CurrentPosition, Constants.RAT_VISION_RANGE);
            if (danger.Position.Vector.Length() < Constants.RAT_VISION_RANGE)
            {
                var conv = Converter.AngleToSample(danger.Position.Angle);
                AngledDangers.Add(conv, danger);
            }

            //Список опасностей
            var dangersList = new List<Danger>();
            bool catIsVisible = false;
            foreach (var dan in AngledDangers)
            {

                if (dan.Value.Position.Vector.Length() <= dan.Value.Lenght)
                {
                    dangersList.Add(dan.Value);
                    if (dan.Value.Lenght == Constants.RAT_VISION_RANGE)
                    {
                        catIsVisible = true;
                    }
                }
            }
            //Вычищение углов
            Vector2 upLeftCorner = CurrentPosition;
            Vector2 upRightCorner = new Vector2(Constants.DefaultArenaWidth, 0);
            upRightCorner = CurrentPosition - upRightCorner;
            Vector2 downRightCorner = new Vector2(Constants.DefaultArenaWidth, Constants.DefaultArenaHeight);
            downRightCorner = CurrentPosition - downRightCorner;
            Vector2 downLeftCorner = new Vector2(0, Constants.DefaultArenaHeight);
            downLeftCorner = CurrentPosition - downLeftCorner;
            if (catIsVisible &&
                (upLeftCorner.Length() < 300 ||
                upRightCorner.Length() < 300 ||
                downRightCorner.Length() < 300 ||
                downLeftCorner.Length() < 300))
            {
                AngledCleaner2(dangersList);
            }
            else
            {
                AngledCleaner(dangersList);
            }

            
            for (int i = 0; i < Vision.Length; i++)
            {
                if (Vision[i] != 0)
                {
                    angles.Add(i);
                }
            }
            var random = EntryPoint.Game.StateMachine.Random.Next(angles.Count);
            var randomAngle = Converter.SampleToAngle(angles[random]);
            result.SetVector(randomAngle, (float)EntryPoint.Game.StateMachine.Random.Next(75, 450));
            result.Vector = result.Vector + CurrentPosition;
            EntryPoint.Game.StateMachine.DarKonMaw.TargetTimer -= EntryPoint.Game.Elapsed;
            if (EntryPoint.Game.StateMachine.DarKonMaw.TargetTimer < 0) 
                EntryPoint.Game.StateMachine.DarKonMaw.TargetTimer = 0;

            EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoostTimer -= EntryPoint.Game.Elapsed;
            if (EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoostTimer < 0)
                EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoostTimer = 0;

            if (dangersList.Count > 0) 
            {
                if (catIsVisible)
                {
                    if(EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoost==0)
                    {
                        EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoost = Constants.DARKONMAW_FIRST_SPEED_BOOST;
                        EntryPoint.Game.StateMachine.DarKonMaw.SetVelocity(EntryPoint.Game.StateMachine.DarKonMaw.Velocity + EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoost);
                        EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoost = Constants.DARKONMAW_SPEED_BOOST;
                    }
                    else
                    {
                        if (EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoostTimer == 0)
                        {
                            if(EntryPoint.Game.StateMachine.DarKonMaw.Velocity + EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoost> Constants.DARKONMAW_MAX_SPEED)
                            {
                                EntryPoint.Game.StateMachine.DarKonMaw.SetVelocity(Constants.DARKONMAW_MAX_SPEED);
                            }
                            else
                            {
                                EntryPoint.Game.StateMachine.DarKonMaw.SetVelocity( EntryPoint.Game.StateMachine.DarKonMaw.Velocity + EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoost);
                                EntryPoint.Game.StateMachine.DarKonMaw.SpeedBoostTimer = Constants.DARKONMAW_SPEED_BOOST_COOLDOWN;
                            }
                        }
                    }
                }
                if (EntryPoint.Game.StateMachine.DarKonMaw.TargetTimer == 0)
                {
                    TargetPosition = result.Vector;
                    EntryPoint.Game.StateMachine.DarKonMaw.TargetTimer = (double)EntryPoint.Game.StateMachine.Random.Next(10, 30) * 0.1f;
                }
                else
                {
                    if (targetPosition == CurrentPosition)
                    {
                        TargetPosition = result.Vector;
                        EntryPoint.Game.StateMachine.DarKonMaw.TargetTimer = 0;
                    }
                    else
                    {
                        TargetPosition = targetPosition;
                    }
                }
                
            }
            else
            {
                if (targetPosition == CurrentPosition)
                {
                    TargetPosition = result.Vector;
                    EntryPoint.Game.StateMachine.DarKonMaw.TargetTimer = 0;
                }
                else
                {
                    TargetPosition = targetPosition;
                }
            }
            
        }
        private void AngledCleaner(List<Danger> dangersList)
        {
            foreach (var dan in dangersList)
            {
                var realAngle = Converter.AngleToSample(dan.Position.Angle);
                int deltaRealAngle = 2* Constants.DELTA_ANGLE ;
                for (var i = 0; i < Vision.Length; i++)
                {
                    if (realAngle < deltaRealAngle)
                    {
                        if ((i >= realAngle - deltaRealAngle && i <= realAngle + deltaRealAngle) || i >= 400 - deltaRealAngle + realAngle)
                        {
                            Vision[i] = 0;
                        }

                    }
                    if (realAngle > 400 - deltaRealAngle)
                    {
                        if ((i >= realAngle - deltaRealAngle && i <= realAngle + deltaRealAngle) || i <= realAngle - (400 - deltaRealAngle))
                        {
                            Vision[i] = 0;
                        }
                    }
                    else if ((i <= realAngle + deltaRealAngle &&
                              i >= realAngle - deltaRealAngle) )
                    {
                        Vision[i] = 0;
                    }
                }
            }
        }
        private void AngledCleaner2(List<Danger> dangersList)
        {
            foreach (var dan in dangersList)
            {
                var realAngle = Converter.AngleToSample(dan.Position.Angle);
                int imagineAngle = 0;
                int deltaRealAngle = 2 * Constants.DELTA_ANGLE;
                int deltaImagineAngle = Constants.DELTA_ANGLE ;
                
                if (realAngle >= 200)
                {
                    imagineAngle = realAngle-200;
                }
                else
                {
                    imagineAngle = 200 + realAngle;
                }
                for (var i = 0; i < Vision.Length; i++)
                {
                    if (realAngle < deltaRealAngle)
                    {
                        if ((i >= realAngle - deltaRealAngle && i <= realAngle + deltaRealAngle) || i >= 400 - deltaRealAngle + realAngle)
                        {
                            Vision[i] = 0;
                        }

                    }
                    if (imagineAngle < deltaImagineAngle)
                    {
                        if ((i >= imagineAngle - deltaImagineAngle && i <= imagineAngle + deltaImagineAngle) || i >= 400 - deltaImagineAngle + imagineAngle)
                        {
                            Vision[i] = 0;
                        }

                    }
                    if (realAngle > 400 - deltaRealAngle)
                    {
                        if ((i >= realAngle - deltaRealAngle && i <= realAngle + deltaRealAngle) || i <= realAngle - (400 - deltaRealAngle))
                        {
                            Vision[i] = 0;
                        }
                    }
                    if (imagineAngle > 400 - deltaImagineAngle)
                    {
                        if ((i >= imagineAngle - deltaImagineAngle && i <= imagineAngle + deltaImagineAngle) || i <= imagineAngle - (400 - deltaImagineAngle))
                        {
                            Vision[i] = 0;
                        }
                    }
                    else if ((i <= realAngle + deltaRealAngle &&
                              i >= realAngle - deltaRealAngle) ||
                              (i <= imagineAngle + deltaImagineAngle &&
                              i >= imagineAngle - deltaImagineAngle ))
                    {
                        Vision[i] = 0;
                    }
                }
            }
        }
    }
}
