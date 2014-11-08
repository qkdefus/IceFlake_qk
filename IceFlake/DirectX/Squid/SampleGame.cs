using System;
using Framework;
using Squid;
using D3D = IceFlake.DirectX.Direct3D;

namespace SquidSlimDX
{
    public class SampleGame : Game
    {
        protected override void Initialize()
        {
            //GuiHost.Renderer = new RendererSlimDX(Device);
            GuiHost.Renderer = new RendererSlimDX(D3D.Device);

            //InputManager input = new InputManager(this);
            //Components.Add(input);

            SampleScene scene = new SampleScene(this);
            Components.Add(scene);

            base.Initialize();
        }

        protected override void DeviceReset()
        {
            GuiHost.Renderer.Dispose();
            //GuiHost.Renderer = new RendererSlimDX(Device);
            GuiHost.Renderer = new RendererSlimDX(D3D.Device);

            base.DeviceReset();
       }
    }
}
