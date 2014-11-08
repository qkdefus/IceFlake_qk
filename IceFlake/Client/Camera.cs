using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;
using IceFlake.DirectX;
using IceFlake.Runtime;
using SlimDX;

namespace IceFlake.Client
{
    public unsafe struct CameraInfo
    {
        fixed int unk0[2];
        public Vector3 Position;
        public Matrix3 Facing;
        public float NearZ;
        public float FarZ;
        public float FieldOfView;
        public float Aspect;
    }

    public unsafe class Camera
    {
        public Camera()
        {
            this.Pointer = Manager.Memory.Read<IntPtr>(new IntPtr(Manager.Memory.Read<uint>((IntPtr)Pointers.Drawing.WorldFrame) + Pointers.Drawing.ActiveCamera));
        }

        public IntPtr Pointer
        {
            get;
            private set;
        }

        public bool IsValid { get { return Pointer != IntPtr.Zero; } }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* ForwardDelegate(IntPtr ptr, Vector3* vecOut);
        private ForwardDelegate _Forward;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* RightDelegate(IntPtr ptr, Vector3* vecOut);
        private RightDelegate _Right;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate Vector3* UpDelegate(IntPtr ptr, Vector3* vecOut);
        private UpDelegate _Up;

        public Vector3 Forward
        {
            get
            {
                if (_Forward == null)
                    _Forward = Manager.Memory.RegisterDelegate<ForwardDelegate>(Manager.Memory.GetObjectVtableFunction(Pointer, 1));

                var res = new Vector3();
                _Forward(Pointer, &res);
                return res;
            }
        }

        public Vector3 Right
        {
            get
            {
                if (_Right == null)
                    _Right = Manager.Memory.RegisterDelegate<RightDelegate>(Manager.Memory.GetObjectVtableFunction(Pointer, 2));

                var res = new Vector3();
                _Right(Pointer, &res);
                return res;
            }
        }

        public Vector3 Up
        {
            get
            {
                if (_Up == null)
                    _Up = Manager.Memory.RegisterDelegate<UpDelegate>(Manager.Memory.GetObjectVtableFunction(Pointer, 3));

                var res = new Vector3();
                _Up(Pointer, &res);
                return res;
            }
        }

        public Matrix Projection
        {
            get
            {
                var cam = GetCamera();
                return Matrix.PerspectiveFovRH(cam.FieldOfView * 0.6f, cam.Aspect, cam.NearZ, cam.FarZ);
            }
        }

        /*/
        private Matrix view = Matrix.Identity;
        public Matrix ProjectionTEST0
        {
            get
            {
                var cam = GetCamera();
                view = Matrix.PerspectiveFovRH(cam.FieldOfView * 0.6f, cam.Aspect, cam.NearZ, cam.FarZ);
                return view = Matrix.Identity;
                //return Matrix.PerspectiveFovLH(cam.FieldOfView * 0.6f, cam.Aspect, cam.NearZ, cam.FarZ);
            }
        }

        public Matrix ProjectionTEST1
        {
            get
            {
                var cam = GetCamera();
                float fovy = cam.FieldOfView * 0.6f;
                float aspect = cam.Aspect;
                float zn = cam.NearZ;
                float zf = cam.FarZ;

                float xs = (float)Math.Tan(fovy * 0.5f) * zn;
                //float xs = (float)Math.Atan(fovy * 0.5f) * zn;

                float xn = zn / (xs * aspect);

                var mat = Extension.CreateMatrix(
                    xn, 0.0f, 0.0f, 0.0f,
                    0.0f, zn / xs, 0.0f, 0.0f,
                    0.0f, 0.0f, (zn + zf) / (zf - zn), 1.0f,
                    0.0f, 0.0f, ((zf * -2.0f) * zn) / (zf - zn), 0.0f
                    );

                //return mat = Matrix.Identity;
                return mat;
            }
        }

        public Matrix ProjectionTEST2
        {
            get
            {
                var cam = GetCamera();
                float fovy = cam.FieldOfView;
                float aspect = cam.Aspect;
                float zn = cam.NearZ;
                float zf = cam.FarZ;

                float ys = (1.0f / (float)Math.Tan(fovy / 2.0f));
                float xs = (ys / aspect);

                var mat = Extension.CreateMatrix(
                      xs, 0.0f, 0.0f, 0.0f,
                    0.0f, ys, 0.0f, 0.0f,
                    0.0f, 0.0f, (zf / (zf - zn)), 1.0f,
                    0.0f, 0.0f, (-zn * zf / (zf - zn)), 0.0f
                    );

                //return mat = Matrix.Identity;
                return mat;
            }
        }

        static Matrix ProjectionTEST3(float fovy, float aspect, float zn, float zf)
        {
            float ys = (1.0f / (float)Math.Atan(fovy / 2.0f));
            float xs = (ys / aspect);

            var mat = Extension.CreateMatrix(
                  xs,  0.0f,                 0.0f,  0.0f,
                0.0f,    ys,                 0.0f,  0.0f,
                0.0f,  0.0f,       zf / (zf - zn),  1.0f,
                0.0f,  0.0f, -zn * zf / (zf - zn),  0.0f 
                );

            return mat;
        }
        /*/



        public Matrix ViewTEST
        {
            get
            {
                var cam = GetCamera();
                return Matrix.LookAtRH(cam.Position, cam.Position + Forward, Up);
            }
        }

        public Matrix View
        {
            get
            {
                var cam = GetCamera();
                var eye = cam.Position;
                var at = eye + Forward;
                return Matrix.LookAtRH(eye, at, new Vector3(0, 0, 1));
            }
        }

        public CameraInfo GetCamera()
        {
            return Manager.Memory.Read<CameraInfo>(Pointer);
        }
    }
}
