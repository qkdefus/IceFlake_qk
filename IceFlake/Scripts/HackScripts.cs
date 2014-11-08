using System;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;
using System.Linq;

namespace IceFlake.Scripts
{
    #region SpeedHackScript

    public class SpeedHackScriptV2 : Script
    {
        public SpeedHackScriptV2()
            : base("Speed HackV2", "Hack")
        { }

        private readonly IntPtr POINTER = new IntPtr(0x6F14A8); 
        private const uint START_SPEED = 0x9000E6C1;
        private const uint SPEED_MODIFIER = 3; // Max 5

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Manager.Memory.WriteBytes(POINTER, BitConverter.GetBytes(START_SPEED));
            Manager.Memory.WriteBytes(POINTER, BitConverter.GetBytes(START_SPEED + ((0x10000 * SPEED_MODIFIER) + 0x1000)));
            Print("Applying speed hack");
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            Manager.Memory.WriteBytes(POINTER, BitConverter.GetBytes(START_SPEED));
            Print("Removing speed hack");
        }
    }

    #endregion

    #region FlyHackScript // qk

    public class FlyHackScriptV2 : Script
    {
        public FlyHackScriptV2()
            : base("Fly HackV2", "Hack")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Manager.Memory.WriteBytes(new IntPtr(0x75ede5), BitConverter.GetBytes(0xeb022000));
            Manager.Memory.WriteBytes(new IntPtr(0x987e67), BitConverter.GetBytes(0x90022000));
            Manager.Memory.WriteBytes(new IntPtr(0x5fbff3), BitConverter.GetBytes(0x96e902));
            Print("Applying fly hack");
        }

        public override void OnTick()
        {
        }

        public override void OnTerminate()
        {
            Manager.Memory.WriteBytes(new IntPtr(0x75ede5), BitConverter.GetBytes(0x75022000));
            Manager.Memory.WriteBytes(new IntPtr(0x987e67), BitConverter.GetBytes(0x74022000));
            Manager.Memory.WriteBytes(new IntPtr(0x5fbff3), BitConverter.GetBytes(0x95850f02));
            Print("Removing fly hack");
        }
    }

    #endregion

    #region ClickToTeleportScript // qk

    public class ClickToTeleportScriptV2 : Script
    {
        public ClickToTeleportScriptV2()
            : base("Click To TeleportV2", "Hack")
        { }

        private float mouseX, mouseY, mouseZ;
        private Location _loc = new Location();

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;
        }

        public override void OnTick()
        {
            uint CtmState = Manager.Memory.Read<uint>((IntPtr)Pointers.CTM.State);
            if (CtmState != 13)
            {
                mouseX = Manager.Memory.Read<float>((IntPtr)Pointers.CTM.posX);
                mouseY = Manager.Memory.Read<float>((IntPtr)Pointers.CTM.posY);
                mouseZ = Manager.Memory.Read<float>((IntPtr)Pointers.CTM.posZ);

                Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Pointer + (uint)Pointers.PositionPointers.UNIT_X), mouseX);
                Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Pointer + (uint)Pointers.PositionPointers.UNIT_Y), mouseY);
                Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Pointer + (uint)Pointers.PositionPointers.UNIT_Z), mouseZ + 0.05f);
            }
        }

        public override void OnTerminate()
        {

        }
    }

    #endregion
}
