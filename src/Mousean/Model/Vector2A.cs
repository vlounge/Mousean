using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mousean.Model
{
    public class Vector2A
    {
        public Vector2 Vector;
        public double Angle;

        public Vector2A(Vector2 vector)
        {
            Vector = new Vector2();
            SetVector(vector);
        }

        public void SetVector(Vector2 vector)
        {
            Vector = vector;
            Angle = GetAngle(vector);
        }

        public void SetVector(double angle, float lenght)
        {
            var vector = new Vector2(lenght * (float)Math.Cos(angle), lenght * (float)Math.Sin(angle));
            SetVector(vector);
        }

        public double GetAngle(Vector2 vector)
        {
            return Math.Atan2(vector.Y, vector.X);
        }
    }
}
