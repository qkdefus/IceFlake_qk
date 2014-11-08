namespace IceFlake.Client.Patchables
{
    internal static class Pointers
    {
        #region Nested type: CVar

        internal class CVar
        {
            internal static uint LookupRegistered = 0x6CA80; // UPDATE
            internal static uint Set = 0x6BF50; // UPDATE
        }

        #endregion

        #region Nested type: Container

        // 3.3.5a: 12340
        internal class Container
        {
            internal static uint GetBagAtIndex = 0x005D6F20;
            internal static uint LootWindowOffset = 0x00BFA8D8;
        }

        #endregion

        #region Nested type: Drawing

        // 3.3.5a: 12340
        internal class Drawing
        {
            internal static uint WorldFrame = 0xB7436C; 
            internal static uint ActiveCamera = 0x7E20;
            //internal static uint RenderBackground = 0x2532E0; // CGWorldFrame__Render // RenderBackgroundDelegate 4.3.4.15595 // UPDATE 

            // 0x715380; // 0x7153C0; // 0x7153A0; // 0x7153C0; // 0x007F0870; // 0x681F60; // 0x682960; // 0x7F9EC0; // 0x007F9EC0; // 0x007F0870;// 0x007F9EC0;
            //internal static uint _break_RenderMiniMap = 0x007E5580; // qk // 3.3.5a: 12340
            //internal static uint _break_RenderText = 0x747AE0;
            //internal static uint RenderWeaponGlow = 0x77F030; // works kinda
            //internal static uint RenderBackground = 0x7153C0; // works better? // renders behind water and wmo
            //internal static uint RenderWater = 0x77F020;
            //internal static uint RenderBlur = 0x780620;
            //internal static uint RenderSky = 0x781610;


            // 0x4F8EA0 // CGWorldFrame__Render
            // 0x7F0870 // <-

            internal static uint RenderBackground = 0x7F0870; // 0x79FCC0;
        }

        // 3.3.5a: 12340_qk // not used for anything, you can get aspect ratio from camera struct
        internal class GameWindow
        {
            internal static uint AspectRatio = 0xBD07F8;
            internal static uint GameWindowSizeX = 0xBD07F0;
            internal static uint GameWindowSizeY = 0xBD07F4;
        }

        #endregion

        #region Nested type: Events

        internal class Events
        {
            internal static uint EventVictim = 0x004DDBD0;
        }

        #endregion

        #region Nested type: Item

        // 3.3.5a: 12340
        internal class Item
        {
            internal static uint UseItem = 0x00708C20;
            internal static uint CanUseItem = 0x006DC3F0;
            internal static uint InventoryType = 0x00707280;
        }

        #endregion

        #region Nested type: LocalPlayer

        // 3.3.5a: 12340
        internal class LocalPlayer
        {
            internal static uint ClickToMove = 0x00727400;
            internal static uint SetFacing = 0x0072EA50;
            internal static uint IsClickMoving = 0x00721F90;
            internal static uint StopCTM = 0x0072B3A0;
            internal static uint GetRuneReadyBySlot = 0x629940; // UPDATE
            internal static uint CorpsePosition = 0x0051F430;
            internal static uint ComboPoints = 0x00BD084D;
        }

        #endregion

        #region Nested type: LuaInterface

        // 3.3.5a: 12340
        internal class LuaInterface
        {
            internal static uint LuaState = 0x00D3F78C;
            internal static uint LuaLoadBuffer = 0x0084F860;
            internal static uint LuaPCall = 0x0084EC50;
            internal static uint LuaGetTop = 0x0084DBD0;
            internal static uint LuaSetTop = 0x0084DBF0;
            internal static uint LuaType = 0x0084DEB0;
            internal static uint LuaToNumber = 0x0084E030;
            internal static uint LuaToLString = 0x0084E0E0;
            internal static uint LuaToBoolean = 0x0084E0B0;
        };

        #endregion

        #region Nested type: Object

        // 3.3.5a: 12340
        internal class Object // VMT
        {
            internal static uint GetObjectName = 54;
            internal static uint GetObjectLocation = 12;
            internal static uint GetObjectFacing = 14;
            internal static uint Interact = 44;
            internal static uint SelectObject = 0x00524BF0;
        }

        //public enum VMTableIndex // 403
        //{
        //    GetWorldPosition = 11,
        //    GetPosition = 12,
        //    UpdateFacing = 13,
        //    GetFacing = 14,
        //    GetScale = 15,
        //    GetTransportGuid = 16,
        //    CopyMatrix = 18,
        //    GetQuestStatus = 21,
        //    GetModel = 24,
        //    Interact = 46,
        //    GetName = 57,
        //}

        #endregion

        #region Nested type: DBC

        // 3.3.5a: 12340
        internal class DBC
        {
            internal static uint RegisterBase = 0x006337D0;
            internal static uint GetRow = 0x004BB1C0;
            internal static uint GetLocalizedRow = 0x004CFD20;
        }

        #endregion

        #region Nested type: WDB

        // 3.3.5a: 12340
        internal class WDB
        {
            internal static uint DbWoWCache_GetInfoBlockById = 0x0067FA80;
            internal static uint DBItemCache_GetInfoBlockByID = 0x0067CA30;

            internal static uint ItemInfo = 0x00C5D828;
        }

        #endregion

        #region Nested type: ObjectManager

        // 3.3.5a: 12340
        internal class ObjectManager
        {
            internal static uint EnumVisibleObjects = 0x004D4B30;
            internal static uint GetObjectByGuid = 0x004D4DB0;
            internal static uint GetLocalPlayerGuid = 0x004D3790;
        }

        #endregion

        #region Nested type: Other

        internal class Other
        {
            internal static uint PerformanceCounter = 0x0086AE20; // UNCONFIRMED
            internal static uint LastHardwareAction = 0x00B499A4;
            internal static uint IsBobbing = 0xBC;
            internal static uint GameState = 0x00B6A9E0;
            internal static uint IsLoading = 0x00B6AA38;
            internal static uint RealmName = 0x00C79B9E;
            // return ( InstanceDifficulty* )0x00C4EC2C;
            internal static uint InstanceDifficulty = 0x00C4EC2C;
        }

        #endregion

        #region Nested type: Party

        // 3.3.5a: 12340
        internal class Party
        {
            internal static uint PartyArray = 0x00BD1948; // UNCONFIRMED: 0x00C4FCC8?
        }

        #endregion

        #region Nested type: Raid

        // 3.3.5a: 12340
        internal class Raid
        {
            //return ( int* )0x00C543E0;
            internal static uint RaidCount = 0x00C543E0;
            // return ( ulong** )0x00C54340;
            internal static uint RaidArray = 0x00C54340;
        }

        #endregion

        #region Nested type: Spell

        // 3.3.5a: 12340
        internal class Spell
        {
            internal static uint SpellCount = 0x00BE8D9C;
            internal static uint SpellBook = 0x00BE5D88;
            internal static uint CastSpell = 0x0080DA40;
            internal static uint GetSpellCooldown = 0x00807980;
            internal static uint GetSpellEffectRec = 0x953B0; // UPDATE

            internal static uint FirstActionBarSpellId = 0xC1E358;
        }

        #endregion

        #region Nested type: Unit

        // 3.3.5a: 12340
        internal class Unit
        {
            internal static uint FishChanneledCasting = 0xD70;
            internal static uint ChanneledCastingId = 0xC20;
            internal static uint CastingId = 0xc08;
            internal static uint UnitReaction = 0x007251C0;
            internal static uint HasAuraBySpellId = 0x007282A0;
            internal static uint GetAura = 0x00556E10;
            internal static uint GetAuraCount = 0x004F8850;
            internal static uint GetCreatureType = 0x0071F300;
            internal static uint GetCreatureRank = 0x00718A00;
            internal static uint ShapeshiftFormId = 0x0071AF70;
            internal static uint CalculateThreat = 0x007374C0;
        }

        #endregion

        #region Nested type: World

        // 3.3.5a: 12340
        internal class World
        {
            internal static uint CGWorldFrame__Intersect = 0x0077F310; // qk
            internal static uint Traceline = 0x007A3B70; // qk
            internal static uint CurrentMapId = 0x00AB63BC;
            internal static uint InternalMapName = 0x00CE06D0;
            internal static uint ZoneID = 0x00BD080C;
            internal static uint ZoneText = 0x00BD0780;
            internal static uint SubZoneText = 0x00BD0784;
        }

        #endregion

        // 3.3.5a: 12340 // qk
        internal enum PositionPointers
        {
            UNIT_X = 0x798,
            UNIT_Y = UNIT_X + 0x4,
            UNIT_Z = UNIT_X + 0x8,
            UNIT_R = UNIT_X + 0x10,
            UNIT_P = UNIT_X + 0x14,

            UNIT_SPEED = 0x814,
            UNIT_MAXSPEED = UNIT_SPEED + 0x8,

            MOVEMENT_FIELD = 0xd8,

            OBJECT_X = 0xe8,
            OBJECT_Y = OBJECT_X + 0x4,
            OBJECT_Z = OBJECT_X + 0x8, 
        }

        // 3.3.5a: 12340 // qk
        internal enum MouseClick
        {
            posX = 0x00CA1264,
            posY = posX + 4,
            posZ = posX + 8,
        }

        // 3.3.5a: 12340 // qk
        internal enum CTM
        {
            Base = 0xCA11E4,
            posX = Base + 0x80,
            posY = Base + 0x84,
            posZ = Base + 0x88,
            State = Base + 0x10,
        };

        // 3.3.5a: 12340 // qk
        internal enum ToggleAutoLoot
        {
            Pointer = 0xbd0914,
            Offset = 0x30
        }

        // 3.3.5a: 12340 // qk
        internal enum ToggleCtm
        {
            Pointer = 0xbd08f4,
            Offset = 0x30
        }

        // 3.3.5a: 12340 // qk
        internal enum ToggleSelfCast
        {
            Pointer = 0xbd0920,
            Offset = 0x30
        }

        // 3.3.5a: 12340 // qk
        internal enum ToggleRenderTerrain
        {
            Pointer = 0xcd774c,
            Offset = 0xc5df7c,
            Default_Value = 0x1f104f73,
        }
    }
}