using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using SlimDX.Direct3D9;
using System.Diagnostics;

using D3D = IceFlake.DirectX.Direct3D;

namespace Framework
{
    public abstract class Game : IDisposable
    {
        public static float ElapsedSeconds { get; private set; }
        public static float ElapsedMilliseconds { get; private set; }

        private static PresentParameters Params;
        //private GameTime Time = new GameTime();
        //private Stopwatch Clock = new Stopwatch();

        #region public properties

        public Device Device { get; private set; }
        public Form Form { get; private set; }
        public List<GameComponent> Components { get; private set; }
        public bool Active { get; private set; }
        public bool Running { get; private set; }

        #endregion

        #region ctor
       
        public Game()
        {
            //Form = new Form();
            //Form.Activated += Window_Activated;
            //Form.Deactivate += Window_Deactivate;
            //Form.ResizeEnd += Window_ResizeEnd;
        }

        #endregion

        #region public methods

        public void Run()
        {
            if (Running) return;

            Start();
            Initialize();
            Load();

            Running = true;

            //while (NativeMethods.AppStillIdle)
            //{
            //    if (Running)
                    Tick();
            //}

            //Application.Idle += Application_Idle;
            //Application.Run(Form);

                    Dispose();
        }

        public void AddComponent(GameComponent component)
        {
            Components.Add(component);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region private methods

        private void Start()
        {
            //Time = new GameTime();
            Components = new List<GameComponent>();

            //Params = new PresentParameters();
            //Params.DeviceWindowHandle = Form.Handle;
            //Params.BackBufferFormat = Format.X8R8G8B8;
            //Params.BackBufferCount = 1;
            //Params.BackBufferWidth = Form.ClientSize.Width;
            //Params.BackBufferHeight = Form.ClientSize.Height;
            //Params.Multisample = MultisampleType.None;
            //Params.SwapEffect = SwapEffect.Discard;
            //Params.EnableAutoDepthStencil = true;
            //Params.AutoDepthStencilFormat = Format.D16;
            //Params.PresentFlags = PresentFlags.DiscardDepthStencil;
            //Params.PresentationInterval = PresentInterval.Default;
            //Params.Windowed = true;

            //Device = new Device(new Direct3D(), 0, DeviceType.Hardware, Form.Handle, CreateFlags.HardwareVertexProcessing, Params);

            Device = D3D.Device;
        }

        //private void Application_Idle(object sender, EventArgs e)
        //{
        //    while (NativeMethods.AppStillIdle)
        //    {
        //        if (Running)
        //          Tick();
        //    }
        //}

        /*/
        private void Window_Deactivate(object sender, EventArgs e)
        {
            Active = false;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Active = true;
        }

        private void Window_ResizeEnd(object sender, EventArgs e)
        {
            Params.BackBufferWidth = Form.ClientSize.Width;
            Params.BackBufferHeight = Form.ClientSize.Height;

            //DeviceReset();

            //Device.Reset(Params);
            //D3D.Device.Reset(Params);
        }
         */

        private void Tick()
        {
            //Clock.Reset();
            //Clock.Start();
           
            //Update(Time);
            Update();

            //Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, 0, 1.0f, 0);
            //Device.BeginScene();

            //Draw(Time);
            Draw();

            //Device.EndScene();
            //Device.Present();

            //Clock.Stop();

            //Time.Update((float)Clock.Elapsed.TotalMilliseconds);
            
            //ElapsedMilliseconds = Time.ElapsedMilliseconds;
            //ElapsedSeconds = Time.ElapsedSeconds;
        }

        private void Dispose(bool disposed)
        {
            Unload();
        }

        #endregion

        #region virtual methods

        protected virtual void Initialize() { }

        protected virtual void DeviceReset() { }

        protected virtual void Load()
        {
            foreach (GameComponent component in Components)
                component.Load();
        }

        protected virtual void Unload()
        {
            foreach (GameComponent component in Components)
                component.Unload();
        }

        /*/
        protected virtual void Update(GameTime time)
        {
            foreach (GameComponent component in Components)
                component.Update(time);
        }

        protected virtual void Draw(GameTime time)
        {
            foreach (GameComponent component in Components)
            {
                if (component is DrawableGameComponent)
                {
                    ((DrawableGameComponent)component).Draw(time);
                }
            }
        }
        /*/

        protected virtual void Update()
        {
            foreach (GameComponent component in Components)
                component.Update();
        }

        protected virtual void Draw()
        {
            foreach (GameComponent component in Components)
            {
                if (component is DrawableGameComponent)
                {
                    ((DrawableGameComponent)component).Draw();
                }
            }
        }

        #endregion
    }
}
