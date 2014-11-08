using System;
using System.Collections.Generic;
using System.Text;
using Squid;
using Framework;
using SampleControls;

namespace SquidSlimDX
{
    public class SampleScene : DrawableGameComponent
    {
        private Desktop Desktop;

        public SampleScene(Game game)
            : base(game)
        {
        }

        public override void Load()
        {
            //Desktop = new GameGui { Name = "desk" };
            Desktop = new SampleDesktop { Name = "desk" };
            Desktop.ShowCursor = true;

            // -- Uncomment to load and apply the style texture
            //ReadAtlas("SampleMap");

            base.Load();
        }

        //public override void Draw(GameTime time)
        public override void Draw()
        {
            //Desktop.Size = new Point(Game.Device.Viewport.Width, Game.Device.Viewport.Height);

            int _w = 1280;
            int _h = 1024;

            Desktop.Size = new Point(_w, _h);

            Desktop.Update();
            Desktop.Draw();

            //int tex = GuiHost.Renderer.GetTexture("slimdx_logo.png");
            //Squid.Point size = GuiHost.Renderer.GetTextureSize(tex);
           // Squid.Rectangle rect = new Squid.Rectangle(Point.Zero, size);

            //GuiHost.Renderer.StartBatch();
            //GuiHost.Renderer.DrawTexture(tex, _w - 130, _h - 130, 128, 128, rect, -1);
            //GuiHost.Renderer.EndBatch(true);
        }

        /*/
        private void ReadAtlas(string mapName)
        {
            Atlas atlas = new Atlas();
            atlas.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "Content\\" + mapName + ".xml");

            foreach (ControlStyle style in GuiHost.GetSkin().Styles.Values)
            {
                foreach (Style state in style.Styles.Values)
                {
                    if (string.IsNullOrEmpty(state.Texture))
                        continue;

                    if (atlas.Contains(state.Texture))
                    {
                        state.TextureRect = atlas.GetRect(state.Texture);
                        state.Texture = mapName + ".png";
                    }
                }
            }
        }
         */
    }
}
