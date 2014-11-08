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

using Squid;
using SquidSlimDX;

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

    public class QKDrawMenuScript : Script
    {
        public QKDrawMenuScript(): base("QK", "_QKDrawMenuScript"){ }

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
        }

        public override void OnTick()
        {
            UIGame(D3D.Device);
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>

        public void UIGame(Device device)
        {
            // DRAW IMAGE
            //_pnt = new Squid.Point(50, 50);
            //_rect = new Squid.Rectangle(_pnt, _pnt);
            ////string filename = "image.png";
            //string filename = "TestImage.png";
            //var tex = GuiHost.Renderer.GetTexture(filename);
            //GuiHost.Renderer.StartBatch();
            ////GuiHost.Renderer.DrawTexture(tex, 550, 600, 128, 128, _rect, -1);
            //GuiHost.Renderer.DrawTexture(tex, 200, 250, 300, 450, _rect, -1);
            //GuiHost.Renderer.EndBatch(true);
            //GuiHost.Renderer.Dispose();

            using (SampleGame game = new SampleGame())
            {
                game.Run();
            }

            // cleanup
            GuiHost.Renderer.Dispose();
        }

        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>



        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>



        /// <summary>
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// ///////////////////////////////
        /// </summary>
    }

    #endregion
}
