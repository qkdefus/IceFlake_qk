using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;

using IceFlake.Runtime;
using IceFlake.Client.Patchables;
using GreyMagic.Internals;

using SlimDX;
using SlimDX.Direct3D9;

using D3D = IceFlake.DirectX.Direct3D;

namespace IceFlake.DirectX
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColored
    {
        public static readonly VertexFormat FVF = VertexFormat.Position | VertexFormat.Diffuse;
        public static readonly int Stride = Vector3.SizeInBytes + sizeof(int);

        public Vector3 Position;
        public int Color;

        public PositionColored(Vector3 pos, int col)
        {
            Position = pos;
            Color = col;
        }
    }

    public static class Rendering
    {
        #region Nested type: Direct3D39_RenderBackground

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void Direct3D39RenderBackground();
        private static Direct3D39RenderBackground _renderBackgroundDelegate;
        private static Detour _renderBackgroundHook;
        #endregion

        private static readonly List<IResource> _resources = new List<IResource>();
        private static IntPtr _usedDevicePointer = IntPtr.Zero;

        public static Device Device { get; private set; }

        public static bool IsInitialized { get { return Device != null; } }

        public static void Initialize(IntPtr devicePointer)
        {
            if (_usedDevicePointer != devicePointer)
            {
                Debug.WriteLine("Rendering: Device initialized on " + devicePointer);
                Device = Device.FromPointer(devicePointer);
                _usedDevicePointer = devicePointer;
            }

            _renderBackgroundDelegate = Manager.Memory.RegisterDelegate<Direct3D39RenderBackground>((IntPtr)Pointers.Drawing.RenderBackground);
            _renderBackgroundHook = Manager.Memory.Detours.CreateAndApply(_renderBackgroundDelegate, new Direct3D39RenderBackground(RenderBackground), "RenderBackground");
        }

        //public static void RegisterResource(IResource source)
        //{
        //    _resources.Add(source);
        //}

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        public static void SetTarget(Vector3 target, float yaw = 0, float pitch = 0, float roll = 0)
        {
            var worldMatrix = Matrix.Translation(target) * Matrix.RotationYawPitchRoll(yaw, pitch, roll);
            Device.SetTransform(TransformState.World, worldMatrix);
        }

        public static void OnLostDevice()
        {
            foreach (var resource in _resources)
                resource.OnLostDevice();
        }

        public static void OnResetDevice()
        {
            foreach (var resource in _resources)
                resource.OnResetDevice();
        }

        //public static bool CanDraw 
        //{ 
        //    get 
        //    {
        //        return true; // Manager.IsInGame && IsInitialized && Camera.IsValid; 
        //    } 
        //}

        public static void Pulse()
        {
        
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        private static List<Client.Location> LocationList = new List<Client.Location>();
        //private static System.Drawing.Color _colorAmbient = System.Drawing.Color.Red;
        private static System.Drawing.Color _colorLight = System.Drawing.Color.Red;

        private static void RenderBackground()
        {
            //if (!CanDraw)
            //    return;

                return;

            var viewport = Device.Viewport;
            viewport.MinZ = 0.0f;
            viewport.MaxZ = 0.94f; // 0.94f;
            Device.Viewport = viewport;

            Device.VertexShader = null;
            Device.PixelShader = null;
            Device.SetRenderState(RenderState.ZEnable, true);
            Device.SetRenderState(RenderState.ZWriteEnable, true);
            Device.SetRenderState(RenderState.ZFunc, Compare.LessEqual);
            Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            Device.SetRenderState(RenderState.Lighting, false);
            Device.SetTexture(0, null);
            Device.SetRenderState(RenderState.CullMode, Cull.None);

            Device.SetTransform(TransformState.View, Manager.Camera.View);
            Device.SetTransform(TransformState.Projection, Manager.Camera.Projection);

            //// Setup Lights
            //Light light = new Light();
            //light.Type = LightType.Directional;
            //light.Diffuse = _colorLight;
            //light.Attenuation0 = 0.2f;
            //light.Range = 1000.0f;
            //light.Direction = new SlimDX.Vector3(0.0f, 0.0f, -1.0f);
            //light.Direction.Normalize();

            //// Set the light and enable lighting.
            //Device.SetLight(0, light);
            //Device.EnableLight(0, true);
            //Device.SetRenderState(RenderState.Lighting, true);

            //foreach (var res in _resources)
            //    res.Draw();
            //_resources.Clear();

            var _me = Manager.LocalPlayer;

            if (LocationList.Count < 10000)
            {
                if (!LocationList.Contains(Manager.LocalPlayer.Location))
                    LocationList.Add(Manager.LocalPlayer.Location);
            }

            foreach (Client.Location _loc in LocationList)
            {
                //DrawMesh(new SlimDX.Vector3(_loc.X, _loc.Y, _loc.Z));

                DrawCubeTest(new SlimDX.Vector3(_loc.X, _loc.Y, _loc.Z), 0.5f, 0.5f, 0.5f, true, false);

                //DrawLineVertex();
            }

            // needed ??
            //_renderBackgroundHook.CallOriginal();
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        private static SlimDX.Direct3D9.StateBlock _SB = new StateBlock(D3D.Device, StateBlockType.All);
        //public static VertexBuffer Vertices;    // Vertex buffer object used to hold vertices.
        private static SlimDX.Direct3D9.Mesh _mesh;

        // DrawCubeTest
        private static void DrawCubeTest(SlimDX.Vector3 loc, float width, float height, float depth, bool isFilled = true, bool wireframe = false)
        {
            if (_mesh == null)
                _mesh = SlimDX.Direct3D9.Mesh.CreateBox(D3D.Device, width, height, depth);

            _SB.Capture();

            var worldMatrix = SlimDX.Matrix.Translation(loc);
            D3D.Device.SetTransform(TransformState.World, worldMatrix);

            _mesh.DrawSubset(0);

            D3D.Device.SetRenderState(RenderState.FillMode, SlimDX.Direct3D9.FillMode.Solid);

            _SB.Apply();
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>
    }
}