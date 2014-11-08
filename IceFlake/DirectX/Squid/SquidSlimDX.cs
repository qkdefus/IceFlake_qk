using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D9;
using System.Runtime.InteropServices;
using D3D = IceFlake.DirectX.Direct3D;

namespace SquidSlimDX
{
    public class RendererSlimDX : Squid.ISquidRenderer, IDisposable
    {
        [DllImport("user32.dll")]
        private static extern int GetKeyboardLayout(int dwLayout);
        [DllImport("user32.dll")]
        private static extern int GetKeyboardState(ref byte pbKeyState);
        [DllImport("user32.dll", EntryPoint = "MapVirtualKeyEx")]
        private static extern int MapVirtualKeyExA(int uCode, int uMapType, int dwhkl);
        [DllImport("user32.dll")]
        private static extern int ToAsciiEx(int uVirtKey, int uScanCode, ref byte lpKeyState, ref short lpChar, int uFlags, int dwhkl);

        private int KeyboardLayout;
        private byte[] KeyStates;

        private Device Device;
        private Sprite Spritebatch;
        private Texture BlankTexture;

        private int FontIndex;
        private int TextureIndex;

        private System.Drawing.Rectangle ScissorRect;

        private Dictionary<int, Font> Fonts = new Dictionary<int, Font>();
        private Dictionary<string, int> FontLookup = new Dictionary<string, int>();
        private Dictionary<int, Texture> Textures = new Dictionary<int, Texture>();
        private Dictionary<string, int> TextureLookup = new Dictionary<string, int>();
        private Dictionary<string, Squid.Font> FontTypes = new Dictionary<string, Squid.Font>();

        public RendererSlimDX(Device device)
        {
            Device = device;
            Spritebatch = new Sprite(device);

            BlankTexture = new Texture(D3D.Device, 8, 8, 0, Usage.None, Format.A8R8G8B8, Pool.Managed);
            BlankTexture.Fill(new Fill2DCallback(delegate(SlimDX.Vector2 a, SlimDX.Vector2 b) { return new SlimDX.Color4(System.Drawing.Color.White); }));

            FontTypes.Add(Squid.Font.Default, new Squid.Font { Name = "Arial10", Family = "Arial", Size = 14, Bold = true, International = true });

            KeyboardLayout = GetKeyboardLayout(0);
            KeyStates = new byte[0x100];
        }

        public bool TranslateKey(int code, ref char character)
        {
            short lpChar = 0;
            if (GetKeyboardState(ref KeyStates[0]) == 0)
                return false;

            int result = ToAsciiEx(MapVirtualKeyExA(code, 1, KeyboardLayout), code, ref KeyStates[0], ref lpChar, 0, KeyboardLayout);
            if (result == 1)
            {
                character = (char)((ushort)lpChar);
                return true;
            }

            return false;
        }

        public int GetTexture(string name)
        {
            if (TextureLookup.ContainsKey(name))
                return TextureLookup[name];

            string filename = name;
            string[] files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, System.IO.Path.GetFileName(name), System.IO.SearchOption.AllDirectories);

            if (files.Length > 0)
                filename = files[0];

            Texture texture = Texture.FromFile
                (
                    Device,
                    //D3D.Device,

                    filename,
                    D3DX.DefaultNonPowerOf2,
                    D3DX.DefaultNonPowerOf2,
                    0,
                    Usage.None,
                    Format.A8R8G8B8,
                    Pool.Managed, Filter.Default,
                    Filter.Default, 0
                 );

            TextureIndex++;

            TextureLookup.Add(name, TextureIndex);
            Textures.Add(TextureIndex, texture);

            return TextureIndex;
        }

        public int GetFont(string name)
        {
            if (FontLookup.ContainsKey(name))
                return FontLookup[name];

            if (!FontTypes.ContainsKey(name))
                return -1;

            Squid.Font type = FontTypes[name];

            Font font = new Font
                (
                    Device,
                    //D3D.Device,

                    type.Size,
                    0,
                    FontWeight.Bold,
                    0,
                    false,
                    CharacterSet.Ansi,
                    Precision.TrueType,
                    FontQuality.ClearType,
                    PitchAndFamily.Default,
                    type.Family
                );

            FontIndex++;

            FontLookup.Add(name, FontIndex);
            Fonts.Add(FontIndex, font);

            return FontIndex;
        }

        public Squid.Point GetTextSize(string text, int font)
        {
            if (string.IsNullOrEmpty(text))
                return new Squid.Point();

            Font f = Fonts[font];

            System.Drawing.Rectangle size = f.MeasureString(Spritebatch, text, DrawTextFormat.SingleLine);

            return new Squid.Point(size.Width + 3, size.Height);
        }

        public Squid.Point GetTextureSize(int texture)
        {
            Texture tex = Textures[texture];
            return new Squid.Point(tex.GetLevelDescription(0).Width, tex.GetLevelDescription(0).Height);
        }

        public void DrawBox(int x, int y, int width, int height, int color)
        {
            System.Drawing.Rectangle destination = new System.Drawing.Rectangle(x, y, width, height);

            Spritebatch.Transform = SlimDX.Matrix.Translation(x, y, 0);
            Spritebatch.Draw(BlankTexture, destination, new SlimDX.Color4(color));
            Spritebatch.Transform = SlimDX.Matrix.Identity;
        }

        public void DrawText(string text, int x, int y, int font, int color)
        {
            if (!Fonts.ContainsKey(font))
                return;

            Font f = Fonts[font];
            f.DrawString(Spritebatch, text, x, y, color);
        }

        public void DrawTexture(int texture, int x, int y, int width, int height, Squid.Rectangle rect, int color)
        {
            if (!Textures.ContainsKey(texture))
                return;

            Texture tex = Textures[texture];

            System.Drawing.Rectangle source = new System.Drawing.Rectangle();

            source.X = rect.Left;
            source.Y = rect.Top;
            source.Width = rect.Width;
            source.Height = rect.Height;

            float scaleX = (float)width / (float)source.Width;
            float scaleY = (float)height / (float)source.Height;

            Spritebatch.Transform = SlimDX.Matrix.Scaling(scaleX, scaleY, 0) * SlimDX.Matrix.Translation(x, y, 0);
            Spritebatch.Draw(tex, source, SlimDX.Vector3.Zero, SlimDX.Vector3.Zero, new SlimDX.Color4(color));
            Spritebatch.Transform = SlimDX.Matrix.Identity;
        }

        public void Scissor(int x, int y, int width, int height)
        {
            ScissorRect = new System.Drawing.Rectangle(x, y, width, height);
            D3D.Device.SetRenderState(RenderState.ScissorTestEnable, true);
            D3D.Device.ScissorRect = ScissorRect;
        }

        public void StartBatch()
        {
            Spritebatch.Begin(SpriteFlags.AlphaBlend | SpriteFlags.DoNotSaveState);

            if (D3D.Device.TestCooperativeLevel().IsSuccess)
                D3D.Device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.None);
        }

        public void EndBatch(bool final)
        {
            D3D.Device.SetRenderState(RenderState.AlphaTestEnable, false);
            D3D.Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            Spritebatch.End();
        }

        private void Dispose(bool disposed)
        {
            Spritebatch.Dispose();
            BlankTexture.Dispose();

            foreach (Texture t in Textures.Values)
                t.Dispose();

            foreach (Font f in Fonts.Values)
                f.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
