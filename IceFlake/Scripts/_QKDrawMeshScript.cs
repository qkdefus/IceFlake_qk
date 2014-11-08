using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Scripts;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using D3D = IceFlake.DirectX.Direct3D;

using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Direct2D;
using SlimDX.Windows;

using IceFlake.Client;

using Squid;

//using SlimDX.Direct2D;
//using SlimDX.Windows;
//using SlimDX.Direct3D10;
//using SlimDX.Direct3D11;
//using SlimDX.D3DCompiler;
//using SlimDX.DXGI;


//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Content;
//using Xna = Microsoft.Xna.Framework;

//using SharpDX;
//using SharpDX.D3DCompiler;
//using SharpDX.Direct3D;
//using SharpDX.Direct3D11;
//using SharpDX.DXGI;
//using SharpDX.Windows;
//using Buffer = SharpDX.Direct3D11.Buffer;
//using Device = SharpDX.Direct3D11.Device;
//using MapFlags = SharpDX.Direct3D11.MapFlags;


namespace IceFlake.Scripts
{
    #region DrawMeshScript

    public class QKDrawMeshScript : Script
    {
        public QKDrawMeshScript() : base("QK", "_QKDrawMeshScript")
        {
            //colorGreen = System.Drawing.Color.FromArgb(150, 0, 255, 0);
            //colorRed = System.Drawing.Color.FromArgb(150, 255, 0, 0);
            //colorBlue = System.Drawing.Color.FromArgb(150, 0, 0, 255);
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        #region Vertex StructLayout

        [StructLayout(LayoutKind.Sequential)]
        public struct PositionColored
        {
            public static readonly VertexFormat FVF = VertexFormat.Position | VertexFormat.Diffuse;
            public static readonly int Stride = SlimDX.Vector3.SizeInBytes + sizeof(int);

            public SlimDX.Vector3 Position;
            public int Color;

            public PositionColored(SlimDX.Vector3 pos, int col)
            {
                Position = pos;
                Color = col;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex4
        {
            public SlimDX.Vector4 Position;
            public int Color;
        }

        // used?
        //[StructLayout(LayoutKind.Sequential)]
        //public struct Vertex
        //{
        //    public SlimDX.Vector3 Position;
        //    public int color;
        //    public Vertex(SlimDX.Vector3 position, int color)
        //    {
        //        this.Position = position;
        //        this.color = color;
        //    }
        //}

        class CustomVertex
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct TransformedColored
            {
                public SlimDX.Vector4 Position;
                public int Color;
                public static readonly VertexFormat format = VertexFormat.Diffuse | VertexFormat.PositionRhw;
            }
            [StructLayout(LayoutKind.Sequential)]
            public struct PositionColored
            {
                public SlimDX.Vector3 Position;
                public int Color;
                public static readonly VertexFormat format = VertexFormat.Diffuse | VertexFormat.Position;
            }
            [StructLayout(LayoutKind.Sequential)]
            public struct PositionNormalColored
            {
                public SlimDX.Vector3 Position;
                public SlimDX.Vector3 Normal;
                public int Color;
                public static readonly VertexFormat format = VertexFormat.Diffuse | VertexFormat.Position | VertexFormat.Normal;
            }
        }

        #endregion

        private System.Drawing.Color colorGreen;
        private System.Drawing.Color colorRed;
        private System.Drawing.Color colorBlue;

        private List<Location> LocationList = new List<Location>();
        //private Location _tmpLoc = new Location();
        //private Movement _mov = new Movement();

        //private System.Drawing.Color _greenColor = HexColor("33ff66", 100); // green

        //private Color _ambient = Color.Red;
        private Material _material;
        //private Light _light;
        private SlimDX.Direct3D9.Mesh _mesh;

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        public override void OnStart()
        {
            //InitializeD3D();

            if (!Manager.ObjectManager.IsInGame)
            {
                Stop();
                return;
            }
        }

        public override void OnTerminate()
        {
            if (!Manager.ObjectManager.IsInGame)
            {
                Stop();
                return;
            }

            //if (LocationList.Count > 0)
            //    LocationList.Clear();

            // reset
            D3D.Device.SetRenderState(RenderState.FillMode, SlimDX.Direct3D9.FillMode.Solid);
        }

        public override void OnTick()
        {

            var _me = Manager.LocalPlayer;

            if (LocationList.Count < 10000)
            {
                if (!LocationList.Contains(Manager.LocalPlayer.Location))
                    LocationList.Add(Manager.LocalPlayer.Location);
            }

            foreach (Location _loc in LocationList)
            {
                //DrawMesh(new SlimDX.Vector3(_loc.X, _loc.Y, _loc.Z));

                DrawCubeTest(new SlimDX.Vector3(_loc.X, _loc.Y, _loc.Z), 0.5f, 0.5f, 0.5f, true, false);
                
                //DrawLineVertex();
            }

            //if (Manager.LocalPlayer.TargetGuid != 0)
            //    DrawCircle(Manager.LocalPlayer.Target.Location , 5f, colorGreen, colorGreen);

        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        // Variables
        //public static RenderForm Window;        // Window used for rendering.
        //public static Device D3DDevice;         // Direct3D device.
        public static VertexBuffer Vertices;    // Vertex buffer object used to hold vertices.
        //public static int time;                 // Used for rotation caculations.
        //public static float angle;              // Angle of rottaion.

        private static SlimDX.Direct3D9.StateBlock _SB = new SlimDX.Direct3D9.StateBlock(D3D.Device, StateBlockType.All);

        /// <summary>
        /// Renders all of our geometry each frame.
        /// </summary>
        public void DrawTriangleList(SlimDX.Vector3 loc)
        {
            _SB.Capture();

            // Create the vertex buffer and fill with the triangle vertices.
            Vertices = new VertexBuffer(D3D.Device, 3 * D3D.Vertex.SizeBytes, Usage.WriteOnly, VertexFormat.None, Pool.Managed);
            SlimDX.DataStream stream = Vertices.Lock(0, 0, LockFlags.None);
            stream.WriteRange(D3D.BuildVertexData());
            Vertices.Unlock();

            var worldMatrix = SlimDX.Matrix.Translation(loc);
            D3D.Device.SetTransform(TransformState.World, worldMatrix);

            D3D.Device.VertexFormat = D3D.Vertex.Format;

            // Render the vertex buffer.
            D3D.Device.SetStreamSource(0, Vertices, 0, D3D.Vertex.SizeBytes);

            //D3D.Device.DrawPrimitives(PrimitiveType.LineStrip, 0, 1);
            D3D.Device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);

            if (Vertices != null)
                Vertices.Dispose();

            _SB.Apply();
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        private RenderTarget renderTarget;

        public void DrawSomething()
        {
            // Get desktop DPI
            //var dpi = factory.DesktopDpi;

            // Create bitmap render target from DXGI surface
            ////renderTarget = RenderTarget.FromDXGI(factory, backBuffer, new RenderTargetProperties()
            ////{
            ////    //HorizontalDpi = dpi.Width,
            ////    //VerticalDpi = dpi.Height,
            ////    MinimumFeatureLevel = SlimDX.Direct2D.FeatureLevel.Default,
            ////    //PixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Ignore),
            ////    Type = RenderTargetType.Default,
            ////    Usage = RenderTargetUsage.None
            ////});
            
            using (var brush = new SolidColorBrush(renderTarget, new Color4(Color.LightSlateGray)))
            {
                for (int x = 0; x < renderTarget.Size.Width; x += 10)
                    renderTarget.DrawLine(brush, x, 0, x, renderTarget.Size.Height, 0.5f);
                for (int y = 0; y < renderTarget.Size.Height; y += 10)
                    renderTarget.DrawLine(brush, 0, y, renderTarget.Size.Width, y, 0.5f);
                renderTarget.FillRectangle(brush, new RectangleF(renderTarget.Size.Width / 2 - 50, renderTarget.Size.Height / 2 - 50, 100, 100));
            }

            renderTarget.DrawRectangle(new SolidColorBrush(renderTarget, new Color4(Color.CornflowerBlue)),
                new RectangleF(renderTarget.Size.Width / 2 - 100, renderTarget.Size.Height / 2 - 100, 200, 200));
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        // DrawCircle
        private void DrawCircle(Location loc, float radius, System.Drawing.Color innerColor, System.Drawing.Color outerColor, int complexity = 24, bool isFilled = true)
        {
            var vertices = new List<PositionColored>();
            if (isFilled)
                vertices.Add(new PositionColored(SlimDX.Vector3.Zero, innerColor.ToArgb()));

            double stepAngle = (Math.PI * 2) / complexity;
            for (int i = 0; i <= complexity; i++)
            {
                double angle = (Math.PI * 2) - (i * stepAngle);
                float x = (float)(radius * Math.Cos(angle));
                float y = (float)(-radius * Math.Sin(angle));
                vertices.Add(new PositionColored(new SlimDX.Vector3(x, y, 0), outerColor.ToArgb()));
            }

            var buffer = vertices.ToArray();

            SetTarget(loc.ToVector3() + new SlimDX.Vector3(0, 0, 0.3f));

            D3D.Device.DrawUserPrimitives(SlimDX.Direct3D9.PrimitiveType.TriangleFan, buffer.Length - 2, buffer);
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        // SetTarget
        private void SetTarget(SlimDX.Vector3 target, float yaw = 0, float pitch = 0, float roll = 0)
        {
            var worldMatrix = SlimDX.Matrix.Translation(target) * SlimDX.Matrix.RotationYawPitchRoll(yaw, pitch, roll);
            D3D.Device.SetTransform(TransformState.World, worldMatrix);
        }

        //SlimDX.Direct2D.RenderTarget renderTarget;

        //private void DrawBrush()
        //{
        //    // Create bitmap render target from DXGI surface
        //    renderTarget = SlimDX.Direct2D.RenderTarget.FromDXGI(factory, backBuffer, new RenderTargetProperties()
        //    {
        //        HorizontalDpi = 1024,
        //        VerticalDpi = 768,
        //        MinimumFeatureLevel = SlimDX.Direct2D.FeatureLevel.Default,
        //        PixelFormat = new SlimDX.Direct2D.PixelFormat(SlimDX.DXGI.Format.Unknown, SlimDX.Direct2D.AlphaMode.Ignore),
        //        Type = SlimDX.Direct2D.RenderTargetType.Default,
        //        Usage = SlimDX.Direct2D.RenderTargetUsage.None
        //    });

        //    using (var brush = new SlimDX.Direct2D.SolidColorBrush(renderTarget, new Color4(Color.LightSlateGray)))
        //    {
        //        for (int x = 0; x < renderTarget.Size.Width; x += 10)
        //            renderTarget.DrawLine(brush, x, 0, x, renderTarget.Size.Height, 0.5f);
 
        //        for (int y = 0; y < renderTarget.Size.Height; y += 10)
        //            renderTarget.DrawLine(brush, 0, y, renderTarget.Size.Width, y, 0.5f);
 
        //        renderTarget.FillRectangle(brush, new RectangleF(renderTarget.Size.Width / 2 - 50, renderTarget.Size.Height / 2 - 50, 100, 100));
        //    }

        //    renderTarget.DrawRectangle(new SlimDX.Direct2D.SolidColorBrush(renderTarget, new Color4(Color.CornflowerBlue)),
        //        new RectangleF(renderTarget.Size.Width / 2 - 100, renderTarget.Size.Height / 2 - 100, 200, 200));
        //}

        // DrawMesh
        //private void DrawMesh(SlimDX.Direct3D9.Mesh mesh, SlimDX.Vector3 loc, bool wireframe = false, bool isFilled = false)

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>
        
        // DrawCubeTest
        private void DrawCubeTest(SlimDX.Vector3 loc, float width, float height, float depth, bool isFilled = true, bool wireframe = false)
        {
            if (_mesh == null)
                _mesh = SlimDX.Direct3D9.Mesh.CreateBox(D3D.Device, width, height, depth);

            _SB.Capture();

            //D3D.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);

            var worldMatrix = SlimDX.Matrix.Translation(loc);
            D3D.Device.SetTransform(TransformState.World, worldMatrix);

            _mesh.DrawSubset(0);

            D3D.Device.SetRenderState(RenderState.FillMode, SlimDX.Direct3D9.FillMode.Solid);

            _SB.Apply();

            // cleanup
            if (Vertices != null)
                Vertices.Dispose();
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        public struct LineVertex
        {
            public Vector3 Position { get; set; }
            public int Color { get; set; }
        }

        public static void DrawLineVertex()
        {
            LineVertex[] lineData;
            VertexBuffer vertexBuffer;
            Random rnd = new Random(DateTime.Now.Millisecond);

            lineData = new LineVertex[100];

            for (int i = 0; i < lineData.Length; i++)
            {
                float x = (float)(rnd.NextDouble() * 2) - 1;
                float y = (float)(rnd.NextDouble() * 2) - 1;
                lineData[i].Position = new Vector3(x, y, 0f);
            }

            
            D3D.Device.VertexFormat = VertexFormat.Position | VertexFormat.Diffuse;
            D3D.Device.DrawUserPrimitives<LineVertex>(PrimitiveType.LineList,
                                                0,
                                                lineData.Length / 2,
                                                lineData);
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        public static void DrawLineList(Vector3 from, Vector3 to, Color4 color)
        {
            PositionColored[] vertices = new PositionColored[2];
            vertices[0] = new PositionColored(from, color.ToArgb());
            vertices[1] = new PositionColored(to, color.ToArgb());
            D3D.Device.DrawUserPrimitives(PrimitiveType.LineList, 1, vertices);
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        // DrawLine
        private void DrawLineStrip(SlimDX.Vector3 from, SlimDX.Vector3 to, Color4 color)
        {
            var vertices = new List<PositionColored>();

            vertices.Add(new PositionColored(from, color.ToArgb()));
            vertices.Add(new PositionColored(to, color.ToArgb()));

            var buffer = vertices.ToArray();

            //SetTarget(SlimDX.Vector3.Zero);

            D3D.Device.DrawUserPrimitives(SlimDX.Direct3D9.PrimitiveType.LineStrip, vertices.Count - 1, buffer);
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        /// <summary>
        /// Disposes all of the Direct3D objects we created.
        /// </summary>
        public static void Cleanup()
        {
            if (Vertices != null)
                Vertices.Dispose();

            if (D3D.Device != null)
                D3D.Device.Dispose();
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>
    }

    #endregion
}
