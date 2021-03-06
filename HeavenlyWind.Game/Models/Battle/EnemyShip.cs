﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sakuno.KanColle.Amatsukaze.Game.Models.Battle
{
    class EnemyShip : ModelBase, IParticipant, ICombatAbility
    {
        static Dictionary<int, int[]> r_PlaneCount;

        internal static IDictionary<int, int> FighterPowers { get; }

        public ShipInfo Info { get; }
        public bool IsAbyssalShip => Info.IsAbyssalShip;

        public int Level { get; }
        public IList<ShipSlot> Slots { get; }

        public bool IsMVP => false;
        public Ship Ship => null;
        public ShipSlot ExtraSlot => null;

        public ShipCombatAbility CombatAbility => null;
        public bool IsDamageControlVisible => false;
        public bool IsDamageControlConsumed => false;

        static EnemyShip()
        {
            var rFile = new FileInfo(@"Data\AbyssalShipPlane.json");
            if (rFile.Exists)
                using (var rStream = rFile.OpenRead())
                {
                    var rJson = JArray.Load(new JsonTextReader(new StreamReader(rStream)));

                    r_PlaneCount = rJson.ToDictionary(r => (int)r["id"], r => r["plane_count"].ToObject<int[]>());
                    FighterPowers = rJson.ToDictionary(r => (int)r["id"], r => (int)r["fighter_power"]);
                }
        }
        public EnemyShip(int rpID, int rpLevel, int[] rpEquipment = null)
        {
            Info = KanColleGame.Current.MasterInfo.Ships[rpID];

            Level = rpLevel;
            if (rpEquipment != null)
                Slots = rpEquipment.TakeWhile(r => r != -1).Select((r, i) =>
                {
                    var rMaxPlaneCount = -1;

                    if (i < Info.SlotCount && Info.PlaneCountInSlot != null)
                        rMaxPlaneCount = Info.PlaneCountInSlot[i];
                    else
                    {
                        int[] rPlaneCount;
                        if (r_PlaneCount != null && r_PlaneCount.TryGetValue(Info.ID, out rPlaneCount) && i < rPlaneCount.Length)
                            rMaxPlaneCount = rPlaneCount[i];
                    }

                    return new ShipSlot(Equipment.GetDummy(r), rMaxPlaneCount, rMaxPlaneCount);
                }).ToArray();
        }

        public override string ToString() => $"{Info} Lv. {Level}";
    }
}
