using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;
using IceFlake.Client.Scripts;
using System.Linq;

namespace IceFlake.Scripts
{
    #region SpeedHackScript //qk (Protected Memory)

    public class SpeedHackScript : Script
    {
        public SpeedHackScript()
            : base("Speed Hack", "Hack")
        { }

        private readonly IntPtr POINTER = new IntPtr(0x6F14A8);
        private const uint START_SPEED = 0x9000E6C1;
        private const uint SPEED_MODIFIER = 3; // Max 5

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            // 006F14A8 - 85 C0                 - test eax,eax
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

    #region SpeedHackScript // qk NEW

    public class QKSpeedHackScript : Script
    {
        public QKSpeedHackScript()
            : base("Hack", "_QKSpeedHack")
        { }

        private float SPEED_MODIFIER = 200; // Default 7 // Max 20000++

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Print("Applying speed hack");
        }

        public override void OnTick()
        {
            Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.MovementField + 0x8c), SPEED_MODIFIER);
        }

        public override void OnTerminate()
        {
            Print("Removing speed hack");
        }
    }

    #endregion

    #region FlyHackScript // qk (Protected Memory)

    public class FlyHackScript : Script
    {
        public FlyHackScript()
            : base("Fly Hack", "Hack")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            // 0075EDDF <- change jne to je

            // 0075EDE5 - 00 20                 - add [eax],ah
            // 00987E67 - 00 20                 - add [eax],ah
            // 005FBFF3 - 02 0F                 - add cl,[edi]
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

    #region Collision_ADT // Terrain // qk (Protected Memory)

    public class ADTCollisionScript : Script
    {
        public ADTCollisionScript()
            : base("ADT Collision", "Hack")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            // 007D889B <- change jg to je
            // 007D889B - 0F8F 4A050000         - jg 007D8DEB
            // 007D889B - 0F84 4A050000         - je 007D8DEB




            Manager.Memory.WriteBytes(new IntPtr(0x7D889B), BitConverter.GetBytes(0x054a840f));
            Print("Applying ADTCollision hack");
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
            Manager.Memory.WriteBytes(new IntPtr(0x7D889B), BitConverter.GetBytes(0x054a8f0f));
            Print("Removing ADTCollision hack");
        }
    }

    #endregion

    #region ClickToTeleportScript // qk

    public class ClickToTeleportScript : Script
    {
        public ClickToTeleportScript()
            : base("Click To Teleport", "Hack")
        { }

        private float mouseX, mouseY, mouseZ;

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

    #region InfiniteJumpScript // qk (Protected Memory)

    public class InfiniteJumpScript : Script
    {
        public InfiniteJumpScript()
            : base("Infinite Jump", "Hack")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            // 0098842D - F7 C7 00180002        - test edi,2001800
            Manager.Memory.WriteBytes(new IntPtr(0x98842D), BitConverter.GetBytes(0x0000c7f7)); // F7 C7 00 18 // 0x1800c7f7
            Print("Applying InfiniteJump hack");
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
            Manager.Memory.WriteBytes(new IntPtr(0x98842D), BitConverter.GetBytes(0x1800c7f7)); // F7 C7 00 18 // 0x1800c7f7
            Print("Removing InfiniteJump hack");
        }
    }

    #endregion

    #region LuaPatchScript // qk (Protected Memory)

    public class LuaPatchScript : Script
    {
        public LuaPatchScript()
            : base("LuaPatch", "Hack")
        { }

        private byte[] Default_Bytes = null;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            // 005191E0 - FF 24 95 50925100     - jmp dword ptr [edx*4+00519250]
            Default_Bytes = BitConverter.GetBytes(Manager.Memory.Read<uint>((IntPtr)0x005191E0));
            Manager.Memory.WriteBytes(new IntPtr(0x005191E0), BitConverter.GetBytes(0x57EB)); // FF 24 95 50925100

            Print("Applying Lua Patch");
            Print("\tYour now able to execute protected lua i.e");
            Print("\t/run MoveForwardStart();");
            Print("\t/run MoveForwardStop();");

            Print("\t " + BitConverter.GetBytes(Manager.Memory.Read<uint>((IntPtr)0x005191E0)).ToString());
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
            Manager.Memory.WriteBytes(new IntPtr(0x005191E0), Default_Bytes); // FF 24 95 50925100    
            Print("Removing Lua Patch");
        }
    }

    #endregion

    #region LanguagePatchScript // qk (Protected Memory)

    public class LanguagePatchScript : Script
    {
        public LanguagePatchScript()
            : base("LanguagePatch", "Hack")
        { }

        //private byte[] Default_Bytes = null;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            //0050599F - 72 11                 - jb 005059B2
            //Default_Bytes = BitConverter.GetBytes(Manager.Memory.Read<uint>((IntPtr)0x0075E439));

            Manager.Memory.WriteBytes(new IntPtr(0x0050599F), BitConverter.GetBytes(0x558b9090));

            Print("Applying Language Patch");
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
            //0050599F - 72 11                 - jb 005059B2
            //Manager.Memory.WriteBytes(new IntPtr(0x0050599F), Default_Bytes);

            Manager.Memory.WriteBytes(new IntPtr(0x0050599F), BitConverter.GetBytes(0x558b1172));

            Print("Removing Language Patch");
        }
    }

    #endregion

    #region NoFallDamageScript // qk (Protected Memory)

    public class NoFallDamageScript : Script
    {
        public NoFallDamageScript()
            : base("NoFall Damage", "Hack")
        { }

        private byte[] Default_Bytes = null;
        private byte[] Default_Bytes2 = null;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            // 00987255 - 8B 87 80 00 00 00        - mov eax,[edi+00000080]
            Default_Bytes = BitConverter.GetBytes(Manager.Memory.Read<uint>((IntPtr)0x00987255)); // qk
            Manager.Memory.WriteBytes(new IntPtr(0x00987255), BitConverter.GetBytes(0x909090));


            Default_Bytes2 = BitConverter.GetBytes(Manager.Memory.Read<uint>((IntPtr)0x00987258)); // qk
            Manager.Memory.WriteBytes(new IntPtr(0x00987258), BitConverter.GetBytes(0x909090));

            Print("Applying NoFall Damage hack");
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
            Manager.Memory.WriteBytes(new IntPtr(0x00987258), Default_Bytes2);

            Manager.Memory.WriteBytes(new IntPtr(0x00987255), Default_Bytes);

            Print("Removing NoFall Damage hack");
        }
    }

    #endregion

    #region WallClimbScript // qk (Protected Memory)

    public class WallClimbScript : Script
    {
        public WallClimbScript()
            : base("Wallclimb", "Hack")
        { }

        private byte[] Default_Bytes = null;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Default_Bytes = BitConverter.GetBytes(Manager.Memory.Read<uint>((IntPtr)0x00A37F0C));

            //00A37F0C - BB 8D 24 3F D4
            Manager.Memory.WriteBytes(new IntPtr(0x00A37F0C), BitConverter.GetBytes(0x00000000));

            Print("Applying Wallclimb hack");
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
            //00A37F0C - BB 8D 24 3F D4
            //Manager.Memory.WriteBytes(new IntPtr(0x00A37F0C), BitConverter.GetBytes(0xD43f248dbb));

            Manager.Memory.WriteBytes(new IntPtr(0x00A37F0C), Default_Bytes);

            Print("Removing Wallclimb hack");
        }
    }

    #endregion

    #region WaterWalkScript // qk (Protected Memory)

    public class WaterWalkScript : Script
    {
        public WaterWalkScript()
            : base("WaterWalk", "Hack")
        { }

        private byte[] Default_Bytes = null;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            //0075E439 - 74 14                 - je 0075E44F
            //Default_Bytes = BitConverter.GetBytes(Manager.Memory.Read<uint>((IntPtr)0x0075E439));

            Manager.Memory.WriteBytes(new IntPtr(0x0075E439), BitConverter.GetBytes(0xCF811475));

            Print("Applying WaterWalk hack");
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
            //0075E439 - 74 14                 - je 0075E44F
            //Manager.Memory.WriteBytes(new IntPtr(0x0075E439), Default_Bytes);

            Manager.Memory.WriteBytes(new IntPtr(0x0075E439), BitConverter.GetBytes(0xCF811474));

            Print("Removing WaterWalk hack");
        }
    }

    #endregion

    #region NoSwimScript // qk (Protected Memory)

    public class NoSwimScript : Script
    {
        public NoSwimScript()
            : base("NoSwim", "Hack")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            //00730D87 - 74 07                 - je 00730D90
            //74 07 C7 45

            Manager.Memory.WriteBytes(new IntPtr(0x00730D87), BitConverter.GetBytes(0x45c79090));
            Print("Applying NoSwim hack");
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
            Manager.Memory.WriteBytes(new IntPtr(0x00730D87), BitConverter.GetBytes(0x45c70774));
            Print("Removing NoSwim hack");
        }
    }

    #endregion

    #region MinimapUnitTrackingScript // qk

    public class MinimapUnitTrackingScript : Script
    {
        public MinimapUnitTrackingScript()
            : base("Tracking Unit's", "Hack")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Manager.Memory.Write<uint>(new IntPtr((uint)Manager.LocalPlayer.Pointer + 0x2950), 255);
            Print("Applying Unit Tracking hack");
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
            Manager.Memory.Write<uint>(new IntPtr((uint)Manager.LocalPlayer.Pointer + 0x2950), 0);
            Print("Removing Unit Tracking hack");
        }
    }

    #endregion

    #region MinimapNodeTrackingScript // qk

    public class MinimapNodeTrackingScript : Script
    {
        public MinimapNodeTrackingScript()
            : base("Tracking Node's", "Hack")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            Manager.Memory.Write<uint>(new IntPtr((uint)Manager.LocalPlayer.Pointer + 0x2954), 255);
            Print("Applying Node Tracking hack");
        }

        public override void OnTick()
        {

        }

        public override void OnTerminate()
        {
            Manager.Memory.Write<uint>(new IntPtr((uint)Manager.LocalPlayer.Pointer + 0x2954), 0);
            Print("Removing Node Tracking hack");
        }
    }

    #endregion

    #region MorphScaleScript // qk

    public class MorphScaleScript : Script
    {
        public MorphScaleScript()
            : base("Morph & Scale", "Hack")
        { }

        public static uint UpdateModel_Offset = 0x0073E410;
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UpdateModelDelegate(IntPtr ptr, int a2);
        private UpdateModelDelegate _UpdateModel;

        private const bool CHANGE_DISPLAYID = false;
        private const uint MORPH_DISPLAYID = 69;
        private uint DefaultDisplayID = 0;
        private uint _tmpDisplayID = 0;

        private const bool CHANGE_SCALE = false;
        private const float MORPH_SCALE = 1.0f;
        private float DefaultScale = 0f;
        private float _tmpScale = 0f;

        private Location _tmpLoc = new Location();
        private Location _localLoc = new Location();

        private ulong tmpGUID = 0;

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            DefaultDisplayID = Manager.LocalPlayer.GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_NATIVEDISPLAYID);
            DefaultScale = Manager.LocalPlayer.GetDescriptor<float>(WoWObjectFields.OBJECT_FIELD_SCALE_X);
        }


        private bool forceLocCheck = false;

        public override void OnTick()
        {
            if (Manager.LocalPlayer.TargetGuid != 0)
            {
                if (tmpGUID != Manager.LocalPlayer.Target.Guid)
                {
                    tmpGUID = Manager.LocalPlayer.Target.Guid;

                    _localLoc = Manager.LocalPlayer.Location;
                    _tmpLoc = Manager.LocalPlayer.Target.Location;

                    Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Pointer + (uint)Pointers.PositionPointers.UNIT_X), _tmpLoc.X);
                    Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Pointer + (uint)Pointers.PositionPointers.UNIT_Y), _tmpLoc.Y);
                    Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Pointer + (uint)Pointers.PositionPointers.UNIT_Z), _tmpLoc.Z + 0.05f);

                    //WoWScript.ExecuteNoResults("JumpOrAscendStart()");
                    //WoWScript.ExecuteNoResults("MoveForwardStart()");
                    //WoWScript.ExecuteNoResults("MoveForwardStop()");
                    //WoWScript.ExecuteNoResults("MoveForwardStop()");

                    //DisplayID
                    //_tmpDisplayID = Manager.LocalPlayer.Target.GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_NATIVEDISPLAYID);
                    Manager.LocalPlayer.SetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_DISPLAYID, _tmpDisplayID);
                    Print("Changing display ID from {0} to {1}", DefaultDisplayID, _tmpDisplayID);

                    // Scale // Crash due to getdescriptor unavailable to Target
                    //TmpScale = Manager.LocalPlayer.Target.GetDescriptor<uint>(0x0004);
                    //Manager.LocalPlayer.SetDescriptor<float>(WoWObjectFields.OBJECT_FIELD_SCALE_X, TmpScale);
                    //Print("Changing scale from {0:1} to {1:1}", DefaultScale, TmpScale);

                    //UpdateModel
                    _UpdateModel = Manager.Memory.RegisterDelegate<UpdateModelDelegate>((IntPtr)UpdateModel_Offset);
                    _UpdateModel(Manager.LocalPlayer.Pointer, 1);

                    //forceLocCheck = true;

                    Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Target.Pointer + (uint)Pointers.PositionPointers.UNIT_X), _localLoc.X);
                    Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Target.Pointer + (uint)Pointers.PositionPointers.UNIT_Y), _localLoc.Y);
                    Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Target.Pointer + (uint)Pointers.PositionPointers.UNIT_Z), _localLoc.Z + 0.05f);
                }


                
            }




        }

        public override void OnTerminate()
        {
            // DisplayID
            Manager.LocalPlayer.SetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_DISPLAYID, DefaultDisplayID);

            // Scale
            Manager.LocalPlayer.SetDescriptor<float>(WoWObjectFields.OBJECT_FIELD_SCALE_X, DefaultScale);

            // UpdateModel
            _UpdateModel = Manager.Memory.RegisterDelegate<UpdateModelDelegate>((IntPtr)UpdateModel_Offset);
            _UpdateModel(Manager.LocalPlayer.Pointer, 1);
        }
    }

    #endregion

}