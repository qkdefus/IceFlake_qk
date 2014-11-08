using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace IceFlake.Runtime
{
    public static class Extension
    {
        public static string ToLongString(this Exception self)
        {
            string result = "";

            Exception exception = self;
            do
            {
                result += "Exception " + exception.GetType() + ": " + exception.Message + Environment.NewLine +
                          Environment.NewLine + exception.StackTrace + Environment.NewLine + Environment.NewLine;
                exception = exception.InnerException;
            } while (exception != null);

            return result;
        }


        public static Matrix CreateMatrix(float M11, float M12, float M13, float M14,
float M21, float M22, float M23, float M24,
float M31, float M32, float M33, float M34,
float M41, float M42, float M43, float M44)
        {
            var m = new Matrix();
            m.M11 = M11; m.M12 = M12; m.M13 = M13; m.M14 = M14;
            m.M21 = M21; m.M22 = M22; m.M23 = M23; m.M24 = M24;
            m.M31 = M31; m.M32 = M32; m.M33 = M33; m.M34 = M34;
            m.M41 = M41; m.M42 = M42; m.M43 = M43; m.M44 = M44;
            return m;
        }
    }
}
