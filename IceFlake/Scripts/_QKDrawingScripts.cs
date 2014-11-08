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
//using SlimDX.Direct2D;
using SlimDX.Direct3D9;
//using SlimDX.Direct3D10;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;


//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Content;
//using Xna = Microsoft.Xna.Framework;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;


namespace IceFlake.Scripts
{
    #region DrawUnitsScript

    public class QKDrawScript : Script
    {






















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
        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex
        {
            public SharpDX.Vector3 Position;
            public int color;
            public Vertex(SharpDX.Vector3 position, int color)
            {
                this.Position = position;
                this.color = color;
            }
        }

        class CustomVertex
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct TransformedColored
            {
                public SharpDX.Vector4 Position;
                public int Color;
                public static readonly VertexFormat format = VertexFormat.Diffuse | VertexFormat.PositionRhw;
            }
            [StructLayout(LayoutKind.Sequential)]
            public struct PositionColored
            {
                public SharpDX.Vector3 Position;
                public int Color;
                public static readonly VertexFormat format = VertexFormat.Diffuse | VertexFormat.Position;
            }
            [StructLayout(LayoutKind.Sequential)]
            public struct PositionNormalColored
            {
                public SharpDX.Vector3 Position;
                public SharpDX.Vector3 Normal;
                public int Color;
                public static readonly VertexFormat format = VertexFormat.Diffuse | VertexFormat.Position | VertexFormat.Normal;
            }
        }

        public QKDrawScript()
            : base("QK", "_QKDrawScript")
        {
            colorGreen = System.Drawing.Color.FromArgb(150, 0, 255, 0);
            colorRed = System.Drawing.Color.FromArgb(150, 255, 0, 0);
            colorBlue = System.Drawing.Color.FromArgb(150, 0, 0, 255);
        }

        private System.Drawing.Color colorGreen;
        private System.Drawing.Color colorRed;
        private System.Drawing.Color colorBlue;

        public override void OnStart()
        {
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
                return;
            }

            if (LocationList.Count > 0)
                LocationList.Clear();

            D3D.Device.SetRenderState(RenderState.FillMode, SlimDX.Direct3D9.FillMode.Solid);

        }

        private List<Location> LocationList = new List<Location>();
        private Location _tmpLoc = new Location();
        private Movement _mov = new Movement();

        private System.Drawing.Color _greenColor = HexColor("33ff66", 100); // green
        private SlimDX.Direct3D9.Mesh _mesh;

        public override void OnTick()
        {



            //DrawTriangle();



            //try
            //{
            //    //if (_mov.generatedPath.Count > 0)
            //    //{
            //    foreach (Location _loc in _mov.generatedPath)
            //    {
            //        DrawCircle(_loc, 1f, color, color);
            //    }
            //    //}
            //}
            //catch { }

            //float x1, x2, y1, y2;
            //x1 = Manager.LocalPlayer.Location.X;
            //x2 = Manager.LocalPlayer.Location.X + 20;
            //y1 = Manager.LocalPlayer.Location.Y;
            //y2 = Manager.LocalPlayer.Location.Y + 20;
            //Line line1 = new Line(D3D.Device);
            //Vector2[] a = new Vector2[2];
            //a[0].X = x1;
            //a[0].Y = y1;
            //a[1].X = x2;

            //a[1].Y = y2;
            //line1.Width = 40;
            //line1.Draw(a, color);


            /*/
            string Title = "Cosmic";
            int SizeX = 150;
            int SizeY = 200;
            int RealPointX = 50;
            int RealPointY = 150;
            System.Drawing.Color TitleTextColor = HexColor("0", 100);
            System.Drawing.Color TitleColor = HexColor("33ff66", 100);
            System.Drawing.Color BackgroundColor = HexColor("4e4e4e", 100);
            System.Drawing.Color BorderColor = HexColor("33ff66", 100);
            DrawControl2D(Title, SizeX, SizeY, RealPointX, RealPointY, TitleTextColor, TitleColor, BackgroundColor, BorderColor);
           /*/


            // WireFrame World
            //D3D.Device.SetRenderState(RenderState.FillMode, SlimDX.Direct3D9.FillMode.Wireframe);





            //D3D.Device.SetRenderState(RenderState.FillMode, SlimDX.Direct3D9.FillMode.Solid);

           //DrawLineTest(Manager.LocalPlayer.Location.ToVector3(), Manager.LocalPlayer.Target.Location.ToVector3(), colorBlue, 20);

            if (LocationList.Count < 10000)
            {

                if (!LocationList.Contains(Manager.LocalPlayer.Location))
                    LocationList.Add(Manager.LocalPlayer.Location);
            }

            //if (!LocationList.Contains(Manager.LocalPlayer.Location))
            //    LocationList.Add(Manager.LocalPlayer.Location);
            foreach (Location _loc in LocationList)
            {
                //_tmpLoc = _loc;

                //var color = HexColor("33ff66", 100); // green
                //DrawCircle(_tmpLoc, 1f, color, color, 24 , false);

                //DrawCubeTest(_loc.ToVector3(), 4, 4, 5, false);
                //DrawCubeTest(new Vector3(_loc.X, _loc.Y, (_loc.Z + 2)), 4, 4, 5, false);
                DrawCubeTest(new SlimDX.Vector3(_loc.X, _loc.Y, _loc.Z), 0.5f, 0.5f, 0.5f, true, false);
            }

            /*/
            foreach (var u in Manager.ObjectManager.Objects.Where(x => x.IsValid && x.IsUnit).OfType<WoWUnit>())
            {
                if (u == null || !u.IsValid)
                    continue;

                var color = (!(u.IsFriendly || u.IsNeutral) ? colorRed : colorGreen);
                DrawCircle(u.Location, 3f, color, color);
                DrawLine(Manager.LocalPlayer.Location.ToVector3(), u.Location.ToVector3(), colorBlue);
            }
            /*/













        }











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

                //vertices.Add(new PositionColored(new SlimDX.Vector3(x, y, 0), outerColor.ToArgb()));

                vertices.Add(new PositionColored(new SlimDX.Vector3(x, y, 0), outerColor.ToArgb()));
            }

            var buffer = vertices.ToArray();

            SetTarget(loc.ToVector3() + new SlimDX.Vector3(0, 0, 0.3f));

            D3D.Device.DrawUserPrimitives(SlimDX.Direct3D9.PrimitiveType.TriangleFan, buffer.Length - 2, buffer);


        }

        // DrawLine
        private void DrawLine(SlimDX.Vector3 from, SlimDX.Vector3 to, System.Drawing.Color color)
        {
            var vertices = new List<PositionColored>();

            vertices.Add(new PositionColored(from, color.ToArgb()));
            vertices.Add(new PositionColored(to, color.ToArgb()));
            
            var buffer = vertices.ToArray();

            SetTarget(SlimDX.Vector3.Zero);

            D3D.Device.DrawUserPrimitives(SlimDX.Direct3D9.PrimitiveType.LineStrip, vertices.Count - 1, buffer);
        }

        // DrawTriangle
        private void DrawTriangle() // qk
        {
            var vertices = new VertexBuffer(D3D.Device, 3 * 20, SlimDX.Direct3D9.Usage.WriteOnly, VertexFormat.None, Pool.Managed);
            vertices.Lock(0, 0, LockFlags.None).WriteRange(new[] {
                new Vertex4() { Color = System.Drawing.Color.Red.ToArgb(), Position = new SlimDX.Vector4(400.0f, 100.0f, 0.5f, 10.0f) },
                new Vertex4() { Color = System.Drawing.Color.Blue.ToArgb(), Position = new SlimDX.Vector4(650.0f, 500.0f, 0.5f, 10.0f) },
                new Vertex4() { Color = System.Drawing.Color.Green.ToArgb(), Position = new SlimDX.Vector4(150.0f, 500.0f, 0.5f, 10.0f) },
            });
            vertices.Unlock();

            var vertexElems = new[] {
                        new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.PositionTransformed, 0),
                        new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                                VertexElement.VertexDeclarationEnd
                };
            var vertexDecl = new VertexDeclaration(D3D.Device, vertexElems);

            D3D.Device.SetStreamSource(0, vertices, 0, 20);
            D3D.Device.VertexDeclaration = vertexDecl;
            D3D.Device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
        }







        // DrawTextMesh
        private void DrawText(SlimDX.Vector3 loc, string text)
        {
            var font = new System.Drawing.Font("Consolas", .875f);
            var mesh = Mesh.CreateText(D3D.Device, font, text, 0f, 0f);

            

            DrawMesh(mesh, loc + new SlimDX.Vector3(0, 0, 2f));
        }

        // DrawCube
        //private void DrawCube(SlimDX.Vector3 loc, float width, float height, float depth)
        //{
        //    //var mesh = Mesh.CreateBox(D3D.Device, width, height, depth);
        //    if (_mesh == null)
        //        _mesh = Mesh.CreateBox(D3D.Device, width, height, depth);

        //    DrawMesh(_mesh, loc + new SlimDX.Vector3(0, 0, 1.3f), true);
        //}











        // DrawCubeTest
        private void DrawCubeTest(SlimDX.Vector3 loc, float width, float height, float depth, bool isFilled = true, bool wireframe = false)
        {
            //var mesh = Mesh.CreateBox(D3D.Device, width, height, depth);
            if (_mesh == null)
                _mesh = Mesh.CreateBox(D3D.Device, width, height, depth);

            //light = new Light();
            //light.Type = LightType.Point;
            //light.Range = 75;
            //light.Position = new SlimDX.Vector3(10, 25, 0);
            //light.Falloff = 1.0f;
            //light.Diffuse = System.Drawing.Color.LemonChiffon;
            //light.Ambient = ambient;

            //activeMaterial = new Material();
            //activeMaterial.Diffuse = System.Drawing.Color.Orange;
            //activeMaterial.Ambient = ambient;

            //passiveMaterial = new Material();
            //passiveMaterial.Diffuse = System.Drawing.Color.Red;
            //passiveMaterial.Ambient = ambient;

            //groundMaterial = new Material();
            //groundMaterial.Diffuse = System.Drawing.Color.Green;
            //groundMaterial.Ambient = ambient;







            //_mesh.Device.
            //_mesh.Device.ColorFill()

            //DrawMesh(_mesh, loc + new SlimDX.Vector3(0, 0, 0.3f));
            //DrawMesh(mesh, loc);


            //_mesh.

            DrawMesh(_mesh, loc + new SlimDX.Vector3(0, 0, (depth / 2)), wireframe, isFilled); // (depth / 2) prevent drawing below ground if using player coords to draw

            //DrawMesh(_mesh, loc + new SlimDX.Vector3(0, 0, (depth / 2)), wireframe, isFilled); // (depth / 2) prevent drawing below ground if using player coords to draw



            //SlimDX.Color4 w_color4 = new SlimDX.Color4();

            //w_color4

           // GetTexture(D3D.Device, 50, 50, System.Drawing.Color.Red;


        }







        // SlimDX.Direct3D10
        private SlimDX.Direct3D10.ShaderResourceView GetTexture(SlimDX.Direct3D10.Device device, int width, int height, SlimDX.Color4 color)
        {
            //create the texture
            SlimDX.Direct3D10.Texture2D texture = null;
            SlimDX.Direct3D10.Texture2DDescription desc2 = new SlimDX.Direct3D10.Texture2DDescription();
            desc2.SampleDescription = new SlimDX.DXGI.SampleDescription(1, 0);
            desc2.Width = width;
            desc2.Height = height;
            desc2.MipLevels = 1;
            desc2.ArraySize = 1;
            desc2.Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm;
            desc2.Usage = SlimDX.Direct3D10.ResourceUsage.Dynamic;
            desc2.BindFlags = SlimDX.Direct3D10.BindFlags.ShaderResource;
            desc2.CpuAccessFlags = SlimDX.Direct3D10.CpuAccessFlags.Write;
            texture = new SlimDX.Direct3D10.Texture2D(device, desc2);

            
            // fill the texture with rgba values
            SlimDX.DataRectangle rect = texture.Map(0, SlimDX.Direct3D10.MapMode.WriteDiscard, SlimDX.Direct3D10.MapFlags.None);
            if (rect.Data.CanWrite)
            {
                for (int row = 0; row < texture.Description.Height; row++)
                {
                    int rowStart = row * rect.Pitch;
                    rect.Data.Seek(rowStart, System.IO.SeekOrigin.Begin);
                    for (int col = 0; col < texture.Description.Width; col++)
                    {
                        rect.Data.WriteByte((byte)color.Red);
                        rect.Data.WriteByte((byte)color.Green);
                        rect.Data.WriteByte((byte)color.Blue);
                        rect.Data.WriteByte((byte)color.Alpha);
                    }
                }
            }
            texture.Unmap(0);
            
            // create shader resource that is what the renderer needs
            SlimDX.Direct3D10.ShaderResourceViewDescription desc = new SlimDX.Direct3D10.ShaderResourceViewDescription();
            desc.Format = texture.Description.Format;
            desc.Dimension = SlimDX.Direct3D10.ShaderResourceViewDimension.Texture2D;
            desc.MostDetailedMip = 0;
            desc.MipLevels = 1;


            SlimDX.Direct3D10.ShaderResourceView srv = new SlimDX.Direct3D10.ShaderResourceView(device, texture, desc);

            return srv;



        }








        Light light;
        System.Drawing.Color ambient = System.Drawing.Color.Red;
        Material activeMaterial, passiveMaterial, groundMaterial;

        // DrawMesh
        private void DrawMesh(SlimDX.Direct3D9.Mesh mesh, SlimDX.Vector3 loc, bool wireframe = false, bool isFilled = false)
        {










            light = new Light();
            light.Type = LightType.Point;
            light.Range = 75;
            light.Position = new SlimDX.Vector3(10, 25, 0);
            light.Falloff = 1.0f;
            light.Diffuse = System.Drawing.Color.LemonChiffon;
            light.Ambient = ambient;



            activeMaterial = new Material();
            activeMaterial.Diffuse = System.Drawing.Color.Orange;
            activeMaterial.Ambient = ambient;

            passiveMaterial = new Material();
            passiveMaterial.Diffuse = System.Drawing.Color.Red;
            passiveMaterial.Ambient = ambient;

            groundMaterial = new Material();
            groundMaterial.Diffuse = System.Drawing.Color.Green;
            groundMaterial.Ambient = ambient;






            if (isFilled)
            {
                if (wireframe)
                    D3D.Device.SetRenderState(RenderState.FillMode, SlimDX.Direct3D9.FillMode.Solid);
            }
            else
            {
                if (wireframe)
                    D3D.Device.SetRenderState(RenderState.FillMode, SlimDX.Direct3D9.FillMode.Wireframe);
            }
            var worldMatrix = SlimDX.Matrix.Translation(loc);





            //D3D.Device.SetLight(0, light);
            //D3D.Device.EnableLight(0, true);
            //D3D.Device.SetRenderState(RenderState.Ambient, ambient.ToArgb());
            //D3D.Device.Material = groundMaterial;

            D3D.Device.SetTransform(TransformState.World, worldMatrix);

            mesh.DrawSubset(0);


            if (isFilled)
            {
                if (wireframe)
                    D3D.Device.SetRenderState(RenderState.FillMode, SlimDX.Direct3D9.FillMode.Wireframe);
            }
            else
            {
                //if (wireframe)
                    //D3D.Device.SetRenderState(RenderState.FillMode, SlimDX.Direct3D9.FillMode.Solid);
            }





        }

        // SetTarget
        private void SetTarget(SlimDX.Vector3 target, float yaw = 0, float pitch = 0, float roll = 0)
        {
            var worldMatrix = SlimDX.Matrix.Translation(target) * SlimDX.Matrix.RotationYawPitchRoll(yaw, pitch, roll);
            D3D.Device.SetTransform(TransformState.World, worldMatrix);
        }

        /// <summary>
        /// Creates a solid color Brush object based on a hex color code string.
        /// </summary>
        /// <param name="sHex">The hex color to use.</param>
        /// <param name="Alpha">The alpha color value.</param>
        public static System.Drawing.Color HexColor(string sHex, int Alpha = 255)
        {
            sHex = sHex.ToLower().Trim();
            if (sHex.StartsWith("0x"))
                sHex = sHex.Substring(2);
            try
            {
                int red = Convert.ToInt32(sHex.Substring(0, 2), 16);
                int green = Convert.ToInt32(sHex.Substring(2, 2), 16);
                int blue = Convert.ToInt32(sHex.Substring(4, 2), 16);

                if (Alpha > 255)
                    return System.Drawing.Color.FromArgb(red, green, blue);
                else
                    return System.Drawing.Color.FromArgb(Alpha, red, green, blue);
            }
            catch { }

            return System.Drawing.Color.Black;
        }

        // Draw Font
        internal SlimDX.Direct3D9.Font _font = new SlimDX.Direct3D9.Font(D3D.Device, new System.Drawing.Font("Arial", 10f, FontStyle.Bold));
        internal void DrawString2D(string Text, int X, int Y, System.Drawing.Color Color, string FontName, bool FontStyleBold = false)
        {
            if (FontStyleBold)
                _font = new SlimDX.Direct3D9.Font(D3D.Device, new System.Drawing.Font(FontName, 12f, FontStyle.Bold));
            else
                _font = new SlimDX.Direct3D9.Font(D3D.Device, new System.Drawing.Font(FontName, 12f));

            DrawString2D(Text, X, Y, Color);
        }
        internal void DrawString2D(string Text, int X, int Y, System.Drawing.Color Color)
        {
            _font.DrawString(null, Text, X, Y, (SlimDX.Color4)Color);
        }

        // DrawControl2D
        internal void DrawControl2D(string Title, int SizeX, int SizeY, int PosX, int PosY, System.Drawing.Color TitleTextColor, System.Drawing.Color TitleColor, System.Drawing.Color BackgroundColor, System.Drawing.Color BorderColor)
        {
            int num = 18;
            int num2 = PosY +1;
            if (SizeY >= 18)
            {
                // Draw Header
                SlimDX.Direct3D9.Line line = new SlimDX.Direct3D9.Line(D3D.Device);
                List<SlimDX.Vector2> list = new List<SlimDX.Vector2>();
                for (int i = 0; i < num; i++)
                {
                    list.Add(new SlimDX.Vector2((float)PosX, (float)num2));
                    list.Add(new SlimDX.Vector2((float)(PosX + SizeX), (float)num2));
                    num2++;
                }
                line.Draw(list.ToArray(), TitleColor);


                // Draw Title
                DrawString2D(Title, PosX + 4, PosY + 1, TitleTextColor);

                // Draw Background
                List<SlimDX.Vector2> list2 = new List<SlimDX.Vector2>();
                for (int j = num; j < SizeY; j++)
                {
                    list2.Add(new SlimDX.Vector2((float)PosX, (float)num2));
                    list2.Add(new SlimDX.Vector2((float)(PosX + SizeX), (float)num2));
                    num2++;
                }
                line.Draw(list2.ToArray(), BackgroundColor);

                // Draw top border
                SlimDX.Vector2[] vertexList = new SlimDX.Vector2[] { new SlimDX.Vector2((float)PosX, (float)PosY), new SlimDX.Vector2((float)(PosX + SizeX), (float)PosY) };
                line.Draw(vertexList, BorderColor);

                // Draw bottom border
                SlimDX.Vector2[] vectorArray2 = new SlimDX.Vector2[] { new SlimDX.Vector2((float)PosX, (float)(PosY + SizeY)), new SlimDX.Vector2((float)((PosX + SizeX) + 1), (float)(PosY + SizeY)) };
                line.Draw(vectorArray2, BorderColor);

                // Draw right border
                SlimDX.Vector2[] vectorArray3 = new SlimDX.Vector2[] { new SlimDX.Vector2((float)(PosX + SizeX), (float)PosY), new SlimDX.Vector2((float)(PosX + SizeX), (float)(PosY + SizeY)) };
                line.Draw(vectorArray3, BorderColor);

                // Draw left border
                SlimDX.Vector2[] vectorArray4 = new SlimDX.Vector2[] { new SlimDX.Vector2((float)PosX, (float)PosY), new SlimDX.Vector2((float)PosX, (float)(PosY + SizeY)) };
                line.Draw(vectorArray4, BorderColor);

                // Dispose
                line.Dispose();
            }
        }

        // DrawLineTest
        private void DrawLineTest(SlimDX.Vector3 from, SlimDX.Vector3 to, System.Drawing.Color color, int SizeY)
        {
            int num = 0x12;
            //int num2 = PosY;
            if (SizeY >= 0x12)
            {
                //var vertices = new List<PositionColored>();
                //for (int i = 0; i < num; i++)
                //{
                //    vertices.Add(new PositionColored(from, color.ToArgb()));
                //    vertices.Add(new PositionColored(to, color.ToArgb()));
                //}
                //var buffer = vertices.ToArray();

                //SetTarget(SlimDX.Vector3.Zero);

                //D3D.Device.DrawUserPrimitives(SlimDX.Direct3D9.PrimitiveType.LineStrip, vertices.Count - 1, buffer);





                Line line = new Line(D3D.Device);
                List<SlimDX.Vector3> list2 = new List<SlimDX.Vector3>();
                for (int j = num; j < SizeY; j++)
                {
                    list2.Add(new SlimDX.Vector3(from.X, from.Y, from.Z));
                    list2.Add(new SlimDX.Vector3(from.X + 1f, from.Y + 1f, from.Z));
                    list2.Add(new SlimDX.Vector3(from.X + 2f, from.Y + 2f, from.Z));

                    //list2.Add(new Vector3((float)PosX, (float)num2), (float)num2);
                    //list2.Add(new Vector2((float)(PosX + SizeX), (float)num2));
                    //num2++;
                }
                var worldMatrix = SlimDX.Matrix.Translation(from);
                //var worldMatrix = SlimDX.Matrix.Translation(target) * SlimDX.Matrix.RotationYawPitchRoll(yaw, pitch, roll);
                line.DrawTransformed(list2.ToArray(), worldMatrix, color);



                //Line line1 = new Line(D3D.Device);
                //Vector2[] a = new Vector2[2];
                //a[0].X = from.X;
                //a[0].Y = from.Y;
                //a[1].X = to.X;
                //a[1].Y = to.Y;
                //line1.Width = 40;
                //line1.Draw(a, color);



                //Line line = new Line(D3D.Device);
                //List<Vector2> list = new List<Vector2>();
                //for (int i = 0; i < num; i++)
                //{
                //    list.Add(new Vector2((float)from.X, (float)from.Y));
                //    list.Add(new Vector2((float)to.X, (float)to.Y));
                //    //num2++;
                //}
                //line.Width = 40;
                //line.Draw(list.ToArray(), color);



            }
        }













        /*/
        private void DrawLineStrip(Xna.Vector3 from, Xna.Vector3 to, Xna.Color color)
        {
            var vertices = new List<PositionColored>();


            int points = 8;
            short[] lineStripIndices;

            // Initialize an array of indices of type short.
            lineStripIndices = new short[points + 1];

            // Populate the array with references to indices in the vertex buffer.
            for (int i = 0; i < points; i++)
            {
                lineStripIndices[i] = (short)(i + 1);
            }

            lineStripIndices[points] = 1;

            D3D.Device.DrawUserIndexedPrimitives(
            PrimitiveType.LineStrip,
            pointList,
            0,   // vertex buffer offset to add to each element of the index buffer
            9,   // number of vertices to draw
            lineStripIndices,
            0,   // first index element to read
            8    // number of primitives to draw
            );

            


            vertices.Add(new PositionColored(from, color.ToArgb()));
            vertices.Add(new PositionColored(to, color.ToArgb()));

            var buffer = vertices.ToArray();

            SetTarget(Vector3.Zero);

            D3D.Device.DrawUserPrimitives(PrimitiveType.LineStrip, vertices.Count - 1, buffer);
        }
        /*/





































        private void DrawLineTest2(SlimDX.Vector3 from, SlimDX.Vector3 to, System.Drawing.Color color)
        {



            //Device device;
            // create test vertex data, making sure to rewind the stream afterward
            //var vertices = new DataStream(12 * 3, true, true);
            //vertices.Write(new Vector3(0.0f, 0.5f, 0.5f));
            //vertices.Write(new Vector3(0.5f, -0.5f, 0.5f));
            //vertices.Write(new Vector3(-0.5f, -0.5f, 0.5f));
            //vertices.Position = 0;
            //var vertexBuffer = new SlimDX.Direct3D11.Buffer(device, vertices, 12 * 3, 0, 0);




            float x1, x2, y1, y2;

            x1 = Manager.LocalPlayer.Location.X;
            x2 = Manager.LocalPlayer.Location.X + 20;
            y1 = Manager.LocalPlayer.Location.Y;
            y2 = Manager.LocalPlayer.Location.Y + 20;



            Line line1 = new Line(D3D.Device);
            SlimDX.Vector2[] a = new SlimDX.Vector2[2];
            a[0].X = x1;
            a[0].Y = y1;
            a[1].X = x2;
            a[1].Y = y2;
            line1.Width = 40;
            line1.Draw(a, color);






            //var vertices = new List<PositionColored>();
            //vertices.Add(new PositionColored(from, color.ToArgb()));
            //vertices.Add(new PositionColored(to, color.ToArgb()));
            //var buffer = vertices.ToArray();

            //SetTarget(SlimDX.Vector3.Zero);

            //D3D.Device.DrawUserPrimitives(SlimDX.Direct3D9.PrimitiveType.LineStrip, vertices.Count - 1, buffer);
        }




















        /*/
        public void renderAxis(IntPtr device, Microsoft.Xna.Framework.Matrix view, Microsoft.Xna.Framework.Matrix projection, Microsoft.Xna.Framework.Matrix world, float size)
        {

            world = Microsoft.Xna.Framework.Matrix.CreateScale(size) * world;

            VertexPositionColor[] pointList = new VertexPositionColor[6];
            pointList[0].Position = new Microsoft.Xna.Framework.Vector3(0.0f, 0.0f, 0.0f);
            pointList[0].Color = Microsoft.Xna.Framework.Color.Red;
            pointList[1].Position = new Microsoft.Xna.Framework.Vector3(1.0f, 0.0f, 0.0f);
            pointList[1].Color = Microsoft.Xna.Framework.Color.Red;
            pointList[2].Position = new Microsoft.Xna.Framework.Vector3(0.0f, 0.0f, 0.0f);
            pointList[2].Color = Microsoft.Xna.Framework.Color.Green;
            pointList[3].Position = new Microsoft.Xna.Framework.Vector3(0.0f, 1.0f, 0.0f);
            pointList[3].Color = Microsoft.Xna.Framework.Color.Green;
            pointList[4].Position = new Microsoft.Xna.Framework.Vector3(0.0f, 0.0f, 0.0f);
            pointList[4].Color = Microsoft.Xna.Framework.Color.Blue;
            pointList[5].Position = new Microsoft.Xna.Framework.Vector3(0.0f, 0.0f, 1.0f);
            pointList[5].Color = Microsoft.Xna.Framework.Color.Blue;

            Microsoft.Xna.Framework.Graphics.VertexDeclaration decl = new Microsoft.Xna.Framework.Graphics.VertexDeclaration(device, VertexPositionColor.VertexElements);

            device.VertexDeclaration = decl;

            simple_effect.Parameters["World"].SetValue(world);
            simple_effect.Parameters["View"].SetValue(view);
            simple_effect.Parameters["Projection"].SetValue(projection);

            simple_effect.Begin();
            foreach (EffectPass pass in simple_effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                device.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList,
                    pointList,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    3  // number of vertices in pointList
                );
                pass.End();
            }
            simple_effect.End();
        }
        /*/
    }

    #endregion
}
