using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBehaviour
{
    //a class for some handy extra math functions, specifically for clamping values
    public static class HandyMath
    {
        
        public static float Clamp(float value, float min, float max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }
        public static float Clamp01(float value)
        {
            return Clamp(value, 0, 1);
        }
    }
}
