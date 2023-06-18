using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mousean.Model
{
    public class Converter
    {
        public int AngleToSample(double angle)
        {
            int result = 0;
            if (angle < 0)
            {
                result = (int)(angle / Math.PI * (-200));
            }
            else
            {
                result = (int)((2 - (angle / Math.PI)) * 200);
            }
            if (result == 400)
            {
                return 0;
            }
            else
            {
                return result;
            }
        }
        public double SampleToAngle(int sample)
        {
            if (sample < 200)
            {
                return (double)sample / 200 * (-Math.PI);
            }
            else
            {
                return Math.PI * (2 - (double)sample / 200);
            }
        }
    }
}
