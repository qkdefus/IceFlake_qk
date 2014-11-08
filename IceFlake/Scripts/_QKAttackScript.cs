using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Scripts;
using System.Threading;

namespace IceFlake.Scripts
{
    public class QKAttackScript : Script
    {
        public QKAttackScript()
            : base("QK", "_QKAttackScript")
        { }

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            //Print("Facing: {0}", Manager.LocalPlayer.Facing);
            foreach (var p in Manager.ObjectManager.Objects.Where(x => x.IsPlayer).Cast<WoWPlayer>())
            {
                Print("-- {0}", p.Name);
                Print("\tGUID: 0x{0}", p.Guid.ToString("X8"));
                Print("\tPosition: {0}", p.Location);
            }

            //foreach (var obj in Manager.ObjectManager.Objects.Where(x => x.IsUnit).Cast<WoWUnit>())
            //{
            //    if (obj.IsHostile)
            //    {
            //        Print("-- {0}", obj.Name);
            //        Print("\tGUID: 0x{0}", obj.Guid.ToString("0"));
            //        Print("\tPosition: {0}", obj.Location);
            //    }
            //}

            List<WoWUnit> _list = new List<WoWUnit>(GetClosestAttackables());
            if (_list.Count > 0)
            {
                foreach (var obj in _list)
                {
                    Print("-- {0}", obj.Name);
                    Print("\tGUID: 0x{0}", obj.Guid.ToString("0"));
                    Print("\tPosition: {0}", obj.Location);
                    Print("\tDistance: {0}", obj.Distance);
                    Manager.Movement.PathTo(obj.Location);

                    //while (obj.Distance > 5)
                    //    Thread.Sleep(100);

                    Print("\tExecute Facing:");
                    obj.Face();

                    Print("\tExecute Interact:");
                    obj.Interact();

                    Print("\tExecute Attack:");
                    WoWScript.ExecuteNoResults("/startattack");

                }
            }


        }

        #region Get Closest Attackers

        public static List<WoWUnit> GetClosestAttackers()
        {
            var attackers = new List<WoWUnit>();
            var numQuery =
            from U in Manager.ObjectManager.Objects.Where(x => x.IsUnit).Cast<WoWUnit>()
            where

            U.IsValid &&
            U.TargetGuid == Manager.LocalPlayer.Guid &&
            U.IsInCombat &&
            !U.IsDead &&
            (Manager.ObjectManager.LocalPlayer.Pet != null && U.TargetGuid == Manager.ObjectManager.LocalPlayer.Pet.Guid) &&
            U.Distance < 80

            select U;
            var sortednumQuery =
            from p in numQuery
            orderby p.Location.Distance2D(Manager.LocalPlayer.Location)
            select p;

            if (sortednumQuery.Count() > 0)
            {
                foreach (var unit in sortednumQuery)
                {
                    attackers.Add(unit);
                }
            }
            return attackers;
        }

        #endregion

        #region Get Closest Attackables Units

        public static List<WoWUnit> GetClosestAttackables()
        {
            var result = new List<WoWUnit>();
            var numQuery =
            from U in Manager.ObjectManager.Objects.Where(x => x.IsUnit).Cast<WoWUnit>()
            where
            U.IsValid && !U.IsDead && !U.IsFriendly && (U.CreatureType != Client.Patchables.CreatureType.Critter)
            select U;
            try
            {
                var sortednumQuery =
                (from p in numQuery
                orderby Manager.LocalPlayer.Location.DistanceTo(p.Location)
                select p).Last(); // TEMP SET TO LAST FOR FUN :P
                result.Add(sortednumQuery);
            }
            catch { }

            return result;
        }

        #endregion
    }
}
