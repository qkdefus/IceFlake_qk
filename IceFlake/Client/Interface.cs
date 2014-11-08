using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using IceFlake.Client.Patchables;
using IceFlake.DirectX;
using SlimDX;

namespace IceFlake.Client
{
    public unsafe class Interface
    {
        public static void SetClickToMove(bool Value)
        {
            if (Value)
            {
                Manager.Memory.Write<uint>((IntPtr)Manager.Memory.Read<uint>((IntPtr)Pointers.ToggleCtm.Pointer) + 0x30, 1);
            }
            else
            {
                Manager.Memory.Write<uint>((IntPtr)Manager.Memory.Read<uint>((IntPtr)Pointers.ToggleCtm.Pointer) + 0x30, 0);
            }
        }

        public static void SetAutoLoot(bool Value)
        {
            if (Value)
            {
                Manager.Memory.Write<uint>((IntPtr)Manager.Memory.Read<uint>((IntPtr)Pointers.ToggleAutoLoot.Pointer) + 0x30, 1);
            }
            else
            {
                Manager.Memory.Write<uint>((IntPtr)Manager.Memory.Read<uint>((IntPtr)Pointers.ToggleAutoLoot.Pointer) + 0x30, 0);
            }
        }

        public static void SetAutoSelfCast(bool Value)
        {
            if (Value)
            {
                Manager.Memory.Write<uint>((IntPtr)Manager.Memory.Read<uint>((IntPtr)Pointers.ToggleSelfCast.Pointer) + 0x30, 1);
            }
            else
            {
                Manager.Memory.Write<uint>((IntPtr)Manager.Memory.Read<uint>((IntPtr)Pointers.ToggleSelfCast.Pointer) + 0x30, 0);
            }
        }

        public static void SetRenderTerrain(bool Value)
        {
            if (Value)
            {
                Manager.Memory.Write<uint>((IntPtr)Pointers.ToggleRenderTerrain.Pointer, (uint)Pointers.ToggleRenderTerrain.Default_Value);
                Manager.Memory.Write<int>((IntPtr)Pointers.ToggleRenderTerrain.Offset, 100);
            }
            else
            {
                Manager.Memory.Write<uint>((IntPtr)Pointers.ToggleRenderTerrain.Pointer, 0);
                Manager.Memory.Write<int>((IntPtr)Pointers.ToggleRenderTerrain.Offset, 20);
            }
        }
    }
}