using System.Drawing;
using System.Linq;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Scripts;
using IceFlake.DirectX;
//using IceFlake.D3D;
using SlimDX;

namespace IceFlake.Scripts
{
    public class QKDrawingScript : Script
    {
        public QKDrawingScript()
            : base("Drawing", "_QKDrawingScript")
        {
        }

        private const float CIRCLE_RADIUS = 3f;
        private Location CIRCLE_LOC = new Location();

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;
        }

        public override void OnTick()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            //var greenCircle = Rendering.DrawCircle(CIRCLE_LOC, CIRCLE_RADIUS, Color.FromArgb(0x8f, 0, 0xff, 0), Color.FromArgb(0x8f, 0, 0xff, 0));
            //var redCircle = new Circle(CIRCLE_RADIUS, Color.FromArgb(0x8f, 0xff, 0, 0), Color.FromArgb(0x8f, 0xff, 0, 0));
            //var blueCircle = new Circle(CIRCLE_RADIUS, Color.Black, Color.FromArgb(0x8f, 0, 0, 0xff), isFilled: false);

           // Manager.ObjectManager.Objects.Where(x => x.IsPlayer).Cast<WoWPlayer>())
            var units = Manager.ObjectManager.Objects.Where(x => x.IsValid && (x.IsUnit || x.IsPlayer)).OfType<WoWUnit>();
            foreach (var u in units)
            {
                //if (u.IsNeutral || u.IsHostile)
                    //redCircle.Add(u.Location.ToVector3());
                //else
                    //greenCircle.Add(u.Location.ToVector3());

                //blueCircle.Add(u.Location.ToVector3());

                //Rendering.DrawCircle(u.Location, CIRCLE_RADIUS, Color.FromArgb(0x8f, 0, 0xff, 0), Color.FromArgb(0x8f, 0, 0xff, 0));


                var color = (u.InLoS ? Color.FromArgb(0x8f, 0, 0xff, 0) : Color.FromArgb(0x8f, 0xff, 0, 0));
                //var line = new Line();
                //line.Add(Manager.LocalPlayer.Location, color);
                //line.Add(u.Location, color);
                //Rendering.RegisterResource(line);
            }

            //Rendering.RegisterResource(redCircle);
            //Rendering.RegisterResource(greenCircle);
            //Rendering.RegisterResource(blueCircle);
        }

        public override void OnTerminate()
        {
        }
    }
}
