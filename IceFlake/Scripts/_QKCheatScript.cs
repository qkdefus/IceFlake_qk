using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Scripts;
using System.Threading;
//using WhiteMagic.Internals;
//using GreyMagic.Internals;


namespace IceFlake.Scripts
{
    public class QKCheatScript : Script
    {
        public QKCheatScript()
            : base("QK", "_QKCheatScript")
        { }

        public bool SpeedHackActive = false;

        public WhiteMagic.Magic _magic = new WhiteMagic.Magic();

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            //Print("-- SpeedHack Enabled : " + SpeedHackActive);
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


            var unit = Manager.LocalPlayer.Target as WoWUnit;
            Print("\t xx Classification : " + unit.Classification);






            //foreach (var item in Manager.LocalPlayer.InventoryItems)
            //{
            //    Print("\t IsUsable : " + item.IsUsable);
            //}




        }
    }
}
