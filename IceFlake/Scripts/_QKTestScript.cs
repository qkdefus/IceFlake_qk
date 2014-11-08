using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Scripts;
using IceFlake.Client.Patchables;
using System.Threading;
//using WhiteMagic.Internals;
//using GreyMagic.Internals;


namespace IceFlake.Scripts
{
    public class QKTestScript : Script
    {
        public QKTestScript()
            : base("QK", "_QKTestScript")
        { }

        public bool SpeedHackActive = false;

        //public WhiteMagic.Magic _magic = new WhiteMagic.Magic();



        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CMovementDelegate(IntPtr ptr, float x, float y, float z);

        public static uint CMovementOffset = 0x0075F0A0;
        public static CMovementDelegate CMov;


        public static uint MovementInitOffset = 0x00401520;
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MovementInitDelegate();
        public static MovementInitDelegate CMovementInit;



        public static uint InitializeGameOffset = 0x0052B550;  // qk
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void InitializeGameDelegate();
        public static InitializeGameDelegate CInitializeGame;




        public static uint aShowingHelm_Offset = 0x009FD944; // qk
        public static uint Lua_ShowingHelmOffset = 0x0051BFD0; // qk
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Lua_ShowingHelmDelegate(uint ptr);
        public static Lua_ShowingHelmDelegate CLua_ShowingHelm;








        public static uint Lua_TurnLeftStartOffset = 0x005FC320;  // qk
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Lua_TurnLeftStartDelegate();
        public static Lua_TurnLeftStartDelegate CLua_TurnLeftStart;

        public static uint Lua_TurnLeftStopOffset = 0x005FC360; // qk
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Lua_TurnLeftStopDelegate();
        public static Lua_TurnLeftStopDelegate CLua_TurnLeftStop;




        // CInputControl = 0x00C24954
        // CInputControl__SetPlayerPitch = 0x005FB3A0
        // CGUnit_C__HandleTrackingFacing2 = 0x00731600






        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void HandleTrackingFacing2_Delegate(IntPtr ptr, float rotation, float pitch);
        //public static HandleTrackingFacing2_Delegate _HandleTrackingFacing2;



        //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        //private delegate int HandleTrackingFacing2(int unk0, float unk1);
        //private HandleTrackingFacing2 _handleTF;



        public static uint HandleTrackingFacing2_Offset = 0x00731600; // qk
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int HandleTracking2FacingDelegate(uint thisObj, uint a2, float a3); 
        private HandleTracking2FacingDelegate _handleTrackingFacing2;


        public static uint CInputControl = 0x00C24954; // qk
        public static uint SetPlayerPitch_Offset = 0x005FB3A0; // qk
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int SetPlayerPitchDelegate(uint ptr, uint a2, uint a3, float a4); 
        private SetPlayerPitchDelegate _setPlayerPitch;







        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;




























            return;








            // Facing
            _handleTrackingFacing2 = Manager.Memory.RegisterDelegate<HandleTracking2FacingDelegate>((IntPtr)HandleTrackingFacing2_Offset);
            _handleTrackingFacing2((uint)Manager.LocalPlayer.Pointer, (uint)Helper.PerformanceCount, (Manager.LocalPlayer.R + 0.5f));

            // Pitch
            uint _CInputControl = Manager.Memory.Read<uint>((IntPtr)CInputControl);
            _setPlayerPitch = Manager.Memory.RegisterDelegate<SetPlayerPitchDelegate>((IntPtr)SetPlayerPitch_Offset);
            _setPlayerPitch((uint)_CInputControl, (uint)Manager.LocalPlayer.Pointer, (uint)Helper.PerformanceCount, (Manager.LocalPlayer.P + 0.5f));
            return;




            Print("\t X : " + Manager.LocalPlayer.Location.X);
            Print("\t Y : " + Manager.LocalPlayer.Location.Y);
            Print("\t Z : " + Manager.LocalPlayer.Location.Z);
            Print("\t R : " + Manager.LocalPlayer.R);
            Print("\t P : " + Manager.LocalPlayer.P);


            Print("\t Pointer : " + Manager.LocalPlayer.Pointer.ToString("X"));
            Print("\t CTM.Base : " + Pointers.CTM.Base.ToString("X"));

            return;
































            float _R = Manager.LocalPlayer.R + 1f;
            //float _Rtmp = _R + 1f;



            //float _P = Manager.LocalPlayer.P + 1f;

            Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Pointer + (uint)Pointers.PositionPointers.UNIT_R), _R);
            //Manager.Memory.Write<float>(new IntPtr((uint)Manager.LocalPlayer.Pointer + (uint)Pointers.PositionPointers.UNIT_P), _P);
            //CLua_TurnLeftStart = Manager.Memory.RegisterDelegate<Lua_TurnLeftStartDelegate>((IntPtr)Lua_TurnLeftStartOffset);

            //while (Manager.LocalPlayer.R != _Rtmp)
            //{
            //CLua_TurnLeftStart();

            return;

            //}
                



            //CLua_TurnLeftStop = Manager.Memory.RegisterDelegate<Lua_TurnLeftStopDelegate>((IntPtr)Lua_TurnLeftStopOffset);
            //CLua_TurnLeftStop();



            //return;















            //// face ctm
            //var _testObj = Manager.LocalPlayer.Target as WoWUnit;
            //Manager.LocalPlayer.FaceCtm(_testObj);

            //return;



            //CLua_TurnLeftStart = Manager.Memory.RegisterDelegate<Lua_TurnLeftStartDelegate>((IntPtr)Lua_TurnLeftStartOffset);
            //CLua_TurnLeftStart();
            //return;






            Location _testLoc = new Location();
            _testLoc.X = Manager.LocalPlayer.Target.Location.X;
            _testLoc.Y = Manager.LocalPlayer.Target.Location.Y;
            _testLoc.Z = Manager.LocalPlayer.Target.Location.Z;
            Manager.LocalPlayer.LookAt(_testLoc);

            return;




            //CLua_CameraOrSelectOrMoveStop = Manager.Memory.RegisterDelegate<Lua_CameraOrSelectOrMoveStopDelegate>((IntPtr)Lua_CameraOrSelectOrMoveStopOffset);
            //CLua_CameraOrSelectOrMoveStop(1);
            //return;


















            CInitializeGame = Manager.Memory.RegisterDelegate<InitializeGameDelegate>((IntPtr)InitializeGameOffset);
            CInitializeGame();
            return;

            CMovementInit = Manager.Memory.RegisterDelegate<MovementInitDelegate>((IntPtr)MovementInitOffset);
            CMovementInit();
            return;

            CMov = Manager.Memory.RegisterDelegate<CMovementDelegate>((IntPtr)CMovementOffset);
            CMov(Manager.LocalPlayer.Pointer, Manager.LocalPlayer.Location.X, Manager.LocalPlayer.Location.Y, Manager.LocalPlayer.Location.Z);
            return;




            var unit = Manager.LocalPlayer.Target as WoWUnit;
            Print("\t Target : " + unit.Name);
            Print("\t Class : " + unit.Class);
            Print("\t Classification : " + unit.Classification);
            Print("\t Classification_qk : " + unit.Classification_qk);
            Print("\t PowerType : " + unit.PowerType);
            Print("\t Power : " + unit.Power);
            Print("\t Runes : " + unit.Runes);
            Print("\t RunicPower : " + unit.RunicPower);
            Print("\t Energy : " + unit.Energy);



            Print("\t -");
            Print("\t IsFlying : " + unit.IsFlying);
            Print("\t IsFlyingCapable : " + unit.IsFlyingCapable);
            Print("\t -");


            return;










            foreach (Client.Chat.ChatMessageStruct ch in Client.Chat.WoWChat)
            {
                Print("Player : " + ch.Player);
                Print("Message : " + ch.Message);
                Print("Channel : " + ch.Channel);
                //Print("FormattedMsg : " + ch.FormattedMsg);
                Print("Type : " + ch.Type);
                Print("--");
            }
            Print("Chat Dump End");

            return;







            //var unit2 = Manager.LocalPlayer.Target as WoWUnit;




            //foreach (var item in Manager.LocalPlayer.InventoryItems)
            //{
            //    Print("\t IsUsable : " + item.IsUsable);
            //}



            Interface.SetRenderTerrain(true);
            Interface.SetAutoLoot(true);
            Interface.SetClickToMove(true);
            Interface.SetAutoSelfCast(true);

            return;

            //Print("-- SpeedHack Eabled : " + SpeedHackActive);
            //Print("\t x5");

            //Manager.Memory.WriteBytes(
            //Manager.Memory.Write<uint>(0x6f14a8, 0x9000e6c1);
            //Manager.Memory.Write<uint>(Manager.Memory.GetAbsolute((IntPtr)0x6f14a8), 0x9002f6c1);
            //Timestamp = Manager.Memory.Read<int>(new IntPtr(BaseMsg + 0x17c0))
            //Manager.Memory.Write<uint>(Manager.Memory.GetAbsolute((IntPtr)0x6f14a8), 0x9002f6c1);
            //Memory.m.Write<uint>((uint)0x6f14a8, 0x9000e6c1);
            //Manager.Memory.Write<uint>((IntPtr)0x6f14a8, 2416113345);
            //Manager.Memory.w<uint>((IntPtr)0x6f14a8, 2416113345);

            // 1x = 0x9000e6c1 // 2415978177
            // 2x = 0x9001f6c1 // 2416047809
            // 5x = 0x9002f6c1 // 2416113345

            //Print("\t " + 0x9002f6c1);
            //Print("\t " + (uint)0x9002f6c1);
            //Print("\t " + (UInt32)0x9002f6c1);
            //Manager.Memory.Write<uint>((IntPtr)0x6f14a8, 2416113345);
            //Manager.Memory.Write<uint>((IntPtr)0x6f14a8, 0x9002f6c1);
            //int ix = Manager.Memory.Read<int>((IntPtr)0x6f14a8);
            //Manager.Memory.Write<int>(new IntPtr(0x6f14a8), ix);
            //Manager.Memory.Write<uint>((IntPtr)0x6f14a8, 0x9002f6c1);
            //_magic.Write<uint>((IntPtr)0x6f14a8, 0x9002f6c1);
            //Print("\t xxx : " + Manager.Memory.Read<uint>((IntPtr)0x6f14a8));
            //Print("\t xxxx : " + Manager.Memory.Read<uint>(Manager.Memory.GetAbsolute((IntPtr)0x2f14a8)));
            //Manager.Memory.Write<uint>((IntPtr)0x6f14a8, 2416113345);
            //Manager.Memory.Write<uint>(Manager.Memory.GetAbsolute((IntPtr)0x2f14a8), 2416113345);
            //Print("\t xxxx : " + Manager.Memory.Read<uint>((IntPtr)0x6f14a8));
            //Manager.Memory.WriteBytes((IntPtr)0x52b25f, new byte[] { 0x90, 0xe9 });
            //Print("\t AFK Disabled");





            //_magic.Write<byte[]>((uint)0x52b25f, new byte[] { 0x90, 0xe9 });
            //Print("\t AFK Disabled");


        }
    }
}
