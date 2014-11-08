using System;
using System.Runtime.InteropServices;
using System.Threading;
using IceFlake.Runtime;
using IceFlake.Client.Patchables;
using GreyMagic.Internals;
using SlimDX.Direct3D9;
using SlimDX;

namespace IceFlake.DirectX
{
    public delegate void EndSceneCallback();

    public static class Direct3D
    {
        private const int VMT_ENDSCENE = 42;
        private const int VMT_RESET = 16;

        public static CallbackManager<EndSceneCallback> CallbackManager = new CallbackManager<EndSceneCallback>();

        private static Direct3D9EndScene _endSceneDelegate;
        private static Detour _endSceneHook;

        private static Direct3D9Reset _resetDelegate;
        private static Detour _resetHook;

        private static Direct3D39RenderBackground _renderBackgroundDelegate;
        private static Detour _renderBackgroundHook;

        public static Device Device { get; private set; }

        public static int FrameCount { get; private set; }

        public static event EventHandler OnFirstFrame;
        public static event EventHandler OnLastFrame;

        private static int EndSceneHook(IntPtr device)
        {
            try
            {
                if (FrameCount == -1)
                {
                    if (OnLastFrame != null)
                        OnLastFrame(null, new EventArgs());
                    Device = null;
                }
                else
                {
                    if (Device == null)
                        Device = Device.FromPointer(device);

                    if (FrameCount == 0)
                        if (OnFirstFrame != null)
                            OnFirstFrame(null, new EventArgs());

                    if (!Rendering.IsInitialized)
                    {
                        Rendering.Initialize(device);
                    }

                    Rendering.Pulse();

                    PrepareRenderState();
                    CallbackManager.Invoke();

                    // TEMP SOLUTION
                    // TODO: FIX THIS
                    IceFlake.Client.Scripts.ScriptManager.Pulse();
                }
            }
            catch (Exception e)
            {
                Log.WriteLine("Error: " + e.ToLongString());
            }

            if (FrameCount != -1)
                FrameCount += 1;

            return (int)_endSceneHook.CallOriginal(device);
        }

        private static int ResetHook(IntPtr device, PresentParameters pp)
        {
            Device = null;
            return (int)_resetHook.CallOriginal(device, pp);
        }

        public static void Initialize()
        {
            var endScenePointer = IntPtr.Zero;
            var resetPointer = IntPtr.Zero;
            using (var d3d = new SlimDX.Direct3D9.Direct3D())
            {
                using (
                    var tmpDevice = new Device(d3d, 0, DeviceType.Hardware, IntPtr.Zero,
                                               CreateFlags.HardwareVertexProcessing,
                                               new PresentParameters { BackBufferWidth = 1, BackBufferHeight = 1 }))
                {
                    endScenePointer = Manager.Memory.GetObjectVtableFunction(tmpDevice.ComPointer, VMT_ENDSCENE);
                    resetPointer = Manager.Memory.GetObjectVtableFunction(tmpDevice.ComPointer, VMT_RESET);
                }
            }

            _endSceneDelegate = Manager.Memory.RegisterDelegate<Direct3D9EndScene>(endScenePointer);
            _endSceneHook = Manager.Memory.Detours.CreateAndApply(_endSceneDelegate, new Direct3D9EndScene(EndSceneHook), "D9EndScene");

            _resetDelegate = Manager.Memory.RegisterDelegate<Direct3D9Reset>(resetPointer);
            _resetHook = Manager.Memory.Detours.CreateAndApply(_resetDelegate, new Direct3D9Reset(ResetHook), "D9Reset");

            // NEEDED FOR DRAWING IN RENDERBACKGROUND
            //_renderBackgroundDelegate = Manager.Memory.RegisterDelegate<Direct3D39RenderBackground>((IntPtr)Pointers.Drawing.RenderBackground);
            //_renderBackgroundHook = Manager.Memory.Detours.CreateAndApply(_renderBackgroundDelegate, new Direct3D39RenderBackground(PrepareRenderState), "RenderBackground");    

            Log.WriteLine("EndScene detoured at 0x{0:X}", endScenePointer);
        }

        public static void Shutdown()
        {
            if (Device == null)
                return;

            if (FrameCount > 0)
            {
                FrameCount = -1;
                while (Device != null)
                    Thread.Sleep(0);
            }
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        private static System.Drawing.Color _colorAmbient = System.Drawing.Color.Red;
        private static System.Drawing.Color _colorLight = System.Drawing.Color.Red;

        private static Material _material;
        private static Light _light;
        private static Mesh _mesh;

        /*/
        private static void PrepareRenderState()
        {
            if (Device == null)
                return;

            if (Manager.Camera == null)
                return;

            // Clear the backbuffer to a black color.
            //Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);

            // Begin the scene.
            //Device.BeginScene();

            var viewport = Device.Viewport;
            viewport.MinZ = 0.0f;
            viewport.MaxZ = 0.94f;
            Device.Viewport = viewport;

            Device.SetTransform(TransformState.View, Manager.Camera.View);
            Device.SetTransform(TransformState.Projection, Manager.Camera.Projection);

            Device.VertexShader = null;
            Device.PixelShader = null;

            // Create a new PresentParameters object and fill in the necessary fields.
            //PresentParameters presentParams = new PresentParameters { BackBufferWidth = 1, BackBufferHeight = 1 };

            // Below are the required bare mininum, needed to initialize the D3D device.
            //presentParams.BackBufferHeight = Window.ClientRectangle.Height;         // BackBufferHeight, set to  the Window's height.
            //presentParams.BackBufferWidth = Window.ClientRectangle.Width;           // BackBufferWidth, set to  the Window's width.
            //presentParams.DeviceWindowHandle = Window.Handle;                       // DeviceWindowHandle, set to  the Window's handle.

            // the vertex buffer and fill with the triangle vertices.
            ////Vertices = new VertexBuffer(Device, 3 * Vertex.SizeBytes, Usage.WriteOnly, VertexFormat.None, Pool.Managed);
            ////SlimDX.DataStream stream = Vertices.Lock(0, 0, LockFlags.None);
            ////stream.WriteRange(BuildVertexData());
            ////Vertices.Unlock();

            // Enable the z-buffer.
            Device.SetRenderState(RenderState.ZEnable, true);
            Device.SetRenderState(RenderState.ZWriteEnable, true);
            //Device.SetRenderState(RenderState.ZFunc, 4); //
            Device.SetRenderState(RenderState.ZFunc, Compare.LessEqual); //

            Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);

            // Setup Lights
            Light light = new Light();
            light.Type = LightType.Directional;
            light.Diffuse = _colorLight;
            light.Attenuation0 = 0.2f;
            light.Range = 1000.0f;
            light.Direction = new SlimDX.Vector3(0.0f, 0.0f, -1.0f);
            light.Direction.Normalize();

            //// Setup Material
            //Material _material = new Material();
            //_material.Diffuse = System.Drawing.Color.Blue;
            //_material.Ambient = _ambient;
            //Device.Material = _material;

            // Set the light and enable lighting.
            Device.SetLight(0, light);
            Device.EnableLight(0, true);
            Device.SetRenderState(RenderState.Lighting, true);

            // Set an ambient light.
            Device.SetRenderState(RenderState.Ambient, _colorAmbient.ToArgb());

            Device.SetTexture(0, null);

            // Turn off culling, so we see the front and back of the triangle
            Device.SetRenderState(RenderState.CullMode, Cull.None);

            // End the scene.
            //Device.EndScene();

            // Present the backbuffer contents to the screen.
            //Device.Present();
        }
        /*/

        public static float ViewPortMaxZ = 0.94f;

        private static void PrepareRenderState()
        {
            if (Device == null)
                return;

            if (Manager.Camera == null)
                return;

            //var viewport = Device.Viewport;
            //viewport.MinZ = 0.0f;
            //viewport.MaxZ = 0.94f; // 0.94f;
            //Device.Viewport = viewport;

            //Device.VertexShader = null;
            //Device.PixelShader = null;
            //Device.SetRenderState(RenderState.ZEnable, true); 
            //Device.SetRenderState(RenderState.ZWriteEnable, true); 
            //Device.SetRenderState(RenderState.ZFunc, Compare.LessEqual); 

            //Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            //Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            //Device.SetRenderState(RenderState.Lighting, false);
            //Device.SetTexture(0, null);
            //Device.SetRenderState(RenderState.CullMode, Cull.None);

            //Device.SetTransform(TransformState.View, Manager.Camera.View);
            //Device.SetTransform(TransformState.Projection, Manager.Camera.Projection);

            // Setup Lights
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


            // setup viewport
            var viewport = Device.Viewport;
            viewport.MinZ = 0.0f;
            viewport.MaxZ = 0.94f;
            Device.Viewport = viewport;

            Device.PixelShader = null;
            Device.VertexShader = null;
            //Device.VertexDeclaration = null;
            Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            Device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
            Device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            Device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            Device.SetRenderState(RenderState.Lighting, false);
            Device.SetTexture(0, null);
            Device.SetRenderState(RenderState.CullMode, Cull.None);

            // transform
            Device.SetTransform(TransformState.View, Manager.Camera.View);
            Device.SetTransform(TransformState.Projection, Manager.Camera.Projection);

            // Enable the z-buffer.
            Device.SetRenderState(RenderState.ZEnable, true);
            Device.SetRenderState(RenderState.ZWriteEnable, true);
            Device.SetRenderState(RenderState.ZFunc, Compare.LessEqual);

            // Set the light and enable lighting.
            //Device.SetLight(0, light);
            //Device.EnableLight(0, true);
            //Device.SetRenderState(RenderState.Lighting, true);

            // Set an ambient light.
            Device.SetRenderState(RenderState.Ambient, _colorAmbient.ToArgb());

        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        public static VertexBuffer Vertices;    // Vertex buffer object used to hold vertices.

        // Vertex structure.
        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex
        {
            public SlimDX.Vector3 Position;
            public SlimDX.Vector3 Normal;

            public static int SizeBytes
            {
                get { return Marshal.SizeOf(typeof(Vertex)); }
            }

            public static VertexFormat Format
            {
                get { return VertexFormat.Position | VertexFormat.Normal; }
            }
        }

        /// <summary>
        /// Builds an array of vertices that can be written to a vertex buffer.
        /// </summary>
        /// <returns>An array of vertices.</returns>
        public static Vertex[] BuildVertexData()
        {
            Vertex[] vertexData = new Vertex[3];

            vertexData[0].Position = new SlimDX.Vector3(-1.0f, -1.0f, 0.0f);
            vertexData[0].Normal = new SlimDX.Vector3(0.0f, 0.0f, -1.0f);

            vertexData[1].Position = new SlimDX.Vector3(1.0f, -1.0f, 0.0f);
            vertexData[1].Normal = new SlimDX.Vector3(0.0f, 0.0f, -1.0f);

            vertexData[2].Position = new SlimDX.Vector3(0.0f, 1.0f, 0.0f);
            vertexData[2].Normal = new SlimDX.Vector3(0.0f, 0.0f, -1.0f);

            return vertexData;
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        #region Nested type: Direct3D9EndScene

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int Direct3D9EndScene(IntPtr device);

        #endregion

        #region Nested type: Direct3D39Reset

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int Direct3D9Reset(IntPtr device, PresentParameters presentationParameters);

        #endregion

        #region Nested type: Direct3D39_RenderBackground

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void Direct3D39RenderBackground();

        #endregion

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>
    }
}