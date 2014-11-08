using System;
using System.Runtime.InteropServices;
using IceFlake.Client.Collections;
using IceFlake.Client.Patchables;
using System.Windows.Forms;

namespace IceFlake.Client.Objects
{
    public class WoWUnit : WoWObject
    {

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int CreatureTypeDelegate(IntPtr thisObj);
        private static CreatureTypeDelegate _creatureType;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int CreatureRankDelegate(IntPtr thisObj);
        private static CreatureRankDelegate _creatureRank;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr GetAuraDelegate(IntPtr thisObj, int index);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int GetShapeshiftFormIdDelegate(IntPtr thisObj);
        private static GetShapeshiftFormIdDelegate _getShapeshiftFormId;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitReactionDelegate(IntPtr thisObj, IntPtr unitToCompare);
        private static UnitReactionDelegate _unitReaction;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitThreatInfoDelegate(
            IntPtr pThis, IntPtr guid, ref IntPtr threatStatus, ref IntPtr threatPct, ref IntPtr rawPct,
            ref int threatValue);
        private static UnitThreatInfoDelegate _unitThreatInfo;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int GetAuraCountDelegate(IntPtr thisObj);
        private static GetAuraCountDelegate _getAuraCount;
        private static GetAuraDelegate _getAura;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate bool HasAuraDelegate(IntPtr thisObj, int spellId);
        private static HasAuraDelegate _hasAura;

        private readonly AuraCollection _auras;

        public WoWUnit(IntPtr pointer)
            : base(pointer)
        {
            _auras = new AuraCollection(this);
        }

        public UnitReaction Reaction
        {
            get
            {
                if (_unitReaction == null)
                    _unitReaction =
                        Manager.Memory.RegisterDelegate<UnitReactionDelegate>((IntPtr)Pointers.Unit.UnitReaction);
                return (UnitReaction)_unitReaction(Pointer, Manager.LocalPlayer.Pointer);
            }
        }

        public bool IsFriendly
        {
            get { return (int)Reaction > (int)UnitReaction.Neutral; }
        }

        public bool IsNeutral
        {
            get { return Reaction == UnitReaction.Neutral; }
        }

        public bool IsHostile
        {
            get { return (int)Reaction < (int)UnitReaction.Neutral; }
        }

        public ulong TargetGuid
        {
            get
            {
                try
                {
                    return GetDescriptor<ulong>(WoWUnitFields.UNIT_FIELD_TARGET);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public WoWObject Target
        {
            get { return Manager.ObjectManager.GetObjectByGuid(TargetGuid); }
        }

        public bool IsDead
        {
            get { return Health <= 0 || HasFlag(WoWUnitFields.UNIT_DYNAMIC_FLAGS, UnitDynamicFlags.Dead); }
        }

        private byte[] DisplayPower
        {
            get { return BitConverter.GetBytes(GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER1)); }
        }

        private byte[] UnitBytes0
        {
            get { return BitConverter.GetBytes(GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BYTES_0)); }
        }

        private byte[] UnitBytes1
        {
            get { return BitConverter.GetBytes(GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BYTES_1)); }
        }

        private byte[] UnitBytes2
        {
            get { return BitConverter.GetBytes(GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BYTES_2)); }
        }

        public WoWRace Race
        {
            get { return (WoWRace)UnitBytes0[0]; }
        }

        public WoWClass Class
        {
            get { return (WoWClass)UnitBytes0[1]; }
        }

        public bool IsLootable
        {
            get { return HasFlag(WoWUnitFields.UNIT_DYNAMIC_FLAGS, UnitDynamicFlags.Lootable); }
        }

        public bool IsTapped
        {
            get { return HasFlag(WoWUnitFields.UNIT_DYNAMIC_FLAGS, UnitDynamicFlags.TaggedByOther); }
        }

        public bool IsTappedByMe
        {
            get { return HasFlag(WoWUnitFields.UNIT_DYNAMIC_FLAGS, UnitDynamicFlags.TaggedByMe); }
        }

        public bool IsInCombat
        {
            get { return HasFlag(WoWUnitFields.UNIT_FIELD_FLAGS, UnitFlags.Combat); }
        }

        public bool IsLooting
        {
            get { return HasFlag(WoWUnitFields.UNIT_FIELD_FLAGS, UnitFlags.Looting); }
        }

        public uint Health
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_HEALTH); }
        }

        public uint MaxHealth
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXHEALTH); }
        }

        public double HealthPercentage
        {
            get { return (Health / (double)MaxHealth) * 100; }
        }

        public double PowerPercentage
        {
            get { return (Power / (double)MaxPower) * 100; }
        }

        public uint Level
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_LEVEL); }
        }

        public UnitFlags Flags
        {
            get { return (UnitFlags)GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_FLAGS); }
        }

        public UnitFlags2 Flags2
        {
            get { return GetDescriptor<UnitFlags2>(WoWUnitFields.UNIT_FIELD_FLAGS_2); }
        }

        public UnitDynamicFlags DynamicFlags
        {
            get { return GetDescriptor<UnitDynamicFlags>(WoWUnitFields.UNIT_DYNAMIC_FLAGS); }
        }

        public UnitNPCFlags NpcFlags
        {
            get { return GetDescriptor<UnitNPCFlags>(WoWUnitFields.UNIT_NPC_FLAGS); }
        }

        public bool IsVendor
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_VENDOR); }
        }

        public bool IsRepairer
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_REPAIR); }
        }

        public bool IsClassTrainer
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_TRAINER_CLASS); }
        }

        public bool IsProfessionTrainer
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_TRAINER_PROFESSION); }
        }

        public bool IsFlightmaster
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_FLIGHTMASTER); }
        }

        public bool IsInnkeeper
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_INNKEEPER); }
        }

        public bool IsAuctioneer
        {
            get { return HasFlag(WoWUnitFields.UNIT_NPC_FLAGS, UnitNPCFlags.UNIT_NPC_FLAG_AUCTIONEER); }
        }

        public uint Faction
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_FACTIONTEMPLATE); }
        }

        public uint BaseAttackTime
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BASEATTACKTIME); }
        }

        public uint RangedAttackTime
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_RANGEDATTACKTIME); }
        }

        public float BoundingRadius
        {
            get { return GetDescriptor<float>(WoWUnitFields.UNIT_FIELD_BOUNDINGRADIUS); }
        }

        public float CombatReach
        {
            get { return GetDescriptor<float>(WoWUnitFields.UNIT_FIELD_COMBATREACH); }
        }

        public uint DisplayId
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_DISPLAYID); }
        }

        public uint MountDisplayId
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MOUNTDISPLAYID); }
        }

        public bool IsMounted // qk
        {
            get { return MountDisplayId != 0; }
        }

        public uint NativeDisplayId
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_NATIVEDISPLAYID); }
        }

        public uint MinDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MINDAMAGE); }
        }

        public uint MaxDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXDAMAGE); }
        }

        public uint MinOffhandDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MINOFFHANDDAMAGE); }
        }

        public uint MaxOffhandDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXOFFHANDDAMAGE); }
        }

        public uint PetExperience
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_PETEXPERIENCE); }
        }

        public uint PetNextLevelExperience
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_PETNEXTLEVELEXP); }
        }

        public uint BaseMana
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BASE_MANA); }
        }

        public uint BaseHealth
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_BASE_HEALTH); }
        }

        public uint AttackPower
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_ATTACK_POWER); }
        }

        public uint RangedAttackPower
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_RANGED_ATTACK_POWER); }
        }

        public uint MinRangedDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MINRANGEDDAMAGE); }
        }

        public uint MaxRangedDamage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXRANGEDDAMAGE); }
        }

        public uint Power
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER1); }
        }

        public uint MaxPower
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER1); }
        }

        public ulong SummonedBy
        {
            get { return GetDescriptor<ulong>(WoWUnitFields.UNIT_FIELD_SUMMONEDBY); }
        }

        public ulong CreatedBy
        {
            get { return GetDescriptor<ulong>(WoWUnitFields.UNIT_FIELD_CREATEDBY); }
        }

        public uint ChanneledCastingId
        {
            get { return Manager.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + Pointers.Unit.ChanneledCastingId)); }
        }

        public uint CastingId
        {
            get { return Manager.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + Pointers.Unit.CastingId)); }
        }

        public bool IsCasting
        {
            get { return (ChanneledCastingId != 0 || CastingId != 0); }
        }

        public int GetAuraCount
        {
            get
            {
                if (_getAuraCount == null)
                    _getAuraCount =
                        Manager.Memory.RegisterDelegate<GetAuraCountDelegate>((IntPtr)Pointers.Unit.GetAuraCount);
                return _getAuraCount(Pointer);
            }
        }

        public AuraCollection Auras
        {
            get
            {
                _auras.Update();
                return _auras;
            }
        }

        public UnitClassificationType Classification
        {
            get
            {
                if (_creatureRank == null)
                    _creatureRank =
                        Manager.Memory.RegisterDelegate<CreatureRankDelegate>((IntPtr)Pointers.Unit.GetCreatureRank);
                return (UnitClassificationType)_creatureRank(Pointer);
            }
        }

        public CreatureType CreatureType
        {
            get
            {
                if (_creatureType == null)
                    _creatureType =
                        Manager.Memory.RegisterDelegate<CreatureTypeDelegate>((IntPtr)Pointers.Unit.GetCreatureType);
                return (CreatureType)_creatureType(Pointer);
            }
        }

        public ShapeshiftForm Shapeshift
        {
            get
            {
                if (_getShapeshiftFormId == null)
                    _getShapeshiftFormId =
                        Manager.Memory.RegisterDelegate<GetShapeshiftFormIdDelegate>(
                            (IntPtr)Pointers.Unit.ShapeshiftFormId);
                return (ShapeshiftForm)_getShapeshiftFormId(Pointer);
            }
        }

        public bool IsStealthed
        {
            get { return Shapeshift == ShapeshiftForm.Stealth; }
        }

        public bool IsTotem
        {
            get { return CreatureType == CreatureType.Totem; }
        }

        public int CalculateThreat
        {
            get
            {
                if (_unitThreatInfo == null)
                    _unitThreatInfo =
                        Manager.Memory.RegisterDelegate<UnitThreatInfoDelegate>((IntPtr)Pointers.Unit.CalculateThreat);

                var threatStatus = new IntPtr();
                var threatPct = new IntPtr();
                var threatRawPct = new IntPtr();
                int threatValue = 0;
                var storageField = Manager.Memory.Read<IntPtr>(Manager.LocalPlayer.Pointer + 0x08);
                _unitThreatInfo(Pointer, storageField, ref threatStatus, ref threatPct, ref threatRawPct,
                                ref threatValue);

                return (int)threatStatus;
            }
        }

        public bool HasAura(int spellId)
        {
            if (_hasAura == null)
                _hasAura = Manager.Memory.RegisterDelegate<HasAuraDelegate>((IntPtr)Pointers.Unit.HasAuraBySpellId);
            return _hasAura(Pointer, spellId);
        }

        public IntPtr GetAuraPointer(int index)
        {
            if (_getAura == null)
                _getAura = Manager.Memory.RegisterDelegate<GetAuraDelegate>((IntPtr)Pointers.Unit.GetAura);
            return _getAura(Pointer, index);
        }

        //public override string ToString()
        //{
        //    return "[\"" + Name + "\", Distance = " + (int)Distance + ", Type = " + Type + ", React = " + Reaction + "]";
        //}

        //public UnitClassificationType Classification_qk // qk
        //{
        //get { return (UnitClassificationType)Manager.Memory.Read<uint>((IntPtr)Manager.Memory.Read<uint>((IntPtr)this.Pointer + 0x964) + 0x18); }
        //}

        public UnitClassificationType Classification_qk // qk
        {
            get
            {
                UnitClassificationType num = UnitClassificationType.Normal;

                try
                {
                    return (UnitClassificationType)Manager.Memory.Read<uint>((IntPtr)Manager.Memory.Read<uint>((IntPtr)this.Pointer + 0x964) + 0x18);
                }
                catch { }

                return num;
            }
        }

        public PowerType PowerType // qk
        {
            get
            {
                return (PowerType)Manager.Memory.Read<byte>(((IntPtr)this.StorageField + 0x5c) + 0x3);
            }
        }

        public bool IsFlying // qk // working fine on players // not working on npc's in mango's emulator ?
        {
            get
            {
                uint num = Manager.Memory.Read<uint>((IntPtr)this.Pointer + 0xd8);
                return ((Manager.Memory.Read<uint>((IntPtr)num + 0x44) & 0x2000000) != 0);
            }
        }

        public bool IsFlyingCapable // qk  // working fine on players // not working on npc's in mango's emulator ?
        {
            get
            {
                uint num = Manager.Memory.Read<uint>((IntPtr)this.Pointer + 0xd8);
                return ((Manager.Memory.Read<uint>((IntPtr)num + 0x44) & 0x1000000) != 0);
            }
        }

        /// <summary>
        /// The unit's energy.
        /// </summary>
        public uint Energy
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER4); }
        }

        /// <summary>
        /// The unit's happiness.
        /// </summary>
        public uint Happiness
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER4); }
        }

        /// <summary>
        /// The unit's runic power.
        /// </summary>
        public uint RunicPower
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER5); }
        }

        /// <summary>
        /// The unit's runes.
        /// </summary>
        public uint Runes
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_POWER6); }
        }

        /// <summary>
        /// The unit's maximum mana.
        /// </summary>
        public uint MaximumMana
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER1); }
        }

        /// <summary>
        /// The unit's maximum rage.
        /// </summary>
        public uint MaximumRage
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER2); }
        }

        /// <summary>
        /// The unit's maximum focus.
        /// </summary>
        public uint MaximumFocus
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER3); }
        }

        /// <summary>
        /// The unit's maximum energy.
        /// </summary>
        public uint MaximumEnergy
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER4); }
        }

        /// <summary>
        /// The unit's maximum happiness.
        /// </summary>
        public uint MaximumHappiness
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER4); }
        }

        /// <summary>
        /// The unit's maximum runic power.
        /// </summary>
        public uint MaximumRunicPower
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER5); }
        }

        /// <summary>
        /// The unit's maximum runes.
        /// <!-- This is presumably always 6, two blood, two frost, two unholy. But may be different on DK based bosses/mobs? -->
        /// </summary>
        public uint MaximumRunes
        {
            get { return GetDescriptor<uint>(WoWUnitFields.UNIT_FIELD_MAXPOWER6); }
        }


        /*/
        /// <summary>
        /// Returns the object's X position.
        /// </summary>
        public virtual float X
        {
            get
            {
                try
                {
                    return Memory.BlackMagic.ReadFloat(BaseAddress + (uint)Pointers.PositionPointers.UNIT_X);
                }
                catch { }
                return 0f;
            }
        }

        /// <summary>
        /// Returns the object's Y position.
        /// </summary>
        public virtual float Y
        {
            get
            {
                try
                {
                    return Memory.BlackMagic.ReadFloat(BaseAddress + (uint)Pointers.PositionPointers.UNIT_Y);
                }
                catch { }
                return 0f;
            }
        }

        /// <summary>
        /// Returns the object's Z position.
        /// </summary>
        public virtual float Z
        {

            get
            {
                try
                {
                    return Memory.BlackMagic.ReadFloat(BaseAddress + (uint)Pointers.PositionPointers.UNIT_Z);
                }
                catch { }
                return 0f;
            }
        }
        /*/

        /// <summary>
        /// Returns the object's Rotation.
        /// </summary>
        public virtual float R
        {
            get
            {
                try
                {
                    return Manager.Memory.Read<float>(new IntPtr((uint)Pointer + (uint)Pointers.PositionPointers.UNIT_R));
                }
                catch { }
                return 0f;
            }
        }

        /// <summary>
        /// Returns the object's Pitch.
        /// </summary>
        /// 
        public virtual float P
        {
            get
            {
                try
                {
                    return Manager.Memory.Read<float>(new IntPtr((uint)Pointer + (uint)Pointers.PositionPointers.UNIT_P));
                }
                catch { }
                return 0f;
            }
        }

        /// <summary>
        /// Return Unit MovementField
        /// </summary>
        public uint MovementField
        {
            get
            {
                try
                {
                    return Manager.Memory.Read<uint>(new IntPtr((uint)Pointer + (uint)Pointers.PositionPointers.MOVEMENT_FIELD));
                }
                catch { }
                return 0;
            }
        }

        /// <summary>
        /// Return Unit Speed
        /// </summary>
        public float Speed
        {
            get
            {
                try
                {
                    return Manager.Memory.Read<float>(new IntPtr((uint)Pointer + (uint)Pointers.PositionPointers.UNIT_SPEED));
                }
                catch { }
                return 0f;
            }
        }

        /// <summary>
        /// Returns True if unit is moving, else False. by reading UNIT_SPEED
        /// </summary>
        public bool IsMoving
        {
            get
            {
                return (Speed > 0f);
            }
        }

        /*/

        /// <summary>
        /// Return Unit Max Speed
        /// </summary>
        public float Speed_Max
        {
            get
            {
                return Memory.BlackMagic.ReadFloat((uint)BaseAddress + Pointers.PositionPointers.UNIT_MAXSPEED);
            }
        }

        /// <summary>
        /// Return Unit Speed // based on MovementField
        /// </summary>
        public float Speed_MovementField
        {
            get
            {
                return Memory.BlackMagic.ReadFloat(MovementField + 0x80); // 433
            }
        }

        /// <summary>
        /// Return Unit MovementField
        /// </summary>
        public uint MovementField
        {
            get
            {
                return Memory.BlackMagic.ReadUInt((uint)BaseAddress + Pointers.PositionPointers.MOVEMENT_FIELD);
            }
        }

        /// <summary>
        /// Returns True if unit is moving, else False. by reading movment field
        /// </summary>
        public bool IsMoving_MovementField
        {
            get
            {
                return (Speed_MovementField > 0f);
            }
        }

        /// <summary>
        /// Returns True if unit is moving, else False. by reading XYZ
        /// </summary>
        public bool IsMoving_XYZ
        {
            get
            {

                float FirstX = X;
                float FirstY = Y;
                float FirstZ = Z;

                System.Threading.Thread.Sleep(100);

                if ((FirstX == X) && (FirstY == Y) && (FirstZ == Z))
                    return false;
                else
                    return true;

            }
        }

        /// <summary>
        /// Returns True if unit is moving, else False. by reading XYZ
        /// </summary>
        public bool IsMoving_Position
        {
            get
            {
                try
                {
                    bool flag = false;
                    Location point = Manager.LocalPlayer.Location;
                    //Thread.Sleep(50);
                    if (((Math.Round((double)point.X, 1) != Math.Round((double)Manager.LocalPlayer.Location.X, 1)) || (Math.Round((double)point.Z, 1) != Math.Round((double)Manager.LocalPlayer.Location.Z, 1))) || (Math.Round((double)point.Y, 1) != Math.Round((double)Manager.LocalPlayer.Location.Y, 1)))
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        //Thread.Sleep(30);
                        if (((Math.Round((double)point.X, 1) != Math.Round((double)Manager.LocalPlayer.Location.X, 1)) || (Math.Round((double)point.Z, 1) != Math.Round((double)Manager.LocalPlayer.Location.Z, 1))) || (Math.Round((double)point.Y, 1) != Math.Round((double)Manager.LocalPlayer.Location.Y, 1)))
                        {
                            flag = true;
                        }
                    }
                    return flag;
                }
                catch (Exception ex)
                {
                    //Logging.WriteError("WoWUnit > GetMove: " + ex, true);
                    return true;
                }
            }
        }
         */


        internal T GetDescriptor<T>(Enum idx) where T : struct
        {
            return GetDescriptor<T>(Convert.ToInt32(idx));
        }

        internal T GetDescriptor<T>(int idx) where T : struct
        {
            return GetAbsoluteDescriptor<T>(idx * 0x4);
        }

        internal T GetAbsoluteDescriptor<T>(int offset) where T : struct
        {
            var descriptorArray = Manager.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + 0x8));
            return Manager.Memory.Read<T>(new IntPtr(descriptorArray + offset));
        }

        internal void SetDescriptor<T>(Enum idx, T value) where T : struct
        {
            SetDescriptor<T>(Convert.ToInt32(idx), value);
        }

        internal void SetDescriptor<T>(int idx, T value) where T : struct
        {
            SetAbsoluteDescriptor<T>(idx * 0x4, value);
        }

        internal void SetAbsoluteDescriptor<T>(int offset, T value) where T : struct
        {
            var descriptorArray = Manager.Memory.Read<uint>(new IntPtr(Pointer.ToInt64() + 0x8));
            Manager.Memory.Write<T>(new IntPtr(descriptorArray + offset), value);
        }
    }
}