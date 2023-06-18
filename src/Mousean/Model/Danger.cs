using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mousean.Model
{
    public class Danger
    {
        public Vector2A Position;
        public float Lenght;
        public Danger(Vector2 position, float lenght)
        {
            Position = new Vector2A(position);
            Lenght = lenght;
        }
    }
}
