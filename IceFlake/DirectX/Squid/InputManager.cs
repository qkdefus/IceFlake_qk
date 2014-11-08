using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using Squid;
using SlimDX;
using SlimDX.RawInput;
using SlimDX.Multimedia;
using D3D = IceFlake.DirectX.Direct3D;

namespace SquidSlimDX
{
    public class KeyboardBuffer : Dictionary<int, bool> { }

    public class InputManager : GameComponent
    {
        private int Wheel;
        private int LastWheel;
        private KeyboardBuffer Buffer = new KeyboardBuffer();
        private bool[] Buttons = new bool[4];
        private static Dictionary<System.Windows.Forms.Keys, int> SpecialKeys = new Dictionary<System.Windows.Forms.Keys, int>();

        public InputManager(Game game)
            : base(game)
        {
            SpecialKeys.Add(System.Windows.Forms.Keys.Home, 0xC7);
            SpecialKeys.Add(System.Windows.Forms.Keys.Up, 0xC8);
            SpecialKeys.Add(System.Windows.Forms.Keys.Left, 0xCB);
            SpecialKeys.Add(System.Windows.Forms.Keys.Right, 0xCD);
            SpecialKeys.Add(System.Windows.Forms.Keys.End, 0xCF);
            SpecialKeys.Add(System.Windows.Forms.Keys.Down, 0xD0);
            SpecialKeys.Add(System.Windows.Forms.Keys.Insert, 0xD2);
            SpecialKeys.Add(System.Windows.Forms.Keys.Delete, 0xD3);
            SpecialKeys.Add(System.Windows.Forms.Keys.MediaPreviousTrack, 0x90);
            
            Device.RegisterDevice(UsagePage.Generic, UsageId.Keyboard, DeviceFlags.None);
            Device.RegisterDevice(UsagePage.Generic, UsageId.Mouse, DeviceFlags.None);

            Device.KeyboardInput += new EventHandler<KeyboardInputEventArgs>(Device_KeyboardInput);
            Device.MouseInput += new EventHandler<MouseInputEventArgs>(Device_MouseInput);
        }

        void Device_MouseInput(object sender, MouseInputEventArgs e)
        {
            switch (e.ButtonFlags)
            {
                case MouseButtonFlags.MouseWheel:
                    Wheel = e.WheelDelta > LastWheel ? -1 : (e.WheelDelta < LastWheel ? 1 : 0);
                    LastWheel = e.WheelDelta;
                    break;
                case MouseButtonFlags.LeftDown:
                    Buttons[0] = true;
                    break;
                case MouseButtonFlags.LeftUp:
                    Buttons[0] = false;
                    break;
                case MouseButtonFlags.RightDown:
                    Buttons[1] = true;
                    break;
                case MouseButtonFlags.RightUp:
                    Buttons[1] = false;
                    break;
            }
        }

        void Device_KeyboardInput(object sender, KeyboardInputEventArgs e)
        {
            if (e.State == KeyState.Pressed || e.State == KeyState.Released)
            {
                int code = e.MakeCode;

                if (SpecialKeys.ContainsKey(e.Key))
                    code = SpecialKeys[e.Key];

                if (!Buffer.ContainsKey(code))
                    Buffer.Add(code, e.State == KeyState.Pressed);
            }
        }

        //public override void Update(GameTime time)
        public override void Update()
        {
            List<KeyData> data = new List<KeyData>();

            foreach (int key in Buffer.Keys)
                data.Add(new KeyData { Pressed = Buffer[key], Released = !Buffer[key], Scancode = key });

            Buffer.Clear();

            //System.Drawing.Point p = Game.Form.PointToClient(System.Windows.Forms.Cursor.Position);
            System.Drawing.Point p = Game.Form.PointToClient(System.Windows.Forms.Cursor.Position);


            GuiHost.SetMouse(p.X, p.Y, Wheel);
            GuiHost.SetButtons(Buttons);
            GuiHost.SetKeyboard(data.ToArray());
            GuiHost.TimeElapsed = Game.ElapsedMilliseconds;
        }
    }
}
