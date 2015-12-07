﻿using Sakuno.KanColle.Amatsukaze.Game.Models.Battle.Phases;
using Sakuno.KanColle.Amatsukaze.Game.Models.Raw.Battle;
using Sakuno.KanColle.Amatsukaze.Game.Parsers;
using System.Collections.Generic;
using System.Linq;

namespace Sakuno.KanColle.Amatsukaze.Game.Models.Battle
{
    public abstract class BattleStage : ModelBase
    {
        public BattleInfo Owner { get; }

        public abstract BattleStageType Type { get; }

        public abstract IList<BattlePhase> Phases { get; }

        public IList<BattleParticipantSnapshot> FriendMain { get; protected set; }
        public IList<BattleParticipantSnapshot> FriendEscort { get; protected set; }

        public IList<BattleParticipantSnapshot> Enemy { get; protected set; }

        internal BattleParticipantSnapshot[] FriendAndEnemy { get; private set; }

        internal protected BattleStage(BattleInfo rpOwner)
        {
            Owner = rpOwner;
        }

        internal void Process(ApiData rpData)
        {
            var rData = rpData.Data as RawBattleBase;

            FriendAndEnemy = rData.CurrentHPs.Zip(rData.MaximumHPs,
                (rpCurrent, rpMaximum) => rpMaximum != -1 ? new BattleParticipantSnapshot(rpMaximum, rpCurrent) : null).Skip(1).ToArray();

            FriendMain = FriendAndEnemy.Take(6).TakeWhile(r => r != null).ToList().AsReadOnly();
            for (var i = 0; i < FriendMain.Count; i++)
                FriendMain[i].Participant = Owner.Participants.FriendMain[i];

            Enemy = FriendAndEnemy.Skip(6).TakeWhile(r => r != null).ToList().AsReadOnly();
            for (var i = 0; i < Enemy.Count; i++)
                Enemy[i].Participant = Owner.Participants.Enemy[i];

            var rCombinedFleetData = rData as IRawCombinedFleet;
            if (rCombinedFleetData != null)
            {
                FriendEscort = rCombinedFleetData.EscortFleetCurrentHPs.Zip(rCombinedFleetData.EscortFleetMaximumHPs,
                    (rpCurrent, rpMaximum) => rpMaximum != -1 ? new BattleParticipantSnapshot(rpMaximum, rpCurrent) : null).Skip(1).ToList().AsReadOnly();

                for (var i = 0; i < FriendMain.Count; i++)
                    FriendEscort[i].Participant = Owner.Participants.FriendEscort[i];

                FriendAndEnemy = Enumerable.Concat(FriendAndEnemy, FriendEscort).ToArray();
            }

            foreach (var rPhase in Phases)
                rPhase.Process();
        }
    }
}
