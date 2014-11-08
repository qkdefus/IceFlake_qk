using System;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;
using System.Linq;

namespace IceFlake.Scripts
{
    #region SpeedHackScript

    public class _QKSpeedScript : Script
    {
        public _QKSpeedScript()
            : base("QK", "_QKSpeedScript")
        { }

        private readonly IntPtr POINTER = new IntPtr(0x6F14A8);
        private const uint DEFAULT_SPEED = 0x9000E6C1;
        private const uint MULTIPLIER = 4;

        private uint CurrentSpeed = 0;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;
            CurrentSpeed = Manager.Memory.Read<uint>(POINTER);
            Manager.Memory.WriteBytes(POINTER, BitConverter.GetBytes(DEFAULT_SPEED));
            Print("Speed x: x{0} ({1})", ((0x10000 * MULTIPLIER) + 0x1000), CurrentSpeed);


            CurrentSpeed = Manager.Memory.Read<uint>(POINTER);
            Manager.Memory.WriteBytes(POINTER, BitConverter.GetBytes(DEFAULT_SPEED + ((0x10000 * MULTIPLIER) + 0x1000)));
            Print("Speed x: x{0} ({1})", ((0x10000 * MULTIPLIER) + 0x1000), CurrentSpeed);

            return;

            CurrentSpeed = Manager.Memory.Read<uint>(POINTER);
            Print("Current Speed: x{0} ({1})", MULTIPLIER, CurrentSpeed);
        }

        public override void OnTick()
        {
            Print("Current Speed: x{0} ({1})", MULTIPLIER, CurrentSpeed);
            Sleep(500);
        }

        public override void OnTerminate()
        {
            Manager.Memory.WriteBytes(POINTER, BitConverter.GetBytes(DEFAULT_SPEED));
            CurrentSpeed = Manager.Memory.Read<uint>(POINTER);
            Print("Current Speed: x{0} ({1})", MULTIPLIER, CurrentSpeed);
        }
    }

    #endregion
}