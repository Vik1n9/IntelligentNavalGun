﻿using Sakuno.KanColle.Amatsukaze.Game.Models;
using Sakuno.KanColle.Amatsukaze.Game.Models.Raw;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Sakuno.KanColle.Amatsukaze.Game.Services.Quest.Triggers
{
    class ExpeditionTrigger : Trigger
    {
        public HashSet<int> Expeditions { get; }

        public ExpeditionTrigger(int[] rpExpeditions)
        {
            if (rpExpeditions?.Length != 0)
                Expeditions = new HashSet<int>(rpExpeditions);

            Observable = SessionService.Instance.GetProcessSucceededSubject("api_req_mission/result").Select(r =>
            {
                var rFleet = KanColleGame.Current.Port.Fleets[int.Parse(r.Parameters["api_deck_id"])];
                var rExpedition = rFleet.ExpeditionStatus.Expedition;

                return new { Expedition = rExpedition, Result = ((RawExpeditionResult)r.Data).Result };
            }).Where(r => r.Result != ExpeditionResult.Failure && (Expeditions == null || Expeditions.Contains(r.Expedition.ID)));
        }

        public override string ToString() => $"Expedition: {(Expeditions != null ? string.Join(", ", Expeditions) : "All")}";
    }
}
