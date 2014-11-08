using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IceFlake.Client
{
    public class Maths
    {

        public static int Random(int For, int To)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(For, To);
        }

        public static float GetAngleZ(Location player, Location point)
        {
            float num = (float)(1.5707963267948966 - Math.Atan(((double)(player.Z - point.Z)) / Math.Sqrt(Math.Pow((double)(player.X - point.X), 2.0) + Math.Pow((double)(player.Y - point.Y), 2.0))));
            return (((float)1.5707963267948966) - num);
        }

        /// <summary>
        /// aforster's math
        /// </summary>
        /// 

        public static Double Distance3D_aforster(float X1, float Y1, float Z1, float X2, float Y2, float Z2)
        {
            float dx = X1 - X2;
            float dy = Y1 - Y2;
            float dz = Z1 - Z2;

            return ((double)((dx * dx) + (dy * dy) + (dz * dz))); //Saves you a sqrt
        }

        public static Double Distance2D_aforster(float X1, float Y1, float X2, float Y2)
        {
            float dx = X1 - X2;
            float dy = Y1 - Y2;

            return ((double)((dx * dx) + (dy * dy))); //Saves you a sqrt
        }

        public static Double Distance2D_aforster(Location Pos)
        {
            return Distance2D_aforster(Pos.X, Pos.Y, Manager.LocalPlayer.Location.X, Manager.LocalPlayer.Location.Y);
        }

        public static Double Distance3D_aforster(Location Pos)
        {
            return Distance3D_aforster(Pos.X, Pos.Y, Pos.Z, Manager.LocalPlayer.Location.X, Manager.LocalPlayer.Location.Y, Manager.LocalPlayer.Location.Z);
        }

        public static Double Distance2D_aforster(Location From, Location To)
        {
            return Distance2D_aforster(From.X, From.Y, To.X, To.Y);
        }

        public static Double Distance3D_aforster(Location From, Location To)
        {
            return Distance3D_aforster(From.X, From.Y, From.Z, To.X, To.Y, To.Z);
        }

        /// <summary>
        /// math
        /// </summary>
        /// 

        //public static Double Distance3D_FromMe(float X2, float Y2, float Z2)
        //{
        //    float X1 = Manager.LocalPlayer.Location.X;
        //    float Y1 = Manager.LocalPlayer.Location.Y;
        //    float Z1 = Manager.LocalPlayer.Location.Z;

        //    if (X1 == 0 && Y2 == 0 && Z1 == 0)
        //        return 0;

        //    float dx = X1 - X2;
        //    float dy = Y1 - Y2;
        //    float dz = Z1 - Z2;

        //    return System.Math.Sqrt((double)((dx * dx) + (dy * dy) + (dz * dz)));
        //}

        public static Double Distance3D(float X2, float Y2, float Z2)
        {
            float X1 = Manager.LocalPlayer.Location.X;
            float Y1 = Manager.LocalPlayer.Location.Y;
            float Z1 = Manager.LocalPlayer.Location.Z;
            if (X1 == 0 && Y2 == 0 && Z1 == 0)
                return 0;

            float dx = X1 - X2;
            float dy = Y1 - Y2;
            float dz = Z1 - Z2;

            return System.Math.Sqrt((double)((dx * dx) + (dy * dy) + (dz * dz)));
        }

        public static Double Distance3D(float X1, float Y1, float Z1, float X2, float Y2, float Z2)
        {
            float dx = X1 - X2;
            float dy = Y1 - Y2;
            float dz = Z1 - Z2;

            return System.Math.Sqrt((double)((dx * dx) + (dy * dy) + (dz * dz)));
        }

        public static Double Distance2D(float X2, float Y2)
        {
            float X1 = Manager.LocalPlayer.Location.X;
            float Y1 = Manager.LocalPlayer.Location.Y;
            if (X1 == 0 && Y2 == 0)
                return 0;

            float dx = X1 - X2;
            float dy = Y1 - Y2;

            return System.Math.Sqrt((double)((dx * dx) + (dy * dy)));
        }

        public static Double Distance2D(float X1, float Y1, float X2, float Y2)
        {
            float dx = X1 - X2;
            float dy = Y1 - Y2;

            return System.Math.Sqrt((double)((dx * dx) + (dy * dy)));
        }

        public static Double Distance2D(Location Pos)
        {
            return Distance2D(Pos.X, Pos.Y, Manager.LocalPlayer.Location.X, Manager.LocalPlayer.Location.Y);
        }

        public static Double Distance3D(Location Pos)
        {
            return Distance3D(Pos.X, Pos.Y, Pos.Z, Manager.LocalPlayer.Location.X, Manager.LocalPlayer.Location.Y, Manager.LocalPlayer.Location.Z);
        }

        public static Double Distance2D(Location From, Location To)
        {
            return Distance2D(From.X, From.Y, To.X, To.Y);
        }

        public static Double Distance3D(Location From, Location To)
        {
            return Distance3D(From.X, From.Y, From.Z, To.X, To.Y, To.Z);
        }

        private static Single negativeAngle(Single angle)
        {
            if (angle < 0f)
            {
                angle += (Single)(System.Math.PI * 2);
            }
            return angle;
        }

        public static Single CalculFace(Single ToX, Single ToY, Single FromX, Single FromY)
        {
            Single Angle = Convert.ToSingle(System.Math.Atan2(Convert.ToDouble(ToY) - Convert.ToDouble(FromY), Convert.ToDouble(ToX) - Convert.ToDouble(FromX)));
            Angle = negativeAngle(Angle);
            return Angle;
        }

        public static float CalculFace(Location From, Location To)
        {
            return CalculFace(To.X, To.Y, From.X, From.Y);
        }

        public static Single CalculFace(Single X, Single Y)
        {
            Single Angle = Convert.ToSingle(System.Math.Atan2(Convert.ToDouble(Y) - Convert.ToDouble(Manager.LocalPlayer.Location.Y), Convert.ToDouble(X) - Convert.ToDouble(Manager.LocalPlayer.Location.X)));
            Angle = negativeAngle(Angle);
            return Angle;
        }

        public static float CalculFace(Location To)
        {
            return CalculFace(To.X, To.Y, Manager.LocalPlayer.Location.X, Manager.LocalPlayer.Location.Y);
        }

        public static float RadianToDegree(float Rotation)
        {
            return (float)(Rotation * (180 / Math.PI));
        }

        // NEW

        public static bool IsFacingH(float angle, float ErrorMarge)
        {
            if (correctAngle(angle - Manager.LocalPlayer.Location.R) < 3.1415926535897931)
            {
                if (correctAngle(angle - Manager.LocalPlayer.Location.R) < ErrorMarge)
                {
                    return true;
                }
            }
            else if (correctAngle(Manager.LocalPlayer.Location.R - angle) < ErrorMarge)
            {
                return true;
            }
            return false;
        }

        public static float correctAngle(float angle)
        {
            if (angle < 0f)
            {
                angle += 6.283185f;
            }
            return angle;
        }

        public static bool IsFacingV(float angle, float ErrorMarge)
        {
            if (correctAngle(angle - Manager.LocalPlayer.Location.P) > 3.1415926535897931)
            {
                if (correctAngle(Manager.LocalPlayer.Location.P - angle) < ErrorMarge)
                {
                    return true;
                }
            }
            else if (correctAngle(angle - Manager.LocalPlayer.Location.P) < ErrorMarge)
            {
                return true;
            }
            return false;
        }

        public static float CalculPitch(Location To)
        {
            return CalculPitch(To.X, To.Y, To.Z, Manager.LocalPlayer.Location.X, Manager.LocalPlayer.Location.Y, Manager.LocalPlayer.Location.Z);
        }

        public static float CalculPitch(float ToX, float ToY, float ToZ, float FromX, float FromY, float FromZ)
        {
            float angle = (float)(1.5707963267948966 - Math.Atan(((double)(ToZ - FromZ)) / Math.Sqrt(Math.Pow((double)(ToX - FromX), 2.0) + Math.Pow((double)(ToY - FromY), 2.0))));
            angle = ((float)1.5707963267948966) - angle;
            return correctAngle(angle);
        }
















        //public static void FaceVerticalWithTimer(float radius, Keys Key)
        public static void FaceVerticalWithTimer(float radius)
        {

            if (radius < 0.2f)
                return;

            Int32 TurnTime = 0;

            if (Manager.LocalPlayer.IsMoving)
            {
                TurnTime = 2700;
            }
            else
            {
                TurnTime = 1980;
            }

            //Helpers.Keyboard.KeyDown(Key);
            System.Threading.Thread.Sleep((int)((radius * TurnTime * Math.PI) / 10));
            //Helpers.Keyboard.KeyUp(Key);
        }

        public static Single CalculPitchNew(Single ToX, Single ToY, Single ToZ, Single FromX, Single FromY, Single FromZ)
        {
            Single newPitch = (float)(Math.PI / 2 - Math.Atan((ToZ - FromZ) / Math.Sqrt(Math.Pow((ToX - FromX), 2) + Math.Pow((ToY - FromY), 2))));
            newPitch = (Single)(Math.PI / 2 - newPitch);
            newPitch = negativeAngleNew(newPitch);
            return newPitch;
        }

        public static Single negativeAngleNew(Single angle)
        {
            if (angle < 0f)
            {
                angle += (Single)(System.Math.PI * 2);
            }
            return angle;
        }











    }
}
